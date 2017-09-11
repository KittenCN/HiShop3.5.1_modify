namespace Hidistro.UI.Web.OpenAPI
{
    using global::Hishop.Open.Api;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Vshop;
    using Hishop.Open.Api;
    using Impl;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Web;

    public class ITradeApi : IHttpHandler
    {
        private SiteSettings site = SettingsManager.GetMasterSettings(true);
        private ITrade tradeApi = new TradeApi();

        public void ChangLogistics(SortedDictionary<string, string> parameters, ref string result)
        {
            if (this.CheckChangLogisticsParameters(parameters, ref result) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref result))
            {
                result = this.tradeApi.ChangLogistics(parameters["tid"], parameters["company_name"], parameters["out_sid"]);
            }
        }

        private bool CheckChangLogisticsParameters(SortedDictionary<string, string> parameters, ref string result)
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

        public bool CheckHasNext(int totalrecord, int pagesize, int pageindex)
        {
            int num = pagesize * pageindex;
            return (totalrecord > num);
        }

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

        private bool CheckUpdateTradeMemoParameters(SortedDictionary<string, string> parameters, out int flag, ref string result)
        {
            flag = 0;
            if (!OpenApiHelper.CheckSystemParameters(parameters, this.site.AppKey, out result))
            {
                return false;
            }
            if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["tid"])))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Missing_Required_Arguments, "tid");
                return false;
            }
            if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["flag"])))
            {
                if (!int.TryParse(DataHelper.CleanSearchString(parameters["flag"]), out flag))
                {
                    result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Parameters_Format_Error, "flag");
                    return false;
                }
                if ((flag < 1) || (flag > 6))
                {
                    result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_Flag_Too_Long, "flag");
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["memo"])) && (DataHelper.CleanSearchString(parameters["memo"]).Length > 300))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_Memo_Too_Long, "memo");
                return false;
            }
            Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9._一-龥-]+$");
            if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["memo"])) && !regex.IsMatch(DataHelper.CleanSearchString(parameters["memo"])))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Parameters_Format_Error, "memo");
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

        public void GetTrade(SortedDictionary<string, string> parameters, ref string results)
        {
            if (this.CheckTradesParameters(parameters, ref results) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref results))
            {
                results = this.tradeApi.GetTrade(parameters["tid"]);
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string results = string.Empty;
            string str2 = string.Empty;
            str2 = context.Request["HIGW"];
            SortedDictionary<string, string> sortedParams = OpenApiHelper.GetSortedParams(context);
            switch (str2)
            {
                case "GetSoldTrades":
                    this.GetSoldTrades(sortedParams, ref results);
                    break;

                case "GetTrade":
                    this.GetTrade(sortedParams, ref results);
                    break;

                case "GetIncrementSoldTrades":
                    this.GetIncrementSoldTrades(sortedParams, ref results);
                    break;

                case "SendLogistic":
                    this.SendLogistic(sortedParams, ref results);
                    break;

                case "UpdateTradeMemo":
                    this.UpdateTradeMemo(sortedParams, ref results);
                    break;

                case "ChangLogistics":
                    this.ChangLogistics(sortedParams, ref results);
                    break;

                default:
                    results = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Method, "HIGW");
                    break;
            }
            context.Response.ContentType = "text/json";
            context.Response.Write(results);
        }

        public void SendLogistic(SortedDictionary<string, string> parameters, ref string result)
        {
            if (this.CheckSendLogisticParameters(parameters, ref result) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref result))
            {
                result = this.tradeApi.SendLogistic(parameters["tid"], parameters["company_name"], parameters["out_sid"]);
            }
        }

        public void UpdateTradeMemo(SortedDictionary<string, string> parameters, ref string result)
        {
            int flag = 0;
            if (this.CheckUpdateTradeMemoParameters(parameters, out flag, ref result) && OpenApiSign.CheckSign(parameters, this.site.CheckCode, ref result))
            {
                result = this.tradeApi.UpdateTradeMemo(parameters["tid"], parameters["memo"], flag);
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

