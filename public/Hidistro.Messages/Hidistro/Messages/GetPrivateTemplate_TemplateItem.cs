namespace Hidistro.Messages
{
    using System;
    using System.Runtime.CompilerServices;

    public class GetPrivateTemplate_TemplateItem
    {
        public IndustryCode ConvertToIndustryCode()
        {
            IndustryCode code;
            if (!Enum.TryParse<IndustryCode>(string.Format("{0}_{1}", this.primary_industry, this.deputy_industry.Replace("|", "_").Replace("/", "_")), true, out code))
            {
                return IndustryCode.其它_其它;
            }
            return code;
        }

        public string content { get; set; }

        public string deputy_industry { get; set; }

        public string example { get; set; }

        public string primary_industry { get; set; }

        public string template_id { get; set; }

        public string title { get; set; }
    }
}

