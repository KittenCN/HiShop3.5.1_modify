namespace Hidistro.Messages
{
    using System;
    using System.Runtime.CompilerServices;

    public class GetIndustry_Item
    {
        public IndustryCode ConvertToIndustryCode()
        {
            IndustryCode code;
            if (!Enum.TryParse<IndustryCode>(string.Format("{0}_{1}", this.first_class, this.second_class.Replace("|", "_").Replace("/", "_")), true, out code))
            {
                return IndustryCode.其它_其它;
            }
            return code;
        }

        public string first_class { get; set; }

        public string second_class { get; set; }
    }
}

