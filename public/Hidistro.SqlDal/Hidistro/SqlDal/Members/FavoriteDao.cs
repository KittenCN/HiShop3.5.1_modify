namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class FavoriteDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddProductToFavorite(int productId, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Favorite(ProductId, UserId, Tags, Remark)VALUES(@ProductId, @UserId, '', '')");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CheckHasCollect(int memberId, int productId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT COUNT(1)");
            builder.AppendFormat(" FROM Hishop_Favorite WHERE UserId={0} AND ProductId ={1} ", memberId, productId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            int num = (int) this.database.ExecuteScalar(sqlStringCommand);
            return (num > 0);
        }

        public int DeleteFavorite(int favoriteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Favorite WHERE FavoriteId = @FavoriteId");
            this.database.AddInParameter(sqlStringCommand, "FavoriteId", DbType.Int32, favoriteId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool DeleteFavorites(string ids)
        {
            string query = "DELETE from Hishop_Favorite WHERE FavoriteId IN (" + ids + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool ExistsProduct(int productId, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_Favorite WHERE UserId=@UserId AND ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public DataTable GetFavorites(MemberInfo member)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT a.*, b.ProductName, b.ThumbnailUrl60, b.MarketPrice,b.ShortDescription,");
            if (member != null)
            {
                discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = b.SkuId AND GradeId = {0}) = 1", member.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = b.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                builder.Append("SalePrice");
            }
            builder.AppendFormat(" FROM Hishop_Favorite a left join vw_Hishop_BrowseProductList b on a.ProductId = b.ProductId WHERE a.UserId={0} ORDER BY a.FavoriteId DESC", member.UserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public int UpdateFavorite(int favoriteId, string tags, string remark)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Favorite SET Tags = @Tags, Remark = @Remark WHERE FavoriteId = @FavoriteId");
            this.database.AddInParameter(sqlStringCommand, "Tags", DbType.String, tags);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "FavoriteId", DbType.Int32, favoriteId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

