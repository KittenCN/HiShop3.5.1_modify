namespace HiTemplate
{
    using RazorEngine;
    using RazorEngine.Templating;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.Caching;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class RazorModuleWebControl : WebControl
    {
        protected virtual void RenderModule(HtmlTextWriter writer, object jsonData)
        {
            string name = "TemplateCacheKey-" + this.ID;
            string key = "TemplateFileCacheKey-" + this.ID;
            string str3 = "";
            string str4 = HttpContext.Current.Cache[key] as string;
            if (string.IsNullOrEmpty(str4) || (str4.Length == 0))
            {
                string path = HttpContext.Current.Request.MapPath(this.TemplateFile);
                str4 = File.ReadAllText(path);
                try
                {
                    str3 = Engine.Razor.RunCompile((ITemplateSource) new LoadedTemplateSource(str4, null), name, null, jsonData, null);
                    HttpContext.Current.Cache.Insert(key, str4, new CacheDependency(path), DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
                }
                catch
                {
                }
                writer.Write(str3);
            }
            else
            {
                str3 = Engine.Razor.IsTemplateCached(name, null) ? Engine.Razor.Run(name, null, jsonData, null) : Engine.Razor.RunCompile(((ITemplateSource) new LoadedTemplateSource(str4, null)), name, null, jsonData, null);
                writer.Write(str3);
            }
        }

        [Bindable(true)]
        public string DataUrl { get; set; }

        [Bindable(true)]
        public string Layout { get; set; }

        [Bindable(true)]
        public string ShowIco { get; set; }

        [Bindable(true)]
        public string showMaketPrice { get; set; }

        [Bindable(true)]
        public string ShowName { get; set; }

        [Bindable(true)]
        public string ShowPrice { get; set; }

        [Bindable(true)]
        public string TemplateFile { get; set; }
    }
}

