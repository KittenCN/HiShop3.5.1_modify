namespace Hidistro.UI.Web.API
{
    using Aop.Api.Response;
   using  global:: ControlPanel.Promotions;
    using  global:: ControlPanel.WeiXin;
    using Hidistro.ControlPanel.Bargain;
    using Hidistro.ControlPanel.CashBack;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Settings;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Core.Json;
    using Hidistro.Entities;
    using Hidistro.Entities.Bargain;
    using Hidistro.Entities.CashBack;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.FenXiao;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Settings;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.Vshop;
    using Hishop.AlipayFuwu.Api.Model;
    using Hishop.AlipayFuwu.Api.Util;
    using Hishop.Weixin.MP.Api;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;

    public class VshopProcess : IHttpHandler, IRequiresSessionState
    {
        private int buyAmount;
        private UpdateStatistics myEvent = new UpdateStatistics();
        private StatisticNotifier myNotifier = new StatisticNotifier();
        private string productSku;

        private void AddCommissions(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            string msg = "";
            if (!DistributorsBrower.IsExitsCommionsRequest())
            {
                if (this.CheckAddCommissions(context, ref msg))
                {
                    string str2 = context.Request["account"].Trim();
                    decimal num = decimal.Parse(context.Request["commissionmoney"].Trim());
                    int result = 0;
                    int.TryParse(context.Request["requesttype"].Trim(), out result);
                    string realName = context.Request["realname"].Trim();
                    string str4 = context.Request["bankname"].Trim();
                    BalanceDrawRequestInfo balancerequestinfo = new BalanceDrawRequestInfo {
                        MerchantCode = str2,
                        Amount = num,
                        RequestType = result
                    };
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    if (realName == "")
                    {
                        realName = currentMember.RealName;
                    }
                    else
                    {
                        currentMember.RealName = realName;
                    }
                    DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(currentMember.UserId);
                    if (distributorInfo != null)
                    {
                        balancerequestinfo.StoreName = distributorInfo.StoreName;
                    }
                    balancerequestinfo.AccountName = realName;
                    balancerequestinfo.BankName = str4;
                    if ((string.IsNullOrEmpty(currentMember.OpenId) || (currentMember.OpenId.Length < 0x1c)) && ((result == 3) || (result == 0)))
                    {
                        msg = "{\"success\":false,\"msg\":\"您的帐号未绑定，无法通过微信支付佣金！\"}";
                    }
                    else if (DistributorsBrower.AddBalanceDrawRequest(balancerequestinfo, currentMember))
                    {
                        try
                        {
                            this.myNotifier.updateAction = UpdateAction.OrderUpdate;
                            this.myNotifier.actionDesc = "申请提现";
                            this.myNotifier.RecDateUpdate = DateTime.Today;
                            this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                            this.myNotifier.UpdateDB();
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            BalanceDrawRequestInfo balance = balancerequestinfo;
                            if (balance != null)
                            {
                                Messenger.SendWeiXinMsg_DrawCashRequest(balance);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        msg = "{\"success\":true,\"msg\":\"申请成功！\"}";
                    }
                    else
                    {
                        msg = "{\"success\":false,\"msg\":\"真实姓名或手机号未填写！\"}";
                    }
                }
            }
            else
            {
                msg = "{\"success\":false,\"msg\":\"您有申请正在审核中！\"}";
            }
            context.Response.Write(msg);
            context.Response.End();
        }

        private void AddCustomDistributorStatistic(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string str = context.Request["logo"];
            string str2 = context.Request["storeName"];
            string str3 = context.Request["orderNums"];
            string s = context.Request["commTotalSum"];
            CustomDistributorStatistic custom = new CustomDistributorStatistic {
                OrderNums = string.IsNullOrEmpty(str3) ? 0 : int.Parse(str3),
                Logo = str,
                StoreName = str2,
                CommTotalSum = string.IsNullOrEmpty(str3) ? 0f : float.Parse(s)
            };
            bool flag = VShopHelper.InsertCustomDistributorStatistic(custom);
            string str5 = string.Empty;
            if (flag)
            {
                str5 = "{\"success\":\"true\",\"message\":\"添加成功！\"}";
            }
            context.Response.Write(str5);
            context.Response.End();
        }

        public void AddDistributor(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            StringBuilder sb = new StringBuilder();
            if (this.CheckRequestDistributors(context, sb))
            {
                if (!SystemAuthorizationHelper.CheckDistributorIsCanAuthorization())
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"平台分销商数已达上限，请联系商家客服！\"}");
                }
                else
                {
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    if (currentMember == null)
                    {
                        context.Response.Write("{\"success\":false,\"msg\":\"请先登录再申请！\"}");
                    }
                    else
                    {
                        bool flag2 = false;
                        SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                        if (!masterSettings.DistributorApplicationCondition)
                        {
                            flag2 = true;
                        }
                        else
                        {
                            int finishedOrderMoney = masterSettings.FinishedOrderMoney;
                            if (finishedOrderMoney > 0)
                            {
                                DataTable userOrderPaidWaitFinish = OrderHelper.GetUserOrderPaidWaitFinish(currentMember.UserId);
                                decimal total = 0M;
                                decimal num3 = 0M;
                                OrderInfo orderInfo = null;
                                for (int i = 0; i < userOrderPaidWaitFinish.Rows.Count; i++)
                                {
                                    orderInfo = OrderHelper.GetOrderInfo(userOrderPaidWaitFinish.Rows[i]["orderid"].ToString());
                                    if (orderInfo != null)
                                    {
                                        total = orderInfo.GetTotal();
                                        if (total > 0M)
                                        {
                                            num3 += total;
                                        }
                                    }
                                }
                                if ((currentMember.Expenditure + num3) >= finishedOrderMoney)
                                {
                                    flag2 = true;
                                }
                            }
                            if (((!flag2 && masterSettings.EnableDistributorApplicationCondition) && (!string.IsNullOrEmpty(masterSettings.DistributorProductsDate) && !string.IsNullOrEmpty(masterSettings.DistributorProducts))) && masterSettings.DistributorProductsDate.Contains("|"))
                            {
                                DateTime result = new DateTime();
                                DateTime time2 = new DateTime();
                                bool flag3 = DateTime.TryParse(masterSettings.DistributorProductsDate.Split(new char[] { '|' })[0].ToString(), out result);
                                bool flag4 = DateTime.TryParse(masterSettings.DistributorProductsDate.Split(new char[] { '|' })[1].ToString(), out time2);
                                if (((flag3 && flag4) && ((DateTime.Now.CompareTo(result) >= 0) && (DateTime.Now.CompareTo(time2) < 0))) && MemberProcessor.CheckMemberIsBuyProds(currentMember.UserId, masterSettings.DistributorProducts, new DateTime?(result), new DateTime?(time2)))
                                {
                                    flag2 = true;
                                }
                            }
                            if ((!flag2 && (masterSettings.RechargeMoneyToDistributor > 0M)) && (MemberAmountProcessor.GetUserMaxAmountDetailed(currentMember.UserId) >= masterSettings.RechargeMoneyToDistributor))
                            {
                                flag2 = true;
                            }
                        }
                        if (!flag2)
                        {
                            context.Response.Write("{\"success\":false,\"msg\":\"您未达到申请分销商的条件！\"}");
                        }
                        else
                        {
                            DistributorsInfo distributors = new DistributorsInfo {
                                RequestAccount = context.Request["acctount"].Trim(),
                                StoreName = context.Request["stroename"].Trim(),
                                StoreDescription = context.Request["descriptions"].Trim(),
                                Logo = context.Request["logo"].Trim(),
                                BackImage = "",
                                CellPhone = context.Request["CellPhone"].Trim()
                            };
                            DistributorGradeInfo isDefaultDistributorGradeInfo = DistributorGradeBrower.GetIsDefaultDistributorGradeInfo();
                            if (isDefaultDistributorGradeInfo == null)
                            {
                                context.Response.Write("{\"success\":false,\"msg\":\"默认分销商等级未设置，请联系商家客服！\"}");
                            }
                            else
                            {
                                distributors.DistriGradeId = isDefaultDistributorGradeInfo.GradeId;
                                if (DistributorsBrower.AddDistributors(distributors))
                                {
                                    if (HttpContext.Current.Request.Cookies["Vshop-Member"] != null)
                                    {
                                        string name = "Vshop-ReferralId";
                                        HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(-1.0);
                                        HttpCookie cookie = new HttpCookie(name) {
                                            Value = Globals.GetCurrentMemberUserId(false).ToString(),
                                            Expires = DateTime.Now.AddYears(10)
                                        };
                                        HttpContext.Current.Response.Cookies.Add(cookie);
                                    }
                                    this.myNotifier.updateAction = UpdateAction.MemberUpdate;
                                    this.myNotifier.actionDesc = "会员申请成为分销商";
                                    this.myNotifier.RecDateUpdate = DateTime.Today;
                                    this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                                    this.myNotifier.UpdateDB();
                                    context.Response.Write("{\"success\":true}");
                                }
                                else
                                {
                                    context.Response.Write("{\"success\":false,\"msg\":\"店铺名称已存在，请重新输入店铺名称\"}");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + sb.ToString() + "\"}");
            }
        }

        private void AddDistributorProducts(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request["Params"]))
            {
                string json = context.Request["Params"];
                JObject source = JObject.Parse(json);
                if (source.Count > 0)
                {
                   
                    DistributorsBrower.AddDistributorProductId((from s in source.Values() select Convert.ToInt32(s)).ToList<int>());
                }
            }
            context.Response.Write("{\"success\":\"true\",\"msg\":\"保存成功\"}");
            context.Response.End();
        }

        private void AddFavorite(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请先登录才可以收藏商品\"}");
            }
            else if (ProductBrowser.AddProductToFavorite(Convert.ToInt32(context.Request["ProductId"]), currentMember.UserId))
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
            }
        }

        private void AddParticipant(HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            string str = context.Request.Form["ActivityId"];
            string str2 = context.Request.Form["buyNum"];
            string str3 = context.Request.Form["SkuId"];
            string str4 = context.Request.Form["Options"];
            if ((string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str2)) || string.IsNullOrEmpty(str3))
            {
                builder.Append("\"Status\":\"false\",\"Msg\":\"参数错误，不能提交1\"");
            }
            else
            {
                int result = 0;
                OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(str);
                if ((int.TryParse(str2, out result) && (result > 0)) && (oneyuanTaoInfoById != null))
                {
                    OneyuanTaoParticipantInfo info = new OneyuanTaoParticipantInfo();
                    OneTaoState state = OneyuanTaoHelp.getOneTaoState(oneyuanTaoInfoById);
                    if (state != OneTaoState.进行中)
                    {
                        builder.Append("\"Status\":\"false\",\"Msg\":\"当前活动" + state.ToString() + "，不能参与\"");
                    }
                    else
                    {
                        MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                        if (!MemberProcessor.CheckCurrentMemberIsInRange(oneyuanTaoInfoById.FitMember, oneyuanTaoInfoById.DefualtGroup, oneyuanTaoInfoById.CustomGroup))
                        {
                            builder.Append("\"Status\":false,\"Msg\":\"您当前不能参与该活动！！\"");
                        }
                        else
                        {
                            int eachCanBuyNum = oneyuanTaoInfoById.EachCanBuyNum;
                            int userAlreadyBuyNum = OneyuanTaoHelp.GetUserAlreadyBuyNum(currentMember.UserId, str);
                            if ((result + userAlreadyBuyNum) > eachCanBuyNum)
                            {
                                builder.Append("\"Status\":false,\"Msg\":\"已经超出最大购买份数！！\"");
                            }
                            else
                            {
                                info.Pid = OneyuanTaoHelp.GetOrderNumber(false);
                                info.ActivityId = str;
                                info.UserId = Globals.GetCurrentMemberUserId(false);
                                info.SkuId = str3;
                                info.SkuIdStr = OneyuanTaoHelp.GetSkuStrBySkuId(str3, true);
                                info.BuyNum = result;
                                info.TotalPrice = oneyuanTaoInfoById.EachPrice * result;
                                info.ProductPrice = oneyuanTaoInfoById.ProductPrice;
                                if (!string.IsNullOrEmpty(str4))
                                {
                                    SKUItem item = ShoppingProcessor.GetProductAndSku(MemberProcessor.GetCurrentMember(), oneyuanTaoInfoById.ProductId, str4);
                                    if (item != null)
                                    {
                                        info.ProductPrice = item.SalePrice;
                                    }
                                }
                                if (OneyuanTaoHelp.AddParticipant(info))
                                {
                                    builder.Append("\"Status\":true,\"Msg\":\"参与活动成功！\",\"Aid\":\"" + info.Pid + "\"");
                                }
                                else
                                {
                                    builder.Append("\"Status\":false,\"Msg\":\"参与活动失败\"");
                                }
                            }
                        }
                    }
                }
                else
                {
                    builder.Append("\"Status\":\"false\",\"Msg\":\"参数错误，不能提交2\"");
                }
            }
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }

        private void AddProductConsultations(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ProductConsultationInfo productConsultation = new ProductConsultationInfo {
                ConsultationDate = DateTime.Now,
                ConsultationText = context.Request["ConsultationText"],
                ProductId = Convert.ToInt32(context.Request["ProductId"]),
                UserEmail = currentMember.Email,
                UserId = currentMember.UserId,
                UserName = currentMember.UserName
            };
            if (ProductBrowser.InsertProductConsultation(productConsultation))
            {
                context.Response.Write("{\"success\":true}");
                try
                {
                    ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(productConsultation.ProductId);
                    if (productBaseInfo != null)
                    {
                        Messenger.SendWeiXinMsg_ProductAsk(productBaseInfo.ProductName, "", productConsultation.ConsultationText, productConsultation.UserName);
                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
            }
        }

        private void AddProductReview(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                bool flag = false;
                int productid = Globals.RequestFormNum("ProductId");
                string orderid = Globals.RequestFormStr("orderid");
                string skuId = Globals.RequestFormStr("skuid");
                int itemid = Globals.RequestFormNum("itemid");
                int userId = currentMember.UserId;
                LineItemInfo latestOrderItemByProductIDAndUserID = null;
                if (string.IsNullOrEmpty(skuId))
                {
                    flag = true;
                    latestOrderItemByProductIDAndUserID = ProductBrowser.GetLatestOrderItemByProductIDAndUserID(productid, userId);
                    if (latestOrderItemByProductIDAndUserID == null)
                    {
                        context.Response.Write("{\"success\":false, \"msg\":\"完成订单后才能评价该商品\"}");
                        return;
                    }
                    skuId = latestOrderItemByProductIDAndUserID.SkuId;
                    orderid = latestOrderItemByProductIDAndUserID.OrderID;
                    itemid = latestOrderItemByProductIDAndUserID.ID;
                }
                if ((latestOrderItemByProductIDAndUserID != null) || ProductBrowser.IsReview(orderid, skuId, productid, userId))
                {
                    string str3 = "该商品您已经评价过";
                    if (flag)
                    {
                        str3 = "完成订单后才能评价该商品";
                    }
                    context.Response.Write("{\"success\":false, \"msg\":\"" + str3 + "\"}");
                }
                else
                {
                    LineItemInfo info3 = ProductBrowser.GetReturnMoneyByOrderIDAndProductID(orderid, skuId, itemid);
                    if (info3 != null)
                    {
                        if (info3.OrderItemsStatus == OrderStatus.Finished)
                        {
                            ProductReviewInfo review = new ProductReviewInfo {
                                ReviewDate = DateTime.Now,
                                ReviewText = context.Request["ReviewText"],
                                ProductId = productid,
                                UserEmail = currentMember.Email,
                                UserId = currentMember.UserId,
                                UserName = currentMember.UserName,
                                OrderId = orderid,
                                SkuId = skuId,
                                OrderItemID = itemid
                            };
                            if (ProductBrowser.InsertProductReview(review))
                            {
                                context.Response.Write("{\"success\":true}");
                            }
                            else
                            {
                                context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"success\":false, \"msg\":\"订单完成后，才能对商品进行评价\"}");
                        }
                    }
                    else
                    {
                        context.Response.Write("{\"success\":false, \"msg\":\"您未购买该商品，不能评价\"}");
                    }
                }
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请先登录\"}");
            }
        }

        private void AddShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                ShippingAddressInfo shippingAddress = new ShippingAddressInfo {
                    Address = context.Request.Form["address"],
                    CellPhone = context.Request.Form["cellphone"],
                    ShipTo = context.Request.Form["shipTo"],
                    Zipcode = "",
                    IsDefault = true,
                    UserId = currentMember.UserId,
                    RegionId = Convert.ToInt32(context.Request.Form["regionSelectorValue"])
                };
                if (MemberProcessor.AddShippingAddress(shippingAddress) > 0)
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        private void AdjustCommissions(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            string msg = "";
            if (this.CheckAjustCommissions(context, ref msg))
            {
                decimal result = 0M;
                decimal num2 = 0M;
                decimal.TryParse(context.Request["adjustprice"], out result);
                decimal.TryParse(context.Request["commssionprice"], out num2);
                string str2 = ShoppingProcessor.UpdateAdjustCommssions(context.Request["orderId"], context.Request["skuId"], num2, result);
                if (str2 == "1")
                {
                    msg = "{\"success\":true,\"msg\":\"修改金额成功！\"}";
                }
                else
                {
                    msg = "{\"success\":false,\"msg\":\"优惠金额修改失败！原因是：" + str2 + "\"}";
                }
            }
            context.Response.Write(msg);
            context.Response.End();
        }

        private bool AutoAddDistributorProducts()
        {
            int total = 0;
            List<int> productList = new List<int>();
            foreach (DataRow row in ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, 0, "", 1, 0x2710, out total, "DisplaySequence", "desc", false).Rows)
            {
                int item = (int) row["ProductId"];
                productList.Add(item);
            }
            if (productList.Count > 0)
            {
                DistributorsBrower.AddDistributorProductId(productList);
                return true;
            }
            return false;
        }

        private bool AutoDeleteDistributorProducts()
        {
            int total = 0;
            List<int> productList = new List<int>();
            foreach (DataRow row in ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, 0, "", 1, 0x186a0, out total, "DisplaySequence", "desc", true).Rows)
            {
                int item = (int) row["ProductId"];
                productList.Add(item);
            }
            if (productList.Count > 0)
            {
                DistributorsBrower.DeleteDistributorProductIds(productList);
                return true;
            }
            return false;
        }

        public void BindOldUserName(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo usernameMember = new MemberInfo();
            string str = context.Request["userName"];
            string sourceData = context.Request["password"];
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (!string.IsNullOrEmpty(str))
            {
                usernameMember = MemberProcessor.GetusernameMember(str);
                if (usernameMember == null)
                {
                    builder.Append("\"Status\":\"-1\",\"Msg\":\"帐号不存在！\"");
                    builder.Append("}");
                    context.Response.Write(builder.ToString());
                    return;
                }
                if ((usernameMember.OpenId != null) && (usernameMember.OpenId.Length > 0))
                {
                    builder.Append("\"Status\":\"-1\",\"Msg\":\"帐号已经绑定过其他微信！\"");
                    builder.Append("}");
                    context.Response.Write(builder.ToString());
                    return;
                }
                if (usernameMember.Status == Convert.ToInt32(UserStatus.DEL))
                {
                    builder.Append("\"Status\":\"-1\",\"Msg\":\"您的帐号在系统中已删除，不能绑定！\"");
                    return;
                }
                if (usernameMember.Password == HiCryptographer.Md5Encrypt(sourceData))
                {
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    if (currentMember.UserId != usernameMember.UserId)
                    {
                        if ((currentMember.ReferralUserId == usernameMember.ReferralUserId) || (currentMember.ReferralUserId == usernameMember.UserId))
                        {
                            DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(currentMember.UserId);
                            DistributorsInfo info4 = DistributorsBrower.GetDistributorInfo(usernameMember.UserId);
                            int userOrders = ShoppingProcessor.GetUserOrders(currentMember.UserId);
                            ShoppingProcessor.GetUserOrders(usernameMember.UserId);
                            if ((info4 == null) && (distributorInfo == null))
                            {
                                if (userOrders == 0)
                                {
                                    if (MemberProcessor.DelUserMessage(currentMember, usernameMember.UserId))
                                    {
                                        HiCache.Remove(string.Format("DataCache-Member-{0}", currentMember.UserId));
                                        builder.Append(this.resultstring(usernameMember.UserId, context));
                                    }
                                    else
                                    {
                                        builder.Append("\"Status\":\"-1\",\"Msg\":\"删除会员信息失败！\"");
                                    }
                                }
                                else
                                {
                                    builder.Append("\"Status\":\"-1\",\"Msg\":\"当前的登录帐号已产生订单，不能合并！\"");
                                }
                            }
                            else if ((info4 != null) && (distributorInfo == null))
                            {
                                if (userOrders == 0)
                                {
                                    if (MemberProcessor.DelUserMessage(currentMember, usernameMember.UserId))
                                    {
                                        HiCache.Remove(string.Format("DataCache-Member-{0}", currentMember.UserId));
                                        builder.Append(this.resultstring(usernameMember.UserId, context));
                                    }
                                    else
                                    {
                                        builder.Append("\"Status\":\"-1\",\"Msg\":\"删除会员信息失败！\"");
                                    }
                                }
                                else
                                {
                                    builder.Append("\"Status\":\"-1\",\"Msg\":\"会员帐号已产生订单，帐号不能合并！\"");
                                }
                            }
                            else
                            {
                                builder.Append("\"Status\":\"-1\",\"Msg\":\"当前分销商不能合并！\"");
                            }
                        }
                        else
                        {
                            builder.Append("\"Status\":\"-1\",\"Msg\":\"两个帐号不属于同一上级！\"");
                        }
                    }
                    else
                    {
                        builder.Append("\"Status\":\"-1\",\"Msg\":\"不能绑定相同帐号！\"");
                    }
                }
                else
                {
                    builder.Append("\"Status\":\"-1\",\"Msg\":\"密码错误！\"");
                }
            }
            else
            {
                builder.Append("\"Status\":\"-1\",\"Msg\":\"帐号不能为空！\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public void BindUserName(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string userName = context.Request["userName"];
            string password = context.Request["password"];
            string passagain = context.Request["passagain"];
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            string str4 = this.BindUserNameRegist(userName, password, passagain, context);
            builder.Append(str4);
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public string BindUserNameRegist(string userName, string password, string passagain, HttpContext context)
        {
            if (!(password == passagain))
            {
                return "\"Status\":\"-2\"";
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            MemberInfo info2 = new MemberInfo();
            if (string.IsNullOrEmpty(userName))
            {
                return "\"Status\":\"-1\"";
            }
            if (MemberProcessor.GetBindusernameMember(userName) != null)
            {
                return "\"Status\":\"-1\"";
            }
            if (MemberProcessor.BindUserName(currentMember.UserId, userName, HiCryptographer.Md5Encrypt(password)))
            {
                return "\"Status\":\"OK\"";
            }
            return "\"Status\":\"-3\"";
        }

        private void CancelOrder(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string orderId = string.Empty;
            string source = Convert.ToString(context.Request["orderId"]);
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (source.Contains<char>(','))
            {
                string[] strArray = source.Trim().Trim(new char[] { ',' }).Split(new char[] { ',' });
                int num = 0;
                foreach (string str3 in strArray)
                {
                    OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(str3);
                    orderInfo.CloseReason = "买家关闭";
                    if ((currentMember.UserId == orderInfo.UserId) && MemberProcessor.CancelOrder(orderInfo))
                    {
                        orderInfo.OnClosed();
                        num++;
                    }
                }
                if (num > 0)
                {
                    context.Response.Write("{\"success\":true,\"icount\":" + num.ToString() + "}");
                }
                else
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"取消订单失败\"}");
                }
            }
            else
            {
                orderId = source;
                OrderInfo order = ShoppingProcessor.GetOrderInfo(orderId);
                order.CloseReason = "买家关闭";
                if (currentMember.UserId == order.UserId)
                {
                    if (MemberProcessor.CancelOrder(order))
                    {
                        order.OnClosed();
                        context.Response.Write("{\"success\":true,\"icount\":1}");
                    }
                    else
                    {
                        context.Response.Write("{\"success\":false, \"msg\":\"取消订单失败\"}");
                    }
                }
                else
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"只能取消自己的订单\"}");
                }
            }
        }

        private bool CheckAddCommissions(HttpContext context, ref string msg)
        {
            int result = 0;
            if (!int.TryParse(context.Request["requesttype"], out result))
            {
                result = 1;
            }
            string str = context.Request["bankname"].Trim();
            string str2 = context.Request["account"];
            if (((result == 1) && !Globals.CheckReg(str2, @"^1\d{10}$")) && !Globals.CheckReg(str2, @"^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$"))
            {
                msg = "{\"success\":false,\"msg\":\"支付宝账号格式不正确！\"}";
                return false;
            }
            if ((result == 2) && (str2.Length < 4))
            {
                msg = "{\"success\":false,\"msg\":\"收款帐号不能为空，请准确填写！\"}";
                return false;
            }
            if ((result == 2) && (str.Length < 2))
            {
                msg = "{\"success\":false,\"msg\":\"帐号说明不能为空！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request["commissionmoney"].Trim()))
            {
                msg = "{\"success\":false,\"msg\":\"提现金额不允许为空！\"}";
                return false;
            }
            if (decimal.Parse(context.Request["commissionmoney"].Trim()) <= 0M)
            {
                msg = "{\"success\":false,\"msg\":\"提现金额必须大于0的纯数字！\"}";
                return false;
            }
            decimal num2 = 0M;
            decimal.TryParse(SettingsManager.GetMasterSettings(false).MentionNowMoney, out num2);
            if ((num2 > 0M) && (decimal.Parse(context.Request["commissionmoney"].Trim()) < 0M))
            {
                msg = "{\"success\":false,\"msg\":\"提现金额必须大于等于" + num2.ToString() + "元！\"}";
                return false;
            }
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(false);
            if (decimal.Parse(context.Request["commissionmoney"].Trim()) > currentDistributors.ReferralBlance)
            {
                msg = "{\"success\":false,\"msg\":\"提现金额必须为小于现有佣金余额！\"}";
                return false;
            }
            return true;
        }

        private bool CheckAjustCommissions(HttpContext context, ref string msg)
        {
            if (string.IsNullOrEmpty(context.Request["orderId"]))
            {
                msg = "{\"success\":false,\"msg\":\"订单号不允许为空！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request["skuId"]))
            {
                msg = "{\"success\":false,\"msg\":\"商品规格不允许为空！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request["adjustprice"]))
            {
                msg = "{\"success\":false,\"msg\":\"请输入要调整的价格！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request["commssionprice"]))
            {
                msg = "{\"success\":false,\"msg\":\"佣金金额值不对！\"}";
                return false;
            }
            if ((Convert.ToDecimal(context.Request["adjustprice"]) >= 0M) && (Convert.ToDecimal(context.Request["ajustprice"]) <= Convert.ToDecimal(context.Request["commssionprice"])))
            {
                return true;
            }
            msg = "{\"success\":false,\"msg\":\"输入金额必须在0~" + context.Request["commssionprice"].ToString() + "之间！\"}";
            return false;
        }

        public void CheckCoupon(HttpContext context)
        {
            string str = context.Request["CouponId"];
            if (string.IsNullOrEmpty(str))
            {
                context.Response.Write("{\"status\":\"N\",\"tips\":\"参数错误1\"}");
            }
            else
            {
                int result = 0;
                if (int.TryParse(str, out result))
                {
                    context.Response.Write("{\"status\":\"Y\",\"tips\":\"" + CouponHelper.GetCouponsProductIds(result) + "\"}");
                }
                else
                {
                    context.Response.Write("{\"status\":\"N\",\"tips\":\"参数错误2\"}");
                }
            }
        }

        public void checkdistribution(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            ShoppingCartInfo shoppingCart = null;
            StringBuilder builder = new StringBuilder();
            if ((int.TryParse(context.Request["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(context.Request["from"])) && (context.Request["from"] == "signBuy"))
            {
                this.productSku = context.Request["productSku"];
                shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
            }
            else
            {
                shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            }
            string regionId = context.Request["city"];
            string str2 = "";
            foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
            {
                if (info2.FreightTemplateId > 0)
                {
                    str2 = str2 + info2.FreightTemplateId + ",";
                }
            }
            if (!string.IsNullOrEmpty(str2))
            {
                DataTable specifyRegionGroupsModeId = SettingsHelper.GetSpecifyRegionGroupsModeId(str2.Substring(0, str2.Length - 1), regionId);
                StringBuilder builder2 = new StringBuilder();
                builder2.AppendLine(" <button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择配送方式<span class=\"caret\"></span></button>");
                builder2.AppendLine("<ul id=\"shippingTypeUl\" class=\"dropdown-menu\" role=\"menu\">");
                if (specifyRegionGroupsModeId.Rows.Count > 0)
                {
                    for (int i = 0; i < specifyRegionGroupsModeId.Rows.Count; i++)
                    {
                        string str3 = this.getModelType(int.Parse(specifyRegionGroupsModeId.Rows[i]["ModeId"].ToString()));
                        builder2.AppendFormat(string.Concat(new object[] { "<li><a href=\"#\" name=\"", specifyRegionGroupsModeId.Rows[i]["ModeId"], "\" value=\"", specifyRegionGroupsModeId.Rows[i]["ModeId"], "\">", str3, "</a></li>" }), new object[0]);
                    }
                }
                else
                {
                    builder2.AppendFormat("<li><a href=\"#\" name=\"0\" value=\"0\">包邮</a></li>", new object[0]);
                }
                builder2.AppendLine("</ul>");
                builder.Append(builder2.ToString() ?? "");
            }
            else
            {
                StringBuilder builder3 = new StringBuilder();
                builder3.AppendLine(" <button type='button' class='btn btn-default dropdown-toggle' data-toggle='dropdown'>请选择配送方式<span class='caret'></span></button>");
                builder3.AppendLine("<ul id='shippingTypeUl' class='dropdown-menu' role='menu'>");
                builder3.AppendFormat("<li><a href='#' name='0' value='0'>包邮</a></li>", new object[0]);
                builder3.AppendLine("</ul>");
                builder.Append(builder3.ToString() ?? "");
            }
            context.Response.Write(builder.ToString());
        }

        private void CheckFavorite(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else if (ProductBrowser.ExistsProduct(Convert.ToInt32(context.Request["ProductId"]), currentMember.UserId))
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false}");
            }
        }

        public int CheckFoucs()
        {
            int num = 0;
            int currentMemberUserId = Globals.GetCurrentMemberUserId(false);
            if (currentMemberUserId == 0)
            {
                return 1;
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            int isFollowWeixin = 0;
            if (currentMember == null)
            {
                string getCurrentWXOpenId = Globals.GetCurrentWXOpenId;
                if (!string.IsNullOrEmpty(getCurrentWXOpenId))
                {
                    currentMember = MemberProcessor.GetOpenIdMember(getCurrentWXOpenId, "wx");
                }
            }
            if (currentMember == null)
            {
                num = 2;
            }
            else
            {
                isFollowWeixin = currentMember.IsFollowWeixin;
                if (isFollowWeixin == 0)
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                    string tOKEN = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
                    if (tOKEN.Contains("errmsg") && tOKEN.Contains("errcode"))
                    {
                        isFollowWeixin = 2;
                    }
                    else if (!string.IsNullOrEmpty(currentMember.OpenId))
                    {
                        string userInfosByOpenID = BarCodeApi.GetUserInfosByOpenID(tOKEN, currentMember.OpenId);
                        var anonymousTypeObject = new {
                            subscribe = "",
                            nickname = "",
                            headimgurl = ""
                        };
                        if (JsonConvert.DeserializeAnonymousType(userInfosByOpenID, anonymousTypeObject).subscribe.Trim() != "1")
                        {
                            isFollowWeixin = 0;
                        }
                        else
                        {
                            isFollowWeixin = 1;
                            MemberProcessor.UpdateUserFollowStateByUserId(currentMemberUserId, 1, "wx");
                        }
                    }
                }
            }
            switch (isFollowWeixin)
            {
                case 0:
                    return 3;

                case 1:
                    return 4;
            }
            return 5;
        }

        private bool CheckRequestDistributors(HttpContext context, StringBuilder sb)
        {
            if (string.IsNullOrEmpty(context.Request["stroename"]))
            {
                sb.AppendFormat("请输入店铺名称", new object[0]);
                return false;
            }
            if (context.Request["stroename"].Length > 20)
            {
                sb.AppendFormat("请输入店铺名称字符不多于20个字符", new object[0]);
                return false;
            }
            if (!string.IsNullOrEmpty(context.Request["descriptions"]) && (context.Request["descriptions"].Trim().Length > 30))
            {
                sb.AppendFormat("店铺描述字不能多于30个字", new object[0]);
                return false;
            }
            return true;
        }

        private bool CheckUpdateDistributors(HttpContext context, StringBuilder sb)
        {
            if (string.IsNullOrEmpty(context.Request["stroename"]))
            {
                sb.AppendFormat("请输入店铺名称", new object[0]);
                return false;
            }
            if (context.Request["stroename"].Length > 20)
            {
                sb.AppendFormat("请输入店铺名称字符不多于20个字符", new object[0]);
                return false;
            }
            if (!string.IsNullOrEmpty(context.Request["descriptions"]) && (context.Request["descriptions"].Trim().Length > 30))
            {
                sb.AppendFormat("店铺描述字不能多于30个字", new object[0]);
                return false;
            }
            return true;
        }

        public void ClearQRCodeScanInfo(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            string str = "";
            string appID = context.Request["AppID"];
            bool flag = WeiXinHelper.DeleteOldQRCode(appID);
            builder.Append("\"Status\":\"" + (flag ? "1" : "-1") + "\",\"RetInfo\":\"" + str + "\"");
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public void CombineOrders(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string str = string.Empty;
            string str2 = Globals.RequestFormStr("orderidlist").Trim(new char[] { ',' });
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                str = "{\"type\":\"0\",\"tips\":\"请重新登录！\"}";
            }
            else
            {
                int userId = currentMember.UserId;
                string[] strArray = str2.Split(new char[] { ',' });
                StringBuilder builder = new StringBuilder();
                string gateway = string.Empty;
                foreach (string str4 in strArray)
                {
                    if (!string.IsNullOrEmpty(str4))
                    {
                        OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(str4);
                        if (orderInfo == null)
                        {
                            str = "{\"type\":\"0\",\"tips\":\"参数错误！\"}";
                        }
                        else if (orderInfo.UserId != userId)
                        {
                            str = "{\"type\":\"0\",\"tips\":\"当前订单不是本人的！\"}";
                        }
                        else
                        {
                            if (((orderInfo.OrderStatus == OrderStatus.WaitBuyerPay) && (orderInfo.PaymentTypeId != 0x63)) && (orderInfo.PaymentTypeId != 0))
                            {
                                if (string.IsNullOrEmpty(gateway))
                                {
                                    gateway = orderInfo.Gateway;
                                    builder.Append(orderInfo.OrderId + ",");
                                }
                                else
                                {
                                    if (orderInfo.Gateway != gateway)
                                    {
                                        str = "{\"type\":\"0\",\"tips\":\"所选订单的支付方式不一致！\"}";
                                        break;
                                    }
                                    builder.Append(orderInfo.OrderId + ",");
                                }
                                continue;
                            }
                            str = "{\"type\":\"0\",\"tips\":\"存在非线上支付的订单或非待付款的订单！\"}";
                        }
                        break;
                    }
                }
                string str5 = builder.ToString().Trim(new char[] { ',' });
                if (!string.IsNullOrEmpty(str5))
                {
                    string orderMarking = this.GenerateOrderId();
                    ShoppingProcessor.CombineOrderToPay(str5, orderMarking);
                    str = "{\"type\":\"1\",\"tips\":\"" + orderMarking + "\"}";
                }
                else if (string.IsNullOrEmpty(str))
                {
                    str = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                }
            }
            context.Response.Write(str);
            context.Response.End();
        }

        public void ConfirmOneyuangPrizeAddr(HttpContext context)
        {
            string s = context.Request["shippingId"];
            string str2 = context.Request["ShipToDate"];
            string cellPhone = "";
            string str4 = "";
            string fullPath = "";
            string str6 = context.Request["pid"];
            string str7 = context.Request["Editid"];
            int result = 0;
            if (!int.TryParse(s, out result))
            {
                context.Response.Write("收货人不能为空");
            }
            else
            {
                ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(result);
                if (shippingAddress == null)
                {
                    context.Response.Write("地址信息不完整！");
                }
                else
                {
                    string shipTo = shippingAddress.ShipTo;
                    cellPhone = shippingAddress.CellPhone;
                    str4 = shippingAddress.Address + "(送货时间：" + str2 + ")";
                    fullPath = shippingAddress.RegionId.ToString();
                    int num2 = 0;
                    int.TryParse(fullPath, out num2);
                    fullPath = RegionHelper.GetFullPath(num2);
                    PrizesDeliveQuery query = new PrizesDeliveQuery {
                        Status = 1,
                        ReggionPath = fullPath,
                        Address = str4,
                        Tel = cellPhone,
                        Receiver = shipTo,
                        LogId = "0"
                    };
                    int num3 = 0;
                    int.TryParse(str7, out num3);
                    query.Id = num3;
                    query.Pid = str6;
                    query.RecordType = 1;
                    if (GameHelper.UpdatePrizesDelivery(query))
                    {
                        context.Response.Write("success");
                    }
                    else
                    {
                        context.Response.Write("保存信息失败");
                    }
                }
            }
        }

        public void ConfirmPrizeAddr(HttpContext context)
        {
            string str = context.Request["ShipToDate"];
            string cellPhone = "";
            string str3 = "";
            string s = "";
            string str5 = context.Request["LogId"];
            string str6 = context.Request["id"];
            string str7 = context.Request["shippingId"];
            int result = 0;
            if (!int.TryParse(str7, out result))
            {
                context.Response.Write("收货人不能为空");
            }
            else
            {
                ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(result);
                if (shippingAddress == null)
                {
                    context.Response.Write("地址信息不完整！");
                }
                else
                {
                    string shipTo = shippingAddress.ShipTo;
                    cellPhone = shippingAddress.CellPhone;
                    str3 = shippingAddress.Address + "(送货时间：" + str + ")";
                    s = shippingAddress.RegionId.ToString();
                    int num2 = 0;
                    int.TryParse(s, out num2);
                    s = RegionHelper.GetFullPath(num2);
                    PrizesDeliveQuery query = new PrizesDeliveQuery {
                        Status = 1,
                        ReggionPath = s,
                        Address = str3,
                        Tel = cellPhone,
                        Receiver = shipTo,
                        LogId = str5
                    };
                    int num3 = 0;
                    int.TryParse(str6.Trim(), out num3);
                    query.Id = num3;
                    if (GameHelper.UpdatePrizesDelivery(query))
                    {
                        context.Response.Write("success");
                    }
                    else
                    {
                        context.Response.Write("保存信息失败");
                    }
                }
            }
        }

        public void ConfirmPrizeArriver(HttpContext context)
        {
            int result = 0;
            if (int.TryParse(context.Request["Tabid"], out result))
            {
                PrizesDeliveQuery query = new PrizesDeliveQuery {
                    Status = 3,
                    ReceiveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Id = result,
                    LogId = "0"
                };
                if (GameHelper.UpdateOneyuanDelivery(query))
                {
                    context.Response.Write("success");
                }
                else
                {
                    context.Response.Write("收货确认失败");
                }
            }
            else
            {
                string s = context.Request["pid"];
                string str2 = context.Request["LogId"];
                if (s == "")
                {
                    context.Response.Write("PID为空，请检查！");
                }
                else if (str2 == "")
                {
                    context.Response.Write("logID为空，请检查！");
                }
                else
                {
                    int num2 = 0;
                    if (!int.TryParse(s, out num2))
                    {
                        context.Response.Write("当前状态下不允许操作！");
                    }
                    else
                    {
                        PrizesDeliveQuery query2 = new PrizesDeliveQuery {
                            Status = 3,
                            ReceiveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            Id = num2,
                            LogId = str2
                        };
                        if (GameHelper.UpdatePrizesDelivery(query2))
                        {
                            context.Response.Write("success");
                        }
                        else
                        {
                            context.Response.Write("收货确认失败");
                        }
                    }
                }
            }
        }

        public void countfreight(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = context.Request["id"];
            int result = 0;
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            string city = "";
            if (int.TryParse(s, out result))
            {
                city = RegionHelper.GetCity(result);
                if (city != "0")
                {
                    builder.Append("\"Status\":\"OK\",\"Msg\":\"" + city + "\"");
                }
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public void countfreighttype(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            ShoppingCartInfo shoppingCart = null;
            if ((int.TryParse(context.Request["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(context.Request["from"])) && (context.Request["from"] == "signBuy"))
            {
                this.productSku = context.Request["productSku"];
                shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
            }
            else
            {
                int result = 0;
                if (!string.IsNullOrEmpty(context.Request["TemplateId"]) && int.TryParse(context.Request["TemplateId"], out result))
                {
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart(result);
                }
            }
            DataView defaultView = new DataView();
            if (shoppingCart != null)
            {
                defaultView = SettingsHelper.GetAllFreightItems().DefaultView;
            }
            float num2 = 0f;
            if (defaultView.Count > 0)
            {
                Dictionary<int, ShoppingCartItemInfo> dictionary = new Dictionary<int, ShoppingCartItemInfo>();
                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
                {
                    if (!dictionary.ContainsKey(info2.FreightTemplateId))
                    {
                        info2.SumSubTotal = info2.SubTotal;
                        info2.CubicMeter *= info2.Quantity;
                        info2.FreightWeight *= info2.Quantity;
                        dictionary.Add(info2.FreightTemplateId, info2);
                    }
                    else
                    {
                        ShoppingCartItemInfo local1 = dictionary[info2.FreightTemplateId];
                        local1.SumSubTotal += info2.SubTotal;
                        ShoppingCartItemInfo local2 = dictionary[info2.FreightTemplateId];
                        local2.FreightWeight += info2.FreightWeight * info2.Quantity;
                        ShoppingCartItemInfo local3 = dictionary[info2.FreightTemplateId];
                        local3.CubicMeter += info2.CubicMeter * info2.Quantity;
                        ShoppingCartItemInfo local4 = dictionary[info2.FreightTemplateId];
                        local4.Quantity += info2.Quantity;
                    }
                }
                shoppingCart.LineItems.Clear();
                foreach (KeyValuePair<int, ShoppingCartItemInfo> pair in dictionary)
                {
                    shoppingCart.LineItems.Add(pair.Value);
                }
                foreach (ShoppingCartItemInfo info3 in shoppingCart.LineItems)
                {
                    string str2;
                    if (!info3.IsfreeShipping)
                    {
                        bool flag = false;
                        FreightTemplate templateMessage = SettingsHelper.GetTemplateMessage(info3.FreightTemplateId);
                        if (((templateMessage != null) && (info3.FreightTemplateId > 0)) && !templateMessage.FreeShip)
                        {
                            if (templateMessage.HasFree)
                            {
                                flag = this.IsFreeTemplateShipping(context.Request["RegionId"], info3.FreightTemplateId, int.Parse(context.Request["ModeId"]), info3);
                            }
                            if (!flag)
                            {
                                defaultView.RowFilter = string.Concat(new object[] { " RegionId=", context.Request["RegionId"], " and ModeId=", context.Request["ModeId"], " and TemplateId=", info3.FreightTemplateId, " and IsDefault=0" });
                                if (defaultView.Count != 1)
                                {
                                    goto Label_05F6;
                                }
                                string str = defaultView[0]["MUnit"].ToString();
                                if (str != null)
                                {
                                    if (!(str == "1"))
                                    {
                                        if (str == "2")
                                        {
                                            goto Label_04A6;
                                        }
                                        if (str == "3")
                                        {
                                            goto Label_054E;
                                        }
                                    }
                                    else
                                    {
                                        num2 += this.setferight(float.Parse(info3.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                                    }
                                }
                            }
                        }
                    }
                    continue;
                Label_04A6:
                    if (info3.FreightWeight > 0M)
                    {
                        num2 += this.setferight(float.Parse(info3.FreightWeight.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                    }
                    continue;
                Label_054E:
                    if (info3.CubicMeter > 0M)
                    {
                        num2 += this.setferight(float.Parse(info3.CubicMeter.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                    }
                    continue;
                Label_05F6:;
                    defaultView.RowFilter = string.Concat(new object[] { "  ModeId=", context.Request["ModeId"], " and TemplateId=", info3.FreightTemplateId, " and  IsDefault=1" });
                    if ((defaultView.Count == 1) && ((str2 = defaultView[0]["MUnit"].ToString()) != null))
                    {
                        if (!(str2 == "1"))
                        {
                            if (str2 == "2")
                            {
                                goto Label_073A;
                            }
                            if (str2 == "3")
                            {
                                goto Label_07E2;
                            }
                        }
                        else
                        {
                            num2 += this.setferight(float.Parse(info3.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                        }
                    }
                    continue;
                Label_073A:
                    if (info3.FreightWeight > 0M)
                    {
                        num2 += this.setferight(float.Parse(info3.FreightWeight.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                    }
                    continue;
                Label_07E2:
                    if (info3.CubicMeter > 0M)
                    {
                        num2 += this.setferight(float.Parse(info3.CubicMeter.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                    }
                }
            }
            builder.Append("\"Status\":\"OK\",\"CountFeright\":\"" + num2.ToString("F2") + "\"");
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        private void DeleteDistributorProducts(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request["Params"]))
            {
                string json = context.Request["Params"];
                JObject source = JObject.Parse(json);
                if (source.Count > 0)
                {
                    DistributorsBrower.DeleteDistributorProductIds((from s in source.Values() select Convert.ToInt32(s)).ToList<int>());
                }
            }
            context.Response.Write("{\"success\":\"true\",\"msg\":\"保存成功\"}");
            context.Response.End();
        }

        private void DelFavorite(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (ProductBrowser.DeleteFavorite(Convert.ToInt32(context.Request["favoriteId"])) == 1)
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"取消失败\"}");
            }
        }

        private void DelShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                int userId = currentMember.UserId;
                if (MemberProcessor.DelShippingAddress(Convert.ToInt32(context.Request.Form["shippingid"]), userId))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        public decimal DiscountMoney(IList<ShoppingCartItemInfo> LineItems, out string ActivitiesId, out string ActivitiesName, MemberInfo member, out int CouponId, out string vItemList)
        {
            decimal num = 0M;
            decimal num2 = 0M;
            decimal num3 = 0M;
            CouponId = 0;
            ActivitiesId = "";
            ActivitiesName = "";
            vItemList = "";
            decimal num4 = 0M;
            int num5 = 0;
            foreach (ShoppingCartItemInfo info in LineItems)
            {
                if (info.Type == 0)
                {
                    num4 += info.SubTotal;
                    num5 += info.Quantity;
                }
            }
            DataTable activities = ActivityHelper.GetActivities();
            for (int i = 0; i < activities.Rows.Count; i++)
            {
                if ((int.Parse(activities.Rows[i]["attendTime"].ToString()) != 0) && (int.Parse(activities.Rows[i]["attendTime"].ToString()) <= ActivityHelper.GetActivitiesMember(member.UserId, int.Parse(activities.Rows[i]["ActivitiesId"].ToString()))))
                {
                    continue;
                }
                decimal num7 = 0M;
                int num8 = 0;
                DataTable table2 = ActivityHelper.GetActivities_Detail(int.Parse(activities.Rows[i]["ActivitiesId"].ToString()));
                foreach (ShoppingCartItemInfo info2 in LineItems)
                {
                    if ((info2.Type == 0) && (ActivityHelper.GetActivitiesProducts(int.Parse(activities.Rows[i]["ActivitiesId"].ToString()), info2.ProductId).Rows.Count > 0))
                    {
                        num7 += info2.SubTotal;
                        num8 += info2.Quantity;
                        vItemList = vItemList + "," + info2.ProductId;
                    }
                }
                bool flag = false;
                if (table2.Rows.Count > 0)
                {
                    for (int j = 0; j < table2.Rows.Count; j++)
                    {
                        string grades = table2.Rows[j]["MemberGrades"].ToString();
                        string defualtGroup = table2.Rows[j]["DefualtGroup"].ToString();
                        string customGroup = table2.Rows[j]["CustomGroup"].ToString();
                        if (MemberProcessor.CheckCurrentMemberIsInRange(grades, defualtGroup, customGroup))
                        {
                            if (bool.Parse(activities.Rows[i]["isAllProduct"].ToString()))
                            {
                                if (decimal.Parse(table2.Rows[j]["MeetMoney"].ToString()) > 0M)
                                {
                                    if ((num4 != 0M) && (num4 >= decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                        ActivitiesId = ActivitiesId + table2.Rows[table2.Rows.Count - 1]["id"].ToString() + ",";
                                        ActivitiesName = ActivitiesName + table2.Rows[table2.Rows.Count - 1]["ActivitiesName"].ToString() + ",";
                                        CouponId = int.Parse(table2.Rows[table2.Rows.Count - 1]["CouponId"].ToString());
                                        break;
                                    }
                                    if ((num4 != 0M) && (num4 < decimal.Parse(table2.Rows[0]["MeetMoney"].ToString())))
                                    {
                                        break;
                                    }
                                    if ((num4 != 0M) && (num4 >= decimal.Parse(table2.Rows[j]["MeetMoney"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[j]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[j]["ReductionMoney"].ToString());
                                        ActivitiesId = ActivitiesId + table2.Rows[j]["id"].ToString() + ",";
                                        ActivitiesName = ActivitiesName + table2.Rows[j]["ActivitiesName"].ToString() + ",";
                                        CouponId = int.Parse(table2.Rows[j]["CouponId"].ToString());
                                    }
                                }
                                else
                                {
                                    if ((num5 != 0) && (num5 >= int.Parse(table2.Rows[table2.Rows.Count - 1]["MeetNumber"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                        num3 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                        ActivitiesId = ActivitiesId + table2.Rows[table2.Rows.Count - 1]["id"].ToString() + ",";
                                        ActivitiesName = ActivitiesName + table2.Rows[table2.Rows.Count - 1]["ActivitiesName"].ToString() + ",";
                                        CouponId = int.Parse(table2.Rows[table2.Rows.Count - 1]["CouponId"].ToString());
                                        flag = true;
                                        break;
                                    }
                                    if ((num5 != 0) && (num5 < int.Parse(table2.Rows[0]["MeetNumber"].ToString())))
                                    {
                                        break;
                                    }
                                    if ((num5 != 0) && (num5 >= int.Parse(table2.Rows[j]["MeetNumber"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[j]["MeetMoney"].ToString());
                                        num3 = decimal.Parse(table2.Rows[j]["ReductionMoney"].ToString());
                                        ActivitiesId = ActivitiesId + table2.Rows[j]["id"].ToString() + ",";
                                        ActivitiesName = ActivitiesName + table2.Rows[j]["ActivitiesName"].ToString() + ",";
                                        CouponId = int.Parse(table2.Rows[j]["CouponId"].ToString());
                                        flag = true;
                                    }
                                }
                            }
                            else
                            {
                                num4 = num7;
                                num5 = num8;
                                if (decimal.Parse(table2.Rows[j]["MeetMoney"].ToString()) > 0M)
                                {
                                    if ((num4 != 0M) && (num4 >= decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                        ActivitiesId = ActivitiesId + table2.Rows[table2.Rows.Count - 1]["id"].ToString() + ",";
                                        ActivitiesName = ActivitiesName + table2.Rows[table2.Rows.Count - 1]["ActivitiesName"].ToString() + ",";
                                        CouponId = int.Parse(table2.Rows[table2.Rows.Count - 1]["CouponId"].ToString());
                                        break;
                                    }
                                    if ((num4 != 0M) && (num4 < decimal.Parse(table2.Rows[0]["MeetMoney"].ToString())))
                                    {
                                        break;
                                    }
                                    if ((num4 != 0M) && (num4 >= decimal.Parse(table2.Rows[j]["MeetMoney"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[j]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[j]["ReductionMoney"].ToString());
                                        ActivitiesId = ActivitiesId + table2.Rows[j]["id"].ToString() + ",";
                                        ActivitiesName = ActivitiesName + table2.Rows[j]["ActivitiesName"].ToString() + ",";
                                        CouponId = int.Parse(table2.Rows[j]["CouponId"].ToString());
                                    }
                                }
                                else
                                {
                                    if ((num5 != 0) && (num5 >= int.Parse(table2.Rows[table2.Rows.Count - 1]["MeetNumber"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                        ActivitiesId = ActivitiesId + table2.Rows[table2.Rows.Count - 1]["id"].ToString() + ",";
                                        ActivitiesName = ActivitiesName + table2.Rows[table2.Rows.Count - 1]["ActivitiesName"].ToString() + ",";
                                        CouponId = int.Parse(table2.Rows[table2.Rows.Count - 1]["CouponId"].ToString());
                                        flag = true;
                                        break;
                                    }
                                    if ((num5 != 0) && (num5 < int.Parse(table2.Rows[0]["MeetNumber"].ToString())))
                                    {
                                        break;
                                    }
                                    if ((num5 != 0) && (num5 >= int.Parse(table2.Rows[j]["MeetNumber"].ToString())))
                                    {
                                        num2 = decimal.Parse(table2.Rows[j]["MeetMoney"].ToString());
                                        num = decimal.Parse(table2.Rows[j]["ReductionMoney"].ToString());
                                        ActivitiesId = ActivitiesId + table2.Rows[j]["id"].ToString() + ",";
                                        ActivitiesName = ActivitiesName + table2.Rows[j]["ActivitiesName"].ToString() + ",";
                                        CouponId = int.Parse(table2.Rows[j]["CouponId"].ToString());
                                        flag = true;
                                    }
                                }
                            }
                        }
                    }
                    if (flag)
                    {
                        if (num5 > 0)
                        {
                            num3 += num;
                        }
                    }
                    else if (((num4 != 0M) && (num2 != 0M)) && (num4 >= num2))
                    {
                        num3 += num;
                    }
                }
            }
            return num3;
        }

        public void EditPassword(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string sourceData = context.Request["oldPwd"];
            string str2 = context.Request["password"];
            string str3 = context.Request["passagain"];
            if (string.IsNullOrEmpty(MemberProcessor.GetCurrentMember().UserBindName))
            {
                context.Response.Write("{\"Status\":\"-5\"}");
            }
            else
            {
                MemberInfo member = new MemberInfo();
                int currentMemberUserId = Globals.GetCurrentMemberUserId(false);
                if (currentMemberUserId <= 0)
                {
                    context.Response.Write("{\"Status\":\"-1\"}");
                }
                else
                {
                    member = MemberProcessor.GetMember(currentMemberUserId, false);
                    StringBuilder builder = new StringBuilder();
                    builder.Append("{");
                    if (member.Password == HiCryptographer.Md5Encrypt(sourceData))
                    {
                        if (str2 == str3)
                        {
                            if (MemberProcessor.SetPwd(currentMemberUserId.ToString(), HiCryptographer.Md5Encrypt(str2)))
                            {
                                builder.Append("\"Status\":\"OK\"");
                                try
                                {
                                    MemberInfo info3 = member;
                                    if (info3 != null)
                                    {
                                        Messenger.SendWeiXinMsg_PasswordReset(info3);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                builder.Append("\"Status\":\"-3\"");
                            }
                        }
                        else
                        {
                            builder.Append("\"Status\":\"-2\"");
                        }
                    }
                    else
                    {
                        builder.Append("\"Status\":\"-4\"");
                    }
                    builder.Append("}");
                    context.Response.Write(builder.ToString());
                }
            }
        }

        private void ExistsBargainDetial(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int bargainId = Convert.ToInt32(context.Request["BargainId"]);
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            string s = "";
            if (currentMember != null)
            {
                int userId = currentMember.UserId;
                BargainDetialInfo bargainDetialInfo = BargainHelper.GetBargainDetialInfo(bargainId, userId);
                string str2 = "img";
                if (bargainDetialInfo != null)
                {
                    BargainInfo bargainInfo = BargainHelper.GetBargainInfo(bargainId);
                    decimal floorPrice = bargainInfo.FloorPrice;
                    decimal num4 = bargainInfo.InitialPrice - floorPrice;
                    decimal num5 = 0M;
                    if (num4 > 0M)
                    {
                        num5 = (bargainInfo.InitialPrice - bargainDetialInfo.Price) / num4;
                    }
                    else
                    {
                        num5 = 1M;
                    }
                    ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(bargainInfo.ProductId);
                    int number = bargainDetialInfo.Number;
                    string sku = bargainDetialInfo.Sku;
                    string str4 = this.LoadHelpBargainDetial(bargainDetialInfo.Id);
                    if (BargainHelper.ActionIsEnd(bargainDetialInfo.Id))
                    {
                        str2 = "order";
                    }
                    if ((bargainInfo.ActivityStock <= bargainInfo.TranNumber) || (bargainInfo.EndDate <= DateTime.Now))
                    {
                        str2 = "end";
                    }
                    s = string.Concat(new object[] { 
                        "{\"success\":1,\"status\":\"", str2, "\",\"SaleStatus\":\"", productBaseInfo.SaleStatus, "\",\"ProductId\":\"", bargainInfo.ProductId, "\", \"Price\":\"", bargainDetialInfo.Price.ToString("f2"), "\", \"progress\":\"", (int) (num5 * 100M), "\", \"BargainDetialId\":\"", bargainDetialInfo.Id, "\", \"Number\":\"", number, "\",\"Sku\":\"", sku, 
                        "\", \"BargainDetialHtml\":\"", str4, "\"}"
                     });
                }
                else
                {
                    s = "{\"success\":0, \"Price\":\"0\"}";
                }
            }
            context.Response.Write(s);
            context.Response.End();
        }

        private void ExistsHelpBargainDetial(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int bargainDetialId = Convert.ToInt32(context.Request["BargainDetialId"]);
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            string s = "";
            if ((currentMember != null) && (BargainHelper.GeHelpBargainDetialInfo(bargainDetialId, currentMember.UserId) != null))
            {
                s = "{\"success\":1, \"msg\":\"已经存在\"}";
            }
            context.Response.Write(s);
            context.Response.End();
        }

        private bool ExistsProduct(int productId, int exchangeId, int number)
        {
            List<ShoppingCartInfo> shoppingCartAviti = ShoppingCartProcessor.GetShoppingCartAviti(1);
            int num = 0;
            if (shoppingCartAviti != null)
            {
                num = shoppingCartAviti.Count<ShoppingCartInfo>();
            }
            PointExchangeProductInfo productInfo = PointExChangeHelper.GetProductInfo(exchangeId, productId);
            if (productInfo != null)
            {
                int eachMaxNumber = productInfo.EachMaxNumber;
                if (eachMaxNumber == 0)
                {
                    return true;
                }
                if (eachMaxNumber > 0)
                {
                    if ((num + number) > eachMaxNumber)
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        private void FinishOrder(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            bool flag = false;
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(Convert.ToString(context.Request["orderId"]));
            Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
            LineItemInfo info2 = new LineItemInfo();
            foreach (KeyValuePair<string, LineItemInfo> pair in lineItems)
            {
                info2 = pair.Value;
                if ((info2.OrderItemsStatus == OrderStatus.ApplyForRefund) || (info2.OrderItemsStatus == OrderStatus.ApplyForReturns))
                {
                    flag = true;
                }
            }
            if (flag)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"订单中商品有退货(款)不允许完成\"}");
            }
            else if ((orderInfo == null) || !MemberProcessor.ConfirmOrderFinish(orderInfo))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"订单当前状态不允许完成\"}");
            }
            else
            {
                DistributorsBrower.UpdateCalculationCommission(orderInfo);
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                int num = 0;
                if (masterSettings.IsRequestDistributor)
                {
                    if ((Globals.ToNum(masterSettings.FinishedOrderMoney) > 0) && (currentMember.Expenditure >= masterSettings.FinishedOrderMoney))
                    {
                        num = 1;
                    }
                    if (((num != 1) && !string.IsNullOrEmpty(masterSettings.DistributorProducts)) && (masterSettings.DistributorProducts.ToString() != "0"))
                    {
                        string[] source = masterSettings.DistributorProducts.Split(new char[] { ',' });
                        if (source.Length > 0)
                        {
                            source.ToList<string>();
                            foreach (KeyValuePair<string, LineItemInfo> pair2 in lineItems)
                            {
                                LineItemInfo info4 = pair2.Value;
                                if (source.Contains<string>(info4.ProductId.ToString()))
                                {
                                    num = 1;
                                    break;
                                }
                            }
                        }
                    }
                }
                foreach (LineItemInfo info5 in orderInfo.LineItems.Values)
                {
                    if (info5.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                    {
                        ShoppingProcessor.UpdateOrderGoodStatu(orderInfo.OrderId, info5.SkuId, 5, info5.ID);
                    }
                }
                DistributorsInfo userIdDistributors = new DistributorsInfo();
                userIdDistributors = DistributorsBrower.GetUserIdDistributors(orderInfo.UserId);
                if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
                {
                    num = 0;
                }
                context.Response.Write("{\"success\":true,\"isapply\":" + num + "}");
            }
        }

        public void followCheck(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = string.Empty;
            s = "{\"type\":\"0\",\"tips\":\"未知错误！\"}";
            int currentMemberUserId = Globals.GetCurrentMemberUserId(false);
            string str2 = context.Request["followtype"];
            if (currentMemberUserId == 0)
            {
                s = "{\"type\":\"1\",\"tips\":\"当前用户未登入！\"}";
            }
            else if (string.IsNullOrEmpty(str2))
            {
                s = "{\"type\":\"0\",\"tips\":\"非法访问！\"}";
            }
            else
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                int isFollowWeixin = 0;
                if (currentMember == null)
                {
                    string getCurrentWXOpenId = Globals.GetCurrentWXOpenId;
                    if (!string.IsNullOrEmpty(getCurrentWXOpenId))
                    {
                        currentMember = MemberProcessor.GetOpenIdMember(getCurrentWXOpenId, "wx");
                    }
                }
                if (currentMember == null)
                {
                    s = "{\"type\":\"0\",\"tips\":\"用户不存在！\"}";
                }
                else
                {
                    if ((str2 == "wx") && !string.IsNullOrEmpty(currentMember.OpenId))
                    {
                        isFollowWeixin = currentMember.IsFollowWeixin;
                        if (isFollowWeixin == 0)
                        {
                            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                            string tOKEN = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
                            if (tOKEN.Contains("errmsg") && tOKEN.Contains("errcode"))
                            {
                                isFollowWeixin = 2;
                            }
                            else
                            {
                                string userInfosByOpenID = BarCodeApi.GetUserInfosByOpenID(tOKEN, currentMember.OpenId);
                                var anonymousTypeObject = new {
                                    subscribe = "",
                                    nickname = "",
                                    headimgurl = ""
                                };
                                if (JsonConvert.DeserializeAnonymousType(userInfosByOpenID, anonymousTypeObject).subscribe.Trim() != "1")
                                {
                                    isFollowWeixin = 0;
                                }
                                else
                                {
                                    isFollowWeixin = 1;
                                    MemberProcessor.UpdateUserFollowStateByUserId(currentMemberUserId, 1, "wx");
                                }
                            }
                        }
                    }
                    else if ((str2 == "fw") && !string.IsNullOrEmpty(currentMember.AlipayOpenid))
                    {
                        isFollowWeixin = currentMember.IsFollowAlipay;
                        if (isFollowWeixin == 0)
                        {
                            if (MemberProcessor.IsFuwuFollowUser(currentMember.AlipayOpenid))
                            {
                                isFollowWeixin = 1;
                                MemberProcessor.UpdateUserFollowStateByUserId(currentMemberUserId, 1, "fw");
                            }
                            else
                            {
                                Articles articles2 = new Articles {
                                    msgType = "text",
                                    toUserId = currentMember.AlipayOpenid
                                };
                                MessageText text = new MessageText {
                                    content = "系统正在检查用户是否关注服务窗，如果已经关注，请忽略此条信息！"
                                };
                                articles2.text = text;
                                Articles articles = articles2;
                                AlipayMobilePublicMessageCustomSendResponse response = AliOHHelper.CustomSend(articles);
                                if ((response != null) && (response.Code == "200"))
                                {
                                    isFollowWeixin = 1;
                                    MemberProcessor.UpdateUserFollowStateByUserId(currentMemberUserId, 1, "fw");
                                }
                            }
                        }
                    }
                    else
                    {
                        isFollowWeixin = 2;
                    }
                    switch (isFollowWeixin)
                    {
                        case 0:
                            s = "{\"type\":\"2\",\"tips\":\"当前用户未关注！\"}";
                            goto Label_0243;

                        case 1:
                            s = "{\"type\":\"3\",\"tips\":\"当前用已关注！\"}";
                            goto Label_0243;
                    }
                    s = "{\"type\":\"4\",\"tips\":\"非正常访问！！\"}";
                }
            }
        Label_0243:
            context.Response.Write(s);
            context.Response.End();
        }

        public string GenerateOrderId()
        {
            return Globals.GetGenerateId();
        }

        public void GetAliFuWuQRCodeScanInfo(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder();
            string key = context.Request["sceneId"];
            builder.Append("{");
            if (AlipayFuwuConfig.BindAdmin.ContainsKey(key))
            {
                string str2 = AlipayFuwuConfig.BindAdmin[key];
                if (!string.IsNullOrEmpty(str2))
                {
                    builder.Append("\"Status\":\"1\",");
                    builder.Append("\"SceneId\":\"" + str2 + "\"");
                }
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        private void GetBargain(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int id = Convert.ToInt32(context.Request["BargainId"]);
            string s = "";
            BargainInfo bargainInfo = BargainHelper.GetBargainInfo(id);
            if (bargainInfo != null)
            {
                s = "{\"success\":1, \"Title\":\"" + bargainInfo.Title + "\", \"ActivityCover\":\"" + bargainInfo.ActivityCover + "\", \"Remarks\":\"" + bargainInfo.Remarks + "\"}";
            }
            context.Response.Write(s);
            context.Response.End();
        }

        private void GetBargainCount(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            DataTable allBargain = BargainHelper.GetAllBargain();
            int count = allBargain.Rows.Count;
            int length = allBargain.Select(string.Concat(new object[] { " BeginDate< '", DateTime.Now, "' AND EndDate> '", DateTime.Now, "'" })).Length;
            int num3 = allBargain.Select("BeginDate>'" + DateTime.Now + "'").Length;
            int num4 = allBargain.Select(" EndDate< '" + DateTime.Now + "'").Length;
            string s = string.Concat(new object[] { "{\"type\":\"1\",\"allCount\":", count, ",\"ingCount\":", length, ",\"unbegunCount\":", num3, ",\"endCount\":", num4, "}" });
            context.Response.Write(s);
            context.Response.End();
        }

        private void GetBargainList(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            DataTable dt = new DataTable();
            string str = context.Request["status"];
            int num = int.Parse(context.Request["pageIndex"]) + 1;
            BargainQuery query = new BargainQuery {
                Type = str,
                PageIndex = num
            };
            int total = BargainHelper.GetTotal(query);
            dt = (DataTable) BargainHelper.GetBargainList(query).Data;
            string liHtml = this.GetLiHtml(dt);
            string s = string.Concat(new object[] { "{\"success\":\"true\",\"rowtotal\":\"", dt.Rows.Count, "\",\"total\":\"", total, "\",\"lihtml\":\"", liHtml, "\"}" });
            context.Response.Write(s);
            context.Response.End();
        }

        private decimal GetBargainPrice(int bargainId, int bargainDetialId)
        {
            decimal bargainTypeMinVlue = 0M;
            if (bargainId > 0)
            {
                BargainInfo bargainInfo = BargainHelper.GetBargainInfo(bargainId);
                BargainDetialInfo bargainDetialInfo = BargainHelper.GetBargainDetialInfo(bargainDetialId);
                if (bargainInfo != null)
                {
                    if (bargainInfo.BargainType == 0)
                    {
                        bargainTypeMinVlue = (decimal) bargainInfo.BargainTypeMinVlue;
                    }
                    else
                    {
                        int maxValue = (int) (bargainInfo.BargainTypeMaxVlue * 100f);
                        int minValue = (int) (bargainInfo.BargainTypeMinVlue * 100f);
                        float num4 = new Random().Next(minValue, maxValue);
                        bargainTypeMinVlue = ((decimal) num4) / 100M;
                    }
                }
                if ((bargainDetialInfo.Price - bargainTypeMinVlue) < bargainInfo.FloorPrice)
                {
                    bargainTypeMinVlue = bargainDetialInfo.Price - bargainInfo.FloorPrice;
                }
            }
            return bargainTypeMinVlue;
        }

        private void GetCashBack(HttpContext context)
        {
            int num = 1;
            string s = context.Request.QueryString["pageIndex"];
            if (!int.TryParse(s, out num))
            {
                num = 1;
            }
            int num2 = 10;
            string str2 = context.Request.QueryString["pageSize"];
            if (!int.TryParse(str2, out num2))
            {
                num2 = 10;
            }
            CashBackQuery query = new CashBackQuery {
                PageSize = num2,
                PageIndex = num,
                UserId = new int?(MemberProcessor.GetCurrentMember().UserId)
            };
            DbQueryResult cashBackByPager = CashBackHelper.GetCashBackByPager(query);
            string str3 = EasyUI.GridData(cashBackByPager.Data, (long) cashBackByPager.TotalRecords);
            context.Response.Write(str3);
            context.Response.End();
        }

        private void GetCashBackDetail(HttpContext context)
        {
            int num = 1;
            string s = context.Request.QueryString["pageIndex"];
            if (!int.TryParse(s, out num))
            {
                num = 1;
            }
            int num2 = 10;
            string str2 = context.Request.QueryString["pageSize"];
            if (!int.TryParse(str2, out num2))
            {
                num2 = 10;
            }
            string str3 = context.Request.QueryString["Id"];
            int num3 = 0;
            int.TryParse(str3, out num3);
            CashBackDetailsQuery query = new CashBackDetailsQuery {
                PageSize = num2,
                PageIndex = num,
                CashBackId = num3,
                UserId = new int?(MemberProcessor.GetCurrentMember().UserId)
            };
            DbQueryResult cashBackDetailsByPager = CashBackHelper.GetCashBackDetailsByPager(query);
            string str4 = EasyUI.GridData(cashBackDetailsByPager.Data, (long) cashBackDetailsByPager.TotalRecords);
            context.Response.Write(str4);
            context.Response.End();
        }

        private void GetCustomDistributorStatistic(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string str = context.Request["orderId"];
            int id = 0;
            if (!string.IsNullOrEmpty(str))
            {
                id = int.Parse(str);
            }
            DataTable customDistributorStatistic = VShopHelper.GetCustomDistributorStatistic(id);
            string s = "";
            if ((customDistributorStatistic != null) && (customDistributorStatistic.Rows.Count > 0))
            {
                s = "{\"success\":\"true\",\"logo\":\"" + customDistributorStatistic.Rows[0]["logo"].ToString() + "\",\"storename\":\"" + customDistributorStatistic.Rows[0]["storename"].ToString() + "\",\"ordernums\":\"" + customDistributorStatistic.Rows[0]["ordernums"].ToString() + "\",\"commtotalsum\":\"" + customDistributorStatistic.Rows[0]["commtotalsum"].ToString() + "\"}";
            }
            context.Response.Write(s);
            context.Response.End();
        }

        private void GetDistributorInfo(HttpContext context)
        {
            DistributorsInfo distributorInfo = new DistributorsInfo();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            int currentDistributorId = Globals.GetCurrentDistributorId();
            if ((currentDistributorId > 0) && masterSettings.IsShowDistributorSelfStoreName)
            {
                distributorInfo = DistributorsBrower.GetDistributorInfo(currentDistributorId);
                if (distributorInfo != null)
                {
                    distributorInfo.Logo = Globals.GetWebUrlStart() + distributorInfo.Logo;
                }
            }
            else
            {
                distributorInfo.StoreName = masterSettings.SiteName;
                distributorInfo.Logo = Globals.GetWebUrlStart() + masterSettings.DistributorLogoPic;
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(distributorInfo));
            context.Response.End();
        }

        public void GetDrawRemarks(HttpContext context)
        {
            int result = 0;
            if (int.TryParse(context.Request["SerialID"], out result))
            {
                BalanceDrawRequestInfo balanceDrawRequestById = DistributorsBrower.GetBalanceDrawRequestById(result.ToString());
                if (balanceDrawRequestById != null)
                {
                    context.Response.Write(balanceDrawRequestById.Remark);
                }
                else
                {
                    context.Response.Write("N");
                }
            }
            else
            {
                context.Response.Write("N");
            }
        }

        private string GetLiHtml(DataTable dt)
        {
            StringBuilder builder = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    builder.Append("<li>");
                    builder.Append("<div class='shopimg'>");
                    builder.Append("<img src='" + (string.IsNullOrEmpty(dt.Rows[i]["ThumbnailUrl60"].ToString()) ? "/utility/pics/none.gif" : dt.Rows[i]["ThumbnailUrl60"].ToString()) + "'  style='width:600px;height:200px'>");
                    builder.Append("<p class='mask'>");
                    builder.Append("<span class='fl'>" + dt.Rows[i]["ProductName"].ToString() + "</span>");
                    builder.Append("<span class='fr'>原价：￥" + (string.IsNullOrEmpty(dt.Rows[i]["SalePrice"].ToString()) ? "0.00" : string.Format("{0:f2}", dt.Rows[i]["SalePrice"].ToString())) + "</span>");
                    builder.Append("</p>");
                    builder.Append("</div>");
                    builder.Append("<div class='bargain-info'>");
                    builder.Append("<p>砍至底价：<strong class='colorr'>￥" + (string.IsNullOrEmpty(dt.Rows[i]["FloorPrice"].ToString()) ? "0.00" : string.Format("{0:F2}", dt.Rows[i]["FloorPrice"].ToString())) + "</strong></p>");
                    builder.Append("<p>结束时间：" + DateTime.Parse(dt.Rows[i]["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
                    builder.Append(BargainHelper.GetLinkHtml(dt.Rows[i]["id"].ToString(), dt.Rows[i]["bargainstatus"].ToString(), "0", "0"));
                    builder.Append("</div>");
                    builder.Append("</li>");
                }
            }
            return builder.ToString();
        }

        public string getModelType(int m)
        {
            switch (m)
            {
                case 1:
                    return "快递";

                case 2:
                    return "EMS";

                case 3:
                    return "顺丰";

                case 4:
                    return "平邮";
            }
            return "包邮";
        }

        private void GetMyDistributors(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int num = int.Parse(context.Request["GradeId"]);
            int num2 = int.Parse(context.Request["PageIndex"]) + 1;
            int num3 = int.Parse(context.Request["ReferralId"]);
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId(false), true);
            int num4 = 10;
            DistributorsQuery query = new DistributorsQuery {
                GradeId = num,
                PageIndex = num2,
                UserId = currentDistributors.UserId,
                ReferralPath = num3.ToString(),
                PageSize = num4
            };
            int total = 0;
            string str = context.Request["sort"];
            if (string.IsNullOrWhiteSpace(str))
            {
                str = "CreateTime";
            }
            string str2 = context.Request["order"];
            if (string.IsNullOrWhiteSpace(str2))
            {
                str2 = "desc";
            }
            DataTable dt = DistributorsBrower.GetDownDistributors(query, out total, str, str2);
            string myDistributorsHtml = this.GetMyDistributorsHtml(dt);
            string s = string.Empty;
            if (dt.Rows.Count > 0)
            {
                s = string.Concat(new object[] { "{\"success\":\"true\",\"rowtotal\":\"", dt.Rows.Count, "\",\"total\":\"", total, "\",\"lihtml\":\"", myDistributorsHtml, "\"}" });
            }
            else
            {
                s = "{\"success\":\"false\"}";
            }
            context.Response.Write(s);
            context.Response.End();
        }

        public string GetMyDistributorsHtml(DataTable dt)
        {
            StringBuilder builder = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    builder.Append("<li>");
                    builder.Append(" <h3> " + dt.Rows[i]["StoreName"].ToString() + "【" + dt.Rows[i]["GradeName"].ToString() + "】</h3>");
                    builder.Append("<div class='userinfobox'>");
                    builder.Append("<div class='userimg'>");
                    builder.Append("<img src='" + (string.IsNullOrEmpty(dt.Rows[i]["Logo"].ToString()) ? "/templates/common/images/user.png" : dt.Rows[i]["Logo"].ToString()) + "'>");
                    builder.Append("</div>");
                    builder.Append("<div class='usertextinfo clearfix'>");
                    builder.Append("<div class='left'>");
                    builder.Append("<p><span class='colorc'>用户呢称：</span>" + dt.Rows[i]["UserName"].ToString() + "</p>");
                    builder.Append("<p><span class='colorc'>申请时间：</span>" + (string.IsNullOrEmpty(dt.Rows[i]["CreateTime"].ToString()) ? "" : DateTime.Parse(dt.Rows[i]["CreateTime"].ToString()).ToString("yyyy-MM-dd")) + "</p>");
                    builder.Append("<p><a href='ChirldrenDistributorDetials.aspx?distributorId=" + dt.Rows[i]["UserId"].ToString() + "'><span class='colorc'>销售总额：</span><span class='colorg'>￥" + (string.IsNullOrEmpty(dt.Rows[i]["OrderTotal"].ToString()) ? "0.00" : string.Format("{0:F2}", dt.Rows[i]["OrderTotal"])) + "</span></a></p>");
                    builder.Append("</div>");
                    builder.Append("<div class='right'>");
                    builder.Append("<p><a href='ChirldrenDistributorStores.aspx?UserId=" + dt.Rows[i]["UserId"].ToString() + "&gradeId=" + dt.Rows[i]["GradeId"].ToString() + "'><span class='colorc'>下级分店：</span><span class='colorg'>" + dt.Rows[i]["disTotal"].ToString() + " 家</span></a></p>");
                    builder.Append("<p><span class='colorc'>下级会员：</span>" + dt.Rows[i]["MemberTotal"].ToString() + " 位</p>");
                    builder.Append("<p><a href='ChirldrenDistributorDetials.aspx?distributorId=" + dt.Rows[i]["UserId"].ToString() + "'><span class='colorc'>贡献佣金：</span><span class='colorg'>￥" + (string.IsNullOrEmpty(dt.Rows[i]["CommTotal"].ToString()) ? "0.00" : string.Format("{0:F2}", dt.Rows[i]["CommTotal"])) + "</span></a></p>");
                    builder.Append("</div>");
                    builder.Append("</div>");
                    builder.Append("</div>");
                    builder.Append("<span class='left radius'></span>");
                    builder.Append("<span class='right radius'></span>");
                    builder.Append("</li>");
                }
            }
            return builder.ToString();
        }

        private void GetMyMember(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int referralUserId = int.Parse(context.Request["UserId"]);
            int pageIndex = int.Parse(context.Request["PageIndex"]) + 1;
            int pageSize = 10;
            int total = 0;
            string str = context.Request["sort"];
            if (string.IsNullOrWhiteSpace(str))
            {
                str = "createDate";
            }
            string str2 = context.Request["order"];
            if (string.IsNullOrWhiteSpace(str2))
            {
                str2 = "desc";
            }
            DataTable dt = MemberProcessor.GetMembersByUserId(referralUserId, pageIndex, pageSize, out total, str, str2);
            string myMemberHtml = this.GetMyMemberHtml(dt);
            string s = string.Empty;
            if (dt.Rows.Count > 0)
            {
                s = string.Concat(new object[] { "{\"success\":\"true\",\"rowtotal\":\"", dt.Rows.Count, "\",\"total\":\"", total, "\",\"lihtml\":\"", myMemberHtml, "\"}" });
            }
            else
            {
                s = "{\"success\":\"false\"}";
            }
            context.Response.Write(s);
            context.Response.End();
        }

        private string GetMyMemberHtml(DataTable dt)
        {
            StringBuilder builder = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    builder.Append("<li>");
                    builder.Append(" <h3> " + dt.Rows[i]["username"].ToString() + "【" + dt.Rows[i]["GradeName"].ToString() + "】</h3>");
                    builder.Append("<div class='userinfobox'>");
                    builder.Append("<div class='userimg'>");
                    builder.Append("<img src='" + (string.IsNullOrEmpty(dt.Rows[i]["UserHead"].ToString()) ? "/templates/common/images/user.png" : dt.Rows[i]["UserHead"].ToString()) + "'>");
                    builder.Append("</div>");
                    builder.Append("<div class='usertextinfo clearfix'>");
                    builder.Append("<div class='left'>");
                    builder.Append("<p><span class='colorc'>注册时间：</span>" + (string.IsNullOrEmpty(dt.Rows[i]["CreateDate"].ToString()) ? "" : DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("yyyy-MM-dd")) + "</p>");
                    builder.Append("<p><span class='colorc'>订单数量：</span><span class='colorg'>" + (string.IsNullOrEmpty(dt.Rows[i]["OrderMumber"].ToString()) ? "0" : dt.Rows[i]["OrderMumber"].ToString()) + "</span> 单</p>");
                    builder.Append("</div>");
                    builder.Append("<div class='right'>");
                    builder.Append("<p><span class='colorc'>最近下单：</span>" + (string.IsNullOrEmpty(dt.Rows[i]["OrderDate"].ToString()) ? "" : DateTime.Parse(dt.Rows[i]["OrderDate"].ToString()).ToString("yyyy-MM-dd")) + "</p>");
                    builder.Append("<p><span class='colorc'>消费金额：</span><span class='colorg'>￥" + (string.IsNullOrEmpty(dt.Rows[i]["OrdersTotal"].ToString()) ? "0" : string.Format("{0:F2}", dt.Rows[i]["OrdersTotal"])) + "</span></p>");
                    builder.Append("</div>");
                    builder.Append("</div>");
                    builder.Append("</div>");
                    builder.Append("<span class='left radius'></span>");
                    builder.Append("<span class='right radius'></span>");
                    builder.Append("</li>");
                }
            }
            return builder.ToString();
        }

        private void GetNotice(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{\"success\":\"false\"}";
            int num = Globals.RequestFormNum("type");
            int num2 = Globals.RequestFormNum("readtype");
            int num3 = Globals.RequestFormNum("page");
            int num4 = Globals.RequestFormNum("pagesize");
            if (num4 < 5)
            {
                num4 = 10;
            }
            if (num3 < 1)
            {
                num3 = 1;
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                int userId = currentMember.UserId;
                DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(userId);
                NoticeQuery entity = new NoticeQuery {
                    SortBy = "ID",
                    SortOrder = SortAction.Desc
                };
                Globals.EntityCoding(entity, true);
                entity.PageIndex = num3;
                entity.PageSize = num4;
                entity.UserId = new int?(userId);
                entity.SendType = num;
                entity.IsDistributor = new bool?(distributorInfo != null);
                entity.IsPub = 1;
                entity.IsDel = 0;
                if (num2 == 1)
                {
                    entity.IsNotShowRead = 1;
                }
                DbQueryResult noticeRequest = NoticeBrowser.GetNoticeRequest(entity);
                object data = noticeRequest.Data;
                if (data != null)
                {
                    DataTable table = (DataTable) data;
                    StringBuilder builder = new StringBuilder();
                    int count = table.Rows.Count;
                    if (count > 0)
                    {
                        int num7 = 0;
                        builder.Append("{\"id\":" + table.Rows[num7]["ID"].ToString() + ",\"isread\":" + (NoticeBrowser.IsView(currentMember.UserId, Globals.ToNum(table.Rows[num7]["ID"])) ? "1" : "0") + ",\"title\":\"" + Globals.String2Json(table.Rows[num7]["title"].ToString()) + "\",\"pubtime\":\"" + DateTime.Parse(table.Rows[num7]["PubTime"].ToString()).ToString("yyyy-MM-dd") + "\"}");
                        for (num7 = 1; num7 < count; num7++)
                        {
                            builder.Append(",{\"id\":" + table.Rows[num7]["ID"].ToString() + ",\"isread\":" + (NoticeBrowser.IsView(currentMember.UserId, Globals.ToNum(table.Rows[num7]["ID"])) ? "1" : "0") + ",\"title\":\"" + Globals.String2Json(table.Rows[num7]["title"].ToString()) + "\",\"pubtime\":\"" + DateTime.Parse(table.Rows[num7]["PubTime"].ToString()).ToString("yyyy-MM-dd") + "\"}");
                        }
                    }
                    s = string.Concat(new object[] { "{\"success\":\"true\",\"rowtotal\":\"", table.Rows.Count, "\",\"total\":\"", noticeRequest.TotalRecords, "\",\"lihtml\":[", builder.ToString(), "]}" });
                }
            }
            context.Response.Write(s);
            context.Response.End();
        }

        private void GetOrderItemStatus(HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            string orderId = Globals.RequestFormStr("orderid");
            string skuid = Globals.RequestFormStr("skuid");
            int orderitemid = Globals.RequestFormNum("orderitemid");
            RefundInfo info = RefundHelper.GetByOrderIdAndProductID(orderId, 0, skuid, orderitemid);
            if (info != null)
            {
                builder.Append(string.Concat(new object[] { "\"Status\":1,\"HandleStatus\":", (int) info.HandleStatus, ",\"Reason\":\"", Globals.String2Json(info.AdminRemark), "\"" }));
            }
            else
            {
                builder.Append("\"Status\":0,\"Tips\":\"未找到项目\"");
            }
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }

        public void GetOrderRedPager(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int num = 0;
            int.TryParse(context.Request["id"], out num);
            int num2 = 0;
            int.TryParse(context.Request["userid"], out num2);
            int currentMemberUserId = Globals.GetCurrentMemberUserId(false);
            if (currentMemberUserId <= 0)
            {
                context.Response.Write("{\"status\":\"-1\",\"tips\":\"用户未登录！\"}");
            }
            else
            {
                ShareActivityInfo act = ShareActHelper.GetAct(num);
                if (act == null)
                {
                    context.Response.Write("{\"status\":\"-2\",\"tips\":\"活动不存在！\"}");
                }
                else if (act.BeginDate > DateTime.Now)
                {
                    context.Response.Write("{\"status\":\"-2\",\"tips\":\"活动未开始！\"}");
                }
                else if (act.EndDate < DateTime.Now)
                {
                    context.Response.Write("{\"status\":\"-2\",\"tips\":\"活动已结束！\"}");
                }
                else if (ShareActHelper.GeTAttendCount(num, num2) > act.CouponNumber)
                {
                    context.Response.Write("{\"status\":\"-3\",\"tips\":\"您来晚了，领取机会已用完！\"}");
                }
                else if (ShareActHelper.HasAttend(num, currentMemberUserId))
                {
                    context.Response.Write("{\"status\":\"-5\",\"tips\":\"" + currentMemberUserId.ToString() + "\"}");
                }
                else
                {
                    int couponId = act.CouponId;
                    if (CouponHelper.GetCoupon(couponId) == null)
                    {
                        context.Response.Write("{\"status\":\"-2\",\"tips\":\"优惠券不存在！\"}");
                    }
                    else
                    {
                        SendCouponResult result = CouponHelper.IsCanSendCouponToMember(couponId, currentMemberUserId);
                        if (result.GetHashCode() == 1)
                        {
                            context.Response.Write("{\"status\":\"-2\",\"tips\":\"优惠券已结束！\"}");
                        }
                        else if (result.GetHashCode() == 2)
                        {
                            context.Response.Write("{\"status\":\"-2\",\"tips\":\"会员不在此活动范围内！\"}");
                        }
                        else if (result.GetHashCode() == 3)
                        {
                            context.Response.Write("{\"status\":\"-3\",\"tips\":\"优惠券已领完！\"}");
                        }
                        else if (result.GetHashCode() == 4)
                        {
                            context.Response.Write("{\"status\":\"-2\",\"tips\":\"已到领取上限！\"}");
                        }
                        else if (result.GetHashCode() == 5)
                        {
                            context.Response.Write("{\"status\":\"-4\",\"tips\":\"领取优惠券失败！\"}");
                        }
                        else
                        {
                            ShareActivityRecordInfo record = new ShareActivityRecordInfo {
                                shareId = num,
                                shareUser = num2,
                                attendUser = currentMemberUserId
                            };
                            if (ShareActHelper.AddRecord(record))
                            {
                                if (CouponHelper.SendCouponToMember(couponId, currentMemberUserId).GetHashCode() == 0)
                                {
                                    context.Response.Write("{\"status\":\"0\",\"tips\":\"" + currentMemberUserId.ToString() + "\"}");
                                }
                                else
                                {
                                    context.Response.Write("{\"status\":\"-4\",\"tips\":\"领取优惠券失败！\"}");
                                }
                            }
                            else
                            {
                                context.Response.Write("{\"status\":\"-4\",\"tips\":\"领取优惠券失败！\"}");
                            }
                        }
                    }
                }
            }
        }

        public void GetQRCodeScanInfo(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            string openID = "";
            string nickName = "";
            string headImageUrl = "";
            string text1 = context.Request["AppID"];
            string str4 = context.Request["Ticket"];
            bool flag = false;
            if (!string.IsNullOrEmpty(str4) && WeiXinHelper.BindAdminOpenId.ContainsKey(str4.Trim()))
            {
                openID = WeiXinHelper.BindAdminOpenId[str4.Trim()];
                flag = true;
                WeiXinHelper.BindAdminOpenId.Remove(str4.Trim());
            }
            string str5 = "";
            string retInfo = "";
            if (flag)
            {
                try
                {
                    str5 = "#1";
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    str5 = "#1OK";
                    str5 = str5 + "WeixinAppId=" + masterSettings.WeixinAppId + "--WeixinAppSecret=" + masterSettings.WeixinAppSecret;
                    string tOKEN = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
                    str5 = str5 + "  accessToken=" + tOKEN;
                    str5 = str5 + "#2OK";
                    BarCodeApi.GetHeadImageUrlByOpenID(tOKEN, openID, out retInfo, out nickName, out headImageUrl);
                }
                catch (Exception exception)
                {
                    retInfo = str5 + "从腾讯服务器获取头像信息出错：" + exception.Message;
                }
            }
            else
            {
                retInfo = "无扫描用户。";
            }
            builder.Append("\"Status\":\"" + (flag ? "1" : "-1") + "\",\"OpenID\":\"" + openID + "\",\"NickName\":\"" + nickName + "\",\"UserHead\":\"" + headImageUrl + "\",\"RetInfo\":\"" + retInfo + "\"");
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        private void GetSecondDistributors(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int num = int.Parse(context.Request["GradeId"]);
            int num2 = int.Parse(context.Request["PageIndex"]) + 1;
            int num3 = int.Parse(context.Request["ReferralId"]);
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId(false), true);
            int num4 = 10;
            DistributorsQuery query = new DistributorsQuery {
                GradeId = num,
                PageIndex = num2,
                UserId = currentDistributors.UserId,
                ReferralPath = num3.ToString(),
                PageSize = num4
            };
            int total = 0;
            string str = context.Request["sort"];
            if (string.IsNullOrWhiteSpace(str))
            {
                str = "CreateTime";
            }
            string str2 = context.Request["order"];
            if (string.IsNullOrWhiteSpace(str2))
            {
                str2 = "desc";
            }
            DataTable dt = DistributorsBrower.GetDownDistributors(query, out total, str, str2);
            string secondDistributorsHtml = this.GetSecondDistributorsHtml(dt);
            string s = string.Empty;
            if (dt.Rows.Count > 0)
            {
                s = string.Concat(new object[] { "{\"success\":\"true\",\"rowtotal\":\"", dt.Rows.Count, "\",\"total\":\"", total, "\",\"lihtml\":\"", secondDistributorsHtml, "\"}" });
            }
            else
            {
                s = "{\"success\":\"false\"}";
            }
            context.Response.Write(s);
            context.Response.End();
        }

        public string GetSecondDistributorsHtml(DataTable dt)
        {
            StringBuilder builder = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    builder.Append("<li>");
                    builder.Append(" <h3> " + Globals.String2Json(dt.Rows[i]["StoreName"].ToString()) + "【" + Globals.String2Json(dt.Rows[i]["GradeName"].ToString()) + "】</h3>");
                    builder.Append("<div class='userinfobox'>");
                    builder.Append("<div class='userimg'>");
                    builder.Append("<img src='" + (string.IsNullOrEmpty(dt.Rows[i]["Logo"].ToString()) ? "/templates/common/images/user.png" : dt.Rows[i]["Logo"].ToString()) + "'>");
                    builder.Append("</div>");
                    builder.Append("<div class='usertextinfo clearfix'>");
                    builder.Append("<div class='left'>");
                    builder.Append("<p><span class='colorc'>用户呢称：</span>" + Globals.String2Json(dt.Rows[i]["UserName"].ToString()) + "</p>");
                    builder.Append("<p><span class='colorc'>申请时间：</span>" + (string.IsNullOrEmpty(dt.Rows[i]["CreateTime"].ToString()) ? "" : DateTime.Parse(dt.Rows[i]["CreateTime"].ToString()).ToString("yyyy-MM-dd")) + "</p>");
                    builder.Append("<p><a href='ChirldrenDistributorDetials.aspx?distributorId=" + dt.Rows[i]["UserId"].ToString() + "'><span class='colorc'>销售总额：</span><span class='colorg'>￥" + (string.IsNullOrEmpty(dt.Rows[i]["OrderTotal"].ToString()) ? "0.00" : string.Format("{0:F2}", dt.Rows[i]["OrderTotal"])) + "</span></a></p>");
                    builder.Append("</div>");
                    builder.Append("<div class='right'>");
                    builder.Append("<p><span class='colorc'>上级店铺：</span><span >" + dt.Rows[i]["LStoreName"].ToString() + "</span></p>");
                    builder.Append("<p><span class='colorc'>下级会员：</span>" + dt.Rows[i]["MemberTotal"].ToString() + " 位</p>");
                    builder.Append("<p><a href='ChirldrenDistributorDetials.aspx?distributorId=" + dt.Rows[i]["UserId"].ToString() + "'><span class='colorc'>贡献佣金：</span><span class='colorg'>￥" + (string.IsNullOrEmpty(dt.Rows[i]["CommTotal"].ToString()) ? "0.00" : string.Format("{0:F2}", dt.Rows[i]["CommTotal"])) + "</span></a></p>");
                    builder.Append("</div>");
                    builder.Append("</div>");
                    builder.Append("</div>");
                    builder.Append("<span class='left radius'></span>");
                    builder.Append("<span class='right radius'></span>");
                    builder.Append("</li>");
                }
            }
            return builder.ToString();
        }

        public void GetShippingTypes(HttpContext context)
        {
            ShoppingCartInfo shoppingCart = null;
            StringBuilder builder = new StringBuilder();
            if ((int.TryParse(context.Request["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(context.Request["from"])) && (context.Request["from"] == "signBuy"))
            {
                this.productSku = context.Request["productSku"];
                shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
            }
            else
            {
                int result = 0;
                if (!string.IsNullOrEmpty(context.Request["TemplateId"]) && int.TryParse(context.Request["TemplateId"], out result))
                {
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart(result);
                }
            }
            StringBuilder builder2 = new StringBuilder();
            context.Response.ContentType = "application/json";
            if (shoppingCart != null)
            {
                string regionId = context.Request["city"];
                string str2 = "";
                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
                {
                    if (info2.FreightTemplateId > 0)
                    {
                        str2 = str2 + info2.FreightTemplateId + ",";
                    }
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    DataTable specifyRegionGroupsModeId = SettingsHelper.GetSpecifyRegionGroupsModeId(str2.Substring(0, str2.Length - 1), regionId);
                    if (specifyRegionGroupsModeId.Rows.Count > 0)
                    {
                        for (int i = 0; i < specifyRegionGroupsModeId.Rows.Count; i++)
                        {
                            string str3 = this.getModelType(int.Parse(specifyRegionGroupsModeId.Rows[i]["ModeId"].ToString()));
                            builder2.Append(string.Concat(new object[] { ",{\"modelId\":\"", specifyRegionGroupsModeId.Rows[i]["ModeId"], "\",\"text\":\"", str3, "\"}" }));
                        }
                    }
                    else
                    {
                        builder2.Append(",{\"modelId\":\"0\",\"text\":\"包邮\"}");
                    }
                    builder.Append(builder2.ToString() ?? "");
                }
                else
                {
                    builder2.Append(",{\"modelId\":\"0\",\"text\":\"包邮\"}");
                }
                if (builder2.Length > 0)
                {
                    builder2.Remove(0, 1);
                }
                builder2.Insert(0, "{\"data\":[").Append("]}");
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(builder2.ToString());
        }

        private void GetSiteSettings(HttpContext context)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            ForceFollowInfo info = new ForceFollowInfo();
            if (masterSettings != null)
            {
                info.EnableGuidePageSet = masterSettings.EnableGuidePageSet;
                info.IsAutoGuide = masterSettings.IsAutoGuide;
                info.IsMustConcern = masterSettings.IsMustConcern;
                info.GuideConcernType = masterSettings.GuideConcernType;
                info.GuidePageSet = masterSettings.GuidePageSet;
                info.ConcernMsg = masterSettings.ConcernMsg;
                if (masterSettings.IsMustConcern)
                {
                    info.IsAutoGuide = true;
                }
            }
            int num = this.CheckFoucs();
            info.FollowInfo = num;
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(info));
            context.Response.End();
        }

        private void GetStatisticalData(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            BargainStatisticalData bargainStatisticalDataInfo = BargainHelper.GetBargainStatisticalDataInfo(Convert.ToInt32(context.Request["BargainId"]));
            string s = "";
            if (bargainStatisticalDataInfo != null)
            {
                int num2 = bargainStatisticalDataInfo.ActivityStock - bargainStatisticalDataInfo.ActivitySales;
                string str2 = "0";
                if (bargainStatisticalDataInfo.ActivitySales > 0)
                {
                    str2 = (bargainStatisticalDataInfo.AverageTransactionPrice / bargainStatisticalDataInfo.ActivitySales).ToString("f2");
                }
                s = string.Concat(new object[] { "{\"success\":1, \"NumberOfParticipants\":\"", bargainStatisticalDataInfo.NumberOfParticipants, "\", \"SingleMember\":\"", bargainStatisticalDataInfo.SingleMember, "\", \"ActivitySales\":\"", bargainStatisticalDataInfo.ActivitySales, "\", \"SurplusInventory\":\"", num2, "\", \"AverageTransactionPrice\":\"", str2, "\", \"ActiveState\":\"", bargainStatisticalDataInfo.ActiveState, "\"}" });
            }
            context.Response.Write(s);
            context.Response.End();
        }

        private void GetWinXinInfo(HttpContext context)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            context.Response.ContentType = "application/json";
            string log = string.Empty;
            if ((currentMember == null) || string.IsNullOrEmpty(currentMember.OpenId))
            {
                log = "{\"success\":\"false\",\"message\":\"非微信注册会员，获取失败！\"}";
            }
            else
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                string tOKEN = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
                if (tOKEN.Contains("errmsg") && tOKEN.Contains("errcode"))
                {
                    log = "{\"success\":\"false\",\"message\":\"获取微信令牌失败！\"}";
                }
                else
                {
                    log = BarCodeApi.GetUserInfosByOpenID(tOKEN, currentMember.OpenId);
                    Globals.Debuglog(log, "_DebuglogGetPic.txt");
                }
            }
            context.Response.Write(log);
            context.Response.End();
        }

        private void HelpBargain(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "";
            string userAgent = context.Request.UserAgent;
            if (!userAgent.ToLower().Contains("micromessenger") && !userAgent.ToLower().Contains("alipay"))
            {
                context.Response.Write("{\"success\":3, \"msg\":\"砍价只能在微信端商城或支付宝服务窗进行\"}");
            }
            else
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                if (currentMember == null)
                {
                    context.Response.Write("{\"success\":0, \"msg\":\"请先登录\"}");
                }
                else
                {
                    int bargainId = Convert.ToInt32(context.Request["BargainId"]);
                    int bargainDetialId = Convert.ToInt32(context.Request["BargainDetialId"]);
                    HelpBargainDetialInfo helpBargainDetial = new HelpBargainDetialInfo {
                        BargainDetialId = bargainDetialId,
                        BargainId = bargainId,
                        UserId = currentMember.UserId,
                        CreateDate = DateTime.Now
                    };
                    if (BargainHelper.ActionIsEnd(bargainDetialId))
                    {
                        context.Response.Write("{\"success\":6, \"msg\":\"商品已经下单了，谢谢您的参与\"}");
                    }
                    else
                    {
                        decimal bargainPrice = this.GetBargainPrice(bargainId, bargainDetialId);
                        if (bargainPrice == 0M)
                        {
                            context.Response.Write("{\"success\":4, \"msg\":\"商品已经砍到底价了，谢谢您的参与\"}");
                        }
                        else
                        {
                            helpBargainDetial.BargainPrice = bargainPrice;
                            if (BargainHelper.ExistsHelpBargainDetial(helpBargainDetial))
                            {
                                context.Response.Write("{\"success\":1, \"msg\":\"你已经帮忙砍过了\"}");
                            }
                            else
                            {
                                string str3 = BargainHelper.InsertHelpBargainDetial(helpBargainDetial);
                                if (str3 == "1")
                                {
                                    s = "{\"success\":2, \"msg\":\"您帮忙砍了" + helpBargainDetial.BargainPrice + "元.\"}";
                                }
                                else
                                {
                                    context.Response.Write("{\"success\":5, \"msg\":\"" + str3 + "\"}");
                                    return;
                                }
                                context.Response.Write(s);
                                context.Response.End();
                            }
                        }
                    }
                }
            }
        }

        private void HelpBargainDetial(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int id = Convert.ToInt32(context.Request["BargainDetialId"]);
            int num2 = Convert.ToInt32(context.Request["BargainId"]);
            string s = "";
            BargainDetialInfo bargainDetialInfo = BargainHelper.GetBargainDetialInfo(id);
            if (bargainDetialInfo != null)
            {
                string str2 = "";
                BargainInfo bargainInfo = BargainHelper.GetBargainInfo(num2);
                str2 = ProductHelper.GetProductBaseInfo(bargainInfo.ProductId).SaleStatus.ToString();
                decimal floorPrice = bargainInfo.FloorPrice;
                decimal num4 = (bargainInfo.InitialPrice - bargainDetialInfo.Price) / (bargainInfo.InitialPrice - floorPrice);
                int number = bargainDetialInfo.Number;
                string sku = bargainDetialInfo.Sku;
                string str4 = this.LoadHelpBargainDetial(bargainDetialInfo.Id);
                if (BargainHelper.ActionIsEnd(bargainDetialInfo.Id))
                {
                    str2 = "order";
                }
                s = string.Concat(new object[] { 
                    "{\"success\":1,\"SaleStatus\":\"", str2, "\",\"ProductId\":\"", bargainInfo.ProductId, "\", \"Price\":\"", bargainDetialInfo.Price.ToString("f2"), "\", \"progress\":\"", (int) (num4 * 100M), "\", \"BargainDetialId\":\"", bargainDetialInfo.Id, "\", \"Number\":\"", number, "\",\"Sku\":\"", sku, "\", \"BargainDetialHtml\":\"", str4, 
                    "\"}"
                 });
            }
            else
            {
                BargainInfo info4 = BargainHelper.GetBargainInfo(num2);
                ProductInfo productBaseInfo = null;
                if (info4 != null)
                {
                    productBaseInfo = ProductHelper.GetProductBaseInfo(info4.ProductId);
                }
                s = "{\"success\":0, \"Price\":\"0\", \"SaleStatus\":\"" + productBaseInfo.SaleStatus + "\"}";
            }
            context.Response.Write(s);
            context.Response.End();
        }

        public bool IsFreeTemplateShipping(string RegionId, int FreightTemplateId, int ModeId, ShoppingCartItemInfo info)
        {
            bool flag = false;
            DataTable table = SettingsHelper.GetFreeTemplateShipping(RegionId, FreightTemplateId, ModeId);
            if (table.Rows.Count > 0)
            {
                string str2 = table.Rows[0]["ConditionType"].ToString();
                if (str2 == null)
                {
                    goto Label_0160;
                }
                if (!(str2 == "1"))
                {
                    if (str2 == "2")
                    {
                        if (info.SumSubTotal >= decimal.Parse(table.Rows[0]["ConditionNumber"].ToString()))
                        {
                            flag = true;
                        }
                        return flag;
                    }
                    if (str2 == "3")
                    {
                        if ((info.Quantity >= int.Parse(table.Rows[0]["ConditionNumber"].ToString().Split(new char[] { '$' })[0])) && (info.SumSubTotal >= decimal.Parse(table.Rows[0]["ConditionNumber"].ToString().Split(new char[] { '$' })[1])))
                        {
                            flag = true;
                        }
                        return flag;
                    }
                    goto Label_0160;
                }
                if (info.Quantity >= int.Parse(table.Rows[0]["ConditionNumber"].ToString()))
                {
                    flag = true;
                }
            }
            return flag;
        Label_0160:
            return false;
        }

        private string LoadHelpBargainDetial(int bargainDetialId)
        {
            DataTable helpBargainDetials = BargainHelper.GetHelpBargainDetials(bargainDetialId);
            int helpBargainDetialCount = BargainHelper.GetHelpBargainDetialCount(bargainDetialId);
            StringBuilder builder = new StringBuilder();
            builder.Append("<h6>已有" + helpBargainDetialCount + "位好友帮忙砍价</h6>");
            builder.Append("<ul>");
            if (helpBargainDetials.Rows.Count > 0)
            {
                for (int i = 0; i < helpBargainDetials.Rows.Count; i++)
                {
                    if (i == 2)
                    {
                        break;
                    }
                    builder.Append("<li class='clearfix'>");
                    builder.Append("<p class='fl'><span class='colorl'>" + helpBargainDetials.Rows[i]["UserName"].ToString() + "</span>, 价格砍掉￥" + (string.IsNullOrEmpty(helpBargainDetials.Rows[i]["BargainPrice"].ToString()) ? "0.00" : string.Format("{0:F2}", helpBargainDetials.Rows[i]["BargainPrice"])) + "</p>");
                    builder.Append("<p class='fr colorc'>" + DateTime.Parse(helpBargainDetials.Rows[i]["CreateDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
                    builder.Append("</li>");
                }
                builder.Append("</ul>");
                builder.Append("<p class='look-all'>");
                if (helpBargainDetials.Rows.Count > 2)
                {
                    builder.Append("<span class='fr colorl' onclick='LoadMore()'>显示更多&#62;&#62;</span>");
                }
                builder.Append("</p>");
            }
            return builder.ToString();
        }

        private void LoadHelpBargainDetial(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int bargainDetialId = Convert.ToInt32(context.Request["BargainDetialId"]);
            Convert.ToInt32(context.Request["Size"]);
            DataTable helpBargainDetials = BargainHelper.GetHelpBargainDetials(bargainDetialId);
            BargainHelper.GetHelpBargainDetialCount(bargainDetialId);
            string s = "";
            StringBuilder builder = new StringBuilder();
            builder.Append("<h6>已有" + helpBargainDetials.Rows.Count + "位好友帮忙砍价</h6>");
            builder.Append("<ul>");
            if (helpBargainDetials.Rows.Count > 0)
            {
                for (int i = 0; i < helpBargainDetials.Rows.Count; i++)
                {
                    builder.Append("<li class='clearfix'>");
                    builder.Append("<p class='fl'><span class='colorl'>" + helpBargainDetials.Rows[i]["UserName"].ToString() + "</span>价格砍掉￥" + (string.IsNullOrEmpty(helpBargainDetials.Rows[i]["BargainPrice"].ToString()) ? "0.00" : string.Format("{0:f2}", helpBargainDetials.Rows[i]["BargainPrice"].ToString())) + "</p>");
                    builder.Append("<p class='fr colorc'>" + DateTime.Parse(helpBargainDetials.Rows[i]["CreateDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
                    builder.Append("</li>");
                }
                builder.Append("</ul>");
                if (helpBargainDetials.Rows.Count > 2)
                {
                    builder.Append("<p class='look-all'><span class='fr colorl'>显示更多&#62;&#62;</span></p>");
                }
                s = "{\"success\":1, \"msg\":\"" + builder.ToString() + "\"}";
            }
            else
            {
                s = "{\"success\":0, \"msg\":\"暂无数据\"}";
            }
            context.Response.Write(s);
            context.Response.End();
        }

        private void LoadMoreHelpBargainDetial(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            DataTable helpBargainDetials = BargainHelper.GetHelpBargainDetials(Convert.ToInt32(context.Request["BargainDetialId"]));
            string s = "";
            StringBuilder builder = new StringBuilder();
            builder.Append("<h6>已有" + helpBargainDetials.Rows.Count + "位好友帮忙砍价</h6>");
            builder.Append("<ul>");
            if (helpBargainDetials.Rows.Count > 0)
            {
                for (int i = 0; i < helpBargainDetials.Rows.Count; i++)
                {
                    builder.Append("<li class='clearfix'>");
                    builder.Append("<p class='fl'><span class='colorl'>" + helpBargainDetials.Rows[i]["UserName"].ToString() + "</span>, 价格砍掉￥" + (string.IsNullOrEmpty(helpBargainDetials.Rows[i]["BargainPrice"].ToString()) ? "0.00" : string.Format("{0:F2}", helpBargainDetials.Rows[i]["BargainPrice"])) + "</p>");
                    builder.Append("<p class='fr colorc'>" + DateTime.Parse(helpBargainDetials.Rows[i]["CreateDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
                    builder.Append("</li>");
                }
                builder.Append("</ul>");
                builder.Append("<p class='look-all'><span class='fr colorl'></span></p>");
                s = "{\"success\":1, \"msg\":\"" + builder.ToString() + "\"}";
            }
            else
            {
                s = "{\"success\":0, \"msg\":\"暂无数据\"}";
            }
            context.Response.Write(s);
            context.Response.End();
        }

        public string MemberAutoToDistributor(MemberInfo memberinfo)
        {
            string str = string.Empty;
            if (!SystemAuthorizationHelper.CheckDistributorIsCanAuthorization())
            {
                return "平台分销商数已达上限";
            }
            str = DistributorsBrower.MemberAutoToDistributor(memberinfo);
            if (str == "1")
            {
                if (HttpContext.Current.Request.Cookies["Vshop-Member"] != null)
                {
                    string name = "Vshop-ReferralId";
                    HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(-1.0);
                    HttpCookie cookie = new HttpCookie(name) {
                        Value = memberinfo.UserId.ToString(),
                        Expires = DateTime.Now.AddYears(10)
                    };
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                this.myNotifier.updateAction = UpdateAction.MemberUpdate;
                this.myNotifier.actionDesc = "满足条件自动成为分销商";
                this.myNotifier.RecDateUpdate = DateTime.Today;
                this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                this.myNotifier.UpdateDB();
                return str;
            }
            Globals.Debuglog("自动生成分销商失败，原因是：" + str, "_DebuglogMemberAutoToDistributor.txt");
            return str;
        }

        private void OnekeySet(HttpContext context)
        {
            WxJsonResult result = WxTemplateSendHelp.QuickSetWeixinTemplates();
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(result));
            context.Response.End();
        }

        private void OpenBargain(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":0, \"msg\":\"请先登录\"}");
            }
            else
            {
                int id = Convert.ToInt32(context.Request["BargainId"]);
                int num2 = Convert.ToInt32(context.Request["number"]);
                string str = context.Request["sku"];
                string s = "";
                int bargainDetialId = 0;
                BargainInfo bargainInfo = BargainHelper.GetBargainInfo(id);
                if (bargainInfo != null)
                {
                    string str3 = BargainHelper.IsCanBuyByBarginId(id);
                    if (str3 == "1")
                    {
                        BargainDetialInfo bargainDetial = new BargainDetialInfo {
                            BargainId = id,
                            UserId = currentMember.UserId,
                            Number = num2,
                            Sku = str,
                            Price = bargainInfo.InitialPrice,
                            CreateDate = DateTime.Now
                        };
                        bool flag = BargainHelper.InsertBargainDetial(bargainDetial, out bargainDetialId);
                        HelpBargainDetialInfo helpBargainDetial = new HelpBargainDetialInfo {
                            BargainDetialId = bargainDetialId,
                            BargainId = id,
                            UserId = currentMember.UserId,
                            CreateDate = DateTime.Now,
                            BargainPrice = this.GetBargainPrice(id, bargainDetialId)
                        };
                        if (flag)
                        {
                            if (BargainHelper.InsertHelpBargainDetial(helpBargainDetial) == "1")
                            {
                                s = string.Concat(new object[] { "{\"success\":\"1\",\"bargainDetialId\":\"", bargainDetialId, "\",\"msg\":\"发起成功,自己砍掉", helpBargainDetial.BargainPrice.ToString("f2"), "元,请邀请好友砍价.\"}" });
                            }
                            else
                            {
                                s = "{\"success\":\"2\",\"msg\":\"添加失败\"}";
                            }
                        }
                        else
                        {
                            s = "{\"success\":\"2\",\"msg\":\"添加失败\"}";
                        }
                    }
                    else
                    {
                        s = "{\"success\":\"2\",\"msg\":\"发起砍价失败，" + str3 + "！\"}";
                    }
                }
                context.Response.Write(s);
                context.Response.End();
            }
        }

        private void OperateAllDistributorProducts(HttpContext context)
        {
            string str = context.Request["deleteAll"];
            if (string.IsNullOrEmpty(str))
            {
                str = "false";
            }
            if (str == "true")
            {
                this.AutoDeleteDistributorProducts();
            }
            else
            {
                this.AutoAddDistributorProducts();
            }
            context.Response.Write("{\"success\":\"true\",\"msg\":\"保存成功\"}");
            context.Response.End();
        }

        public string ordersummit(ShoppingCartInfo cart, HttpContext context, string remark, int shippingId, string couponCode, string selectCouponValue, string shippingTypeinfo, bool summittype, string OrderMarking, IList<ShoppingCartItemInfo> ItemInfo, int PointExchange, int bargainDetialId, out string ActivitiesIds)
        {
            ActivitiesIds = "";
            int couponId = 0;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            StringBuilder builder = new StringBuilder();
            OrderInfo order = ShoppingProcessor.ConvertShoppingCartToOrder(cart, false, false);
            if (order == null)
            {
                builder.Append("\"Status\":\"None\"");
                goto Label_110E;
            }
            order.OrderId = this.GenerateOrderId();
            order.OrderDate = DateTime.Now;
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            order.UserId = currentMember.UserId;
            order.Username = currentMember.UserName;
            order.EmailAddress = currentMember.Email;
            order.RealName = currentMember.RealName;
            order.QQ = currentMember.QQ;
            order.Remark = remark;
            string activitiesId = "";
            string activitiesName = "";
            string vItemList = string.Empty;
            if (bargainDetialId > 0)
            {
                order.BargainDetialId = bargainDetialId;
                order.DiscountAmount = 0M;
            }
            else
            {
                order.DiscountAmount = this.DiscountMoney(ItemInfo, out activitiesId, out activitiesName, currentMember, out couponId, out vItemList);
            }
            ActivitiesIds = activitiesId;
            if ((couponId > 0) && (CouponHelper.IsCanSendCouponToMember(couponId, currentMember.UserId) != SendCouponResult.正常领取))
            {
                couponId = 0;
            }
            order.OrderMarking = OrderMarking;
            order.ActivitiesId = activitiesId.Trim(new char[] { ',' });
            order.ActivitiesName = activitiesName.Trim(new char[] { ',' });
            order.OrderStatus = OrderStatus.WaitBuyerPay;
            order.RefundStatus = RefundStatus.None;
            order.ShipToDate = context.Request["shiptoDate"];
            bool flag = context.Request["useMembersPoint"].ToString() == "1";
            bool isUseBalance = context.Request["useBalance"].ToString() == "1";
            if (!masterSettings.EnableBalancePayment)
            {
                isUseBalance = false;
            }
            int currentDistributorId = Globals.GetCurrentDistributorId();
            if (currentDistributorId > 0)
            {
                DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(currentDistributorId);
                if (distributorInfo == null)
                {
                    currentDistributorId = 0;
                }
                else
                {
                    switch ((masterSettings.IsDistributorBuyCanGetCommission ? 0 : 1))
                    {
                        case 0:
                            order.ReferralPath = distributorInfo.ReferralPath;
                            break;

                        case 1:
                        {
                            if (currentDistributorId != order.UserId)
                            {
                                order.ReferralPath = distributorInfo.ReferralPath;
                                break;
                            }
                            MemberInfo member = MemberHelper.GetMember(currentDistributorId);
                            if (member == null)
                            {
                                currentDistributorId = 0;
                                break;
                            }
                            currentDistributorId = member.ReferralUserId;
                            DistributorsInfo info5 = DistributorsBrower.GetDistributorInfo(currentDistributorId);
                            if (info5 != null)
                            {
                                order.ReferralPath = info5.ReferralPath;
                            }
                            break;
                        }
                    }
                }
            }
            order.ReferralUserId = currentDistributorId;
            int result = 0;
            int num5 = 0;
            ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(shippingId);
            if (shippingAddress != null)
            {
                order.ShippingRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, "，");
                order.RegionId = shippingAddress.RegionId;
                order.Address = shippingAddress.Address;
                order.ZipCode = shippingAddress.Zipcode;
                order.ShipTo = shippingAddress.ShipTo;
                order.TelPhone = shippingAddress.TelPhone;
                order.CellPhone = shippingAddress.CellPhone;
                MemberProcessor.SetDefaultShippingAddress(shippingId, MemberProcessor.GetCurrentMember().UserId);
            }
            if (int.TryParse(shippingTypeinfo, out result))
            {
                order.ShippingModeId = result;
                order.ModeName = this.getModelType(result);
                order.AdjustedFreight = 0M;
                if (result > 0)
                {
                    DataView defaultView = new DataView();
                    if (cart != null)
                    {
                        defaultView = SettingsHelper.GetAllFreightItems().DefaultView;
                    }
                    float num6 = 0f;
                    if (defaultView.Count > 0)
                    {
                        Dictionary<int, ShoppingCartItemInfo> dictionary = new Dictionary<int, ShoppingCartItemInfo>();
                        foreach (ShoppingCartItemInfo info7 in cart.LineItems)
                        {
                            if (!dictionary.ContainsKey(info7.FreightTemplateId))
                            {
                                info7.SumSubTotal = info7.SubTotal;
                                info7.CubicMeter *= info7.Quantity;
                                info7.FreightWeight *= info7.Quantity;
                                dictionary.Add(info7.FreightTemplateId, info7);
                            }
                            else
                            {
                                ShoppingCartItemInfo local1 = dictionary[info7.FreightTemplateId];
                                local1.SumSubTotal += info7.SubTotal;
                                ShoppingCartItemInfo local2 = dictionary[info7.FreightTemplateId];
                                local2.FreightWeight += info7.FreightWeight * info7.Quantity;
                                ShoppingCartItemInfo local3 = dictionary[info7.FreightTemplateId];
                                local3.CubicMeter += info7.CubicMeter * info7.Quantity;
                                ShoppingCartItemInfo local4 = dictionary[info7.FreightTemplateId];
                                local4.Quantity += info7.Quantity;
                            }
                        }
                        cart.LineItems.Clear();
                        foreach (KeyValuePair<int, ShoppingCartItemInfo> pair in dictionary)
                        {
                            cart.LineItems.Add(pair.Value);
                        }
                        foreach (ShoppingCartItemInfo info8 in cart.LineItems)
                        {
                            string str7;
                            if (!info8.IsfreeShipping)
                            {
                                bool flag3 = false;
                                FreightTemplate templateMessage = SettingsHelper.GetTemplateMessage(info8.FreightTemplateId);
                                if (((templateMessage != null) && (info8.FreightTemplateId > 0)) && !templateMessage.FreeShip)
                                {
                                    if (templateMessage.HasFree)
                                    {
                                        flag3 = this.IsFreeTemplateShipping(context.Request["Shippingcity"], info8.FreightTemplateId, result, info8);
                                    }
                                    if (!flag3)
                                    {
                                        defaultView.RowFilter = string.Concat(new object[] { " RegionId=", context.Request["Shippingcity"], " and ModeId=", result, " and TemplateId=", info8.FreightTemplateId, " and IsDefault=0" });
                                        if (defaultView.Count != 1)
                                        {
                                            goto Label_084C;
                                        }
                                        string str6 = defaultView[0]["MUnit"].ToString();
                                        if (str6 != null)
                                        {
                                            if (!(str6 == "1"))
                                            {
                                                if (str6 == "2")
                                                {
                                                    goto Label_06F4;
                                                }
                                                if (str6 == "3")
                                                {
                                                    goto Label_07A0;
                                                }
                                            }
                                            else
                                            {
                                                num6 += this.setferight(float.Parse(info8.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                                            }
                                        }
                                    }
                                }
                            }
                            continue;
                        Label_06F4:
                            if (info8.FreightWeight > 0M)
                            {
                                num6 += this.setferight(float.Parse(info8.FreightWeight.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                            }
                            continue;
                        Label_07A0:
                            if (info8.CubicMeter > 0M)
                            {
                                num6 += this.setferight(float.Parse(info8.CubicMeter.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                            }
                            continue;
                        Label_084C:;
                            defaultView.RowFilter = string.Concat(new object[] { "  ModeId=", result, " and TemplateId=", info8.FreightTemplateId, " and  IsDefault=1" });
                            if ((defaultView.Count == 1) && ((str7 = defaultView[0]["MUnit"].ToString()) != null))
                            {
                                if (!(str7 == "1"))
                                {
                                    if (str7 == "2")
                                    {
                                        goto Label_098E;
                                    }
                                    if (str7 == "3")
                                    {
                                        goto Label_0A3A;
                                    }
                                }
                                else
                                {
                                    num6 += this.setferight(float.Parse(info8.Quantity.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                                }
                            }
                            continue;
                        Label_098E:
                            if (info8.FreightWeight > 0M)
                            {
                                num6 += this.setferight(float.Parse(info8.FreightWeight.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                            }
                            continue;
                        Label_0A3A:
                            if (info8.CubicMeter > 0M)
                            {
                                num6 += this.setferight(float.Parse(info8.CubicMeter.ToString()), float.Parse(defaultView[0]["FristNumber"].ToString()), float.Parse(defaultView[0]["FristPrice"].ToString()), float.Parse(defaultView[0]["AddNumber"].ToString()), float.Parse(defaultView[0]["AddPrice"].ToString()));
                            }
                        }
                    }
                    string s = num6.ToString("F2");
                    order.AdjustedFreight = decimal.Parse(s);
                }
            }
            if (int.TryParse(context.Request["paymentType"], out num5))
            {
                order.PaymentTypeId = num5;
                switch (num5)
                {
                    case 0:
                    case -1:
                        order.PaymentType = "货到付款";
                        order.Gateway = "hishop.plugins.payment.podrequest";
                        goto Label_0BD6;

                    case 0x58:
                        order.PaymentType = "微信支付";
                        order.Gateway = "hishop.plugins.payment.weixinrequest";
                        goto Label_0BD6;

                    case 0x63:
                        order.PaymentType = "线下付款";
                        order.Gateway = "hishop.plugins.payment.offlinerequest";
                        goto Label_0BD6;
                }
                PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(num5);
                if (paymentMode != null)
                {
                    order.PaymentTypeId = paymentMode.ModeId;
                    order.PaymentType = paymentMode.Name;
                    order.Gateway = paymentMode.Gateway;
                }
            }
        Label_0BD6:
            if (!string.IsNullOrEmpty(couponCode) && (bargainDetialId == 0))
            {
                CouponInfo info10 = ShoppingProcessor.UseCoupon(cart.GetTotal(), couponCode);
                order.CouponName = info10.CouponName;
                if (info10.ConditionValue > 0M)
                {
                    order.CouponAmount = info10.ConditionValue;
                }
                order.CouponCode = couponCode;
                order.CouponValue = info10.CouponValue;
            }
            if ((!string.IsNullOrEmpty(selectCouponValue) && (selectCouponValue != "0")) && (bargainDetialId == 0))
            {
                order.RedPagerActivityName = selectCouponValue.Split(new char[] { '|' })[0];
                order.RedPagerID = new int?(int.Parse(selectCouponValue.Split(new char[] { '|' })[1]));
                order.RedPagerOrderAmountCanUse = decimal.Parse(selectCouponValue.Split(new char[] { '|' })[2]);
                order.RedPagerAmount = decimal.Parse(selectCouponValue.Split(new char[] { '|' })[3]);
                if (CouponHelper.CheckCouponsIsUsed(order.RedPagerID.Value))
                {
                    builder.Append("\"Status\":\"Error\"");
                    builder.AppendFormat(",\"ErrorMsg\":\"优惠券已被使用，请重新提交订单！\"", new object[0]);
                    return builder.ToString();
                }
            }
            else
            {
                selectCouponValue = "";
            }
            order.PointToCash = 0M;
            order.PointExchange = 0;
            decimal amount = order.GetAmount();
            decimal num8 = 0M;
            if (order.RedPagerAmount < 0M)
            {
                builder.Append("\"Status\":\"Error\"");
                builder.AppendFormat(",\"ErrorMsg\":\"优惠券金额不正确，请重新提交订单！\"", new object[0]);
                return builder.ToString();
            }
            order.CouponFreightMoneyTotal = 0M;
            if ((amount + order.AdjustedFreight) <= order.RedPagerAmount)
            {
                order.RedPagerAmount = amount + order.AdjustedFreight;
                order.CouponFreightMoneyTotal = order.AdjustedFreight;
                num8 = 0M;
            }
            else if (amount <= order.RedPagerAmount)
            {
                order.CouponFreightMoneyTotal = order.RedPagerAmount - amount;
                num8 = 0M;
            }
            else
            {
                num8 = amount - order.RedPagerAmount;
            }
            if ((masterSettings.PonitToCash_Enable && (bargainDetialId == 0)) && flag)
            {
                int pointToCashRate = masterSettings.PointToCashRate;
                decimal num10 = masterSettings.PonitToCash_MaxAmount;
                int num12 = currentMember.Points - order.GetTotalPointNumber();
                int num13 = (int) (num10 * pointToCashRate);
                if (num12 > num13)
                {
                    num12 = num13;
                }
                decimal num14 = num8;
                if (num14 < 0M)
                {
                    num14 = 0M;
                }
                int num15 = (int) (pointToCashRate * num14);
                if (num15 > num12)
                {
                    num15 = num12;
                }
                order.PointExchange = num15;
                order.PointToCash = Math.Round((decimal) (num15 / pointToCashRate), 2);
            }
            else
            {
                PointExchange = 0;
            }
            try
            {
                decimal remainingMondy = 0M;
                this.SetOrderItemStatus(order, order.RedPagerAmount - order.CouponFreightMoneyTotal, order.PointToCash, order.DiscountAmount, vItemList, out remainingMondy);
                BargainInfo bargainInfoByDetialId = BargainHelper.GetBargainInfoByDetialId(order.BargainDetialId);
                if ((bargainInfoByDetialId == null) || ((bargainInfoByDetialId != null) && bargainInfoByDetialId.IsCommission))
                {
                    if (order.BargainDetialId > 0)
                    {
                        BargainDetialInfo bargainDetialInfo = BargainHelper.GetBargainDetialInfo(order.BargainDetialId);
                        if (bargainDetialInfo != null)
                        {
                            foreach (LineItemInfo info13 in order.LineItems.Values)
                            {
                                info13.ItemAdjustedPrice = bargainDetialInfo.Price;
                                info13.ItemListPrice = bargainDetialInfo.Price;
                                info13.CommissionDiscount = bargainInfoByDetialId.CommissionDiscount;
                            }
                        }
                    }
                    order = ShoppingProcessor.GetCalculadtionCommission(order);
                }
                else
                {
                    order.ThirdCommission = 0M;
                    order.SecondCommission = 0M;
                    order.FirstCommission = 0M;
                }
                order.logisticsTools = LogisticsTools.Kuaidi100;
                if (!string.IsNullOrEmpty(masterSettings.Exp_appKey) && !string.IsNullOrEmpty(masterSettings.Exp_appSecret))
                {
                    order.logisticsTools = LogisticsTools.Kuaidiniao;
                }
                int num17 = ShoppingProcessor.CreatOrder(order, isUseBalance, remainingMondy);
                if (num17 > 0)
                {
                    MemberHelper.SetOrderDate(order.UserId, 0);
                    if (num17 == 2)
                    {
                        order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
                        MemberHelper.SetOrderDate(order.UserId, 1);
                    }
                    if (summittype)
                    {
                        ShoppingCartProcessor.ClearShoppingCart();
                    }
                    try
                    {
                        OrderInfo info14 = order;
                        if (info14 != null)
                        {
                            Messenger.SendWeiXinMsg_OrderCreate(info14);
                        }
                    }
                    catch (Exception exception)
                    {
                        string message = exception.Message;
                    }
                    builder.Append("\"Status\":\"OK\",\"OrderMarkingStatus\":\"" + order.OrderMarking + "\",");
                    builder.AppendFormat("\"OrderId\":\"{0}\"", order.OrderMarking);
                }
                else
                {
                    builder.Append("\"Status\":\"Error\"");
                    builder.AppendFormat(",\"ErrorMsg\":\"提交订单失败！\"", new object[0]);
                }
            }
            catch (OrderException exception2)
            {
                builder.Append("\"Status\":\"Error\"");
                builder.AppendFormat(",\"ErrorMsg\":\"{0}\"", exception2.Message);
            }
        Label_110E:
            return builder.ToString();
        }

        private void ProcessAddToCartByExchange(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int number = int.Parse(context.Request["quantity"], NumberStyles.None);
            string skuId = context.Request["productSkuId"];
            int categoryid = int.Parse(context.Request["categoryid"], NumberStyles.None);
            int templateid = int.Parse(context.Request["Templateid"], NumberStyles.None);
            int productId = int.Parse(context.Request["ProductId"], NumberStyles.None);
            int result = 0;
            int.TryParse(context.Request["type"], out result);
            int num6 = 0;
            int.TryParse(context.Request["exchangeId"], out num6);
            if (MemberProcessor.GetCurrentMember() == null)
            {
                context.Response.Write("{\"Status\":\"2\"}");
            }
            else if (!this.ExistsProduct(productId, int.Parse(context.Request["exchangeId"]), number))
            {
                context.Response.Write("{\"Status\":\"10\"}");
            }
            else
            {
                ShoppingCartProcessor.AddLineItem(skuId, number, categoryid, templateid, result, num6, 0);
                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                context.Response.Write("{\"Status\":\"OK\",\"TotalMoney\":\"" + shoppingCart.GetTotal().ToString(".00") + "\",\"Quantity\":\"" + shoppingCart.GetQuantity().ToString() + "\"}");
            }
        }

        private void ProcessAddToCartBySkus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int quantity = int.Parse(context.Request["quantity"], NumberStyles.None);
            string skuId = context.Request["productSkuId"];
            int categoryid = int.Parse(context.Request["categoryid"], NumberStyles.None);
            int templateid = int.Parse(context.Request["Templateid"], NumberStyles.None);
            int limitedTimeDiscountId = 0;
            int result = 0;
            int.TryParse(context.Request["type"], out result);
            int num6 = 0;
            int.TryParse(context.Request["exchangeId"], out num6);
            if (MemberProcessor.GetCurrentMember() == null)
            {
                context.Response.Write("{\"Status\":\"2\"}");
            }
            else
            {
                ShoppingCartProcessor.AddLineItem(skuId, quantity, categoryid, templateid, result, num6, limitedTimeDiscountId);
                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                context.Response.Write("{\"Status\":\"OK\",\"TotalMoney\":\"" + shoppingCart.GetTotal().ToString(".00") + "\",\"Quantity\":\"" + shoppingCart.GetQuantity().ToString() + "\"}");
            }
        }

        private void ProcessChageQuantity(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuId"];
            int result = 1;
            int.TryParse(context.Request["quantity"], out result);
            int num2 = 0;
            int.TryParse(context.Request["type"], out num2);
            int num3 = 0;
            int.TryParse(context.Request["exchangeId"], out num3);
            int id = Globals.RequestFormNum("limitedTimeDiscountId");
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            int num5 = ShoppingCartProcessor.GetSkuStock(skuId, num2, num3);
            if (result > num5)
            {
                builder.AppendFormat("\"Status\":\"{0}\"", num5);
                result = num5;
            }
            else
            {
                bool flag = true;
                if (id > 0)
                {
                    LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(id);
                    if (discountInfo != null)
                    {
                        int limitNumber = discountInfo.LimitNumber;
                        if (limitNumber > 0)
                        {
                            int num7 = ShoppingCartProcessor.GetLimitedTimeDiscountUsedNum(id, skuId, 0, -1, true);
                            if ((result + num7) > limitNumber)
                            {
                                int num8 = limitNumber - num7;
                                num5 = (num8 > 0) ? num8 : 0;
                                int num9 = ShoppingCartProcessor.GetLimitedTimeDiscountUsedNum(id, skuId, 0, -1, false);
                                if ((num5 == 0) && (result > (limitNumber - num9)))
                                {
                                    flag = false;
                                    num5 = limitNumber - num9;
                                }
                            }
                        }
                    }
                }
                if (!flag)
                {
                    builder.AppendFormat("\"Status\":\"{0}\"", num5);
                }
                else
                {
                    builder.Append("\"Status\":\"OK\",");
                    ShoppingCartProcessor.UpdateLineItemQuantity(skuId, (result > 0) ? result : 1, num2, id);
                    builder.AppendFormat("\"TotalPrice\":\"{0}\"", ShoppingCartProcessor.GetShoppingCart().GetAmount());
                }
            }
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }

        private void ProcessDeleteCartProduct(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuId"];
            int result = 0;
            int.TryParse(context.Request["type"], out result);
            int limitedTimeDiscountId = Globals.RequestFormNum("limitedTimeDiscountId");
            StringBuilder builder = new StringBuilder();
            ShoppingCartProcessor.RemoveLineItem(skuId, result, limitedTimeDiscountId);
            builder.Append("{");
            builder.Append("\"Status\":\"OK\"");
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }

        private void ProcessGetSkuByOptions(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = int.Parse(context.Request["productId"], NumberStyles.None);
            string str = context.Request["options"];
            if (string.IsNullOrEmpty(str))
            {
                context.Response.Write("{\"Status\":\"0\"}");
            }
            else
            {
                if (str.EndsWith(","))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                SKUItem item = ShoppingProcessor.GetProductAndSku(MemberProcessor.GetCurrentMember(), productId, str);
                if (item == null)
                {
                    context.Response.Write("{\"Status\":\"1\"}");
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("{");
                    builder.Append("\"Status\":\"OK\",");
                    builder.AppendFormat("\"SkuId\":\"{0}\",", item.SkuId);
                    builder.AppendFormat("\"SKU\":\"{0}\",", item.SKU);
                    builder.AppendFormat("\"Weight\":\"{0}\",", item.Weight);
                    builder.AppendFormat("\"Stock\":\"{0}\",", item.Stock);
                    builder.AppendFormat("\"SalePrice\":\"{0}\"", item.SalePrice.ToString("F2"));
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request["action"])
            {
                case "GetCashBack":
                    this.GetCashBack(context);
                    return;

                case "GetCashBackDetail":
                    this.GetCashBackDetail(context);
                    return;

                case "onekeySet":
                    this.OnekeySet(context);
                    return;

                case "GetOrderItemStatus":
                    this.GetOrderItemStatus(context);
                    return;

                case "AddParticipant":
                    this.AddParticipant(context);
                    return;

                case "GetWinXinInfo":
                    this.GetWinXinInfo(context);
                    return;

                case "followCheck":
                    this.followCheck(context);
                    return;

                case "OperateAllDistributorProducts":
                    this.OperateAllDistributorProducts(context);
                    return;

                case "GetDrawStatus":
                    this.GetDrawRemarks(context);
                    return;

                case "CheckCoupon":
                    this.CheckCoupon(context);
                    return;

                case "SignToday":
                    this.SignToday(context);
                    return;

                case "ConfirmPrizeArriver":
                    this.ConfirmPrizeArriver(context);
                    return;

                case "ConfirmOneyuangPrizeAddr":
                    this.ConfirmOneyuangPrizeAddr(context);
                    return;

                case "ConfirmPrizeAddr":
                    this.ConfirmPrizeAddr(context);
                    return;

                case "AddToCartBySkus":
                    this.ProcessAddToCartBySkus(context);
                    return;

                case "GetSkuByOptions":
                    this.ProcessGetSkuByOptions(context);
                    return;

                case "DeleteCartProduct":
                    this.ProcessDeleteCartProduct(context);
                    return;

                case "ChageQuantity":
                    this.ProcessChageQuantity(context);
                    return;

                case "SubmitMemberCard":
                    this.ProcessSubmitMemberCard(context);
                    return;

                case "AddShippingAddress":
                    this.AddShippingAddress(context);
                    return;

                case "DelShippingAddress":
                    this.DelShippingAddress(context);
                    return;

                case "SetDefaultShippingAddress":
                    this.SetDefaultShippingAddress(context);
                    return;

                case "UpdateShippingAddress":
                    this.UpdateShippingAddress(context);
                    return;

                case "Vote":
                    this.Vote(context);
                    return;

                case "SetUserName":
                    this.SetUserName(context);
                    return;

                case "Submmitorder":
                    this.ProcessSubmmitorder(context);
                    return;

                case "CancelOrder":
                    this.CancelOrder(context);
                    return;

                case "FinishOrder":
                    this.FinishOrder(context);
                    return;

                case "RequestReturn":
                    this.RequestReturn(context);
                    return;

                case "AddProductConsultations":
                    this.AddProductConsultations(context);
                    return;

                case "AddProductReview":
                    this.AddProductReview(context);
                    return;

                case "AddFavorite":
                    this.AddFavorite(context);
                    return;

                case "DelFavorite":
                    this.DelFavorite(context);
                    return;

                case "CheckFavorite":
                    this.CheckFavorite(context);
                    return;

                case "ProcessAddToCartByExchange":
                    this.ProcessAddToCartByExchange(context);
                    return;

                case "Logistic":
                    this.SearchExpressData(context);
                    return;

                case "GetShippingTypes":
                    this.GetShippingTypes(context);
                    return;

                case "UserLogin":
                    this.UserLogin(context);
                    return;

                case "RegisterUser":
                    this.RegisterUser(context);
                    return;

                case "BindUserName":
                    this.BindUserName(context);
                    return;

                case "BindOldUserName":
                    this.BindOldUserName(context);
                    return;

                case "AddDistributor":
                    this.AddDistributor(context);
                    return;

                case "SetDistributorMsg":
                    this.SetDistributorMsg(context);
                    return;

                case "DeleteProducts":
                    this.DeleteDistributorProducts(context);
                    return;

                case "AddDistributorProducts":
                    this.AddDistributorProducts(context);
                    return;

                case "UpdateDistributor":
                    this.UpdateDistributor(context);
                    return;

                case "AddCommissions":
                    this.AddCommissions(context);
                    return;

                case "AdjustCommissions":
                    this.AdjustCommissions(context);
                    return;

                case "EditPassword":
                    this.EditPassword(context);
                    return;

                case "GetOrderRedPager":
                    this.GetOrderRedPager(context);
                    return;

                case "countfreight":
                    this.countfreight(context);
                    return;

                case "checkdistribution":
                    this.checkdistribution(context);
                    return;

                case "countfreighttype":
                    this.countfreighttype(context);
                    return;

                case "getqrcodescaninfo":
                    this.GetQRCodeScanInfo(context);
                    return;

                case "getalifuwuqrcodescaninfo":
                    this.GetAliFuWuQRCodeScanInfo(context);
                    return;

                case "clearqrcodescaninfo":
                    this.ClearQRCodeScanInfo(context);
                    return;

                case "CombineOrders":
                    this.CombineOrders(context);
                    return;

                case "AddCustomDistributorStatistic":
                    this.AddCustomDistributorStatistic(context);
                    return;

                case "GetCustomDistributorStatistic":
                    this.GetCustomDistributorStatistic(context);
                    return;

                case "GetMyMember":
                    this.GetMyMember(context);
                    return;

                case "GetNotice":
                    this.GetNotice(context);
                    return;

                case "GetMyDistributors":
                    this.GetMyDistributors(context);
                    return;

                case "GetSecondDistributors":
                    this.GetSecondDistributors(context);
                    return;

                case "GetBargainCount":
                    this.GetBargainCount(context);
                    return;

                case "GetBargainList":
                    this.GetBargainList(context);
                    return;

                case "OpenBargain":
                    this.OpenBargain(context);
                    return;

                case "HelpBargain":
                    this.HelpBargain(context);
                    return;

                case "LoadHelpBargainDetial":
                    this.LoadHelpBargainDetial(context);
                    return;

                case "LoadMoreHelpBargainDetial":
                    this.LoadMoreHelpBargainDetial(context);
                    return;

                case "ExistsBargainDetial":
                    this.ExistsBargainDetial(context);
                    return;

                case "GetBargain":
                    this.GetBargain(context);
                    return;

                case "HelpBargainDetial":
                    this.HelpBargainDetial(context);
                    return;

                case "ExistsHelpBargainDetial":
                    this.ExistsHelpBargainDetial(context);
                    return;

                case "GetStatisticalData":
                    this.GetStatisticalData(context);
                    return;

                case "UpdateBargainEndDate":
                    this.UpdateBargainEndDate(context);
                    return;

                case "GetSiteSettings":
                    this.GetSiteSettings(context);
                    return;

                case "GetDistributorInfo":
                    this.GetDistributorInfo(context);
                    return;

                case "SetUserPointByAdmin":
                    this.ProcessUserPointByAdmin(context);
                    return;
            }
        }

        private void ProcessSubmitMemberCard(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                currentMember.Address = context.Request.Form.Get("address");
                currentMember.RealName = context.Request.Form.Get("name");
                currentMember.CellPhone = context.Request.Form.Get("phone");
                currentMember.QQ = context.Request.Form.Get("qq");
                if (!string.IsNullOrEmpty(currentMember.QQ))
                {
                    currentMember.Email = currentMember.QQ + "@qq.com";
                }
                currentMember.VipCardNumber = SettingsManager.GetMasterSettings(true).VipCardPrefix + currentMember.UserId.ToString();
                currentMember.VipCardDate = new DateTime?(DateTime.Now);
                string s = MemberProcessor.UpdateMember(currentMember) ? "{\"success\":true}" : "{\"success\":false}";
                context.Response.Write(s);
            }
        }

        private void ProcessSubmmitorder(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (currentMember == null)
            {
                builder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"请先登录！\"");
                builder.Append("}");
                context.Response.ContentType = "application/json";
                context.Response.Write(builder.ToString());
            }
            else
            {
                int num4;
                int num5;
                int shippingId = 0;
                string couponCode = context.Request["couponCode"];
                int bargainDetialId = Globals.RequestFormNum("bargainDetialId");
                string[] strArray = context.Request["selectCouponValue"].Split(new char[] { ',' });
                if (bargainDetialId > 0)
                {
                    if (OrderHelper.ExistsOrderByBargainDetialId(currentMember.UserId, bargainDetialId))
                    {
                        builder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"您已经参加该活动，不能重复下单！\"");
                        builder.Append("}");
                        context.Response.ContentType = "application/json";
                        context.Response.Write(builder.ToString());
                        return;
                    }
                    string str2 = BargainHelper.IsCanBuyByBarginDetailId(bargainDetialId);
                    if (str2 != "1")
                    {
                        builder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"" + str2 + ",不能提交订单！\"");
                        builder.Append("}");
                        context.Response.ContentType = "application/json";
                        context.Response.Write(builder.ToString());
                        return;
                    }
                }
                string str3 = "";
                string activitiesIds = "";
                shippingId = int.Parse(context.Request["shippingId"]);
                int.TryParse(context.Request["groupbuyId"], out num4);
                int relNumber = 0;
                string remark = context.Request["remark"];
                string orderMarking = this.GenerateOrderId();
                if (((int.TryParse(context.Request["buyAmount"], out num5) && !string.IsNullOrEmpty(context.Request["productSku"])) && !string.IsNullOrEmpty(context.Request["from"])) && ((context.Request["from"] == "signBuy") || (context.Request["from"] == "groupBuy")))
                {
                    string productSkuId = context.Request["productSku"];
                    if (context.Request["from"] == "signBuy")
                    {
                        List<ShoppingCartInfo> list = null;
                        if (bargainDetialId > 0)
                        {
                            if (BargainHelper.UpdateNumberById(bargainDetialId, num5, out relNumber))
                            {
                                list = ShoppingCartProcessor.GetListShoppingCart(productSkuId, relNumber, bargainDetialId, 0);
                            }
                        }
                        else
                        {
                            int id = Globals.RequestFormNum("limitedTimeDiscountId");
                            int buyAmount = num5;
                            if (id > 0)
                            {
                                bool flag3 = true;
                                LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(id);
                                if (discountInfo == null)
                                {
                                    flag3 = false;
                                }
                                if (flag3)
                                {
                                    int num9 = ShoppingCartProcessor.GetLimitedTimeDiscountUsedNum(id, productSkuId, 0, currentMember.UserId, false);
                                    if ((discountInfo.LimitNumber > 0) && (num5 > (discountInfo.LimitNumber - num9)))
                                    {
                                        if (MemberHelper.CheckCurrentMemberIsInRange(discountInfo.ApplyMembers, discountInfo.DefualtGroup, discountInfo.CustomGroup, currentMember.UserId))
                                        {
                                            buyAmount = discountInfo.LimitNumber - num9;
                                        }
                                        else
                                        {
                                            buyAmount = 0;
                                        }
                                    }
                                }
                                if ((buyAmount <= 0) || !flag3)
                                {
                                    id = 0;
                                    buyAmount = num5;
                                }
                            }
                            list = ShoppingCartProcessor.GetListShoppingCart(productSkuId, buyAmount, 0, id);
                        }
                        if (list == null)
                        {
                            builder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"您选择的商品库存不足！\"");
                        }
                        else
                        {
                            foreach (ShoppingCartInfo info3 in list)
                            {
                                string str8 = this.ordersummit(info3, context, remark, shippingId, couponCode, strArray[0], context.Request["shippingType"], false, orderMarking, info3.LineItems, 0, bargainDetialId, out activitiesIds);
                                builder.Append(str8);
                                str3 = activitiesIds + ",";
                            }
                        }
                    }
                }
                else
                {
                    List<ShoppingCartInfo> orderSummitCart = null;
                    orderSummitCart = ShoppingCartProcessor.GetOrderSummitCart();
                    string[] strArray2 = context.Request["shippingType"].Split(new char[] { ',' });
                    string[] strArray3 = context.Request["remark"].Split(new char[] { ',' });
                    int index = 0;
                    int num11 = 0;
                    foreach (ShoppingCartInfo info4 in orderSummitCart)
                    {
                        foreach (ShoppingCartItemInfo info5 in info4.LineItems)
                        {
                            if (info5.Type == 1)
                            {
                                num11 += info5.PointNumber;
                            }
                        }
                    }
                    if (num11 > currentMember.Points)
                    {
                        builder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"您当前积分不足！\"");
                    }
                    else
                    {
                        foreach (ShoppingCartInfo info6 in orderSummitCart)
                        {
                            this.ordersummit(info6, context, strArray3[index], shippingId, couponCode, strArray[index], strArray2[index], true, orderMarking, info6.LineItems, 0, 0, out activitiesIds);
                            str3 = str3 + activitiesIds + ",";
                            index++;
                        }
                        builder.Append("\"Status\":\"OK\",\"OrderMarkingStatus\":\"1\",");
                        builder.AppendFormat("\"OrderId\":\"{0}\"", orderMarking);
                    }
                }
                if (!string.IsNullOrEmpty(str3))
                {
                    foreach (string str9 in str3.Substring(0, str3.Length - 1).Split(new char[] { ',' }))
                    {
                        if (!string.IsNullOrEmpty(str9) && (int.Parse(str9) > 0))
                        {
                            int activitiesId = ActivityHelper.GetHishop_Activities(int.Parse(str9));
                            int userId = currentMember.UserId;
                            ActivityHelper.AddActivitiesMember(activitiesId, userId);
                        }
                    }
                }
                builder.Append("}");
                context.Response.ContentType = "application/json";
                context.Response.Write(builder.ToString());
            }
        }

        private void ProcessUserPointByAdmin(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int userId = Globals.RequestFormNum("userid");
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            MemberHelper.GetMember(userId);
            string s = "{\"success\":\"false\"}";
            decimal num2 = decimal.Parse(context.Request["setPoint"]);
            string str2 = context.Request["remark"];
            IntegralDetailInfo point = new IntegralDetailInfo {
                IntegralSourceType = (num2 > 0M) ? 1 : 2,
                IntegralSource = "(管理员)" + currentManager.UserName + ":手动调整积分",
                Userid = userId,
                IntegralChange = num2,
                IntegralStatus = Convert.ToInt32(IntegralDetailStatus.OrderToIntegral),
                Remark = str2
            };
            if (IntegralDetailHelp.AddIntegralDetail(point, null))
            {
                s = "{\"success\":\"true\"}";
            }
            context.Response.Write(s);
            context.Response.End();
        }

        public string regist(string userName, string password, string passagain, string openId, string headimgurl, string referralUserId, HttpContext context)
        {
            if (!(password == passagain))
            {
                return "\"Status\":\"-2\"";
            }
            MemberInfo info = new MemberInfo();
            if (MemberProcessor.GetusernameMember(userName) != null)
            {
                return "\"Status\":\"-1\"";
            }
            MemberInfo member = new MemberInfo();
            string generateId = Globals.GetGenerateId();
            member.GradeId = MemberProcessor.GetDefaultMemberGrade();
            member.OpenId = openId;
            member.UserHead = headimgurl;
            member.UserName = userName;
            member.ReferralUserId = string.IsNullOrEmpty(referralUserId) ? 0 : Convert.ToInt32(referralUserId);
            member.CreateDate = DateTime.Now;
            member.SessionId = generateId;
            member.SessionEndTime = DateTime.Now.AddYears(10);
            member.Password = HiCryptographer.Md5Encrypt(password);
            member.UserBindName = userName;
            if (MemberProcessor.CreateMember(member))
            {
                this.myNotifier.updateAction = UpdateAction.MemberUpdate;
                this.myNotifier.actionDesc = "会员注册";
                this.myNotifier.RecDateUpdate = DateTime.Today;
                this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                this.myNotifier.UpdateDB();
            }
            MemberInfo info3 = MemberProcessor.GetMember(generateId);
            this.setLogin(info3.UserId);
            return ("\"Status\":\"OK\",\"referralUserId\":" + member.ReferralUserId);
        }

        public void RegisterUser(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string userName = context.Request["userName"];
            string password = context.Request["password"];
            string passagain = context.Request["passagain"];
            string str4 = context.Request["openId"];
            string headimgurl = context.Request["headimgurl"];
            int distributorid = 0;
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if (cookie != null)
            {
                distributorid = Globals.ToNum(cookie.Value);
            }
            if ((distributorid > 0) && (DistributorsBrower.GetDistributorInfo(distributorid) == null))
            {
                distributorid = 0;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (!string.IsNullOrEmpty(str4))
            {
                if (MemberProcessor.GetOpenIdMember(str4, "wx") == null)
                {
                    string str6 = this.regist(userName, password, passagain, str4, headimgurl, distributorid.ToString(), context);
                    builder.Append(str6);
                }
                else
                {
                    builder.Append("\"Status\":\"-3\"");
                }
            }
            else
            {
                string str7 = this.regist(userName, password, passagain, str4, headimgurl, distributorid.ToString(), context);
                builder.Append(str7);
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public void RequestReturn(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            decimal num = decimal.Parse(context.Request["Money"]);
            RefundInfo refundInfo = new RefundInfo {
                OrderId = context.Request["orderid"],
                ApplyForTime = DateTime.Now,
                Comments = context.Request["Reason"],
                HandleStatus = RefundInfo.Handlestatus.NoneAudit,
                Account = context.Request["Account"],
                RefundMoney = num,
                SkuId = context.Request["skuid"],
                ProductId = int.Parse(context.Request["productid"]),
                OrderItemID = Globals.RequestFormNum("orderitemid")
            };
            StringBuilder builder = new StringBuilder();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            refundInfo.UserId = currentMember.UserId;
            int orderItemsStatus = 7;
            refundInfo.RefundType = 1;
            if (int.Parse(context.Request["OrderStatus"].ToString()) == 2)
            {
                orderItemsStatus = 6;
                refundInfo.HandleStatus = RefundInfo.Handlestatus.NoRefund;
                refundInfo.RefundType = 2;
                refundInfo.AuditTime = DateTime.Now.ToString();
            }
            builder.Append("{");
            if (!ShoppingProcessor.GetReturnInfo(refundInfo.UserId, refundInfo.OrderId, refundInfo.ProductId, refundInfo.SkuId))
            {
                if (ShoppingProcessor.InsertOrderRefund(refundInfo))
                {
                    if (ShoppingProcessor.UpdateOrderGoodStatu(refundInfo.OrderId, refundInfo.SkuId, orderItemsStatus, refundInfo.OrderItemID))
                    {
                        try
                        {
                            this.myNotifier.updateAction = UpdateAction.OrderUpdate;
                            this.myNotifier.actionDesc = "申请退货或退款";
                            this.myNotifier.RecDateUpdate = DateTime.Today;
                            this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                            this.myNotifier.UpdateDB();
                        }
                        catch (Exception)
                        {
                        }
                        if ((refundInfo.RefundType == 1) || (refundInfo.RefundType == 2))
                        {
                            try
                            {
                                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(refundInfo.OrderId);
                                if (orderInfo != null)
                                {
                                    orderInfo.RefundRemark = refundInfo.Comments.Replace("\r", "").Replace("\n", "");
                                    Messenger.SendWeiXinMsg_ServiceRequest(orderInfo, refundInfo.RefundType);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        builder.Append("\"Status\":\"OK\"");
                    }
                    else
                    {
                        builder.Append("\"Status\":\"Error\"");
                    }
                }
                else
                {
                    builder.Append("\"Status\":\"Error\"");
                }
            }
            else
            {
                builder.Append("\"Status\":\"Repeat\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public string resultstring(int userid, HttpContext context)
        {
            this.setLogin(userid);
            return "\"Status\":\"0\"";
        }

        private void SearchExpressData(HttpContext context)
        {
            string s = string.Empty;
            Uri urlReferrer = context.Request.UrlReferrer;
            if ((urlReferrer == null) || !urlReferrer.ToString().StartsWith(Globals.GetWebUrlStart()))
            {
                context.Response.Write("0");
                context.Response.End();
            }
            if (!string.IsNullOrEmpty(context.Request["OrderId"]))
            {
                string orderId = context.Request["OrderId"];
                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
                if (((orderInfo != null) && (((orderInfo.OrderStatus == OrderStatus.SellerAlreadySent) || (orderInfo.OrderStatus == OrderStatus.Finished)) || (orderInfo.OrderStatus == OrderStatus.Deleted))) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
                {
                    s = ExpressHelper.GetExpressData(orderInfo.logisticsTools, orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
            context.Response.End();
        }

        private void SetDefaultShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                int userId = currentMember.UserId;
                if (MemberProcessor.SetDefaultShippingAddress(Convert.ToInt32(context.Request.Form["shippingid"]), userId))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        public void SetDistributorMsg(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            currentMember.VipCardDate = new DateTime?(DateTime.Now);
            currentMember.CellPhone = context.Request["CellPhone"];
            currentMember.MicroSignal = context.Request["MicroSignal"];
            currentMember.RealName = context.Request["RealName"];
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (MemberProcessor.UpdateMember(currentMember))
            {
                builder.Append("\"Status\":\"OK\"");
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public float setferight(float counttype, float FristNumber, float FristPrice, float AddNumber, float AddPrice)
        {
            double num = 0.0;
            double num2 = Math.Round((double) (counttype - FristNumber), 2);
            if (num2 <= 0.0)
            {
                num += FristPrice;
            }
            else
            {
                num += FristPrice;
                int num3 = (int) Math.Ceiling((double) (num2 / ((double) AddNumber)));
                num += num3 * AddPrice;
            }
            return (float) num;
        }

        private void setLogin(int UserId)
        {
            HttpCookie cookie = new HttpCookie("Vshop-Member") {
                Value = UserId.ToString(),
                Expires = DateTime.Now.AddYears(1)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpCookie cookie2 = new HttpCookie("Vshop-Member-Verify") {
                Value = Globals.EncryptStr(UserId.ToString()),
                Expires = DateTime.Now.AddYears(1)
            };
            HttpContext.Current.Response.Cookies.Add(cookie2);
            HttpContext.Current.Session["userid"] = UserId.ToString();
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(UserId);
            if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
            {
                Globals.SetDistributorCookie(userIdDistributors.UserId);
            }
        }

        public void SetOrderItemStatus(OrderInfo order, decimal redPagerAmount, decimal pointDiscountAverage, decimal DiscountAverage, string productItemList, out decimal remainingMondy)
        {
            remainingMondy = 0M;
            productItemList = productItemList.Trim(new char[] { ',' });
            if (!string.IsNullOrEmpty(productItemList))
            {
                productItemList = "," + productItemList + ",";
            }
            decimal num = 0M;
            decimal num2 = 0M;
            decimal num3 = 0M;
            Dictionary<string, LineItemInfo> lineItems = order.LineItems;
            LineItemInfo info = new LineItemInfo();
            string couponsProductIdsByMemberCouponIDByRedPagerId = string.Empty;
            if (order.RedPagerID.HasValue)
            {
                couponsProductIdsByMemberCouponIDByRedPagerId = CouponHelper.GetCouponsProductIdsByMemberCouponIDByRedPagerId(order.RedPagerID.Value);
                if (!string.IsNullOrEmpty(couponsProductIdsByMemberCouponIDByRedPagerId))
                {
                    couponsProductIdsByMemberCouponIDByRedPagerId = "_" + couponsProductIdsByMemberCouponIDByRedPagerId.Trim(new char[] { '_' }) + "_";
                }
            }
            foreach (KeyValuePair<string, LineItemInfo> pair in lineItems)
            {
                info = pair.Value;
                info.OrderItemsStatus = OrderStatus.WaitBuyerPay;
                if (info.Type == 0)
                {
                    decimal subTotal = info.GetSubTotal();
                    num += subTotal;
                    if (string.IsNullOrEmpty(productItemList) || productItemList.Contains("," + info.ProductId.ToString() + ","))
                    {
                        num2 += subTotal;
                    }
                    if (string.IsNullOrEmpty(couponsProductIdsByMemberCouponIDByRedPagerId) || couponsProductIdsByMemberCouponIDByRedPagerId.Contains("_" + info.ProductId.ToString() + "_"))
                    {
                        num3 += subTotal;
                    }
                }
            }
            if (lineItems.Count > 1)
            {
                if (((redPagerAmount > 0M) || (DiscountAverage > 0M)) || (pointDiscountAverage > 0M))
                {
                    foreach (KeyValuePair<string, LineItemInfo> pair2 in lineItems)
                    {
                        info = pair2.Value;
                        if (info.Type == 0)
                        {
                            float num5 = 0f;
                            num5 = (float.Parse(info.GetSubTotal().ToString()) * float.Parse(pointDiscountAverage.ToString())) / float.Parse(num.ToString());
                            if (string.IsNullOrEmpty(productItemList) || productItemList.Contains("," + info.ProductId.ToString() + ","))
                            {
                                num5 += (float.Parse(info.GetSubTotal().ToString()) * float.Parse(DiscountAverage.ToString())) / float.Parse(num2.ToString());
                            }
                            if (string.IsNullOrEmpty(couponsProductIdsByMemberCouponIDByRedPagerId) || couponsProductIdsByMemberCouponIDByRedPagerId.Contains("_" + info.ProductId.ToString() + "_"))
                            {
                                num5 += (float.Parse(info.GetSubTotal().ToString()) * float.Parse(redPagerAmount.ToString())) / float.Parse(num3.ToString());
                            }
                            info.DiscountAverage = Convert.ToDecimal(num5);
                            if (info.DiscountAverage > (info.ItemAdjustedPrice * info.Quantity))
                            {
                                remainingMondy += info.DiscountAverage - (info.ItemAdjustedPrice * info.Quantity);
                                info.DiscountAverage = info.ItemAdjustedPrice * info.Quantity;
                            }
                        }
                        else
                        {
                            info.DiscountAverage = 0M;
                        }
                    }
                }
            }
            else if (lineItems.Count == 1)
            {
                using (Dictionary<string, LineItemInfo>.Enumerator enumerator3 = lineItems.GetEnumerator())
                {
                    while (enumerator3.MoveNext())
                    {
                        KeyValuePair<string, LineItemInfo> current = enumerator3.Current;
                        if (info.Type == 0)
                        {
                            info.DiscountAverage = (redPagerAmount + pointDiscountAverage) + DiscountAverage;
                        }
                        else
                        {
                            info.DiscountAverage = 0M;
                        }
                        if (info.DiscountAverage > (info.ItemAdjustedPrice * info.Quantity))
                        {
                            remainingMondy += info.DiscountAverage - (info.ItemAdjustedPrice * info.Quantity);
                            info.DiscountAverage = info.ItemAdjustedPrice * info.Quantity;
                        }
                    }
                }
            }
        }

        public void SetUserName(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            currentMember.UserName = context.Request["userName"];
            currentMember.VipCardDate = new DateTime?(DateTime.Now);
            currentMember.CellPhone = context.Request["CellPhone"];
            currentMember.QQ = context.Request["QQ"];
            if (!string.IsNullOrEmpty(currentMember.QQ))
            {
                currentMember.Email = currentMember.QQ + "@qq.com";
            }
            currentMember.RealName = context.Request["RealName"];
            currentMember.CardID = context.Request["CardID"];
            if (!string.IsNullOrEmpty(context.Request["userHead"]))
            {
                currentMember.UserHead = context.Request["userHead"];
            }
            new DistributorsInfo();
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (MemberProcessor.UpdateMember(currentMember))
            {
                builder.Append("\"Status\":\"OK\"");
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public void SignToday(HttpContext context)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("未找到会员信息");
            }
            else if (UserSignHelper.IsSign(currentMember.UserId))
            {
                int num = UserSignHelper.USign(currentMember.UserId);
                context.Response.Write("suss" + num.ToString());
            }
            else
            {
                context.Response.Write("已签到");
            }
        }

        private void UpdateBargainEndDate(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int bargainId = Convert.ToInt32(context.Request["BargainId"]);
            DateTime endDate = DateTime.Parse(context.Request["EndDate"]);
            string s = "";
            if (BargainHelper.UpdateBargain(bargainId, endDate))
            {
                s = "{\"success\":1, \"msg\":\"修改成功\"}";
            }
            else
            {
                s = "{\"success\":0, \"msg\":\"修改失败\"}";
            }
            context.Response.Write(s);
            context.Response.End();
        }

        private void UpdateDistributor(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            StringBuilder sb = new StringBuilder();
            if (this.CheckUpdateDistributors(context, sb))
            {
                DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId(false), true);
                currentDistributors.StoreName = context.Request["stroename"].Trim();
                currentDistributors.StoreDescription = context.Request["descriptions"].Trim();
                currentDistributors.RequestAccount = context.Request["accountname"].Trim();
                currentDistributors.Logo = context.Request["logo"].Trim();
                currentDistributors.CellPhone = context.Request["CellPhone"].Trim();
                if (DistributorsBrower.UpdateDistributorMessage(currentDistributors))
                {
                    DistributorsBrower.UpdateStoreCard(currentDistributors.UserId, "");
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"店铺名称已存在，请重新命名!\"}");
                }
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + sb.ToString() + "\"}");
            }
        }

        private void UpdateShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                ShippingAddressInfo shippingAddress = new ShippingAddressInfo {
                    Address = context.Request.Form["address"],
                    CellPhone = context.Request.Form["cellphone"],
                    ShipTo = context.Request.Form["shipTo"],
                    Zipcode = "",
                    UserId = currentMember.UserId,
                    ShippingId = Convert.ToInt32(context.Request.Form["shippingid"]),
                    RegionId = Convert.ToInt32(context.Request.Form["regionSelectorValue"])
                };
                if (MemberProcessor.UpdateShippingAddress(shippingAddress))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        public void UserLogin(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo member = new MemberInfo();
            string str = context.Request["userName"];
            string sourceData = context.Request["password"];
            string str3 = context.Request["openId"];
            string str4 = context.Request["headimgurl"];
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (!string.IsNullOrEmpty(str))
            {
                member = MemberProcessor.GetusernameMember(str);
                if (member == null)
                {
                    builder.Append("\"Status\":\"-1\"");
                    builder.Append("}");
                    context.Response.Write(builder.ToString());
                    return;
                }
                if (member.Status == Convert.ToInt32(UserStatus.DEL))
                {
                    builder.Append("\"Status\":\"-4\"");
                    builder.Append("}");
                    context.Response.Write(builder.ToString());
                    return;
                }
                if (member.Password == HiCryptographer.Md5Encrypt(sourceData))
                {
                    if (!string.IsNullOrEmpty(str3))
                    {
                        member.OpenId = str3;
                        member.UserHead = str4;
                        MemberProcessor.UpdateMember(member);
                    }
                    this.setLogin(member.UserId);
                    builder.Append("\"Status\":\"OK\"");
                }
                else
                {
                    builder.Append("\"Status\":\"-2\"");
                }
            }
            else
            {
                builder.Append("\"Status\":\"-3\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        private void Vote(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int result = 1;
            int.TryParse(context.Request["voteId"], out result);
            string itemIds = context.Request["itemIds"];
            itemIds = itemIds.Remove(itemIds.Length - 1);
            if (MemberProcessor.GetCurrentMember() == null)
            {
                MemberInfo member = new MemberInfo();
                string generateId = Globals.GetGenerateId();
                member.ReferralUserId = Globals.GetCurrentDistributorId();
                member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                member.UserName = "";
                member.OpenId = "";
                member.CreateDate = DateTime.Now;
                member.SessionId = generateId;
                member.SessionEndTime = DateTime.Now;
                MemberProcessor.CreateMember(member);
                member = MemberProcessor.GetMember(generateId);
                this.setLogin(member.UserId);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (VshopBrowser.Vote(result, itemIds))
            {
                builder.Append("\"Status\":\"OK\"");
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
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

