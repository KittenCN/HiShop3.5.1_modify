namespace HiTemplate
{
    using HiTemplate.Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;

    public class GoodsMobule : RazorModuleWebControl
    {
        public string GetDataJson()
        {
            Exception exception;
            string str = "";
            try
            {
                string s = string.Format("ShowPrice={0}&Layout={1}&ShowIco={2}&ShowName={3}&IDs={4}", new object[] { base.ShowPrice, base.Layout, base.ShowIco, base.ShowName, this.IDs });
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                string applicationPath = Urls.ApplicationPath;
                if (string.IsNullOrEmpty(base.DataUrl))
                {
                    applicationPath = applicationPath + "/api/Hi_Ajax_GoodsList.ashx";
                }
                else
                {
                    applicationPath = applicationPath + base.DataUrl;
                }
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(applicationPath);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                try
                {
                    request.GetRequestStream().Write(bytes, 0, bytes.Length);
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    StringBuilder builder = new StringBuilder();
                    while (-1 != reader.Peek())
                    {
                        builder.Append(reader.ReadLine());
                    }
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (responseStream != null)
                    {
                        responseStream.Close();
                    }
                    if (response != null)
                    {
                        response.Close();
                    }
                    str = builder.ToString();
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    str = "错误：" + exception.Message;
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                str = "错误：" + exception.Message;
            }
            return str;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                Hi_Json_GoodGourpContent jsonData = ((JObject) JsonConvert.DeserializeObject(this.GetDataJson())).ToObject<Hi_Json_GoodGourpContent>();
                base.RenderModule(writer, jsonData);
            }
            catch (Exception)
            {
                base.RenderModule(writer, new Hi_Json_GoodGourpContent());
            }
        }

        [Bindable(true)]
        public string IDs { get; set; }
    }
}

