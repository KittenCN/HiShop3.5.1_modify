namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web;

    public class LogisticsPlugins : PluginContainer
    {
        private static volatile LogisticsPlugins instance = null;
        private static readonly object LockHelper = new object();

        private LogisticsPlugins()
        {
        }

        public override PluginItem GetPluginItem(string fullName)
        {
            return null;
        }

        public override PluginItemCollection GetPlugins()
        {
            throw new NotImplementedException();
        }

        public static LogisticsPlugins Instance()
        {
            if (instance == null)
            {
                lock (LockHelper)
                {
                    if (instance == null)
                    {
                        instance = new LogisticsPlugins();
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
                return "plugin-logistics-index";
            }
        }

        protected override string PluginLocalPath
        {
            get
            {
                return HttpContext.Current.Request.MapPath("~/plugins/logistics");
            }
        }

        protected override string PluginVirtualPath
        {
            get
            {
                return (Utils.ApplicationPath + "/plugins/logistics");
            }
        }

        protected override string TypeCacheKey
        {
            get
            {
                return "plugin-logistics-type";
            }
        }
    }
}

