namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;

    public class TaobaoProductDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public DataSet GetTaobaoProductDetails(int productId)
        {
            DataTable table;
            DataTable table2;
            DataTable table3;
            DataTable table4;
            DataTable table5;
            DataSet set = new DataSet();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId, HasSKU, ProductName, ProductCode, MarketPrice, (SELECT [Name] FROM Hishop_Categories WHERE CategoryId = p.CategoryId) AS CategoryName, (SELECT [Name] FROM Hishop_ProductLines WHERE LineId = p.LineId) AS ProductLineName, (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId = p.BrandId) AS BrandName, (SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice, (SELECT MIN(CostPrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS CostPrice, (SELECT SUM(Stock) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Stock FROM Hishop_Products p WHERE ProductId = @ProductId SELECT AttributeName, ValueStr FROM Hishop_ProductAttributes pa join Hishop_Attributes a ON pa.AttributeId = a.AttributeId JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC SELECT Weight AS '重量', Stock AS '库存', CostPrice AS '成本价', SalePrice AS '一口价', SkuId AS '商品编码' FROM Hishop_SKUs s WHERE ProductId = @ProductId; SELECT SkuId AS '商品编码',AttributeName,UseAttributeImage,ValueStr,ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC SELECT * FROM Taobao_Products WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table2 = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table3 = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table4 = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table5 = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if (((table3 != null) && (table3.Rows.Count > 0)) && ((table4 != null) && (table4.Rows.Count > 0)))
            {
                foreach (DataRow row in table4.Rows)
                {
                    DataColumn column = new DataColumn {
                        ColumnName = (string) row["AttributeName"]
                    };
                    if (!table3.Columns.Contains(column.ColumnName))
                    {
                        table3.Columns.Add(column);
                    }
                }
                foreach (DataRow row2 in table3.Rows)
                {
                    foreach (DataRow row3 in table4.Rows)
                    {
                        if (string.Compare((string) row2["商品编码"], (string) row3["商品编码"]) == 0)
                        {
                            row2[(string) row3["AttributeName"]] = row3["ValueStr"];
                        }
                    }
                }
            }
            set.Tables.Add(table);
            set.Tables.Add(table2);
            set.Tables.Add(table3);
            set.Tables.Add(table5);
            return set;
        }

        public bool IsExitTaobaoProduct(long taobaoProductId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT COUNT(*) FROM Hishop_Products WHERE TaobaoProductId = {0}", taobaoProductId));
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Taobao_Products WHERE ProductId = @ProductId;INSERT INTO Taobao_Products(Cid, StuffStatus, ProductId, ProTitle,Num, LocationState, LocationCity, FreightPayer, PostFee, ExpressFee, EMSFee, HasInvoice, HasWarranty, HasDiscount, ValidThru, ListTime, PropertyAlias,InputPids,InputStr, SkuProperties, SkuQuantities, SkuPrices, SkuOuterIds) VALUES(@Cid, @StuffStatus, @ProductId, @ProTitle,@Num, @LocationState, @LocationCity, @FreightPayer, @PostFee, @ExpressFee, @EMSFee, @HasInvoice, @HasWarranty, @HasDiscount, @ValidThru, @ListTime,@PropertyAlias,@InputPids, @InputStr, @SkuProperties, @SkuQuantities, @SkuPrices, @SkuOuterIds);");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, taobaoProduct.ProductId);
            this.database.AddInParameter(sqlStringCommand, "Cid", DbType.Int64, taobaoProduct.Cid);
            this.database.AddInParameter(sqlStringCommand, "StuffStatus", DbType.String, taobaoProduct.StuffStatus);
            this.database.AddInParameter(sqlStringCommand, "ProTitle", DbType.String, taobaoProduct.ProTitle);
            this.database.AddInParameter(sqlStringCommand, "Num", DbType.Int64, taobaoProduct.Num);
            this.database.AddInParameter(sqlStringCommand, "LocationState", DbType.String, taobaoProduct.LocationState);
            this.database.AddInParameter(sqlStringCommand, "LocationCity", DbType.String, taobaoProduct.LocationCity);
            this.database.AddInParameter(sqlStringCommand, "FreightPayer", DbType.String, taobaoProduct.FreightPayer);
            this.database.AddInParameter(sqlStringCommand, "PostFee", DbType.Currency, taobaoProduct.PostFee);
            this.database.AddInParameter(sqlStringCommand, "ExpressFee", DbType.Currency, taobaoProduct.ExpressFee);
            this.database.AddInParameter(sqlStringCommand, "EMSFee", DbType.Currency, taobaoProduct.EMSFee);
            this.database.AddInParameter(sqlStringCommand, "HasInvoice", DbType.Boolean, taobaoProduct.HasInvoice);
            this.database.AddInParameter(sqlStringCommand, "HasWarranty", DbType.Boolean, taobaoProduct.HasWarranty);
            this.database.AddInParameter(sqlStringCommand, "HasDiscount", DbType.Boolean, taobaoProduct.HasDiscount);
            this.database.AddInParameter(sqlStringCommand, "ValidThru", DbType.Int64, taobaoProduct.ValidThru);
            this.database.AddInParameter(sqlStringCommand, "ListTime", DbType.DateTime, taobaoProduct.ListTime);
            this.database.AddInParameter(sqlStringCommand, "PropertyAlias", DbType.String, taobaoProduct.PropertyAlias);
            this.database.AddInParameter(sqlStringCommand, "InputPids", DbType.String, taobaoProduct.InputPids);
            this.database.AddInParameter(sqlStringCommand, "InputStr", DbType.String, taobaoProduct.InputStr);
            this.database.AddInParameter(sqlStringCommand, "SkuProperties", DbType.String, taobaoProduct.SkuProperties);
            this.database.AddInParameter(sqlStringCommand, "SkuQuantities", DbType.String, taobaoProduct.SkuQuantities);
            this.database.AddInParameter(sqlStringCommand, "SkuPrices", DbType.String, taobaoProduct.SkuPrices);
            this.database.AddInParameter(sqlStringCommand, "SkuOuterIds", DbType.String, taobaoProduct.SkuOuterIds);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

