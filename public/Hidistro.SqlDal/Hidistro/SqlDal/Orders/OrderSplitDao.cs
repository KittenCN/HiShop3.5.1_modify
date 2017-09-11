namespace Hidistro.SqlDal.Orders
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class OrderSplitDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool DelOrderSplitByOrderID(string oldorderid, DbTransaction dbTran = null)
        {
            string query = "delete from vshop_OrderSplit where OldOrderID=@OldOrderID";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OldOrderID", DbType.String, oldorderid);
            if (dbTran == null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
        }

        public bool DelOrderSplitInfo(int id)
        {
            string query = "delete  from vshop_OrderSplit where ID=" + id;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int GetMaxOrderIDNum(string orderid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select isnull(Max(OrderIDNum),0) from vshop_OrderSplit ");
            builder.Append(" where OldOrderID=@OldOrderID ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OldOrderID", DbType.String, orderid);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public OrderSplitInfo GetOrderSplitInfo(int id)
        {
            string query = "select * from vshop_OrderSplit where ID=" + id;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<OrderSplitInfo>(reader);
            }
        }

        public OrderSplitInfo GetOrderSplitInfoByOrderIDAndNum(int orderidnum, string oldorderid)
        {
            string query = "select * from vshop_OrderSplit where OldOrderID=@OldOrderID and OrderIDNum=@OrderIDNum";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderIDNum", DbType.Int32, orderidnum);
            this.database.AddInParameter(sqlStringCommand, "OldOrderID", DbType.String, oldorderid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<OrderSplitInfo>(reader);
            }
        }

        public IList<OrderSplitInfo> GetOrderSplitItems(string orderid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select Id,OrderIDNum,OldOrderID,ItemList,UpdateTime,AdjustedFreight from vshop_OrderSplit ");
            builder.Append(" where OldOrderID=@OldOrderID order by ID asc ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OldOrderID", DbType.String, orderid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<OrderSplitInfo>(reader);
            }
        }

        public int NewOrderSplit(OrderSplitInfo info)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("insert into vshop_OrderSplit(OrderIDNum,OldOrderID,ItemList,UpdateTime,AdjustedFreight) ");
            builder.Append(" values(@OrderIDNum,@OldOrderID,@ItemList,@UpdateTime,@AdjustedFreight);select @@identity ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderIDNum", DbType.Int32, info.OrderIDNum);
            this.database.AddInParameter(sqlStringCommand, "OldOrderID", DbType.String, info.OldOrderId);
            this.database.AddInParameter(sqlStringCommand, "ItemList", DbType.String, info.ItemList);
            this.database.AddInParameter(sqlStringCommand, "UpdateTime", DbType.DateTime, info.UpdateTime);
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Decimal, info.AdjustedFreight);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public bool UpdateOrderSplitFright(OrderSplitInfo info)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE vshop_OrderSplit SET ").Append("AdjustedFreight=@AdjustedFreight ").Append(" WHERE ID=@ID");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Decimal, info.AdjustedFreight);
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, info.Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateOrderSplitInfo(OrderSplitInfo info)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE vshop_OrderSplit SET ").Append("OrderIDNum=@OrderIDNum,").Append("ItemList=@ItemList,").Append("UpdateTime=@UpdateTime").Append(" WHERE ID=@ID");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderIDNum", DbType.Int32, info.OrderIDNum);
            this.database.AddInParameter(sqlStringCommand, "ItemList", DbType.String, info.ItemList);
            this.database.AddInParameter(sqlStringCommand, "UpdateTime", DbType.DateTime, info.UpdateTime);
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, info.Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

