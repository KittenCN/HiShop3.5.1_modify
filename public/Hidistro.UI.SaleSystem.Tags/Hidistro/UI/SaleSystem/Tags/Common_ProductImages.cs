namespace Hidistro.UI.SaleSystem.Tags
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_ProductImages : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(this.ImageUrl1))
            {
                builder.AppendFormat(" <li><a><img src=\"{0}\"/></a></li>", this.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_")).AppendLine();
            }
            if (!string.IsNullOrEmpty(this.ImageUrl2))
            {
                builder.AppendFormat(" <li><a><img src=\"{0}\"/></a></li>", this.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_")).AppendLine();
            }
            if (!string.IsNullOrEmpty(this.ImageUrl3))
            {
                builder.AppendFormat(" <li><a><img src=\"{0}\"/></a></li>", this.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_")).AppendLine();
            }
            if (!string.IsNullOrEmpty(this.ImageUrl4))
            {
                builder.AppendFormat(" <li><a><img src=\"{0}\"/></a></li>", this.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_")).AppendLine();
            }
            if (!string.IsNullOrEmpty(this.ImageUrl5))
            {
                builder.AppendFormat(" <li><a><img src=\"{0}\"/></a></li>", this.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_")).AppendLine();
            }
            writer.Write(builder.ToString());
        }

        public string ImageUrl1 { get; set; }

        public string ImageUrl2 { get; set; }

        public string ImageUrl3 { get; set; }

        public string ImageUrl4 { get; set; }

        public string ImageUrl5 { get; set; }
    }
}

