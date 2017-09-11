namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_GetItems : IHttpHandler
    {
        public string GetGoodsListJson(DbQueryResult GoodsTable)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\"list\":[");
            DataTable data = (DataTable) GoodsTable.Data;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"item_id\":\"" + data.Rows[i]["ProductId"] + "\",");
                builder.Append("\"title\":\"" + data.Rows[i]["ProductName"].ToString().Replace(@"\", "") + "\",");
                builder.Append("\"price\":\"" + Convert.ToDecimal(data.Rows[i]["SalePrice"]).ToString("f2") + "\",");
                builder.Append("\"original_price\":\"" + Convert.ToDecimal(data.Rows[i]["MarketPrice"]).ToString("f2") + "\",");
                builder.Append("\"create_time\":\"" + Convert.ToDateTime(data.Rows[i]["AddedDate"]).ToString("yyyy-MM-dd HH:mm:ss") + "\",");
                builder.Append("\"link\":\"/ProductDetails.aspx?productId=" + data.Rows[i]["ProductId"] + "\",");
                builder.Append("\"pic\":\"" + data.Rows[i]["ThumbnailUrl310"] + "\",");
                builder.Append("\"is_compress\":0");
                builder.Append("},");
            }
            return (builder.ToString().TrimEnd(new char[] { ',' }) + "]");
        }

        public DbQueryResult GetGoodsTable(HttpContext context)
        {
            return ProductHelper.GetProducts(this.GetProductQuery(context));
        }

        public string GetModelJson(HttpContext context)
        {
            DbQueryResult goodsTable = this.GetGoodsTable(context);
            int pageCount = TemplatePageControl.GetPageCount(goodsTable.TotalRecords, 10);
            if (goodsTable != null)
            {
                string str = "{\"status\":1,";
                return (((str + this.GetGoodsListJson(goodsTable) + ",") + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"") + "}");
            }
            return "{\"status\":1,\"list\":[],\"page\":\"\"}";
        }

        public string GetPageHtml(int pageCount, HttpContext context)
        {
            int pageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]);
            return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
        }

        public ProductQuery GetProductQuery(HttpContext context)
        {
            return new ProductQuery { Keywords = context.Request.Form["title"], PageSize = 10, PageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]), SortOrder = SortAction.Desc, SortBy = "DisplaySequence", SaleStatus = (context.Request.Form["status"] == null) ? ProductSaleStatus.OnSale : ((ProductSaleStatus) Convert.ToInt32(context.Request.Form["status"])) };
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string text1 = context.Request.Form["p"];
            context.Response.Write(this.GetModelJson(context));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

