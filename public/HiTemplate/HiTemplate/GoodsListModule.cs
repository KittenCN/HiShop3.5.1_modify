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

    public class GoodsListModule : RazorModuleWebControl
    {
        public string GetDataJson()
        {
            Exception exception;
            string str = "";
            try
            {
                if (string.IsNullOrEmpty(base.showMaketPrice))
                {
                    base.showMaketPrice = "true";
                }
                string s = string.Format("GroupID={0}&GoodListSize={1}&FirstPriority={2}&SecondPriority={3}&ShowPrice={4}&Layout={5}&ShowIco={6}&ShowName={7}&ShowMaketPrice={8}", new object[] { this.GroupID, this.GoodListSize, this.FirstPriority, this.SecondPriority, base.ShowPrice, base.Layout, base.ShowIco, base.ShowName, base.showMaketPrice });
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                string applicationPath = Urls.ApplicationPath;
                if (string.IsNullOrEmpty(base.DataUrl))
                {
                    applicationPath = applicationPath + "/api/Hi_Ajax_GoodsListGroup.ashx";
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
            catch
            {
                base.RenderModule(writer, new Hi_Json_GoodGourpContent());
            }
        }

        [Bindable(true)]
        public string FirstPriority { get; set; }

        [Bindable(true)]
        public string GoodListSize { get; set; }

        [Bindable(true)]
        public string GroupID { get; set; }

        [Bindable(true)]
        public string SecondPriority { get; set; }

        [Bindable(true)]
        public string ShowOrder { get; set; }

        [Bindable(true)]
        public string ThirdPriority { get; set; }
    }
}

