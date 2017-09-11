namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VUserInfo : VMemberTemplatedWebControl
    {
        private HtmlImage imglogo;
        private HtmlContainerControl Nickname;
        private HtmlControl WeixinHead;

        protected override void AttachChildControls()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                Literal control = (Literal) this.FindControl("txtUserBindName");
                HtmlInputText text = (HtmlInputText) this.FindControl("txtRealName");
                HtmlInputText text2 = (HtmlInputText) this.FindControl("txtPhone");
                HtmlInputText text3 = (HtmlInputText) this.FindControl("txtEmail");
                HtmlInputHidden hidden = (HtmlInputHidden) this.FindControl("txtUserName");
                HtmlInputText text4 = (HtmlInputText) this.FindControl("txtCardID");
                this.imglogo = (HtmlImage) this.FindControl("imglogo");
                this.Nickname = (HtmlContainerControl) this.FindControl("Nickname");
                this.WeixinHead = (HtmlControl) this.FindControl("WeixinHead");
                if (!string.IsNullOrEmpty(currentMember.UserHead))
                {
                    this.imglogo.Src = currentMember.UserHead;
                }
                this.Nickname.InnerText = currentMember.UserName;
                if (string.IsNullOrEmpty(currentMember.OpenId))
                {
                    this.WeixinHead.Attributes.Add("noOpenId", "true");
                }
                control.SetWhenIsNotNull(currentMember.UserBindName);
                hidden.SetWhenIsNotNull(currentMember.UserName);
                text.SetWhenIsNotNull(currentMember.RealName);
                text2.SetWhenIsNotNull(currentMember.CellPhone);
                text3.SetWhenIsNotNull(currentMember.QQ);
                text4.SetWhenIsNotNull(currentMember.CardID);
            }
            PageTitle.AddSiteNameTitle("修改用户信息");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VUserInfo.html";
            }
            base.OnInit(e);
        }
    }
}

