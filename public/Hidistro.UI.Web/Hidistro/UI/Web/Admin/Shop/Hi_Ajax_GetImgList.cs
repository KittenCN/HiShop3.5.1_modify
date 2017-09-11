namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_GetImgList : IHttpHandler
    {
        public string GetGoodsListJson(DbQueryResult GoodsTable)
        {
            StringBuilder builder = new StringBuilder();
            DataTable data = (DataTable) GoodsTable.Data;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"id\":\"" + data.Rows[i]["ProductId"] + "\",");
                builder.Append("\"file\":\"" + data.Rows[i]["img"] + "\",");
                builder.Append("\"name\":\"" + Globals.String2Json(data.Rows[i]["ProductName"].ToString()) + "\"");
                builder.Append("},");
            }
            return builder.ToString().TrimEnd(new char[] { ',' });
        }

        public DbQueryResult GetGoodsTable(HttpContext context, int pagesize, string keyword, int maincategoryid)
        {
            return ProductHelper.GetProductsImgList(this.GetProductQuery(context, pagesize, keyword, maincategoryid));
        }

        public string GetImgItemJson()
        {
            return "";
        }

        public string GetImgItemsJson(DbQueryResult mamagerRecordset, HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            DataTable data = (DataTable) mamagerRecordset.Data;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append("{");
                builder.Append("\"id\":\"" + data.Rows[i]["PhotoId"] + "\",");
                builder.Append(string.Concat(new object[] { "\"file\":\"http://", context.Request.Url.Authority, data.Rows[i]["PhotoPath"], "\"," }));
                builder.Append("\"name\":\"" + Globals.String2Json(data.Rows[i]["PhotoName"].ToString()) + "\"");
                builder.Append("},");
            }
            return builder.ToString().TrimEnd(new char[] { ',' });
        }

        public string GetListJson(HttpContext context)
        {
            int pagesize = 0x15;
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"status\":1,");
            builder.Append("\"data\":[");
            int type = Globals.RequestFormNum("type");
            int maincategoryid = Globals.RequestFormNum("id");
            string keyword = Globals.RequestFormStr("file_name");
            int pageCount = 0;
            if (type == 3)
            {
                DbQueryResult goodsTable = this.GetGoodsTable(context, pagesize, keyword, maincategoryid);
                pageCount = TemplatePageControl.GetPageCount(goodsTable.TotalRecords, pagesize);
                builder.Append(this.GetGoodsListJson(goodsTable));
            }
            else
            {
                DbQueryResult mamagerRecordset = GalleryHelper.GetPhotoList(keyword, new int?(maincategoryid), Convert.ToInt32(context.Request.Form["p"]), pagesize, PhotoListOrder.UploadTimeDesc, type);
                pageCount = TemplatePageControl.GetPageCount(mamagerRecordset.TotalRecords, pagesize);
                builder.Append(this.GetImgItemsJson(mamagerRecordset, context));
            }
            return (((builder.ToString().TrimEnd(new char[] { ',' }) + "],") + "\"page\": \"" + this.GetPageHtml(pageCount, context) + "\",") + "\"msg\": \"\"" + "}");
        }

        public string GetPageHtml(int pageCount, HttpContext context)
        {
            int pageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]);
            return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
        }

        public ProductQuery GetProductQuery(HttpContext context, int pagesize, string keyword, int maincategoryid)
        {
            return new ProductQuery { Keywords = keyword, PageSize = pagesize, PageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]), SortOrder = SortAction.Desc, SortBy = "ProductName", MaiCategoryPath = maincategoryid.ToString(), SaleStatus = (context.Request.Form["status"] == null) ? ProductSaleStatus.OnSale : ((ProductSaleStatus) Convert.ToInt32(context.Request.Form["status"])) };
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string listJson = this.GetListJson(context);
            context.Response.Write(listJson);
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

