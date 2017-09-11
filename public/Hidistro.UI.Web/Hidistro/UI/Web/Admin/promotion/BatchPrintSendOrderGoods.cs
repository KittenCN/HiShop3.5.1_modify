namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI.HtmlControls;

    public class BatchPrintSendOrderGoods : AdminPage
    {
        protected HtmlGenericControl divContent;

        protected BatchPrintSendOrderGoods() : base("m03", "00000")
        {
        }

        private DataSet GetPrintData(string orderIds, string pIds)
        {
            orderIds = "'" + orderIds.Replace(",", "','") + "'";
            pIds = "'" + pIds.Replace(",", "','") + "'";
            return GameHelper.GetOrdersAndLines(orderIds, pIds);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request["OrderIds"].Trim(new char[] { ',' });
            string str2 = base.Request["PIds"].Trim(new char[] { ',' });
            if (!string.IsNullOrEmpty(str) || !string.IsNullOrEmpty(str2))
            {
                DataTable table = this.GetPrintData(str, str2).Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = table.Rows[i];
                    HtmlGenericControl child = new HtmlGenericControl("div");
                    child.Attributes["class"] = "order print";
                    child.Attributes["style"] = "padding-bottom:60px;padding-top:40px;";
                    StringBuilder builder = new StringBuilder("");
                    builder.AppendFormat("<div class=\"info clear\"><ul class=\"sub-info\"><li><span>中奖编号： </span>{0}</li><li><span>开奖时间： </span>{1}</li></ul></div>", (int.Parse(row["Ptype"].ToString()) == 1) ? row["PrizeNums"].ToString().Remove(row["PrizeNums"].ToString().Length - 1) : (Convert.ToDateTime(row["WinTime"]).ToString("yyyy-MM-dd-") + row["LogId"].ToString()), Convert.ToDateTime(row["WinTime"]).ToString("yyyy-MM-dd HH:mm:ss"));
                    builder.Append("<table><col class=\"col-1\" /><col class=\"col-3\" /><col class=\"col-3\" /><col class=\"col-3\" /><thead><tr><th>奖品信息</th><th>活动名称</th><th>收货人</th><th>奖品等级</th></tr></thead><tbody>");
                    builder.AppendFormat("<tr><td>{0}<br><span style=\"color: #888\">{1}</span></td>", row["ProductName"].ToString(), row["SkuIdStr"].ToString());
                    builder.AppendFormat("<td>{0}<br /><span class=\"jpname\">[{1}]</span></td>", row["Title"].ToString(), GameHelper.GetGameTypeName(row["GameType"].ToString()));
                    builder.AppendFormat("<td>{0}<br />{1}</td>", row["Receiver"].ToString(), row["Tel"].ToString());
                    builder.AppendFormat("<td style='padding-left:15px;'>{0}</td>", GameHelper.GetPrizeGradeName(row["PrizeGrade"].ToString()));
                    builder.Append("</tr>");
                    builder.Append("</tbody></table>");
                    child.InnerHtml = builder.ToString();
                    this.divContent.Controls.AddAt(0, child);
                }
            }
        }
    }
}

