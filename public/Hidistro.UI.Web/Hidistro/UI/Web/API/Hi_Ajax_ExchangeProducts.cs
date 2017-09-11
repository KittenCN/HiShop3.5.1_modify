namespace Hidistro.UI.Web.API
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Newtonsoft.Json;
    using System;
    using System.Data;
    using System.Web;

    public class Hi_Ajax_ExchangeProducts : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int result = 0;
            int.TryParse(context.Request.Params["id"], out result);
            if (result > 0)
            {
                int num2;
                int num3;
                PointExChangeInfo info = PointExChangeHelper.Get(result);
                string str = context.Request.Params["sort"];
                if (string.IsNullOrWhiteSpace(str))
                {
                    str = "ProductId";
                }
                string str2 = context.Request.Params["order"];
                if (string.IsNullOrWhiteSpace(str2))
                {
                    str2 = "asc";
                }
                if (!int.TryParse(context.Request.Params["page"], out num2))
                {
                    num2 = 1;
                }
                if (!int.TryParse(context.Request.Params["size"], out num3))
                {
                    num3 = 10;
                }
                if ((info.BeginDate <= DateTime.Now) && (info.EndDate >= DateTime.Now))
                {
                    int num4;
                    DataTable table = PointExChangeHelper.GetProducts(result, num2, num3, out num4, str, str2);
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["ProductNumber"].ToString() == "0")
                        {
                            int num5 = 0;
                            int.TryParse(row["ProductId"].ToString(), out num5);
                            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), num5);
                            if ((product != null) && (product.SaleStatus == ProductSaleStatus.OnSale))
                            {
                                row["ProductNumber"] = product.Stock.ToString();
                            }
                        }
                        else
                        {
                            int num6 = 0;
                            int.TryParse(row["ProductId"].ToString(), out num6);
                            int num7 = 0;
                            int.TryParse(row["ProductNumber"].ToString(), out num7);
                            int productExchangedCount = PointExChangeHelper.GetProductExchangedCount(result, num6);
                            int num9 = ((num7 - productExchangedCount) >= 0) ? (num7 - productExchangedCount) : 0;
                            row["ProductNumber"] = num9;
                        }
                    }
                    string s = JsonConvert.SerializeObject(table, Formatting.Indented);
                    context.Response.Write(s);
                }
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

