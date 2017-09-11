namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Setting : AdminPage
    {
        protected bool _EnabeHomePageBottomLink;
        protected bool _EnableHomePageBottomCopyright;
        protected bool _IsHomeShowFloatMenu;
        protected Button btnResetHomePageBottomLink;
        protected Button Button1;
        protected HtmlInputText fenxiaoAddress;
        protected HtmlInputText fenxiaoCopyLink;
        protected HtmlInputText fenxiaoCopyright;
        protected HtmlInputText fenxiaoName;
        protected Script Script4;
        private SiteSettings siteSettings;
        protected HtmlForm thisForm;
        protected TextBox txtCopyLink;
        protected TextBox txtCopyName;
        protected TextBox txtDistributionLink;
        protected TextBox txtName;

        protected Setting() : base("m01", "dpp12")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void btnResetHomePageBottomLink_Click(object sender, EventArgs e)
        {
            this.siteSettings.DistributionLinkName = "申请分销";
            this.siteSettings.DistributionLink = "/Vshop/DistributorRegCheck.aspx";
            SettingsManager.Save(this.siteSettings);
            this._EnabeHomePageBottomLink = this.siteSettings.EnabeHomePageBottomLink;
            this._EnableHomePageBottomCopyright = this.siteSettings.EnableHomePageBottomCopyright;
            this.txtName.Text = this.siteSettings.DistributionLinkName;
            this.txtDistributionLink.Text = this.siteSettings.DistributionLink;
            this.txtCopyName.Text = this.siteSettings.CopyrightLinkName;
            this.txtCopyLink.Text = this.siteSettings.CopyrightLink;
            this.ShowMsgAndReUrl("底部文字链接恢复默认成功！", true, "Setting.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.siteSettings.CopyrightLinkName = "Hishop技术支持";
            this.siteSettings.CopyrightLink = "http://www.hishop.com.cn/support/";
            SettingsManager.Save(this.siteSettings);
            this._EnabeHomePageBottomLink = this.siteSettings.EnabeHomePageBottomLink;
            this._EnableHomePageBottomCopyright = this.siteSettings.EnableHomePageBottomCopyright;
            this.txtName.Text = this.siteSettings.DistributionLinkName;
            this.txtDistributionLink.Text = this.siteSettings.DistributionLink;
            this.txtCopyName.Text = this.siteSettings.CopyrightLinkName;
            this.txtCopyLink.Text = this.siteSettings.CopyrightLink;
            this.fenxiaoName.Value = this.siteSettings.DistributionLinkName;
            this.fenxiaoAddress.Value = this.siteSettings.DistributionLink;
            this.fenxiaoCopyright.Value = this.siteSettings.CopyrightLinkName;
            this.fenxiaoCopyLink.Value = this.siteSettings.CopyrightLink;
            this.ShowMsgAndReUrl("底部版权信息恢复默认成功！", true, "Setting.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                switch (Globals.RequestFormStr("type"))
                {
                    case "EnabeHomePageBottomLink":
                        try
                        {
                            base.Response.ContentType = "text/plain";
                            bool flag = bool.Parse(Globals.RequestFormStr("enable"));
                            this.siteSettings.EnabeHomePageBottomLink = flag;
                            SettingsManager.Save(this.siteSettings);
                            base.Response.Write("保存成功");
                        }
                        catch (Exception exception)
                        {
                            base.Response.Write("保存失败！（" + exception.ToString() + ")");
                        }
                        base.Response.End();
                        return;

                    case "EnableHomePageBottomCopyright":
                        try
                        {
                            base.Response.ContentType = "text/plain";
                            bool flag2 = bool.Parse(Globals.RequestFormStr("enable"));
                            this.siteSettings.EnableHomePageBottomCopyright = flag2;
                            SettingsManager.Save(this.siteSettings);
                            base.Response.Write("保存成功");
                        }
                        catch (Exception exception2)
                        {
                            base.Response.Write("保存失败！（" + exception2.ToString() + ")");
                        }
                        base.Response.End();
                        return;

                    case "DistributionLink":
                        try
                        {
                            base.Response.ContentType = "text/plain";
                            string str2 = Globals.RequestFormStr("txt1");
                            string str3 = Globals.RequestFormStr("txt2");
                            this.siteSettings.DistributionLinkName = str2;
                            this.siteSettings.DistributionLink = str3;
                            SettingsManager.Save(this.siteSettings);
                            base.Response.Write("保存成功");
                        }
                        catch (Exception exception3)
                        {
                            base.Response.Write("保存失败！（" + exception3.ToString() + ")");
                        }
                        base.Response.End();
                        return;

                    case "CopyrightLink":
                        try
                        {
                            base.Response.ContentType = "text/plain";
                            string str4 = Globals.RequestFormStr("txt3");
                            string str5 = Globals.RequestFormStr("txt4");
                            this.siteSettings.CopyrightLinkName = str4;
                            this.siteSettings.CopyrightLink = str5;
                            SettingsManager.Save(this.siteSettings);
                            base.Response.Write("保存成功");
                        }
                        catch (Exception exception4)
                        {
                            base.Response.Write("保存失败！（" + exception4.ToString() + ")");
                        }
                        base.Response.End();
                        return;

                    case "IsHomeShowFloatMenu":
                        try
                        {
                            base.Response.ContentType = "text/plain";
                            bool flag3 = bool.Parse(Globals.RequestFormStr("enable"));
                            this.siteSettings.IsHomeShowFloatMenu = flag3;
                            SettingsManager.Save(this.siteSettings);
                            base.Response.Write("保存成功");
                        }
                        catch (Exception exception5)
                        {
                            base.Response.Write("保存失败！（" + exception5.ToString() + ")");
                        }
                        base.Response.End();
                        return;
                }
                this._EnabeHomePageBottomLink = this.siteSettings.EnabeHomePageBottomLink;
                this._EnableHomePageBottomCopyright = this.siteSettings.EnableHomePageBottomCopyright;
                this.txtName.Text = this.siteSettings.DistributionLinkName;
                this.txtDistributionLink.Text = this.siteSettings.DistributionLink;
                this.txtCopyName.Text = this.siteSettings.CopyrightLinkName;
                this.txtCopyLink.Text = this.siteSettings.CopyrightLink;
                this.fenxiaoName.Value = this.siteSettings.DistributionLinkName;
                this.fenxiaoAddress.Value = this.siteSettings.DistributionLink;
                this.fenxiaoCopyright.Value = this.siteSettings.CopyrightLinkName;
                this.fenxiaoCopyLink.Value = this.siteSettings.CopyrightLink;
                if (this.siteSettings.IsHomeShowFloatMenu)
                {
                    this._IsHomeShowFloatMenu = true;
                }
            }
        }
    }
}

