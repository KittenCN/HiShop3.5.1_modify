namespace Hidistro.UI.Web.Admin.AliFuwu
{
    using ASPNET.WebControls;
    using  global:: ControlPanel.WeiXin;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.WeiXin;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.AlipayFuwu.Api.Model;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class Alisendalllist : AdminPage
    {
        protected string ArticleTitle;
        protected int articletype;
        protected string htmlMenuTitleAdd;
        private int pageno;
        protected Pager pager;
        protected int recordcount;
        protected Repeater rptList;
        private string title;

        protected Alisendalllist() : base("m11", "fwp03")
        {
            this.htmlMenuTitleAdd = string.Empty;
            this.ArticleTitle = string.Empty;
            this.title = string.Empty;
        }

        private void BindData(int pageno)
        {
            SendAllQuery entity = new SendAllQuery {
                SortBy = "ID",
                SortOrder = SortAction.Desc
            };
            Globals.EntityCoding(entity, true);
            entity.PageIndex = pageno;
            entity.PageSize = this.pager.PageSize;
            DbQueryResult sendAllRequest = WeiXinHelper.GetSendAllRequest(entity, 1);
            this.rptList.DataSource = sendAllRequest.Data;
            this.rptList.DataBind();
            int totalRecords = sendAllRequest.TotalRecords;
            this.pager.TotalRecords = totalRecords;
            this.recordcount = totalRecords;
            if (this.pager.TotalRecords <= this.pager.PageSize)
            {
                this.pager.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                if (AlipayFuwuConfig.appId.Length < 0x10)
                {
                    this.ShowMsgAndReUrl("请先绑定服务窗", false, "AliFuwuConfig.aspx");
                }
                else
                {
                    this.pageno = Globals.RequestQueryNum("pageindex");
                    if (this.pageno < 1)
                    {
                        this.pageno = 1;
                    }
                    this.BindData(this.pageno);
                    if (this.pageno == 1)
                    {
                        WeiXinHelper.DelOldSendAllList();
                    }
                }
            }
        }
    }
}

