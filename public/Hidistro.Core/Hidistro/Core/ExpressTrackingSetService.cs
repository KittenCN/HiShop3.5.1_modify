namespace Hidistro.Core
{
    using Hidistro.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;

    public static class ExpressTrackingSetService
    {
        public static string GetHiShopExpTrackInfo(string shipperCode, string logisticsCode)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str = masterSettings.Exp_appKey.Trim();
            string appSecret = masterSettings.Exp_appSecret.Trim();
            string str3 = "http://wuliu.kuaidiantong.cn/api/logistics";
            if (string.IsNullOrWhiteSpace(str3))
            {
                return "没有配置快递接口地址!";
            }
            if (string.IsNullOrWhiteSpace(shipperCode))
            {
                return "没有输入快递公司编码,无法查询!";
            }
            if (string.IsNullOrWhiteSpace(logisticsCode))
            {
                return "没有输入快递编号,无法查询!";
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("app_key", str);
            parameters.Add("timestamp", DateTime.Now.ToString());
            parameters.Add("shipperCode", shipperCode);
            parameters.Add("logisticsCode", logisticsCode);
            string sign = GetSign(parameters, appSecret);
            return GetKuaidi100Format(GetRequestAPI(str3, parameters, sign));
        }

        private static string GetKuaidi100Format(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return str.Replace("\"traces\"", "\"data\"").Replace("\"acceptTime\"", "\"time\"").Replace("\"acceptStation\"", "\"context\"");
        }

        private static string GetRequestAPI(string apiUrl, IDictionary<string, string> dic, string sign)
        {
            string str3;
            string requestUriString = string.Format("{0}?app_key={1}&timestamp={2}&shipperCode={3}&logisticsCode={4}&sign={5}", new object[] { apiUrl, dic["app_key"], dic["timestamp"], dic["shipperCode"], dic["logisticsCode"], sign });
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream responseStream = null;
            StreamReader reader = null;
            string str2 = "";
            try
            {
                request = WebRequest.Create(requestUriString) as HttpWebRequest;
                request.Method = "GET";
                response = request.GetResponse() as HttpWebResponse;
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);
                str2 = reader.ReadToEnd();
                str3 = str2;
            }
            catch (Exception)
            {
                str3 = str2;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }
                if (responseStream != null)
                {
                    responseStream.Dispose();
                    responseStream = null;
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
            }
            return str3;
        }

        private static string GetSign(IDictionary<string, string> parameters, string appSecret)
        {
            IDictionary<string, string> dictionary = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> enumerator = dictionary.GetEnumerator();
            StringBuilder builder = new StringBuilder();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                string key = current.Key;
                KeyValuePair<string, string> pair2 = enumerator.Current;
                string str2 = pair2.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(str2))
                {
                    builder.Append(key).Append(str2);
                }
            }
            builder.Append(appSecret);
            byte[] buffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));
            StringBuilder builder2 = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder2.Append(buffer[i].ToString("X2"));
            }
            return builder2.ToString();
        }
    }
}

