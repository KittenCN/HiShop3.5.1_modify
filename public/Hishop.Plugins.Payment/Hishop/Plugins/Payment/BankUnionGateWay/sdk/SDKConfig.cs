namespace Hishop.Plugins.Payment.BankUnionGateWay.sdk
{
    using System;
    using System.Web;
    public class SDKConfig
    {
        public static string MerId = "";
        public static string publicCertPath = HttpContext.Current.Request.MapPath("~/config/publickey.cer");
        public static string signCertPath;
        public static string SignCertPwd = "";
        public static string validateCertDir = HttpContext.Current.Request.MapPath("~/config");
    }
}

