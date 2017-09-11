namespace Hidistro.Messages
{
    using Hidistro.Core;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.Store;
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;

    public static class MessageTemplateHelper
    {
        private const string CacheKey = "Message-{0}";
        private const string DistributorCacheKey = "Message-{0}-{1}";

        public static List<string> GetAdminUserMsgList(string FieldName)
        {
            return new MessageTemplateDao().GetAdminUserMsgList(FieldName);
        }

        internal static MailMessage GetEmailTemplate(MessageTemplate template, string emailTo)
        {
            if (((template == null) || !template.SendEmail) || string.IsNullOrEmpty(emailTo))
            {
                return null;
            }
            MailMessage message = new MailMessage {
                IsBodyHtml = true,
                Priority = MailPriority.High,
                Body = template.EmailBody.Trim(),
                Subject = template.EmailSubject.Trim()
            };
            message.To.Add(emailTo);
            return message;
        }

        public static List<string> GetFuwuAdminUserMsgList(string FieldName)
        {
            return new MessageTemplateDao().GetFuwuAdminUserMsgList(FieldName);
        }

        public static MessageTemplate GetFuwuMessageTemplateByDetailType(string DetailType)
        {
            if (string.IsNullOrEmpty(DetailType))
            {
                return null;
            }
            return new MessageTemplateDao().GetFuwuMessageTemplateByDetailType(DetailType);
        }

        internal static MessageTemplate GetFuwuTemplate(string messageType)
        {
            messageType = messageType.ToLower();
            SettingsManager.GetMasterSettings(false);
            string key = string.Format("Message-{0}", "fuwu" + messageType);
            MessageTemplate fuwuMessageTemplate = HiCache.Get(key) as MessageTemplate;
            if (fuwuMessageTemplate == null)
            {
                if (string.IsNullOrEmpty(messageType))
                {
                    return null;
                }
                fuwuMessageTemplate = new MessageTemplateDao().GetFuwuMessageTemplate(messageType);
                if (fuwuMessageTemplate != null)
                {
                    HiCache.Max(key, fuwuMessageTemplate);
                }
            }
            return fuwuMessageTemplate;
        }

        public static MessageTemplate GetMessageTemplate(string messageType)
        {
            if (string.IsNullOrEmpty(messageType))
            {
                return null;
            }
            return new MessageTemplateDao().GetMessageTemplate(messageType);
        }

        public static MessageTemplate GetMessageTemplateByDetailType(string DetailType)
        {
            if (string.IsNullOrEmpty(DetailType))
            {
                return null;
            }
            return new MessageTemplateDao().GetMessageTemplateByDetailType(DetailType);
        }

        public static IList<MessageTemplate> GetMessageTemplates()
        {
            return new MessageTemplateDao().GetMessageTemplates();
        }

        internal static MessageTemplate GetTemplate(string messageType)
        {
            messageType = messageType.ToLower();
            SettingsManager.GetMasterSettings(false);
            string key = string.Format("Message-{0}", messageType);
            MessageTemplate messageTemplate = HiCache.Get(key) as MessageTemplate;
            if (messageTemplate == null)
            {
                messageTemplate = GetMessageTemplate(messageType);
                if (messageTemplate != null)
                {
                    HiCache.Max(key, messageTemplate);
                }
            }
            return messageTemplate;
        }

        public static string GetUserOpenIdByUserId(int UserId)
        {
            if (UserId <= 0)
            {
                return "";
            }
            return new MessageTemplateDao().GetUserOpenIdByUserId(UserId);
        }

        public static void UpdateSettings(IList<MessageTemplate> templates)
        {
            if ((templates != null) && (templates.Count != 0))
            {
                new MessageTemplateDao().UpdateSettings(templates);
                foreach (MessageTemplate template in templates)
                {
                    HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
                }
            }
        }

        public static void UpdateTemplate(MessageTemplate template)
        {
            if (template != null)
            {
                new MessageTemplateDao().UpdateTemplate(template);
                HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
            }
        }
    }
}

