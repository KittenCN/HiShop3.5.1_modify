using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;

public class Crawler
{
    public Crawler(string sourceUrl, HttpServerUtility server)
    {
        this.SourceUrl = sourceUrl;
        this.Server = server;
    }

    public Crawler Fetch()
    {
        HttpWebRequest request = WebRequest.Create(this.SourceUrl) as HttpWebRequest;
        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                this.State = string.Concat(new object[] { "Url returns ", response.StatusCode, ", ", response.StatusDescription });
                return this;
            }
            if (response.ContentType.IndexOf("image") == -1)
            {
                this.State = "Url is not an image";
                return this;
            }
            this.ServerUrl = PathFormatter.Format(Path.GetFileName(this.SourceUrl), Config.GetString("catcherPathFormat"));
            string path = this.Server.MapPath(this.ServerUrl);
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            try
            {
                byte[] buffer;
                BinaryReader reader = new BinaryReader(response.GetResponseStream());
                using (MemoryStream stream2 = new MemoryStream())
                {
                    int num;
                    byte[] buffer2 = new byte[0x1000];
                    while ((num = reader.Read(buffer2, 0, buffer2.Length)) != 0)
                    {
                        stream2.Write(buffer2, 0, num);
                    }
                    buffer = stream2.ToArray();
                }
                System.IO.File.WriteAllBytes(path, buffer);
                this.State = "SUCCESS";
            }
            catch (Exception exception)
            {
                this.State = "抓取错误：" + exception.Message;
            }
            return this;
        }
    }

    private HttpServerUtility Server { get; set; }

    public string ServerUrl { get; set; }

    public string SourceUrl { get; set; }

    public string State { get; set; }
}

