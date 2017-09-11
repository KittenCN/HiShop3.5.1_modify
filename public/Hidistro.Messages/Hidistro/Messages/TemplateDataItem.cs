namespace Hidistro.Messages
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class TemplateDataItem
    {
        public TemplateDataItem(string v, string c = "#173177")
        {
            this.value = v;
            this.color = c;
        }

        public string color { get; set; }

        public string value { get; set; }
    }
}

