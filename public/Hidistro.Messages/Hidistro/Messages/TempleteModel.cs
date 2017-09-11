namespace Hidistro.Messages
{
    using System;
    using System.Runtime.CompilerServices;

    public class TempleteModel
    {
        public TempleteModel()
        {
            this.topcolor = "#FF0000";
        }

        public object data { get; set; }

        public string template_id { get; set; }

        public string topcolor { get; set; }

        public string touser { get; set; }

        public string url { get; set; }
    }
}

