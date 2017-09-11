namespace Hidistro.SqlDal.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class CustomPageDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int Create(CustomPage page)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_CustomPage (Name, Status, FormalJson, DraftJson,TempIndexName,PageUrl,IsShowMenu,Details,CreateTime,PV, DraftName, DraftDetails, DraftPageUrl, DraftIsShowMenu) VALUES (@Name, @Status, @FormalJson, @DraftJson,@TempIndexName,@PageUrl,@IsShowMenu,@Details,@CreateTime,@PV, @DraftName, @DraftDetails, @DraftPageUrl, @DraftIsShowMenu) SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, page.Name);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, page.Status);
            this.database.AddInParameter(sqlStringCommand, "FormalJson", DbType.String, page.FormalJson);
            this.database.AddInParameter(sqlStringCommand, "DraftJson", DbType.String, page.DraftJson);
            this.database.AddInParameter(sqlStringCommand, "TempIndexName", DbType.String, page.TempIndexName);
            this.database.AddInParameter(sqlStringCommand, "PageUrl", DbType.String, page.PageUrl);
            this.database.AddInParameter(sqlStringCommand, "Details", DbType.String, page.Details);
            this.database.AddInParameter(sqlStringCommand, "IsShowMenu", DbType.Boolean, page.IsShowMenu);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, page.CreateTime);
            this.database.AddInParameter(sqlStringCommand, "PV", DbType.Int32, page.PV);
            this.database.AddInParameter(sqlStringCommand, "DraftName", DbType.String, page.DraftName);
            this.database.AddInParameter(sqlStringCommand, "DraftDetails", DbType.String, page.DraftDetails);
            this.database.AddInParameter(sqlStringCommand, "DraftPageUrl", DbType.String, page.DraftPageUrl);
            this.database.AddInParameter(sqlStringCommand, "DraftIsShowMenu", DbType.Boolean, page.DraftIsShowMenu);
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }

        public bool DeletePage(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_CustomPage WHERE Id = @Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public CustomPage GetCustomDraftPageByPath(string path)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_CustomPage WHERE DraftPageUrl = @DraftPageUrl");
            this.database.AddInParameter(sqlStringCommand, "DraftPageUrl", DbType.String, path);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CustomPage>(reader);
            }
        }

        public CustomPage GetCustomPageByID(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CustomPage WHERE Id = @Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CustomPage>(reader);
            }
        }

        public CustomPage GetCustomPageByPath(string path)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_CustomPage WHERE PageUrl = @PageUrl");
            this.database.AddInParameter(sqlStringCommand, "PageUrl", DbType.String, path);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CustomPage>(reader);
            }
        }

        public DbQueryResult GetPages(CustomPageQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" Status={0} ", query.Status);
            if (!string.IsNullOrEmpty(query.Name))
            {
                builder.AppendFormat(" And Name LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
            }
            if (query.Status.HasValue)
            {
                builder.AppendFormat(" And Status = {0}", query.Status.Value);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_CustomPage", "Id", builder.ToString(), "*");
        }

        public bool Update(CustomPage page)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_CustomPage SET Name = @Name, TempIndexName = @TempIndexName,PageUrl = @PageUrl,IsShowMenu=@IsShowMenu,Details=@Details,Status = @Status, FormalJson = @FormalJson, DraftJson = @DraftJson,PV=@PV,DraftName=@DraftName,DraftDetails=@DraftDetails,DraftPageUrl=@DraftPageUrl,DraftIsShowMenu=@DraftIsShowMenu WHERE Id = @Id");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, page.Name);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, page.Status);
            this.database.AddInParameter(sqlStringCommand, "FormalJson", DbType.String, page.FormalJson);
            this.database.AddInParameter(sqlStringCommand, "DraftJson", DbType.String, page.DraftJson);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, page.Id);
            this.database.AddInParameter(sqlStringCommand, "TempIndexName", DbType.String, page.TempIndexName);
            this.database.AddInParameter(sqlStringCommand, "IsShowMenu", DbType.Boolean, page.IsShowMenu);
            this.database.AddInParameter(sqlStringCommand, "Details", DbType.String, page.Details);
            this.database.AddInParameter(sqlStringCommand, "PageUrl", DbType.String, page.PageUrl);
            this.database.AddInParameter(sqlStringCommand, "PV", DbType.Int32, page.PV);
            this.database.AddInParameter(sqlStringCommand, "DraftName", DbType.String, page.DraftName);
            this.database.AddInParameter(sqlStringCommand, "DraftDetails", DbType.String, page.DraftDetails);
            this.database.AddInParameter(sqlStringCommand, "DraftPageUrl", DbType.String, page.DraftPageUrl);
            this.database.AddInParameter(sqlStringCommand, "DraftIsShowMenu", DbType.Boolean, page.DraftIsShowMenu);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

