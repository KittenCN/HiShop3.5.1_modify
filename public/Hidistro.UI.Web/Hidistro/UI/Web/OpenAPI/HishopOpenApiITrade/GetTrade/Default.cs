namespace Hidistro.UI.Web.OpenAPI.HishopOpenApiITrade.GetTrade
{
    using global::Hishop.Open.Api;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Web.OpenAPI;
    using Hishop.Open.Api;
    using Impl;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;

    public class Default : Page
    {
        private SiteSettings site = SettingsManager.GetMasterSettings(true);
        private ITrade tradeApi = new TradeApi();

        private bool CheckTradesParameters(SortedDictionary<string, string> parameters, ref string results)
        {
            if (!OpenApiHelper.CheckSystemParameters(parameters, this.site.AppKey, out results))
            {
                return false;
            }
            if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["tid"])))
            {
                results = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Missing_Required_Arguments, "tid");
                return false;
            }
            return true;
        }

        public void GetTrade(SortedDictionary<string, string> parameters, ref string results)
        {
            if (this.CheckTradesParameters(parameters, ref results) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref results))
            {
                results = this.tradeApi.GetTrade(parameters["tid"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sortedParams = OpenApiHelper.GetSortedParams(this.Context);
            string results = string.Empty;
            this.GetTrade(sortedParams, ref results);
            base.Response.ContentType = "text/json";
            base.Response.Write(results);
            base.Response.End();
        }
    }
}

