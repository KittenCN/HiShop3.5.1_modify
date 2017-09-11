namespace Hidistro.UI.Web.Admin.Shop.api
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Store;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_GetCustomPages : IHttpHandler
    {
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
                builder.Append("\"item_id\":\"" + table.Rows[i]["Id"] + "\",");
                builder.Append("\"title\":\"" + Globals.String2Json(table.Rows[i]["Name"].ToString()) + "\",");
                builder.Append("\"create_time\":\"" + DateTime.Now + "\",");
                builder.Append("\"link\":\"\",");
                builder.Append("\"pic\":\"\"");
                builder.Append("},");
            }
            return builder.ToString().TrimEnd(new char[] { ',' });
        }

        public CustomPageQuery GetCustomPageSearch(HttpContext context)
        {
            return new CustomPageQuery { Name = (context.Request.Form["title"] == null) ? "" : context.Request.Form["title"], PageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]), SortOrder = SortAction.Desc, SortBy = "Id", Status = 0 };
        }

        public DbQueryResult GetCustomPageTable(HttpContext context)
        {
            return CustomPageHelp.GetPages(this.GetCustomPageSearch(context));
        }

        public string GetGraphicesListJson(DbQueryResult GraphicesTable, HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\"list\":[");
            DataTable data = (DataTable) GraphicesTable.Data;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"item_id\":\"" + Globals.String2Json(data.Rows[i]["Id"].ToString()) + "\",");
                builder.Append("\"title\":\"" + Globals.String2Json(data.Rows[i]["Name"].ToString()) + "\",");
                builder.Append("\"create_time\":\"" + DateTime.Now + "\",");
                builder.Append("\"link\":\"/custom/" + data.Rows[i]["PageUrl"].ToString() + "\",");
                builder.Append("\"pic\":\"\"");
                builder.Append("},");
            }
            return (builder.ToString().TrimEnd(new char[] { ',' }) + "]");
        }

        public string GetModelJson(HttpContext context)
        {
            DbQueryResult customPageTable = this.GetCustomPageTable(context);
            int pageCount = TemplatePageControl.GetPageCount(customPageTable.TotalRecords, 10);
            if (customPageTable != null)
            {
                string str = "{\"status\":1,";
                return (((str + this.GetGraphicesListJson(customPageTable, context) + ",") + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"") + "}");
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

