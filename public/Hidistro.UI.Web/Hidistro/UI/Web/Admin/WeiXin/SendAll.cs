namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Weixin.MP.Api;
    using System;
    using System.Data;
    using System.Text;

    public class SendAll : AdminPage
    {
        protected SendAll() : base("m06", "wxp04")
        {
        }

        public static string GetArticlesJsonStr(DataTable dt)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"articles\":[");
            int num = 0;
            foreach (DataRow row in dt.Rows)
            {
                string str = row["media_id"].ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    builder.Append("{");
                    builder.Append("\"thumb_media_id\":\"" + str + "\",");
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

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public string Send(string access_token, string openid)
        {
            string text1 = base.Request["type"];
            string text2 = base.Request["data"];
            string postData = "{\"touser\":\"" + openid + "\",\"msgtype\":\"text\",\"text\": {\"content\":\"欢迎您的来访！\"}}";
            string str2 = null;
            string jsonValue = NewsApi.GetJsonValue(NewsApi.KFSend(access_token, postData), "media_id");
            str2 = NewsApi.Send(access_token, NewsApi.CreateImageNewsJson(jsonValue));
            if (string.IsNullOrWhiteSpace(str2))
            {
                return "{\"code\":0,\"msg\":\"type参数错误\"}";
            }
            string str5 = NewsApi.GetJsonValue(str2, "errcode");
            string str6 = NewsApi.GetJsonValue(str2, "errmsg");
            if (str5 == "0")
            {
                return "{\"code\":1,\"msg\":\"\"}";
            }
            return ("{\"code\":0,\"msg\":\"errcode:" + str5 + ", errmsg:" + str6 + "\"}");
        }
    }
}

