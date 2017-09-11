namespace Hidistro.UI.Web.Admin.Shop
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_GetGames : IHttpHandler
    {
        public GameSearch GetGameSearch(HttpContext context)
        {
            return new GameSearch { Status = "0", GameType = new int?((context.Request.Form["type"] == null) ? 1 : Convert.ToInt32(context.Request.Form["type"])), PageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]), SortOrder = SortAction.Desc, EndTime = new DateTime?(DateTime.Now), SortBy = "GameId" };
        }

        public string GetGamesListJson(DbQueryResult CouponsTable, HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\"list\":[");
            DataTable data = (DataTable) CouponsTable.Data;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"game_id\":\"" + data.Rows[i]["GameId"].ToString() + "\",");
                builder.Append("\"title\":\"" + data.Rows[i]["GameTitle"].ToString() + "\",");
                builder.Append("\"create_time\":\"" + DateTime.Now + "\",");
                builder.Append("\"type\":\"" + data.Rows[i]["GameType"].ToString() + "\",");
                builder.Append("\"link\":\"" + data.Rows[i]["GameUrl"].ToString() + "\"");
                builder.Append("},");
            }
            return (builder.ToString().TrimEnd(new char[] { ',' }) + "]");
        }

        public DbQueryResult GetGamesTable(HttpContext context)
        {
            return GameHelper.GetGameList(this.GetGameSearch(context));
        }

        public string GetModelJson(HttpContext context)
        {
            DbQueryResult gamesTable = this.GetGamesTable(context);
            int pageCount = TemplatePageControl.GetPageCount(gamesTable.TotalRecords, 10);
            if (gamesTable != null)
            {
                string str = "{\"status\":1,";
                return (((str + this.GetGamesListJson(gamesTable, context) + ",") + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"") + "}");
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
            string text1 = context.Request.Form["id"];
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

