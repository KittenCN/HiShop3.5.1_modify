namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_GetFolderTree : IHttpHandler
    {
        private int GetImgCount(int type)
        {
            if (type == 3)
            {
                return ProductHelper.GetProductsImgList(this.GetProductQuery(0)).TotalRecords;
            }
            return GalleryHelper.GetPhotoList("", 0, 10, PhotoListOrder.UploadTimeDesc, type, 20).TotalRecords;
        }

        public string GetImgTypeJson(int type)
        {
            StringBuilder builder = new StringBuilder();
            DataTable photoCategories = null;
            if (type == 3)
            {
                IList<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
                for (int i = 0; i < mainCategories.Count; i++)
                {
                    builder.Append("{");
                    builder.Append("\"name\":\"" + mainCategories[i].Name + "\",");
                    builder.Append("\"parent_id\":" + mainCategories[i].ParentCategoryId + ",");
                    builder.Append("\"id\":" + mainCategories[i].CategoryId + ",");
                    builder.Append("\"picNum\":" + ProductHelper.GetProductsImgList(this.GetProductQuery(mainCategories[i].CategoryId)).TotalRecords);
                    builder.Append("},");
                }
            }
            else
            {
                photoCategories = GalleryHelper.GetPhotoCategories(type);
                for (int j = 0; j < photoCategories.Rows.Count; j++)
                {
                    builder.Append("{");
                    builder.Append("\"name\":\"" + photoCategories.Rows[j]["CategoryName"] + "\",");
                    builder.Append("\"parent_id\":0,");
                    builder.Append("\"id\":" + photoCategories.Rows[j]["CategoryId"] + ",");
                    builder.Append("\"picNum\":" + GalleryHelper.GetPhotoList("", new int?(Convert.ToInt32(photoCategories.Rows[j]["CategoryId"])), 10, PhotoListOrder.UploadTimeDesc, type, 20).TotalRecords);
                    builder.Append("},");
                }
            }
            return builder.ToString().TrimEnd(new char[] { ',' });
        }

        public ProductQuery GetProductQuery(int categoryid)
        {
            if (categoryid > 0)
            {
                return new ProductQuery { PageSize = 1, PageIndex = 1, SortOrder = SortAction.Desc, SortBy = "ProductName", MaiCategoryPath = categoryid.ToString() };
            }
            return new ProductQuery { PageSize = 1, PageIndex = 1, SortOrder = SortAction.Desc, SortBy = "ProductName" };
        }

        public string GetTreeListJson(int type)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"status\":1,");
            builder.Append("\"data\":{");
            builder.Append("\"total\":" + this.GetImgCount(type) + ",");
            builder.Append("\"tree\":[");
            builder.Append(this.GetImgTypeJson(type));
            builder.Append("]");
            builder.Append("},");
            builder.Append("\"msg\":\"\"");
            builder.Append("}");
            return builder.ToString();
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int type = Globals.RequestFormNum("type");
            context.Response.Write(this.GetTreeListJson(type));
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

