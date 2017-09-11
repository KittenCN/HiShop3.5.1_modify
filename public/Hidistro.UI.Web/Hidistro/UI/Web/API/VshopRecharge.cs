namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;

    public class VshopRecharge : IHttpHandler, IRequiresSessionState
    {
        private bool CheckAddAmountApply(HttpContext context, ref string msg)
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
            if (string.IsNullOrEmpty(context.Request["applymoney"].Trim()))
            {
                msg = "{\"success\":false,\"msg\":\"提现金额不允许为空！\"}";
                return false;
            }
            if (decimal.Parse(context.Request["applymoney"].Trim()) <= 0M)
            {
                msg = "{\"success\":false,\"msg\":\"提现金额必须大于0！\"}";
                return false;
            }
            decimal num2 = 0M;
            decimal.TryParse(SettingsManager.GetMasterSettings(false).MentionNowMoney, out num2);
            if ((num2 > 0M) && (decimal.Parse(context.Request["applymoney"].Trim()) < num2))
            {
                msg = "{\"success\":false,\"msg\":\"提现金额必须大于等于" + num2.ToString() + "元！\"}";
                return false;
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (decimal.Parse(context.Request["applymoney"].Trim()) > currentMember.AvailableAmount)
            {
                msg = "{\"success\":false,\"msg\":\"提现金额必须为小于现有余额！\"}";
                return false;
            }
            return true;
        }

        private void GetBalanceWithdrawList(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            string s = "{\"success\":\"false\"}";
            int type = Globals.RequestFormNum("type");
            int page = Globals.RequestFormNum("page");
            int pagesize = Globals.RequestFormNum("pagesize");
            if (pagesize < 5)
            {
                pagesize = 10;
            }
            if (page < 1)
            {
                page = 1;
            }
            DbQueryResult result = MemberAmountProcessor.GetBalanceWithdrawListRequest(type, page, pagesize, currentMember.UserId);
            object data = result.Data;
            if (data != null)
            {
                DataTable table = (DataTable) data;
                StringBuilder builder = new StringBuilder();
                int count = table.Rows.Count;
                if (count > 0)
                {
                    table.Rows[0]["State"].ToString();
                    int num5 = 0;
                    builder.Append("{\"State\":" + table.Rows[num5]["State"].ToString() + ",\"id\":" + table.Rows[num5]["ID"].ToString() + ",\"Amount\":\"" + Globals.String2Json(Math.Round((decimal) table.Rows[num5]["Amount"], 2).ToString()) + "\",\"RequestTime\":\"" + DateTime.Parse(table.Rows[num5]["RequestTime"].ToString()).ToString("yyyy-MM-dd") + "\",\"RequestType\":\"" + VShopHelper.GetCommissionPayType(table.Rows[num5]["RequestType"].ToString()) + "\"}");
                    for (num5 = 1; num5 < count; num5++)
                    {
                        builder.Append(",{\"State\":" + table.Rows[num5]["State"].ToString() + ",\"id\":" + table.Rows[num5]["ID"].ToString() + ",\"Amount\":\"" + Globals.String2Json(Math.Round((decimal) table.Rows[num5]["Amount"], 2).ToString()) + "\",\"RequestTime\":\"" + DateTime.Parse(table.Rows[num5]["RequestTime"].ToString()).ToString("yyyy-MM-dd") + "\",\"RequestType\":\"" + VShopHelper.GetCommissionPayType(table.Rows[num5]["RequestType"].ToString()) + "\"}");
                    }
                }
                s = string.Concat(new object[] { "{\"success\":\"true\",\"rowtotal\":\"", table.Rows.Count, "\",\"total\":\"", result.TotalRecords, "\",\"lihtml\":[", builder.ToString(), "]}" });
            }
            context.Response.Write(s);
            context.Response.End();
        }

        private void ProcessAddAmountApply(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            string msg = "";
            if (this.CheckAddAmountApply(context, ref msg))
            {
                string str2 = context.Request["account"].Trim();
                decimal num = decimal.Parse(context.Request["applymoney"].Trim());
                int result = 0;
                int.TryParse(context.Request["requesttype"].Trim(), out result);
                string str3 = context.Request["remark"].Trim();
                string str4 = context.Request["realname"].Trim();
                string str5 = context.Request["bankname"].Trim();
                MemberAmountRequestInfo applyInfo = new MemberAmountRequestInfo();
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                applyInfo.UserId = currentMember.UserId;
                applyInfo.UserName = currentMember.UserName;
                applyInfo.RequestTime = DateTime.Now;
                applyInfo.Amount = num;
                applyInfo.RequestType = (RequesType) result;
                applyInfo.AccountCode = str2;
                switch (result)
                {
                    case 3:
                    case 0:
                        applyInfo.AccountCode = currentMember.OpenId;
                        break;
                }
                string userName = string.IsNullOrEmpty(str4) ? currentMember.RealName : str4;
                if (string.IsNullOrEmpty(userName))
                {
                    userName = currentMember.UserName;
                }
                if (string.IsNullOrEmpty(userName))
                {
                    userName = "未设置";
                }
                applyInfo.AccountName = userName;
                applyInfo.BankName = str5;
                applyInfo.Remark = str3;
                applyInfo.State = RequesState.未审核;
                applyInfo.CellPhone = currentMember.CellPhone;
                if ((string.IsNullOrEmpty(currentMember.OpenId) || (currentMember.OpenId.Length < 0x1c)) && ((result == 3) || (result == 0)))
                {
                    msg = "{\"success\":false,\"msg\":\"您的帐号未绑定，无法通过微信支付余额！\"}";
                }
                else if (MemberAmountProcessor.CreatAmountApplyRequest(applyInfo))
                {
                    try
                    {
                        MemberAmountRequestInfo balance = applyInfo;
                        if (balance != null)
                        {
                            Messenger.SendWeiXinMsg_MemberAmountDrawCashRequest(balance);
                        }
                    }
                    catch (Exception)
                    {
                    }
                    msg = "{\"success\":true,\"msg\":\"申请成功！\"}";
                }
                else
                {
                    msg = "{\"success\":false,\"msg\":\"申请失败！\"}";
                }
            }
            context.Response.Write(msg);
            context.Response.End();
        }

        private void ProcessCommissionToAmount(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId(false));
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (userIdDistributors == null)
            {
                builder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"您不是分销商！\"");
                builder.Append("}");
                context.Response.ContentType = "application/json";
                context.Response.Write(builder.ToString());
            }
            else
            {
                decimal num = DistributorsBrower.CommionsRequestSumMoney(userIdDistributors.UserId);
                decimal amount = decimal.Parse(context.Request["Amount"]);
                if ((amount < 0.01M) || (amount > (userIdDistributors.ReferralBlance - num)))
                {
                    string str = "您输入正确的金额";
                    if ((amount - 0.01M) < (userIdDistributors.ReferralBlance - num))
                    {
                        str = "最多可提现金额为：" + ((amount - 0.01M)).ToString("F2") + "元";
                    }
                    builder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"" + str + "！\"");
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                }
                else
                {
                    MemberAmountDetailedInfo amountinfo = new MemberAmountDetailedInfo {
                        UserId = currentMember.UserId,
                        UserName = currentMember.UserName,
                        PayId = Globals.GetGenerateId(),
                        TradeAmount = amount,
                        TradeType = TradeType.CommissionTransfer,
                        TradeTime = DateTime.Now,
                        State = 1,
                        AvailableAmount = currentMember.AvailableAmount + amount,
                        TradeWays = TradeWays.ShopCommission,
                        Remark = "佣金转入余额"
                    };
                    if (MemberAmountProcessor.CommissionToAmount(amountinfo, userIdDistributors.UserId, amount))
                    {
                        builder.Append("\"Status\":\"OK\"");
                    }
                    else
                    {
                        builder.Append("\"Status\":\"Error\"");
                        builder.AppendFormat(",\"ErrorMsg\":\"佣金转余额失败！\"", new object[0]);
                    }
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                }
            }
        }

        private void ProcessGetAmountList(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            string s = "{\"success\":\"false\"}";
            int type = Globals.RequestFormNum("type");
            int page = Globals.RequestFormNum("page");
            int pagesize = Globals.RequestFormNum("pagesize");
            if (pagesize < 5)
            {
                pagesize = 10;
            }
            if (page < 1)
            {
                page = 1;
            }
            DbQueryResult result = MemberAmountProcessor.GetAmountListRequest(type, page, pagesize, currentMember.UserId);
            object data = result.Data;
            if (data != null)
            {
                DataTable table = (DataTable) data;
                StringBuilder builder = new StringBuilder();
                int count = table.Rows.Count;
                string str2 = string.Empty;
                if (count > 0)
                {
                    str2 = Math.Round(decimal.Parse(table.Rows[0]["TradeAmount"].ToString()), 2).ToString();
                    int num6 = 0;
                    builder.Append("{\"id\":" + table.Rows[num6]["ID"].ToString() + ",\"AvailableAmount\":\"" + Globals.String2Json(Math.Round((decimal) table.Rows[num6]["AvailableAmount"], 2).ToString()) + "\",\"TradeTime\":\"" + DateTime.Parse(table.Rows[num6]["TradeTime"].ToString()).ToString("yyyy-MM-dd") + "\",\"TradeAmount\":\"" + str2 + "\",\"TradeType\":\"" + MemberHelper.StringToTradeType(table.Rows[num6]["TradeType"].ToString()) + "\"}");
                    for (num6 = 1; num6 < count; num6++)
                    {
                        str2 = Math.Round(decimal.Parse(table.Rows[num6]["TradeAmount"].ToString()), 2).ToString();
                        builder.Append(",{\"id\":" + table.Rows[num6]["ID"].ToString() + ",\"AvailableAmount\":\"" + Globals.String2Json(Math.Round((decimal) table.Rows[num6]["AvailableAmount"], 2).ToString()) + "\",\"TradeTime\":\"" + DateTime.Parse(table.Rows[num6]["TradeTime"].ToString()).ToString("yyyy-MM-dd") + "\",\"TradeAmount\":\"" + str2 + "\",\"TradeType\":\"" + MemberHelper.StringToTradeType(table.Rows[num6]["TradeType"].ToString()) + "\"}");
                    }
                }
                s = string.Concat(new object[] { "{\"success\":\"true\",\"rowtotal\":\"", table.Rows.Count, "\",\"total\":\"", result.TotalRecords, "\",\"lihtml\":[", builder.ToString(), "]}" });
            }
            context.Response.Write(s);
            context.Response.End();
        }

        private void ProcessGetMemberAmountDetails(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo member = MemberHelper.GetMember(Globals.RequestFormNum("userid"));
            string s = "{\"success\":\"false\"}";
            int num2 = Globals.RequestFormNum("type");
            int page = Globals.RequestFormNum("page");
            int pagesize = Globals.RequestFormNum("pagesize");
            string str2 = context.Request["startTime"].ToString();
            string str3 = context.Request["endTime"].ToString();
            if (pagesize < 5)
            {
                pagesize = 5;
            }
            if (page < 1)
            {
                page = 1;
            }
            switch (num2)
            {
                case 0:
                {
                    MemberDetailOrderQuery query = new MemberDetailOrderQuery {
                        UserId = new int?(member.UserId)
                    };
                    query.Status = new OrderStatus[] { OrderStatus.Finished, OrderStatus.BuyerAlreadyPaid, OrderStatus.SellerAlreadySent };
                    if (!string.IsNullOrEmpty(str2))
                    {
                        query.StartFinishDate = new DateTime?(Convert.ToDateTime(str2));
                    }
                    if (!string.IsNullOrEmpty(str3))
                    {
                        query.EndFinishDate = new DateTime?(Convert.ToDateTime(str3));
                    }
                    query.PageIndex = page;
                    query.PageSize = pagesize;
                    query.SortBy = "OrderDate";
                    query.SortOrder = SortAction.Desc;
                    DbQueryResult memberDetailOrders = OrderHelper.GetMemberDetailOrders(query);
                    object data = memberDetailOrders.Data;
                    if (data != null)
                    {
                        DataTable table = (DataTable) data;
                        StringBuilder builder = new StringBuilder();
                        int count = table.Rows.Count;
                        string str4 = "-";
                        string str5 = "-";
                        if (count > 0)
                        {
                            int num6 = 0;
                            str4 = !table.Rows[num6].IsNull(table.Columns["GatewayOrderId"]) ? table.Rows[num6]["GatewayOrderId"].ToString() : "-";
                            str5 = (table.Rows[num6]["FinishDate"] != DBNull.Value) ? DateTime.Parse(table.Rows[num6]["FinishDate"].ToString()).ToString("yyyy-MM-dd") : "-";
                            builder.Append("{\"GatewayOrderId\":\"" + str4 + "\",\"OrderTotal\":\"" + Globals.String2Json(Math.Round((decimal) table.Rows[num6]["OrderTotal"], 2).ToString()) + "\",\"OrderDate\":\"" + str5 + "\",\"PaymentType\":\"" + table.Rows[num6]["PaymentType"].ToString() + "\",\"OrderId\":\"" + table.Rows[num6]["OrderId"].ToString() + "\",\"ShipTo\":\"" + table.Rows[num6]["ShipTo"].ToString() + "\",\"Remark\":\"" + table.Rows[num6]["Remark"].ToString() + "\"}");
                            for (num6 = 1; num6 < count; num6++)
                            {
                                str4 = !table.Rows[num6].IsNull(table.Columns["GatewayOrderId"]) ? table.Rows[num6]["GatewayOrderId"].ToString() : "-";
                                str5 = (table.Rows[num6]["FinishDate"] != DBNull.Value) ? DateTime.Parse(table.Rows[num6]["FinishDate"].ToString()).ToString("yyyy-MM-dd") : "-";
                                builder.Append(",{\"GatewayOrderId\":\"" + str4 + "\",\"OrderTotal\":\"" + Globals.String2Json(Math.Round((decimal) table.Rows[num6]["OrderTotal"], 2).ToString()) + "\",\"OrderDate\":\"" + str5 + "\",\"PaymentType\":\"" + table.Rows[num6]["PaymentType"].ToString() + "\",\"OrderId\":\"" + table.Rows[num6]["OrderId"].ToString() + "\",\"ShipTo\":\"" + table.Rows[num6]["ShipTo"].ToString() + "\",\"Remark\":\"" + table.Rows[num6]["Remark"].ToString() + "\"}");
                            }
                        }
                        s = string.Concat(new object[] { "{\"success\":\"true\",\"rowtotal\":\"", table.Rows.Count, "\",\"total\":\"", memberDetailOrders.TotalRecords, "\",\"lihtml\":[", builder.ToString(), "]}" });
                    }
                    break;
                }
                case 1:
                {
                    DbQueryResult result2 = MemberAmountProcessor.GetAmountListRequestByTime(0, page, pagesize, member.UserId, str2, str3);
                    object obj3 = result2.Data;
                    if (obj3 != null)
                    {
                        DataTable table2 = (DataTable) obj3;
                        StringBuilder builder2 = new StringBuilder();
                        int num7 = table2.Rows.Count;
                        string str6 = string.Empty;
                        if (num7 > 0)
                        {
                            int num9 = 0;
                            str6 = Math.Round(decimal.Parse(table2.Rows[num9]["TradeAmount"].ToString()), 2).ToString();
                            builder2.Append("{\"id\":" + table2.Rows[num9]["ID"].ToString() + ",\"AvailableAmount\":\"" + Globals.String2Json(Math.Round((decimal) table2.Rows[num9]["AvailableAmount"], 2).ToString()) + "\",\"TradeTime\":\"" + DateTime.Parse(table2.Rows[num9]["TradeTime"].ToString()).ToString("yyyy-MM-dd") + "\",\"TradeAmount\":\"" + str6 + "\",\"TradeType\":\"" + MemberHelper.StringToTradeType(table2.Rows[num9]["TradeType"].ToString()) + "\",\"PayId\":\"" + table2.Rows[num9]["PayId"].ToString() + "\",\"TradeWays\":\"" + MemberHelper.StringToTradeWays(table2.Rows[num9]["TradeWays"].ToString()) + "\",\"Remark\":\"" + table2.Rows[num9]["Remark"].ToString() + "\"}");
                            for (num9 = 1; num9 < num7; num9++)
                            {
                                str6 = Math.Round(decimal.Parse(table2.Rows[num9]["TradeAmount"].ToString()), 2).ToString();
                                builder2.Append(",{\"id\":" + table2.Rows[num9]["ID"].ToString() + ",\"AvailableAmount\":\"" + Globals.String2Json(Math.Round((decimal) table2.Rows[num9]["AvailableAmount"], 2).ToString()) + "\",\"TradeTime\":\"" + DateTime.Parse(table2.Rows[num9]["TradeTime"].ToString()).ToString("yyyy-MM-dd") + "\",\"TradeAmount\":\"" + str6 + "\",\"TradeType\":\"" + MemberHelper.StringToTradeType(table2.Rows[num9]["TradeType"].ToString()) + "\",\"PayId\":\"" + table2.Rows[num9]["PayId"].ToString() + "\",\"TradeWays\":\"" + MemberHelper.StringToTradeWays(table2.Rows[num9]["TradeWays"].ToString()) + "\",\"Remark\":\"" + table2.Rows[num9]["Remark"].ToString() + "\"}");
                            }
                        }
                        s = string.Concat(new object[] { "{\"success\":\"true\",\"rowtotal\":\"", table2.Rows.Count, "\",\"total\":\"", result2.TotalRecords, "\",\"lihtml\":[", builder2.ToString(), "]}" });
                    }
                    break;
                }
            }
            context.Response.Write(s);
            context.Response.End();
        }

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request["action"])
            {
                case "SubmmitAmount":
                    this.ProcessSubmmitAmount(context);
                    return;

                case "GetAmountList":
                    this.ProcessGetAmountList(context);
                    return;

                case "GetBalanceWithdrawList":
                    this.GetBalanceWithdrawList(context);
                    return;

                case "CommissionToAmount":
                    this.ProcessCommissionToAmount(context);
                    return;

                case "AddAmountApply":
                    this.ProcessAddAmountApply(context);
                    return;

                case "GetMemberAmountDetails":
                    this.ProcessGetMemberAmountDetails(context);
                    return;

                case "SetUserAmountByAdmin":
                    this.ProcessUserAmountByAdmin(context);
                    return;
            }
        }

        private void ProcessSubmmitAmount(HttpContext context)
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
                int modeId = int.Parse(context.Request["paymentType"]);
                decimal num2 = decimal.Parse(context.Request["Amount"]);
                string generateId = Globals.GetGenerateId();
                if (num2 > 1000000M)
                {
                    builder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"最大充值金额不大于100万！\"");
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                }
                else
                {
                    MemberAmountDetailedInfo amountInfo = new MemberAmountDetailedInfo {
                        UserId = currentMember.UserId,
                        UserName = currentMember.UserName,
                        PayId = generateId,
                        TradeAmount = num2,
                        TradeType = TradeType.Recharge,
                        TradeTime = DateTime.Now,
                        State = 0,
                        AvailableAmount = currentMember.AvailableAmount + num2,
                        Remark = "余额充值"
                    };
                    if (modeId == 0x58)
                    {
                        amountInfo.TradeWays = TradeWays.WeChatWallet;
                    }
                    else
                    {
                        PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(modeId);
                        if (paymentMode != null)
                        {
                            if (paymentMode.Gateway == "hishop.plugins.payment.ws_wappay.wswappayrequest")
                            {
                                amountInfo.TradeWays = TradeWays.Alipay;
                            }
                            else if (paymentMode.Gateway == "Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest")
                            {
                                amountInfo.TradeWays = TradeWays.ShengFutong;
                            }
                        }
                    }
                    if (MemberAmountProcessor.CreatAmount(amountInfo))
                    {
                        builder.Append("\"Status\":\"OK\",\"PayIdStatus\":\"" + amountInfo.PayId + "\",");
                        builder.AppendFormat("\"PayId\":\"{0}\"", amountInfo.PayId);
                    }
                    else
                    {
                        builder.Append("\"Status\":\"Error\"");
                        builder.AppendFormat(",\"ErrorMsg\":\"提交充值失败！\"", new object[0]);
                    }
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                }
            }
        }

        private void ProcessUserAmountByAdmin(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int userId = Globals.RequestFormNum("userid");
            MemberInfo member = MemberHelper.GetMember(userId);
            string s = "{\"success\":\"false\"}";
            decimal num2 = decimal.Parse(context.Request["setAmount"]);
            string str2 = context.Request["remark"];
            MemberAmountDetailedInfo model = new MemberAmountDetailedInfo {
                UserId = userId,
                UserName = member.UserName,
                PayId = Globals.GetGenerateId(),
                TradeAmount = num2,
                TradeType = TradeType.ShopAdjustment,
                TradeTime = DateTime.Now,
                State = 1,
                TradeWays = TradeWays.Balance,
                AvailableAmount = member.AvailableAmount + num2,
                Remark = str2
            };
            if (MemberAmountProcessor.SetAmountByShopAdjustment(model))
            {
                s = "{\"success\":\"true\"}";
            }
            context.Response.Write(s);
            context.Response.End();
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

