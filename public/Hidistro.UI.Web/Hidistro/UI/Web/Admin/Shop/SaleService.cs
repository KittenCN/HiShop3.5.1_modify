namespace Hidistro.UI.Web.Admin.Shop
{
   using  global:: ControlPanel.Settings;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.MeiQia.Api.Api;
    using Hishop.MeiQia.Api.Util;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Security;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SaleService : AdminPage
    {
        protected Button btnBindUser;
        protected Button btnClear;
        protected Button ChangePwd;
        protected bool enable;
        protected Repeater grdCustomers;
        protected HtmlInputHidden htxtRoleId;
        protected Button OpenAccount;
        protected TextBox txt_cphone;
        protected TextBox txt_cpwd;
        protected TextBox txt_id;
        protected TextBox txt_name;
        protected TextBox txt_phone;
        protected TextBox txt_pwd;

        protected SaleService() : base("m01", "dpp05")
        {
        }

        private void BindCustomers(string unit)
        {
            DataTable customers = CustomerServiceHelper.GetCustomers(unit);
            this.grdCustomers.DataSource = customers;
            this.grdCustomers.DataBind();
        }

        protected void btnBindUser_Click(object sender, EventArgs e)
        {
            string str = this.txt_phone.Text.Trim();
            string text = this.txt_pwd.Text;
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请输入邮箱！", false);
            }
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请输入密码！", false);
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str3 = string.Empty;
            if (!string.IsNullOrEmpty(masterSettings.DistributorLogoPic))
            {
                str3 = Globals.DomainName + masterSettings.DistributorLogoPic;
            }
            CustomerServiceSettings settings = CustomerServiceManager.GetMasterSettings(false);
            string tokenValue = TokenApi.GetTokenValue(settings.AppId, settings.AppSecret);
            string str5 = FormsAuthentication.HashPasswordForStoringInConfigFile(text, "MD5").ToLower();
            if (!string.IsNullOrEmpty(tokenValue))
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("unit", str);
                parameters.Add("password", str5);
                parameters.Add("unitname", masterSettings.SiteName);
                parameters.Add("activated", "1");
                parameters.Add("logo", str3);
                parameters.Add("url", "");
                parameters.Add("tel", masterSettings.ShopTel);
                parameters.Add("contact", "");
                parameters.Add("location", "");
                string str6 = EnterpriseApi.CreateEnterprise(tokenValue, parameters);
                if (!string.IsNullOrWhiteSpace(str6))
                {
                    string jsonValue = Common.GetJsonValue(str6, "errcode");
                    Common.GetJsonValue(str6, "errmsg");
                    if (jsonValue == "10020")
                    {
                        string unitId = EnterpriseApi.GetUnitId(tokenValue, str);
                        if (!string.IsNullOrEmpty(unitId))
                        {
                            settings.unitid = unitId;
                            settings.unit = this.txt_phone.Text;
                            settings.password = this.txt_pwd.Text;
                            CustomerServiceManager.Save(settings);
                            this.ShowMsgAndReUrl("绑定成功。", true, "SaleService.aspx");
                        }
                    }
                    else
                    {
                        this.ShowMsgAndReUrl("账号不存在！", true, "SaleService.aspx");
                    }
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
                if (masterSettings != null)
                {
                    masterSettings.unitid = "";
                    masterSettings.unit = "";
                    masterSettings.password = "";
                    CustomerServiceManager.Save(masterSettings);
                    this.ShowMsgAndReUrl("已回到初始状态，请重新配置在线客服！", true, "SaleService.aspx");
                }
            }
            catch (Exception)
            {
            }
        }

        protected void ChangePwd_Click(object sender, EventArgs e)
        {
            CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
            if (string.IsNullOrEmpty(this.txt_phone.Text))
            {
                this.ShowMsg("请输入手机号码！", false);
            }
            if (string.IsNullOrEmpty(this.txt_pwd.Text))
            {
                this.ShowMsg("请输入密码！", false);
            }
            string tokenValue = TokenApi.GetTokenValue(masterSettings.AppId, masterSettings.AppSecret);
            if (!string.IsNullOrEmpty(tokenValue))
            {
                SiteSettings settings2 = SettingsManager.GetMasterSettings(false);
                string str2 = string.Empty;
                if (!string.IsNullOrEmpty(settings2.DistributorLogoPic))
                {
                    str2 = Globals.DomainName + settings2.DistributorLogoPic;
                }
                string str3 = FormsAuthentication.HashPasswordForStoringInConfigFile(this.txt_pwd.Text, "MD5").ToLower();
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("unit", this.txt_phone.Text);
                parameters.Add("password", str3);
                parameters.Add("unitname", settings2.SiteName);
                parameters.Add("activated", "1");
                parameters.Add("logo", str2);
                parameters.Add("url", "");
                parameters.Add("tel", settings2.ShopTel);
                parameters.Add("contact", "");
                parameters.Add("location", "");
                string str4 = EnterpriseApi.UpdateEnterprise(tokenValue, parameters);
                if (!string.IsNullOrWhiteSpace(str4))
                {
                    string jsonValue = Common.GetJsonValue(str4, "errcode");
                    string str6 = Common.GetJsonValue(str4, "errmsg");
                    if (jsonValue == "0")
                    {
                        masterSettings.password = this.txt_pwd.Text;
                        CustomerServiceManager.Save(masterSettings);
                        this.ShowMsg("修改密码成功！", true);
                    }
                    else
                    {
                        this.ShowMsg("修改密码失败！(" + str6 + ")", false);
                    }
                }
                else
                {
                    this.ShowMsg("修改密码失败！", false);
                }
                this.enable = settings2.EnableSaleService;
            }
            else
            {
                this.ShowMsg("获取access_token失败！", false);
            }
        }

        protected void OpenAccount_Click(object sender, EventArgs e)
        {
            CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
            if (string.IsNullOrEmpty(this.txt_phone.Text))
            {
                this.ShowMsg("请输入手机号码！", false);
            }
            if (string.IsNullOrEmpty(this.txt_pwd.Text))
            {
                this.ShowMsg("请输入密码！", false);
            }
            string tokenValue = TokenApi.GetTokenValue(masterSettings.AppId, masterSettings.AppSecret);
            if (!string.IsNullOrEmpty(tokenValue))
            {
                SiteSettings settings2 = SettingsManager.GetMasterSettings(false);
                string str2 = string.Empty;
                if (!string.IsNullOrEmpty(settings2.DistributorLogoPic))
                {
                    str2 = Globals.DomainName + settings2.DistributorLogoPic;
                }
                string str3 = FormsAuthentication.HashPasswordForStoringInConfigFile(this.txt_pwd.Text, "MD5").ToLower();
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("unit", this.txt_phone.Text);
                parameters.Add("password", str3);
                parameters.Add("unitname", settings2.SiteName);
                parameters.Add("activated", "1");
                parameters.Add("logo", str2);
                parameters.Add("url", "");
                parameters.Add("tel", settings2.ShopTel);
                parameters.Add("contact", "");
                parameters.Add("location", "");
                string str4 = EnterpriseApi.CreateEnterprise(tokenValue, parameters);
                if (!string.IsNullOrWhiteSpace(str4))
                {
                    string jsonValue = Common.GetJsonValue(str4, "errcode");
                    string str6 = Common.GetJsonValue(str4, "errmsg");
                    if (jsonValue == "0")
                    {
                        string unitId = EnterpriseApi.GetUnitId(tokenValue, this.txt_phone.Text);
                        if (!string.IsNullOrEmpty(unitId))
                        {
                            masterSettings.unitid = unitId;
                            masterSettings.unit = this.txt_phone.Text;
                            masterSettings.password = this.txt_pwd.Text;
                            CustomerServiceManager.Save(masterSettings);
                            this.ShowMsgAndReUrl("开通主账号成功！", true, "saleservice.aspx");
                        }
                        else
                        {
                            this.ShowMsg("获取主账号Id失败！", false);
                        }
                    }
                    else
                    {
                        string str8 = str6;
                        if (str8.Contains("has registered"))
                        {
                            this.ShowMsg("您输入的账号名称已被注册，请更换一个再注册！", false);
                        }
                        else
                        {
                            this.ShowMsg("开通主账号失败！(" + str6 + ")", false);
                        }
                    }
                }
                else
                {
                    this.ShowMsg("开通主账号失败！", false);
                }
                this.enable = settings2.EnableSaleService;
            }
            else
            {
                this.ShowMsg("获取access_token失败！", false);
            }
        }

        [PrivilegeCheck(Privilege.Summary)]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (!string.IsNullOrEmpty(masterSettings.MeiQiaEntId))
                {
                    base.Response.Redirect("MeiQia.aspx");
                }
                this.enable = masterSettings.EnableSaleService;
                CustomerServiceSettings settings = CustomerServiceManager.GetMasterSettings(false);
                this.txt_phone.Text = settings.unit;
                this.txt_pwd.Attributes["Value"] = settings.password;
                if (!string.IsNullOrEmpty(settings.unit))
                {
                    this.txt_phone.Enabled = false;
                    this.OpenAccount.Visible = false;
                    this.ChangePwd.Visible = true;
                    this.btnBindUser.Visible = false;
                    this.btnClear.Visible = true;
                    if (string.IsNullOrEmpty(settings.unitid))
                    {
                        string tokenValue = TokenApi.GetTokenValue(settings.AppId, settings.AppSecret);
                        if (!string.IsNullOrEmpty(tokenValue))
                        {
                            string unitId = EnterpriseApi.GetUnitId(tokenValue, settings.unit);
                            settings.unitid = unitId;
                            CustomerServiceManager.Save(settings);
                        }
                    }
                }
                else
                {
                    base.Response.Redirect("MeiQia.aspx");
                }
                this.BindCustomers(settings.unit);
            }
        }
    }
}

