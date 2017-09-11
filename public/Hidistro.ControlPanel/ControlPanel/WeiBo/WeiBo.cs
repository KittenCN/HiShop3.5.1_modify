namespace ControlPanel.WeiBo
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Weibo;
    using Hishop.WeiBo.Api;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web;

    public class WeiBo
    {
        private SinaWeiboClient sinaweibo;

        public WeiBo()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            this.sinaweibo = new GetAuth().GetOpenAuthClient(masterSettings.Access_Token);
        }

        public string commentscreate(string id, string comment)
        {
            string result = "{\"IsAuthorized\":\"0\"}";
            if (this.sinaweibo.IsAuthorized)
            {
                result = this.sinaweibo.HttpPost("comments/create.json", new { id = id, comment = comment }).Content.ReadAsStringAsync().Result;
            }
            return result;
        }

        public string createmenu(string comment)
        {
            string result = "{\"IsAuthorized\":\"0\"}";
            if (this.sinaweibo.IsAuthorized)
            {
                result = this.sinaweibo.HttpPost("https://m.api.weibo.com/2/messages/menu/create.json", new { menus = comment }).Content.ReadAsStringAsync().Result;
            }
            return result;
        }

        public string deletemenu()
        {
            string str = "{\"IsAuthorized\":\"0\"}";
            if (this.sinaweibo.IsAuthorized)
                str = this.sinaweibo.HttpPost("https://m.api.weibo.com/2/messages/menu/delete.json", (object)new
                {
                }).Content.ReadAsStringAsync().Result;
            return str;
        }

        public string friends_timeline(int page)
        {
            string result = "{\"IsAuthorized\":\"0\"}";
            if (this.sinaweibo.IsAuthorized)
            {
                result = this.sinaweibo.HttpGet("statuses/friends_timeline.json", new { page = page }).Content.ReadAsStringAsync().Result;
            }
            return result;
        }

        public string get_uid()
        {
            string str = "{\"IsAuthorized\":\"0\"}";
            if (this.sinaweibo.IsAuthorized)
                str = JsonConvert.DeserializeObject<WeiBo.user>(this.sinaweibo.HttpGet("account/get_uid.json", (object)new
                {
                }).Content.ReadAsStringAsync().Result).uid;
            return str;
        }

        public string getfriends()
        {
            string result = "{\"IsAuthorized\":\"0\"}";
            if (this.sinaweibo.IsAuthorized)
            {
                result = this.sinaweibo.HttpGet("friendships/friends.json", new { uid = this.get_uid() }).Content.ReadAsStringAsync().Result;
            }
            return result;
        }

        public string repost(string id, string comment)
        {
            string result = "{\"IsAuthorized\":\"0\"}";
            if (this.sinaweibo.IsAuthorized)
            {
                result = this.sinaweibo.HttpPost("statuses/repost.json", new { id = id, status = comment }).Content.ReadAsStringAsync().Result;
            }
            return result;
        }

        public string sendmessage(string type, string receiver_id, string data)
        {
            string result = "{\"IsAuthorized\":\"0\"}";
            if (this.sinaweibo.IsAuthorized)
            {
                result = this.sinaweibo.HttpPost("https://m.api.weibo.com/2/messages/reply.json", new { type = type, receiver_id = receiver_id, data = HttpUtility.UrlDecode(data) }).Content.ReadAsStringAsync().Result;
            }
            return result;
        }

        public string SendMsg(string sendall, string url)
        {
            try
            {
                string requestUriString = url;
                string s = sendall;
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                HttpWebRequest request = WebRequest.Create(requestUriString) as HttpWebRequest;
                Encoding encoding1 = Encoding.UTF8;
                request.Method = "POST";
                request.KeepAlive = false;
                request.AllowAutoRedirect = true;
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR  3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string str4 = reader.ReadToEnd();
                reader.Close();
                return str4;
            }
            catch
            {
                return "{\"result\":false}";
            }
        }

        public string SendToUIDMessage(string msgtype, string displayname, string summary, string image, string url, string Content, string ArticleId)
        {

            string json = "{\"IsAuthorized\":\"0\"}";
            if (this.sinaweibo.IsAuthorized)
            {
                json = this.sinaweibo.HttpGet("https://m.api.weibo.com/2/messages/subscribers/get.json", (object)new
                {
                }).Content.ReadAsStringAsync().Result;
                JObject jobject1 = JObject.Parse(json);
                string str1 = "{\"touser\": [";
                if (jobject1["data"] != null)
                {
                    JObject jobject2 = JObject.Parse(((object)jobject1["data"]).ToString());
                    if (jobject2["uids"] != null)
                    {
                        foreach (JToken jtoken in (IEnumerable<JToken>)jobject2["uids"])
                        {
                            string str2 = (string)jtoken;
                            str1 = str1 + "\"" + str2 + "\",";
                        }
                        string str3 = str1.Substring(0, str1.Length - 1);
                        string str4 = "\"text\": {\"content\": \"" + Content + "\"}";
                        if (msgtype == "articles")
                        {
                            if (string.IsNullOrEmpty(summary))
                                summary = displayname;
                            string str2 = "{\"display_name\": \"" + displayname + "\",\"summary\":\"" + summary + "\",\"image\":\"" + image + "\",\"url\":\"" + url + "\"},";
                            IList<ArticleItemsInfo> articleItems = ArticleHelper.GetArticleItems(int.Parse(ArticleId));
                            if (articleItems.Count > 0)
                            {
                                foreach (ArticleItemsInfo articleItemsInfo in (IEnumerable<ArticleItemsInfo>)articleItems)
                                {
                                    string str5 = !string.IsNullOrEmpty(articleItemsInfo.Content) ? articleItemsInfo.Content : articleItemsInfo.Title;
                                    str2 = str2 + "{\"display_name\": \"" + articleItemsInfo.Title + "\",\"summary\":\"" + str5 + "\",\"image\":\"http://" + Globals.DomainName + articleItemsInfo.ImageUrl + "\",\"url\":\"" + articleItemsInfo.Url + "\"},";
                                }
                            }
                            str4 = "\"articles\": [" + str2.Substring(0, str2.Length - 1) + "]";
                        }
                        json = this.SendMsg(HttpUtility.UrlDecode(str3 + "]," + str4 + ",\"msgtype\": \"" + msgtype + "\"}"), "https://m.api.weibo.com/2/messages/sendall.json?access_token=" + this.sinaweibo.AccessToken);
                    }
                    else
                        json = "{\"result\":false\"}";
                }
            }
            return json;
        }

        public string showemenu()
        {
            string str = "{\"IsAuthorized\":\"0\"}";
            if (this.sinaweibo.IsAuthorized)
                str = this.sinaweibo.HttpGet("https://m.api.weibo.com/2/messages/menu/show.json", (object)new
                {
                }).Content.ReadAsStringAsync().Result;
            return str;
        }

        public string statusesupdate(string status, string img)
        {
            string str = "{\"IsAuthorized\":\"0\"}";
            if (!this.sinaweibo.IsAuthorized)
            {
                return str;
            }
            FileInfo info = new FileInfo(HttpContext.Current.Server.MapPath(img));
            if (info.Exists)
            {
                return this.sinaweibo.HttpPost("statuses/upload.json", new { status = status, pic = info }).Content.ReadAsStringAsync().Result;
            }
            return this.sinaweibo.HttpPost("statuses/update.json", new { status = status }).Content.ReadAsStringAsync().Result;
        }

        public string user_timeline(int page)
        {
            string result = "{\"IsAuthorized\":\"0\"}";
            if (this.sinaweibo.IsAuthorized)
            {
                result = this.sinaweibo.HttpGet("statuses/user_timeline.json", new { page = page }).Content.ReadAsStringAsync().Result;
            }
            return result;
        }

        public string userinfo()
        {
            string str = "{\"IsAuthorized\":\"0\"}";
            if (!this.sinaweibo.IsAuthorized)
            {
                return str;
            }
            if (string.IsNullOrEmpty(this.get_uid()))
            {
                return "{\"IsAuthorized\":\"0\"}";
            }
            return this.sinaweibo.HttpGet("users/show.json", new { uid = this.get_uid() }).Content.ReadAsStringAsync().Result;
        }

        public string userinfo(string id)
        {
            string str = "{\"IsAuthorized\":\"0\"}";
            if (!this.sinaweibo.IsAuthorized)
            {
                return str;
            }
            if (string.IsNullOrEmpty(this.get_uid()))
            {
                return "{\"IsAuthorized\":\"0\"}";
            }
            return this.sinaweibo.HttpGet("users/show.json", new { uid = id }).Content.ReadAsStringAsync().Result;
        }

        public class user
        {
            public string uid { get; set; }
        }
    }
}

