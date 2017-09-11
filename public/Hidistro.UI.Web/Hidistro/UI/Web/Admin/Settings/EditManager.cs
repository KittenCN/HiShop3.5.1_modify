namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditManager : AdminPage
    {
        protected Button btnEditProfile;
        protected RoleDropDownList dropRole;
        protected Literal lblLoginNameValue;
        protected FormatedTimeLabel lblRegsTimeValue;
        protected HtmlForm thisForm;
        protected TextBox txtprivateEmail;
        private int userId;

        protected EditManager() : base("m09", "szp11")
        {
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                ManagerInfo manager = ManagerHelper.GetManager(this.userId);
                manager.Email = this.txtprivateEmail.Text;
                if (this.ValidationManageEamilr(manager))
                {
                    manager.RoleId = this.dropRole.SelectedValue;
                    if (ManagerHelper.Update(manager))
                    {
                        this.ShowMsg("成功修改了当前管理员的个人资料", true);
                    }
                    else
                    {
                        this.ShowMsg("当前管理员的个人信息修改失败", false);
                    }
                }
            }
        }

        private void GetAccountInfo(ManagerInfo user)
        {
            this.lblLoginNameValue.Text = user.UserName;
            this.lblRegsTimeValue.Time = user.CreateDate;
            this.dropRole.SelectedValue = user.RoleId;
            if (Globals.GetCurrentManagerUserId() == this.userId)
            {
                this.dropRole.Enabled = false;
            }
        }

        private void GetPersonaInfo(ManagerInfo user)
        {
            this.txtprivateEmail.Text = user.Email;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditProfile.Click += new EventHandler(this.btnEditProfile_Click);
                if (!this.Page.IsPostBack)
                {
                    this.dropRole.DataBind();
                    ManagerInfo manager = ManagerHelper.GetManager(this.userId);
                    if (manager == null)
                    {
                        this.ShowMsg("匿名用户或非管理员用户不能编辑", false);
                    }
                    else
                    {
                        this.GetAccountInfo(manager);
                        this.GetPersonaInfo(manager);
                    }
                }
            }
        }

        private bool ValidationManageEamilr(ManagerInfo siteManager)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ManagerInfo>(siteManager, new string[] { "ValManagerEmail" });
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

