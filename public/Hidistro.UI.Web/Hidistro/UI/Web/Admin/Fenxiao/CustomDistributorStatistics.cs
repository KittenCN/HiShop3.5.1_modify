namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.FenXiao;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class CustomDistributorStatistics : AdminPage
    {
        protected Button btnSaveComm;
        protected HiddenField hiddid;
        private int id;
        protected Button lkbDelectSelect;
        protected Repeater repCustomDistributorStatisticList;
        protected TextBox txtCommTotal;
        protected TextBox txtOrderNum;
        protected TextBox txtStoreName;
        protected UpImg uploader1;

        protected CustomDistributorStatistics() : base("m05", "fxp11")
        {
        }

        private void BindData()
        {
            this.repCustomDistributorStatisticList.DataSource = VShopHelper.GetCustomDistributorStatisticList().Data;
            this.repCustomDistributorStatisticList.DataBind();
        }

        private void btnSaveComm_Click(object sender, EventArgs e)
        {
            string str = this.hiddid.Value;
            string uploadedImageUrl = "";
            if (!string.IsNullOrEmpty(this.uploader1.UploadedImageUrl.ToString()))
            {
                uploadedImageUrl = this.uploader1.UploadedImageUrl;
            }
            else
            {
                this.ShowMsg("请选择图片上传！", false);
                return;
            }
            string str3 = this.txtStoreName.Text.Trim();
            string str4 = this.txtOrderNum.Text.Trim();
            string str5 = this.txtCommTotal.Text.Trim();
            CustomDistributorStatistic custom = new CustomDistributorStatistic {
                OrderNums = string.IsNullOrEmpty(str4) ? 0 : int.Parse(str4),
                Logo = uploadedImageUrl,
                StoreName = str3,
                CommTotalSum = string.IsNullOrEmpty(str5) ? 0f : float.Parse(str5)
            };
            if (!string.IsNullOrEmpty(str))
            {
                custom.id = int.Parse(str);
                DataTable customDistributorStatistic = VShopHelper.GetCustomDistributorStatistic(custom.StoreName);
                if ((customDistributorStatistic.Rows.Count > 0) && (custom.id != int.Parse(customDistributorStatistic.Rows[0]["id"].ToString())))
                {
                    this.ShowMsg("店铺名称已经存在，请重新添加店铺名称!", false);
                }
                else
                {
                    VShopHelper.UpdateCustomDistributorStatistic(custom);
                    this.ShowMsgAndReUrl("修改成功", true, "CustomDistributorStatistics.aspx");
                }
            }
            else
            {
                DataTable data = (DataTable) VShopHelper.GetCustomDistributorStatisticList().Data;
                if ((data != null) && (data.Rows.Count >= 10))
                {
                    this.ShowMsg("自定义排行榜最多添加10条记录!", false);
                }
                else if (VShopHelper.GetCustomDistributorStatistic(custom.StoreName).Rows.Count > 0)
                {
                    this.ShowMsg("店铺名称已经存在，请重新添加店铺名称!", false);
                }
                else
                {
                    VShopHelper.InsertCustomDistributorStatistic(custom);
                    this.ShowMsgAndReUrl("添加成功", true, "CustomDistributorStatistics.aspx");
                }
            }
        }

        protected void lkbDelectSelect_Click(object sender, EventArgs e)
        {
            string id = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                id = base.Request["CheckBoxGroup"];
            }
            if (id.Length <= 0)
            {
                this.ShowMsg("请先选择要删除的分销商排名", false);
            }
            else
            {
                VShopHelper.DeleteCustomDistributorStatistic(id);
                this.ShowMsg("删除成功", true);
                this.BindData();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveComm.Click += new EventHandler(this.btnSaveComm_Click);
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void repCustomDistributorStatisticList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (((e.CommandName != "Edit") && (e.CommandName != "Update")) && (e.CommandName == "Delete"))
            {
                string str = e.CommandArgument.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    VShopHelper.DeleteCustomDistributorStatistic(str);
                    this.ShowMsg("删除成功", true);
                    this.BindData();
                }
            }
        }
    }
}

