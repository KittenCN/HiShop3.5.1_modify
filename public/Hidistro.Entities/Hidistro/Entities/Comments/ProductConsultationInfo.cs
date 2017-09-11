namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductConsultationInfo
    {
        public DateTime ConsultationDate { get; set; }

        public int ConsultationId { get; set; }

        [HtmlCoding, StringLengthValidator(1, 300, Ruleset="Refer", MessageTemplate="咨询内容为必填项，长度限制在300字符以内")]
        public string ConsultationText { get; set; }

        public int ProductId { get; set; }

        public DateTime? ReplyDate { get; set; }

        [NotNullValidator(Ruleset="Reply", MessageTemplate="回内容为必填项")]
        public string ReplyText { get; set; }

        public int? ReplyUserId { get; set; }

        [StringLengthValidator(1, 0x100, Ruleset="Refer", MessageTemplate="邮箱不能为空，长度限制在256字符以内"), RegexValidator(@"^[a-zA-Z\.0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", Ruleset="Refer", MessageTemplate="邮箱地址必须为有效格式")]
        public string UserEmail { get; set; }

        public int UserId { get; set; }

        [StringLengthValidator(1, 30, Ruleset="Refer", MessageTemplate="用户昵称为必填项，长度限制在30字符以内"), HtmlCoding]
        public string UserName { get; set; }
    }
}

