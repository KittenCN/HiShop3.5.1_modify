namespace Hishop.Plugins.SMS
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Web;

    [Plugin("短信接口")]
    public class ymSMS : SMSSender
    {
        private const string Gateway = "http://sms.kuaidiantong.cn/SendMsg.aspx";

        public override bool Send(string cellPhone, string message, out string returnMsg)
        {
            return this.Send(cellPhone, message, out returnMsg, "0");
        }

        public override bool Send(string[] phoneNumbers, string message, out string returnMsg)
        {
            return this.Send(phoneNumbers, message, out returnMsg, "1");
        }

        public override bool Send(string[] phoneNumbers, string message, out string returnMsg, string speed = "1")
        {
            if ((((phoneNumbers == null) || (phoneNumbers.Length == 0)) || string.IsNullOrEmpty(message)) || (message.Trim().Length == 0))
            {
                returnMsg = "手机号码和消息内容不能为空";
                return false;
            }
            SortedDictionary<string, string> dicArrayPre = new SortedDictionary<string, string>();
            dicArrayPre.Add("mobiles", string.Join(",", phoneNumbers));
            dicArrayPre.Add("text", message);
            dicArrayPre.Add("appkey", this.Appkey);
            dicArrayPre.Add("sendtime", DateTime.Now.ToString());
            dicArrayPre.Add("speed", speed);
            Dictionary<string, string> dicArray = SMSAPiHelper.Parameterfilter(dicArrayPre);
            string str = SMSAPiHelper.BuildSign(dicArray, this.Appsecret, "MD5", "utf-8");
            dicArray.Add("sign", str);
            dicArray.Add("sign_type", "MD5");
            string postData = SMSAPiHelper.CreateLinkstring(dicArray);
            try
            {
                string str3 = SMSAPiHelper.PostData("http://sms.kuaidiantong.cn/SendMsg.aspx", postData);
                if (str3 == "发送成功")
                {
                    returnMsg = "发送成功!";
                    return true;
                }
                returnMsg = str3;
                return false;
            }
            catch (Exception)
            {
                returnMsg = "未知错误";
                return false;
            }
        }

        public override bool Send(string cellPhone, string message, out string returnMsg, string speed = "0")
        {
            if (((string.IsNullOrEmpty(cellPhone) || string.IsNullOrEmpty(message)) || (cellPhone.Trim().Length == 0)) || (message.Trim().Length == 0))
            {
                returnMsg = "手机号码和消息内容不能为空";
                return false;
            }
            SortedDictionary<string, string> dicArrayPre = new SortedDictionary<string, string>();
            dicArrayPre.Add("mobiles", cellPhone);
            dicArrayPre.Add("text", message);
            dicArrayPre.Add("appkey", this.Appkey);
            dicArrayPre.Add("sendtime", DateTime.Now.ToString());
            dicArrayPre.Add("speed", speed);
            Dictionary<string, string> dicArray = SMSAPiHelper.Parameterfilter(dicArrayPre);
            string str = SMSAPiHelper.BuildSign(dicArray, this.Appsecret, "MD5", "utf-8");
            dicArray.Add("sign", str);
            dicArray.Add("sign_type", "MD5");
            this.writeError(str, cellPhone + "|" + message + "|" + this.Appkey + "|" + DateTime.Now.ToString() + "|" + speed);
            string postData = SMSAPiHelper.CreateLinkstring(dicArray);
            try
            {
                string str3 = SMSAPiHelper.PostData("http://sms.kuaidiantong.cn/SendMsg.aspx", postData);
                if (str3 == "发送成功")
                {
                    returnMsg = "发送成功!";
                    return true;
                }
                returnMsg = str3;
                return false;
            }
            catch (Exception)
            {
                returnMsg = "未知错误";
                return false;
            }
        }

        public void writeError(string syssign, string param)
        {
            DataTable table = new DataTable {
                TableName = "SMSLog"
            };
            table.Columns.Add(new DataColumn("time"));
            table.Columns.Add(new DataColumn("SysSign"));
            table.Columns.Add(new DataColumn("Sign"));
            DataRow row = table.NewRow();
            row["time"] = DateTime.Now;
            row["SysSign"] = syssign;
            row["sign"] = param;
            table.Rows.Add(row);
            table.WriteXml(HttpContext.Current.Request.MapPath("/SMSLog.xml"));
        }

        [ConfigElement("Appkey", Nullable=false)]
        public string Appkey { get; set; }

        [ConfigElement("Appsecret", Nullable=false)]
        public string Appsecret { get; set; }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        public override string Logo
        {
            get
            {
                return string.Empty;
            }
        }

        protected override bool NeedProtect
        {
            get
            {
                return true;
            }
        }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }
    }
}

