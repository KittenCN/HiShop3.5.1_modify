namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;

    public class OrderRedPagerDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool CreateOrderRedPager(OrderRedPagerInfo orderredpager)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO vshop_OrderRedPager(OrderID,RedPagerActivityId,RedPagerActivityName,MaxGetTimes,AlreadyGetTimes,ItemAmountLimit,OrderAmountCanUse,ExpiryDays,UserID) VALUES(@OrderID,@RedPagerActivityId,@RedPagerActivityName,@MaxGetTimes,@AlreadyGetTimes,@ItemAmountLimit,@OrderAmountCanUse,@ExpiryDays,@UserID);select @@identity");
            this.database.AddInParameter(sqlStringCommand, "OrderID", DbType.String, orderredpager.OrderID);
            this.database.AddInParameter(sqlStringCommand, "RedPagerActivityId", DbType.Int32, orderredpager.RedPagerActivityId);
            this.database.AddInParameter(sqlStringCommand, "RedPagerActivityName", DbType.String, orderredpager.RedPagerActivityName);
            this.database.AddInParameter(sqlStringCommand, "MaxGetTimes", DbType.Int32, orderredpager.MaxGetTimes);
            this.database.AddInParameter(sqlStringCommand, "AlreadyGetTimes", DbType.Int32, orderredpager.AlreadyGetTimes);
            this.database.AddInParameter(sqlStringCommand, "ItemAmountLimit", DbType.Decimal, orderredpager.ItemAmountLimit);
            this.database.AddInParameter(sqlStringCommand, "OrderAmountCanUse", DbType.Decimal, orderredpager.OrderAmountCanUse);
            this.database.AddInParameter(sqlStringCommand, "ExpiryDays", DbType.Int32, orderredpager.ExpiryDays);
            this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, orderredpager.UserID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CreateOrderRedPager(string orderid, decimal ordertotalprice, int userid)
        {
            string query = "select top 1 ID,CouponId,CouponNumber,CouponName,ActivityName,ImgUrl,ShareTitle,Description from Hishop_ShareActivity where MeetValue<=@MeetValue and BeginDate<=GETDATE() and EndDate>=GETDATE() order by MeetValue desc ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "MeetValue", DbType.Decimal, ordertotalprice);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if ((!string.IsNullOrEmpty(orderid) && (userid > 0)) && (table.Rows.Count > 0))
            {
                int couponId = Globals.ToNum(table.Rows[0]["CouponId"]);
                if (couponId > 0)
                {
                    CouponInfo couponDetails = new CouponDao().GetCouponDetails(couponId);
                    if (couponDetails != null)
                    {
                        OrderRedPagerInfo orderredpager = new OrderRedPagerInfo {
                            OrderID = orderid,
                            RedPagerActivityId = int.Parse(table.Rows[0]["ID"].ToString()),
                            RedPagerActivityName = table.Rows[0]["ActivityName"].ToString(),
                            MaxGetTimes = int.Parse(table.Rows[0]["CouponNumber"].ToString()),
                            AlreadyGetTimes = 0,
                            ItemAmountLimit = couponDetails.CouponValue,
                            OrderAmountCanUse = couponDetails.ConditionValue
                        };
                        TimeSpan span = (TimeSpan) (couponDetails.EndDate - DateTime.Now);
                        orderredpager.ExpiryDays = span.Days;
                        orderredpager.UserID = userid;
                        return this.CreateOrderRedPager(orderredpager);
                    }
                }
            }
            return false;
        }

        public bool DelOrderRedPager(int orderid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete  vshop_OrderRedPager  where OrderID=@OrderID");
            this.database.AddInParameter(sqlStringCommand, "OrderID", DbType.Int32, orderid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public OrderRedPagerInfo GetOrderRedPagerInfo(string orderid)
        {
            if (string.IsNullOrEmpty(orderid))
            {
                return null;
            }
            OrderRedPagerInfo info = new OrderRedPagerInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_OrderRedPager where OrderID=@OrderID");
            this.database.AddInParameter(sqlStringCommand, "OrderID", DbType.String, orderid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrderRedPagerInfo(reader);
                }
            }
            return info;
        }

        public bool SetIsOpen(int orderredpagerid, bool isopen)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_OrderRedPager set IsOpen=@IsOpen where OrderRedPagerId=@OrderRedPagerId");
            this.database.AddInParameter(sqlStringCommand, "OrderRedPagerId", DbType.Int32, orderredpagerid);
            this.database.AddInParameter(sqlStringCommand, "IsOpen", DbType.Boolean, isopen);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateOrderRedPager(OrderRedPagerInfo orderredpager)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_OrderRedPager SET RedPagerActivityId=@RedPagerActivityId,MaxGetTimes=@MaxGetTimes,AlreadyGetTimes=@AlreadyGetTimes,ItemAmountLimit=@ItemAmountLimit,OrderAmountCanUse=@OrderAmountCanUse,ExpiryDays=@ExpiryDays,UserID=@UserID WHERE OrderID=@OrderID");
            this.database.AddInParameter(sqlStringCommand, "OrderID", DbType.String, orderredpager.OrderID);
            this.database.AddInParameter(sqlStringCommand, "RedPagerActivityId", DbType.Int32, orderredpager.RedPagerActivityId);
            this.database.AddInParameter(sqlStringCommand, "MaxGetTimes", DbType.Int32, orderredpager.MaxGetTimes);
            this.database.AddInParameter(sqlStringCommand, "AlreadyGetTimes", DbType.Int32, orderredpager.AlreadyGetTimes);
            this.database.AddInParameter(sqlStringCommand, "ItemAmountLimit", DbType.Decimal, orderredpager.ItemAmountLimit);
            this.database.AddInParameter(sqlStringCommand, "OrderAmountCanUse", DbType.Decimal, orderredpager.OrderAmountCanUse);
            this.database.AddInParameter(sqlStringCommand, "ExpiryDays", DbType.Int32, orderredpager.ExpiryDays);
            this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, orderredpager.UserID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

