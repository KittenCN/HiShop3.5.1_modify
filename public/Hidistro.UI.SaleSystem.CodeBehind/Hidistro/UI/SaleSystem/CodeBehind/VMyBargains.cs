namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Bargain;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Bargain;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;

    public class VMyBargains : VshopTemplatedWebControl
    {
        private HtmlInputHidden hiddPageIndex;
        private HtmlInputHidden hiddTotal;
        private VshopTemplatedRepeater rpMyBargainList;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("我的砍价");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                int num;
                int num2;
                this.rpMyBargainList = (VshopTemplatedRepeater) this.FindControl("rpMyBargainList");
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
                int num3 = 0;
                if (!int.TryParse(this.Page.Request.QueryString["status"], out num3))
                {
                    num3 = 0;
                }
                BargainQuery query = new BargainQuery {
                    PageSize = num2,
                    PageIndex = num,
                    UserId = currentMember.UserId,
                    Status = num3
                };
                DbQueryResult myBargainList = BargainHelper.GetMyBargainList(query);
                this.hiddTotal.Value = ((DataTable) myBargainList.Data).Rows.Count.ToString();
                this.hiddPageIndex.Value = num.ToString();
                this.rpMyBargainList.DataSource = myBargainList.Data;
                this.rpMyBargainList.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMyBargains.html";
            }
            base.OnInit(e);
        }
    }
}

