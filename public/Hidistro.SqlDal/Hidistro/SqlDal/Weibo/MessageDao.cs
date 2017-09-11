namespace Hidistro.SqlDal.Weibo
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Weibo;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class MessageDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public MessageInfo GetMessageInfo(int MessageId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Message WHERE MessageId = @MessageId");
            this.database.AddInParameter(sqlStringCommand, "MessageId", DbType.Int32, MessageId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MessageInfo>(reader);
            }
        }

        public DbQueryResult GetMessages(MessageQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.Status > -1)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("Status = {0}", query.Status);
            }
            query.SortBy = "Created_at";
            if (builder.Length > 0)
            {
                builder.Append(" AND ");
            }
            builder.AppendFormat("Access_Token = '{0}'", SettingsManager.GetMasterSettings(false).Access_Token);
            query.SortOrder = SortAction.Desc;
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Weibo_Message", "MessageId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public int SaveMessage(MessageInfo messageInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Weibo_Message ([Type],[Receiver_id],[Sender_id],[Created_at],[Text],[Vfid],[Tovfid],[Status],[Access_Token]) VALUES(@Type,@Receiver_id,@Sender_id,@Created_at,@Text,@Vfid,@Tovfid,@Status,@Access_Token);SELECT @@Identity");
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, messageInfo.Type);
            this.database.AddInParameter(sqlStringCommand, "Receiver_id", DbType.String, messageInfo.Receiver_id);
            this.database.AddInParameter(sqlStringCommand, "Sender_id", DbType.String, messageInfo.Sender_id);
            this.database.AddInParameter(sqlStringCommand, "Created_at", DbType.DateTime, messageInfo.Created_at);
            this.database.AddInParameter(sqlStringCommand, "Text", DbType.String, messageInfo.Text);
            this.database.AddInParameter(sqlStringCommand, "Vfid", DbType.String, messageInfo.Vfid);
            this.database.AddInParameter(sqlStringCommand, "Tovfid", DbType.String, messageInfo.Tovfid);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, messageInfo.Status);
            this.database.AddInParameter(sqlStringCommand, "Access_Token", DbType.String, messageInfo.Access_Token);
            int num = 0;
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                num = Convert.ToInt32(obj2.ToString());
            }
            return num;
        }

        public bool UpdateMessage(MessageInfo messageInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Weibo_Message set SenderMessage=@SenderMessage,SenderDate=@SenderDate,Display_Name=@Display_Name,Summary=@Summary,Image=@Image,Url=@Url,Status=@Status,ArticleId=@ArticleId where MessageId=@MessageId");
            this.database.AddInParameter(sqlStringCommand, "SenderMessage", DbType.String, messageInfo.SenderMessage);
            this.database.AddInParameter(sqlStringCommand, "Display_Name", DbType.String, messageInfo.DisplayName);
            this.database.AddInParameter(sqlStringCommand, "Summary", DbType.String, messageInfo.Summary);
            this.database.AddInParameter(sqlStringCommand, "SenderDate", DbType.DateTime, messageInfo.SenderDate);
            this.database.AddInParameter(sqlStringCommand, "Image", DbType.String, messageInfo.Image);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, messageInfo.Url);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, messageInfo.Status);
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, messageInfo.ArticleId);
            this.database.AddInParameter(sqlStringCommand, "MessageId", DbType.Int32, messageInfo.MessageId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }
    }
}

