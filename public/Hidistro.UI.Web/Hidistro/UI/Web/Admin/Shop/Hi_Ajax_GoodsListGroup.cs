namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using HiTemplate.Model;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;

    public class Hi_Ajax_GoodsListGroup : IHttpHandler
    {
        public DataTable GetGoods(HttpContext context)
        {
            int top = (context.Request.Form["GoodListSize"] != null) ? Convert.ToInt32(context.Request.Form["GoodListSize"]) : 6;
            ProductShowOrderPriority nULLORDER = ProductShowOrderPriority.NULLORDER;
            ProductShowOrderPriority show = !string.IsNullOrEmpty(context.Request.Form["SecondPriority"]) ? ((ProductShowOrderPriority) Convert.ToInt32(context.Request.Form["SecondPriority"])) : ProductShowOrderPriority.NULLORDER;
            string str = ProductTempSQLADD.ReturnShowOrder(nULLORDER);
            if (!string.IsNullOrEmpty(str))
            {
                str = str + ",";
            }
            if (!string.IsNullOrEmpty(ProductTempSQLADD.ReturnShowOrder(show)))
            {
                str = str + ProductTempSQLADD.ReturnShowOrder(show);
            }
            return ProductHelper.GetTopProductOrder(top, str);
        }

        public string GoodGroupJson(HttpContext context)
        {
            Hi_Json_GoodGourpContent content = new Hi_Json_GoodGourpContent {
                showPrice = (context.Request.Form["ShowPrice"] != null) ? Convert.ToBoolean(context.Request.Form["ShowPrice"]) : true,
                layout = (context.Request.Form["Layout"] != null) ? Convert.ToInt32(context.Request.Form["Layout"]) : 1,
                showName = (context.Request.Form["showName"] != null) ? Convert.ToBoolean(context.Request.Form["showName"]) : true,
                showIco = (context.Request.Form["ShowIco"] != null) ? Convert.ToBoolean(context.Request.Form["ShowIco"]) : true,
                goodsize = (context.Request.Form["GoodListSize"] != null) ? Convert.ToInt32(context.Request.Form["GoodListSize"]) : 6,
                showMaketPrice = (context.Request.Form["ShowMaketPrice"] != null) ? Convert.ToBoolean(context.Request.Form["ShowMaketPrice"]) : true
            };
            content.secondPriority=((Globals.RequestFormStr("SecondPriority").Trim() != "") ? Convert.ToInt32(context.Request.Form["SecondPriority"]) : 3);
            List<HiShop_Model_Good> list = new List<HiShop_Model_Good>();
            DataTable goods = this.GetGoods(context);
            for (int i = 0; i < goods.Rows.Count; i++)
            {
                HiShop_Model_Good item = new HiShop_Model_Good {
                    item_id = goods.Rows[i]["ProductId"].ToString(),
                    title = goods.Rows[i]["ProductName"].ToString(),
                    price = Convert.ToDouble(goods.Rows[i]["MinShowPrice"]).ToString("f2"),
                    original_price = Convert.ToDouble(goods.Rows[i]["MarketPrice"]).ToString("f2"),
                    link = Globals.GetWebUrlStart() + "/ProductDetails.aspx?productId=" + goods.Rows[i]["ProductId"].ToString(),
                    pic = goods.Rows[i]["ThumbnailUrl310"].ToString()
                };
                list.Add(item);
            }
            content.goodslist = list;
            return JsonConvert.SerializeObject(content);
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string text1 = context.Request.Form["id"];
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

