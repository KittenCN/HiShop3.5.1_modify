namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class BrandCategoryDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int AddBrandCategory(BrandCategoryInfo brandCategory)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_BrandCategories;INSERT INTO Hishop_BrandCategories(BrandName, Logo, CompanyUrl,RewriteName,MetaKeywords,MetaDescription, Description, DisplaySequence) VALUES(@BrandName, @Logo, @CompanyUrl,@RewriteName,@MetaKeywords,@MetaDescription, @Description, @DisplaySequence); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "BrandName", DbType.String, brandCategory.BrandName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, brandCategory.Logo);
            this.database.AddInParameter(sqlStringCommand, "CompanyUrl", DbType.String, brandCategory.CompanyUrl);
            this.database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, brandCategory.RewriteName);
            this.database.AddInParameter(sqlStringCommand, "MetaKeywords", DbType.String, brandCategory.MetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "MetaDescription", DbType.String, brandCategory.MetaDescription);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, brandCategory.Description);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public void AddBrandProductTypes(int brandId, IList<int> productTypes)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
            this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            foreach (int num in productTypes)
            {
                this.database.SetParameterValue(sqlStringCommand, "ProductTypeId", num);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public bool BrandHvaeProducts(int brandId)
        {
            bool flag = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(ProductName) FROM Hishop_Products Where BrandId=@BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    flag = reader.GetInt32(0) > 0;
                }
            }
            return flag;
        }

        public bool DeleteBrandCategory(int brandId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_BrandCategories WHERE BrandId = @BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteBrandProductTypes(int brandId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypeBrands WHERE BrandId=@BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            try
            {
                this.database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public DataTable GetBrandCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories ORDER BY DisplaySequence");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetBrandCategories(string brandName)
        {
            string str = "1=1";
            if (!string.IsNullOrEmpty(brandName))
            {
                str = str + " AND BrandName LIKE '%" + DataHelper.CleanSearchString(brandName) + "%'";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories  WHERE " + str + " ORDER BY DisplaySequence");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public BrandCategoryInfo GetBrandCategory(int brandId)
        {
            BrandCategoryInfo info = new BrandCategoryInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories WHERE BrandId = @BrandId;SELECT * FROM Hishop_ProductTypeBrands WHERE BrandId = @BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<BrandCategoryInfo>(reader);
                IList<int> list = new List<int>();
                reader.NextResult();
                while (reader.Read())
                {
                    list.Add((int) reader["ProductTypeId"]);
                }
                info.ProductTypes = list;
            }
            return info;
        }

        public DbQueryResult Query(BrandQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1 ");
            if (!string.IsNullOrEmpty(query.Name))
            {
                builder.AppendFormat("and BrandName like '%{0}%'  ", query.Name);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_BrandCategories", "BrandId", builder.ToString(), "*");
        }

        public bool SetBrandCategoryThemes(int brandid, string themeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_BrandCategories set Theme = @Theme where BrandId = @BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandid);
            this.database.AddInParameter(sqlStringCommand, "Theme", DbType.String, themeName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_BrandCategories SET BrandName = @BrandName, Logo = @Logo, CompanyUrl = @CompanyUrl,RewriteName=@RewriteName,MetaKeywords=@MetaKeywords,MetaDescription=@MetaDescription, Description = @Description WHERE BrandId = @BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandCategory.BrandId);
            this.database.AddInParameter(sqlStringCommand, "BrandName", DbType.String, brandCategory.BrandName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, brandCategory.Logo);
            this.database.AddInParameter(sqlStringCommand, "CompanyUrl", DbType.String, brandCategory.CompanyUrl);
            this.database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, brandCategory.RewriteName);
            this.database.AddInParameter(sqlStringCommand, "MetaKeywords", DbType.String, brandCategory.MetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "MetaDescription", DbType.String, brandCategory.MetaDescription);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, brandCategory.Description);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void UpdateBrandCategoryDisplaySequence(int brandId, SortAction action)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_BrandCategory_DisplaySequence");
            this.database.AddInParameter(storedProcCommand, "BrandId", DbType.Int32, brandId);
            this.database.AddInParameter(storedProcCommand, "Sort", DbType.Int32, action);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public bool UpdateBrandCategoryDisplaySequence(int brandId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_BrandCategories set DisplaySequence=@DisplaySequence where BrandId=@BrandId");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@BrandId", DbType.Int32, brandId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

