namespace Hidistro.SqlDal.Orders
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Orders;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class DebitNoteDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool DelDebitNote(string noteId)
        {
            string query = string.Format("DELETE FROM Hishop_OrderDebitNote WHERE NoteId='{0}'", noteId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DbQueryResult GetAllDebitNote(DebitNoteQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderDebitNote", "NoteId", builder.ToString(), "*");
        }

        public bool SaveDebitNote(DebitNoteInfo note)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" insert into Hishop_OrderDebitNote(NoteId,OrderId,Operator,Remark) values(@NoteId,@OrderId,@Operator,@Remark)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "NoteId", DbType.String, note.NoteId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, note.OrderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, note.Operator);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, note.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

