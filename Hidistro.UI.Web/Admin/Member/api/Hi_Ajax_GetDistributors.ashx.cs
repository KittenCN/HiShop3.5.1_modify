using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.Member.api
{
    /// <summary>
    /// Hi_Ajax_GetDistributors 的摘要说明
    /// </summary>
    public class Hi_Ajax_GetDistributors : IHttpHandler
    {

        public string GetJson(HttpContext context)
        {
            DistributorsQuery query = new DistributorsQuery
            {
                StoreName = (context.Request.QueryString["userName"] != null) ? context.Request.QueryString["userName"] : ""
            };
            DataTable table = DistributorsBrower.SelectDistributors(query);
            string str = "[";
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    object obj2 = str + "{";
                    object obj3 = string.Concat(new object[] { obj2, "\"StoreName\":\"", table.Rows[i]["StoreName"], "\"," });
                    str = string.Concat(new object[] { obj3, "\"UserId\":\"", table.Rows[i]["UserId"], "\"" }) + "},";
                }
                str = str.TrimEnd(new char[] { ',' });
            }
            return (str + "]");
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.GetJson(context));
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