namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ActivityDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddActivitiesMember(int ActivitiesId, int Userid, DbTransaction dbTran = null)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ActivitiesMember  (ActivitiesId,Userid)VALUES(@ActivitiesId,@Userid)");
            this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, ActivitiesId);
            this.database.AddInParameter(sqlStringCommand, "Userid", DbType.Int32, Userid);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool AddProducts(int actId, int productID)
        {
            try
            {
                ActivityInfo act = this.GetAct(actId);
                if (act != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat(" insert into Hishop_Activities_Product(ActivitiesId , ProductId) values({0} , {1}) ", act.ActivitiesId, productID);
                    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
                    this.database.ExecuteNonQuery(sqlStringCommand);
                    sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Activities set IsAllProduct=0  WHERE ActivitiesId = @ActivitiesId");
                    this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, actId);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddProducts(int actId, bool IsAllProduct, IList<string> productIDs)
        {
            try
            {
                ActivityInfo act = this.GetAct(actId);
                if (act != null)
                {
                    if (IsAllProduct)
                    {
                        act.isAllProduct = true;
                    }
                    else
                    {
                        act.isAllProduct = false;
                    }
                    if (IsAllProduct)
                    {
                        DbCommand command = this.database.GetSqlStringCommand("update Hishop_Activities set IsAllProduct=@IsAllProduct  WHERE ActivitiesId = @ActivitiesId");
                        this.database.AddInParameter(command, "ActivitiesId", DbType.Int32, actId);
                        this.database.AddInParameter(command, "IsAllProduct", DbType.Boolean, act.isAllProduct);
                        this.database.ExecuteNonQuery(command);
                        command = this.database.GetSqlStringCommand("Delete from  Hishop_Activities_Product WHERE ActivitiesId = @ActivitiesId");
                        this.database.AddInParameter(command, "ActivitiesId", DbType.Int32, actId);
                        this.database.ExecuteNonQuery(command);
                        return true;
                    }
                    string str = "";
                    if (productIDs.Count > 0)
                    {
                        foreach (string str2 in productIDs)
                        {
                            str = str + "," + str2;
                        }
                    }
                    str = str.Substring(1);
                    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Delete from  Hishop_Activities_Product WHERE ActivitiesId ={0} and ProductId in ( {1} )", actId, str));
                    this.database.ExecuteNonQuery(sqlStringCommand);
                    StringBuilder builder = new StringBuilder();
                    foreach (string str3 in productIDs)
                    {
                        builder.AppendFormat(" insert into Hishop_Activities_Product(ActivitiesId , ProductId) values({0} , {1}) ", act.ActivitiesId, str3);
                    }
                    sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
                    this.database.ExecuteNonQuery(sqlStringCommand);
                    sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Activities set IsAllProduct=@IsAllProduct WHERE ActivitiesId = @ActivitiesId");
                    this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, actId);
                    this.database.AddInParameter(sqlStringCommand, "IsAllProduct", DbType.Boolean, act.isAllProduct);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Create(ActivityInfo act, ref string msg)
        {
            msg = "未知错误";
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ActivitiesId  FROM Hishop_Activities WHERE ActivitiesName=@Name");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, act.ActivitiesName);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    msg = "减免活动重名";
                    return 0;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT ActivitiesId  FROM Hishop_Activities WHERE isAllProduct=1 and  EndTime>@NowTime");
                this.database.AddInParameter(sqlStringCommand, "NowTime", DbType.DateTime, DateTime.Now);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    msg = "已有全部商品满减活动未结束，不能再添加新活动！";
                    return 0;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_Activities] ([ActivitiesName] ,[ActivitiesType],[StartTime],[EndTime],[ActivitiesDescription],[TakeEffect],[Type],[MemberGrades],[DefualtGroup],[CustomGroup],[attendTime],[attendType],[isAllProduct],[MeetMoney],[ReductionMoney],[MeetType]) VALUES (@ActivitiesName,@ActivitiesType,@StartTime,@EndTime,@ActivitiesDescription,@TakeEffect,@Type,@MemberGrades,@DefualtGroup,@CustomGroup,@attendTime,@attendType,@isAllProduct,@MeetMoney,@ReductionMoney ,@MeetType); SELECT CAST(scope_identity() AS int);");
                this.database.AddInParameter(sqlStringCommand, "ActivitiesName", DbType.String, act.ActivitiesName);
                this.database.AddInParameter(sqlStringCommand, "ActivitiesType", DbType.Int32, act.ActivitiesType);
                this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, act.StartTime);
                this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, act.EndTime);
                this.database.AddInParameter(sqlStringCommand, "ActivitiesDescription", DbType.String, act.ActivitiesDescription);
                this.database.AddInParameter(sqlStringCommand, "TakeEffect", DbType.Int32, act.TakeEffect);
                this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, act.Type);
                this.database.AddInParameter(sqlStringCommand, "MemberGrades", DbType.String, act.MemberGrades);
                this.database.AddInParameter(sqlStringCommand, "DefualtGroup", DbType.String, act.DefualtGroup);
                this.database.AddInParameter(sqlStringCommand, "CustomGroup", DbType.String, act.CustomGroup);
                this.database.AddInParameter(sqlStringCommand, "attendTime", DbType.Int32, act.attendTime);
                this.database.AddInParameter(sqlStringCommand, "attendType", DbType.Int32, act.attendType);
                this.database.AddInParameter(sqlStringCommand, "isAllProduct", DbType.Boolean, act.isAllProduct);
                this.database.AddInParameter(sqlStringCommand, "MeetMoney", DbType.Decimal, act.MeetMoney);
                this.database.AddInParameter(sqlStringCommand, "ReductionMoney", DbType.Decimal, act.ReductionMoney);
                this.database.AddInParameter(sqlStringCommand, "MeetType", DbType.Int32, act.MeetType);
                int num = (int) this.database.ExecuteScalar(sqlStringCommand);
                act.ActivitiesId = num;
                if ((act.Details != null) && (act.Details.Count > 0))
                {
                    string query = "INSERT INTO [Hishop_Activities_Detail]([ActivitiesId],[MeetMoney],[ReductionMoney],[bFreeShipping],[Integral],[CouponId],[MeetNumber])VALUES(@ActivitiesId,@MeetMoney,@ReductionMoney,@bFreeShipping,@Integral,@CouponId ,@MeetNumber)";
                    foreach (ActivityDetailInfo info in act.Details)
                    {
                        sqlStringCommand = this.database.GetSqlStringCommand(query);
                        this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, act.ActivitiesId);
                        this.database.AddInParameter(sqlStringCommand, "MeetMoney", DbType.Decimal, info.MeetMoney);
                        this.database.AddInParameter(sqlStringCommand, "ReductionMoney", DbType.Decimal, info.ReductionMoney);
                        this.database.AddInParameter(sqlStringCommand, "bFreeShipping", DbType.Boolean, info.bFreeShipping);
                        this.database.AddInParameter(sqlStringCommand, "Integral", DbType.Int32, info.Integral);
                        this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, info.CouponId);
                        this.database.AddInParameter(sqlStringCommand, "MeetNumber", DbType.Int32, info.MeetNumber);
                        this.database.ExecuteScalar(sqlStringCommand);
                    }
                }
                msg = "";
                return num;
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return 0;
            }
        }

        public bool Delete(int Id)
        {
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Activities WHERE ActivitiesId = @Id");
                this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
                DbCommand command = this.database.GetSqlStringCommand("DELETE FROM Hishop_Activities_Detail WHERE ActivitiesId = @Id");
                this.database.AddInParameter(command, "Id", DbType.Int32, Id);
                DbCommand command3 = this.database.GetSqlStringCommand("DELETE FROM Hishop_Activities_Product WHERE ActivitiesId = @Id");
                this.database.AddInParameter(command3, "Id", DbType.Int32, Id);
                try
                {
                    this.database.ExecuteNonQuery(sqlStringCommand, transaction);
                    this.database.ExecuteNonQuery(command, transaction);
                    this.database.ExecuteNonQuery(command3, transaction);
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
                finally
                {
                    if (transaction.Connection != null)
                    {
                        connection.Close();
                    }
                }
            }
            return false;
        }

        public bool DeleteProducts(int actId, string ProductIds)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Delete from  Hishop_Activities_Product WHERE ActivitiesId ={0} and ProductId in ( {1} )", actId, ProductIds.ReplaceSingleQuoteMark()));
                this.database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EndAct(int Aid)
        {
            string query = "update Hishop_Activities set EndTime=@EndTime where ActivitiesId=@ActivitiesId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, Aid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public ActivityInfo GetAct(int Id)
        {
            ActivityInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Activities WHERE ActivitiesId = @ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, Id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<ActivityInfo>(reader);
            }
            sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Activities_Detail WHERE ActivitiesId = @ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, Id);
            using (IDataReader reader2 = this.database.ExecuteReader(sqlStringCommand))
            {
                IList<ActivityDetailInfo> list = ReaderConvert.ReaderToList<ActivityDetailInfo>(reader2);
                info.Details = list;
            }
            return info;
        }

        public ActivityInfo GetAct(string name)
        {
            ActivityInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Activities WHERE ActivitiesName= @Name");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.Int32, name);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<ActivityInfo>(reader);
            }
            sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Activities_Detail WHERE ActivitiesId = @ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, info.ActivitiesId);
            using (IDataReader reader2 = this.database.ExecuteReader(sqlStringCommand))
            {
                IList<ActivityDetailInfo> list = ReaderConvert.ReaderToList<ActivityDetailInfo>(reader2);
                info.Details = list;
            }
            return info;
        }

        public DataTable GetActivities()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from  Hishop_Activities  WHERE  StartTime<=getdate() and getdate()<=EndTIme  ");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetActivities_Detail(int actId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from  vw_Hishop_Activities_Detail  WHERE  StartTime<=getdate() and getdate()<=EndTIme and ActivitiesId=@ActivitiesId order by MeetMoney asc ");
            this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, actId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public int GetActivitiesMember(int Userid, int ActivitiesId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(id) from Hishop_ActivitiesMember where Userid=@Userid and ActivitiesId=@ActivitiesId  ");
            this.database.AddInParameter(sqlStringCommand, "Userid", DbType.Int32, Userid);
            this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, ActivitiesId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return int.Parse(DataHelper.ConverDataReaderToDataTable(reader).Rows[0][0].ToString());
            }
        }

        public DataTable GetActivitiesProducts(int actId, int ProductID)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_Activities_Product  where status=0 and ActivitiesId=@ActivitiesId and ProductID=@ProductID");
            this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, actId);
            this.database.AddInParameter(sqlStringCommand, "ProductID", DbType.Int32, ProductID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetActivityTopics(string types = "0")
        {
            string query = "SELECT ROW_NUMBER() OVER(ORDER BY BeginDate) num,*  FROM ((select Id,Name,MemberGrades,DefualtGroup,CustomGroup,BeginDate,EndDate,isnull(ImgUrl,'') as 'ImgUrl',1 as 'ActivityType' from Hishop_PointExChange_PointExChanges where BeginDate <= getdate() and EndDate >= getdate())  union (select ActivitiesId as 'Id',ActivitiesName as 'Name',MemberGrades,DefualtGroup,CustomGroup,StartTime as 'BeginDate',EndTime as 'EndDate', '' as 'ImgUrl', 2 as 'ActivityType' from Hishop_Activities where StartTime <= getdate() and EndTime >= getdate()) union (select CouponId as 'Id',CouponName as 'Name',MemberGrades,DefualtGroup,CustomGroup,BeginDate,EndDate,isnull(ImgUrl,'') as 'ImgUrl',3 as 'ActivityType' from Hishop_Coupon_Coupons where BeginDate <= getdate() and EndDate >= getdate() and Finished = 0 and CouponTypes like '%" + 2.ToString() + "%') union (select VoteId as 'Id',VoteName as 'Name','0' as 'MemberGrades',DefualtGroup,CustomGroup,StartDate as 'BeginDate',EndDate,isnull(ImageUrl,'') as 'ImgUrl',4 as 'ActivityType' from Hishop_Votes where StartDate <= getdate() and EndDate >= getdate()) union (select GameId as 'Id',GameTitle as 'Name',ApplyMembers as 'MemberGrades',DefualtGroup,CustomGroup,BeginTime as 'BeginDate',EndTime as 'EndDate', '' as 'ImgUrl', 5 as 'ActivityType' from Hishop_PromotionGame where status=0 and BeginTime <= getdate() and EndTime >= getdate())) tb ";
            if (types != "0")
            {
                query = query + " where ActivityType in (" + types + ")";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public int GetActivityTopicsNum(string types = "0")
        {
            string query = "SELECT count(id) FROM ((select Id,1 as 'ActivityType' from Hishop_PointExChange_PointExChanges where BeginDate <= getdate() and EndDate >= getdate()) union (select ActivitiesId as 'Id',2 as 'ActivityType' from Hishop_Activities where StartTime <= getdate() and EndTime >= getdate()) union (select CouponId as 'Id',3 as 'ActivityType' from Hishop_Coupon_Coupons where BeginDate <= getdate() and EndDate >= getdate() and CouponTypes like '%" + 2.ToString() + "%') union (select VoteId as 'Id',4 as 'ActivityType' from Hishop_Votes where StartDate <= getdate() and EndDate >= getdate()) union (select GameId as 'Id', 5 as 'ActivityType' from Hishop_PromotionGame where BeginTime <= getdate() and EndTime >= getdate())) tb ";
            if (types != "0")
            {
                query = query + " where ActivityType in (" + types + ")";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return int.Parse(DataHelper.ConverDataReaderToDataTable(reader).Rows[0][0].ToString());
            }
        }

        public DataTable GetActProducts(int actId)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ActivitiesId, a.ProductId , b.ProductName , a.status from Hishop_Activities_Product a\tjoin Hishop_Products b on a.ProductId=b.ProductId where a.ActivitiesId=@ActivitiesId");
            this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, actId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public int GetHishop_Activities(int Activities_DetailID)
        {
            string query = "select ActivitiesId from Hishop_Activities_Detail where id=" + Activities_DetailID;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && !(obj2 is DBNull))
            {
                return int.Parse(obj2.ToString());
            }
            return 0;
        }

        public bool HasPartProductAct()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(1) FROM Hishop_Activities WHERE isAllProduct=0 and EndTime >= getdate()");
            return (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public DbQueryResult Query(ActivitySearch query)
        {
            StringBuilder builder = new StringBuilder("1=1 ");
            if (query.status != ActivityStatus.All)
            {
                if (query.status == ActivityStatus.In)
                {
                    builder.AppendFormat("and [StartTime] <= '{0}' and  [EndTime] >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (query.status == ActivityStatus.End)
                {
                    builder.AppendFormat("and [EndTime] < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (query.status == ActivityStatus.unBegin)
                {
                    builder.AppendFormat("and [StartTime] > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            if (query.begin.HasValue)
            {
                builder.AppendFormat("and [StartTime] >= '{0}'", query.begin.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (query.end.HasValue)
            {
                builder.AppendFormat("and [EndTime] <= '{0}'", query.end.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                builder.AppendFormat("and ActivitiesName like '%{0}%'  ", query.Name);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Activities", "ActivitiesId", builder.ToString(), "*");
        }

        public DataTable QueryProducts()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select * from  Hishop_Activities_Product", new object[0]);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable QueryProducts(int actid)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select * from  Hishop_Activities_Product where ActivitiesId = {0} ", actid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable QuerySelProducts()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select * from Hishop_Activities_Product where ActivitiesId in(select ActivitiesId from Hishop_Activities where EndTime>getdate())", new object[0]);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public bool SetProductsStatus(int actId, int status, string ProductIds)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Update Hishop_Activities_Product set status={0}   WHERE ActivitiesId ={1} and ProductId in ({2})", status, actId, ProductIds.ReplaceSingleQuoteMark()));
                this.database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(ActivityInfo act, ref string msg)
        {
            msg = "未知错误";
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ActivitiesId  FROM Hishop_Activities WHERE ActivitiesName=@Name and ActivitiesId<>@ID");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, act.ActivitiesName);
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, act.ActivitiesId);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    msg = "满减活动重名";
                    return false;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT ActivitiesId  FROM Hishop_Activities WHERE isAllProduct=1 and  EndTime>@NowTime and ActivitiesId<>@ActivitiesId");
                this.database.AddInParameter(sqlStringCommand, "NowTime", DbType.DateTime, DateTime.Now);
                this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.String, act.ActivitiesId);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    msg = "已有全部商品满减活动正在进行中，不能再添加新活动！";
                    return false;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [dbo].[Hishop_Activities] SET [ActivitiesName] = @ActivitiesName,  [ActivitiesType] = @ActivitiesType  ,[StartTime] = @StartTime   ,[EndTime] = @EndTime  ,[ActivitiesDescription] = @ActivitiesDescription ,[Type] = @Type ,[MemberGrades] = @MemberGrades  ,[DefualtGroup]=@DefualtGroup  ,[CustomGroup]=@CustomGroup  ,[attendTime] = @attendTime ,[attendType] = @attendType  ,[isAllProduct] = @isAllProduct  ,[MeetMoney] = @MeetMoney  ,[MeetType] = @MeetType  ,[ReductionMoney] = @ReductionMoney where ActivitiesId=@ID");
                this.database.AddInParameter(sqlStringCommand, "ActivitiesName", DbType.String, act.ActivitiesName);
                this.database.AddInParameter(sqlStringCommand, "ActivitiesType", DbType.Int32, act.ActivitiesType);
                this.database.AddInParameter(sqlStringCommand, "ActivitiesDescription", DbType.String, act.ActivitiesDescription);
                this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, act.Type);
                this.database.AddInParameter(sqlStringCommand, "attendTime", DbType.Int32, act.attendTime);
                this.database.AddInParameter(sqlStringCommand, "attendType", DbType.Int32, act.attendType);
                this.database.AddInParameter(sqlStringCommand, "MeetMoney", DbType.Decimal, act.MeetMoney);
                this.database.AddInParameter(sqlStringCommand, "ReductionMoney", DbType.Decimal, act.ReductionMoney);
                this.database.AddInParameter(sqlStringCommand, "MemberGrades", DbType.String, act.MemberGrades);
                this.database.AddInParameter(sqlStringCommand, "DefualtGroup", DbType.String, act.DefualtGroup);
                this.database.AddInParameter(sqlStringCommand, "CustomGroup", DbType.String, act.CustomGroup);
                this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, act.StartTime);
                this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, act.EndTime);
                this.database.AddInParameter(sqlStringCommand, "isAllProduct", DbType.Boolean, act.isAllProduct);
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.String, act.ActivitiesId);
                this.database.AddInParameter(sqlStringCommand, "MeetType", DbType.Int32, act.MeetType);
                this.database.ExecuteScalar(sqlStringCommand);
                if ((act.Details != null) && (act.Details.Count > 0))
                {
                    string query = string.Format("delete from Hishop_Activities_Detail where ActivitiesId={0};", act.ActivitiesId);
                    sqlStringCommand = this.database.GetSqlStringCommand(query);
                    this.database.ExecuteScalar(sqlStringCommand);
                    string str2 = "INSERT INTO [Hishop_Activities_Detail]([ActivitiesId],[MeetMoney],[ReductionMoney],[bFreeShipping],[Integral],[CouponId],[MeetNumber])VALUES(@ActivitiesId,@MeetMoney,@ReductionMoney,@bFreeShipping,@Integral,@CouponId,@MeetNumber)";
                    foreach (ActivityDetailInfo info in act.Details)
                    {
                        sqlStringCommand = this.database.GetSqlStringCommand(str2);
                        this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, act.ActivitiesId);
                        this.database.AddInParameter(sqlStringCommand, "MeetMoney", DbType.Decimal, info.MeetMoney);
                        this.database.AddInParameter(sqlStringCommand, "ReductionMoney", DbType.Decimal, info.ReductionMoney);
                        this.database.AddInParameter(sqlStringCommand, "bFreeShipping", DbType.Boolean, info.bFreeShipping);
                        this.database.AddInParameter(sqlStringCommand, "Integral", DbType.Int32, info.Integral);
                        this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, info.CouponId);
                        this.database.AddInParameter(sqlStringCommand, "MeetNumber", DbType.Int32, info.MeetNumber);
                        this.database.ExecuteScalar(sqlStringCommand);
                    }
                }
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

