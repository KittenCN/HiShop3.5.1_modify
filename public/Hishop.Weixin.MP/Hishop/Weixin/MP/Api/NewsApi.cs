namespace Hishop.Weixin.MP.Api
{
    using Hishop.Weixin.MP.Util;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;

    public class NewsApi
    {
        private static object LockLog = new object();

        public static string CreateImageNewsJson(string media_id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"filter\":{\"is_to_all\":true},");
            builder.Append("\"msgtype\":\"mpnews\",");
            builder.Append("\"mpnews\":{\"media_id\":\"" + media_id + "\"}");
            builder.Append("}");
            return builder.ToString();
        }

        public static string CreateImageNewsJson(string media_id, List<string> openidList)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"touser\":[");
            builder.Append(string.Join(",", openidList.ConvertAll<string>(a => "\"" + a + "\"").ToArray()));
            builder.Append("],");
            builder.Append("\"msgtype\":\"mpnews\",");
            builder.Append("\"mpnews\":{\"media_id\":\"" + media_id + "\"}");
            builder.Append("}");
            return builder.ToString();
        }

        public static string CreateKFImageNewsJson(string openid, string media_id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"touser\":\"" + openid + "\",\"msgtype\":\"image\",\"image\": {\"media_id\":\"" + media_id + "\"}}");
            return builder.ToString();
        }

        public static string CreateKFTxtNewsJson(string openid, string content)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"touser\":\"" + openid + "\",\"msgtype\":\"text\",\"text\": {\"content\":\"" + content + "\"}}");
            return builder.ToString();
        }

        public static string CreateTxtNewsJson(string media_id)
        {
            string str = "内容测试";
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"filter\":{\"is_to_all\":true},");
            builder.Append("\"text\":{\"content\":\"" + str + "\"},");
            builder.Append("\"mpnews\":{\"media_id\":\"" + media_id + "\"}");
            builder.Append("}");
            return builder.ToString();
        }

        public static void Debuglog(string log, string logname = "_Debuglog.txt")
        {
            object obj2;
            Monitor.Enter(obj2 = LockLog);
            try
            {
                StreamWriter writer = System.IO.File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "App_Data/" + (DateTime.Now.ToString("yyyyMMdd") + logname));
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + log);
                writer.WriteLine("---------------");
                writer.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                Monitor.Exit(obj2);
            }
        }

        public static string GetArticlesJsonStr(string access_token, DataTable dt)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"articles\":[");
            int num = 0;
            foreach (DataRow row in dt.Rows)
            {
                string path = row["ImgUrl"].ToString();
                if (!System.IO.File.Exists(path))
                {
                    return "{\"code\":0,\"msg\":\"要发送的图片不存在\"}";
                }
                string jsonValue = GetJsonValue(UploadMedia(access_token, "image", path), "media_id");
                if (!string.IsNullOrEmpty(jsonValue))
                {
                    builder.Append("{");
                    builder.Append("\"thumb_media_id\":\"" + jsonValue + "\",");
                    builder.Append("\"author\":\"" + row["Author"].ToString() + "\",");
                    builder.Append("\"title\":\"" + row["Title"].ToString() + "\",");
                    builder.Append("\"content_source_url\":\"" + row["TextUrl"].ToString() + "\",");
                    builder.Append("\"content\":\"" + row["Content"].ToString() + "\",");
                    builder.Append("\"digest\":\"" + row["Content"].ToString() + "\",");
                    if (num == (dt.Rows.Count - 1))
                    {
                        builder.Append("\"show_cover_pic\":\"1\"}");
                    }
                    else
                    {
                        builder.Append("\"show_cover_pic\":\"1\"},");
                    }
                }
                num++;
            }
            builder.Append("]}");
            return builder.ToString();
        }

        public static string GetErrorCodeMsg(string errcode)
        {
            switch (errcode)
            {
                case "45028":
                    return "发送条数已经用完";

                case "-1":
                    return "系统繁忙，此时请开发者稍候再试";

                case "0":
                    return "请求成功";

                case "40001":
                    return "获取access_token时AppSecret错误，或者access_token无效。请开发者认真比对AppSecret的正确性，或查看是否正在为恰当的公众号调用接口";

                case "40002":
                    return "不合法的凭证类型";

                case "40003":
                    return "不合法的OpenID，请开发者确认OpenID（该用户）是否已关注公众号，或是否是其他公众号的OpenID";

                case "40004":
                    return "不合法的媒体文件类型";

                case "40005":
                    return "不合法的文件类型";

                case "40006":
                    return "不合法的文件大小";

                case "40007":
                    return "不合法的媒体文件id";

                case "40008":
                    return "不合法的消息类型";

                case "40009":
                    return "不合法的图片文件大小";

                case "40010":
                    return "不合法的语音文件大小";

                case "40011":
                    return "不合法的视频文件大小";

                case "40012":
                    return "不合法的缩略图文件大小";

                case "40013":
                    return "不合法的AppID，请开发者检查AppID的正确性，避免异常字符，注意大小写";

                case "40014":
                    return "不合法的access_token，请开发者认真比对access_token的有效性（如是否过期），或查看是否正在为恰当的公众号调用接口";

                case "40015":
                    return "不合法的菜单类型";

                case "40016":
                    return "不合法的按钮个数";

                case "40017":
                    return "不合法的按钮个数";

                case "40018":
                    return "不合法的按钮名字长度";

                case "40019":
                    return "不合法的按钮KEY长度";

                case "40020":
                    return "不合法的按钮URL长度";

                case "40021":
                    return "不合法的菜单版本号";

                case "40022":
                    return "不合法的子菜单级数";

                case "40023":
                    return "不合法的子菜单按钮个数";

                case "40024":
                    return "不合法的子菜单按钮类型";

                case "40025":
                    return "不合法的子菜单按钮名字长度";

                case "40026":
                    return "不合法的子菜单按钮KEY长度";

                case "40027":
                    return "不合法的子菜单按钮URL长度";

                case "40028":
                    return "不合法的自定义菜单使用用户";

                case "40029":
                    return "不合法的oauth_code";

                case "40030":
                    return "不合法的refresh_token";

                case "40031":
                    return "不合法的openid列表";

                case "40032":
                    return "不合法的openid列表长度";

                case "40033":
                    return @"不合法的请求字符，不能包含\uxxxx格式的字符";

                case "40035":
                    return "不合法的参数";

                case "40038":
                    return "不合法的请求格式";

                case "40039":
                    return "不合法的URL长度";

                case "40050":
                    return "不合法的分组id";

                case "40051":
                    return "分组名字不合法";

                case "40117":
                    return "分组名字不合法";

                case "40118":
                    return "media_id大小不合法";

                case "40119":
                    return "button类型错误";

                case "40120":
                    return "button类型错误";

                case "40121":
                    return "不合法的media_id类型";

                case "40132":
                    return "微信号不合法";

                case "40137":
                    return "不支持的图片格式";

                case "41001":
                    return "缺少access_token参数";

                case "41002":
                    return "缺少appid参数";

                case "41003":
                    return "缺少refresh_token参数";

                case "41004":
                    return "缺少secret参数";

                case "41005":
                    return "缺少多媒体文件数据";

                case "41006":
                    return "缺少media_id参数";

                case "41007":
                    return "缺少子菜单数据";

                case "41008":
                    return "缺少oauth code";

                case "41009":
                    return "缺少openid";

                case "42001":
                    return "access_token超时，请检查access_token的有效期，请参考基础支持-获取access_token中，对access_token的详细机制说明";

                case "42002":
                    return "refresh_token超时";

                case "42003":
                    return "oauth_code超时";

                case "43001":
                    return "需要GET请求";

                case "43002":
                    return "需要POST请求";

                case "43003":
                    return "需要HTTPS请求";

                case "43004":
                    return "需要接收者关注";

                case "43005":
                    return "需要好友关系";

                case "44001":
                    return "多媒体文件为空";

                case "44002":
                    return "POST的数据包为空";

                case "44003":
                    return "图文消息内容为空";

                case "44004":
                    return "文本消息内容为空";

                case "45001":
                    return "多媒体文件大小超过限制";

                case "45002":
                    return "消息内容超过限制";

                case "45003":
                    return "标题字段超过限制";

                case "45004":
                    return "描述字段超过限制";

                case "45005":
                    return "链接字段超过限制";

                case "45006":
                    return "图片链接字段超过限制";

                case "45007":
                    return "语音播放时间超过限制";

                case "45008":
                    return "图文消息超过限制";

                case "45009":
                    return "接口调用超过限制";

                case "45010":
                    return "创建菜单个数超过限制";

                case "45015":
                    return "回复时间超过限制";

                case "45016":
                    return "系统分组，不允许修改";

                case "45017":
                    return "分组名字过长";

                case "45018":
                    return "分组数量超过上限";

                case "46001":
                    return "不存在媒体数据";

                case "46002":
                    return "不存在的菜单版本";

                case "46003":
                    return "不存在的菜单数据";

                case "46004":
                    return "不存在的用户";

                case "47001":
                    return "解析JSON/XML内容错误";

                case "48001":
                    return "api功能未授权，请确认公众号已获得该接口，可以在公众平台官网-开发者中心页中查看接口权限";

                case "50001":
                    return "用户未授权该api";

                case "50002":
                    return "用户受限，可能是违规后接口被封禁";

                case "61451":
                    return "参数错误(invalid parameter)";

                case "61452":
                    return "无效客服账号(invalid kf_account)";

                case "61453":
                    return "客服帐号已存在(kf_account exsited)";

                case "61454":
                    return "客服帐号名长度超过限制(仅允许10个英文字符，不包括@及@后的公众号的微信号)(invalid kf_acount length)";

                case "61455":
                    return "客服帐号名包含非法字符(仅允许英文+数字)(illegal character in kf_account)";

                case "61456":
                    return "客服帐号个数超过限制(10个客服账号)(kf_account count exceeded)";

                case "61457":
                    return "无效头像文件类型(invalid file type)";

                case "61450":
                    return "系统错误(system error)";

                case "61500":
                    return "日期格式错误";

                case "61501":
                    return "日期范围错误";

                case "9001001":
                    return "POST数据参数不合法";

                case "9001002":
                    return "远端服务不可用";

                case "9001003":
                    return "Ticket不合法";

                case "9001004":
                    return "获取摇周边用户信息失败";

                case "9001005":
                    return "获取商户信息失败";

                case "9001006":
                    return "获取OpenID失败";

                case "9001007":
                    return "上传文件缺失";

                case "9001008":
                    return "上传素材的文件类型不合法";

                case "9001009":
                    return "上传素材的文件尺寸不合法";

                case "9001010":
                    return "上传失败";

                case "9001020":
                    return "帐号不合法";

                case "9001021":
                    return "已有设备激活率低于50%，不能新增设备";

                case "9001022":
                    return "设备申请数不合法，必须为大于0的数字";

                case "9001023":
                    return "已存在审核中的设备ID申请";

                case "9001024":
                    return "一次查询设备ID数量不能超过50";

                case "9001025":
                    return "设备ID不合法";

                case "9001026":
                    return "页面ID不合法";

                case "9001027":
                    return "页面参数不合法";

                case "9001028":
                    return "一次删除页面ID数量不能超过10";

                case "9001029":
                    return "页面已应用在设备中，请先解除应用关系再删除";

                case "9001030":
                    return "一次查询页面ID数量不能超过50";

                case "9001031":
                    return "时间区间不合法";

                case "9001032":
                    return "保存设备与页面的绑定关系参数错误";

                case "9001033":
                    return "门店ID不合法";

                case "9001034":
                    return "设备备注信息过长";

                case "9001035":
                    return "设备申请参数不合法";

                case "9001036":
                    return "查询起始值begin不合法 ";
            }
            return "未知错误";
        }

        public static string GetFilePath(string path)
        {
            string str = HttpRuntime.AppDomainAppPath.ToString();
            if (path.ToLower().StartsWith("http"))
            {
                return path;
            }
            if (path.ToLower().StartsWith("/") || path.ToLower().StartsWith(@"\"))
            {
                return (str + path);
            }
            return (str + @"\" + path);
        }

        public static string GetJsonValue(string msg, string Field)
        {
            string str = "";
            try
            {
                JObject obj2 = JObject.Parse(msg);
                if (obj2[Field] != null)
                {
                    str = obj2[Field].ToString();
                }
            }
            catch (Exception)
            {
                Debuglog(msg, "_debuglogtext.txt");
            }
            return str;
        }

        public static string GetMedia_IDByPath(string access_token, string path)
        {
            string input = path;
            if (input.StartsWith("http"))
            {
                input = Regex.Replace(input, "(http|https)://([^/]*)", "");
            }
            if (input.StartsWith("/"))
            {
                try
                {
                    return UploadMedia(access_token, "image", input);
                }
                catch (Exception exception)
                {
                    return exception.ToString();
                }
            }
            return "图片路径不对";
        }

        public static List<string> GetOpenIDs(string access_token)
        {
            List<string> list = new List<string>();
            List<string> openIDs = GetOpenIDs(access_token, null);
            list.AddRange(openIDs);
            while (openIDs.Count > 0)
            {
                openIDs = GetOpenIDs(access_token, openIDs[openIDs.Count - 1]);
                list.AddRange(openIDs);
            }
            return list;
        }

        public static List<string> GetOpenIDs(string access_token, string next_openid)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}", access_token, string.IsNullOrEmpty(next_openid) ? "" : next_openid);
            string msg = new WebUtils().DoPost(url, "");
            if (int.Parse(GetJsonValue(msg, "count")) > 0)
            {
                string str3 = "\"openid\":[";
                int startIndex = msg.IndexOf(str3) + str3.Length;
                int index = msg.IndexOf("]", startIndex);
                return msg.Substring(startIndex, index - startIndex).Replace("\"", "").Split(new char[] { ',' }).ToList<string>();
            }
            return new List<string>();
        }

        public static string HttpUploadFile(string url, string path)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer container = new CookieContainer();
            request.CookieContainer = container;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string str = DateTime.Now.Ticks.ToString("X");
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + str;
            byte[] bytes = Encoding.UTF8.GetBytes("\r\n--" + str + "\r\n");
            byte[] buffer = Encoding.UTF8.GetBytes("\r\n--" + str + "--\r\n");
            int num = path.LastIndexOf(@"\");
            string str2 = path.Substring(num + 1);
            StringBuilder builder = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", str2));
            byte[] buffer3 = Encoding.UTF8.GetBytes(builder.ToString());
            FileStream stream = new FileStream(GetFilePath(path), FileMode.Open, FileAccess.Read);
            byte[] buffer4 = new byte[stream.Length];
            stream.Read(buffer4, 0, buffer4.Length);
            stream.Close();
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Write(buffer3, 0, buffer3.Length);
            requestStream.Write(buffer4, 0, buffer4.Length);
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public static string KFSend(string access_token, string postData)
        {
            return new WebUtils().DoPost(string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", access_token), postData);
        }

        public static string Send(string access_token, string postData)
        {
            return new WebUtils().DoPost(string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}", access_token), postData);
        }

        public static string SendAll(string access_token, string postData)
        {
            return new WebUtils().DoPost(string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}", access_token), postData);
        }

        public static string UploadMedia(string access_token, string type, string path)
        {
            return HttpUploadFile(string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", access_token, type), path);
        }

        public static string UploadNews(string access_token, string postData)
        {
            return new WebUtils().DoPost(string.Format("https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}", access_token), postData);
        }
    }
}

