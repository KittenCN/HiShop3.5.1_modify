namespace Hidistro.SqlDal.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class PhotoGalleryDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddPhote(int categoryId, string photoName, string photoPath, int fileSize)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_PhotoGallery(CategoryId, PhotoName, PhotoPath, FileSize, UploadTime, LastUpdateTime) VALUES (@CategoryId, @PhotoName, @PhotoPath, @FileSize, @UploadTime, @LastUpdateTime)");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(sqlStringCommand, "PhotoName", DbType.String, Globals.SubStr(photoName, 100, ""));
            this.database.AddInParameter(sqlStringCommand, "PhotoPath", DbType.String, photoPath);
            this.database.AddInParameter(sqlStringCommand, "FileSize", DbType.Int32, fileSize);
            this.database.AddInParameter(sqlStringCommand, "UploadTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "LastUpdateTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool AddPhotoCategory(string name)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence INT; SELECT @DisplaySequence = ISNULL(MAX(DisplaySequence), 0) + 1 FROM Hishop_PhotoCategories; INSERT Hishop_PhotoCategories (CategoryName, DisplaySequence) VALUES (@CategoryName, @DisplaySequence)");
            this.database.AddInParameter(sqlStringCommand, "CategoryName", DbType.String, name);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int AddPhotoCategory2(string name)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence INT; SELECT @DisplaySequence = ISNULL(MAX(DisplaySequence), 0) + 1 FROM Hishop_PhotoCategories; INSERT Hishop_PhotoCategories (CategoryName, DisplaySequence) VALUES (@CategoryName, @DisplaySequence);SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "CategoryName", DbType.String, name);
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }

        public bool DeletePhoto(int photoId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PhotoGallery WHERE PhotoId = @PhotoId ");
            this.database.AddInParameter(sqlStringCommand, "PhotoId", DbType.Int32, photoId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeletePhotoCategory(int categoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PhotoCategories WHERE CategoryId = @CategoryId; UPDATE Hishop_PhotoGallery SET CategoryId = 0 WHERE CategoryId = @CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int GetDefaultPhotoCount()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PhotoGallery where CategoryId=0 and TypeId=0");
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }

        public DataTable GetPhotoCategories(int type)
        {
            string str = string.Empty;
            switch (type)
            {
                case 0:
                case 1:
                    str = " where TypeId=" + type;
                    break;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT COUNT(PhotoId) FROM Hishop_PhotoGallery WHERE CategoryId = pc.CategoryId) AS PhotoCounts FROM Hishop_PhotoCategories pc " + str + " ORDER BY DisplaySequence DESC");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public int GetPhotoCount()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PhotoGallery where  TypeId=0");
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }

        public DbQueryResult GetPhotoList(string keyword, int? categoryId, Pagination page, int type)
        {
            string str = string.Empty;
            if (type != -1)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str = str + " AND ";
                }
                str = str + string.Format(" TypeId = {0}", type);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str = str + " AND ";
                }
                str = str + string.Format("PhotoName LIKE '%{0}%'", DataHelper.CleanSearchString(keyword));
            }
            if (categoryId.HasValue && ((type == 0) || ((type > 0) && (categoryId.Value > 0))))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str = str + " AND ";
                }
                str = str + string.Format(" CategoryId = {0}", categoryId.Value);
            }
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_PhotoGallery", "ProductId", str, "*");
        }

        public string GetPhotoPath(int photoId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT PhotoPath FROM Hishop_PhotoGallery WHERE PhotoId = @PhotoId");
            this.database.AddInParameter(sqlStringCommand, "PhotoId", DbType.Int32, photoId);
            return this.database.ExecuteScalar(sqlStringCommand).ToString();
        }

        public int MovePhotoType(List<int> pList, int pTypeId)
        {
            if (pList.Count <= 0)
            {
                return 0;
            }
            string str = string.Empty;
            foreach (int num in pList)
            {
                str = str + num + ",";
            }
            str = str.Remove(str.Length - 1);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_PhotoGallery SET CategoryId = @CategoryId WHERE PhotoId IN ({0})", str));
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, pTypeId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public void RenamePhoto(int photoId, string newName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PhotoGallery SET PhotoName = @PhotoName WHERE PhotoId = @PhotoId");
            this.database.AddInParameter(sqlStringCommand, "PhotoId", DbType.Int32, photoId);
            this.database.AddInParameter(sqlStringCommand, "PhotoName", DbType.String, newName);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public void ReplacePhoto(int photoId, int fileSize)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PhotoGallery SET FileSize = @FileSize, LastUpdateTime = @LastUpdateTime WHERE PhotoId = @PhotoId");
            this.database.AddInParameter(sqlStringCommand, "PhotoId", DbType.Int32, photoId);
            this.database.AddInParameter(sqlStringCommand, "FileSize", DbType.Int32, fileSize);
            this.database.AddInParameter(sqlStringCommand, "LastUpdateTime", DbType.DateTime, DateTime.Now);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public void SwapSequence(int categoryId1, int categoryId2)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence1 INT , @DisplaySequence2 INT;  SELECT @DisplaySequence1 = DisplaySequence FROM Hishop_PhotoCategories WHERE CategoryId = @CategoryId1; SELECT @DisplaySequence2 = DisplaySequence FROM Hishop_PhotoCategories WHERE CategoryId = @CategoryId2; UPDATE Hishop_PhotoCategories SET DisplaySequence = @DisplaySequence1 WHERE CategoryId = @CategoryId2; UPDATE Hishop_PhotoCategories SET DisplaySequence = @DisplaySequence2 WHERE CategoryId = @CategoryId1");
            this.database.AddInParameter(sqlStringCommand, "CategoryId1", DbType.Int32, categoryId1);
            this.database.AddInParameter(sqlStringCommand, "CategoryId2", DbType.Int32, categoryId2);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public int UpdatePhotoCategories(Dictionary<int, string> photoCategorys)
        {
            if (photoCategorys.Count <= 0)
            {
                return 0;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            StringBuilder builder = new StringBuilder();
            foreach (int num in photoCategorys.Keys)
            {
                string str = num.ToString();
                builder.AppendFormat("UPDATE Hishop_PhotoCategories SET CategoryName = @CategoryName{0} WHERE CategoryId = {0}", str);
                this.database.AddInParameter(sqlStringCommand, "CategoryName" + str, DbType.String, photoCategorys[num]);
            }
            sqlStringCommand.CommandText = builder.ToString();
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

