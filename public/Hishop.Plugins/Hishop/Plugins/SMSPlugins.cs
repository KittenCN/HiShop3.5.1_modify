namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web;

    public class SMSPlugins : PluginContainer
    {
        private static volatile SMSPlugins instance = null;
        private static readonly object LockHelper = new object();

        private SMSPlugins()
        {
        }

        public override PluginItem GetPluginItem(string fullName)
        {
            return base.GetPluginItem("SMSSender", fullName);
        }

        public override PluginItemCollection GetPlugins()
        {
            return base.GetPlugins("SMSSender");
        }

        public static SMSPlugins Instance()
        {
            if (instance == null)
            {
                lock (LockHelper)
                {
                    if (instance == null)
                    {
                        instance = new SMSPlugins();
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
                return "plugin-sms-index";
            }
        }

        protected override string PluginLocalPath
        {
            get
            {
                return HttpContext.Current.Request.MapPath("~/plugins/sms");
            }
        }

        protected override string PluginVirtualPath
        {
            get
            {
                return (Utils.ApplicationPath + "/plugins/sms");
            }
        }

        protected override string TypeCacheKey
        {
            get
            {
                return "plugin-sms-type";
            }
        }
    }
}

