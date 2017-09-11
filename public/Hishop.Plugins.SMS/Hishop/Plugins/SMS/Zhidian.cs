namespace Hishop.Plugins.SMS
{
    using Hishop.Plugins;
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    [Plugin("默认短信接口")]
    public class Zhidian : SMSSender
    {
        private const string Gateway = "http://agentin.zhidian3g.cn/MSMSEND.ewing?";

        private static bool Send(string url, out string returnMsg)
        {
            string str = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Timeout = 0x1388;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Default))
                    {
                        StringBuilder builder = new StringBuilder();
                        while (-1 != reader.Peek())
                        {
                            builder.Append(reader.ReadLine());
                        }
                        str = builder.ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                returnMsg = exception.Message;
                return false;
            }
            bool flag = false;
            switch (str)
            {
                case "1":
                    flag = true;
                    returnMsg = "提交短信成功";
                    return flag;

                case "-1":
                    returnMsg = "不能初始化SO";
                    return flag;

                case "-2":
                    returnMsg = "网络不通";
                    return flag;

                case "-3":
                    returnMsg = "一次发送的手机号码过多";
                    return flag;

                case "-4":
                    returnMsg = "内容包含不合法文字";
                    return flag;

                case "-5":
                    returnMsg = "登录账户错误";
                    return flag;

                case "-6":
                    returnMsg = "通信数据传送";
                    return flag;

                case "-7":
                    returnMsg = "没有进行参数初始化";
                    return flag;

                case "-8":
                    returnMsg = "扩展号码长度不对";
                    return flag;

                case "-9":
                    returnMsg = "手机号码不合";
                    return flag;

                case "-10":
                    returnMsg = "号码太长";
                    return flag;

                case "-11":
                    returnMsg = "内容太长";
                    return flag;

                case "-12":
                    returnMsg = "内部错误";
                    return flag;

                case "-13":
                    returnMsg = "余额不足";
                    return flag;

                case "-14":
                    returnMsg = "扩展号不正确";
                    return flag;

                case "-17":
                    returnMsg = "发送内容为空";
                    return flag;

                case "-19":
                    returnMsg = "没有找到该动作（不存在的url地址）";
                    return flag;

                case "-20":
                    returnMsg = "手机号格式不正确";
                    return flag;

                case "-50":
                    returnMsg = "配置参数错误";
                    return flag;
            }
            returnMsg = "未知错误";
            return flag;
        }

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
            StringBuilder builder = new StringBuilder();
            builder.Append("http://agentin.zhidian3g.cn/MSMSEND.ewing?");
            builder.Append("ECODE=").Append(this.ECode);
            builder.Append("&USERNAME=").Append(this.Username);
            builder.Append("&PASSWORD=").Append(this.Password);
            builder.Append("&MOBILE=").Append(string.Join(",", phoneNumbers));
            builder.Append("&CONTENT=");
            builder.Append(Uri.EscapeDataString(message));
            return Send(builder.ToString(), out returnMsg);
        }

        public override bool Send(string cellPhone, string message, out string returnMsg, string speed = "0")
        {
            if (((string.IsNullOrEmpty(cellPhone) || string.IsNullOrEmpty(message)) || (cellPhone.Trim().Length == 0)) || (message.Trim().Length == 0))
            {
                returnMsg = "手机号码和消息内容不能为空";
                return false;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("http://agentin.zhidian3g.cn/MSMSEND.ewing?");
            builder.Append("ECODE=").Append(this.ECode);
            builder.Append("&USERNAME=").Append(this.Username);
            builder.Append("&PASSWORD=").Append(this.Password);
            builder.Append("&MOBILE=").Append(cellPhone);
            builder.Append("&CONTENT=");
            builder.Append(Uri.EscapeDataString(message));
            return Send(builder.ToString(), out returnMsg);
        }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        [ConfigElement("企业代码", Nullable=false)]
        public string ECode { get; set; }

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

        [ConfigElement("用户密码", Nullable=false, InputType=InputType.Password)]
        public string Password { get; set; }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }

        [ConfigElement("用户名", Nullable=false)]
        public string Username { get; set; }
    }
}

