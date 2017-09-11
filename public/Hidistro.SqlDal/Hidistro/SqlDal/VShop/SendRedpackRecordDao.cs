namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class SendRedpackRecordDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddSendRedpackRecord(SendRedpackRecordInfo sendredpackinfo, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into vshop_SendRedpackRecord(BalanceDrawRequestID,UserID,OpenID,Amount,ActName,Wishing,ClientIP,IsSend,SendTime)values(@BalanceDrawRequestID,@UserID,@OpenID,@Amount,@ActName,@Wishing,@ClientIP,@IsSend,@SendTime)");
            this.database.AddInParameter(sqlStringCommand, "BalanceDrawRequestID", DbType.Int32, sendredpackinfo.BalanceDrawRequestID);
            this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, sendredpackinfo.UserID);
            this.database.AddInParameter(sqlStringCommand, "OpenID", DbType.String, sendredpackinfo.OpenID);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Int32, sendredpackinfo.Amount);
            this.database.AddInParameter(sqlStringCommand, "ActName", DbType.String, sendredpackinfo.ActName);
            this.database.AddInParameter(sqlStringCommand, "Wishing", DbType.String, sendredpackinfo.Wishing);
            this.database.AddInParameter(sqlStringCommand, "ClientIP", DbType.String, sendredpackinfo.ClientIP);
            this.database.AddInParameter(sqlStringCommand, "IsSend", DbType.Int32, 0);
            this.database.AddInParameter(sqlStringCommand, "SendTime", DbType.DateTime, DBNull.Value);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DataTable GetNotSendRedpackRecord(int balancedrawrequestid)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from vshop_SendRedpackRecord where BalanceDrawRequestID=@BalanceDrawRequestID and IsSend=0");
            this.database.AddInParameter(sqlStringCommand, "BalanceDrawRequestID", DbType.Int32, balancedrawrequestid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public int GetRedPackTotalAmount(int balancedrawrequestid, int userid)
        {
            string query = "select isnull(sum(Amount),0) from vshop_SendRedpackRecord where IsSend=1";
            if (balancedrawrequestid > 0)
            {
                query = query + " and BalanceDrawRequestID=" + balancedrawrequestid;
            }
            else if (userid > 0)
            {
                query = query + " and UserID=" + userid;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public SendRedpackRecordInfo GetSendRedpackRecordByID(string id = null, string sid = null)
        {
            if ((id == null) && (sid == null))
            {
                return null;
            }
            SendRedpackRecordInfo info = null;
            string query = "";
            int result = 0;
            if (id != null)
            {
                if (!int.TryParse(id, out result))
                {
                    return null;
                }
                query = string.Format("select * FROM vshop_SendRedpackRecord WHERE ID={0}", id);
            }
            else
            {
                if (!int.TryParse(sid, out result))
                {
                    return null;
                }
                query = string.Format("select * FROM vshop_SendRedpackRecord WHERE BalanceDrawRequestID={0}", sid);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateSendRedpackRecordInfo(reader);
                }
            }
            return info;
        }

        public DbQueryResult GetSendRedpackRecordList(SendRedpackRecordQuery sendredpackrecordquery)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" UserId={0} and BalanceDrawRequestID={1} ", sendredpackrecordquery.UserID);
            return DataHelper.PagingByRownumber(sendredpackrecordquery.PageIndex, sendredpackrecordquery.PageSize, sendredpackrecordquery.SortBy, sendredpackrecordquery.SortOrder, sendredpackrecordquery.IsCount, "vshop_SendRedpackRecord", "ID", builder.ToString(), "*");
        }

        public DbQueryResult GetSendRedpackRecordRequest(SendRedpackRecordQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.BalanceDrawRequestID > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" BalanceDrawRequestID={0} ", query.BalanceDrawRequestID);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vshop_SendRedpackRecord ", "ID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public bool HasDrawRequest(int serialid)
        {
            bool flag = false;
            string query = "select top 1 ID from vshop_SendRedpackRecord where BalanceDrawRequestID=" + serialid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                flag = true;
            }
            reader.Close();
            return flag;
        }

        public bool SetRedpackRecordIsUsed(int id, bool issend)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_SendRedpackRecord set IsSend=@IsSend,SendTime=getdate() where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, id);
            this.database.AddInParameter(sqlStringCommand, "IsSend", DbType.Boolean, issend);
            bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            if (flag)
            {
                SendRedpackRecordInfo sendRedpackRecordByID = this.GetSendRedpackRecordByID(id.ToString(), null);
                if (sendRedpackRecordByID != null)
                {
                    string query = string.Format("update Hishop_BalanceDrawRequest set IsCheck=2,CheckTime=getdate(), Remark='红包提现记录' where SerialID={0} and IsCheck=1 and not exists(select id from vshop_SendRedpackRecord a where a.IsSend=0 and a.BalanceDrawRequestID={0}) and exists(select id from vshop_SendRedpackRecord a where a.IsSend=1 and a.BalanceDrawRequestID={0})", sendRedpackRecordByID.BalanceDrawRequestID);
                    sqlStringCommand = this.database.GetSqlStringCommand(query);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                }
            }
            return flag;
        }
    }
}

