namespace WebSite.Public.plugins.ueditor
{
    using System;
    using System.Web;

    public class controller : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            Handler handler = null;
            switch (context.Request["action"])
            {
                case "config":
                    handler = new ConfigHandler(context);
                    break;

                case "uploadimage":
                {
                    UploadConfig config = new UploadConfig {
                        AllowExtensions = Config.GetStringList("imageAllowFiles"),
                        PathFormat = Config.GetString("imagePathFormat"),
                        SizeLimit = Config.GetInt("imageMaxSize"),
                        UploadFieldName = Config.GetString("imageFieldName")
                    };
                    handler = new UploadHandler(context, config);
                    break;
                }
                case "uploadscrawl":
                {
                    UploadConfig config2 = new UploadConfig {
                        AllowExtensions = new string[] { ".png" },
                        PathFormat = Config.GetString("scrawlPathFormat"),
                        SizeLimit = Config.GetInt("scrawlMaxSize"),
                        UploadFieldName = Config.GetString("scrawlFieldName"),
                        Base64 = true,
                        Base64Filename = "scrawl.png"
                    };
                    handler = new UploadHandler(context, config2);
                    break;
                }
                case "uploadvideo":
                {
                    UploadConfig config3 = new UploadConfig {
                        AllowExtensions = Config.GetStringList("videoAllowFiles"),
                        PathFormat = Config.GetString("videoPathFormat"),
                        SizeLimit = Config.GetInt("videoMaxSize"),
                        UploadFieldName = Config.GetString("videoFieldName")
                    };
                    handler = new UploadHandler(context, config3);
                    break;
                }
                case "uploadfile":
                {
                    UploadConfig config4 = new UploadConfig {
                        AllowExtensions = Config.GetStringList("fileAllowFiles"),
                        PathFormat = Config.GetString("filePathFormat"),
                        SizeLimit = Config.GetInt("fileMaxSize"),
                        UploadFieldName = Config.GetString("fileFieldName")
                    };
                    handler = new UploadHandler(context, config4);
                    break;
                }
                case "listimage":
                    handler = new ListFileManager(context, Config.GetString("imageManagerListPath"), Config.GetStringList("imageManagerAllowFiles"));
                    break;

                case "listfile":
                    handler = new ListFileManager(context, Config.GetString("fileManagerListPath"), Config.GetStringList("fileManagerAllowFiles"));
                    break;

                case "catchimage":
                    handler = new CrawlerHandler(context);
                    break;

                default:
                    handler = new NotSupportedHandler(context);
                    break;
            }
            handler.Process();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

