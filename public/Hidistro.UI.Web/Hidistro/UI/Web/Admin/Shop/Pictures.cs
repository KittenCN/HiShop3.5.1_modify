namespace Hidistro.UI.Web.Admin.Shop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Pictures : AdminPage
    {
        protected Button btnImagetSearch;
        protected HtmlForm form1;
        public string GlobalsPath;
        private string keyOrder;
        private string keyWordIName;
        private int pageIndex;
        protected Pager pager;
        protected DataList photoDataList;
        protected TextBox txtWordName;
        private int? typeId;
        protected UpImg uploader1;

        protected Pictures() : base("m01", "00000")
        {
            this.keyWordIName = string.Empty;
            this.typeId = null;
            this.GlobalsPath = Globals.ApplicationPath;
        }

        private void BindImageData()
        {
            this.pageIndex = this.pager.PageIndex;
            PhotoListOrder uploadTimeDesc = PhotoListOrder.UploadTimeDesc;
            DbQueryResult result = GalleryHelper.GetPhotoList(this.keyWordIName, this.typeId, this.pageIndex, uploadTimeDesc, 0, 20);
            this.photoDataList.DataSource = result.Data;
            this.photoDataList.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
        }

        private void btnImagetSearch_Click(object sender, EventArgs e)
        {
            this.keyWordIName = this.txtWordName.Text;
            this.BindImageData();
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
            if (Globals.RequestFormStr("posttype") == "togallery")
            {
                base.Response.ContentType = "application/json";
                string s = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
                string str3 = Globals.RequestFormStr("photourl");
                if (!string.IsNullOrEmpty(str3))
                {
                    Bitmap bitmap = new Bitmap(base.Server.MapPath(str3));
                    MemoryStream stream = new MemoryStream();
                    bitmap.Save(stream, ImageFormat.Jpeg);
                    GalleryHelper.AddPhote(0, "wb" + DateTime.Now.ToString("yyyyMMddHHmmss"), str3, (int) stream.Length);
                    s = "{\"type\":\"1\",\"tips\":\"操作成功\"}";
                    stream.Dispose();
                }
                base.Response.Write(s);
                base.Response.End();
            }
            else
            {
                this.btnImagetSearch.Click += new EventHandler(this.btnImagetSearch_Click);
                if (!base.IsPostBack)
                {
                    this.LoadParameters();
                    this.BindImageData();
                }
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

