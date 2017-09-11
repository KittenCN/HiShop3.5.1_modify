namespace Hidistro.UI.Web.Admin.Shop
{
    using ASPNET.WebControls;
    using  global:: ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class AtMessage : AdminPage
    {
        protected bool _enable;
        protected Pager pager;
        protected Repeater repreply;

        protected AtMessage() : base("m07", "wbp07")
        {
        }

        public void bind()
        {
            this.repreply.DataSource = WeiboHelper.GetReplyTypeInfo(3);
            this.repreply.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this._enable = masterSettings.ByRemind;
                this.bind();
            }
        }
    }
}

