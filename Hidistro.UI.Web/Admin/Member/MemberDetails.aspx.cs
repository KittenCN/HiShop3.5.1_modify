using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
    public partial class MemberDetails : AdminPage
    {
       
        private int currentUserId;
        

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