namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Sales;
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ExpressPrintButton : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            DataTable isUserExpressTemplates = SalesHelper.GetIsUserExpressTemplates();
            if ((isUserExpressTemplates != null) && (isUserExpressTemplates.Rows.Count > 0))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<div>");
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
                {
                    foreach (DataRow row in isUserExpressTemplates.Rows)
                    {
                        builder.AppendFormat("<a href=\"flex/print.html?ShipperId={0}&OrderId={1}&XmlFile={2}\" style=\"margin-right:10px;\" >{3}</a> ", new object[] { this.ShipperId, this.Page.Request.QueryString["OrderId"], row["XmlFile"], row["ExpressName"] });
                    }
                }
                else if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
                {
                    foreach (DataRow row2 in isUserExpressTemplates.Rows)
                    {
                        builder.AppendFormat("<a href=\"flex/print.html?ShipperId={0}&PurchaseOrderId={1}&XmlFile={2}\" style=\"margin-right:10px;\" >{3}</a> ", new object[] { this.ShipperId, this.Page.Request.QueryString["PurchaseOrderId"], row2["XmlFile"], row2["ExpressName"] });
                    }
                }
                builder.Append("</div>");
                writer.Write(builder.ToString());
            }
        }

        public int ShipperId { get; set; }
    }
}

