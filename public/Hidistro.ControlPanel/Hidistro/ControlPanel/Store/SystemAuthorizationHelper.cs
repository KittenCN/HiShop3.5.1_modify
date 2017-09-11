namespace Hidistro.ControlPanel.Store
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.Caching;

    public static class SystemAuthorizationHelper
    {
        private static readonly string authorizationUrl = "http://ysc.kuaidiantong.cn/wfxvalid.ashx";
        public static readonly string licenseMsg = ("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body>\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的系统未经官方授权，无法登录后台管理。请联系官方购买软件使用权。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.ApplicationPath + "/Default.aspx\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2015 hishop.com.cn all Rights Reserved. 本产品资源均为 Hishop 版权所有</div>\r\n</div>\r\n</body>\r\n</html>");
        public static readonly string noticeMsg = ("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body>\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的系统已过授权有效期，无法登录后台管理。请续费。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.ApplicationPath + "/Default.aspx\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2015 hishop.com.cn all Rights Reserved. 本产品资源均为 Hishop 版权所有</div>\r\n</div>\r\n</body>\r\n</html>");

        public static bool CheckDistributorIsCanAuthorization()
        {
            int leftNumber = 0;
            return CheckDistributorIsCanAuthorization(1, out leftNumber);
        }

        public static bool CheckDistributorIsCanAuthorization(int number, out int leftNumber)
        {
            leftNumber = 0;
            SystemAuthorizationInfo systemAuthorization = GetSystemAuthorization(false);
            if (systemAuthorization.DistributorCount > 0)
            {
                int systemDistributorsCount = MemberHelper.GetSystemDistributorsCount();
                leftNumber = systemAuthorization.DistributorCount - systemDistributorsCount;
                return (systemAuthorization.DistributorCount >= (systemDistributorsCount + number));
            }
            return true;
        }

        public static SystemAuthorizationInfo GetSystemAuthorization(bool iscreate)
        {
            string key = "DataCache-SystemAuthorizationInfo";
            SystemAuthorizationInfo info = HiCache.Get(key) as SystemAuthorizationInfo;
            if ((info == null) || iscreate)
            {
                string str2 = PostData(authorizationUrl, "host=" + Globals.DomainName);
                if (!string.IsNullOrEmpty(str2))
                {
                    TempAuthorizationInfo info2 = JsonConvert.DeserializeObject<TempAuthorizationInfo>(str2);
                    info = new SystemAuthorizationInfo {
                        state = (SystemAuthorizationState) info2.state,
                        DistributorCount = info2.count,
                        type = info2.type,
                        IsShowJixuZhiChi = info2.isshowjszc == "1"
                    };
                    HiCache.Insert(key, info, 360, CacheItemPriority.Normal);
                }
            }
            return info;
        }

        public static string PostData(string url, string postData)
        {
            string str = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stream2 = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        Stream stream3 = stream2;
                        if (response.ContentEncoding.ToLower() == "gzip")
                        {
                            stream3 = new GZipStream(stream2, CompressionMode.Decompress);
                        }
                        else if (response.ContentEncoding.ToLower() == "deflate")
                        {
                            stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
                        }
                        using (StreamReader reader = new StreamReader(stream3, encoding))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {
                str = string.Empty;
            }
            return str;
        }
    }
}

