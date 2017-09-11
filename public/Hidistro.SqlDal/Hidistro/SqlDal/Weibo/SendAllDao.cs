namespace Hidistro.SqlDal.Weibo
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.Entities.WeiXin;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class SendAllDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public string ClearWeiXinMediaID()
        {
            string query = "update vshop_Article set mediaid=null where len(mediaid)>0;update vshop_ArticleItems set mediaid=null where len(mediaid)>0";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteNonQuery(sqlStringCommand).ToString();
        }

        public bool DeleteOldQRCode(string AppID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Delete from vshop_ScanOpenID where 1=1 or AppID=@AppID");
            this.database.AddInParameter(sqlStringCommand, "AppID", DbType.String, AppID);
            return (Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public bool DelOldSendAllList()
        {
            string query = "update WeiXin_SendAll set SendState=2 where SendState=0 and DATEADD(HOUR,2, SendTime)<getdate()";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int getAlypayUserNum()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(UserId) as n FROM aspnet_Members where Len(AlipayUserId)>15");
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public bool GetQRCodeScanInfo(string AppID, bool IsClearAfterRead, out string SCannerUserOpenID, out string SCannerUserNickName, out string UserHead)
        {
            bool flag = false;
            SCannerUserOpenID = "";
            SCannerUserNickName = "";
            UserHead = "";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" select a.SCannerUserOpenID, b.UserName as NickName, b.UserHead    from  vshop_ScanOpenID  a  left join aspnet_Members b  on a.SCannerUserOpenID= b.OpenId   where 1=1 or a.AppID= @AppID  order by AutoID desc ");
            this.database.AddInParameter(sqlStringCommand, "AppID", DbType.String, AppID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    flag = true;
                    object obj2 = reader["SCannerUserOpenID"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        SCannerUserOpenID = (string) obj2;
                        flag = true;
                    }
                    obj2 = reader["NickName"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        SCannerUserNickName = (string) obj2;
                    }
                    obj2 = reader["UserHead"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        UserHead = (string) obj2;
                    }
                }
            }
            if (flag && IsClearAfterRead)
            {
                DbCommand command = this.database.GetSqlStringCommand(" delete  from  vshop_ScanOpenID  where 1=1 or  AppID= @AppID    ");
                this.database.AddInParameter(command, "AppID", DbType.String, AppID);
                this.database.ExecuteNonQuery(command);
            }
            return flag;
        }

        public DataTable GetRencentAliOpenID()
        {
            string query = "select  AliOpenID from vshop_AlipayActiveOpendId where DATEADD(day,2, PubTime)>getdate() order by PubTime desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetRencentOpenID(int topnum)
        {
            int num = topnum;
            if (num < 1)
            {
                num = 1;
            }
            string query = "select top " + num + " OpenID from WeiXin_RecentOpenID where DATEADD(day,2, PubTime)>getdate() order by PubTime desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public SendAllInfo GetSendAllInfo(int sendID)
        {
            SendAllInfo info = null;
            if (sendID > 0)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM WeiXin_SendAll WHERE ID = @ID");
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, sendID);
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    info = new SendAllInfo();
                    if (!reader.Read())
                    {
                        return info;
                    }
                    info.Id = sendID;
                    info.Title = reader["Title"].ToString();
                    object obj2 = reader["MessageType"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info.MessageType = (MessageType) obj2;
                    }
                    obj2 = reader["ArticleID"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info.ArticleID = (int) obj2;
                    }
                    info.Content = reader["Content"].ToString();
                    obj2 = reader["SendState"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info.SendState = (int) obj2;
                    }
                    obj2 = reader["SendTime"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info.SendTime = (DateTime) obj2;
                    }
                    obj2 = reader["SendCount"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info.SendCount = (int) obj2;
                    }
                    obj2 = reader["msgid"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info.MsgID = obj2.ToString();
                    }
                }
            }
            return info;
        }

        public DbQueryResult GetSendAllRequest(SendAllQuery query, int Platform = 0)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.Title))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" Title LIKE '%{0}%' ", DataHelper.CleanSearchString(query.Title));
            }
            if (Platform > -1)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" platform={0} ", Platform);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "WeiXin_SendAll ", "ID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public bool SaveQRCodeScanInfo(string AppID, string SCannerUserOpenID, string SCannerUserNickName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" delete  from  vshop_ScanOpenID  where 1=1 or AppID= @AppID    ");
            this.database.AddInParameter(sqlStringCommand, "AppID", DbType.String, AppID);
            this.database.ExecuteNonQuery(sqlStringCommand);
            DbCommand command = this.database.GetSqlStringCommand("insert into vshop_ScanOpenID(AppID,SCannerUserOpenID,SCannerUserNickName,ScanDate) values ( @AppID, @SCannerUserOpenID, @SCannerUserNickName , getdate() ) ");
            this.database.AddInParameter(command, "AppID", DbType.String, AppID);
            this.database.AddInParameter(command, "SCannerUserOpenID", DbType.String, SCannerUserOpenID);
            this.database.AddInParameter(command, "SCannerUserNickName", DbType.String, SCannerUserNickName);
            return (Globals.ToNum(this.database.ExecuteScalar(command)) > 0);
        }

        public string SaveSendAllInfo(SendAllInfo sendAllInfo, int platform = 0)
        {
            string str = string.Empty;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO WeiXin_SendAll (Title,MessageType,ArticleID,Content,SendState,SendTime,SendCount,platform) VALUES(@Title,@MessageType,@ArticleID,@Content,@SendState,@SendTime,@SendCount,@platform);select @@identity");
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, sendAllInfo.Title);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int) sendAllInfo.MessageType);
            this.database.AddInParameter(sqlStringCommand, "ArticleID", DbType.Int32, sendAllInfo.ArticleID);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, sendAllInfo.Content);
            this.database.AddInParameter(sqlStringCommand, "SendState", DbType.Int32, sendAllInfo.SendState);
            this.database.AddInParameter(sqlStringCommand, "SendTime", DbType.DateTime, DateTime.Now.ToString());
            this.database.AddInParameter(sqlStringCommand, "SendCount", DbType.Int32, sendAllInfo.SendCount);
            this.database.AddInParameter(sqlStringCommand, "platform", DbType.Int32, platform);
            int num = Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
            if (num > 0)
            {
                str = num.ToString();
            }
            return str;
        }

        public bool UpdateAddSendCount(int id, int addcount, int SendState = -1)
        {
            StringBuilder builder = new StringBuilder();
            if (id > 0)
            {
                builder.Append("UPDATE WeiXin_SendAll SET sendcount=sendcount+@addcount ");
                if (SendState >= 0)
                {
                    builder.Append(",sendstate=@sendstate ");
                }
                builder.Append(" WHERE ID=@ID");
            }
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, id);
                this.database.AddInParameter(sqlStringCommand, "sendstate", DbType.Int32, SendState);
                this.database.AddInParameter(sqlStringCommand, "addcount", DbType.Int32, addcount);
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return false;
        }

        public bool UpdateMsgId(int id, string msgid, int sendstate, int sendcount, int totalcount, string returnjsondata)
        {
            StringBuilder builder = new StringBuilder();
            if (id > 0)
            {
                builder.Append("UPDATE WeiXin_SendAll SET msgid=@msgid,sendstate=@sendstate,sendcount=@sendcount,totalcount=@totalcount,returnjsondata=@returnjsondata WHERE ID=@ID");
            }
            else if (msgid.Length > 0)
            {
                builder.Append("UPDATE WeiXin_SendAll SET sendstate=@sendstate,sendcount=@sendcount,totalcount=@totalcount,returnjsondata=@returnjsondata WHERE msgid=@msgid and sendcount=0");
            }
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, id);
                this.database.AddInParameter(sqlStringCommand, "msgid", DbType.String, msgid);
                this.database.AddInParameter(sqlStringCommand, "sendstate", DbType.Int32, sendstate);
                this.database.AddInParameter(sqlStringCommand, "sendcount", DbType.Int32, sendcount);
                this.database.AddInParameter(sqlStringCommand, "totalcount", DbType.Int32, totalcount);
                this.database.AddInParameter(sqlStringCommand, "returnjsondata", DbType.String, returnjsondata);
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return false;
        }

        public int UpdateRencentAliOpenID(string openid)
        {
            string query = "delete from vshop_AlipayActiveOpendId where AliOpenID=@OpenID;insert into vshop_AlipayActiveOpendId(AliOpenID)values(@OpenID)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OpenID", DbType.String, openid);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public int UpdateRencentOpenID(string openid)
        {
            string query = "delete from WeiXin_RecentOpenID where OpenID=@OpenID;insert into WeiXin_RecentOpenID(OpenID)values(@OpenID)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OpenID", DbType.String, openid);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

