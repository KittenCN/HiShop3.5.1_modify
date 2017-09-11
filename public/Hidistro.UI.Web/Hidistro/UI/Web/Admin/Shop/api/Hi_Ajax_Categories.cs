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

    public class Hi_Ajax_Categories : IHttpHandler
    {
        public CategoriesQuery GetCategoriesSearch(HttpContext context)
        {
            return new CategoriesQuery { Name = (context.Request.Form["title"] == null) ? "" : context.Request.Form["title"], PageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]), SortOrder = SortAction.Desc, SortBy = "DisplaySequence" };
        }

        public DbQueryResult GetCategoriesTable(HttpContext context)
        {
            return CatalogHelper.Query(this.GetCategoriesSearch(context));
        }

        private string GetChildGraphicesListJson(DataTable dt, string categoryId)
        {
            StringBuilder builder = new StringBuilder();
            string str = "";
            dt.DefaultView.RowFilter = "ParentCategoryId=" + categoryId;
            DataTable table = dt.DefaultView.ToTable();
            if (table.Rows.Count <= 0)
            {
                return str;
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"item_id\":\"" + table.Rows[i]["CategoryId"] + "\",");
                builder.Append("\"title\":\"" + Globals.String2Json(table.Rows[i]["Name"].ToString()) + "\",");
                builder.Append("\"create_time\":\"" + DateTime.Now + "\",");
                builder.Append("\"link\":\"/ProductList.aspx?categoryId=" + table.Rows[i]["CategoryId"] + "\",");
                builder.Append("\"pic\":\"\"");
                builder.Append("},");
            }
            return builder.ToString().TrimEnd(new char[] { ',' });
        }

        public string GetGraphicesListJson(DbQueryResult GraphicesTable, HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\"list\":[");
            DataTable data = (DataTable) GraphicesTable.Data;
            data.DefaultView.RowFilter = "ParentCategoryId=0";
            DataTable table2 = data.DefaultView.ToTable();
            for (int i = 0; i < table2.Rows.Count; i++)
            {
                string s = table2.Rows[i]["CategoryId"].ToString();
                builder.Append("{");
                builder.Append("\"item_id\":\"" + Globals.String2Json(s) + "\",");
                builder.Append("\"title\":\"" + Globals.String2Json(table2.Rows[i]["Name"].ToString()) + "\",");
                builder.Append("\"create_time\":\"" + DateTime.Now + "\",");
                builder.Append("\"link\":\"/ProductList.aspx?categoryId=" + table2.Rows[i]["CategoryId"] + "\",");
                builder.Append("\"pic\":\"\",");
                builder.Append("\"children\":[" + this.GetChildGraphicesListJson(data, s) + "]");
                builder.Append("},");
            }
            return (builder.ToString().TrimEnd(new char[] { ',' }) + "]");
        }

        public string GetModelJson(HttpContext context)
        {
            DbQueryResult categoriesTable = this.GetCategoriesTable(context);
            TemplatePageControl.GetPageCount(categoriesTable.TotalRecords, 0x2710);
            if (categoriesTable != null)
            {
                string str = "{\"status\":1,";
                return ((str + this.GetGraphicesListJson(categoriesTable, context) + ",") + "\"page\":\"\"" + "}");
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

