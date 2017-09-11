namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductConsultationAndReplyQuery : Pagination
    {
        public int? CategoryId { get; set; }

        public int ConsultationId { get; set; }

        public bool? HasReplied { get; set; }

        [HtmlCoding]
        public string Keywords { get; set; }

        [HtmlCoding]
        public string ProductCode { get; set; }

        public virtual int ProductId { get; set; }

        public ConsultationReplyType Type { get; set; }

        public int UserId { get; set; }
    }
}

