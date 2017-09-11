namespace Hidistro.UI.Web.OpenAPI.HishopOpenApiIProduct.GetSoldProducts
{
    using global::Hishop.Open.Api;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Web.OpenAPI;
    using Hishop.Open.Api;
    using Impl;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Web.UI;

    public class Default : Page
    {
        private IProduct productApi = new ProductApi();
        private SiteSettings site = SettingsManager.GetMasterSettings(true);

        public bool CheckSoldProductsParameters(SortedDictionary<string, string> parameters, out DateTime? start_modified, out DateTime? end_modified, out string status, out int page_no, out int page_size, out string result)
        {
            start_modified = new DateTime?();
            end_modified = new DateTime?();
            status = string.Empty;
            page_no = 1;
            page_size = 10;
            if (!OpenApiHelper.CheckSystemParameters(parameters, this.site.AppKey, out result))
                return false;
            status = DataHelper.CleanSearchString(parameters["approve_status"]);
            if (!string.IsNullOrWhiteSpace(status) && status != "On_Sale" && (status != "Un_Sale" && status != "In_Stock"))
            {
                result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Product_Status_is_Invalid, "approve_status");
                return false;
            }
            else if (!string.IsNullOrEmpty(parameters["start_modified"]) && !OpenApiHelper.IsDate(parameters["start_modified"]))
            {
                result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Invalid_Timestamp, "start_modified");
                return false;
            }
            else if (!string.IsNullOrEmpty(parameters["end_modified"]) && !OpenApiHelper.IsDate(parameters["end_modified"]))
            {
                result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Invalid_Timestamp, "end_modified");
                return false;
            }
            else
            {
                if (!string.IsNullOrEmpty(parameters["start_modified"]))
                {
                    DateTime result1;
                    DateTime.TryParse(parameters["start_modified"], out result1);
                    start_modified = new DateTime?(result1);
                    if (result1 > DateTime.Now)
                    {
                        result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Time_Start_Now, "start_modified and currenttime");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(parameters["end_modified"]))
                    {
                        DateTime result2;
                        DateTime.TryParse(parameters["end_modified"], out result2);
                        end_modified = new DateTime?(result2);
                        if (result1 > result2)
                        {
                            result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Time_Start_End, "start_modified and end_created");
                            return false;
                        }
                        else if (result2 > DateTime.Now)
                        {
                            result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Time_End_Now, "end_modified and currenttime");
                            return false;
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(parameters["end_modified"]))
                {
                    DateTime result1;
                    DateTime.TryParse(parameters["end_modified"], out result1);
                    if (result1 > DateTime.Now)
                    {
                        result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Time_End_Now, "end_modified and currenttime");
                        return false;
                    }
                }
                if (!string.IsNullOrEmpty(parameters["order_by"]))
                {
                    if (parameters["order_by"].Split(':').Length != 2)
                    {
                        result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Invalid_Format, "order_by");
                        return false;
                    }
                    else
                    {
                        string[] strArray = parameters["order_by"].Split(':');
                        string str1 = DataHelper.CleanSearchString(strArray[0]);
                        string str2 = DataHelper.CleanSearchString(strArray[1]);
                        if (string.IsNullOrEmpty(str1))
                        {
                            result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Invalid_Format, "order_by");
                            return false;
                        }
                        else if (str1 != "display_sequence" || str1 != "create_time" || str1 != "sold_quantity")
                        {
                            result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Invalid_Format, "order_by");
                            return false;
                        }
                        else if (str2 != "desc" || str2 != "asc")
                        {
                            result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Invalid_Format, "order_by");
                            return false;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && !int.TryParse(((object)parameters["page_size"]).ToString(), out page_size))
                {
                    result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Parameters_Format_Error, "page_size");
                    return false;
                }
                else if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && (page_size <= 0 || page_size > 100))
                {
                    result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Page_Size_Too_Long, "page_size");
                    return false;
                }
                else if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_no"])) && !int.TryParse(((object)parameters["page_no"]).ToString(), out page_no))
                {
                    result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Parameters_Format_Error, "page_no");
                    return false;
                }
                else
                {
                    if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_no"])) || page_no > 0)
                        return true;
                    result = OpenApiErrorMessage.ShowErrorMsg((Enum)OpenApiErrorCode.Page_Size_Too_Long, "page_no");
                    return false;
                }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sortedParams = OpenApiHelper.GetSortedParams(this.Context);
            string result = string.Empty;
            this.GetSoldProducts(sortedParams, ref result);
            base.Response.ContentType = "text/json";
            base.Response.Write(result);
            base.Response.End();
        }
    }
}

