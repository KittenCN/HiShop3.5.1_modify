namespace Hidistro.SqlDal.Weibo
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Weibo;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class ReplyDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool DeleteReplyInfo(int ReplyInfoid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE Weibo_Reply WHERE id = @id");
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, ReplyInfoid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteReplyKeyInfo(int ReplyKeyInfoid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE Weibo_ReplyKeys WHERE id = @id");
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, ReplyKeyInfoid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private int GetAllWeibo_ReplyKeysCount()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from Weibo_ReplyKeys");
            return (1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)));
        }

        public DataTable GetReplyAll(int type)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  from vw_Hishop_ReplyKeysReply where type=" + type);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public IList<ReplyInfo> GetReplyInfo(int ReplyKeyId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Reply WHERE ReplyKeyId = @ReplyKeyId");
            this.database.AddInParameter(sqlStringCommand, "ReplyKeyId", DbType.Int32, ReplyKeyId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<ReplyInfo>(reader);
            }
        }

        public ReplyInfo GetReplyInfoMes(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Reply WHERE id = @id");
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ReplyInfo>(reader);
            }
        }

        public IList<ReplyInfo> GetReplyTypeInfo(int Type)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Reply WHERE Type = @Type");
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, Type);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<ReplyInfo>(reader);
            }
        }

        public IList<ReplyKeyInfo> GetTopReplyInfos(int Type)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_ReplyKeys  where Type=" + Type + "   ORDER BY id ASC");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<ReplyKeyInfo>(reader);
            }
        }

        public DataTable GetWeibo_Reply(int type)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  from Weibo_Reply where type=" + type);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public bool SaveReplyInfo(ReplyInfo replyInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Weibo_Reply (ReplyKeyId,IsDisable,EditDate,Content,Type,ReceiverType,Displayname,Summary,Image,Url,ArticleId) VALUES(@ReplyKeyId,@IsDisable,@EditDate,@Content,@Type,@ReceiverType,@Displayname,@Summary,@Image,@Url,@ArticleId)");
            this.database.AddInParameter(sqlStringCommand, "ReplyKeyId", DbType.String, replyInfo.ReplyKeyId);
            this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, replyInfo.IsDisable);
            this.database.AddInParameter(sqlStringCommand, "EditDate", DbType.DateTime, DateTime.Now.ToString());
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, replyInfo.Content);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, replyInfo.Type);
            this.database.AddInParameter(sqlStringCommand, "ReceiverType", DbType.String, replyInfo.ReceiverType);
            this.database.AddInParameter(sqlStringCommand, "Displayname", DbType.String, replyInfo.Displayname);
            this.database.AddInParameter(sqlStringCommand, "Summary", DbType.String, replyInfo.Summary);
            this.database.AddInParameter(sqlStringCommand, "Image", DbType.String, replyInfo.Image);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, replyInfo.Url);
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, replyInfo.ArticleId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool SaveReplyKeyInfo(ReplyKeyInfo replyKeyInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Weibo_ReplyKeys (Keys,Type,Matching) VALUES(@Keys, @Type,0)");
            this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, replyKeyInfo.Keys);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, replyKeyInfo.Type);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool UpdateMatching(ReplyKeyInfo replyKeyInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Weibo_ReplyKeys SET Matching = @Matching  WHERE id = @id");
            this.database.AddInParameter(sqlStringCommand, "Matching", DbType.Int32, replyKeyInfo.Matching);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, replyKeyInfo.Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool UpdateReplyInfo(ReplyInfo replyInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Weibo_Reply SET EditDate=@EditDate,Content=@Content,Type=@Type,ReceiverType=@ReceiverType,Displayname=@Displayname,Summary=@Summary,Image=@Image,Url=@Url,ArticleId=@ArticleId  WHERE id = @id");
            this.database.AddInParameter(sqlStringCommand, "EditDate", DbType.DateTime, DateTime.Now.ToString());
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, replyInfo.Content);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, replyInfo.Type);
            this.database.AddInParameter(sqlStringCommand, "ReceiverType", DbType.String, replyInfo.ReceiverType);
            this.database.AddInParameter(sqlStringCommand, "Displayname", DbType.String, replyInfo.Displayname);
            this.database.AddInParameter(sqlStringCommand, "Summary", DbType.String, replyInfo.Summary);
            this.database.AddInParameter(sqlStringCommand, "Image", DbType.String, replyInfo.Image);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, replyInfo.Url);
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, replyInfo.ArticleId);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, replyInfo.Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool UpdateReplyKeyInfo(ReplyKeyInfo replyKeyInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Weibo_ReplyKeys SET Keys = @Keys  WHERE id = @id");
            this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, replyKeyInfo.Keys);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, replyKeyInfo.Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }
    }
}

