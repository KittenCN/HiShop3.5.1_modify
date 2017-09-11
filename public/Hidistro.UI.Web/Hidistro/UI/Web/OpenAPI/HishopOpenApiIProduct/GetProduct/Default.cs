namespace Hidistro.UI.Web.OpenAPI.HishopOpenApiIProduct.GetProduct
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

        public void GetProduct(SortedDictionary<string, string> parameters, ref string result)
        {
            int num = 0;
            if (this.CheckProductParameters(parameters, out num, out result) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref result))
            {
                result = this.productApi.GetProduct(num);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sortedParams = OpenApiHelper.GetSortedParams(this.Context);
            string result = string.Empty;
            this.GetProduct(sortedParams, ref result);
            base.Response.ContentType = "text/json";
            base.Response.Write(result);
            base.Response.End();
        }
    }
}

