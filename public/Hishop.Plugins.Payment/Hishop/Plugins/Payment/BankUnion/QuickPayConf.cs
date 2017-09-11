namespace Hishop.Plugins.Payment.BankUnion
{
    using System;

    public class QuickPayConf
    {
        public static string backStagegateWay = "http://172.17.136.37:8080/UpopWeb/api/BSPay.action";
        public static string charset = "UTF-8";
        public static string gateWay = "http://172.17.136.37:8080/UpopWeb/api/Pay.action";
        public static string merCode = "105550149170027";
        public static string merName = "银联商城";
        public static string[] notifyVo = new string[] { 
            "charset", "cupReserved", "exchangeDate", "exchangeRate", "merAbbr", "merId", "orderAmount", "orderCurrency", "orderNumber", "qid", "respCode", "respMsg", "respTime", "settleAmount", "settleCurrency", "settleDate", 
            "traceNumber", "traceTime", "transType", "version"
         };
        public static string queryUrl = "http://172.17.136.37:8080/UpopWeb/api/Query.action";
        public static string[] queryVo = new string[] { "version", "charset", "transType", "merId", "orderNumber", "orderTime", "merReserved" };
        public static string[] reqVo = new string[] { 
            "version", "charset", "transType", "origQid", "merId", "merAbbr", "acqCode", "merCode", "commodityUrl", "commodityName", "commodityUnitPrice", "commodityQuantity", "commodityDiscount", "transferFee", "orderNumber", "orderAmount", 
            "orderCurrency", "orderTime", "customerIp", "customerName", "defaultPayType", "defaultBankNumber", "transTimeout", "frontEndUrl", "backEndUrl", "merReserved"
         };
        public static string securityKey = "88888888";
        public static string signature = "signature";
        public static string signMethod = "signMethod";
        public static string signType = "MD5";
        public static string version = "1.0.0";
    }
}

