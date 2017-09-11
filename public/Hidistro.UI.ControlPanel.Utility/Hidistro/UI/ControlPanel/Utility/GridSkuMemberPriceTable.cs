namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Commodities;
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class GridSkuMemberPriceTable : WebControl
    {
        private void CreateRow(DataRow row, DataTable dtSkus, StringBuilder sb)
        {
            string str = row["SkuId"].ToString();
            sb.AppendFormat("<tr class=\"SkuPriceRow\" skuId=\"{0}\" >", str).AppendLine();
            sb.AppendFormat("<td>&nbsp;{0}</td>", row["ProductCode"]).AppendLine();
            sb.AppendFormat("<td>{0} {1}</td>", row["ProductName"], row["SKUContent"]).AppendLine();
            sb.AppendFormat("<td>&nbsp;{0}</td>", (row["MarketPrice"] != DBNull.Value) ? decimal.Parse(row["MarketPrice"].ToString()).ToString("F2") : "").AppendLine();
            sb.AppendFormat("<td style=\"display:none\"><input type=\"text\" id=\"tdCostPrice_{0}\" style=\"width:50px\" value=\"{1}\" />", str, (row["CostPrice"] != DBNull.Value) ? decimal.Parse(row["CostPrice"].ToString()).ToString("F2") : "").AppendLine();
            sb.AppendFormat("<td style=\"display:none\"><input type=\"text\" id=\"tdSalePrice_{0}\" style=\"width:50px\" value=\"{1}\" />", str, decimal.Parse(row["SalePrice"].ToString()).ToString("F2")).AppendLine();
            for (int i = 7; i < dtSkus.Columns.Count; i++)
            {
                string columnName = dtSkus.Columns[i].ColumnName;
                string[] strArray = row[columnName].ToString().Split(new char[] { '|' });
                string str3 = "";
                string str4 = "";
                if (strArray[0].ToString() != "")
                {
                    str3 = decimal.Parse(strArray[0].ToString()).ToString("F2");
                }
                str4 = strArray[1].ToString();
                sb.AppendFormat("<td><input type=\"text\" id=\"{0}_tdMemberPrice_{1}\" name=\"tdMemberPrice_{1}\" style=\"width:50px\" value=\"{2}\" />{3}</td>", new object[] { columnName.Substring(0, columnName.IndexOf("_")), str, str3, str4 }).AppendLine();
            }
            sb.AppendLine("</tr>");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string str = this.Page.Request.QueryString["productIds"];
            if (!string.IsNullOrEmpty(str))
            {
                DataTable skuMemberPrices = ProductHelper.GetSkuMemberPrices(str);
                if ((skuMemberPrices != null) && (skuMemberPrices.Rows.Count > 0))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<table cellspacing=\"0\" class=\"table table-bordered table-hover\" border=\"0\" style=\"width:100%;border-collapse:collapse;\">");
                    sb.AppendLine("<tr class=\"table_title\">");
                    sb.AppendLine("<th class=\"td_right td_left\" style=\"width:200px;\" scope=\"col\">商品编码</th>");
                    sb.AppendLine("<th class=\"td_right td_left\" style=\"width:400px;\" scope=\"col\">商品</th>");
                    sb.AppendLine("<th class=\"td_right td_left\" style=\"width:130px;\" scope=\"col\">原价</th>");
                    sb.AppendLine("<th class=\"td_right td_left\"  scope=\"col\" style=\"display:none\">成本价</th>");
                    sb.AppendLine("<th class=\"td_right td_left\" style=\"display:none\" scope=\"col\">一口价</th>");
                    for (int i = 7; i < skuMemberPrices.Columns.Count; i++)
                    {
                        string columnName = skuMemberPrices.Columns[i].ColumnName;
                        columnName = columnName.Substring(columnName.IndexOf("_") + 1) + "价";
                        sb.AppendFormat("<th class=\"td_right td_left\" scope=\"col\" style=\"width:200px;\">{0}</th>", columnName).AppendLine();
                    }
                    sb.AppendLine("</tr>");
                    foreach (DataRow row in skuMemberPrices.Rows)
                    {
                        this.CreateRow(row, skuMemberPrices, sb);
                    }
                    sb.AppendLine("</table>");
                    writer.Write(sb.ToString());
                }
            }
        }
    }
}

