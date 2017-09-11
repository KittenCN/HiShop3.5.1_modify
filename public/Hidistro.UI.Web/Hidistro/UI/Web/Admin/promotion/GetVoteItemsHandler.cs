namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Newtonsoft.Json;
    using System;
    using System.Web;

    public class GetVoteItemsHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            long result = 0L;
            long.TryParse(context.Request.QueryString["id"], out result);
            try
            {
                if (Globals.GetCurrentManagerUserId() <= 0)
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请先登录\"}");
                    context.Response.End();
                }
                if (result > 0L)
                {
                    VoteInfo vote = VoteHelper.GetVote(result);
                    if (vote != null)
                    {
                        var type = new {
                            type = "success",
                            VoteName = vote.VoteName,
                            data = vote.VoteItems
                        };
                        string s = JsonConvert.SerializeObject(type);
                        context.Response.Write(s);
                    }
                    else
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"该投票调查不存在！\"}");
                    }
                }
                else
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"参数错误！\"}");
                }
            }
            catch (Exception exception)
            {
                context.Response.Write("{\"type\":\"error\",\"data\":\"" + Globals.String2Json(exception.ToString()) + "\"}");
            }
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

