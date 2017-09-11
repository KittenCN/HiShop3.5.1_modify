namespace Hishop.Plugins
{
    using System;
    using System.Net.Mail;
    using System.Text;
    using System.Xml;

    public abstract class EmailSender : ConfigablePlugin, IPlugin
    {
        protected EmailSender()
        {
        }

        public static EmailSender CreateInstance(string name)
        {
            return CreateInstance(name, null);
        }

        public static EmailSender CreateInstance(string name, string configXml)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            Type plugin = EmailPlugins.Instance().GetPlugin("EmailSender", name);
            if (plugin == null)
            {
                return null;
            }
            EmailSender sender = Activator.CreateInstance(plugin) as EmailSender;
            if (!((sender == null) || string.IsNullOrEmpty(configXml)))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                sender.InitConfig(document.FirstChild);
            }
            return sender;
        }

        public abstract bool Send(MailMessage mail, Encoding emailEncoding);
    }
}

