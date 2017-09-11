namespace Hishop.Plugins.Email
{
    using Hishop.Plugins;
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml;

    [Plugin("ASP.NET邮件发送组件")]
    public class ASPNETMail : EmailSender
    {
        private SmtpClient smtp;

        protected override void InitConfig(XmlNode configXml)
        {
            base.InitConfig(configXml);
            SmtpClient client = new SmtpClient(this.SmtpServer, this.SmtpPort) {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(this.Username, this.Password),
                EnableSsl = this.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            this.smtp = client;
        }

        public override bool Send(MailMessage mail, Encoding emailEncoding)
        {
            if (mail == null)
            {
                throw new ArgumentNullException("mail");
            }
            if (mail.From == null)
            {
                mail.From = new MailAddress(this.ReplyAddress, this.DisplayName, emailEncoding);
            }
            if (mail.IsBodyHtml)
            {
                mail.Body = mail.Body;
            }
            mail.BodyEncoding = emailEncoding;
            this.smtp.Send(mail);
            return true;
        }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        [ConfigElement("显示名称", Nullable=false)]
        public string DisplayName { get; set; }

        [ConfigElement("安全连接(SSL)", InputType=InputType.CheckBox, Nullable=false)]
        public bool EnableSsl { get; set; }

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

        [ConfigElement("SMTP用户密码", InputType=InputType.Password, Nullable=false)]
        public string Password { get; set; }

        [ConfigElement("SMTP邮箱", Nullable=false)]
        public string ReplyAddress { get; set; }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }

        [ConfigElement("SMTP服务器端口", Nullable=false)]
        public int SmtpPort { get; set; }

        [ConfigElement("SMTP服务器", Nullable=false)]
        public string SmtpServer { get; set; }

        [ConfigElement("SMTP用户名", Nullable=false)]
        public string Username { get; set; }
    }
}

