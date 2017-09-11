namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UserGroupSet : AdminPage
    {
        protected Button btnSaveClientSettings;
        protected Script Script5;
        protected Script Script6;
        protected HtmlForm thisForm;
        protected HtmlInputText txt_time;

        protected UserGroupSet() : base("m04", "hyp05")
        {
        }

        protected void btnSaveClientSettings_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_time.Value))
            {
                this.ShowMsg("请输出时间间隔！", false);
            }
            else if (Convert.ToInt32(this.txt_time.Value) < 1)
            {
                this.ShowMsg("请输入大于1的整数！", false);
            }
            else if (this.txt_time.Value.Length > 3)
            {
                this.ShowMsg("间隔时间不能超过999天！", false);
            }
            else if (Convert.ToInt32(this.txt_time.Value) > 0x3e7)
            {
                this.ShowMsg("间隔时间不能超过999天！", false);
            }
            else if (MemberHelper.SetUserGroup(Convert.ToInt32(this.txt_time.Value)) > 0)
            {
                this.ShowMsg("设置成功", true);
            }
            else
            {
                this.ShowMsg("设置失败", false);
            }
        }

        public void DataBind()
        {
            this.txt_time.Value = MemberHelper.SelectUserGroupSet().ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveClientSettings.Click += new EventHandler(this.btnSaveClientSettings_Click);
            if (!base.IsPostBack)
            {
                this.DataBind();
            }
        }
    }
}

