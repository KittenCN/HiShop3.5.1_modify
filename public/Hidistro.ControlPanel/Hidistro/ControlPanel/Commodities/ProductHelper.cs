namespace Hidistro.ControlPanel.Commodities
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.SqlDal.Commodities;
    using Hishop.Open.Api;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Web;

    public static class ProductHelper
    {
        public static ProductActionStatus AddProduct(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> tagsId)
        {
            if (product == null)
            {
                return ProductActionStatus.UnknowError;
            }
            Globals.EntityCoding(product, true);
            int decimalLength = SettingsManager.GetMasterSettings(true).DecimalLength;
            if (product.MarketPrice.HasValue)
            {
                product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
            }
            ProductActionStatus unknowError = ProductActionStatus.UnknowError;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    ProductDao dao = new ProductDao();
                    int productId = dao.AddProduct(product, dbTran);
                    if (productId == 0)
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.DuplicateSKU;
                    }
                    product.ProductId = productId;
                    if (((skus != null) && (skus.Count > 0)) && !dao.AddProductSKUs(productId, skus, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.SKUError;
                    }
                    if (((attrs != null) && (attrs.Count > 0)) && !dao.AddProductAttributes(productId, attrs, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.AttributeError;
                    }
                    if (((tagsId != null) && (tagsId.Count > 0)) && !new TagDao().AddProductTags(productId, tagsId, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.ProductTagEroor;
                    }
                    dbTran.Commit();
                    unknowError = ProductActionStatus.Success;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
            if (unknowError == ProductActionStatus.Success)
            {
                EventLogs.WriteOperationLog(Privilege.AddProducts, string.Format(CultureInfo.InvariantCulture, "上架了一个新商品:”{0}”", new object[] { product.ProductName }));
            }
            return unknowError;
        }

        public static string AddProductNew(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> tagsId)
        {
            string str = string.Empty;
            if (product == null)
            {
                return "未知错误";
            }
            Globals.EntityCoding(product, true);
            int decimalLength = SettingsManager.GetMasterSettings(true).DecimalLength;
            if (product.MarketPrice.HasValue)
            {
                product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
            }
            ProductActionStatus unknowError = ProductActionStatus.UnknowError;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    ProductDao dao = new ProductDao();
                    int productId = dao.AddProduct(product, dbTran);
                    if (productId == 0)
                    {
                        dbTran.Rollback();
                        return "货号重复";
                    }
                    str = productId.ToString();
                    product.ProductId = productId;
                    if (((skus != null) && (skus.Count > 0)) && !dao.AddProductSKUs(productId, skus, dbTran))
                    {
                        dbTran.Rollback();
                        return "添加SUK出错";
                    }
                    if (((attrs != null) && (attrs.Count > 0)) && !dao.AddProductAttributes(productId, attrs, dbTran))
                    {
                        dbTran.Rollback();
                        return "添加商品属性出错";
                    }
                    if (((tagsId != null) && (tagsId.Count > 0)) && !new TagDao().AddProductTags(productId, tagsId, dbTran))
                    {
                        dbTran.Rollback();
                        return "添加商品标签出错";
                    }
                    dbTran.Commit();
                    unknowError = ProductActionStatus.Success;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
            if (unknowError == ProductActionStatus.Success)
            {
                EventLogs.WriteOperationLog(Privilege.AddProducts, string.Format(CultureInfo.InvariantCulture, "上架了一个新商品:”{0}”", new object[] { product.ProductName }));
            }
            return str;
        }

        public static bool AddSkuStock(string productIds, int addStock)
        {
            return new ProductBatchDao().AddSkuStock(productIds, addStock);
        }

        public static bool CheckPrice(string productIds, int baseGradeId, decimal checkPrice, bool isMember)
        {
            return new ProductBatchDao().CheckPrice(productIds, baseGradeId, checkPrice, isMember);
        }

        private static ProductInfo ConverToProduct(DataRow productRow, int categoryId, int lineId, int? bandId, ProductSaleStatus saleStatus, bool includeImages)
        {
            ProductInfo info = new ProductInfo {
                CategoryId = categoryId,
                TypeId = new int?((int) productRow["SelectedTypeId"]),
                ProductName = (string) productRow["ProductName"],
                ProductCode = (string) productRow["ProductCode"],
                BrandId = bandId,
                Unit = (string) productRow["Unit"],
                ShortDescription = (string) productRow["ShortDescription"],
                Description = (string) productRow["Description"],
                AddedDate = DateTime.Now,
                SaleStatus = saleStatus,
                HasSKU = (bool) productRow["HasSKU"],
                MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|",
                ImageUrl1 = (string) productRow["ImageUrl1"],
                ImageUrl2 = (string) productRow["ImageUrl2"],
                ImageUrl3 = (string) productRow["ImageUrl3"],
                ImageUrl4 = (string) productRow["ImageUrl4"],
                ImageUrl5 = (string) productRow["ImageUrl5"]
            };
            if (productRow["MarketPrice"] != DBNull.Value)
            {
                info.MarketPrice = new decimal?((decimal) productRow["MarketPrice"]);
            }
            if (includeImages)
            {
                HttpContext current = HttpContext.Current;
                if (!string.IsNullOrEmpty(info.ImageUrl1) && (info.ImageUrl1.Length > 0))
                {
                    string[] strArray = ProcessImages(current, info.ImageUrl1);
                    info.ThumbnailUrl40 = strArray[0];
                    info.ThumbnailUrl60 = strArray[1];
                    info.ThumbnailUrl100 = strArray[2];
                    info.ThumbnailUrl160 = strArray[3];
                    info.ThumbnailUrl180 = strArray[4];
                    info.ThumbnailUrl220 = strArray[5];
                    info.ThumbnailUrl310 = strArray[6];
                    info.ThumbnailUrl410 = strArray[7];
                }
                if (!string.IsNullOrEmpty(info.ImageUrl2) && (info.ImageUrl2.Length > 0))
                {
                    ProcessImages(current, info.ImageUrl2);
                }
                if (!string.IsNullOrEmpty(info.ImageUrl3) && (info.ImageUrl3.Length > 0))
                {
                    ProcessImages(current, info.ImageUrl3);
                }
                if (!string.IsNullOrEmpty(info.ImageUrl4) && (info.ImageUrl4.Length > 0))
                {
                    ProcessImages(current, info.ImageUrl4);
                }
                if (!string.IsNullOrEmpty(info.ImageUrl5) && (info.ImageUrl5.Length > 0))
                {
                    ProcessImages(current, info.ImageUrl5);
                }
            }
            return info;
        }

        private static Dictionary<string, SKUItem> ConverToSkus(int mappedProductId, DataSet productData, bool includeCostPrice, bool includeStock)
        {
            DataRow[] rowArray = productData.Tables["skus"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
            if (rowArray.Length == 0)
            {
                return null;
            }
            Dictionary<string, SKUItem> dictionary = new Dictionary<string, SKUItem>();
            foreach (DataRow row in rowArray)
            {
                string key = (string) row["NewSkuId"];
                SKUItem item = new SKUItem {
                    SkuId = key,
                    SKU = (string) row["SKU"],
                    SalePrice = (decimal) row["SalePrice"]
                };
                if (row["Weight"] != DBNull.Value)
                {
                    item.Weight = (decimal) row["Weight"];
                }
                if (includeCostPrice && (row["CostPrice"] != DBNull.Value))
                {
                    item.CostPrice = (decimal) row["CostPrice"];
                }
                if (includeStock)
                {
                    item.Stock = (int) row["Stock"];
                }
                foreach (DataRow row2 in productData.Tables["skuItems"].Select("NewSkuId='" + key + "' AND MappedProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture)))
                {
                    item.SkuItems.Add((int) row2["SelectedAttributeId"], (int) row2["SelectedValueId"]);
                }
                dictionary.Add(key, item);
            }
            return dictionary;
        }

        private static Dictionary<int, IList<int>> ConvertToAttributes(int mappedProductId, DataSet productData)
        {
            DataRow[] rowArray = productData.Tables["attributes"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
            if (rowArray.Length == 0)
            {
                return null;
            }
            Dictionary<int, IList<int>> dictionary = new Dictionary<int, IList<int>>();
            foreach (DataRow row in rowArray)
            {
                int key = (int) row["SelectedAttributeId"];
                if (!dictionary.ContainsKey(key))
                {
                    IList<int> list = new List<int>();
                    dictionary.Add(key, list);
                }
                dictionary[key].Add((int) row["SelectedValueId"]);
            }
            return dictionary;
        }

        public static int DeleteProduct(string productIds, bool isDeleteImage)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
            if (string.IsNullOrEmpty(productIds))
            {
                return 0;
            }
            string[] strArray = productIds.Split(new char[] { ',' });
            IList<int> list = new List<int>();
            foreach (string str in strArray)
            {
                list.Add(int.Parse(str));
            }
            IList<ProductInfo> products = new ProductDao().GetProducts(list, false);
            int num = new ProductDao().DeleteProduct(productIds);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProducts, string.Format(CultureInfo.InvariantCulture, "删除了 “{0}” 件商品", new object[] { list.Count }));
                if (!isDeleteImage)
                {
                    return num;
                }
                foreach (ProductInfo info in products)
                {
                    try
                    {
                        DeleteProductImage(info);
                    }
                    catch
                    {
                    }
                }
            }
            return num;
        }

        private static void DeleteProductImage(ProductInfo product)
        {
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl1))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl1);
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
                if (!string.IsNullOrEmpty(product.ImageUrl2))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl2);
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
                if (!string.IsNullOrEmpty(product.ImageUrl3))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl3);
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
                if (!string.IsNullOrEmpty(product.ImageUrl4))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl4);
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
                if (!string.IsNullOrEmpty(product.ImageUrl5))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl5);
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
            }
        }

        public static void EnsureMapping(DataSet mappingSet)
        {
            new ProductDao().EnsureMapping(mappingSet);
        }

        public static ProductInfo GetBrowseProductListByView(int productId)
        {
            return new ProductDao().GetBrowseProductListByView(productId);
        }

        public static DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
        {
            return new ProductDao().GetExportProducts(query, removeProductIds);
        }

        public static DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
        {
            DataSet set = new ProductDao().GetExportProducts(query, includeCostPrice, includeStock, removeProductIds);
            set.Tables[0].TableName = "types";
            set.Tables[1].TableName = "attributes";
            set.Tables[2].TableName = "values";
            set.Tables[3].TableName = "products";
            set.Tables[4].TableName = "skus";
            set.Tables[5].TableName = "skuItems";
            set.Tables[6].TableName = "productAttributes";
            set.Tables[7].TableName = "taobaosku";
            string str = string.Empty;
            for (int i = 0; i < set.Tables[3].Rows.Count; i++)
            {
                str = (set.Tables[3].Rows[i]["Description"] == null) ? "" : set.Tables[3].Rows[i]["Description"].ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    set.Tables[3].Rows[i]["Description"] = Regex.Replace(str, "alt=\"([^\"]+)\"", "");
                }
            }
            return set;
        }

        public static DataTable GetGroupBuyProducts(ProductQuery query)
        {
            return new ProductDao().GetGroupBuyProducts(query);
        }

        public static int GetMaxSequence()
        {
            return new ProductDao().GetMaxSequence();
        }

        public static ProductInfo GetProductBaseInfo(int productId)
        {
            return new ProductBatchDao().GetProductBaseInfo(productId);
        }

        public static DataTable GetProductBaseInfo(string productIds)
        {
            return new ProductBatchDao().GetProductBaseInfo(productIds);
        }

        public static ProductInfo GetProductDetails(int productId)
        {
            return new ProductDao().GetProductDetails(productId);
        }

        public static ProductInfo GetProductDetails(int productId, out Dictionary<int, IList<int>> attrs, out IList<int> tagsId)
        {
            ProductDao dao = new ProductDao();
            attrs = dao.GetProductAttributes(productId);
            tagsId = dao.GetProductTags(productId);
            return dao.GetProductDetails(productId);
        }

        public static product_item_model GetProductForApi(int productId)
        {
            return new ProductDao().GetProductForApi(productId);
        }

        public static bool GetProductHasSku(string skuid, int quantity)
        {
            return new ProductDao().GetProductHasSku(skuid, quantity);
        }

        public static IList<int> GetProductIds(ProductQuery query)
        {
            return new ProductDao().GetProductIds(query);
        }

        public static DataTable GetProductNum()
        {
            return new ProductDao().GetProductNum();
        }

        public static DbQueryResult GetProducts(ProductQuery query)
        {
            return new ProductDao().GetProducts(query);
        }

        public static DataTable GetProducts(string products)
        {
            return new ProductDao().GetProducts(products);
        }

        public static IList<ProductInfo> GetProducts(IList<int> productIds, bool Resort = false)
        {
            return new ProductDao().GetProducts(productIds, Resort);
        }

        public static decimal GetProductSalePrice(int productId)
        {
            return new ProductDao().GetProductSalePrice(productId);
        }

        public static int GetProductsCount()
        {
            return new ProductDao().GetProductsCount();
        }

        public static int GetProductsCountByDistributor(int rid)
        {
            return new ProductDao().GetProductsCountByDistributor(rid);
        }

        public static DbQueryResult GetProductsForApi(ProductQuery query)
        {
            return new ProductDao().GetProductsForApi(query);
        }

        public static DbQueryResult GetProductsFromGroup(ProductQuery query, string productIds)
        {
            return new ProductDao().GetProductsFromGroup(query, productIds);
        }

        public static DbQueryResult GetProductsImgList(ProductQuery query)
        {
            return new ProductDao().GetProductsImgList(query);
        }

        public static long GetProductSumStock(int productId)
        {
            return new ProductDao().GetProductSumStock(productId);
        }

        public static string GetPropsForApi(int productId)
        {
            return new ProductDao().GetPropsForApi(productId);
        }

        public static bool GetSKUMemberPrice(string productIds, int gradeId)
        {
            return new ProductBatchDao().GetSKUMemberPrice(productIds, gradeId);
        }

        public static DataTable GetSkuMemberPrices(string productIds)
        {
            return new ProductBatchDao().GetSkuMemberPrices(productIds);
        }

        public static IList<product_sku_model> GetSkusForApi(int productId)
        {
            return new ProductDao().GetSkusForApi(productId);
        }

        public static DataTable GetSkuStocks(string productIds)
        {
            return new ProductBatchDao().GetSkuStocks(productIds);
        }

        public static DataSet GetTaobaoProductDetails(int productId)
        {
            return new TaobaoProductDao().GetTaobaoProductDetails(productId);
        }

        public static DataTable GetTopProductOrder(int top, string showOrder)
        {
            if (top < 1)
            {
                top = 6;
            }
            if (string.IsNullOrEmpty(showOrder))
            {
                showOrder = " ProductId DESC";
            }
            return new ProductDao().GetTopProductOrder(top, showOrder);
        }
        public static void ImportProducts(DataTable productData, int categoryId, int lineId, int? brandId, ProductSaleStatus saleStatus, bool isImportFromTaobao)
        {
            if ((productData != null) && (productData.Rows.Count > 0))
            {
                foreach (DataRow row in productData.Rows)
                {
                    string[] strArray;
                    ProductInfo product = new ProductInfo
                    {
                        CategoryId = categoryId,
                        MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|",
                        ProductName = (string)row["ProductName"],
                        ProductCode = (string)row["SKU"],
                        BrandId = brandId
                    };
                    if (row["Description"] != DBNull.Value)
                    {
                        product.Description = (string)row["Description"];
                    }
                    product.MarketPrice = new decimal?((decimal)row["SalePrice"]);
                    product.AddedDate = DateTime.Now;
                    product.SaleStatus = saleStatus;
                    product.HasSKU = false;
                    HttpContext current = HttpContext.Current;
                    if (row["ImageUrl1"] != DBNull.Value)
                    {
                        product.ImageUrl1 = (string)row["ImageUrl1"];
                    }
                    if (!(string.IsNullOrEmpty(product.ImageUrl1) || (product.ImageUrl1.Length <= 0)))
                    {
                        strArray = ProcessImages(current, product.ImageUrl1);
                        product.ThumbnailUrl40 = strArray[0];
                        product.ThumbnailUrl60 = strArray[1];
                        product.ThumbnailUrl100 = strArray[2];
                        product.ThumbnailUrl160 = strArray[3];
                        product.ThumbnailUrl180 = strArray[4];
                        product.ThumbnailUrl220 = strArray[5];
                        product.ThumbnailUrl310 = strArray[6];
                        product.ThumbnailUrl410 = strArray[7];
                    }
                    if (row["ImageUrl2"] != DBNull.Value)
                    {
                        product.ImageUrl2 = (string)row["ImageUrl2"];
                    }
                    if (!(string.IsNullOrEmpty(product.ImageUrl2) || (product.ImageUrl2.Length <= 0)))
                    {
                        strArray = ProcessImages(current, product.ImageUrl2);
                    }
                    if (row["ImageUrl3"] != DBNull.Value)
                    {
                        product.ImageUrl3 = (string)row["ImageUrl3"];
                    }
                    if (!(string.IsNullOrEmpty(product.ImageUrl3) || (product.ImageUrl3.Length <= 0)))
                    {
                        strArray = ProcessImages(current, product.ImageUrl3);
                    }
                    if (row["ImageUrl4"] != DBNull.Value)
                    {
                        product.ImageUrl4 = (string)row["ImageUrl4"];
                    }
                    if (!(string.IsNullOrEmpty(product.ImageUrl4) || (product.ImageUrl4.Length <= 0)))
                    {
                        strArray = ProcessImages(current, product.ImageUrl4);
                    }
                    if (row["ImageUrl5"] != DBNull.Value)
                    {
                        product.ImageUrl5 = (string)row["ImageUrl5"];
                    }
                    if (!(string.IsNullOrEmpty(product.ImageUrl5) || (product.ImageUrl5.Length <= 0)))
                    {
                        strArray = ProcessImages(current, product.ImageUrl5);
                    }
                    SKUItem item = new SKUItem
                    {
                        SkuId = "0",
                        SKU = (string)row["SKU"]
                    };
                    if (row["Stock"] != DBNull.Value)
                    {
                        item.Stock = (int)row["Stock"];
                    }
                    if (row["Weight"] != DBNull.Value)
                    {
                        item.Weight = (decimal)row["Weight"];
                    }
                    item.SalePrice = (decimal)row["SalePrice"];
                    Dictionary<string, SKUItem> skus = new Dictionary<string, SKUItem>();
                    skus.Add(item.SkuId, item);
                    ProductActionStatus status = AddProduct(product, skus, null, null);
                    ProductDao dao = new ProductDao();
                    if (status == ProductActionStatus.Success)
                    {
                        dao.AddProductMinPriceAndMaxPrice(product.ProductId);
                    }
                    if (isImportFromTaobao && (status == ProductActionStatus.Success))
                    {
                        TaobaoProductInfo taobaoProduct = new TaobaoProductInfo
                        {
                            ProductId = product.ProductId,
                            ProTitle = product.ProductName,
                            Cid = (long)row["Cid"]
                        };
                        if (row["StuffStatus"] != DBNull.Value)
                        {
                            taobaoProduct.StuffStatus = (string)row["StuffStatus"];
                        }
                        taobaoProduct.Num = (long)row["Num"];
                        taobaoProduct.LocationState = (string)row["LocationState"];
                        taobaoProduct.LocationCity = (string)row["LocationCity"];
                        taobaoProduct.FreightPayer = (string)row["FreightPayer"];
                        if (row["PostFee"] != DBNull.Value)
                        {
                            taobaoProduct.PostFee = (decimal)row["PostFee"];
                        }
                        if (row["ExpressFee"] != DBNull.Value)
                        {
                            taobaoProduct.ExpressFee = (decimal)row["ExpressFee"];
                        }
                        if (row["EMSFee"] != DBNull.Value)
                        {
                            taobaoProduct.EMSFee = (decimal)row["EMSFee"];
                        }
                        taobaoProduct.HasInvoice = (bool)row["HasInvoice"];
                        taobaoProduct.HasWarranty = (bool)row["HasWarranty"];
                        taobaoProduct.HasDiscount = (bool)row["HasDiscount"];
                        taobaoProduct.ValidThru = (long)row["ValidThru"];
                        if (row["ListTime"] != DBNull.Value)
                        {
                            taobaoProduct.ListTime = (DateTime)row["ListTime"];
                        }
                        else
                        {
                            taobaoProduct.ListTime = DateTime.Now;
                        }
                        if (row["PropertyAlias"] != DBNull.Value)
                        {
                            taobaoProduct.PropertyAlias = (string)row["PropertyAlias"];
                        }
                        if (row["InputPids"] != DBNull.Value)
                        {
                            taobaoProduct.InputPids = (string)row["InputPids"];
                        }
                        if (row["InputStr"] != DBNull.Value)
                        {
                            taobaoProduct.InputStr = (string)row["InputStr"];
                        }
                        if (row["SkuProperties"] != DBNull.Value)
                        {
                            taobaoProduct.SkuProperties = (string)row["SkuProperties"];
                        }
                        if (row["SkuQuantities"] != DBNull.Value)
                        {
                            taobaoProduct.SkuQuantities = (string)row["SkuQuantities"];
                        }
                        if (row["SkuPrices"] != DBNull.Value)
                        {
                            taobaoProduct.SkuPrices = (string)row["SkuPrices"];
                        }
                        if (row["SkuOuterIds"] != DBNull.Value)
                        {
                            taobaoProduct.SkuOuterIds = (string)row["SkuOuterIds"];
                        }
                        UpdateToaobProduct(taobaoProduct);
                    }
                }
            }
        }
        public static void ImportProducts2(DataTable productData, int categoryId, int lineId, int? brandId, ProductSaleStatus saleStatus, bool isImportFromTaobao)
        {
            if ((productData != null) && (productData.Rows.Count > 0))
            {
                foreach (DataRow row in productData.Rows)
                {
                    ProductInfo product = new ProductInfo {
                        CategoryId = categoryId,
                        MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|",
                        ProductName = (string) row["ProductName"],
                        ProductCode = (string) row["SKU"],
                        BrandId = brandId
                    };
                    if (row["Description"] != DBNull.Value)
                    {
                        product.Description = (string) row["Description"];
                    }
                    product.MarketPrice = new decimal?((decimal) row["SalePrice"]);
                    product.AddedDate = DateTime.Now;
                    product.SaleStatus = saleStatus;
                    product.HasSKU = false;
                    HttpContext current = HttpContext.Current;
                    if (row["ImageUrl1"] != DBNull.Value)
                    {
                        product.ImageUrl1 = (string) row["ImageUrl1"];
                    }
                    if (!string.IsNullOrEmpty(product.ImageUrl1) && (product.ImageUrl1.Length > 0))
                    {
                        string[] strArray = ProcessImages(current, product.ImageUrl1);
                        product.ThumbnailUrl40 = strArray[0];
                        product.ThumbnailUrl60 = strArray[1];
                        product.ThumbnailUrl100 = strArray[2];
                        product.ThumbnailUrl160 = strArray[3];
                        product.ThumbnailUrl180 = strArray[4];
                        product.ThumbnailUrl220 = strArray[5];
                        product.ThumbnailUrl310 = strArray[6];
                        product.ThumbnailUrl410 = strArray[7];
                    }
                    if (row["ImageUrl2"] != DBNull.Value)
                    {
                        product.ImageUrl2 = (string) row["ImageUrl2"];
                    }
                    if (!string.IsNullOrEmpty(product.ImageUrl2) && (product.ImageUrl2.Length > 0))
                    {
                        ProcessImages(current, product.ImageUrl2);
                    }
                    if (row["ImageUrl3"] != DBNull.Value)
                    {
                        product.ImageUrl3 = (string) row["ImageUrl3"];
                    }
                    if (!string.IsNullOrEmpty(product.ImageUrl3) && (product.ImageUrl3.Length > 0))
                    {
                        ProcessImages(current, product.ImageUrl3);
                    }
                    if (row["ImageUrl4"] != DBNull.Value)
                    {
                        product.ImageUrl4 = (string) row["ImageUrl4"];
                    }
                    if (!string.IsNullOrEmpty(product.ImageUrl4) && (product.ImageUrl4.Length > 0))
                    {
                        ProcessImages(current, product.ImageUrl4);
                    }
                    if (row["ImageUrl5"] != DBNull.Value)
                    {
                        product.ImageUrl5 = (string) row["ImageUrl5"];
                    }
                    if (!string.IsNullOrEmpty(product.ImageUrl5) && (product.ImageUrl5.Length > 0))
                    {
                        ProcessImages(current, product.ImageUrl5);
                    }
                    SKUItem item = new SKUItem {
                        SkuId = "0",
                        SKU = (string) row["SKU"]
                    };
                    if (row["Stock"] != DBNull.Value)
                    {
                        item.Stock = (int) row["Stock"];
                    }
                    if (row["Weight"] != DBNull.Value)
                    {
                        item.Weight = (decimal) row["Weight"];
                    }
                    item.SalePrice = (decimal) row["SalePrice"];
                    Dictionary<string, SKUItem> skus = new Dictionary<string, SKUItem>();
                    skus.Add(item.SkuId, item);
                    ProductActionStatus status = AddProduct(product, skus, null, null);
                    ProductDao dao = new ProductDao();
                    if (status == ProductActionStatus.Success)
                    {
                        dao.AddProductMinPriceAndMaxPrice(product.ProductId);
                    }
                    if (isImportFromTaobao && (status == ProductActionStatus.Success))
                    {
                        TaobaoProductInfo taobaoProduct = new TaobaoProductInfo {
                            ProductId = product.ProductId,
                            ProTitle = product.ProductName,
                            Cid = (long) row["Cid"]
                        };
                        if (row["StuffStatus"] != DBNull.Value)
                        {
                            taobaoProduct.StuffStatus = (string) row["StuffStatus"];
                        }
                        taobaoProduct.Num = (long) row["Num"];
                        taobaoProduct.LocationState = (string) row["LocationState"];
                        taobaoProduct.LocationCity = (string) row["LocationCity"];
                        taobaoProduct.FreightPayer = (string) row["FreightPayer"];
                        if (row["PostFee"] != DBNull.Value)
                        {
                            taobaoProduct.PostFee = (decimal) row["PostFee"];
                        }
                        if (row["ExpressFee"] != DBNull.Value)
                        {
                            taobaoProduct.ExpressFee = (decimal) row["ExpressFee"];
                        }
                        if (row["EMSFee"] != DBNull.Value)
                        {
                            taobaoProduct.EMSFee = (decimal) row["EMSFee"];
                        }
                        taobaoProduct.HasInvoice = (bool) row["HasInvoice"];
                        taobaoProduct.HasWarranty = (bool) row["HasWarranty"];
                        taobaoProduct.HasDiscount = (bool) row["HasDiscount"];
                        taobaoProduct.ValidThru = (long) row["ValidThru"];
                        if (row["ListTime"] != DBNull.Value)
                        {
                            taobaoProduct.ListTime = (DateTime) row["ListTime"];
                        }
                        else
                        {
                            taobaoProduct.ListTime = DateTime.Now;
                        }
                        if (row["PropertyAlias"] != DBNull.Value)
                        {
                            taobaoProduct.PropertyAlias = (string) row["PropertyAlias"];
                        }
                        if (row["InputPids"] != DBNull.Value)
                        {
                            taobaoProduct.InputPids = (string) row["InputPids"];
                        }
                        if (row["InputStr"] != DBNull.Value)
                        {
                            taobaoProduct.InputStr = (string) row["InputStr"];
                        }
                        if (row["SkuProperties"] != DBNull.Value)
                        {
                            taobaoProduct.SkuProperties = (string) row["SkuProperties"];
                        }
                        if (row["SkuQuantities"] != DBNull.Value)
                        {
                            taobaoProduct.SkuQuantities = (string) row["SkuQuantities"];
                        }
                        if (row["SkuPrices"] != DBNull.Value)
                        {
                            taobaoProduct.SkuPrices = (string) row["SkuPrices"];
                        }
                        if (row["SkuOuterIds"] != DBNull.Value)
                        {
                            taobaoProduct.SkuOuterIds = (string) row["SkuOuterIds"];
                        }
                        UpdateToaobProduct(taobaoProduct);
                    }
                }
            }
        }

        public static void ImportProducts(DataSet productData, int categoryId, int lineId, int? bandId, ProductSaleStatus saleStatus, bool includeCostPrice, bool includeStock, bool includeImages)
        {
            foreach (DataRow row in productData.Tables["products"].Rows)
            {
                int mappedProductId = (int) row["ProductId"];
                ProductInfo product = ConverToProduct(row, categoryId, lineId, bandId, saleStatus, includeImages);
                Dictionary<string, SKUItem> skus = ConverToSkus(mappedProductId, productData, includeCostPrice, includeStock);
                Dictionary<int, IList<int>> attrs = ConvertToAttributes(mappedProductId, productData);
                AddProduct(product, skus, attrs, null);
            }
        }

        public static int InStock(string productIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.InStockProduct);
            if (string.IsNullOrEmpty(productIds))
            {
                return 0;
            }
            int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.OnStock);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量入库了 “{0}” 件商品", new object[] { num }));
            }
            return num;
        }

        public static bool IsExitTaobaoProduct(long taobaoProductId)
        {
            return new TaobaoProductDao().IsExitTaobaoProduct(taobaoProductId);
        }

        public static int OffShelf(string productIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.OffShelfProducts);
            if (string.IsNullOrEmpty(productIds))
            {
                return 0;
            }
            int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.UnSale);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量下架了 “{0}” 件商品", new object[] { num }));
            }
            return num;
        }

        private static string[] ProcessImages(HttpContext context, string originalSavePath)
        {
            string fileName = Path.GetFileName(originalSavePath);
            string str2 = "/Storage/master/product/thumbs40/40_" + fileName;
            string str3 = "/Storage/master/product/thumbs60/60_" + fileName;
            string str4 = "/Storage/master/product/thumbs100/100_" + fileName;
            string str5 = "/Storage/master/product/thumbs160/160_" + fileName;
            string str6 = "/Storage/master/product/thumbs180/180_" + fileName;
            string str7 = "/Storage/master/product/thumbs220/220_" + fileName;
            string str8 = "/Storage/master/product/thumbs310/310_" + fileName;
            string str9 = "/Storage/master/product/thumbs410/410_" + fileName;
            string path = context.Request.MapPath(Globals.ApplicationPath + originalSavePath);
            if (File.Exists(path))
            {
                try
                {
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str2), 40, 40);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str3), 60, 60);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str4), 100, 100);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str5), 160, 160);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str6), 180, 180);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str7), 220, 220);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str8), 310, 310);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str9), 410, 410);
                }
                catch
                {
                }
            }
            return new string[] { str2, str3, str4, str5, str6, str7, str8, str9 };
        }

        public static int RemoveProduct(string productIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
            if (string.IsNullOrEmpty(productIds))
            {
                return 0;
            }
            int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.Delete);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量删除了 “{0}” 件商品到回收站", new object[] { num }));
            }
            return num;
        }

        public static bool ReplaceProductNames(string productIds, string oldWord, string newWord)
        {
            return new ProductBatchDao().ReplaceProductNames(productIds, oldWord, newWord);
        }

        public static void SendWXMessage_AddNewProduct(ProductInfo product)
        {
            bool flag;
            AsyncWorkDelegate delegate2 = new AsyncWorkDelegate();
            new WorkDelegate(delegate2.CalData).BeginInvoke(product, out flag, null, null);
        }

        public static int SetFreeShip(string productIds, bool isFree)
        {
            if (string.IsNullOrEmpty(productIds))
            {
                return 0;
            }
            int num = new ProductDao().UpdateProductShipFree(productIds, isFree);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "{0}了“{1}” 件商品包邮", new object[] { isFree ? "设置" : "取消", num }));
            }
            return num;
        }

        public static ProductActionStatus UpdateProduct(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> tagIds)
        {
            if (product == null)
            {
                return ProductActionStatus.UnknowError;
            }
            Globals.EntityCoding(product, true);
            int decimalLength = SettingsManager.GetMasterSettings(true).DecimalLength;
            if (product.MarketPrice.HasValue)
            {
                product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
            }
            ProductActionStatus unknowError = ProductActionStatus.UnknowError;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    ProductDao dao = new ProductDao();
                    if (!dao.UpdateProduct(product, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.DuplicateSKU;
                    }
                    if (!dao.DeleteProductSKUS(product.ProductId, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.SKUError;
                    }
                    if (((skus != null) && (skus.Count > 0)) && !dao.AddProductSKUs(product.ProductId, skus, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.SKUError;
                    }
                    if (!dao.AddProductAttributes(product.ProductId, attrs, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.AttributeError;
                    }
                    if (!new TagDao().DeleteProductTags(product.ProductId, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.ProductTagEroor;
                    }
                    if ((tagIds.Count > 0) && !new TagDao().AddProductTags(product.ProductId, tagIds, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.ProductTagEroor;
                    }
                    dbTran.Commit();
                    unknowError = ProductActionStatus.Success;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
            if (unknowError == ProductActionStatus.Success)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的商品", new object[] { product.ProductId }));
            }
            return unknowError;
        }

        public static int UpdateProductApproveStatusForApi(int num_iid, string approve_status)
        {
            return new ProductDao().UpdateProductApproveStatusForApi(num_iid, approve_status);
        }

        public static bool UpdateProductBaseInfo(DataTable dt)
        {
            return (((dt != null) && (dt.Rows.Count > 0)) && new ProductBatchDao().UpdateProductBaseInfo(dt));
        }

        public static bool UpdateProductCategory(int productId, int newCategoryId)
        {
            bool flag;
            if (newCategoryId != 0)
            {
                flag = new ProductDao().UpdateProductCategory(productId, newCategoryId, CatalogHelper.GetCategory(newCategoryId).Path + "|");
            }
            else
            {
                flag = new ProductDao().UpdateProductCategory(productId, newCategoryId, null);
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改编号 “{0}” 的店铺分类为 “{1}”", new object[] { productId, newCategoryId }));
            }
            return flag;
        }

        public static string UpdateProductContent(int productid, string content)
        {
            return new ProductDao().UpdateProductContent(productid, content);
        }

        public static int UpdateProductFreightTemplate(string productIds, int FreightTemplateId)
        {
            if (string.IsNullOrEmpty(productIds))
            {
                return 0;
            }
            int num = new ProductDao().UpdateProductFreightTemplate(productIds, FreightTemplateId);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "{0}了“{1}” 件商品运费", new object[] { "设置", num }));
            }
            return num;
        }

        public static bool UpdateProductNames(string productIds, string prefix, string suffix)
        {
            return new ProductBatchDao().UpdateProductNames(productIds, prefix, suffix);
        }

        public static int UpdateProductQuantityForApi(int num_iid, string sku_id, int quantity, int type)
        {
            return new ProductDao().UpdateProductQuantityForApi(num_iid, sku_id, quantity, type);
        }

        public static void UpdateShoppingCartsTemplateId(int productId, int templateId)
        {
            new ProductDao().UpdateShoppingCartsTemplateId(productId, templateId);
        }

        public static bool UpdateShowSaleCounts(DataTable dt)
        {
            return (((dt != null) && (dt.Rows.Count > 0)) && new ProductBatchDao().UpdateShowSaleCounts(dt));
        }

        public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
        {
            return new ProductBatchDao().UpdateShowSaleCounts(productIds, showSaleCounts);
        }

        public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
        {
            return new ProductBatchDao().UpdateShowSaleCounts(productIds, showSaleCounts, operation);
        }

        public static bool UpdateSkuMemberPrices(DataSet ds)
        {
            return new ProductBatchDao().UpdateSkuMemberPrices(ds);
        }

        public static bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price)
        {
            return new ProductBatchDao().UpdateSkuMemberPrices(productIds, gradeId, price);
        }

        public static bool UpdateSkuMemberPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
        {
            return new ProductBatchDao().UpdateSkuMemberPrices(productIds, gradeId, baseGradeId, operation, price);
        }

        public static bool UpdateSkuStock(Dictionary<string, int> skuStocks)
        {
            return new ProductBatchDao().UpdateSkuStock(skuStocks);
        }

        public static bool UpdateSkuStock(string productIds, int stock)
        {
            return new ProductBatchDao().UpdateSkuStock(productIds, stock);
        }

        public static bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct)
        {
            return new TaobaoProductDao().UpdateToaobProduct(taobaoProduct);
        }

        public static int UpShelf(string productIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.UpShelfProducts);
            if (string.IsNullOrEmpty(productIds))
            {
                return 0;
            }
            int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.OnSale);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.UpShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量上架了 “{0}” 件商品", new object[] { num }));
            }
            return num;
        }

        public class AsyncWorkDelegate
        {
            public void CalData(ProductInfo product, out bool result)
            {
                result = false;
                try
                {
                    Messenger.SendWeiXinMsg_ProductCreate(product.ProductName, product.MinSalePrice);
                }
                catch (Exception)
                {
                }
            }
        }

        public delegate void WorkDelegate(ProductInfo product, out bool result);
    }
}

