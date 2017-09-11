namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class CategoryDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int CreateCategory(CategoryInfo category)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Category_Create");
            this.database.AddOutParameter(storedProcCommand, "CategoryId", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "Name", DbType.String, category.Name);
            this.database.AddInParameter(storedProcCommand, "SKUPrefix", DbType.String, category.SKUPrefix);
            this.database.AddInParameter(storedProcCommand, "DisplaySequence", DbType.Int32, category.DisplaySequence);
            if (!string.IsNullOrEmpty(category.IconUrl))
            {
                this.database.AddInParameter(storedProcCommand, "IconUrl", DbType.String, category.IconUrl);
            }
            if (!string.IsNullOrEmpty(category.MetaTitle))
            {
                this.database.AddInParameter(storedProcCommand, "Meta_Title", DbType.String, category.MetaTitle);
            }
            if (!string.IsNullOrEmpty(category.MetaDescription))
            {
                this.database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, category.MetaDescription);
            }
            if (!string.IsNullOrEmpty(category.MetaKeywords))
            {
                this.database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, category.MetaKeywords);
            }
            if (!string.IsNullOrEmpty(category.Notes1))
            {
                this.database.AddInParameter(storedProcCommand, "Notes1", DbType.String, category.Notes1);
            }
            if (!string.IsNullOrEmpty(category.Notes2))
            {
                this.database.AddInParameter(storedProcCommand, "Notes2", DbType.String, category.Notes2);
            }
            if (!string.IsNullOrEmpty(category.Notes3))
            {
                this.database.AddInParameter(storedProcCommand, "Notes3", DbType.String, category.Notes3);
            }
            if (!string.IsNullOrEmpty(category.Notes4))
            {
                this.database.AddInParameter(storedProcCommand, "Notes4", DbType.String, category.Notes4);
            }
            if (!string.IsNullOrEmpty(category.Notes5))
            {
                this.database.AddInParameter(storedProcCommand, "Notes5", DbType.String, category.Notes5);
            }
            if (category.ParentCategoryId.HasValue)
            {
                this.database.AddInParameter(storedProcCommand, "ParentCategoryId", DbType.Int32, category.ParentCategoryId.Value);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "ParentCategoryId", DbType.Int32, 0);
            }
            if (category.AssociatedProductType.HasValue)
            {
                this.database.AddInParameter(storedProcCommand, "AssociatedProductType", DbType.Int32, category.AssociatedProductType.Value);
            }
            if (!string.IsNullOrEmpty(category.RewriteName))
            {
                this.database.AddInParameter(storedProcCommand, "RewriteName", DbType.String, category.RewriteName);
            }
            this.database.AddInParameter(storedProcCommand, "FirstCommission", DbType.String, category.FirstCommission);
            this.database.AddInParameter(storedProcCommand, "SecondCommission", DbType.String, category.SecondCommission);
            this.database.AddInParameter(storedProcCommand, "ThirdCommission", DbType.String, category.ThirdCommission);
            this.database.ExecuteNonQuery(storedProcCommand);
            return (int) this.database.GetParameterValue(storedProcCommand, "CategoryId");
        }

        public bool DeleteCategory(int categoryId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Category_Delete");
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            return (this.database.ExecuteNonQuery(storedProcCommand) > 0);
        }

        public int DisplaceCategory(int oldCategoryId, int newCategory)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId=@newCategory, MainCategoryPath=(SELECT Path FROM Hishop_Categories WHERE CategoryId=@newCategory)+'|' WHERE CategoryId=@oldCategoryId");
            this.database.AddInParameter(sqlStringCommand, "oldCategoryId", DbType.Int32, oldCategoryId);
            this.database.AddInParameter(sqlStringCommand, "newCategory", DbType.Int32, newCategory);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public DataTable GetCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CategoryId,Name,IconUrl,DisplaySequence,ParentCategoryId,Depth,[Path],RewriteName,HasChildren,FirstCommission,SecondCommission,ThirdCommission FROM Hishop_Categories ORDER BY DisplaySequence");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public CategoryInfo GetCategory(int categoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Categories WHERE CategoryId =@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CategoryInfo>(reader);
            }
        }

        public DataSet GetCategoryList()
        {
            string query = "select * from Hishop_Categories where ParentCategoryId=0 order by DisplaySequence asc,CategoryId asc ;";
            query = query + "select * from Hishop_Categories where ParentCategoryId<>0 order by DisplaySequence asc,CategoryId asc;";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["CategoryId"];
            DataColumn childColumn = set.Tables[1].Columns["ParentCategoryId"];
            DataRelation relation = new DataRelation("SubCategories", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public bool IsExitProduct(string CategoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(ProductId) FROM Hishop_Products WHERE CategoryId = @CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.String, CategoryId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public DbQueryResult Query(CategoriesQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            if (!string.IsNullOrEmpty(query.Name))
            {
                builder.AppendFormat("and Name like '%{0}%'  ", query.Name);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, 0x2710, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Categories", "VoteId", builder.ToString(), "*");
        }

        public bool SetCategoryThemes(int categoryId, string themeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Categories SET Theme = @Theme WHERE CategoryId = @CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(sqlStringCommand, "Theme", DbType.String, themeName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool SetProductExtendCategory(int productId, string extendCategoryPath)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET ExtendCategoryPath = @ExtendCategoryPath WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "ExtendCategoryPath", DbType.String, extendCategoryPath);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool SwapCategorySequence(int categoryId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Categories  set DisplaySequence=@DisplaySequence where CategoryId=@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@CategoryId", DbType.Int32, categoryId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public CategoryActionStatus UpdateCategory(CategoryInfo category)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Categories SET [Name] = @Name, SKUPrefix = @SKUPrefix,AssociatedProductType = @AssociatedProductType, Meta_Title=@Meta_Title,Meta_Description = @Meta_Description, IconUrl = @IconUrl,Meta_Keywords = @Meta_Keywords, Notes1 = @Notes1, Notes2 = @Notes2, Notes3 = @Notes3,  Notes4 = @Notes4, Notes5 = @Notes5, RewriteName = @RewriteName,FirstCommission=@FirstCommission,SecondCommission=@SecondCommission,ThirdCommission=@ThirdCommission WHERE CategoryId = @CategoryId;UPDATE Hishop_Categories SET FirstCommission=@FirstCommission,SecondCommission=@SecondCommission,ThirdCommission=@ThirdCommission WHERE Path like '%" + category.CategoryId + "|%'");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, category.CategoryId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, category.Name);
            this.database.AddInParameter(sqlStringCommand, "SKUPrefix", DbType.String, category.SKUPrefix);
            this.database.AddInParameter(sqlStringCommand, "AssociatedProductType", DbType.Int32, category.AssociatedProductType);
            this.database.AddInParameter(sqlStringCommand, "Meta_Title", DbType.String, category.MetaTitle);
            this.database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, category.MetaDescription);
            this.database.AddInParameter(sqlStringCommand, "IconUrl", DbType.String, category.IconUrl);
            this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, category.MetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "Notes1", DbType.String, category.Notes1);
            this.database.AddInParameter(sqlStringCommand, "Notes2", DbType.String, category.Notes2);
            this.database.AddInParameter(sqlStringCommand, "Notes3", DbType.String, category.Notes3);
            this.database.AddInParameter(sqlStringCommand, "Notes4", DbType.String, category.Notes4);
            this.database.AddInParameter(sqlStringCommand, "Notes5", DbType.String, category.Notes5);
            this.database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, category.RewriteName);
            this.database.AddInParameter(sqlStringCommand, "FirstCommission", DbType.String, category.FirstCommission);
            this.database.AddInParameter(sqlStringCommand, "SecondCommission", DbType.String, category.SecondCommission);
            this.database.AddInParameter(sqlStringCommand, "ThirdCommission", DbType.String, category.ThirdCommission);
            if (this.database.ExecuteNonQuery(sqlStringCommand) < 1)
            {
                return CategoryActionStatus.UnknowError;
            }
            return CategoryActionStatus.Success;
        }
    }
}

