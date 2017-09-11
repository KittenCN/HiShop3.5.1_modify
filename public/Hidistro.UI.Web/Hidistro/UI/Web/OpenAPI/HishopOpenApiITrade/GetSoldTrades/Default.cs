namespace Hidistro.UI.Web.OpenAPI.HishopOpenApiITrade.GetSoldTrades
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

        private bool CheckSoldTradesParameters(SortedDictionary<string, string> parameters, out DateTime? start_time, out DateTime? end_time, out string status, out int page_no, out int page_size, ref string result)
        {
            start_time = new DateTime?();
            end_time = new DateTime?();
            page_size = 10;
            page_no = 1;
            status = DataHelper.CleanSearchString(parameters["status"]);
            if (OpenApiHelper.CheckSystemParameters(parameters, this.site.AppKey, out result))
            {
                DateTime time2;
                if (!string.IsNullOrEmpty(parameters["start_created"]) && !OpenApiHelper.IsDate(parameters["start_created"]))
                {
                    result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Timestamp, "start_created");
                    return false;
                }
                if (!string.IsNullOrEmpty(parameters["end_created"]) && !OpenApiHelper.IsDate(parameters["end_created"]))
                {
                    result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Timestamp, "end_created");
                    return false;
                }
                if (!string.IsNullOrEmpty(parameters["start_created"]))
                {
                    DateTime time;
                    DateTime.TryParse(parameters["start_created"], out time);
                    start_time = new DateTime?(time);
                    if (time > DateTime.Now)
                    {
                        result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Time_Start_Now, "start_created and currenttime");
                        return false;
                    }
                    if (!string.IsNullOrEmpty(parameters["end_created"]))
                    {
                        DateTime.TryParse(parameters["end_created"], out time2);
                        end_time = new DateTime?(time2);
                        if (time > time2)
                        {
                            result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Time_Start_End, "start_created and end_created");
                            return false;
                        }
                        if (time2 > DateTime.Now)
                        {
                            result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Time_End_Now, "end_created and currenttime");
                            return false;
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(parameters["end_created"]))
                {
                    DateTime.TryParse(parameters["end_created"], out time2);
                    if (time2 > DateTime.Now)
                    {
                        result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Time_End_Now, "end_created and currenttime");
                        return false;
                    }
                }
                if (((!string.IsNullOrWhiteSpace(status) && (status != "WAIT_BUYER_PAY")) && ((status != "WAIT_SELLER_SEND_GOODS") && (status != "WAIT_BUYER_CONFIRM_GOODS"))) && ((status != "TRADE_CLOSED") && (status != "TRADE_FINISHED")))
                {
                    result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_Status_is_Invalid, "status");
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
                if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && !int.TryParse(parameters["page_size"].ToString(), out page_size))
                {
                    result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Parameters_Format_Error, "page_size");
                    return false;
                }
                if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) || ((page_size > 0) && (page_size <= 100)))
                {
                    return true;
                }
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Page_Size_Too_Long, "page_size");
            }
            return false;
        }

        private void GetSoldTrades(SortedDictionary<string, string> parameters, ref string results)
        {
            DateTime? nullable = null;
            DateTime? nullable2 = null;
            string status = string.Empty;
            int num = 0;
            int num2 = 0;
            if (this.CheckSoldTradesParameters(parameters, out nullable, out nullable2, out status, out num, out num2, ref results) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref results))
            {
                results = this.tradeApi.GetSoldTrades(nullable, nullable2, status, parameters["buyer_uname"], num, num2);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sortedParams = OpenApiHelper.GetSortedParams(this.Context);
            string results = string.Empty;
            this.GetSoldTrades(sortedParams, ref results);
            base.Response.ContentType = "text/json";
            Globals.Debuglog(base.Request.Url.ToString() + "||" + Globals.SubStr(results, 200, "------------结束"), "_DebugERP.txt");
            base.Response.Write(results);
            base.Response.End();
        }
    }
}

