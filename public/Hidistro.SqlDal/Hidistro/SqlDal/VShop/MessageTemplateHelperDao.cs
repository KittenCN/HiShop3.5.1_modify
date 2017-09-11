namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;

    public class MessageTemplateHelperDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool DeleteAdminUserMsgList(MsgList myList, out string RetInfo)
        {
            RetInfo = "";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" delete from Hishop_MessageAdminUserMsgList  where UserOpenId='" + myList.UserOpenId + "' ");
            if (this.database.ExecuteNonQuery(sqlStringCommand) == 0)
            {
                RetInfo = "此OpenId不存在，无法删除。";
                return false;
            }
            RetInfo = "删除成功！";
            return true;
        }

        public DataTable GetAdminUserMsgDetail(bool IsDistributor)
        {
            DbCommand sqlStringCommand;
            if (IsDistributor)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("select *,  isnull(IsSelectedByDistributor,0)  as  IsSelected    from Hishop_MessageTemplatesDetail\r\n                    where AllowToDistributor=1\r\n                ");
            }
            else
            {
                sqlStringCommand = this.database.GetSqlStringCommand("select *,  isnull(IsSelectedByMember,0)  as  IsSelected    from Hishop_MessageTemplatesDetail\r\n                    where AllowToMember=1\r\n                ");
            }
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetAdminUserMsgList(int userType = 0)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" select   *,\r\n                    case Msg1 when 1 then '新订单' else '' end as MsgDesc1,\r\n                    case Msg2 when 1 then '订单付款' else '' end as MsgDesc2,\r\n                    case Msg3 when 1 then '退款申请' else '' end as MsgDesc3,\r\n                    case Msg4 when 1 then '用户咨询' else '' end as MsgDesc4,\r\n                    case Msg5 when 1 then '提现申请' else '' end as MsgDesc5,\r\n                    case Msg6 when 1 then '分销商申请成功' else '' end as MsgDesc6\r\n                    from Hishop_MessageAdminUserMsgList where type=" + userType);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public IList<MessageTemplate> GetAliFuWuMessageTemplates()
        {
            IList<MessageTemplate> list = new List<MessageTemplate>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AliFuWuMessageTemplates  where IsValid=1 ORDER BY OrderIndex");
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

        public IList<MessageTemplate> GetMessageTemplates()
        {
            IList<MessageTemplate> list = new List<MessageTemplate>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MessageTemplates  where IsValid=1 ORDER BY OrderIndex");
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

        public MessageTemplate PopulateEmailTempletFromIDataReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }
            return new MessageTemplate((string) reader["TagDescription"], (string) reader["Name"]) { MessageType = (string) reader["MessageType"], SendInnerMessage = (bool) reader["SendInnerMessage"], SendWeixin = (bool) reader["SendWeixin"], SendSMS = (bool) reader["SendSMS"], SendEmail = (bool) reader["SendEmail"], EmailSubject = (string) reader["EmailSubject"], EmailBody = (string) reader["EmailBody"], InnerMessageSubject = (string) reader["InnerMessageSubject"], InnerMessageBody = (string) reader["InnerMessageBody"], SMSBody = (string) reader["SMSBody"], WeixinTemplateId = (reader["WeixinTemplateId"] != DBNull.Value) ? ((string) reader["WeixinTemplateId"]) : "", WXOpenTM = (string) reader["WXOpenTM"] };
        }

        public bool SaveAdminUserMsgList(bool IsInsert, MsgList myList, string OldUserOpenIdIfUpdate, out string RetInfo)
        {
            DbCommand sqlStringCommand;
            int num = 0;
            RetInfo = "";
            if (IsInsert)
            {
                sqlStringCommand = this.database.GetSqlStringCommand(" select count(*) as SumRec from Hishop_MessageAdminUserMsgList where UserOpenId='" + myList.UserOpenId + "' ");
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand).ToString()) > 0)
                {
                    RetInfo = "此OpenId已存在。";
                    return false;
                }
                sqlStringCommand = this.database.GetSqlStringCommand(" insert into Hishop_MessageAdminUserMsgList(UserOpenId, RealName,RoleName, Msg1,Msg2,Msg3,Msg4,Msg5,Msg6,Type )\r\n                        values (@UserOpenId, @RealName,@RoleName, @Msg1,@Msg2,@Msg3,@Msg4,@Msg5,@Msg6,@Type)\r\n                ");
                this.database.AddInParameter(sqlStringCommand, "UserOpenId", DbType.String, myList.UserOpenId);
            }
            else
            {
                sqlStringCommand = this.database.GetSqlStringCommand(" select count(*) as SumRec from Hishop_MessageAdminUserMsgList where UserOpenId='" + OldUserOpenIdIfUpdate + "' ");
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand).ToString()) == 0)
                {
                    RetInfo = "此OpenId不存在，无法更新。";
                    return false;
                }
                sqlStringCommand = this.database.GetSqlStringCommand(" update  Hishop_MessageAdminUserMsgList set  RealName=@RealName,\r\n                        RoleName=@RoleName, Msg1=@Msg1,Msg2=@Msg2,Msg3=@Msg3,Msg4=@Msg4,Msg5=@Msg5,Msg6=@Msg6   \r\n                        where UserOpenId=@OldUserOpenId\r\n                ");
                this.database.AddInParameter(sqlStringCommand, "OldUserOpenId", DbType.String, OldUserOpenIdIfUpdate);
            }
            this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, myList.RealName);
            this.database.AddInParameter(sqlStringCommand, "RoleName", DbType.String, myList.RoleName);
            this.database.AddInParameter(sqlStringCommand, "Msg1", DbType.Int32, myList.Msg1);
            this.database.AddInParameter(sqlStringCommand, "Msg2", DbType.Int32, myList.Msg2);
            this.database.AddInParameter(sqlStringCommand, "Msg3", DbType.Int32, myList.Msg3);
            this.database.AddInParameter(sqlStringCommand, "Msg4", DbType.Int32, myList.Msg4);
            this.database.AddInParameter(sqlStringCommand, "Msg5", DbType.Int32, myList.Msg5);
            this.database.AddInParameter(sqlStringCommand, "Msg6", DbType.Int32, myList.Msg6);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, myList.Type);
            num = this.database.ExecuteNonQuery(sqlStringCommand);
            if (num == 0)
            {
                RetInfo = "保存失败。";
            }
            RetInfo = "保存成功！";
            return (num > 0);
        }

        public void UpdateAliFuWuSettings(IList<MessageTemplate> templates)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_AliFuWuMessageTemplates SET SendEmail = @SendEmail, SendSMS = @SendSMS, SendInnerMessage = @SendInnerMessage,SendWeixin = @SendWeixin , WeixinTemplateId=@WeixinTemplateId  WHERE LOWER(MessageType) = LOWER(@MessageType)");
            this.database.AddInParameter(sqlStringCommand, "SendEmail", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendSMS", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendInnerMessage", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "WeixinTemplateId", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "SendWeixin", DbType.Boolean);
            foreach (MessageTemplate template in templates)
            {
                this.database.SetParameterValue(sqlStringCommand, "SendEmail", template.SendEmail);
                this.database.SetParameterValue(sqlStringCommand, "SendSMS", template.SendSMS);
                this.database.SetParameterValue(sqlStringCommand, "SendInnerMessage", template.SendInnerMessage);
                this.database.SetParameterValue(sqlStringCommand, "MessageType", template.MessageType);
                this.database.SetParameterValue(sqlStringCommand, "WeixinTemplateId", template.WeixinTemplateId);
                this.database.SetParameterValue(sqlStringCommand, "SendWeixin", template.SendWeixin);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public void UpdateSettings(IList<MessageTemplate> templates)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplates SET SendEmail = @SendEmail, SendSMS = @SendSMS, SendInnerMessage = @SendInnerMessage,SendWeixin = @SendWeixin , WeixinTemplateId=@WeixinTemplateId  WHERE LOWER(MessageType) = LOWER(@MessageType)");
            this.database.AddInParameter(sqlStringCommand, "SendEmail", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendSMS", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendInnerMessage", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "WeixinTemplateId", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "SendWeixin", DbType.Boolean);
            foreach (MessageTemplate template in templates)
            {
                this.database.SetParameterValue(sqlStringCommand, "SendEmail", template.SendEmail);
                this.database.SetParameterValue(sqlStringCommand, "SendSMS", template.SendSMS);
                this.database.SetParameterValue(sqlStringCommand, "SendInnerMessage", template.SendInnerMessage);
                this.database.SetParameterValue(sqlStringCommand, "MessageType", template.MessageType);
                this.database.SetParameterValue(sqlStringCommand, "WeixinTemplateId", template.WeixinTemplateId);
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

        public void UpdateWeiXinMsgDetail(bool IsDistributor, IList<MsgDetail> templates)
        {
            DbCommand sqlStringCommand;
            if (IsDistributor)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplatesDetail SET IsSelectedByDistributor = @IsSelectedByDistributor   WHERE LOWER(DetailType) = LOWER(@DetailType)");
                this.database.AddInParameter(sqlStringCommand, "IsSelectedByDistributor", DbType.Int32);
                this.database.AddInParameter(sqlStringCommand, "DetailType", DbType.String);
                foreach (MsgDetail detail in templates)
                {
                    this.database.SetParameterValue(sqlStringCommand, "IsSelectedByDistributor", detail.IsSelectedByDistributor);
                    this.database.SetParameterValue(sqlStringCommand, "DetailType", detail.DetailType);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                }
            }
            else
            {
                sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplatesDetail SET IsSelectedByMember = @IsSelectedByMember  WHERE LOWER(DetailType) = LOWER(@DetailType)");
                this.database.AddInParameter(sqlStringCommand, "IsSelectedByMember", DbType.Int32);
                this.database.AddInParameter(sqlStringCommand, "DetailType", DbType.String);
                foreach (MsgDetail detail2 in templates)
                {
                    this.database.SetParameterValue(sqlStringCommand, "IsSelectedByMember", detail2.IsSelectedByMember);
                    this.database.SetParameterValue(sqlStringCommand, "DetailType", detail2.DetailType);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                }
            }
        }
    }
}

