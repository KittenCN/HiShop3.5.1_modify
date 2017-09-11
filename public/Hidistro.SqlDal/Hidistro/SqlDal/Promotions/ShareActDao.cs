namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class ShareActDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddRecord(ShareActivityRecordInfo record)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_ShareActivity_Record]([shareId],[shareUser],[attendUser]) VALUES (@shareId,@shareUser,@attendUser);");
            this.database.AddInParameter(sqlStringCommand, "shareId", DbType.Int32, record.shareId);
            this.database.AddInParameter(sqlStringCommand, "shareUser", DbType.Int32, record.shareUser);
            this.database.AddInParameter(sqlStringCommand, "attendUser", DbType.Int32, record.attendUser);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int Create(ShareActivityInfo share, ref string msg)
        {
            msg = "未知错误";
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_ShareActivity]([CouponId],[BeginDate],[EndDate],[MeetValue],[CouponNumber],[CouponName],[ActivityName],[ImgUrl],[ShareTitle],[Description])VALUES (@CouponId,@BeginDate,@EndDate,@MeetValue,@CouponNumber,@CouponName,@ActivityName,@ImgUrl,@ShareTitle,@Description); SELECT CAST(scope_identity() AS int);");
                this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, share.CouponId);
                this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.DateTime, share.BeginDate);
                this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, share.EndDate);
                this.database.AddInParameter(sqlStringCommand, "MeetValue", DbType.Decimal, share.MeetValue);
                this.database.AddInParameter(sqlStringCommand, "CouponNumber", DbType.Int32, share.CouponNumber);
                this.database.AddInParameter(sqlStringCommand, "CouponName", DbType.String, share.CouponName);
                this.database.AddInParameter(sqlStringCommand, "ActivityName", DbType.String, share.ActivityName);
                this.database.AddInParameter(sqlStringCommand, "ImgUrl", DbType.String, share.ImgUrl);
                this.database.AddInParameter(sqlStringCommand, "ShareTitle", DbType.String, share.ShareTitle);
                this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, share.Description);
                return (int) this.database.ExecuteScalar(sqlStringCommand);
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return 0;
            }
        }

        public bool Delete(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShareActivity WHERE ID = @Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public ShareActivityInfo GetAct(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShareActivity WHERE id = @ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, Id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ShareActivityInfo>(reader);
            }
        }

        public int GeTAttendCount(int actId, int shareUser)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_ShareActivity_Record WHERE shareId = @shareId and shareUser=@shareUser");
            this.database.AddInParameter(sqlStringCommand, "shareId", DbType.Int32, actId);
            this.database.AddInParameter(sqlStringCommand, "shareUser", DbType.Int32, shareUser);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public DataTable GetOrderRedPager(string OrderID, int UserID)
        {
            DataTable table = null;
            string str = string.Empty;
            if (UserID > 0)
            {
                str = "SELECT * from  vshop_OrderRedPager WHERE  OrderID=@OrderID and UserID=@UserID ";
            }
            else if (UserID == -100)
            {
                str = "SELECT * from  vshop_OrderRedPager WHERE  OrderID=@OrderID ";
            }
            if (string.IsNullOrEmpty(str))
            {
                return table;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "OrderID", DbType.String, OrderID);
            this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, UserID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetShareActivity()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from  Hishop_ShareActivity  WHERE  BeginDate<=getdate() and getdate()<=EndDate  order by MeetValue asc ");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public ShareActivityInfo GetShareActivity(int CouponId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShareActivity WHERE CouponId = @CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, CouponId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ShareActivityInfo>(reader);
            }
        }

        public bool HasAttend(int actId, int attendUser)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_ShareActivity_Record WHERE shareId = @shareId and attendUser=@attendUser");
            this.database.AddInParameter(sqlStringCommand, "shareId", DbType.Int32, actId);
            this.database.AddInParameter(sqlStringCommand, "attendUser", DbType.Int32, attendUser);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public DbQueryResult Query(ShareActivitySearch query)
        {
            StringBuilder builder = new StringBuilder("1=1 ");
            if (!string.IsNullOrEmpty(query.CouponName))
            {
                builder.AppendFormat(" and CouponName like '%{0}%' ", query.CouponName);
            }
            if (query.status != ShareActivityStatus.All)
            {
                if (query.status == ShareActivityStatus.In)
                {
                    builder.AppendFormat("and [BeginDate] <= '{0}' and [EndDate] >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (query.status == ShareActivityStatus.End)
                {
                    builder.AppendFormat("and [EndDate] < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (query.status == ShareActivityStatus.unBegin)
                {
                    builder.AppendFormat("and [BeginDate] > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_ShareActivity", "id", builder.ToString(), "*");
        }

        public bool Update(ShareActivityInfo share, ref string msg)
        {
            msg = "未知错误";
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_ShareActivity] SET [CouponId] = @CouponId ,[BeginDate] = @BeginDate ,[EndDate] = @EndDate ,[MeetValue] = @MeetValue ,[CouponName] = @CouponName ,[CouponNumber] = @CouponNumber ,[ActivityName] = @ActivityName,[ImgUrl] = @ImgUrl,[ShareTitle] = @ShareTitle,[Description]= @Description where ID=@ID");
                this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, share.CouponId);
                this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.DateTime, share.BeginDate);
                this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, share.EndDate);
                this.database.AddInParameter(sqlStringCommand, "MeetValue", DbType.Decimal, share.MeetValue);
                this.database.AddInParameter(sqlStringCommand, "CouponName", DbType.String, share.CouponName);
                this.database.AddInParameter(sqlStringCommand, "CouponNumber", DbType.Int32, share.CouponNumber);
                this.database.AddInParameter(sqlStringCommand, "ActivityName", DbType.String, share.ActivityName);
                this.database.AddInParameter(sqlStringCommand, "ImgUrl", DbType.String, share.ImgUrl);
                this.database.AddInParameter(sqlStringCommand, "ShareTitle", DbType.String, share.ShareTitle);
                this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, share.Description);
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.String, share.Id);
                this.database.ExecuteScalar(sqlStringCommand);
                msg = "";
                return true;
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return false;
            }
        }
    }
}

