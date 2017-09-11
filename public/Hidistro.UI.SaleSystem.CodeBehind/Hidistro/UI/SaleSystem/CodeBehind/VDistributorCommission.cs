namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorCommission : VMemberTemplatedWebControl
    {
        private HyperLink hyrequest;
        private FormatedMoneyLabel lblAlreadycommission;
        private FormatedMoneyLabel lblcommission;
        private FormatedMoneyLabel lblsaleprice;
        private FormatedMoneyLabel lblsurpluscommission;
        private FormatedMoneyLabel lblthreecommission;
        private FormatedMoneyLabel lblthreesaleprice;
        private FormatedMoneyLabel lbltotalcommission;
        private FormatedMoneyLabel lbltwocommission;
        private FormatedMoneyLabel lbltwosaleprice;
        private Literal litMsg;
        private Panel panelthree;
        private Panel paneltwo;

        protected override void AttachChildControls()
        {
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId(false));
            if (userIdDistributors.ReferralStatus != 0)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else
            {
                this.lbltotalcommission = (FormatedMoneyLabel) this.FindControl("lbltotalcommission");
                this.lblsurpluscommission = (FormatedMoneyLabel) this.FindControl("lblsurpluscommission");
                this.lblAlreadycommission = (FormatedMoneyLabel) this.FindControl("lblAlreadycommission");
                this.lblcommission = (FormatedMoneyLabel) this.FindControl("lblcommission");
                this.lbltwocommission = (FormatedMoneyLabel) this.FindControl("lbltwocommission");
                this.lblthreecommission = (FormatedMoneyLabel) this.FindControl("lblthreecommission");
                this.lblsaleprice = (FormatedMoneyLabel) this.FindControl("lblsaleprice");
                this.lbltwosaleprice = (FormatedMoneyLabel) this.FindControl("lbltwosaleprice");
                this.lblthreesaleprice = (FormatedMoneyLabel) this.FindControl("lblthreesaleprice");
                this.paneltwo = (Panel) this.FindControl("paneltwo");
                this.panelthree = (Panel) this.FindControl("panelthree");
                this.litMsg = (Literal) this.FindControl("litMsg");
                this.hyrequest = (HyperLink) this.FindControl("hyrequest");
                PageTitle.AddSiteNameTitle("我的佣金");
                DataTable currentDistributorsCommosion = DistributorsBrower.GetCurrentDistributorsCommosion(userIdDistributors.UserId);
                if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
                {
                    this.lblsurpluscommission.Money = userIdDistributors.ReferralBlance;
                    this.lblAlreadycommission.Money = userIdDistributors.ReferralRequestBalance;
                    if (userIdDistributors.DistributorGradeId == DistributorGrade.TowDistributor)
                    {
                        this.paneltwo.Visible = false;
                    }
                    else if (userIdDistributors.DistributorGradeId == DistributorGrade.ThreeDistributor)
                    {
                        this.paneltwo.Visible = false;
                        this.panelthree.Visible = false;
                    }
                    if ((currentDistributorsCommosion != null) && (currentDistributorsCommosion.Rows.Count > 0))
                    {
                        this.lblcommission.Money = currentDistributorsCommosion.Rows[0]["CommTotal"];
                        this.lblsaleprice.Money = currentDistributorsCommosion.Rows[0]["OrderTotal"];
                    }
                    if (userIdDistributors.DistributorGradeId == DistributorGrade.OneDistributor)
                    {
                        currentDistributorsCommosion = DistributorsBrower.GetDistributorsCommosion(userIdDistributors.UserId, DistributorGrade.TowDistributor);
                        if ((currentDistributorsCommosion != null) && (currentDistributorsCommosion.Rows.Count > 0))
                        {
                            this.lbltwocommission.Money = currentDistributorsCommosion.Rows[0]["CommTotal"];
                            this.lbltwosaleprice.Money = currentDistributorsCommosion.Rows[0]["OrderTotal"];
                        }
                        currentDistributorsCommosion = DistributorsBrower.GetDistributorsCommosion(userIdDistributors.UserId, DistributorGrade.ThreeDistributor);
                        if ((currentDistributorsCommosion != null) && (currentDistributorsCommosion.Rows.Count > 0))
                        {
                            this.lblthreecommission.Money = currentDistributorsCommosion.Rows[0]["CommTotal"];
                            this.lblthreesaleprice.Money = currentDistributorsCommosion.Rows[0]["OrderTotal"];
                        }
                    }
                    if (userIdDistributors.DistributorGradeId == DistributorGrade.TowDistributor)
                    {
                        currentDistributorsCommosion = DistributorsBrower.GetDistributorsCommosion(userIdDistributors.UserId, DistributorGrade.ThreeDistributor);
                        if ((currentDistributorsCommosion != null) && (currentDistributorsCommosion.Rows.Count > 0))
                        {
                            this.lblthreecommission.Money = currentDistributorsCommosion.Rows[0]["CommTotal"];
                            this.lblthreesaleprice.Money = currentDistributorsCommosion.Rows[0]["OrderTotal"];
                        }
                    }
                    this.lbltotalcommission.Money = userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance;
                    if (DistributorsBrower.IsExitsCommionsRequest())
                    {
                        this.hyrequest.Text = "<i class='iconfont color2 icon-iconadvance'></i>您的申请正在审核当中……";
                        this.hyrequest.Enabled = false;
                    }
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    if (string.IsNullOrEmpty(currentMember.RealName) || string.IsNullOrEmpty(currentMember.CellPhone))
                    {
                        this.hyrequest.NavigateUrl = "UserInfo.aspx?edit=true&&returnUrl=" + Globals.UrlEncode(Globals.HostPath(HttpContext.Current.Request.Url) + "/Vshop/RequestCommissions.aspx");
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorCommission.html";
            }
            base.OnInit(e);
        }
    }
}

