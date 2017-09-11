namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorInfo : VMemberTemplatedWebControl
    {
        private HtmlInputHidden hdlogo;
        private HtmlImage imglogo;
        private Literal litJs;
        private HtmlInputText txtacctount;
        private HtmlTextArea txtdescription;
        private HtmlInputText txtstorename;
        private HtmlInputText txtStoreTel;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("店铺消息");
            this.imglogo = (HtmlImage) this.FindControl("imglogo");
            this.hdlogo = (HtmlInputHidden) this.FindControl("hdlogo");
            this.txtstorename = (HtmlInputText) this.FindControl("txtstorename");
            this.txtdescription = (HtmlTextArea) this.FindControl("txtdescription");
            this.txtacctount = (HtmlInputText) this.FindControl("txtacctount");
            this.txtStoreTel = (HtmlInputText) this.FindControl("txtStoreTel");
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId(false));
            if (userIdDistributors.ReferralStatus != 0)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                if (userIdDistributors != null)
                {
                    if (!string.IsNullOrEmpty(userIdDistributors.Logo))
                    {
                        this.imglogo.Src = userIdDistributors.Logo;
                    }
                    this.hdlogo.Value = userIdDistributors.Logo;
                    this.txtstorename.Value = userIdDistributors.StoreName;
                    this.txtdescription.Value = userIdDistributors.StoreDescription;
                    this.txtacctount.Value = userIdDistributors.RequestAccount;
                    this.txtStoreTel.Value = currentMember.CellPhone;
                    this.litJs = (Literal) this.FindControl("litJs");
                    if (SettingsManager.GetMasterSettings(true).IsShowDistributorSelfStoreName)
                    {
                        this.litJs.Text = "<script>$(function () {$('#idFile').uploadPreview({ Img: 'imglogo', Width: 100, Height: 100 });$('#savemes').removeAttr('disabled');$('.notmodifyshop').show(); });</script>";
                    }
                    else
                    {
                        this.litJs.Text = "<script>$(function () {$('input,textarea').attr('disabled','true'); });</script>";
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorInfo.html";
            }
            base.OnInit(e);
        }
    }
}

