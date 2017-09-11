namespace Hidistro.SqlDal.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class NoticeDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool DelNotice(int noticeid)
        {
            string query = string.Concat(new object[] { "delete from Hishop_NoticeRead where NoticeId=", noticeid, ";delete from Hishop_NoticeUser where NoticeId=", noticeid, ";delete from Hishop_Notice where ID=", noticeid });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public NoticeInfo GetNoticeInfo(int id)
        {
            NoticeInfo info = new NoticeInfo();
            string query = "select id,Title,Memo,Author,IsPub,AddTime,PubTime,SendType,SendTo from Hishop_Notice where ID=" + id;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            info.Id = id;
            info.Title = table.Rows[0]["Title"].ToString();
            info.Memo = table.Rows[0]["Memo"].ToString();
            info.Author = table.Rows[0]["Author"].ToString();
            info.IsPub = Globals.ToNum(table.Rows[0]["IsPub"]);
            info.AddTime = DateTime.Parse(table.Rows[0]["AddTime"].ToString());
            info.SendType = Globals.ToNum(table.Rows[0]["SendType"]);
            info.SendTo = Globals.ToNum(table.Rows[0]["SendTo"]);
            if (table.Rows[0]["PubTime"] != DBNull.Value)
            {
                info.PubTime = new DateTime?(DateTime.Parse(table.Rows[0]["PubTime"].ToString()));
            }
            if (info.SendTo == 2)
            {
                info.NoticeUserInfo = new NoticeUserDao().GetNoticeUserInfo(info.Id);
            }
            return info;
        }

        public int GetNoticeNotReadCount(NoticeQuery query)
        {
            string str = "Hishop_Notice";
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" SendType={0} ", query.SendType.ToString());
            builder.AppendFormat(" AND PubTime>='{0}' ", DateTime.Now.AddYears(-1));
            if (query.UserId.HasValue)
            {
                if (query.SendType == 1)
                {
                    str = "vw_Hishop_Notice";
                    builder.AppendFormat(" AND (UserID={0} or UserID=0) ", query.UserId);
                }
                str = "vw_Hishop_Notice";
                builder.Append(" AND id not in(select NoticeId from Hishop_NoticeRead where UserID=" + query.UserId + ") ");
            }
            if (!query.IsDistributor.HasValue || !query.IsDistributor.Value)
            {
                builder.Append(" AND SendTo<>1 ");
            }
            builder.AppendFormat(" AND IsPub={0} ", 1);
            if (query.IsDel.HasValue && (query.IsDel == 1))
            {
                str = "vw_Hishop_Notice";
                builder.AppendFormat(" AND id not in(select NoticeId from Hishop_NoticeRead where UserID={0} and NoticeIsDel=1)  ", query.UserId);
            }
            string str2 = "select count(0) from " + str + " where " + builder.ToString();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public DataTable GetNoticeNotReadDt(NoticeQuery query)
        {
            string str = "vw_Hishop_Notice";
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("  PubTime>='{0}' ", DateTime.Now.AddYears(-1));
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND (UserID={0} or UserID=0) ", query.UserId);
                builder.Append(" AND id not in(select NoticeId from Hishop_NoticeRead where UserID=" + query.UserId + ") ");
            }
            if (!query.IsDistributor.HasValue || (query.IsDistributor.HasValue && !query.IsDistributor.Value))
            {
                builder.Append(" AND SendTo<>1 ");
            }
            builder.AppendFormat(" AND IsPub={0} ", 1);
            builder.Append("  order by PubTime desc ");
            string str2 = "select top 5 * from " + str + " where " + builder.ToString();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            DataTable table = new DataTable();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult GetNoticeRequest(NoticeQuery query)
        {
            string table = "Hishop_Notice";
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" SendType={0} ", query.SendType.ToString());
            if (!string.IsNullOrEmpty(query.Title))
            {
                builder.AppendFormat(" AND Title LIKE '%{0}%' ", DataHelper.CleanSearchString(query.Title));
            }
            if (!string.IsNullOrEmpty(query.Author))
            {
                builder.AppendFormat(" AND Author='{0}' ", DataHelper.CleanSearchString(query.Author));
            }
            if (query.StartTime.HasValue)
            {
                builder.AppendFormat(" AND PubTime>='{0}' ", query.StartTime.Value.ToString("yyyy-MM-dd"));
            }
            if (query.EndTime.HasValue)
            {
                builder.AppendFormat(" AND PubTime<'{0}' ", query.EndTime.Value.AddDays(1.0).ToString("yyyy-MM-dd"));
            }
            if (query.UserId.HasValue)
            {
                if (query.SendType == 1)
                {
                    table = "vw_Hishop_Notice";
                    builder.AppendFormat(" AND (UserID={0} or UserID=0) ", query.UserId);
                }
                if (query.IsNotShowRead.HasValue)
                {
                    table = "vw_Hishop_Notice";
                    builder.Append(" AND id not in(select NoticeId from Hishop_NoticeRead where UserID=" + query.UserId + ") ");
                }
            }
            if (!query.IsDistributor.HasValue || !query.IsDistributor.Value)
            {
                builder.Append(" AND SendTo<>1 ");
            }
            if (query.IsPub.HasValue)
            {
                builder.AppendFormat(" AND IsPub={0} ", query.IsPub.Value);
            }
            if (query.IsDel.HasValue && (query.IsDel == 1))
            {
                table = "vw_Hishop_Notice";
                builder.AppendFormat(" AND id not in(select NoticeId from Hishop_NoticeRead where UserID=" + query.UserId + " and NoticeIsDel=1)  ", query.IsPub.Value);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "ID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public int GetSelectedUser(int noticeid)
        {
            string query = "select count(0) from Hishop_NoticeUser where NoticeId=" + noticeid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public DataSet GetSelectedUser(string adminName)
        {
            string query = "select a.UserId,b.UserName,b.CellPhone,b.UserBindName from Hishop_NoticeTempUser a left join aspnet_Members b on a.userid=b.userid where a.LoginName=@LoginName and b.Status=1";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "LoginName", DbType.String, adminName);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public bool GetUserIsSel(int userid, string adminName)
        {
            string query = "select top 1 userid from Hishop_NoticeTempUser where userid=" + userid + " and LoginName=@LoginName";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "LoginName", DbType.String, adminName);
            return (this.database.ExecuteDataSet(sqlStringCommand).Tables[0].Rows.Count > 0);
        }

        public bool IsView(int userid, int noticeid)
        {
            string query = string.Concat(new object[] { "select top 1 noticeid from Hishop_NoticeRead where NoticeId=", noticeid, " and UserId=", userid });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public bool NoticePub(int noticeid)
        {
            string query = "update Hishop_Notice set IsPub=1,PubTime=getdate() where ID=" + noticeid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public int SaveNotice(NoticeInfo info)
        {
            string query = string.Empty;
            if (info.SendType == 1)
            {
                if (!info.PubTime.HasValue)
                {
                    info.PubTime = new DateTime?(DateTime.Now);
                }
                if (info.IsPub == 0)
                {
                    info.IsPub = 1;
                }
            }
            if (info.Id > 0)
            {
                query = "Update Hishop_Notice set Title=@Title,Memo=@Memo,Author=@Author where Id=" + info.Id;
            }
            else if (info.PubTime.HasValue)
            {
                query = "INSERT INTO Hishop_Notice (Title,Memo,Author,IsPub,AddTime,PubTime,SendType,SendTo) VALUES (@Title,@Memo,@Author,@IsPub,@AddTime,@PubTime,@SendType,@SendTo);select @@identity;";
            }
            else
            {
                query = "INSERT INTO Hishop_Notice (Title,Memo,Author,IsPub,AddTime,SendType,SendTo) VALUES (@Title,@Memo,@Author,@IsPub,@AddTime,@SendType,@SendTo);select @@identity;";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            if (info.PubTime.HasValue)
            {
                this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, info.PubTime);
            }
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, info.Title);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, info.Memo);
            this.database.AddInParameter(sqlStringCommand, "Author", DbType.String, info.Author);
            this.database.AddInParameter(sqlStringCommand, "IsPub", DbType.Int32, info.IsPub);
            this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, info.AddTime);
            this.database.AddInParameter(sqlStringCommand, "SendType", DbType.Int32, info.SendType);
            this.database.AddInParameter(sqlStringCommand, "SendTo", DbType.Int32, info.SendTo);
            int id = Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
            if (info.Id > 0)
            {
                id = info.Id;
            }
            if ((id > 0) && (info.SendTo == 2))
            {
                if (info.Id > 0)
                {
                    new NoticeUserDao().DelAllUser(info.Id);
                }
                foreach (NoticeUserInfo info2 in info.NoticeUserInfo)
                {
                    query = string.Concat(new object[] { "insert into Hishop_NoticeUser(NoticeId,UserID)values(", id, ",", info2.UserId, ")" });
                    sqlStringCommand = this.database.GetSqlStringCommand(query);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                }
            }
            return id;
        }

        public void ViewNotice(int userid, int noticeid)
        {
            if (!this.IsView(userid, noticeid))
            {
                string query = string.Concat(new object[] { "insert into Hishop_NoticeRead(NoticeId,UserId)values(", noticeid, ",", userid, ")" });
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }
    }
}

