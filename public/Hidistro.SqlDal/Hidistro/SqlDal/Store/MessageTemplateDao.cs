namespace Hidistro.SqlDal.Store
{
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class MessageTemplateDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public List<string> GetAdminUserMsgList(string MsgFieldName)
        {
            List<string> list = new List<string>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_MessageAdminUserMsgList WHERE isnull(" + MsgFieldName + ",0) = 1 and  Type=0 ");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((string) reader["UserOpenId"]);
                }
                reader.Close();
            }
            return list;
        }

        public List<string> GetFuwuAdminUserMsgList(string MsgFieldName)
        {
            List<string> list = new List<string>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_MessageAdminUserMsgList WHERE isnull(" + MsgFieldName + ",0) = 1  and  Type=1");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((string) reader["UserOpenId"]);
                }
                reader.Close();
            }
            return list;
        }

        public MessageTemplate GetFuwuMessageTemplate(string messageType)
        {
            MessageTemplate template = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AliFuWuMessageTemplates WHERE LOWER(MessageType) = LOWER(@MessageType)");
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String, messageType);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    template = this.PopulateEmailTempletFromIDataReader(reader);
                }
                reader.Close();
            }
            return template;
        }

        public MessageTemplate GetFuwuMessageTemplateByDetailType(string DetailType)
        {
            MessageTemplate template = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("  select a.DetailType, a.DetailName, a.AllowToAdmin,a.AllowToDistributor,a.AllowToMember, a.IsSelectedByDistributor,a.IsSelectedByMember,\r\n                    b.* from Hishop_AliFuWuMessageTemplatesDetail a \r\n                left join Hishop_AliFuWuMessageTemplates b on a.MessageType= b.MessageType\r\n                where b.MessageType is not null and isnull(b.IsValid,0)=1\r\n                    and  LOWER(DetailType) = LOWER(@DetailType)");
            this.database.AddInParameter(sqlStringCommand, "DetailType", DbType.String, DetailType);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    template = this.PopulateEmailTempletFromIDataReader(reader);
                }
                reader.Close();
            }
            return template;
        }

        public MessageTemplate GetMessageTemplate(string messageType)
        {
            MessageTemplate template = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MessageTemplates WHERE LOWER(MessageType) = LOWER(@MessageType)");
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String, messageType);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    template = this.PopulateEmailTempletFromIDataReader(reader);
                }
                reader.Close();
            }
            return template;
        }

        public MessageTemplate GetMessageTemplateByDetailType(string DetailType)
        {
            MessageTemplate template = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("  select a.DetailType, a.DetailName, a.AllowToAdmin,a.AllowToDistributor,a.AllowToMember, a.IsSelectedByDistributor,a.IsSelectedByMember,\r\n                    b.* from Hishop_MessageTemplatesDetail a \r\n                left join Hishop_MessageTemplates b on a.MessageType= b.MessageType\r\n                where b.MessageType is not null and isnull(b.IsValid,0)=1\r\n                    and  LOWER(DetailType) = LOWER(@DetailType)");
            this.database.AddInParameter(sqlStringCommand, "DetailType", DbType.String, DetailType);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    template = this.PopulateEmailTempletFromIDataReader(reader);
                }
                reader.Close();
            }
            return template;
        }

        public IList<MessageTemplate> GetMessageTemplates()
        {
            IList<MessageTemplate> list = new List<MessageTemplate>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MessageTemplates");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(this.PopulateEmailTempletFromIDataReader(reader));
                }
                reader.Close();
            }
            return list;
        }

        public string GetUserOpenIdByUserId(int UserId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select top 1 OpenId  from aspnet_Members WHERE UserId= " + UserId.ToString());
            return Convert.ToString(this.database.ExecuteScalar(sqlStringCommand));
        }

        public MessageTemplate PopulateEmailTempletFromIDataReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }
            return new MessageTemplate((string) reader["TagDescription"], (string) reader["Name"]) { MessageType = (string) reader["MessageType"], SendInnerMessage = (bool) reader["SendInnerMessage"], SendWeixin = (bool) reader["SendWeixin"], SendSMS = (bool) reader["SendSMS"], SendEmail = (bool) reader["SendEmail"], EmailSubject = (string) reader["EmailSubject"], EmailBody = (string) reader["EmailBody"], InnerMessageSubject = (string) reader["InnerMessageSubject"], InnerMessageBody = (string) reader["InnerMessageBody"], SMSBody = (string) reader["SMSBody"], WeixinTemplateId = (reader["WeixinTemplateId"] != DBNull.Value) ? ((string) reader["WeixinTemplateId"]) : "", IsSendWeixin_ToDistributor = Convert.ToString(reader["IsSelectedByDistributor"]) == "1", IsSendWeixin_ToMember = Convert.ToString(reader["IsSelectedBymember"]) == "1" };
        }

        public void UpdateSettings(IList<MessageTemplate> templates)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplates SET SendEmail = @SendEmail, SendSMS = @SendSMS, SendInnerMessage = @SendInnerMessage,SendWeixin = @SendWeixin WHERE LOWER(MessageType) = LOWER(@MessageType)");
            this.database.AddInParameter(sqlStringCommand, "SendEmail", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendSMS", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendInnerMessage", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "SendWeixin", DbType.Boolean);
            foreach (MessageTemplate template in templates)
            {
                this.database.SetParameterValue(sqlStringCommand, "SendEmail", template.SendEmail);
                this.database.SetParameterValue(sqlStringCommand, "SendSMS", template.SendSMS);
                this.database.SetParameterValue(sqlStringCommand, "SendInnerMessage", template.SendInnerMessage);
                this.database.SetParameterValue(sqlStringCommand, "MessageType", template.MessageType);
                this.database.SetParameterValue(sqlStringCommand, "SendWeixin", template.SendWeixin);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public void UpdateTemplate(MessageTemplate template)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplates SET EmailSubject = @EmailSubject, EmailBody = @EmailBody, InnerMessageSubject = @InnerMessageSubject, InnerMessageBody = @InnerMessageBody,WeixinTemplateId=@WeixinTemplateId, SMSBody = @SMSBody WHERE LOWER(MessageType) = LOWER(@MessageType)");
            this.database.AddInParameter(sqlStringCommand, "EmailSubject", DbType.String, template.EmailSubject);
            this.database.AddInParameter(sqlStringCommand, "EmailBody", DbType.String, template.EmailBody);
            this.database.AddInParameter(sqlStringCommand, "InnerMessageSubject", DbType.String, template.InnerMessageSubject);
            this.database.AddInParameter(sqlStringCommand, "InnerMessageBody", DbType.String, template.InnerMessageBody);
            this.database.AddInParameter(sqlStringCommand, "SMSBody", DbType.String, template.SMSBody);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String, template.MessageType);
            this.database.AddInParameter(sqlStringCommand, "WeixinTemplateId", DbType.String, template.WeixinTemplateId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

