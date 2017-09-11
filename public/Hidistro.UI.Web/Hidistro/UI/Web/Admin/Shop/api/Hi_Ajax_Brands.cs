namespace Hidistro.UI.Web.Admin.Shop.api
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_Brands : IHttpHandler
    {
        public BrandQuery GetBrandSearch(HttpContext context)
        {
            return new BrandQuery { Name = (context.Request.Form["title"] == null) ? "" : context.Request.Form["title"], PageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]), SortOrder = SortAction.Desc, SortBy = "BrandId" };
        }

        public DbQueryResult GetBrandsTable(HttpContext context)
        {
            return CatalogHelper.GetBrandQuery(this.GetBrandSearch(context));
        }

        public string GetGraphicesListJson(DbQueryResult GraphicesTable, HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\"list\":[");
            DataTable data = (DataTable) GraphicesTable.Data;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"item_id\":\"" + data.Rows[i]["BrandId"] + "\",");
                builder.Append("\"title\":\"" + data.Rows[i]["BrandName"] + "\",");
                builder.Append("\"create_time\":\"" + DateTime.Now + "\",");
                builder.Append("\"link\":\"/BrandDetail.aspx?BrandId=" + data.Rows[i]["BrandId"] + "\",");
                builder.Append("\"pic\":\"" + data.Rows[i]["Logo"] + "\"");
                builder.Append("},");
            }
            return (builder.ToString().TrimEnd(new char[] { ',' }) + "]");
        }

        public string GetModelJson(HttpContext context)
        {
            DbQueryResult brandsTable = this.GetBrandsTable(context);
            int pageCount = TemplatePageControl.GetPageCount(brandsTable.TotalRecords, 10);
            if (brandsTable != null)
            {
                string str = "{\"status\":1,";
                return (((str + this.GetGraphicesListJson(brandsTable, context) + ",") + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"") + "}");
            }
            return "{\"status\":1,\"list\":[],\"page\":\"\"}";
        }

        public string GetPageHtml(int pageCount, HttpContext context)
        {
            int pageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]);
            return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
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

