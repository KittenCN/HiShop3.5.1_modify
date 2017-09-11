namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditManagerPassword : AdminPage
    {
        protected Button btnEditPassWordOK;
        protected Literal lblLoginNameValue;
        protected HtmlGenericControl panelOld;
        protected Script Script4;
        protected HtmlForm thisForm;
        protected TextBox txtNewPassWord;
        protected TextBox txtOldPassWord;
        protected TextBox txtPassWordCompare;
        private int userId;

        protected EditManagerPassword() : base("m09", "szp11")
        {
        }

        private void btnEditPassWordOK_Click(object sender, EventArgs e)
        {
            ManagerInfo manager = ManagerHelper.GetManager(this.userId);
            if ((Globals.GetCurrentManagerUserId() == this.userId) && (manager.Password != HiCryptographer.Md5Encrypt(this.txtOldPassWord.Text)))
            {
                this.ShowMsg("旧密码输入不正确", false);
            }
            else if ((string.IsNullOrEmpty(this.txtNewPassWord.Text) || (this.txtNewPassWord.Text.Length > 20)) || (this.txtNewPassWord.Text.Length < 6))
            {
                this.ShowMsg("密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (string.Compare(this.txtNewPassWord.Text, this.txtPassWordCompare.Text) != 0)
            {
                this.ShowMsg("两次输入的密码不一样", false);
            }
            else
            {
                HiConfiguration config = HiConfiguration.GetConfig();
                if ((string.IsNullOrEmpty(this.txtNewPassWord.Text) || (this.txtNewPassWord.Text.Length < 6)) || (this.txtNewPassWord.Text.Length > config.PasswordMaxLength))
                {
                    this.ShowMsg(string.Format("管理员登录密码的长度只能在{0}和{1}个字符之间", 6, config.PasswordMaxLength), false);
                }
                else
                {
                    manager.Password = HiCryptographer.Md5Encrypt(this.txtNewPassWord.Text);
                    if (ManagerHelper.Update(manager))
                    {
                        this.ShowMsg("成功修改了管理员登录密码", true);
                    }
                }
            }
        }

        private void GetSecurity()
        {
            if (Globals.GetCurrentManagerUserId() != this.userId)
            {
                this.panelOld.Visible = false;
            }
            else
            {
                this.panelOld.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditPassWordOK.Click += new EventHandler(this.btnEditPassWordOK_Click);
                if (!this.Page.IsPostBack)
                {
                    ManagerInfo manager = ManagerHelper.GetManager(this.userId);
                    if (manager == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.lblLoginNameValue.Text = manager.UserName;
                        this.GetSecurity();
                    }
                }
            }
        }
    }
}

