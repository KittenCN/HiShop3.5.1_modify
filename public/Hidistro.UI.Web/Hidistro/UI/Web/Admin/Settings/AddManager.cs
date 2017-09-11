namespace Hidistro.UI.Web.Admin.settings
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Text.RegularExpressions;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddManager : AdminPage
    {
        protected Button btnSave;
        protected RoleDropDownList dropRole;
        protected HiddenField hidpic;
        protected HiddenField hidpicdel;
        protected Script Script4;
        protected Script Script5;
        protected Script Script6;
        protected Script Script7;
        protected HtmlForm thisForm;
        protected TextBox txtEmail;
        protected TextBox txtPassword;
        protected TextBox txtPasswordagain;
        protected TextBox txtUserName;

        protected AddManager() : base("m09", "szp11")
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string str = this.txtUserName.Text.Trim();
            if ((str.Length > 20) || (str.Length < 3))
            {
                this.ShowMsg("3-20个字符，支持汉字、字母、数字等组合", false);
            }
            else if ((this.txtPassword.Text.Length > 20) || (this.txtPassword.Text.Length < 6))
            {
                this.ShowMsg("密码为6-20个字符，可由英文‘数字及符号组成", false);
            }
            else if (string.Compare(this.txtPassword.Text, this.txtPasswordagain.Text) != 0)
            {
                this.ShowMsg("请确保两次输入的密码相同", false);
            }
            else if (string.Compare(this.txtPassword.Text, this.txtPasswordagain.Text) != 0)
            {
                this.ShowMsg("请确保两次输入的密码相同", false);
            }
            else
            {
                string input = this.txtEmail.Text.Trim();
                if (!Regex.IsMatch(input, @"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$"))
                {
                    this.ShowMsg("请输入有效的邮箱地址，长度在256个字符以内", false);
                }
                else
                {
                    int result = 0;
                    int.TryParse(this.dropRole.SelectedValue.ToString(), out result);
                    if (result == 0)
                    {
                        this.ShowMsg("所属部门没有选择，请选择", false);
                    }
                    else if (ManagerHelper.GetManager(this.txtUserName.Text.Trim()) != null)
                    {
                        this.ShowMsg("用户名已存在", false);
                    }
                    else
                    {
                        ManagerInfo manager = new ManagerInfo {
                            RoleId = result,
                            UserName = str,
                            Email = input,
                            Password = HiCryptographer.Md5Encrypt(this.txtPassword.Text.Trim())
                        };
                        if (ManagerHelper.Create(manager))
                        {
                            this.txtEmail.Text = string.Empty;
                            this.txtUserName.Text = string.Empty;
                            this.ShowMsg("成功添加了一个管理员", true);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.dropRole.DataBind();
            }
        }
    }
}

