namespace Hidistro.UI.Web.OpenAPI.HishopOpenApiIProduct.UpdateProductQuantity
{
    using Core;
    using global::Hishop.Open.Api;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sortedParams = OpenApiHelper.GetSortedParams(this.Context);
            string result = string.Empty;
            this.UpdateProductQuantity(sortedParams, ref result);
            base.Response.ContentType = "text/json";
            base.Response.Write(result);
            base.Response.End();
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
    }
}

