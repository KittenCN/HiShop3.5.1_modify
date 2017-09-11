namespace Hishop.Plugins
{
    using System;
    using System.Runtime.InteropServices;
    using System.Xml;

    public abstract class SMSSender : ConfigablePlugin, IPlugin
    {
        protected SMSSender()
        {
        }

        public static SMSSender CreateInstance(string name)
        {
            return CreateInstance(name, null);
        }

        public static SMSSender CreateInstance(string name, string configXml)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            Type plugin = SMSPlugins.Instance().GetPlugin("SMSSender", name);
            if (plugin == null)
            {
                return null;
            }
            SMSSender sender = Activator.CreateInstance(plugin) as SMSSender;
            if (!((sender == null) || string.IsNullOrEmpty(configXml)))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                sender.InitConfig(document.FirstChild);
            }
            return sender;
        }

        public abstract bool Send(string cellPhone, string message, out string returnMsg);
        public abstract bool Send(string[] phoneNumbers, string message, out string returnMsg);
        public abstract bool Send(string cellPhone, string message, out string returnMsg, string speed = "0");
        public abstract bool Send(string[] phoneNumbers, string message, out string returnMsg, string speed = "1");
    }
}

