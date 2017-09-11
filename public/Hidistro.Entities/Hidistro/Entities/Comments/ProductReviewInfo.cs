namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductReviewInfo
    {
        public string OrderId { get; set; }

        public int OrderItemID { get; set; }

        public int ProductId { get; set; }

        public DateTime ReviewDate { get; set; }

        public long ReviewId { get; set; }

        [StringLengthValidator(1, 300, Ruleset="Refer", MessageTemplate="评论内容为必填项，长度限制在300字符以内"), HtmlCoding]
        public string ReviewText { get; set; }

        public string SkuId { get; set; }

        [StringLengthValidator(1, 0x100, Ruleset="Refer", MessageTemplate="邮箱不能为空，长度限制在256字符以内"), RegexValidator(@"^[a-zA-Z\.0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", Ruleset="Refer", MessageTemplate="邮箱地址必须为有效格式")]
        public string UserEmail { get; set; }

        public int UserId { get; set; }

        [HtmlCoding, StringLengthValidator(1, 30, Ruleset="Refer", MessageTemplate="用户昵称为必填项，长度限制在30字符以内")]
        public string UserName { get; set; }
    }
}

