namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;
    using System.Text.RegularExpressions;

    public class LimitedTimeDiscountDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int AddLimitedTimeDiscount(LimitedTimeDiscountInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_LimitedTimeDiscount] ([ActivityName],[BeginTime],[EndTime],[Description],[LimitNumber],[ApplyMembers],[DefualtGroup],[CustomGroup],[CreateTime],[Status],[IsCommission],[CommissionDiscount]) VALUES (@ActivityName,@BeginTime,@EndTime,@Description,@LimitNumber,@ApplyMembers,@DefualtGroup,@CustomGroup,@CreateTime,@Status,@IsCommission,@CommissionDiscount); SELECT CAST(scope_identity() AS int);");
            this.database.AddInParameter(sqlStringCommand, "ActivityName", DbType.String, info.ActivityName);
            this.database.AddInParameter(sqlStringCommand, "BeginTime", DbType.DateTime, info.BeginTime);
            this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, info.EndTime);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, info.Description);
            this.database.AddInParameter(sqlStringCommand, "LimitNumber", DbType.Int32, info.LimitNumber);
            this.database.AddInParameter(sqlStringCommand, "ApplyMembers", DbType.String, info.ApplyMembers);
            this.database.AddInParameter(sqlStringCommand, "DefualtGroup", DbType.String, info.DefualtGroup);
            this.database.AddInParameter(sqlStringCommand, "CustomGroup", DbType.String, info.CustomGroup);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, info.CreateTime);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, info.Status);
            this.database.AddInParameter(sqlStringCommand, "IsCommission", DbType.Boolean, info.IsCommission);
            this.database.AddInParameter(sqlStringCommand, "CommissionDiscount", DbType.Decimal, info.CommissionDiscount);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public bool AddLimitedTimeDiscountProduct(LimitedTimeDiscountProductInfo info)
        {
            this.DeleteDiscountProduct(info.LimitedTimeDiscountId, info.ProductId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_LimitedTimeDiscountProduct] ([LimitedTimeDiscountId],[ProductId],[Discount],[Minus],[IsDehorned],[IsChamferPoint],[FinalPrice],[CreateTime],[BeginTime],[EndTime],[Status]) VALUES (@LimitedTimeDiscountId,@ProductId,@Discount,@Minus,@IsDehorned,@IsChamferPoint,@FinalPrice,@CreateTime,@BeginTime,@EndTime,@Status);");
            this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountId", DbType.Int32, info.LimitedTimeDiscountId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, info.ProductId);
            this.database.AddInParameter(sqlStringCommand, "Discount", DbType.Decimal, info.Discount);
            this.database.AddInParameter(sqlStringCommand, "Minus", DbType.Decimal, info.Minus);
            this.database.AddInParameter(sqlStringCommand, "IsDehorned", DbType.Int32, info.IsDehorned);
            this.database.AddInParameter(sqlStringCommand, "IsChamferPoint", DbType.Int32, info.IsChamferPoint);
            this.database.AddInParameter(sqlStringCommand, "FinalPrice", DbType.Decimal, info.FinalPrice);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, info.CreateTime);
            this.database.AddInParameter(sqlStringCommand, "BeginTime", DbType.DateTime, info.BeginTime);
            this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, info.EndTime);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, info.Status);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool ChangeDiscountProductStatus(string ids, int status)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[] { "update Hishop_LimitedTimeDiscountProduct set Status=", status, " WHERE LimitedTimeDiscountProductId in (", ids, ")" }));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteDiscountProduct(string ids)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete Hishop_LimitedTimeDiscountProduct WHERE LimitedTimeDiscountProductId in (" + ids + ")");
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteDiscountProduct(int limitedTimeDiscountId, int prdocutId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete Hishop_LimitedTimeDiscountProduct WHERE LimitedTimeDiscountId =@LimitedTimeDiscountId and ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountId", DbType.Int32, limitedTimeDiscountId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, prdocutId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public LimitedTimeDiscountInfo GetDiscountInfo(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_LimitedTimeDiscount WHERE LimitedTimeDiscountId = @ID and Status!=2");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, Id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<LimitedTimeDiscountInfo>(reader);
            }
        }

        public DbQueryResult GetDiscountProduct(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SaleStatus= " + 1);
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (query.CategoryId.HasValue)
            {
                if (query.CategoryId.Value > 0)
                {
                    builder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%') ", query.MaiCategoryPath);
                }
                else
                {
                    builder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
                }
            }
            string selectFields = "SaleCounts,ThumbnailUrl60,ThumbnailUrl310,ProductId, ProductCode,IsMakeTaobao,ProductName,ProductShortName, ThumbnailUrl40, MarketPrice, ShortDescription,MinShowPrice,MaxShowPrice,SkuNum,SalePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,SaleStatus,AddedDate,ActivityName,LimitedTimeDiscountId,productws,Discount,Minus,FinalPrice";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_DiscountProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public DbQueryResult GetDiscountProducted(ProductQuery query, int discountId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            builder.AppendFormat(" AND SaleStatus <> ({0})", 0);
            builder.AppendFormat(" AND SaleStatus <> ({0})", 2);
            builder.AppendFormat(" AND LimitedTimeDiscountId = ({0})", discountId);
            builder.AppendFormat(" AND Status!= 2", new object[0]);
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (query.CategoryId.HasValue)
            {
                if (query.CategoryId.Value > 0)
                {
                    builder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%') ", query.MaiCategoryPath);
                }
                else
                {
                    builder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
                }
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_DiscountProducted p", "ProductId", builder.ToString(), "*");
        }

        public LimitedTimeDiscountProductInfo GetDiscountProductInfoById(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_LimitedTimeDiscountProduct WHERE LimitedTimeDiscountProductId = @LimitedTimeDiscountProductId");
            this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountProductId", DbType.Int32, id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<LimitedTimeDiscountProductInfo>(reader);
            }
        }

        public LimitedTimeDiscountProductInfo GetDiscountProductInfoByProductId(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_LimitedTimeDiscountProduct WHERE ProductId = @productId and Status=1 and BeginTime <=GETDATE() and EndTime>=GETDATE() ");
            this.database.AddInParameter(sqlStringCommand, "productId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<LimitedTimeDiscountProductInfo>(reader);
            }
        }

        public DbQueryResult GetDiscountQuery(ActivitySearch query)
        {
            StringBuilder builder = new StringBuilder("1=1 ");
            if (query.status != ActivityStatus.All)
            {
                if (query.status == ActivityStatus.In)
                {
                    builder.AppendFormat("and [BeginTime] <= '{0}' and  [EndTime] >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (query.status == ActivityStatus.End)
                {
                    builder.AppendFormat("and [EndTime] < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (query.status == ActivityStatus.unBegin)
                {
                    builder.AppendFormat("and [BeginTime] > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            if (query.begin.HasValue)
            {
                builder.AppendFormat("and [BeginTime] >= '{0}'", query.begin.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (query.end.HasValue)
            {
                builder.AppendFormat("and [EndTime] <= '{0}'", query.end.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                builder.AppendFormat("and ActivityName like '%{0}%'  ", query.Name);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_hishop_DiscountList", "LimitedTimeDiscountId", builder.ToString(), "*");
        }

        public string GetLimitedTimeDiscountIdByProductId(int userid, string skuId, int productId)
        {
            int num = 0;
            LimitedTimeDiscountProductInfo limitedTimeDiscountProductByProductId = this.GetLimitedTimeDiscountProductByProductId(productId);
            if (limitedTimeDiscountProductByProductId != null)
            {
                int limitedTimeDiscountId = limitedTimeDiscountProductByProductId.LimitedTimeDiscountId;
                LimitedTimeDiscountInfo discountInfo = new LimitedTimeDiscountDao().GetDiscountInfo(limitedTimeDiscountId);
                if (discountInfo != null)
                {
                    bool flag = false;
                    if (new MemberDao().CheckCurrentMemberIsInRange(discountInfo.ApplyMembers, discountInfo.DefualtGroup, discountInfo.CustomGroup, userid))
                    {
                        int limitNumber = discountInfo.LimitNumber;
                        if (limitNumber != 0)
                        {
                            int num4 = limitNumber - new ShoppingCartDao().GetLimitedTimeDiscountUsedNum(limitedTimeDiscountId, skuId, productId, userid, true);
                            if (num4 > 0)
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        num = limitedTimeDiscountId;
                    }
                }
            }
            return num.ToString();
        }

        public LimitedTimeDiscountProductInfo GetLimitedTimeDiscountProductByLimitIdAndProductId(int limitedTimeDiscountId, int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_LimitedTimeDiscountProduct WHERE LimitedTimeDiscountId = @LimitedTimeDiscountId and ProductId=@ProductId and Status=1 and BeginTime<=getdate() and getdate()< EndTime order by LimitedTimeDiscountProductId desc");
            this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountId", DbType.Int32, limitedTimeDiscountId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<LimitedTimeDiscountProductInfo>(reader);
            }
        }

        public LimitedTimeDiscountProductInfo GetLimitedTimeDiscountProductByLimitIdAndProductIdAndUserId(int limitedTimeDiscountId, int productId, int userId)
        {
            LimitedTimeDiscountProductInfo info = null;
            LimitedTimeDiscountProductInfo limitedTimeDiscountProductByLimitIdAndProductId = this.GetLimitedTimeDiscountProductByLimitIdAndProductId(limitedTimeDiscountId, productId);
            if (limitedTimeDiscountProductByLimitIdAndProductId != null)
            {
                LimitedTimeDiscountInfo discountInfo = this.GetDiscountInfo(limitedTimeDiscountProductByLimitIdAndProductId.LimitedTimeDiscountId);
                if ((discountInfo != null) && new MemberDao().CheckCurrentMemberIsInRange(discountInfo.ApplyMembers, discountInfo.DefualtGroup, discountInfo.CustomGroup, userId))
                {
                    info = limitedTimeDiscountProductByLimitIdAndProductId;
                }
            }
            return info;
        }

        public LimitedTimeDiscountProductInfo GetLimitedTimeDiscountProductByProductId(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_LimitedTimeDiscountProduct WHERE ProductId=@ProductId and Status=1 and BeginTime<=getdate() and getdate()< EndTime order by LimitedTimeDiscountProductId desc");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<LimitedTimeDiscountProductInfo>(reader);
            }
        }

        public bool UpdateDiscountStatus(int Id, DiscountStatus status)
        {
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_LimitedTimeDiscount set Status=@Status WHERE LimitedTimeDiscountId = @Id");
                this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
                this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, (int) status);
                DbCommand command = null;
                if (DiscountStatus.Delete == status)
                {
                    command = this.database.GetSqlStringCommand("delete Hishop_LimitedTimeDiscountProduct WHERE LimitedTimeDiscountId = @Id");
                    this.database.AddInParameter(command, "Id", DbType.Int32, Id);
                }
                else
                {
                    command = this.database.GetSqlStringCommand("update Hishop_LimitedTimeDiscountProduct set Status=@Status WHERE LimitedTimeDiscountId = @Id");
                    this.database.AddInParameter(command, "Id", DbType.Int32, Id);
                    this.database.AddInParameter(command, "Status", DbType.Int32, (int) status);
                }
                try
                {
                    this.database.ExecuteNonQuery(sqlStringCommand, transaction);
                    this.database.ExecuteNonQuery(command, transaction);
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

        public bool UpdateLimitedTimeDiscount(LimitedTimeDiscountInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  [Hishop_LimitedTimeDiscount] SET   [ActivityName]=@ActivityName,[BeginTime]=@BeginTime,[EndTime]=@EndTime,[Description]=@Description,[LimitNumber]=@LimitNumber,[ApplyMembers]=@ApplyMembers,[DefualtGroup]=@DefualtGroup,[CustomGroup]=@CustomGroup,[CreateTime]=@CreateTime,[Status]=@Status,[IsCommission]=@IsCommission,[CommissionDiscount]=@CommissionDiscount  WHERE LimitedTimeDiscountId=@LimitedTimeDiscountId");
            this.database.AddInParameter(sqlStringCommand, "ActivityName", DbType.String, info.ActivityName);
            this.database.AddInParameter(sqlStringCommand, "BeginTime", DbType.DateTime, info.BeginTime);
            this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, info.EndTime);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, info.Description);
            this.database.AddInParameter(sqlStringCommand, "LimitNumber", DbType.Int32, info.LimitNumber);
            this.database.AddInParameter(sqlStringCommand, "ApplyMembers", DbType.String, info.ApplyMembers);
            this.database.AddInParameter(sqlStringCommand, "DefualtGroup", DbType.String, info.DefualtGroup);
            this.database.AddInParameter(sqlStringCommand, "CustomGroup", DbType.String, info.CustomGroup);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, info.CreateTime);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, info.Status);
            this.database.AddInParameter(sqlStringCommand, "IsCommission", DbType.Boolean, info.IsCommission);
            this.database.AddInParameter(sqlStringCommand, "CommissionDiscount", DbType.Decimal, info.CommissionDiscount);
            this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountId", DbType.Int32, info.LimitedTimeDiscountId);
            bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            if (flag)
            {
                DbCommand command = this.database.GetSqlStringCommand("UPDATE  [Hishop_LimitedTimeDiscountProduct] SET  [BeginTime]=@BeginTime,[EndTime]=@EndTime   WHERE LimitedTimeDiscountId=@LimitedTimeDiscountId");
                this.database.AddInParameter(command, "BeginTime", DbType.DateTime, info.BeginTime);
                this.database.AddInParameter(command, "EndTime", DbType.DateTime, info.EndTime);
                this.database.AddInParameter(command, "LimitedTimeDiscountId", DbType.Int32, info.LimitedTimeDiscountId);
                this.database.ExecuteNonQuery(command);
            }
            return flag;
        }

        public bool UpdateLimitedTimeDiscountProduct(LimitedTimeDiscountProductInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  [Hishop_LimitedTimeDiscountProduct] SET   [Discount]=@Discount,[Minus]=@Minus,[IsDehorned]=@IsDehorned,[IsChamferPoint]=@IsChamferPoint,[FinalPrice]=@FinalPrice   WHERE LimitedTimeDiscountId=@LimitedTimeDiscountId and ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "Discount", DbType.Decimal, info.Discount);
            this.database.AddInParameter(sqlStringCommand, "Minus", DbType.Decimal, info.Minus);
            this.database.AddInParameter(sqlStringCommand, "IsDehorned", DbType.Int32, info.IsDehorned);
            this.database.AddInParameter(sqlStringCommand, "IsChamferPoint", DbType.Int32, info.IsChamferPoint);
            this.database.AddInParameter(sqlStringCommand, "FinalPrice", DbType.Decimal, info.FinalPrice);
            this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountId", DbType.Int32, info.LimitedTimeDiscountId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, info.ProductId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateLimitedTimeDiscountProductById(LimitedTimeDiscountProductInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  [Hishop_LimitedTimeDiscountProduct] SET   [Discount]=@Discount,[Minus]=@Minus,[IsDehorned]=@IsDehorned,[IsChamferPoint]=@IsChamferPoint,[FinalPrice]=@FinalPrice   WHERE LimitedTimeDiscountProductId=@LimitedTimeDiscountProductId");
            this.database.AddInParameter(sqlStringCommand, "Discount", DbType.Decimal, info.Discount);
            this.database.AddInParameter(sqlStringCommand, "Minus", DbType.Decimal, info.Minus);
            this.database.AddInParameter(sqlStringCommand, "IsDehorned", DbType.Int32, info.IsDehorned);
            this.database.AddInParameter(sqlStringCommand, "IsChamferPoint", DbType.Int32, info.IsChamferPoint);
            this.database.AddInParameter(sqlStringCommand, "FinalPrice", DbType.Decimal, info.FinalPrice);
            this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountProductId", DbType.Int32, info.LimitedTimeDiscountProductId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

