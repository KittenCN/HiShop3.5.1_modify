namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SqlDal.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class HomeProductDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddHomeProdcut(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_HomeProducts(ProductId,DisplaySequence) VALUES (@ProductId,0)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            try
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch
            {
                return false;
            }
        }

        public DataTable GetAllFull()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select  * from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 order by MeetMoney asc    ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetHomeProducts()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select p.ProductId, ProductCode, ProductName,ShortDescription,ThumbnailUrl40,ThumbnailUrl160,ThumbnailUrl100,MarketPrice, SalePrice,ShowSaleCounts,SaleCounts, Stock,t.DisplaySequence from vw_Hishop_BrowseProductList p inner join  Vshop_HomeProducts t on p.productid=t.ProductId ");
            builder.AppendFormat(" and SaleStatus = {0}", 1);
            builder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DbQueryResult GetHomeProducts(MemberInfo member, ProductQuery query, bool isdistributor)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            int currentDistributorId = Globals.GetCurrentDistributorId();
            builder.Append("MainCategoryPath,ProductId, ProductCode,ShortDescription,ProductName,ShowSaleCounts, ThumbnailUrl60,ThumbnailUrl40,ThumbnailUrl100,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310, MarketPrice,");
            if (member != null)
            {
                discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", member.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice, ", member.GradeId, discount);
            }
            else
            {
                builder.Append("SalePrice,");
            }
            builder.Append("SaleCounts, Stock");
            StringBuilder builder2 = new StringBuilder(" SaleStatus =" + 1);
            if (query.CategoryId > 0)
            {
                builder2.AppendFormat(" and CategoryId={0}", query.CategoryId);
            }
            if (isdistributor && (currentDistributorId > 0))
            {
                builder2.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", currentDistributorId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder2.ToString(), builder.ToString());
        }

        public DbQueryResult GetProducts(ProductQuery query)
        {
            query.IsIncludeHomeProduct = false;
            return new ProductDao().GetProducts(query);
        }

        public bool RemoveAllHomeProduct()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_HomeProducts");
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool RemoveHomeProduct(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_HomeProducts WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateHomeProductSequence(int ProductId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Vshop_HomeProducts  set DisplaySequence=@DisplaySequence where ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@ProductId", DbType.Int32, ProductId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

