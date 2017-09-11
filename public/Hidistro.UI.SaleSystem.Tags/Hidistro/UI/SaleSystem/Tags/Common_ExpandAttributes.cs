namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_ExpandAttributes : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            DataTable expandAttributes = ProductBrowser.GetExpandAttributes(this.ProductId);
            StringBuilder builder = new StringBuilder();
            if ((expandAttributes != null) && (expandAttributes.Rows.Count > 0))
            {
                foreach (DataRow row in expandAttributes.Rows)
                {
                    builder.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", row["AttributeName"], row["ValueStr"]);
                }
            }
            writer.Write(builder.ToString());
        }

        public int ProductId { get; set; }
    }
}

