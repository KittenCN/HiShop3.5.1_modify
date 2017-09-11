namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SqlDal.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ProductBrowseDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public DataTable GetActiviOne(int ActivitiesType, decimal MeetMoney)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Concat(new object[] { "select top 1 ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 and (ActivitiesType=0 or ActivitiesType=", ActivitiesType, ") and MeetMoney<=", MeetMoney, "  order by MeetMoney desc" }));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetActivitie(int ActivitiesId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType,ActivitiesDescription from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0  and ActivitiesId=" + ActivitiesId + "  order by MeetMoney asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetAllFull(int ActivitiesType)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType,ActivitiesDescription from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 and (ActivitiesType=0 or ActivitiesType=" + ActivitiesType + ")  order by MeetMoney asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetBrandProducts(MemberInfo member, int? brandId, int pageNumber, int maxNum, out int total)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            builder.Append("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,ShowSaleCounts,");
            builder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,");
            if (member != null)
            {
                discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                builder.Append("SalePrice");
            }
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(" SaleStatus=1");
            if (brandId.HasValue)
            {
                builder2.AppendFormat(" AND BrandId = {0}", brandId);
            }
            DbQueryResult result = DataHelper.PagingByRownumber(pageNumber, maxNum, "DisplaySequence", SortAction.Desc, true, "vw_Hishop_BrowseProductList s", "ProductId", builder2.ToString(), builder.ToString());
            DataTable data = (DataTable) result.Data;
            total = result.TotalRecords;
            return data;
        }

        public ProductInfo GetProduct(MemberInfo member, int productId)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            if (member != null)
            {
                discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                builder.Append("SalePrice");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId =@ProductId;SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice, " + builder.ToString() + " FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            ProductInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProduct(reader);
                }
                if (!reader.NextResult())
                {
                    return info;
                }
                while (reader.Read())
                {
                    info.Skus.Add((string) reader["SkuId"], DataMapper.PopulateSKU(reader));
                }
            }
            return info;
        }

        public DataTable GetProducts(MemberInfo member, int? topicId, int? categoryId, int distributorId, string keyWord, int pageNumber, int maxNum, out int toal, string sort, bool isAsc = false, bool isselect = false)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", maxNum);
            builder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,");
            if (member != null)
            {
                discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = vw_Hishop_BrowseProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = vw_Hishop_BrowseProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                builder.Append("SalePrice");
            }
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(" SaleStatus=1");
            if (categoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
                if (category != null)
                {
                    builder2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                builder2.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", keyWord);
            }
            if (isselect)
            {
                builder2.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", distributorId);
            }
            else
            {
                builder2.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", distributorId);
            }
            if (string.IsNullOrWhiteSpace(sort))
            {
                sort = "ProductId";
            }
            DbQueryResult result = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Hishop_BrowseProductList", "ProductId", builder2.ToString(), builder.ToString());
            DataTable data = (DataTable) result.Data;
            toal = result.TotalRecords;
            return data;
        }

        public DataTable GetProducts(MemberInfo member, int? topicId, int? categoryId, int distributorId, string keyWord, int pageNumber, int maxNum, out int toal, string sort, bool isAsc = false, string pIds = "", bool isLimitedTimeDiscountId = false)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,MaxShowPrice,", maxNum);
            builder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,");
            if (member != null)
            {
                discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = vw_Hishop_BrowseProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = vw_Hishop_BrowseProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                builder.Append("SalePrice");
            }
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(" SaleStatus=1");
            if (categoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
                if (category != null)
                {
                    builder2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                builder2.AppendFormat(" AND (ProductName LIKE '%{0}%')", keyWord);
            }
            if (!string.IsNullOrEmpty(pIds))
            {
                string str = pIds.Replace("_", ",");
                builder2.AppendFormat(" AND ProductId IN ({0})", str);
            }
            if (isLimitedTimeDiscountId)
            {
                builder2.Append(" AND ProductId in(select ProductId from Hishop_LimitedTimeDiscountProduct where BeginTime<getdate() and getdate()<EndTime and Status=1)");
            }
            if (string.IsNullOrWhiteSpace(sort))
            {
                sort = "ProductId";
            }
            DbQueryResult result = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Hishop_BrowseProductList", "ProductId", builder2.ToString(), builder.ToString());
            DataTable data = (DataTable) result.Data;
            toal = result.TotalRecords;
            return data;
        }

        public int GetProductsNumber(int distributorId, bool isselect = true)
        {
            int result = 0;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select count(ProductId) as num from vw_Hishop_BrowseProductList where  SaleStatus=1 and ProductId ", new object[0]);
            if (!isselect)
            {
                builder.AppendFormat("NOT", new object[0]);
            }
            builder.AppendFormat(" IN(select ProductId from Hishop_DistributorProducts where UserId={0} )", distributorId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(), out result);
            return result;
        }

        public DataTable GetType()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select distinct ActivitiesType  from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
    }
}

