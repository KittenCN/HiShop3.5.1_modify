namespace Hishop.Plugins
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;

    public abstract class OpenIdNotify : IPlugin
    {
        public event EventHandler<AuthenticatedEventArgs> Authenticated;

        public event EventHandler<FailedEventArgs> Failed;

        protected OpenIdNotify()
        {
        }

        public static OpenIdNotify CreateInstance(string name, NameValueCollection parameters)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            object[] args = new object[] { parameters };
            OpenIdPlugins plugins = OpenIdPlugins.Instance();
            Type plugin = plugins.GetPlugin("OpenIdService", name);
            if (plugin == null)
            {
                return null;
            }
            Type pluginWithNamespace = plugins.GetPluginWithNamespace("OpenIdNotify", plugin.Namespace);
            if (pluginWithNamespace == null)
            {
                return null;
            }
            return (Activator.CreateInstance(pluginWithNamespace, args) as OpenIdNotify);
        }

        protected virtual string GetResponse(string url, int timeout)
        {
            string str;
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Timeout = timeout;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Default))
                    {
                        StringBuilder builder = new StringBuilder();
                        while (-1 != reader.Peek())
                        {
                            builder.Append(reader.ReadLine());
                        }
                        return builder.ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                str = "Error:" + exception.Message;
            }
            return str;
        }

        protected virtual void OnAuthenticated(string openId)
        {
            if (this.Authenticated != null)
            {
                this.Authenticated(this, new AuthenticatedEventArgs(openId));
            }
        }

        protected virtual void OnFailed(string message)
        {
            if (this.Failed != null)
            {
                this.Failed(this, new FailedEventArgs(message));
            }
        }

        public abstract void Verify(int timeout, string configXml);
    }
}

