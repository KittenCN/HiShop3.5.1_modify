namespace Hidistro.UI.Web.Admin.Goods
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    [PrivilegeCheck(Privilege.AddProducts)]
    public class SelectCategory : AdminPage
    {
        protected int categoryid;
        protected int productId;
        protected string reurl;

        protected SelectCategory() : base("m02", "spp01")
        {
            this.categoryid = Globals.RequestQueryNum("categoryId");
            this.productId = Globals.RequestQueryNum("productId");
            this.reurl = Globals.RequestQueryStr("reurl");
        }

        private void DoCallback()
        {
            string str = base.Request.QueryString["action"];
            base.Response.Clear();
            base.Response.ContentType = "application/json";
            if (str.Equals("getlist"))
            {
                int result = 0;
                int.TryParse(base.Request.QueryString["parentCategoryId"], out result);
                IList<CategoryInfo> categories = (result == 0) ? CatalogHelper.GetMainCategories() : CatalogHelper.GetSubCategories(result);
                if ((categories == null) || (categories.Count == 0))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                }
                else
                {
                    base.Response.Write(this.GenerateJson(categories));
                }
            }
            else if (str.Equals("getinfo"))
            {
                int num2 = 0;
                int.TryParse(base.Request.QueryString["categoryId"], out num2);
                if (num2 <= 0)
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                }
                else
                {
                    CategoryInfo category = CatalogHelper.GetCategory(num2);
                    if (category == null)
                    {
                        base.Response.Write("{\"Status\":\"0\"}");
                    }
                    else
                    {
                        base.Response.Write("{\"Status\":\"OK\", \"Name\":\"" + category.Name + "\", \"Path\":\"" + category.Path + "\"}");
                    }
                }
            }
            base.Response.End();
        }

        private string GenerateJson(IList<CategoryInfo> categories)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append("\"Status\":\"OK\",");
            builder.Append("\"Categories\":[");
            foreach (CategoryInfo info in categories)
            {
                builder.Append("{");
                builder.AppendFormat("\"CategoryId\":\"{0}\",", info.CategoryId.ToString(CultureInfo.InvariantCulture));
                builder.AppendFormat("\"HasChildren\":\"{0}\",", info.HasChildren ? "true" : "false");
                builder.AppendFormat("\"CategoryName\":\"{0}\"", info.Name);
                builder.Append("},");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("]}");
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && (base.Request.QueryString["isCallback"] == "true"))
            {
                this.DoCallback();
            }
        }
    }
}

