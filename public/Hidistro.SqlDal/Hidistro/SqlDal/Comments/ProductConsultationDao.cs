namespace Hidistro.SqlDal.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class ProductConsultationDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int DeleteProductConsultation(int consultationId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductConsultations WHERE consultationId = @consultationId");
            this.database.AddInParameter(sqlStringCommand, "ConsultationId", DbType.Int64, consultationId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.Keywords));
            if (consultationQuery.Type == ConsultationReplyType.NoReply)
            {
                builder.Append(" AND ReplyUserId IS NULL ");
            }
            else if (consultationQuery.Type == ConsultationReplyType.Replyed)
            {
                builder.Append(" AND ReplyUserId IS NOT NULL");
            }
            if (consultationQuery.ProductId > 0)
            {
                builder.AppendFormat(" AND ProductId = {0}", consultationQuery.ProductId);
            }
            if (consultationQuery.UserId > 0)
            {
                builder.AppendFormat(" AND UserId = {0}", consultationQuery.UserId);
            }
            if (!string.IsNullOrEmpty(consultationQuery.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.ProductCode));
            }
            if (consultationQuery.CategoryId.HasValue)
            {
                builder.AppendFormat(" AND (CategoryId = {0}", consultationQuery.CategoryId.Value);
                builder.AppendFormat(" OR CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '%'))", consultationQuery.CategoryId.Value);
            }
            if (consultationQuery.HasReplied.HasValue)
            {
                if (consultationQuery.HasReplied.Value)
                {
                    builder.AppendFormat(" AND ReplyText is not null", new object[0]);
                }
                else
                {
                    builder.AppendFormat(" AND ReplyText is null", new object[0]);
                }
            }
            return DataHelper.PagingByRownumber(consultationQuery.PageIndex, consultationQuery.PageSize, consultationQuery.SortBy, consultationQuery.SortOrder, consultationQuery.IsCount, "vw_Hishop_ProductConsultations", "ProductId", builder.ToString(), "*");
        }

        public ProductConsultationInfo GetProductConsultation(int consultationId)
        {
            string query = "SELECT * FROM Hishop_ProductConsultations WHERE ConsultationId=@ConsultationId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ConsultationId", DbType.Int32, consultationId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ProductConsultationInfo>(reader);
            }
        }

        public int GetProductConsultationsCount(int productId, bool includeUnReplied)
        {
            StringBuilder builder = new StringBuilder("SELECT count(1) FROM Hishop_ProductConsultations WHERE ProductId =" + productId);
            if (!includeUnReplied)
            {
                builder.Append(" AND ReplyText is not null");
            }
            return (int) this.database.ExecuteScalar(CommandType.Text, builder.ToString());
        }

        public bool InsertProductConsultation(ProductConsultationInfo productConsultation)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductConsultations(ProductId, UserId, UserName, UserEmail, ConsultationText, ConsultationDate)VALUES(@ProductId, @UserId, @UserName, @UserEmail, @ConsultationText, @ConsultationDate)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productConsultation.ProductId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, productConsultation.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, productConsultation.UserName);
            this.database.AddInParameter(sqlStringCommand, "UserEmail", DbType.String, productConsultation.UserEmail);
            this.database.AddInParameter(sqlStringCommand, "ConsultationText", DbType.String, productConsultation.ConsultationText);
            this.database.AddInParameter(sqlStringCommand, "ConsultationDate", DbType.DateTime, productConsultation.ConsultationDate);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ProductConsultations SET ReplyText = @ReplyText, ReplyDate = @ReplyDate, ReplyUserId = @ReplyUserId WHERE ConsultationId = @ConsultationId");
            this.database.AddInParameter(sqlStringCommand, "ReplyText", DbType.String, productConsultation.ReplyText);
            this.database.AddInParameter(sqlStringCommand, "ReplyDate", DbType.DateTime, productConsultation.ReplyDate);
            this.database.AddInParameter(sqlStringCommand, "ReplyUserId", DbType.Int32, productConsultation.ReplyUserId);
            this.database.AddInParameter(sqlStringCommand, "ConsultationId", DbType.Int32, productConsultation.ConsultationId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

