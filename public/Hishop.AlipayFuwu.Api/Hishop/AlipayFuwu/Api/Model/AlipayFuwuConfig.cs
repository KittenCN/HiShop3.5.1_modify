namespace Hishop.AlipayFuwu.Api.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    public class AlipayFuwuConfig
    {
        public static string alipay_public_key = "";
        public static string appId = "";
        public static Dictionary<string, string> BindAdmin = new Dictionary<string, string>();
        public static string charset = "GBK";
        public static string errstr = "";
        public static string merchant_private_key = "";
        public static string merchant_public_key = "";
        public static string serverUrl = "https://openapi.alipay.com/gateway.do";
        public static bool writeLog = false;

        public static bool CommSetConfig(string _appId, string HostPath, string _charset = "GBK")
        {
            merchant_private_key = HostPath + @"\config\rsa_private_key.pem";
            alipay_public_key = HostPath + @"\config\alipay_pubKey.pem";
            merchant_public_key = HostPath + @"\config\rsa_public_key.pem";
            if (((_appId.Length > 15) && File.Exists(merchant_private_key)) && (File.Exists(merchant_private_key) && File.Exists(merchant_private_key)))
            {
                appId = _appId;
                charset = _charset;
                errstr = "";
                return true;
            }
            merchant_private_key = "";
            alipay_public_key = "";
            merchant_public_key = "";
            errstr = "服务窗参数配置错误！";
            return false;
        }

        public static void SetConfig(string pubkeyfilepath, string privatefilepath, string alipay_public_keypath, string _appId, string _charset = "GBK")
        {
            merchant_private_key = privatefilepath;
            merchant_public_key = pubkeyfilepath;
            alipay_public_key = alipay_public_keypath;
            appId = _appId;
            charset = _charset;
        }

        public static void SetWriteLog(bool _writeLog)
        {
            writeLog = _writeLog;
        }
    }
}

