namespace Hidistro.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GetPrivateTemplateJsonResult : WxJsonResult
    {
        public List<GetPrivateTemplate_TemplateItem> template_list { get; set; }
    }
}

