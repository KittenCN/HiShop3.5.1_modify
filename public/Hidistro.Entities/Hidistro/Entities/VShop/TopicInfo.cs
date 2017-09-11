namespace Hidistro.Entities.VShop
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class TopicInfo
    {
        public DateTime AddedDate { get; set; }

        [StringLengthValidator(1, 0x3b9ac9ff, Ruleset="ValTopicInfo", MessageTemplate="专题内容不能为空")]
        public string Content { get; set; }

        public int DisplaySequence { get; set; }

        public string IconUrl { get; set; }

        public bool IsRelease { get; set; }

        public string Keys { get; set; }

        [StringLengthValidator(1, 60, Ruleset="ValTopicInfo", MessageTemplate="专题标题不能为空，长度限制在60个字符以内"), HtmlCoding]
        public string Title { get; set; }

        public int TopicId { get; set; }
    }
}

