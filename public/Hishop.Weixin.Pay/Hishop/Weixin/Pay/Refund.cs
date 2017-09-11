namespace Hishop.Weixin.Pay
{
    using Hishop.Weixin.Pay.Domain;
    using Hishop.Weixin.Pay.Lib;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class Refund
    {
        public static string SendRequest(RefundInfo info, PayConfig config, out string WxRefundNum)
        {
            WxPayData inputObj = new WxPayData();
            if (!string.IsNullOrEmpty(info.transaction_id))
            {
                inputObj.SetValue("transaction_id", info.transaction_id);
            }
            else
            {
                inputObj.SetValue("out_trade_no", info.out_trade_no);
            }
            inputObj.SetValue("total_fee", (int) info.TotalFee.Value);
            inputObj.SetValue("refund_fee", (int) info.RefundFee.Value);
            inputObj.SetValue("out_refund_no", info.out_refund_no);
            inputObj.SetValue("op_user_id", config.MchID);
            if (!string.IsNullOrEmpty(config.sub_appid))
            {
                inputObj.SetValue("sub_appid", config.sub_appid);
                inputObj.SetValue("sub_mch_id", config.sub_mch_id);
            }
            SortedDictionary<string, object> values = WxPayApi.Refund(inputObj, config, 60).GetValues();
            if ((values.ContainsKey("return_code") && (values["return_code"].ToString() == "SUCCESS")) && (values.ContainsKey("result_code") && (values["result_code"].ToString() == "SUCCESS")))
            {
                WxRefundNum = "";
                return "SUCCESS";
            }
            HttpService.WxDebuglog(JsonConvert.SerializeObject(values), "_wxpay.txt");
            string str = "";
            if (values.ContainsKey("err_code_des"))
            {
                str = values["err_code_des"].ToString();
            }
            if (values.ContainsKey("refund_id"))
            {
                WxRefundNum = values["refund_id"].ToString();
            }
            else
            {
                WxRefundNum = "";
            }
            return (str + values["return_msg"].ToString());
        }
    }
}

