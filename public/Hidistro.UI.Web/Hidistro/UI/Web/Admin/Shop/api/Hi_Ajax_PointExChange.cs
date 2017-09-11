namespace Hidistro.UI.Web.Admin.Shop.api
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_PointExChange : IHttpHandler
    {
        public ExChangeSearch GetExChangeSearch(HttpContext context)
        {
            return new ExChangeSearch { PageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]), SortOrder = SortAction.Desc, status = ExchangeStatus.In, SortBy = "Id" };
        }

        public DataTable GetExChangeTable(HttpContext context, ref int pageCount)
        {
            return PointExChangeHelper.Query(this.GetExChangeSearch(context), ref pageCount);
        }

        public string GetGamesListJson(DataTable dt, HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\"list\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"game_id\":\"" + dt.Rows[i]["Id"].ToString() + "\",");
                builder.Append("\"title\":\"" + dt.Rows[i]["Name"].ToString() + "\",");
                builder.Append("\"create_time\":\"" + DateTime.Now + "\",");
                builder.Append("\"type\":\"0\",");
                builder.Append("\"link\":\"/ExchangeList.aspx?id=" + dt.Rows[i]["Id"].ToString() + "\"");
                builder.Append("},");
            }
            return (builder.ToString().TrimEnd(new char[] { ',' }) + "]");
        }

        public string GetModelJson(HttpContext context)
        {
            int pageCount = 0;
            DataTable exChangeTable = this.GetExChangeTable(context, ref pageCount);
            int num2 = TemplatePageControl.GetPageCount(pageCount, 10);
            if (exChangeTable != null)
            {
                string str = "{\"status\":1,";
                return (((str + this.GetGamesListJson(exChangeTable, context) + ",") + "\"page\":\"" + this.GetPageHtml(num2, context) + "\"") + "}");
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

