namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web;

    public class PaymentPlugins : PluginContainer
    {
        private static volatile PaymentPlugins instance = null;
        private static readonly object LockHelper = new object();

        private PaymentPlugins()
        {
        }

        public override PluginItem GetPluginItem(string fullName)
        {
            return base.GetPluginItem("PaymentRequest", fullName);
        }

        public override PluginItemCollection GetPlugins()
        {
            return base.GetPlugins("PaymentRequest");
        }

        public static PaymentPlugins Instance()
        {
            if (instance == null)
            {
                lock (LockHelper)
                {
                    if (instance == null)
                    {
                        instance = new PaymentPlugins();
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
                return "plugin-payment-index";
            }
        }

        protected override string PluginLocalPath
        {
            get
            {
                return HttpContext.Current.Request.MapPath("~/plugins/payment");
            }
        }

        protected override string PluginVirtualPath
        {
            get
            {
                return (Utils.ApplicationPath + "/plugins/payment");
            }
        }

        protected override string TypeCacheKey
        {
            get
            {
                return "plugin-payment-type";
            }
        }
    }
}

