namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;

    public class PluginItem
    {
        public virtual string ToJsonString()
        {
            return ("{\"FullName\":\"" + this.FullName + "\",\"DisplayName\":\"" + this.DisplayName + "\",\"Logo\":\"" + this.Logo + "\",\"ShortDescription\":\"" + this.ShortDescription + "\",\"Description\":\"" + this.Description + "\"}");
        }

        public virtual string ToXmlString()
        {
            return ("<xml><FullName>" + this.FullName + "</FullName><DisplayName>" + this.DisplayName + "</DisplayName><Logo>" + this.Logo + "</Logo><ShortDescription>" + this.ShortDescription + "</ShortDescription><Description>" + this.Description + "</Description></xml>");
        }

        public virtual string Description { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string FullName { get; set; }

        public virtual string Logo { get; set; }

        public virtual string ShortDescription { get; set; }
    }
}

