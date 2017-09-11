namespace Hidistro.UI.Web.OpenAPI
{
    using global::Hishop.Open.Api;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hishop.Open.Api;
    using Impl;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web;

    public class IProductApi : IHttpHandler
    {
        private IProduct productApi = new ProductApi();
        private SiteSettings site;

        public bool CheckProductParameters(SortedDictionary<string, string> parameters, out int num_iid, out string result)
        {
            num_iid = 0;
            if (!OpenApiHelper.CheckSystemParameters(parameters, this.site.AppKey, out result))
            {
                return false;
            }
            if (!int.TryParse(parameters["num_iid"], out num_iid))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Parameters_Format_Error, "num_iid");
                return false;
            }
            return true;
        }

        public bool CheckSoldProductsParameters(SortedDictionary<string, string> parameters, out DateTime? start_modified, out DateTime? end_modified, out string status, out int page_no, out int page_size, out string result)
        {
            DateTime time2;
            start_modified = new DateTime?();
            end_modified = new DateTime?();
            status = string.Empty;
            page_no = 1;
            page_size = 10;
            if (!OpenApiHelper.CheckSystemParameters(parameters, this.site.AppKey, out result))
            {
                return false;
            }
            status = DataHelper.CleanSearchString(parameters["approve_status"]);
            if ((!string.IsNullOrWhiteSpace(status) && (status != "On_Sale")) && ((status != "Un_Sale") && (status != "In_Stock")))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Product_Status_is_Invalid, "approve_status");
                return false;
            }
            if (!string.IsNullOrEmpty(parameters["start_modified"]) && !OpenApiHelper.IsDate(parameters["start_modified"]))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Timestamp, "start_modified");
                return false;
            }
            if (!string.IsNullOrEmpty(parameters["end_modified"]) && !OpenApiHelper.IsDate(parameters["end_modified"]))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Timestamp, "end_modified");
                return false;
            }
            if (!string.IsNullOrEmpty(parameters["start_modified"]))
            {
                DateTime time;
                DateTime.TryParse(parameters["start_modified"], out time);
                start_modified = new DateTime?(time);
                if (time > DateTime.Now)
                {
                    result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Time_Start_Now, "start_modified and currenttime");
                    return false;
                }
                if (!string.IsNullOrEmpty(parameters["end_modified"]))
                {
                    DateTime.TryParse(parameters["end_modified"], out time2);
                    end_modified = new DateTime?(time2);
                    if (time > time2)
                    {
                        result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Time_Start_End, "start_modified and end_created");
                        return false;
                    }
                    if (time2 > DateTime.Now)
                    {
                        result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Time_End_Now, "end_modified and currenttime");
                        return false;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(parameters["end_modified"]))
            {
                DateTime.TryParse(parameters["end_modified"], out time2);
                if (time2 > DateTime.Now)
                {
                    result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Time_End_Now, "end_modified and currenttime");
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(parameters["order_by"]))
            {
                if (parameters["order_by"].Split(new char[] { ':' }).Length != 2)
                {
                    result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Format, "order_by");
                    return false;
                }
                string[] strArray = parameters["order_by"].Split(new char[] { ':' });
                string str = DataHelper.CleanSearchString(strArray[0]);
                string str2 = DataHelper.CleanSearchString(strArray[1]);
                if (string.IsNullOrEmpty(str))
                {
                    result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Format, "order_by");
                    return false;
                }
                if (((str != "display_sequence") || (str != "create_time")) || (str != "sold_quantity"))
                {
                    result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Format, "order_by");
                    return false;
                }
                if ((str2 != "desc") || (str2 != "asc"))
                {
                    result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Format, "order_by");
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && !int.TryParse(parameters["page_size"].ToString(), out page_size))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Parameters_Format_Error, "page_size");
                return false;
            }
            if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && ((page_size <= 0) || (page_size > 100)))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Page_Size_Too_Long, "page_size");
                return false;
            }
            if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_no"])) && !int.TryParse(parameters["page_no"].ToString(), out page_no))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Parameters_Format_Error, "page_no");
                return false;
            }
            if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_no"])) && (page_no <= 0))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Page_Size_Too_Long, "page_no");
                return false;
            }
            return true;
        }

        public bool CheckUpdateApproveStatusParameters(SortedDictionary<string, string> parameters, out int num_iid, out string status, out string result)
        {
            num_iid = 0;
            status = string.Empty;
            if (!OpenApiHelper.CheckSystemParameters(parameters, this.site.AppKey, out result))
            {
                return false;
            }
            if (!int.TryParse(parameters["num_iid"], out num_iid))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Parameters_Format_Error, "num_iid");
                return false;
            }
            status = DataHelper.CleanSearchString(parameters["approve_status"]);
            if (((status != "On_Sale") && (status != "Un_Sale")) && (status != "In_Stock"))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Product_Status_is_Invalid, "approve_status");
                return false;
            }
            return true;
        }

        public bool CheckUpdateQuantityParameters(SortedDictionary<string, string> parameters, out int num_iid, out int quantity, out int type, out string result)
        {
            num_iid = 0;
            quantity = 0;
            type = 1;
            if (!OpenApiHelper.CheckSystemParameters(parameters, this.site.AppKey, out result))
            {
                return false;
            }
            if (!int.TryParse(parameters["num_iid"], out num_iid))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Parameters_Format_Error, "num_iid");
                return false;
            }
            if (!int.TryParse(parameters["quantity"], out quantity))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Parameters_Format_Error, "quantity");
                return false;
            }
            if (!int.TryParse(parameters["type"], out type))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Parameters_Format_Error, "type");
                return false;
            }
            return true;
        }

        public void GetProduct(SortedDictionary<string, string> parameters, ref string result)
        {
            int num = 0;
            if (this.CheckProductParameters(parameters, out num, out result) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref result))
            {
                result = this.productApi.GetProduct(num);
            }
        }

        public void GetSoldProducts(SortedDictionary<string, string> parameters, ref string result)
        {
            DateTime? nullable = null;
            DateTime? nullable2 = null;
            string status = "";
            int num = 0;
            int num2 = 0;
            if (this.CheckSoldProductsParameters(parameters, out nullable, out nullable2, out status, out num, out num2, out result) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref result))
            {
                result = this.productApi.GetSoldProducts(nullable, nullable2, status, parameters["q"], parameters["order_by"], num, num2);
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            string str2 = string.Empty;
            str2 = context.Request["HIGW"];
            SortedDictionary<string, string> sortedParams = OpenApiHelper.GetSortedParams(context);
            this.site = SettingsManager.GetMasterSettings(true);
            string str3 = str2;
            if (str3 != null)
            {
                if (!(str3 == "GetSoldProducts"))
                {
                    if (str3 == "GetProduct")
                    {
                        this.GetProduct(sortedParams, ref result);
                        goto Label_00AE;
                    }
                    if (str3 == "UpdateProductQuantity")
                    {
                        this.UpdateProductQuantity(sortedParams, ref result);
                        goto Label_00AE;
                    }
                    if (str3 == "UpdateProductApproveStatus")
                    {
                        this.UpdateProductApproveStatus(sortedParams, ref result);
                        goto Label_00AE;
                    }
                }
                else
                {
                    this.GetSoldProducts(sortedParams, ref result);
                    goto Label_00AE;
                }
            }
            result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Method, "HIGW");
        Label_00AE:
            context.Response.ContentType = "text/json";
            context.Response.Write(result);
        }

        public void UpdateProductApproveStatus(SortedDictionary<string, string> parameters, ref string result)
        {
            int num = 0;
            string status = string.Empty;
            if (this.CheckUpdateApproveStatusParameters(parameters, out num, out status, out result) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref result))
            {
                result = this.productApi.UpdateProductApproveStatus(num, status);
            }
        }

        public void UpdateProductQuantity(SortedDictionary<string, string> parameters, ref string result)
        {
            int num = 0;
            int quantity = 0;
            int type = 1;
            if (this.CheckUpdateQuantityParameters(parameters, out num, out quantity, out type, out result) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref result))
            {
                result = this.productApi.UpdateProductQuantity(num, parameters["sku_id"], quantity, type);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

