namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_CouponSelect : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            DataTable coupon = ShoppingProcessor.GetCoupon(this.CartTotal);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择一张优惠券<span class=\"caret\"></span></button>");
            builder.AppendLine("<ul class=\"dropdown-menu\" role=\"menu\">");
            if (coupon != null)
            {
                foreach (DataRow row in coupon.Rows)
                {
                    object[] args = new object[] { row["ClaimCode"], row["Name"], ((decimal) row["Amount"]).ToString("F2"), ((decimal) row["DiscountValue"]).ToString("F2") };
                    builder.AppendFormat("<li><a href=\"#\" name=\"{0}\" value=\"{3}\">{1}(满{2}减{3})</a></li>", args).AppendLine();
                }
            }
            builder.AppendLine("</ul>");
            writer.Write(builder.ToString());
        }

        public decimal CartTotal { get; set; }
    }
}

