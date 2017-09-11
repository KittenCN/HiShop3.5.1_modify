namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SubStoreCommissions : AdminPage
    {
        protected string EndTime;
        protected Pager pager;
        protected Repeater reCommissions;
        private int ReferralUserId;
        protected string StartTime;
        protected HtmlForm thisForm;
        private int userid;

        protected SubStoreCommissions() : base("m05", "fxp03")
        {
            this.StartTime = "";
            this.EndTime = "";
        }

        private void BindData()
        {
            CommissionsQuery entity = new CommissionsQuery {
                UserId = this.userid,
                EndTime = this.EndTime,
                StartTime = this.StartTime,
                PageIndex = this.pager.PageIndex,
                ReferralUserId = this.ReferralUserId,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "CommId"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult commissionsWithStoreName = VShopHelper.GetCommissionsWithStoreName(entity, "");
            this.reCommissions.DataSource = commissionsWithStoreName.Data;
            this.reCommissions.DataBind();
            this.pager.TotalRecords = commissionsWithStoreName.TotalRecords;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (int.TryParse(this.Page.Request.QueryString["ReferralUserId"], out this.ReferralUserId) && int.TryParse(this.Page.Request.QueryString["UserId"], out this.userid))
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
                {
                    this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
                {
                    this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
                }
                this.BindData();
            }
            else
            {
                base.GotoResourceNotFound();
            }
        }
    }
}

