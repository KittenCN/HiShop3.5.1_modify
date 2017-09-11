namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Entities.Bargain;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SqlDal.Bargain;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.Sales;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class ShoppingCartProcessor
    {
        public static void AddLineItem(string skuId, int quantity, int categoryid, int Templateid, int type = 0, int exchangeId = 0, int limitedTimeDiscountId = 0)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (quantity <= 0)
            {
                quantity = 1;
            }
            int num = 1;
            int num2 = 0;
            if (limitedTimeDiscountId == 0)
            {
                int productId = 0;
                SKUItem skuItem = new SkuDao().GetSkuItem(skuId);
                if (skuItem != null)
                {
                    productId = skuItem.ProductId;
                    int id = Globals.ToNum(new LimitedTimeDiscountDao().GetLimitedTimeDiscountIdByProductId(currentMember.UserId, skuId, productId));
                    if (id > 0)
                    {
                        LimitedTimeDiscountInfo discountInfo = new LimitedTimeDiscountDao().GetDiscountInfo(id);
                        if (new MemberDao().CheckCurrentMemberIsInRange(discountInfo.ApplyMembers, discountInfo.DefualtGroup, discountInfo.CustomGroup, currentMember.UserId))
                        {
                            int num5 = GetLimitedTimeDiscountUsedNum(id, skuId, productId, currentMember.UserId, true);
                            if (discountInfo.LimitNumber == 0)
                            {
                                limitedTimeDiscountId = discountInfo.LimitedTimeDiscountId;
                            }
                            else if ((discountInfo.LimitNumber - num5) >= quantity)
                            {
                                limitedTimeDiscountId = discountInfo.LimitedTimeDiscountId;
                            }
                            else if ((discountInfo.LimitNumber - num5) < quantity)
                            {
                                num = 2;
                                limitedTimeDiscountId = discountInfo.LimitedTimeDiscountId;
                                num2 = quantity - (discountInfo.LimitNumber - num5);
                                quantity = discountInfo.LimitNumber - num5;
                            }
                        }
                    }
                }
            }
            new ShoppingCartDao().AddLineItem(currentMember, skuId, quantity, categoryid, Templateid, type, exchangeId, limitedTimeDiscountId);
            if (num == 2)
            {
                new ShoppingCartDao().AddLineItem(currentMember, skuId, num2, categoryid, Templateid, type, exchangeId, 0);
            }
        }

        public static void ClearShoppingCart()
        {
            new ShoppingCartDao().ClearShoppingCart(Globals.GetCurrentMemberUserId(false));
        }

        public static int GetLimitedTimeDiscountUsedNum(int limitedTimeDiscountId, string skuId, int productId, int userid, bool isContainsShippingCart)
        {
            return new ShoppingCartDao().GetLimitedTimeDiscountUsedNum(limitedTimeDiscountId, skuId, productId, userid, isContainsShippingCart);
        }

        public static List<ShoppingCartInfo> GetListShoppingCart(string productSkuId, int buyAmount, int bargainDetialId = 0, int limitedTimeDiscountId = 0)
        {
            List<ShoppingCartInfo> list = new List<ShoppingCartInfo>();
            ShoppingCartInfo item = new ShoppingCartInfo();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ShoppingCartItemInfo info3 = null;
            if (bargainDetialId > 0)
            {
                info3 = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount, 0, bargainDetialId, 0);
                if (info3 == null)
                {
                    return null;
                }
                BargainDetialInfo bargainDetialInfo = new BargainDao().GetBargainDetialInfo(bargainDetialId);
                if (bargainDetialInfo == null)
                {
                    return null;
                }
                item.TemplateId = info3.FreightTemplateId.ToString();
                item.Amount = decimal.Round(bargainDetialInfo.Number * bargainDetialInfo.Price, 2);
                item.Total = item.Amount;
                item.Exemption = 0M;
                item.ShipCost = 0M;
                item.GetPointNumber = info3.PointNumber * info3.Quantity;
                item.MemberPointNumber = currentMember.Points;
                item.LineItems.Add(info3);
                list.Add(item);
                return list;
            }
            info3 = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount, 0, 0, limitedTimeDiscountId);
            if (info3 == null)
            {
                return null;
            }
            item.TemplateId = info3.FreightTemplateId.ToString();
            item.Amount = decimal.Round(info3.SubTotal, 2);
            item.Total = item.Amount;
            item.Exemption = 0M;
            item.ShipCost = 0M;
            item.GetPointNumber = info3.PointNumber * info3.Quantity;
            item.MemberPointNumber = currentMember.Points;
            item.LineItems.Add(info3);
            list.Add(item);
            return list;
        }

        public static List<ShoppingCartInfo> GetOrderSummitCart()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return null;
            }
            List<ShoppingCartInfo> orderSummitCart = new ShoppingCartDao().GetOrderSummitCart(currentMember);
            if (orderSummitCart.Count == 0)
            {
                return null;
            }
            return orderSummitCart;
        }

        public static ShoppingCartInfo GetShoppingCart()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return null;
            }
            ShoppingCartInfo shoppingCart = new ShoppingCartDao().GetShoppingCart(currentMember);
            if (shoppingCart.LineItems.Count == 0)
            {
                return null;
            }
            return shoppingCart;
        }

        public static ShoppingCartInfo GetShoppingCart(int Templateid)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return null;
            }
            ShoppingCartInfo shoppingCart = new ShoppingCartDao().GetShoppingCart(currentMember, Templateid);
            if (shoppingCart.LineItems.Count == 0)
            {
                return null;
            }
            return shoppingCart;
        }

        public static ShoppingCartInfo GetShoppingCart(string productSkuId, int buyAmount)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ShoppingCartItemInfo item = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount, 0, 0, 0);
            if (item == null)
            {
                return null;
            }
            info.LineItems.Add(item);
            return info;
        }

        public static List<ShoppingCartInfo> GetShoppingCartAviti(int type = 0)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return null;
            }
            List<ShoppingCartInfo> shoppingCartAviti = new ShoppingCartDao().GetShoppingCartAviti(currentMember, type);
            if (shoppingCartAviti.Count == 0)
            {
                return null;
            }
            return shoppingCartAviti;
        }

        public static int GetSkuStock(string skuId, int type = 0, int exchangeId = 0)
        {
            int stock = new SkuDao().GetSkuItem(skuId).Stock;
            if (type > 0)
            {
                int productId = int.Parse(skuId.Split(new char[] { '_' })[0]);
                PointExchangeProductInfo productInfo = new PointExChangeDao().GetProductInfo(exchangeId, productId);
                if (productInfo == null)
                {
                    return stock;
                }
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                int eachMaxNumber = 0;
                int num4 = new PointExChangeDao().GetUserProductExchangedCount(exchangeId, productId, currentMember.UserId);
                int productExchangedCount = new PointExChangeDao().GetProductExchangedCount(exchangeId, productId);
                int num6 = ((productInfo.ProductNumber - productExchangedCount) >= 0) ? (productInfo.ProductNumber - productExchangedCount) : 0;
                if (productInfo.EachMaxNumber > 0)
                {
                    if (num4 < productInfo.EachMaxNumber)
                    {
                        if (productInfo.EachMaxNumber <= num6)
                        {
                            eachMaxNumber = productInfo.EachMaxNumber;
                        }
                        else
                        {
                            eachMaxNumber = num6;
                        }
                    }
                    else
                    {
                        eachMaxNumber = 0;
                    }
                }
                else
                {
                    eachMaxNumber = num6;
                }
                if (eachMaxNumber > 0)
                {
                    stock = eachMaxNumber;
                }
            }
            return stock;
        }

        public static void RemoveLineItem(string skuId, int type, int limitedTimeDiscountId)
        {
            new ShoppingCartDao().RemoveLineItem(Globals.GetCurrentMemberUserId(false), skuId, type, limitedTimeDiscountId);
        }

        public static void UpdateLineItemQuantity(string skuId, int quantity, int type, int limitedTimeDiscountId)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (quantity <= 0)
            {
                RemoveLineItem(skuId, type, limitedTimeDiscountId);
            }
            new ShoppingCartDao().UpdateLineItemQuantity(currentMember, skuId, quantity, type, limitedTimeDiscountId);
        }
    }
}

