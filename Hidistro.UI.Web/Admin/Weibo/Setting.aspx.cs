﻿using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Weibo
{
    public partial class Setting : AdminPage
    {
        protected Setting() : base("m07", "wbp01")
        {
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.Access_Token = this.txtAccess_Token.Text;
            SettingsManager.Save(masterSettings);
            this.ShowMsg("修改成功", true);
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.Access_Token = "";
            this.txtAccess_Token.Text = "";
            SettingsManager.Save(masterSettings);
            this.ShowMsg("解除授权成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.txtAccess_Token.Text = masterSettings.Access_Token;
            }
        }
    }
}