namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using HiTemplate.Model;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class Hi_Ajax_GoodsList : IHttpHandler
    {
        public IList<ProductInfo> GetGoods(HttpContext context, string ids)
        {
            return ProductHelper.GetProducts((from s in ids.Split(new char[] { ',' }) select int.Parse(s)).ToList<int>(), true);
        }

        public string GoodGroupJson(HttpContext context)
        {
            Hi_Json_GoodGourpContent content = new Hi_Json_GoodGourpContent {
                showPrice = (context.Request.Form["ShowPrice"] != null) ? Convert.ToBoolean(context.Request.Form["ShowPrice"]) : true,
                layout = (context.Request.Form["Layout"] != null) ? Convert.ToInt32(context.Request.Form["Layout"]) : 1,
                showName = (context.Request.Form["showName"] != null) ? Convert.ToBoolean(Convert.ToInt32(context.Request.Form["showName"])) : true,
                showIco = (context.Request.Form["ShowIco"] != null) ? Convert.ToBoolean(context.Request.Form["ShowIco"]) : true,
                showMaketPrice = true
            };
            string str = (context.Request.Form["IDs"] != null) ? context.Request.Form["IDs"] : "";
            List<HiShop_Model_Good> list = new List<HiShop_Model_Good>();
            if (!string.IsNullOrEmpty(str))
            {
                foreach (ProductInfo info in this.GetGoods(context, str))
                {
                    HiShop_Model_Good item = new HiShop_Model_Good {
                        item_id = info.ProductId.ToString(),
                        title = info.ProductName.ToString(),
                        price = Convert.ToDouble(info.MinShowPrice).ToString("f2"),
                        original_price = Convert.ToDouble(info.MarketPrice).ToString("f2"),
                        link = Globals.GetWebUrlStart() + "/ProductDetails.aspx?productId=" + info.ProductId.ToString(),
                        pic = info.ThumbnailUrl310.ToString()
                    };
                    list.Add(item);
                }
            }
            content.goodslist = list;
            return JsonConvert.SerializeObject(content);
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.GoodGroupJson(context));
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

