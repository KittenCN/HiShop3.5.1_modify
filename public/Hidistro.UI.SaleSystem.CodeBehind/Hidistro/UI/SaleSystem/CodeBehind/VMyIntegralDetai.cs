namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMyIntegralDetai : VMemberTemplatedWebControl
    {
        private Literal litStatus0;
        private Literal litStatus1;
        private Literal litStatus2;
        private Literal litSumIntegral;
        private Literal litSurplusIntegral;
        private Literal littableList0;
        private Literal littableList1;
        private Literal littableList2;
        private VshopTemplatedRepeater rptIntegarlDetail0;
        private VshopTemplatedRepeater rptIntegarlDetail1;
        private VshopTemplatedRepeater rptIntegarlDetail2;
        private HtmlInputHidden txtShowTabNum;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            int num2;
            int num3;
            this.litSurplusIntegral = (Literal) this.FindControl("litSurplusIntegral");
            this.litSumIntegral = (Literal) this.FindControl("litSumIntegral");
            this.litStatus0 = (Literal) this.FindControl("litStatus0");
            this.litStatus1 = (Literal) this.FindControl("litStatus1");
            this.litStatus2 = (Literal) this.FindControl("litStatus2");
            this.littableList0 = (Literal) this.FindControl("littableList0");
            this.littableList1 = (Literal) this.FindControl("littableList1");
            this.littableList2 = (Literal) this.FindControl("littableList2");
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            this.txtShowTabNum = (HtmlInputHidden) this.FindControl("txtShowTabNum");
            this.rptIntegarlDetail0 = (VshopTemplatedRepeater) this.FindControl("rptIntegarlDetail0");
            this.rptIntegarlDetail1 = (VshopTemplatedRepeater) this.FindControl("rptIntegarlDetail1");
            this.rptIntegarlDetail2 = (VshopTemplatedRepeater) this.FindControl("rptIntegarlDetail2");
            int num = 0;
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num2))
            {
                num2 = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num3))
            {
                num3 = 10;
            }
            IntegralDetailQuery query = new IntegralDetailQuery();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            this.litSurplusIntegral.Text = currentMember.Points.ToString();
            this.litSumIntegral.Text = Convert.ToInt32(MemberProcessor.GetIntegral(currentMember.UserId)).ToString();
            query.UserId = currentMember.UserId;
            query.PageIndex = num2;
            query.PageSize = num3;
            if (int.TryParse(this.Page.Request.QueryString["IntegralSourceType"], out num))
            {
                if (num == 0)
                {
                    query.IntegralSourceType = num;
                    DbQueryResult integralDetail = MemberProcessor.GetIntegralDetail(query);
                    this.rptIntegarlDetail0.DataSource = integralDetail.Data;
                    this.rptIntegarlDetail0.DataBind();
                    this.litStatus0.Text = "class=\"active\"";
                    this.litStatus1.Text = "";
                    this.litStatus2.Text = "";
                    this.littableList0.Text = "style=\"display: block;\"";
                    this.littableList1.Text = "style=\"display: none;\"";
                    this.littableList2.Text = "style=\"display: none;\"";
                    this.txtTotal.SetWhenIsNotNull(integralDetail.TotalRecords.ToString());
                }
                else if (num == 1)
                {
                    query.IntegralSourceType = num;
                    DbQueryResult result2 = MemberProcessor.GetIntegralDetail(query);
                    this.rptIntegarlDetail1.DataSource = result2.Data;
                    this.rptIntegarlDetail1.DataBind();
                    this.litStatus0.Text = "";
                    this.litStatus1.Text = "class=\"active\"";
                    this.litStatus2.Text = "";
                    this.littableList0.Text = "style=\"display: none ;\"";
                    this.littableList1.Text = "style=\"display:block;\"";
                    this.littableList2.Text = "style=\"display: none;\"";
                    this.txtTotal.SetWhenIsNotNull(result2.TotalRecords.ToString());
                }
                else
                {
                    query.IntegralSourceType = num;
                    DbQueryResult result3 = MemberProcessor.GetIntegralDetail(query);
                    this.rptIntegarlDetail2.DataSource = result3.Data;
                    this.rptIntegarlDetail2.DataBind();
                    this.litStatus0.Text = "";
                    this.litStatus1.Text = "";
                    this.litStatus2.Text = "class=\"active\"";
                    this.littableList0.Text = "style=\"display: none ;\"";
                    this.littableList1.Text = "style=\"display:none;\"";
                    this.littableList2.Text = "style=\"display: block;\"";
                    this.txtTotal.SetWhenIsNotNull(result3.TotalRecords.ToString());
                }
            }
            PageTitle.AddSiteNameTitle("积分明细");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VMyIntegralDetail.html";
            }
            base.OnInit(e);
        }
    }
}

