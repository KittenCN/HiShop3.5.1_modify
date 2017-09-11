namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web;

    public class OpenIdPlugins : PluginContainer
    {
        private static volatile OpenIdPlugins instance = null;
        private static readonly object LockHelper = new object();

        private OpenIdPlugins()
        {
        }

        public override PluginItem GetPluginItem(string fullName)
        {
            return base.GetPluginItem("OpenIdService", fullName);
        }

        public override PluginItemCollection GetPlugins()
        {
            return base.GetPlugins("OpenIdService");
        }

        public static OpenIdPlugins Instance()
        {
            if (instance == null)
            {
                lock (LockHelper)
                {
                    if (instance == null)
                    {
                        instance = new OpenIdPlugins();
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
                return "plugin-openid-index";
            }
        }

        protected override string PluginLocalPath
        {
            get
            {
                return HttpContext.Current.Request.MapPath("~/plugins/openid");
            }
        }

        protected override string PluginVirtualPath
        {
            get
            {
                return (Utils.ApplicationPath + "/plugins/openid");
            }
        }

        protected override string TypeCacheKey
        {
            get
            {
                return "plugin-openid-type";
            }
        }
    }
}

