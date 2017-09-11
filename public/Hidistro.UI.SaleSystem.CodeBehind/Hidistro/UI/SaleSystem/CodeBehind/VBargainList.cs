namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Bargain;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Bargain;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;

    public class VBargainList : VshopTemplatedWebControl
    {
        private HtmlInputHidden hiddPageIndex;
        private HtmlInputHidden hiddTotal;
        private VshopTemplatedRepeater rpMyMemberList;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            PageTitle.AddSiteNameTitle("砍价活动");
            this.rpMyMemberList = (VshopTemplatedRepeater) this.FindControl("rpBargainList");
            this.hiddTotal = (HtmlInputHidden) this.FindControl("hiddTotal");
            this.hiddPageIndex = (HtmlInputHidden) this.FindControl("hiddPageIndex");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 10;
            }
            int num3 = Globals.RequestQueryNum("status");
            BargainQuery query = new BargainQuery {
                Type = num3.ToString(),
                PageSize = num2,
                PageIndex = num
            };
            BargainHelper.GetTotal(query);
            DbQueryResult bargainList = BargainHelper.GetBargainList(query);
            this.hiddTotal.Value = ((DataTable) bargainList.Data).Rows.Count.ToString();
            this.hiddPageIndex.Value = num.ToString();
            this.rpMyMemberList.DataSource = bargainList.Data;
            this.rpMyMemberList.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VBargainList.html";
            }
            base.OnInit(e);
        }
    }
}

