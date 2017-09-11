namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class MessageTemplate
    {
        public MessageTemplate()
        {
        }

        public MessageTemplate(string tagDescription, string name)
        {
            this.TagDescription = tagDescription;
            this.Name = name;
        }

        public string EmailBody { get; set; }

        public string EmailSubject { get; set; }

        public string InnerMessageBody { get; set; }

        public string InnerMessageSubject { get; set; }

        public bool IsSendWeixin_ToAdmin { get; set; }

        public bool IsSendWeixin_ToDistributor { get; set; }

        public bool IsSendWeixin_ToMember { get; set; }

        public string MessageType { get; set; }

        public string Name { get; private set; }

        public bool SendEmail { get; set; }

        public bool SendInnerMessage { get; set; }

        public bool SendSMS { get; set; }

        public bool SendWeixin { get; set; }

        public string SMSBody { get; set; }

        public string TagDescription { get; private set; }

        public string WeixinTemplateId { get; set; }

        public string WXOpenTM { get; set; }
    }
}

