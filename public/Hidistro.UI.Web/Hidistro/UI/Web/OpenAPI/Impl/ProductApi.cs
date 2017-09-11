namespace Hidistro.UI.Web.OpenAPI.Impl
{
    using global::Hishop.Open.Api;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hishop.Open.Api;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class ProductApi : IProduct
    {
        public string ConvertProductSold(DataTable dt)
        {
            List<product_list_model> list = new List<product_list_model>();
            foreach (DataRow row in dt.Rows)
            {
                product_list_model item = new product_list_model {
                    cid = (int) row["CategoryId"]
                };
                if (row["CategoryName"] != DBNull.Value)
                {
                    item.cat_name = (string) row["CategoryName"];
                }
                if (row["BrandId"] != DBNull.Value)
                {
                    item.brand_id = (int) row["BrandId"];
                }
                if (row["BrandName"] != DBNull.Value)
                {
                    item.brand_name = (string) row["BrandName"];
                }
                if (row["TypeId"] != DBNull.Value)
                {
                    item.type_id = (int) row["TypeId"];
                }
                if (row["TypeName"] != DBNull.Value)
                {
                    item.type_name = (string) row["TypeName"];
                }
                item.num_iid = (int) row["ProductId"];
                item.title = (string) row["ProductName"];
                if (row["ProductCode"] != DBNull.Value)
                {
                    item.outer_id = (string) row["ProductCode"];
                }
                if ((row["ImageUrl1"] != DBNull.Value) && !string.IsNullOrEmpty((string) row["ImageUrl1"]))
                {
                    item.pic_url.Add((string) row["ImageUrl1"]);
                }
                if ((row["ImageUrl2"] != DBNull.Value) && !string.IsNullOrEmpty((string) row["ImageUrl2"]))
                {
                    item.pic_url.Add((string) row["ImageUrl2"]);
                }
                if ((row["ImageUrl3"] != DBNull.Value) && !string.IsNullOrEmpty((string) row["ImageUrl3"]))
                {
                    item.pic_url.Add((string) row["ImageUrl3"]);
                }
                if ((row["ImageUrl4"] != DBNull.Value) && !string.IsNullOrEmpty((string) row["ImageUrl4"]))
                {
                    item.pic_url.Add((string) row["ImageUrl4"]);
                }
                if ((row["ImageUrl5"] != DBNull.Value) && !string.IsNullOrEmpty((string) row["ImageUrl5"]))
                {
                    item.pic_url.Add((string) row["ImageUrl5"]);
                }
                item.list_time = new DateTime?((DateTime) row["AddedDate"]);
                switch (((ProductSaleStatus) row["SaleStatus"]))
                {
                    case ProductSaleStatus.OnSale:
                        item.approve_status = "On_Sale";
                        break;

                    case ProductSaleStatus.UnSale:
                        item.approve_status = "Un_Sale";
                        break;

                    default:
                        item.approve_status = "In_Stock";
                        break;
                }
                item.sold_quantity = (int) row["SaleCounts"];
                item.num = (int) row["Stock"];
                item.price = (decimal) row["SalePrice"];
                list.Add(item);
            }
            return JsonConvert.SerializeObject(list);
        }

        public string GetProduct(int num_iid)
        {
            product_item_model productForApi = ProductHelper.GetProductForApi(num_iid);
            if (productForApi == null)
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Product_Not_Exists, "num_iid");
            }
            productForApi.props_name = ProductHelper.GetPropsForApi(num_iid);
            productForApi.skus = ProductHelper.GetSkusForApi(num_iid);
            string format = "{{\"product_get_response\":{{\"item\":{0}}}}}";
            return string.Format(format, JsonConvert.SerializeObject(productForApi));
        }

        public string GetSoldProducts(DateTime? start_modified, DateTime? end_modified, string approve_status, string q, string order_by, int page_no, int page_size)
        {
            string format = "{{\"products_get_response\":{{\"total_results\":\"{0}\",\"items\":{1}}}}}";
            ProductQuery query = new ProductQuery {
                SortBy = "DisplaySequence",
                SortOrder = SortAction.Asc,
                PageIndex = 1,
                PageSize = 40,
                SaleStatus = ProductSaleStatus.All
            };
            if (start_modified.HasValue)
            {
                query.StartDate = start_modified;
            }
            if (end_modified.HasValue)
            {
                query.EndDate = end_modified;
            }
            if (!string.IsNullOrEmpty(q))
            {
                query.Keywords = DataHelper.CleanSearchString(q);
            }
            if (!string.IsNullOrEmpty(approve_status))
            {
                ProductSaleStatus all = ProductSaleStatus.All;
                EnumDescription.GetEnumValue<ProductSaleStatus>(approve_status, ref all);
                query.SaleStatus = all;
            }
            DbQueryResult productsForApi = ProductHelper.GetProductsForApi(query);
            return string.Format(format, productsForApi.TotalRecords, this.ConvertProductSold((DataTable) productsForApi.Data));
        }

        public string UpdateProductApproveStatus(int num_iid, string approve_status)
        {
            product_item_model productForApi = ProductHelper.GetProductForApi(num_iid);
            if (productForApi == null)
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Product_Not_Exists, "num_iid");
            }
            if (ProductHelper.UpdateProductApproveStatusForApi(num_iid, approve_status) <= 0)
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Product_ApproveStatus_Faild, "update_approve_status");
            }
            productForApi.props_name = ProductHelper.GetPropsForApi(num_iid);
            productForApi.skus = ProductHelper.GetSkusForApi(num_iid);
            productForApi.approve_status = approve_status;
            string format = "{{\"product_get_response\":{{\"item\":{0}}}}}";
            return string.Format(format, JsonConvert.SerializeObject(productForApi));
        }

        public string UpdateProductQuantity(int num_iid, string sku_id, int quantity, int type)
        {
            product_item_model productForApi = ProductHelper.GetProductForApi(num_iid);
            if (productForApi == null)
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Product_Not_Exists, "num_iid");
            }
            if (ProductHelper.UpdateProductQuantityForApi(num_iid, sku_id, quantity, type) <= 0)
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Product_UpdateeQuantity_Faild, "update_quantity");
            }
            productForApi.props_name = ProductHelper.GetPropsForApi(num_iid);
            productForApi.skus = ProductHelper.GetSkusForApi(num_iid);
            string format = "{{\"product_get_response\":{{\"item\":{0}}}}}";
            return string.Format(format, JsonConvert.SerializeObject(productForApi));
        }
    }
}

