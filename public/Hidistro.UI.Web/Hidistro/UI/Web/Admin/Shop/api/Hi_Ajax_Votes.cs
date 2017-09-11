namespace Hidistro.UI.Web.Admin.Shop.api
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

    public class Hi_Ajax_Votes : IHttpHandler
    {
        public string GetGamesListJson(DbQueryResult votesTable, HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\"list\":[");
            DataTable data = (DataTable) votesTable.Data;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"game_id\":\"" + data.Rows[i]["VoteId"].ToString() + "\",");
                builder.Append("\"title\":\"" + data.Rows[i]["VoteName"].ToString() + "\",");
                builder.Append("\"create_time\":\"" + DateTime.Now + "\",");
                builder.Append("\"type\":\"0\",");
                builder.Append("\"link\":\"" + context.Server.UrlPathEncode(this.GetUrl(data.Rows[i]["VoteId"])) + "\"");
                builder.Append("},");
            }
            return (builder.ToString().TrimEnd(new char[] { ',' }) + "]");
        }

        public string GetModelJson(HttpContext context)
        {
            DbQueryResult votesTable = this.GetVotesTable(context);
            int pageCount = TemplatePageControl.GetPageCount(votesTable.TotalRecords, 10);
            if (votesTable != null)
            {
                string str = "{\"status\":1,";
                return (((str + this.GetGamesListJson(votesTable, context) + ",") + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"") + "}");
            }
            return "{\"status\":1,\"list\":[],\"page\":\"\"}";
        }

        public string GetPageHtml(int pageCount, HttpContext context)
        {
            int pageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]);
            return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
        }

        public string GetUrl(object voteId)
        {
            return string.Concat(new object[] { "http://", Globals.DomainName, Globals.ApplicationPath, "/BeginVote.aspx?voteId=", voteId });
        }

        public VoteSearch GetVoteSearch(HttpContext context)
        {
            return new VoteSearch { status = VoteStatus.In, PageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]), SortOrder = SortAction.Desc, SortBy = "VoteId" };
        }

        public DbQueryResult GetVotesTable(HttpContext context)
        {
            return VoteHelper.Query(this.GetVoteSearch(context));
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

