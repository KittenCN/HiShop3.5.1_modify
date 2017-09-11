namespace Hidistro.UI.Web.Admin.Shop.api
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_Coupons : IHttpHandler
    {
        public string GetCouponsListJson(DbQueryResult CouponsTable, HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\"list\":[");
            DataTable data = (DataTable) CouponsTable.Data;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"game_id\":\"" + data.Rows[i]["CouponId"].ToString() + "\",");
                builder.Append("\"title\":\"" + data.Rows[i]["CouponName"].ToString() + "\",");
                builder.Append("\"create_time\":\"" + DateTime.Now + "\",");
                builder.Append("\"type\":\"1\",");
                builder.Append("\"link\":\"/VShop/CouponDetails.aspx?CouponId=" + data.Rows[i]["CouponId"].ToString() + "\"");
                builder.Append("},");
            }
            return (builder.ToString().TrimEnd(new char[] { ',' }) + "]");
        }

        public CouponsSearch GetCouponsSearch(HttpContext context)
        {
            return new CouponsSearch { beginDate = new DateTime?(DateTime.Now), endDate = new DateTime?(DateTime.Now), PageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]), Finished = false, SortOrder = SortAction.Desc, SortBy = "CouponId", SearchType = 2 };
        }

        public DbQueryResult GetCouponsTable(HttpContext context)
        {
            return CouponHelper.GetCouponInfos(this.GetCouponsSearch(context));
        }

        public string GetModelJson(HttpContext context)
        {
            DbQueryResult couponsTable = this.GetCouponsTable(context);
            int pageCount = TemplatePageControl.GetPageCount(couponsTable.TotalRecords, 10);
            if (couponsTable != null)
            {
                string str = "{\"status\":1,";
                return (((str + this.GetCouponsListJson(couponsTable, context) + ",") + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"") + "}");
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

