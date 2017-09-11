namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Web;

    public class LimitedTimeDiscountHandler : IHttpHandler
    {
        private void IsDiscountProduct(HttpContext context)
        {
            int productId = Globals.RequestFormNum("productId");
            if (productId > 0)
            {
                LimitedTimeDiscountProductInfo discountProductInfoByProductId = LimitedTimeDiscountHelper.GetDiscountProductInfoByProductId(productId);
                if (discountProductInfoByProductId != null)
                {
                    LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(discountProductInfoByProductId.LimitedTimeDiscountId);
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    int limitedTimeDiscountId = 0;
                    int limitNumber = discountInfo.LimitNumber;
                    if (discountInfo != null)
                    {
                        if (currentMember != null)
                        {
                            if (MemberHelper.CheckCurrentMemberIsInRange(discountInfo.ApplyMembers, discountInfo.DefualtGroup, discountInfo.CustomGroup, currentMember.UserId))
                            {
                                int num4 = ShoppingCartProcessor.GetLimitedTimeDiscountUsedNum(discountProductInfoByProductId.LimitedTimeDiscountId, null, productId, currentMember.UserId, false);
                                if (discountInfo.LimitNumber == 0)
                                {
                                    limitedTimeDiscountId = discountInfo.LimitedTimeDiscountId;
                                }
                                else if ((discountInfo.LimitNumber - num4) > 0)
                                {
                                    limitedTimeDiscountId = discountInfo.LimitedTimeDiscountId;
                                    limitNumber = discountInfo.LimitNumber - num4;
                                }
                                else
                                {
                                    limitNumber = 0;
                                }
                            }
                        }
                        else
                        {
                            limitedTimeDiscountId = discountInfo.LimitedTimeDiscountId;
                        }
                    }
                    if (discountInfo != null)
                    {
                        context.Response.Write(string.Concat(new object[] { "{\"msg\":\"success\",\"ActivityName\":\"", Globals.String2Json(discountInfo.ActivityName), "\",\"FinalPrice\":\"", discountProductInfoByProductId.FinalPrice.ToString("f2"), "\",\"LimitedTimeDiscountId\":\"", limitedTimeDiscountId, "\",\"LimitNumber\":\"", discountInfo.LimitNumber, "\",\"RemainNumber\":\"", limitNumber, "\"}" }));
                    }
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string str2;
            if (((str2 = Globals.RequestFormStr("action").ToLower()) != null) && (str2 == "isdiscountproduct"))
            {
                this.IsDiscountProduct(context);
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

