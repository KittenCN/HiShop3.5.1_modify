namespace Hidistro.SqlDal.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Orders;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ProductReviewDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int DeleteProductReview(long reviewId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductReviews WHERE ReviewId = @ReviewId");
            this.database.AddInParameter(sqlStringCommand, "ReviewId", DbType.Int64, reviewId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public LineItemInfo GetLatestOrderItemByProductIDAndUserID(int productid, int userid)
        {
            string query = string.Format(string.Concat(new object[] { "select top 1 a.* from Hishop_OrderItems a left join Hishop_ProductReviews b on a.skuid= b.skuid and a.orderid=b.orderid left join Hishop_Orders c on a.orderid=c.orderid where c.userid=", userid, " and a.productid=", productid, " and a.OrderItemsStatus = {0} and b.orderid is null order by a.Id desc" }), 5);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<LineItemInfo>(reader);
            }
        }

        public IList<LineItemInfo> GetOrderItemsList(int userid)
        {
            string query = string.Format("select a.* from Hishop_OrderItems a left join Hishop_ProductReviews b on a.skuid= b.skuid and a.orderid=b.orderid left join Hishop_Orders c on a.orderid=c.orderid where c.userid=" + userid + " and  a.OrderItemsStatus = {0} and b.orderid is null order by a.Id desc", 5);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<LineItemInfo>(reader);
            }
        }

        public DbQueryResult GetOrderMemberComment(OrderQuery query, int userId, int orderItemsStatus)
        {
            StringBuilder builder = new StringBuilder();
            if (userId > 0)
            {
                builder.Append(" UserId=" + userId);
            }
            if (orderItemsStatus > 0)
            {
                builder.Append(" AND OrderItemsStatus=" + orderItemsStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_OrderMmemberComment", "Id", builder.ToString(), "*");
        }

        public ProductReviewInfo GetProductReview(int reviewId)
        {
            ProductReviewInfo info = new ProductReviewInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductReviews WHERE ReviewId=@ReviewId");
            this.database.AddInParameter(sqlStringCommand, "ReviewId", DbType.Int32, reviewId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = ReaderConvert.ReaderToModel<ProductReviewInfo>(reader);
                }
            }
            return info;
        }

        public DbQueryResult GetProductReviews(ProductReviewQuery reviewQuery)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.Keywords));
            if (!string.IsNullOrEmpty(reviewQuery.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.ProductCode));
            }
            if (reviewQuery.productId > 0)
            {
                builder.AppendFormat(" AND ProductId = {0}", reviewQuery.productId);
            }
            if (reviewQuery.CategoryId.HasValue)
            {
                builder.AppendFormat(" AND (CategoryId = {0}", reviewQuery.CategoryId.Value);
                builder.AppendFormat(" OR  CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '%'))", reviewQuery.CategoryId.Value);
            }
            return DataHelper.PagingByRownumber(reviewQuery.PageIndex, reviewQuery.PageSize, reviewQuery.SortBy, reviewQuery.SortOrder, reviewQuery.IsCount, "vw_Hishop_ProductReviews", "ProductId", builder.ToString(), "*");
        }

        public int GetProductReviewsCount(int productId)
        {
            StringBuilder builder = new StringBuilder("SELECT count(1) FROM Hishop_ProductReviews WHERE ProductId =" + productId);
            return (int) this.database.ExecuteScalar(CommandType.Text, builder.ToString());
        }

        public int GetWaitCommentByUserID(int userid)
        {
            string query = string.Format("select count(0) from Hishop_OrderItems a left join Hishop_ProductReviews b on a.skuid= b.skuid and a.orderid=b.orderid left join Hishop_Orders c on a.orderid=c.orderid where c.userid=" + userid + "  and a.OrderItemsStatus = {0} and b.orderid is null", 5);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public bool InsertProductReview(ProductReviewInfo review)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductReviews (ProductId, UserId, ReviewText, UserName, UserEmail, ReviewDate,OrderID,SkuID,OrderItemID) VALUES(@ProductId, @UserId, @ReviewText, @UserName, @UserEmail, @ReviewDate,@OrderID,@SkuID,@OrderItemID)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, review.ProductId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, review.UserId);
            this.database.AddInParameter(sqlStringCommand, "ReviewText", DbType.String, review.ReviewText);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, review.UserName);
            this.database.AddInParameter(sqlStringCommand, "UserEmail", DbType.String, review.UserEmail);
            this.database.AddInParameter(sqlStringCommand, "ReviewDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "OrderID", DbType.String, review.OrderId);
            this.database.AddInParameter(sqlStringCommand, "SkuID", DbType.String, review.SkuId);
            this.database.AddInParameter(sqlStringCommand, "OrderItemID", DbType.Int32, review.OrderItemID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool IsReview(string orderid, string skuid, int productid, int userid)
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(skuid) && !string.IsNullOrEmpty(orderid))
            {
                query = "SELECT COUNT(0) FROM Hishop_ProductReviews WHERE OrderID=@OrderID AND SkuId=@SkuId AND SkuId is not null";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productid);
                this.database.AddInParameter(sqlStringCommand, "OrderID", DbType.String, orderid);
                this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuid);
                return (Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)) > 0);
            }
            return ((userid > 0) && (this.GetLatestOrderItemByProductIDAndUserID(productid, userid) == null));
        }

        public void LoadProductReview(int productId, int userId, out int buyNum, out int reviewNum)
        {
            buyNum = 0;
            reviewNum = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_ProductReviews WHERE ProductId=@ProductId AND UserId = @UserId SELECT ISNULL(SUM(Quantity), 0) FROM Hishop_OrderItems WHERE ProductId=@ProductId AND OrderId IN" + string.Format(" (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId AND OrderStatus = {0})", 5));
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    reviewNum = (int) reader[0];
                }
                reader.NextResult();
                if (reader.Read())
                {
                    buyNum = (int) reader[0];
                }
            }
        }
    }
}

