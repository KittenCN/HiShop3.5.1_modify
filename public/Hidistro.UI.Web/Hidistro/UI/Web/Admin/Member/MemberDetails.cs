namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class MemberDetails : AdminPage
    {
        protected Button btnEdit;
        private int currentUserId;
        protected TextBox lblUserLink;
        protected TextBox litAddress;
        protected TextBox litAlipayOpenid;
        protected TextBox litCellPhone;
        protected TextBox litCreateDate;
        protected TextBox litEmail;
        protected TextBox litGrade;
        protected TextBox litOpenId;
        protected TextBox litQQ;
        protected TextBox litRealName;
        protected TextBox litUserBindName;
        protected Literal litUserName;
        protected HtmlForm thisForm;
        protected TextBox txtCardID;

        protected MemberDetails() : base("m04", "hyp02")
        {
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            base.Response.Redirect(Globals.GetAdminAbsolutePath("/member/EditMember.aspx?userId=" + this.Page.Request.QueryString["userId"]), true);
        }

        private void LoadMemberInfo()
        {
            MemberInfo member = MemberHelper.GetMember(this.currentUserId);
            if (member == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                Uri url = HttpContext.Current.Request.Url;
                this.litUserName.Text = member.UserName;
                MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(member.GradeId);
                if (memberGrade != null)
                {
                    this.litGrade.Text = memberGrade.Name;
                }
                this.litCreateDate.Text = member.CreateDate.ToString();
                this.litRealName.Text = member.RealName;
                this.litAddress.Text = RegionHelper.GetFullRegion(member.RegionId, "") + member.Address;
                this.litQQ.Text = member.QQ;
                this.litCellPhone.Text = member.CellPhone;
                this.litEmail.Text = member.Email;
                this.litOpenId.Text = member.OpenId;
                this.litAlipayOpenid.Text = member.AlipayOpenid;
                this.txtCardID.Text = member.CardID;
                this.litUserBindName.Text = member.UserBindName;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.currentUserId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
                if (!this.Page.IsPostBack)
                {
                    this.LoadMemberInfo();
                }
            }
        }
    }
}

