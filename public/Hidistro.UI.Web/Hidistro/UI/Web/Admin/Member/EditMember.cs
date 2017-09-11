namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditMember)]
    public class EditMember : AdminPage
    {
        protected Button BindCheck;
        protected Button btnEditUser;
        private int currentUserId;
        protected MemberGradeDropDownList drpMemberRankList;
        protected Literal lblLoginNameValue;
        protected FormatedTimeLabel lblRegsTimeValue;
        protected Literal lblTotalAmountValue;
        protected Literal LitUserBindName;
        protected HtmlInputHidden PSWUserIds;
        protected RegionSelector rsddlRegion;
        protected HtmlForm thisForm;
        protected TextBox txtAddress;
        protected TextBox txtBindName;
        protected TextBox txtCardID;
        protected TextBox txtCellPhone;
        protected TextBox txtprivateEmail;
        protected TextBox txtQQ;
        protected TextBox txtRealName;
        protected TextBox txtUserPassword;

        protected EditMember() : base("m04", "hyp02")
        {
        }

        protected void BindCheck_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (int.TryParse(this.BindCheck.CommandName, out result))
            {
                string text = this.txtBindName.Text;
                string sourceData = this.txtUserPassword.Text;
                MemberInfo bindusernameMember = MemberProcessor.GetBindusernameMember(text);
                if ((bindusernameMember != null) && (bindusernameMember.UserId != result))
                {
                    this.ShowMsg("该用户名已经被绑定", false);
                }
                else if (bindusernameMember != null)
                {
                    this.ShowMsg("该用户已经绑定系统帐号", false);
                    this.LoadMemberInfo();
                }
                else if (MemberProcessor.BindUserName(result, text, HiCryptographer.Md5Encrypt(sourceData)))
                {
                    this.ShowMsg("用户绑定成功!", true);
                    MemberInfo member = MemberProcessor.GetMember(result, false);
                    try
                    {
                        Messenger.SendWeiXinMsg_SysBindUserName(member, sourceData);
                    }
                    catch
                    {
                    }
                    this.LoadMemberInfo();
                }
                else
                {
                    this.ShowMsg("用户绑定失败!", false);
                }
            }
            else
            {
                this.ShowMsg("用户不存在！", false);
            }
        }

        protected void btnEditUser_Click(object sender, EventArgs e)
        {
            MemberInfo member = MemberHelper.GetMember(this.currentUserId);
            int gradeId = member.GradeId;
            member.GradeId = this.drpMemberRankList.SelectedValue.Value;
            member.RealName = this.txtRealName.Text.Trim();
            if (this.rsddlRegion.GetSelectedRegionId().HasValue)
            {
                member.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
                member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
            }
            member.Address = Globals.HtmlEncode(this.txtAddress.Text);
            member.QQ = this.txtQQ.Text;
            member.Email = member.QQ + "@qq.com";
            member.CellPhone = this.txtCellPhone.Text;
            member.Email = this.txtprivateEmail.Text;
            member.CardID = this.txtCardID.Text;
            if (this.ValidationMember(member))
            {
                if (gradeId != this.drpMemberRankList.SelectedValue.Value)
                {
                    try
                    {
                        Messenger.SendWeiXinMsg_MemberGradeChange(member);
                    }
                    catch
                    {
                    }
                }
                if (MemberHelper.Update(member))
                {
                    this.ShowMsgAndReUrl("成功修改了当前会员的个人资料", true, "/Admin/member/managemembers.aspx");
                }
                else
                {
                    this.ShowMsg("当前会员的个人信息修改失败", false);
                }
            }
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
                this.drpMemberRankList.SelectedValue = new int?(member.GradeId);
                this.lblLoginNameValue.Text = member.UserName;
                this.lblRegsTimeValue.Time = member.CreateDate;
                this.lblTotalAmountValue.Text = Globals.FormatMoney(member.Expenditure);
                this.txtRealName.Text = member.RealName;
                this.txtAddress.Text = Globals.HtmlDecode(member.Address);
                this.rsddlRegion.SetSelectedRegionId(new int?(member.RegionId));
                this.txtQQ.Text = member.QQ;
                this.txtCellPhone.Text = member.CellPhone;
                this.txtprivateEmail.Text = member.Email;
                this.txtCardID.Text = member.CardID;
                this.LitUserBindName.Text = member.UserBindName;
                this.BindCheck.CommandName = member.UserId.ToString();
                if (string.IsNullOrEmpty(member.UserBindName))
                {
                    this.LitUserBindName.Text = "<a id='bindSysUser'>点击绑定系统用户名</a>";
                }
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
                this.btnEditUser.Click += new EventHandler(this.btnEditUser_Click);
                this.BindCheck.Click += new EventHandler(this.BindCheck_Click);
                if (!this.Page.IsPostBack)
                {
                    this.drpMemberRankList.AllowNull = false;
                    this.drpMemberRankList.DataBind();
                    this.LoadMemberInfo();
                }
            }
        }

        private bool ValidationMember(MemberInfo member)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<MemberInfo>(member, new string[] { "ValMember" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

