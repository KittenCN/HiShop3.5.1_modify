namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class ProductTypeDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int AddProductType(ProductTypeInfo productType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypes(TypeName, Remark) VALUES (@TypeName, @Remark); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, productType.TypeName);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, productType.Remark);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public void AddProductTypeBrands(int typeId, IList<int> brands)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
            this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32);
            foreach (int num in brands)
            {
                this.database.SetParameterValue(sqlStringCommand, "BrandId", num);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public bool DeleteProductTypeBrands(int typeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypeBrands WHERE ProductTypeId=@ProductTypeId");
            this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
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

        public bool DeleteProducType(int typeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypes WHERE TypeId = @TypeId AND not exists (SELECT productId FROM Hishop_Products WHERE TypeId = @TypeId)");
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DataTable GetBrandCategoriesByTypeId(int typeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT B.BrandId,B.BrandName FROM Hishop_BrandCategories B INNER JOIN Hishop_ProductTypeBrands PB ON B.BrandId=PB.BrandId WHERE PB.ProductTypeId=@ProductTypeId ORDER BY B.DisplaySequence DESC");
            this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public string GetBrandName(int typeId)
        {
            DataTable table;
            string str = "";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select BrandName from vw_Hishop_BrandTypeAndBrandCategories where ProductTypeId=@ProductTypeId");
            this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    str = str + table.Rows[i]["BrandName"] + ",";
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        public ProductTypeInfo GetProductType(int typeId)
        {
            ProductTypeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypes WHERE TypeId = @TypeId;SELECT * FROM Hishop_ProductTypeBrands WHERE ProductTypeId = @TypeId");
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<ProductTypeInfo>(reader);
                reader.NextResult();
                while (reader.Read())
                {
                    info.Brands.Add((int) reader["BrandId"]);
                }
            }
            return info;
        }

        public IList<ProductTypeInfo> GetProductTypes()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypes");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<ProductTypeInfo>(reader);
            }
        }

        public DbQueryResult GetProductTypes(ProductTypeQuery query)
        {
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_ProductTypes", "TypeId", string.IsNullOrEmpty(query.TypeName) ? string.Empty : string.Format("TypeName LIKE '%{0}%'", DataHelper.CleanSearchString(query.TypeName)), "*");
        }

        public int GetTypeId(string typeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TypeId FROM Hishop_ProductTypes where TypeName = @TypeName");
            this.database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, typeName);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return (int) obj2;
            }
            return 0;
        }

        public bool UpdateProductType(ProductTypeInfo productType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ProductTypes SET TypeName = @TypeName, Remark = @Remark WHERE TypeId = @TypeId");
            this.database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, productType.TypeName);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, productType.Remark);
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, productType.TypeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

