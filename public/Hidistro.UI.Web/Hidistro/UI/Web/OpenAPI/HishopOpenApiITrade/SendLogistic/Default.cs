namespace Hidistro.UI.Web.OpenAPI.HishopOpenApiITrade.SendLogistic
{
    using global::Hishop.Open.Api;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Web.OpenAPI;
    using Hidistro.Vshop;
    using Hishop.Open.Api;
    using Impl;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;

    public class Default : Page
    {
        private SiteSettings site = SettingsManager.GetMasterSettings(true);
        private ITrade tradeApi = new TradeApi();

        private bool CheckSendLogisticParameters(SortedDictionary<string, string> parameters, ref string result)
        {
            if (!OpenApiHelper.CheckSystemParameters(parameters, this.site.AppKey, out result))
            {
                return false;
            }
            if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["tid"])))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Missing_Required_Arguments, "tid");
                return false;
            }
            if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["company_name"])))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Missing_Required_Arguments, "company_name");
                return false;
            }
            if (!ExpressHelper.IsExitExpress(DataHelper.CleanSearchString(parameters["company_name"])))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Company_not_Exists, "company_name");
                return false;
            }
            if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["out_sid"])))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Missing_Required_Arguments, "out_sid");
                return false;
            }
            if (DataHelper.CleanSearchString(parameters["out_sid"]).Length > 20)
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Out_Sid_Too_Long, "out_sid");
                return false;
            }
            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sortedParams = OpenApiHelper.GetSortedParams(this.Context);
            string result = string.Empty;
            this.SendLogistic(sortedParams, ref result);
            base.Response.ContentType = "text/json";
            base.Response.Write(result);
            base.Response.End();
        }

        public void SendLogistic(SortedDictionary<string, string> parameters, ref string result)
        {
            if (this.CheckSendLogisticParameters(parameters, ref result) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref result))
            {
                result = this.tradeApi.SendLogistic(parameters["tid"], parameters["company_name"], parameters["out_sid"]);
            }
        }
    }
}

