namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web;

    public class EmailPlugins : PluginContainer
    {
        private static volatile EmailPlugins instance = null;
        private static readonly object LockHelper = new object();

        private EmailPlugins()
        {
        }

        public override PluginItem GetPluginItem(string fullName)
        {
            return base.GetPluginItem("EmailSender", fullName);
        }

        public override PluginItemCollection GetPlugins()
        {
            return base.GetPlugins("EmailSender");
        }

        public static EmailPlugins Instance()
        {
            if (instance == null)
            {
                lock (LockHelper)
                {
                    if (instance == null)
                    {
                        instance = new EmailPlugins();
                    }
                }
            }
            instance.VerifyIndex();
            return instance;
        }

        protected override string IndexCacheKey
        {
            get
            {
                return "plugin-email-index";
            }
        }

        protected override string PluginLocalPath
        {
            get
            {
                return HttpContext.Current.Request.MapPath("~/plugins/email");
            }
        }

        protected override string PluginVirtualPath
        {
            get
            {
                return (Utils.ApplicationPath + "/plugins/email");
            }
        }

        protected override string TypeCacheKey
        {
            get
            {
                return "plugin-email-type";
            }
        }
    }
}

