namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Web;

    public class LimitedTimeDiscountHandler : IHttpHandler
    {
        private void ChangeDiscountProductStatus(HttpContext context)
        {
            string ids = Globals.RequestFormStr("id");
            int status = Globals.RequestFormNum("status");
            if (LimitedTimeDiscountHelper.ChangeDiscountProductStatus(ids, status))
            {
                context.Response.Write("{\"msg\":\"success\"}");
            }
            else
            {
                context.Response.Write("{\"msg\":\"fial\"}");
            }
        }

        private void ChangeStatus(HttpContext context)
        {
            int id = Globals.RequestFormNum("id");
            int num2 = Globals.RequestFormNum("status");
            if (LimitedTimeDiscountHelper.UpdateDiscountStatus(id, (num2 == 3) ? DiscountStatus.Normal : DiscountStatus.Stop))
            {
                context.Response.Write("{\"msg\":\"success\"}");
            }
            else
            {
                context.Response.Write("{\"msg\":\"fial\"}");
            }
        }

        private void DeleteDiscountProduct(HttpContext context)
        {
            string str = Globals.RequestFormStr("limitedTimeDiscountProductIds");
            if (!string.IsNullOrEmpty(str))
            {
                if (LimitedTimeDiscountHelper.DeleteDiscountProduct(str))
                {
                    context.Response.Write("{\"msg\":\"success\"}");
                }
                else
                {
                    context.Response.Write("{\"msg\":\"fail\"}");
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string str2 = Globals.RequestFormStr("action").ToLower();
            if (str2 != null)
            {
                if (!(str2 == "changestatus"))
                {
                    if (!(str2 == "changediscountproductstatus"))
                    {
                        if (!(str2 == "savediscountproduct"))
                        {
                            if (!(str2 == "savediscountproductinfo"))
                            {
                                if (!(str2 == "updatediscountproductlist"))
                                {
                                    if (str2 == "deletediscountproduct")
                                    {
                                        this.DeleteDiscountProduct(context);
                                    }
                                    return;
                                }
                                this.UpdateDiscountProductList(context);
                                return;
                            }
                            this.SaveDiscountProductInfo(context);
                            return;
                        }
                        this.SaveDiscountProduct(context);
                        return;
                    }
                }
                else
                {
                    this.ChangeStatus(context);
                    return;
                }
                this.ChangeDiscountProductStatus(context);
            }
        }

        private void SaveDiscountProduct(HttpContext context)
        {
            Globals.RequestFormNum("id");
            foreach (string str2 in Globals.RequestFormStr("discountProductList").Trim(new char[] { ',' }).Split(new char[] { ',' }))
            {
                string[] strArray2 = str2.Split(new char[] { '^' });
                LimitedTimeDiscountProductInfo info = new LimitedTimeDiscountProductInfo();
                if (((!string.IsNullOrEmpty(strArray2[0]) && !string.IsNullOrEmpty(strArray2[1])) && !string.IsNullOrEmpty(strArray2[4])) && (!string.IsNullOrEmpty(strArray2[2]) || !string.IsNullOrEmpty(strArray2[3])))
                {
                    int id = Globals.ToNum(strArray2[0]);
                    LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(id);
                    info.LimitedTimeDiscountId = id;
                    info.ProductId = Globals.ToNum(strArray2[1]);
                    info.Discount = (string.IsNullOrEmpty(strArray2[2]) || (strArray2[2] == "undefined")) ? 0M : decimal.Parse(strArray2[2]);
                    info.Minus = (string.IsNullOrEmpty(strArray2[3]) || (strArray2[2] == "undefined")) ? 0M : decimal.Parse(strArray2[3]);
                    info.FinalPrice = decimal.Parse(strArray2[4]);
                    if (discountInfo != null)
                    {
                        info.BeginTime = discountInfo.BeginTime;
                        info.EndTime = discountInfo.EndTime;
                    }
                    info.CreateTime = DateTime.Now;
                    info.Status = 1;
                    LimitedTimeDiscountHelper.AddLimitedTimeDiscountProduct(info);
                }
            }
            context.Response.Write("{\"msg\":\"success\"}");
        }

        private void SaveDiscountProductInfo(HttpContext context)
        {
            decimal num3;
            decimal num4;
            decimal num5;
            int num = Globals.RequestFormNum("ProductId");
            int num2 = Globals.RequestFormNum("LimitedTimeDiscountId");
            decimal.TryParse(Globals.RequestFormStr("Discount"), out num3);
            decimal.TryParse(Globals.RequestFormStr("Minus"), out num4);
            decimal.TryParse(Globals.RequestFormStr("FinalPrice"), out num5);
            LimitedTimeDiscountProductInfo info = new LimitedTimeDiscountProductInfo {
                ProductId = num,
                LimitedTimeDiscountId = num2,
                Discount = num3,
                Minus = num4,
                FinalPrice = num5
            };
            if (LimitedTimeDiscountHelper.UpdateLimitedTimeDiscountProduct(info))
            {
                context.Response.Write("{\"msg\":\"success\"}");
            }
            else
            {
                context.Response.Write("{\"msg\":\"fial\"}");
            }
        }

        private void UpdateDiscountProductList(HttpContext context)
        {
            int id = Globals.RequestFormNum("LimitedTimeDiscountId");
            foreach (string str2 in Globals.RequestFormStr("discountProductList").Trim(new char[] { ',' }).Split(new char[] { ',' }))
            {
                string[] strArray2 = str2.Split(new char[] { '^' });
                for (int i = 0; i < strArray2.Length; i++)
                {
                    if (((!string.IsNullOrEmpty(strArray2[0]) && !string.IsNullOrEmpty(strArray2[1])) && !string.IsNullOrEmpty(strArray2[4])) && (!string.IsNullOrEmpty(strArray2[2]) || !string.IsNullOrEmpty(strArray2[3])))
                    {
                        LimitedTimeDiscountProductInfo info = new LimitedTimeDiscountProductInfo();
                        int num3 = Globals.ToNum(strArray2[0]);
                        LimitedTimeDiscountHelper.GetDiscountInfo(id);
                        info.LimitedTimeDiscountProductId = num3;
                        info.Discount = (string.IsNullOrEmpty(strArray2[2]) || (strArray2[2] == "undefined")) ? 0M : decimal.Parse(strArray2[2]);
                        info.Minus = (string.IsNullOrEmpty(strArray2[3]) || (strArray2[2] == "undefined")) ? 0M : decimal.Parse(strArray2[3]);
                        info.FinalPrice = decimal.Parse(strArray2[4]);
                        LimitedTimeDiscountHelper.UpdateLimitedTimeDiscountProductById(info);
                    }
                }
            }
            context.Response.Write("{\"msg\":\"success\"}");
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

