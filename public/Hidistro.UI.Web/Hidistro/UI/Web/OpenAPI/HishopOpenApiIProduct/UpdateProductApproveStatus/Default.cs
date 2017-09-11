namespace Hidistro.UI.Web.OpenAPI.HishopOpenApiIProduct.UpdateProductApproveStatus
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

        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sortedParams = OpenApiHelper.GetSortedParams(this.Context);
            string result = string.Empty;
            this.UpdateProductApproveStatus(sortedParams, ref result);
            base.Response.ContentType = "text/json";
            base.Response.Write(result);
            base.Response.End();
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
    }
}

