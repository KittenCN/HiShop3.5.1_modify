namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_SKUSelector : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            DataTable skus = ProductBrowser.GetSkus(this.ProductId);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<input type=\"hidden\" id=\"hiddenSkuId\" value=\"{0}_0\"  />", this.ProductId).AppendLine();
            if ((skus != null) && (skus.Rows.Count > 0))
            {
                IList<string> list = new List<string>();
                builder.AppendFormat("<input type=\"hidden\" id=\"hiddenProductId\" value=\"{0}\" />", this.ProductId).AppendLine();
                builder.AppendLine("<div class=\"specification\">");
                foreach (DataRow row in skus.Rows)
                {
                    if (!list.Contains((string) row["AttributeName"]))
                    {
                        list.Add((string) row["AttributeName"]);
                        builder.AppendFormat("<div class=\"title text-muted\">{0}：</div><input type=\"hidden\" name=\"skuCountname\" AttributeName=\"{0}\" id=\"skuContent_{1}\" />", row["AttributeName"], row["AttributeId"]);
                        builder.AppendFormat("<div class=\"list clearfix\" id=\"skuRow_{0}\">", row["AttributeId"]);
                        IList<string> list2 = new List<string>();
                        foreach (DataRow row2 in skus.Rows)
                        {
                            if ((string.Compare((string) row["AttributeName"], (string) row2["AttributeName"]) == 0) && !list2.Contains((string) row2["ValueStr"]))
                            {
                                string str = string.Concat(new object[] { "skuValueId_", row["AttributeId"], "_", row2["ValueId"] });
                                list2.Add((string) row2["ValueStr"]);
                                builder.AppendFormat("<div class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\">{3}</div>", new object[] { str, row["AttributeId"], row2["ValueId"], (row2["ImageUrl"].ToString() != "") ? ("<img src='" + row2["ImageUrl"] + "' width='50px' height='35px'></img>") : row2["ValueStr"] });
                            }
                        }
                        builder.AppendLine("</div>");
                    }
                }
                builder.AppendLine("</div>");
            }
            writer.Write(builder.ToString());
        }

        public int ProductId { get; set; }
    }
}

