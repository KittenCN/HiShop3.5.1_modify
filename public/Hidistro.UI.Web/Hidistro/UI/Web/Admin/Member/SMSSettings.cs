namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Plugins;
    using Ionic.Zlib;
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class SMSSettings : AdminPage
    {
        protected Button btnSaveSMSSettings;
        protected Button btnTestSend;
        protected Label lbMsg;
        protected Script Script1;
        protected HtmlForm thisForm;
        protected TextBox txtAppkey;
        protected TextBox txtAppsecret;
        protected HiddenField txtConfigData;
        protected HiddenField txtSelectedName;
        protected TextBox txtTestCellPhone;
        protected TextBox txtTestSubject;

        protected SMSSettings() : base("m04", "hyp08")
        {
        }

        private void btnSaveSMSSettings_Click(object sender, EventArgs e)
        {
            string str;
            ConfigData data = this.LoadConfig(out str);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (string.IsNullOrEmpty(str) || (data == null))
            {
                masterSettings.SMSSender = string.Empty;
                masterSettings.SMSSettings = string.Empty;
            }
            else
            {
                if (!data.IsValid)
                {
                    string msg = "";
                    foreach (string str3 in data.ErrorMsgs)
                    {
                        msg = msg + Formatter.FormatErrorMessage(str3);
                    }
                    this.ShowMsg(msg, false);
                    return;
                }
                masterSettings.SMSSender = str;
                masterSettings.SMSSettings = HiCryptographer.Encrypt(data.SettingsXml);
            }
            SettingsManager.Save(masterSettings);
            this.ShowMsg("设置成功", true);
        }

        private void btnTestSend_Click(object sender, EventArgs e)
        {
            string str;
            ConfigData data = this.LoadConfig(out str);
            if (string.IsNullOrEmpty(str) || (data == null))
            {
                this.ShowMsg("请先选择发送方式并填写配置信息", false);
            }
            else if (!data.IsValid)
            {
                string msg = "";
                foreach (string str3 in data.ErrorMsgs)
                {
                    msg = msg + Formatter.FormatErrorMessage(str3);
                }
                this.ShowMsg(msg, false);
            }
            else if ((string.IsNullOrEmpty(this.txtTestCellPhone.Text) || string.IsNullOrEmpty(this.txtTestSubject.Text)) || ((this.txtTestCellPhone.Text.Trim().Length == 0) || (this.txtTestSubject.Text.Trim().Length == 0)))
            {
                this.ShowMsg("接收手机号和发送内容不能为空", false);
            }
            else if (!Regex.IsMatch(this.txtTestCellPhone.Text.Trim(), @"^(13|14|15|18)\d{9}$"))
            {
                this.ShowMsg("请填写正确的手机号码", false);
            }
            else
            {
                string str4;
                bool success = SMSSender.CreateInstance(str, data.SettingsXml).Send(this.txtTestCellPhone.Text.Trim(), this.txtTestSubject.Text.Trim(), out str4);
                this.ShowMsg(str4, success);
            }
        }

        protected string GetAmount(SiteSettings settings)
        {
            int num;
            if (string.IsNullOrEmpty(settings.SMSSettings))
            {
                return "";
            }
            string xml = HiCryptographer.Decrypt(settings.SMSSettings);
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            string innerText = document.SelectSingleNode("xml/Appkey").InnerText;
            string postData = "method=getAmount&Appkey=" + innerText;
            string s = this.PostData("http://sms.kuaidiantong.cn/getAmount.aspx", postData);
            if (int.TryParse(s, out num))
            {
                return ("您的短信剩余条数为：" + s.ToString());
            }
            return "获取短信条数发生错误，请检查Appkey是否输入正确!";
        }

        private string GetBetweenStr(string FromStr, string SubStr1, string SubStr2)
        {
            return Regex.Match(this.txtConfigData.Value, "(?<=" + SubStr1 + ").*?(?=" + SubStr2 + ")").Value;
        }

        private ConfigData LoadConfig(out string selectedName)
        {
            selectedName = base.Request.Form["ddlSms"];
            this.txtSelectedName.Value = selectedName;
            this.txtConfigData.Value = "";
            if (string.IsNullOrEmpty(selectedName) || (selectedName.Length == 0))
            {
                return null;
            }
            if (SMSSender.CreateInstance(selectedName) == null)
            {
                return null;
            }
            this.txtConfigData.Value = string.Format("<xml><Appkey>{0}</Appkey><Appsecret>{1}</Appsecret></xml>", this.txtAppkey.Text.Trim(), this.txtAppsecret.Text.Trim());
            ConfigData data = new ConfigData(this.txtConfigData.Value);
            if (data != null)
            {
                this.txtConfigData.Value = data.SettingsXml;
            }
            return data;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSaveSMSSettings.Click += new EventHandler(this.btnSaveSMSSettings_Click);
            this.btnTestSend.Click += new EventHandler(this.btnTestSend_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (masterSettings.SMSEnabled)
                {
                    ConfigData data = new ConfigData(HiCryptographer.Decrypt(masterSettings.SMSSettings));
                    this.txtConfigData.Value = data.SettingsXml;
                }
                this.txtSelectedName.Value = "hishop.plugins.sms.ymsms";
                string str = this.GetBetweenStr(this.txtConfigData.Value, "<Appkey>", "</Appkey>");
                string str2 = this.GetBetweenStr(this.txtConfigData.Value, "<Appsecret>", "</Appsecret>");
                this.txtAppkey.Text = str;
                this.txtAppsecret.Text = str2;
            }
        }

        public string PostData(string url, string postData)
        {
            string str = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stream2 = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        Stream stream3 = stream2;
                        if (response.ContentEncoding.ToLower() == "gzip")
                        {
                            stream3 = new GZipStream(stream2, CompressionMode.Decompress);
                        }
                        else if (response.ContentEncoding.ToLower() == "deflate")
                        {
                            stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
                        }
                        using (StreamReader reader = new StreamReader(stream3, encoding))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                str = string.Format("获取信息错误：{0}", exception.Message);
            }
            return str;
        }
    }
}

