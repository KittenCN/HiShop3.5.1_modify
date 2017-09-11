namespace Hidistro.UI.Web.OpenAPI.HishopOpenApiITrade.GetIncrementSoldTrades
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
    using System.Web.UI.HtmlControls;

    public class Default : Page
    {
        protected HtmlForm form1;
        private SiteSettings site = SettingsManager.GetMasterSettings(true);
        private ITrade tradeApi = new TradeApi();

        private bool CheckIncrementSoldTradesParameters(SortedDictionary<string, string> parameters, out DateTime start_modified, out DateTime end_modified, out string status, out int page_no, out int page_size, ref string result)
        {
            start_modified = DateTime.Now;
            end_modified = DateTime.Now;
            page_size = 10;
            page_no = 1;
            status = DataHelper.CleanSearchString(parameters["status"]);
            if (!OpenApiHelper.CheckSystemParameters(parameters, this.site.AppKey, out result))
            {
                return false;
            }
            if (((!string.IsNullOrWhiteSpace(status) && (status != "WAIT_BUYER_PAY")) && ((status != "WAIT_SELLER_SEND_GOODS ") && (status != "WAIT_BUYER_CONFIRM_GOODS"))) && ((status != "TRADE_CLOSED") && (status != "TRADE_FINISHED")))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_Status_is_Invalid, "status");
                return false;
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
            if (string.IsNullOrEmpty(parameters["start_modified"]))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Missing_Required_Arguments, "start_modified");
                return false;
            }
            if (!OpenApiHelper.IsDate(parameters["start_modified"]))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Timestamp, "start_modified");
                return false;
            }
            DateTime.TryParse(parameters["start_modified"], out start_modified);
            if (start_modified > DateTime.Now)
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Time_Start_Now, "start_modified and currenttime");
                return false;
            }
            if (string.IsNullOrEmpty(parameters["end_modified"]))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Missing_Required_Arguments, "end_modified");
                return false;
            }
            if (!OpenApiHelper.IsDate(parameters["end_modified"]))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Timestamp, "end_modified");
                return false;
            }
            DateTime.TryParse(parameters["end_modified"], out end_modified);
            if (start_modified > end_modified)
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Time_Start_End, "start_modified and end_modified");
                return false;
            }
            TimeSpan span = (TimeSpan) (end_modified - start_modified);
            if (span.TotalDays > 1.0)
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Time_StartModified_AND_EndModified, "start_modified and end_modified");
                return false;
            }
            if (end_modified > DateTime.Now)
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Time_End_Now, "end_modified and currenttime");
                return false;
            }
            return true;
        }

        private void GetIncrementSoldTrades(SortedDictionary<string, string> parameters, ref string results)
        {
            DateTime time;
            DateTime time2;
            string status = "";
            int num = 0;
            int num2 = 0;
            if (this.CheckIncrementSoldTradesParameters(parameters, out time, out time2, out status, out num, out num2, ref results) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref results))
            {
                results = this.tradeApi.GetIncrementSoldTrades(time, time2, status, parameters["buyer_uname"], num, num2);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sortedParams = OpenApiHelper.GetSortedParams(this.Context);
            string results = string.Empty;
            this.GetIncrementSoldTrades(sortedParams, ref results);
            base.Response.ContentType = "text/json";
            base.Response.Write(results);
            base.Response.End();
        }
    }
}

