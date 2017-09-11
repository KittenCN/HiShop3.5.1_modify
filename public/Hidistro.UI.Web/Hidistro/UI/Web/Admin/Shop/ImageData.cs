namespace Hidistro.UI.Web.Admin.Shop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ImageData : AdminPage
    {
        protected HtmlButton btnDelete1;
        protected Button btnHiddenDel;
        protected Button btnImagetSearch;
        protected Button btnMoveImageData;
        protected Button btnSaveImageDataName;
        protected ImageDataGradeDropDownList dropImageFtp;
        protected System.Web.UI.WebControls.FileUpload FileUpload;
        public string GlobalsPath;
        protected HiddenField hdfSelIDList;
        protected ImageOrderDropDownList ImageOrder;
        protected ImageTypeLabel ImageTypeID;
        private string keyOrder;
        private string keyWordIName;
        protected Label lblImageData;
        protected string localUrl;
        private int orderby;
        private int pageIndex;
        protected Pager pager;
        protected TextBox ReImageDataName;
        protected HiddenField ReImageDataNameId;
        protected HiddenField RePlaceId;
        protected HiddenField RePlaceImg;
        protected Repeater rptList;
        protected Script Script1;
        protected Script Script4;
        protected Script Script7;
        protected HtmlForm thisForm;
        protected TextBox txtWordName;
        private int? typeId;

        protected ImageData() : base("m01", "dpp07")
        {
            this.localUrl = string.Empty;
            this.keyWordIName = string.Empty;
            this.typeId = null;
            this.GlobalsPath = Globals.ApplicationPath;
        }

        private void BindImageData()
        {
            int type = Globals.RequestQueryNum("type");
            this.pageIndex = this.pager.PageIndex;
            this.orderby = Globals.RequestQueryNum("orderby");
            switch (this.orderby)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    this.ImageOrder.SelectedValue = new int?(this.orderby);
                    break;
            }
            PhotoListOrder order = (PhotoListOrder) Enum.ToObject(typeof(PhotoListOrder), this.orderby);
            DbQueryResult result = GalleryHelper.GetPhotoList(this.keyWordIName, this.typeId, this.pageIndex, order, type, 0x12);
            this.rptList.DataSource = result.Data;
            this.rptList.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
            this.lblImageData.Text = this.pager.TotalRecords.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool flag = true;
            foreach (string str in this.hdfSelIDList.Value.Trim(new char[] { ',' }).Split(new char[] { ',' }))
            {
                try
                {
                    if (!GalleryHelper.DeletePhoto(Globals.ToNum(str)))
                    {
                        flag = false;
                    }
                }
                catch
                {
                    this.ShowMsg("删除文件错误", false);
                    this.BindImageData();
                }
            }
            if (flag)
            {
                this.ShowMsg("删除图片成功", true);
            }
            this.BindImageData();
        }

        private void btnImagetSearch_Click(object sender, EventArgs e)
        {
            this.keyWordIName = this.txtWordName.Text;
            this.BindImageData();
        }

        private void btnMoveImageData_Click(object sender, EventArgs e)
        {
            List<int> pList = new List<int>();
            int pTypeId = Globals.ToNum(this.dropImageFtp.SelectedItem.Value);
            foreach (string str in this.hdfSelIDList.Value.Trim(new char[] { ',' }).Split(new char[] { ',' }))
            {
                int item = Globals.ToNum(str);
                if (item > 0)
                {
                    pList.Add(item);
                }
            }
            if (GalleryHelper.MovePhotoType(pList, pTypeId) > 0)
            {
                this.ShowMsg("图片移动成功！", true);
            }
            this.BindImageData();
        }

        private void btnSaveImageDataName_Click(object sender, EventArgs e)
        {
            string text = this.ReImageDataName.Text;
            if (string.IsNullOrEmpty(text) || (text.Length > 30))
            {
                this.ShowMsg("图片名称不能为空长度限制在30个字符以内！", false);
            }
            else
            {
                GalleryHelper.RenamePhoto(Convert.ToInt32(this.ReImageDataNameId.Value), text);
                this.BindImageData();
            }
        }

        public static string Html_ToClient(string Str)
        {
            if (Str == null)
            {
                return null;
            }
            if (Str != string.Empty)
            {
                return HttpContext.Current.Server.HtmlDecode(Str.Trim());
            }
            return string.Empty;
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyWordIName"]))
            {
                this.keyWordIName = Globals.UrlDecode(this.Page.Request.QueryString["keyWordIName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyWordSel"]))
            {
                this.keyOrder = Globals.UrlDecode(this.Page.Request.QueryString["keyWordSel"]);
            }
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["imageTypeId"], out result))
            {
                this.typeId = new int?(result);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.localUrl = base.Request.Url.ToString();
            this.btnHiddenDel.Click += new EventHandler(this.btnDelete_Click);
            this.btnSaveImageDataName.Click += new EventHandler(this.btnSaveImageDataName_Click);
            this.btnMoveImageData.Click += new EventHandler(this.btnMoveImageData_Click);
            this.btnImagetSearch.Click += new EventHandler(this.btnImagetSearch_Click);
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.ImageOrder.DataBind();
                this.dropImageFtp.DataBind();
                this.BindImageData();
            }
        }

        public static string TruncStr(string str, int maxSize)
        {
            str = Html_ToClient(str);
            if (!(str != string.Empty))
            {
                return string.Empty;
            }
            int num = 0;
            byte[] bytes = new ASCIIEncoding().GetBytes(str);
            for (int i = 0; i <= (bytes.Length - 1); i++)
            {
                if (bytes[i] == 0x3f)
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                if (num > maxSize)
                {
                    str = str.Substring(0, i);
                    return str;
                }
            }
            return str;
        }
    }
}

