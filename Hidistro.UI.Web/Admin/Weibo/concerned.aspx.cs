using ASPNET.WebControls;
using global::ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Weibo
{
    public partial class concerned : AdminPage
    {
        protected bool _enable;
     

        protected concerned() : base("m07", "wbp07")
        {
        }

        public void bind()
        {
            this.repreply.DataSource = WeiboHelper.GetReplyTypeInfo(2);
            this.repreply.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this._enable = masterSettings.SubscribeReply;
                this.bind();
            }
        }
    }
}