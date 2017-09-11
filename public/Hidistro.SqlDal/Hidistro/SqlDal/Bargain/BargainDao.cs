namespace Hidistro.SqlDal.Bargain
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Bargain;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class BargainDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool ActionIsEnd(int bargainDetialId)
        {
            string query = "select count(0) from Hishop_Orders where BargainDetialId=" + bargainDetialId;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "BargainDetialId", DbType.Int32, bargainDetialId);
            return (Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public bool DeleteBargainById(string ids)
        {
            string query = "update Hishop_Bargain set IsDelete=1 where id  in (" + ids + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool ExistsHelpBargainDetial(HelpBargainDetialInfo helpBargainDetial)
        {
            string query = "select count(*) from Hishop_HelpBargainDetial where BargainDetialId=@BargainDetialId and UserId=@UserId and BargainId=@BargainId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "BargainDetialId", DbType.Int32, helpBargainDetial.BargainDetialId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, helpBargainDetial.UserId);
            this.database.AddInParameter(sqlStringCommand, "BargainId", DbType.Int32, helpBargainDetial.BargainId);
            return (int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString()) > 0);
        }

        public HelpBargainDetialInfo GeHelpBargainDetialInfo(int bargainDetialId, int userId)
        {
            HelpBargainDetialInfo info = new HelpBargainDetialInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_HelpBargainDetial WHERE BargainDetialId = @BargainDetialId and UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "BargainDetialId", DbType.Int32, bargainDetialId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<HelpBargainDetialInfo>(reader);
            }
        }

        public DataTable GetAllBargain()
        {
            string query = "select * from Hishop_Bargain where IsDelete=0 ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetBargainById(string ids)
        {
            string query = "select * from Hishop_Bargain where id  in (" + ids + ") and BeginDate< GETDATE() AND  GETDATE()< EndDate AND  IsDelete=0";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public BargainDetialInfo GetBargainDetialInfo(int id)
        {
            BargainDetialInfo info = new BargainDetialInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BargainDetial WHERE id = @id AND IsDelete=0");
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<BargainDetialInfo>(reader);
            }
        }

        public BargainDetialInfo GetBargainDetialInfo(int bargainId, int userId)
        {
            BargainDetialInfo info = new BargainDetialInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BargainDetial WHERE BargainId = @bargainId and UserId=@UserId AND IsDelete=0");
            this.database.AddInParameter(sqlStringCommand, "bargainId", DbType.Int32, bargainId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<BargainDetialInfo>(reader);
            }
        }

        public BargainInfo GetBargainInfo(int id)
        {
            BargainInfo info = new BargainInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Bargain WHERE id = @id AND  IsDelete=0");
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<BargainInfo>(reader);
            }
        }

        public BargainInfo GetBargainInfoByDetialId(int bargainDetialId)
        {
            BargainInfo info = new BargainInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_Bargain where id =(select BargainId from Hishop_BargainDetial where id=@id AND IsDelete=0) AND IsDelete=0");
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, bargainDetialId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<BargainInfo>(reader);
            }
        }

        public DbQueryResult GetBargainList(BargainQuery query)
        {
            string str;
            new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            builder.Append(" IsDelete=0 and SaleStatus=1");
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            if (!string.IsNullOrEmpty(query.Title))
            {
                builder.AppendFormat(" AND Title LIKE '%{0}%'", DataHelper.CleanSearchString(query.Title));
            }
            if (((str = query.Type) != null) && (str != "0"))
            {
                if (!(str == "1"))
                {
                    if (str == "2")
                    {
                        builder.AppendFormat(" AND bargainstatus='已结束' ", new object[0]);
                    }
                    else if (str == "3")
                    {
                        builder.AppendFormat(" AND bargainstatus='未开始' ", new object[0]);
                    }
                }
                else
                {
                    builder.AppendFormat(" AND bargainstatus='进行中'", new object[0]);
                }
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BargainList", "Id", builder.ToString(), "*");
        }

        public BargainStatisticalData GetBargainStatisticalDataInfo(int bargainId)
        {
            BargainStatisticalData data = new BargainStatisticalData();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select (SELECT COUNT(*) FROM Hishop_HelpBargainDetial WHERE BargainId=@id) AS NumberOfParticipants,(SELECT COUNT(*)  from ( SELECT   a.UserId  FROM Hishop_Orders a join Hishop_BargainDetial b on a.BargainDetialId=b.id where BargainId =@id and  (OrderStatus=5 OR OrderStatus=2 OR (OrderStatus=1 and Gateway='hishop.plugins.payment.podrequest')) group by a.UserId ) as w) as SingleMember, (SELECT SUM(b.Number)  FROM Hishop_Orders a join Hishop_BargainDetial b on a.BargainDetialId=b.id where BargainId =@id and  (OrderStatus=5 OR OrderStatus=2 OR (OrderStatus=1 and Gateway='hishop.plugins.payment.podrequest')) ) as ActivitySales, (SELECT ActivityStock  FROM  Hishop_Bargain  where id=@id) as ActivityStock, (SELECT sum(Number*price)  FROM Hishop_Orders a join Hishop_BargainDetial b on a.BargainDetialId=b.id where BargainId =@id and  (OrderStatus=5 OR OrderStatus=2 OR (OrderStatus=1 and Gateway='hishop.plugins.payment.podrequest')) ) as AverageTransactionPrice, CASE WHEN BeginDate <GETDATE() and GETDATE()<EndDate  THEN '进行中' WHEN BeginDate >GETDATE()  THEN '未开始' WHEN EndDate < GETDATE()  THEN '已结束' ELSE NULL END ActiveState  from Hishop_Bargain where id=@id");
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, bargainId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<BargainStatisticalData>(reader);
            }
        }

        public int GetHelpBargainDetialCount(int bargainDetialId)
        {
            string query = "select count(*)  from Hishop_HelpBargainDetial where BargainDetialId=" + bargainDetialId;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
        }

        public DataTable GetHelpBargainDetials(int bargainDetialId)
        {
            string query = "select  b.UserName,a.* from Hishop_HelpBargainDetial a join aspnet_Members b on a.userid=b.userid where BargainDetialId  = " + bargainDetialId + " order by a.CreateDate desc ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DbQueryResult GetMyBargainList(BargainQuery query)
        {
            new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" IsDelete=0 AND bargainstatus='{0}' ", "进行中");
            if (query.Status > 0)
            {
                builder.AppendFormat("AND bargainDetialID in (select bargainDetialID from Hishop_HelpBargainDetial where UserId={0})", query.UserId);
                builder.AppendFormat("and userid!={0}", query.UserId);
            }
            else
            {
                builder.AppendFormat(" AND UserId={0}", query.UserId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_MyBargainList", "Id", builder.ToString(), "*");
        }

        public int GetTotal(BargainQuery query)
        {
            string str2;
            new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            builder.Append(" IsDelete=0 ");
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            if (!string.IsNullOrEmpty(query.Title))
            {
                builder.AppendFormat(" AND Title LIKE '%{0}%'", DataHelper.CleanSearchString(query.Title));
            }
            if (((str2 = query.Type) != null) && (str2 != "0"))
            {
                if (!(str2 == "1"))
                {
                    if (str2 == "2")
                    {
                        builder.AppendFormat(" AND bargainstatus='已结束' ", new object[0]);
                    }
                    else if (str2 == "3")
                    {
                        builder.AppendFormat(" AND bargainstatus='未开始' ", new object[0]);
                    }
                }
                else
                {
                    builder.AppendFormat(" AND bargainstatus='进行中'", new object[0]);
                }
            }
            string str = string.Format(" SELECT count(*) FROM vw_Hishop_BargainList where {0}", builder.ToString());
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            int num = 0;
            if (this.database.ExecuteScalar(sqlStringCommand) != null)
            {
                num = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
            }
            return num;
        }

        public int HelpBargainCount(int bargainId)
        {
            string query = "select count(*) from Hishop_HelpBargainDetial where BargainId=@BargainId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "BargainId", DbType.Int32, bargainId);
            return int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
        }

        public bool InsertBargain(BargainInfo bargain)
        {
            string query = "insert into Hishop_Bargain(Title,ActivityCover,BeginDate,EndDate,Remarks,CreateDate,ProductId,ActivityStock,PurchaseNumber,BargainType,BargainTypeMaxVlue,BargainTypeMinVlue,InitialPrice,IsCommission,CommissionDiscount,FloorPrice) values(@Title,@ActivityCover,@BeginDate,@EndDate,@Remarks,@CreateDate,@ProductId,@ActivityStock,@PurchaseNumber,@BargainType,@BargainTypeMaxVlue,@BargainTypeMinVlue,@InitialPrice,@IsCommission,@CommissionDiscount,@FloorPrice)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, bargain.Title);
            this.database.AddInParameter(sqlStringCommand, "ActivityCover", DbType.String, bargain.ActivityCover);
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.DateTime, bargain.BeginDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, bargain.EndDate);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, bargain.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, bargain.ProductId);
            this.database.AddInParameter(sqlStringCommand, "ActivityStock", DbType.Int32, bargain.ActivityStock);
            this.database.AddInParameter(sqlStringCommand, "PurchaseNumber", DbType.Int32, bargain.PurchaseNumber);
            this.database.AddInParameter(sqlStringCommand, "BargainType", DbType.Int32, bargain.BargainType);
            this.database.AddInParameter(sqlStringCommand, "BargainTypeMaxVlue", DbType.Double, bargain.BargainTypeMaxVlue);
            this.database.AddInParameter(sqlStringCommand, "BargainTypeMinVlue", DbType.Double, bargain.BargainTypeMinVlue);
            this.database.AddInParameter(sqlStringCommand, "InitialPrice", DbType.Decimal, bargain.InitialPrice);
            this.database.AddInParameter(sqlStringCommand, "IsCommission", DbType.Boolean, bargain.IsCommission);
            this.database.AddInParameter(sqlStringCommand, "CommissionDiscount", DbType.Int32, bargain.CommissionDiscount);
            this.database.AddInParameter(sqlStringCommand, "FloorPrice", DbType.Decimal, bargain.FloorPrice);
            this.database.AddInParameter(sqlStringCommand, "Remarks", DbType.String, bargain.Remarks);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool InsertBargainDetial(BargainDetialInfo bargainDetial, out int bargainDetialId)
        {
            bargainDetialId = 0;
            string query = "insert into Hishop_BargainDetial(UserId,BargainId,Number,Price,NumberOfParticipants,CreateDate,Sku) values(@UserId,@BargainId,@Number,@Price,@NumberOfParticipants,@CreateDate,@Sku)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, bargainDetial.UserId);
            this.database.AddInParameter(sqlStringCommand, "BargainId", DbType.Int32, bargainDetial.BargainId);
            this.database.AddInParameter(sqlStringCommand, "Number", DbType.Int32, bargainDetial.Number);
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.Decimal, bargainDetial.Price);
            this.database.AddInParameter(sqlStringCommand, "NumberOfParticipants", DbType.Int32, bargainDetial.NumberOfParticipants);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, bargainDetial.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "Sku", DbType.String, bargainDetial.Sku);
            bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            if (flag)
            {
                string str2 = "select max(id) from Hishop_BargainDetial ";
                sqlStringCommand = this.database.GetSqlStringCommand(str2);
                bargainDetialId = (int) this.database.ExecuteScalar(sqlStringCommand);
            }
            return flag;
        }

        public bool InsertHelpBargainDetial(HelpBargainDetialInfo helpBargainDetial)
        {
            string query = "insert into Hishop_HelpBargainDetial(BargainDetialId,UserId,BargainPrice,CreateDate,BargainId) values(@BargainDetialId,@UserId,@BargainPrice,@CreateDate,@BargainId)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "BargainDetialId", DbType.Int32, helpBargainDetial.BargainDetialId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, helpBargainDetial.UserId);
            this.database.AddInParameter(sqlStringCommand, "BargainPrice", DbType.Decimal, helpBargainDetial.BargainPrice);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, helpBargainDetial.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "BargainId", DbType.Int32, helpBargainDetial.BargainId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public string IsCanBuyByBarginDetailId(int bargainDetailId)
        {
            string str = "1";
            string query = "select top 1 id,BeginDate,EndDate from Hishop_Bargain where id =(select BargainId from Hishop_BargainDetial where id=" + bargainDetailId + " AND IsDelete=0) and ActivityStock>TranNumber";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count > 0)
            {
                DateTime time = DateTime.Parse(table.Rows[0]["EndDate"].ToString());
                DateTime time2 = DateTime.Parse(table.Rows[0]["BeginDate"].ToString());
                if (time < DateTime.Now)
                {
                    return "该活动已结束";
                }
                if (time2 > DateTime.Now)
                {
                    str = "该活动还未开始";
                }
                return str;
            }
            return "该活动商品无库存";
        }

        public string IsCanBuyByBarginId(int bargainId)
        {
            string str = "1";
            string query = "select top 1 id,BeginDate,EndDate from Hishop_Bargain where id=" + bargainId + " and ActivityStock>TranNumber";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count > 0)
            {
                DateTime time = DateTime.Parse(table.Rows[0]["EndDate"].ToString());
                DateTime time2 = DateTime.Parse(table.Rows[0]["BeginDate"].ToString());
                if (time < DateTime.Now)
                {
                    return "该活动已结束";
                }
                if (time2 > DateTime.Now)
                {
                    str = "该活动还未开始";
                }
                return str;
            }
            return "该活动商品无库存";
        }

        public bool UpdateBargain(BargainInfo bargain)
        {
            string query = "update  Hishop_Bargain set  Title=@Title,ActivityCover=@ActivityCover,BeginDate=@BeginDate,EndDate=@EndDate,Remarks=@Remarks,CreateDate=@CreateDate,ProductId=@ProductId,ActivityStock=@ActivityStock,PurchaseNumber=@PurchaseNumber,BargainType=@BargainType,BargainTypeMaxVlue=@BargainTypeMaxVlue,BargainTypeMinVlue=@BargainTypeMinVlue,InitialPrice=@InitialPrice,IsCommission=@IsCommission,CommissionDiscount=@CommissionDiscount,FloorPrice=@FloorPrice where id=@id";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, bargain.Title);
            this.database.AddInParameter(sqlStringCommand, "ActivityCover", DbType.String, bargain.ActivityCover);
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.DateTime, bargain.BeginDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, bargain.EndDate);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, bargain.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, bargain.ProductId);
            this.database.AddInParameter(sqlStringCommand, "ActivityStock", DbType.Int32, bargain.ActivityStock);
            this.database.AddInParameter(sqlStringCommand, "PurchaseNumber", DbType.Int32, bargain.PurchaseNumber);
            this.database.AddInParameter(sqlStringCommand, "BargainType", DbType.Int32, bargain.BargainType);
            this.database.AddInParameter(sqlStringCommand, "BargainTypeMaxVlue", DbType.Double, bargain.BargainTypeMaxVlue);
            this.database.AddInParameter(sqlStringCommand, "BargainTypeMinVlue", DbType.Double, bargain.BargainTypeMinVlue);
            this.database.AddInParameter(sqlStringCommand, "InitialPrice", DbType.Decimal, bargain.InitialPrice);
            this.database.AddInParameter(sqlStringCommand, "IsCommission", DbType.Boolean, bargain.IsCommission);
            this.database.AddInParameter(sqlStringCommand, "CommissionDiscount", DbType.Int32, bargain.CommissionDiscount);
            this.database.AddInParameter(sqlStringCommand, "FloorPrice", DbType.Decimal, bargain.FloorPrice);
            this.database.AddInParameter(sqlStringCommand, "Remarks", DbType.String, bargain.Remarks);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.String, bargain.Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateBargain(int bargainId, DateTime endDate)
        {
            string query = "update  Hishop_Bargain set  EndDate=@EndDate where id=@id";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, endDate);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.String, bargainId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateBargainDetial(HelpBargainDetialInfo helpBargainDetial)
        {
            string query = "update Hishop_BargainDetial set  Price=Price-@BargainPrice,NumberOfParticipants=NumberOfParticipants+1 where id=@id";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "BargainPrice", DbType.Decimal, helpBargainDetial.BargainPrice);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, helpBargainDetial.BargainDetialId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateNumberById(int bargainDetialId, int number)
        {
            string query = "update Hishop_BargainDetial set Number=@number where id  =@id";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "number", DbType.Int32, number);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, bargainDetialId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

