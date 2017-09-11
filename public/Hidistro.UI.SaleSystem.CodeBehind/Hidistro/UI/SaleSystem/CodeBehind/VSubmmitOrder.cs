namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ControlPanel.Promotions;
    using global::ControlPanel.Promotions;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VSubmmitOrder : VMemberTemplatedWebControl
    {
        private HtmlInputHidden BalanceCanPayMoney;
        private int buyAmount;
        private DataTable dtActivities = ActivityHelper.GetActivities();
        public DataTable GetUserCoupons;
        private HtmlInputControl groupbuyHiddenBox;
        private bool isbargain = (Globals.RequestQueryNum("bargainDetialId") > 0);
        private Literal litAddAddress;
        private Literal litAddress;
        private Literal litCellPhone;
        private Literal litDisplayPoint;
        private Literal litDisplayPointNumber;
        private Literal litIsUseBalance;
        private Literal litOrderTotal;
        private Literal litPointNumber;
        private Literal litShipTo;
        private Literal litShowMes;
        private Literal litUseMembersPointShow;
        private HtmlInputHidden MembersPointMoney;
        private string productSku;
        private HtmlInputHidden regionId;
        private VshopTemplatedRepeater rptAddress;
        private VshopTemplatedRepeater rptCartProducts;
        private HtmlInputHidden selectShipTo;

        protected override void AttachChildControls()
        {
            this.litShipTo = (Literal) this.FindControl("litShipTo");
            this.litIsUseBalance = (Literal) this.FindControl("litIsUseBalance");
            this.litCellPhone = (Literal) this.FindControl("litCellPhone");
            this.litAddress = (Literal) this.FindControl("litAddress");
            this.litShowMes = (Literal) this.FindControl("litShowMes");
            this.GetUserCoupons = MemberProcessor.GetUserCoupons();
            this.rptCartProducts = (VshopTemplatedRepeater) this.FindControl("rptCartProducts");
            this.rptCartProducts.ItemDataBound += new RepeaterItemEventHandler(this.rptCartProducts_ItemDataBound);
            this.litOrderTotal = (Literal) this.FindControl("litOrderTotal");
            this.litPointNumber = (Literal) this.FindControl("litPointNumber");
            this.litUseMembersPointShow = (Literal) this.FindControl("litUseMembersPointShow");
            this.litDisplayPointNumber = (Literal) this.FindControl("litDisplayPointNumber");
            this.litDisplayPoint = (Literal) this.FindControl("litDisplayPoint");
            this.BalanceCanPayMoney = (HtmlInputHidden) this.FindControl("BalanceCanPayMoney");
            this.groupbuyHiddenBox = (HtmlInputControl) this.FindControl("groupbuyHiddenBox");
            this.rptAddress = (VshopTemplatedRepeater) this.FindControl("rptAddress");
            this.selectShipTo = (HtmlInputHidden) this.FindControl("selectShipTo");
            this.MembersPointMoney = (HtmlInputHidden) this.FindControl("MembersPointMoney");
            this.regionId = (HtmlInputHidden) this.FindControl("regionId");
            this.litAddAddress = (Literal) this.FindControl("litAddAddress");
            IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
            this.rptAddress.DataSource = from item in shippingAddresses
                orderby item.IsDefault
                select item;
            this.rptAddress.DataBind();
            ShippingAddressInfo info = shippingAddresses.FirstOrDefault<ShippingAddressInfo>(item => item.IsDefault);
            if (info == null)
            {
                info = (shippingAddresses.Count > 0) ? shippingAddresses[0] : null;
            }
            if (info != null)
            {
                this.litShipTo.Text = info.ShipTo;
                this.litCellPhone.Text = info.CellPhone;
                this.litAddress.Text = info.Address;
                this.selectShipTo.SetWhenIsNotNull(info.ShippingId.ToString());
                this.regionId.SetWhenIsNotNull(info.RegionId.ToString());
            }
            this.litAddAddress.Text = "<li><a href='/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "'>新增收货地址</a></li>";
            if ((shippingAddresses == null) || (shippingAddresses.Count == 0))
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
            }
            else
            {
                List<ShoppingCartInfo> orderSummitCart = new List<ShoppingCartInfo>();
                if (((int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"])) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"])) && ((this.Page.Request.QueryString["from"] == "signBuy") || (this.Page.Request.QueryString["from"] == "groupBuy")))
                {
                    this.productSku = this.Page.Request.QueryString["productSku"];
                    if (this.isbargain)
                    {
                        int bargainDetialId = Globals.RequestQueryNum("bargainDetialId");
                        orderSummitCart = ShoppingCartProcessor.GetListShoppingCart(this.productSku, this.buyAmount, bargainDetialId, 0);
                    }
                    else
                    {
                        int buyAmount = this.buyAmount;
                        int id = Globals.RequestQueryNum("limitedTimeDiscountId");
                        if (id > 0)
                        {
                            bool flag = true;
                            LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(id);
                            if (discountInfo == null)
                            {
                                flag = false;
                            }
                            if (flag)
                            {
                                if (MemberHelper.CheckCurrentMemberIsInRange(discountInfo.ApplyMembers, discountInfo.DefualtGroup, discountInfo.CustomGroup, base.CurrentMemberInfo.UserId))
                                {
                                    if (discountInfo.LimitNumber != 0)
                                    {
                                        int num4 = ShoppingCartProcessor.GetLimitedTimeDiscountUsedNum(id, this.productSku, 0, base.CurrentMemberInfo.UserId, false);
                                        if (this.buyAmount > (discountInfo.LimitNumber - num4))
                                        {
                                            buyAmount = discountInfo.LimitNumber - num4;
                                        }
                                    }
                                }
                                else
                                {
                                    id = 0;
                                }
                            }
                            else
                            {
                                id = 0;
                            }
                        }
                        if (id > 0)
                        {
                            ShoppingCartProcessor.RemoveLineItem(this.productSku, 0, id);
                        }
                        if ((buyAmount == 0) && (id > 0))
                        {
                            buyAmount = this.buyAmount;
                            id = 0;
                        }
                        orderSummitCart = ShoppingCartProcessor.GetListShoppingCart(this.productSku, buyAmount, 0, id);
                    }
                }
                else
                {
                    orderSummitCart = ShoppingCartProcessor.GetOrderSummitCart();
                }
                if (orderSummitCart == null)
                {
                    HttpContext.Current.Response.Write("<script>alert('商品已下架或没有需要结算的订单！');location.href='/Vshop/ShoppingCart.aspx'</script>");
                }
                else
                {
                    if (orderSummitCart.Count > 1)
                    {
                        this.litShowMes.Text = "<div style=\"color: #F60; \"><img  src=\"/Utility/pics/u77.png\">您所购买的商品不支持同一个物流规则发货，系统自动拆分成多个子订单处理</div>";
                    }
                    this.rptCartProducts.DataSource = orderSummitCart;
                    this.rptCartProducts.DataBind();
                    decimal num5 = 0M;
                    decimal num6 = 0M;
                    decimal num7 = 0M;
                    int num8 = 0;
                    foreach (ShoppingCartInfo info3 in orderSummitCart)
                    {
                        num8 += info3.GetPointNumber;
                        num5 += info3.Total;
                        num6 += info3.Exemption;
                        num7 += info3.ShipCost;
                    }
                    decimal num9 = num6;
                    decimal d = num5 - num9;
                    if (d <= 0M)
                    {
                        d = 0M;
                    }
                    d = decimal.Round(d, 2);
                    this.litOrderTotal.Text = d.ToString("F2");
                    if (num8 == 0)
                    {
                        this.litDisplayPointNumber.Text = "style=\"display:none;\"";
                    }
                    this.litPointNumber.Text = num8.ToString();
                    int num11 = base.CurrentMemberInfo.Points - num8;
                    decimal num12 = 0M;
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    if ((d * masterSettings.PointToCashRate) > base.CurrentMemberInfo.Points)
                    {
                        if (num11 > (masterSettings.PonitToCash_MaxAmount * masterSettings.PointToCashRate))
                        {
                            num12 = masterSettings.PonitToCash_MaxAmount;
                            num11 = ((int) masterSettings.PonitToCash_MaxAmount) * masterSettings.PointToCashRate;
                        }
                        else
                        {
                            num12 = num11 / masterSettings.PointToCashRate;
                        }
                    }
                    else
                    {
                        num12 = masterSettings.PonitToCash_MaxAmount;
                        if (num12 > d)
                        {
                            num12 = d;
                        }
                        num12 = decimal.Round(num12, 2);
                        num11 = (int) (num12 * masterSettings.PointToCashRate);
                    }
                    if (num11 <= 0)
                    {
                        num11 = 0;
                        num12 = 0M;
                    }
                    this.MembersPointMoney.Value = num12.ToString("F2");
                    if (num11 > 0)
                    {
                        this.litUseMembersPointShow.Text = string.Concat(new object[] { "<input type='hidden' id='hdCanUsePoint' value='", num11, "'/><input type='hidden' id='hdCanUsePointMoney' value='", num12.ToString("F2"), "'/><div class=\"prompt-text pull-left\" id=\"divUseMembersPointShow\">可用<span  id=\"usepointnum\">", num11, "</span>积分抵 <span class=\"colorr\">\x00a5<span  id=\"usepointmoney\">", num12.ToString("F2"), "</span></span>元</div><div class=\"switch pull-right\" id=\"mySwitchUseMembersPoint\"><input  type=\"checkbox\" /></div>" });
                    }
                    else
                    {
                        this.litUseMembersPointShow.Text = "<input type='hidden' id='hdCanUsePoint' value='0'/><input type='hidden' id='hdCanUsePointMoney' value='0'/><div class=\"prompt-text pull-left\" id=\"divUseMembersPointShow\">可用<span  id=\"usepointnum\">0</span>积分 <span  id=\"usepointmoney\" style=\"display:none\">" + num12.ToString("F2") + "</span></div><div class=\"switch pull-right\" id=\"mySwitchUseMembersPoint\" style=\"display:none\"><input type=\"checkbox\" disabled /></div>";
                    }
                    decimal availableAmount = 0M;
                    if (d > base.CurrentMemberInfo.AvailableAmount)
                    {
                        availableAmount = base.CurrentMemberInfo.AvailableAmount;
                        this.BalanceCanPayMoney.Value = base.CurrentMemberInfo.AvailableAmount.ToString("F2");
                    }
                    else
                    {
                        availableAmount = d;
                        this.BalanceCanPayMoney.Value = d.ToString("F2");
                    }
                    if ((base.CurrentMemberInfo.AvailableAmount > 0M) && masterSettings.EnableBalancePayment)
                    {
                        this.litIsUseBalance.Text = "<div class=\"prompt-text pull-left\">余额支付 <span class=\"colorr\">\x00a5<span id=\"spCanpayMoney\">" + availableAmount.ToString("F2") + "</span></span>(可用 \x00a5<span id=\"spAvailableAmount\">" + base.CurrentMemberInfo.AvailableAmount.ToString("F2") + "</span>)</div><div class=\"switch pull-right\" id=\"mySwitchUseBalance\"><input type=\"checkbox\" " + ((availableAmount > 0M) ? "" : " disabled") + " /></div></div>";
                    }
                    else
                    {
                        this.litIsUseBalance.Text = "<div class=\"prompt-text pull-left\"" + (masterSettings.EnableBalancePayment ? "" : " style=\"display:none\"") + ">余额可用 <span class=\"colorr\">\x00a5<span id=\"spCanpayMoney\">0.00</span></span><span id=\"spAvailableAmount\" style=\"display:none\">0.00</span></div><div class=\"switch pull-right\" id=\"mySwitchUseBalance\" style=\"display:none\"><input type=\"checkbox\" disabled /></div></div>";
                    }
                    if (!masterSettings.PonitToCash_Enable)
                    {
                        this.litDisplayPoint.Text = " style=\"display:none;\"";
                    }
                    PageTitle.AddSiteNameTitle("订单确认");
                }
            }
        }

        public decimal DiscountMoney(List<ShoppingCartInfo> infoList)
        {
            decimal num = 0M;
            decimal num2 = 0M;
            decimal num3 = 0M;
            decimal num4 = 0M;
            int num5 = 0;
            foreach (ShoppingCartInfo info in infoList)
            {
                foreach (ShoppingCartItemInfo info2 in info.LineItems)
                {
                    if (info2.Type == 0)
                    {
                        num4 += info2.SubTotal;
                        num5 += info2.Quantity;
                    }
                }
            }
            for (int i = 0; i < this.dtActivities.Rows.Count; i++)
            {
                decimal num7 = 0M;
                int num8 = 0;
                DataTable table = ActivityHelper.GetActivities_Detail(int.Parse(this.dtActivities.Rows[i]["ActivitiesId"].ToString()));
                foreach (ShoppingCartInfo info3 in infoList)
                {
                    foreach (ShoppingCartItemInfo info4 in info3.LineItems)
                    {
                        if ((info4.Type == 0) && (ActivityHelper.GetActivitiesProducts(int.Parse(this.dtActivities.Rows[i]["ActivitiesId"].ToString()), info4.ProductId).Rows.Count > 0))
                        {
                            num7 += info4.SubTotal;
                            num8 += info4.Quantity;
                        }
                    }
                }
                if (table.Rows.Count > 0)
                {
                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        if (bool.Parse(this.dtActivities.Rows[i]["isAllProduct"].ToString()))
                        {
                            if (decimal.Parse(table.Rows[j]["MeetMoney"].ToString()) > 0M)
                            {
                                if (num4 >= decimal.Parse(table.Rows[table.Rows.Count - 1]["MeetMoney"].ToString()))
                                {
                                    num2 = decimal.Parse(table.Rows[table.Rows.Count - 1]["MeetMoney"].ToString());
                                    num = decimal.Parse(table.Rows[table.Rows.Count - 1]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num4 < decimal.Parse(table.Rows[0]["MeetMoney"].ToString()))
                                {
                                    break;
                                }
                                if (num4 >= decimal.Parse(table.Rows[j]["MeetMoney"].ToString()))
                                {
                                    num2 = decimal.Parse(table.Rows[j]["MeetMoney"].ToString());
                                    num = decimal.Parse(table.Rows[j]["ReductionMoney"].ToString());
                                }
                            }
                            else
                            {
                                if (num5 >= int.Parse(table.Rows[table.Rows.Count - 1]["MeetNumber"].ToString()))
                                {
                                    num2 = decimal.Parse(table.Rows[table.Rows.Count - 1]["MeetMoney"].ToString());
                                    num3 = decimal.Parse(table.Rows[table.Rows.Count - 1]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num5 < int.Parse(table.Rows[0]["MeetNumber"].ToString()))
                                {
                                    break;
                                }
                                if (num5 >= int.Parse(table.Rows[j]["MeetNumber"].ToString()))
                                {
                                    num2 = decimal.Parse(table.Rows[j]["MeetMoney"].ToString());
                                    num3 = decimal.Parse(table.Rows[j]["ReductionMoney"].ToString());
                                }
                            }
                        }
                        else
                        {
                            num4 = num7;
                            num5 = num8;
                            if (decimal.Parse(table.Rows[j]["MeetMoney"].ToString()) > 0M)
                            {
                                if (num4 >= decimal.Parse(table.Rows[table.Rows.Count - 1]["MeetMoney"].ToString()))
                                {
                                    num2 = decimal.Parse(table.Rows[table.Rows.Count - 1]["MeetMoney"].ToString());
                                    num = decimal.Parse(table.Rows[table.Rows.Count - 1]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num4 <= decimal.Parse(table.Rows[0]["MeetMoney"].ToString()))
                                {
                                    num2 = decimal.Parse(table.Rows[0]["MeetMoney"].ToString());
                                    num = decimal.Parse(table.Rows[0]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num4 >= decimal.Parse(table.Rows[j]["MeetMoney"].ToString()))
                                {
                                    num2 = decimal.Parse(table.Rows[j]["MeetMoney"].ToString());
                                    num = decimal.Parse(table.Rows[j]["ReductionMoney"].ToString());
                                }
                            }
                            else
                            {
                                if (num5 >= int.Parse(table.Rows[table.Rows.Count - 1]["MeetNumber"].ToString()))
                                {
                                    num2 = decimal.Parse(table.Rows[table.Rows.Count - 1]["MeetMoney"].ToString());
                                    num = decimal.Parse(table.Rows[table.Rows.Count - 1]["ReductionMoney"].ToString());
                                    break;
                                }
                                if (num5 < int.Parse(table.Rows[0]["MeetNumber"].ToString()))
                                {
                                    break;
                                }
                                if (num5 >= int.Parse(table.Rows[j]["MeetNumber"].ToString()))
                                {
                                    num2 = decimal.Parse(table.Rows[j]["MeetMoney"].ToString());
                                    num = decimal.Parse(table.Rows[j]["ReductionMoney"].ToString());
                                }
                            }
                        }
                    }
                    if ((num4 >= num2) || (num2 == 0M))
                    {
                        num3 += num;
                    }
                }
            }
            return num3;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VSubmmitOrder.html";
            }
            base.OnInit(e);
        }

        private void rptCartProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                List<ShoppingCartItemInfo> list = (List<ShoppingCartItemInfo>) DataBinder.Eval(e.Item.DataItem, "LineItems");
                Literal literal = (Literal) e.Item.Controls[0].FindControl("LitCoupon");
                Literal literal2 = (Literal) e.Item.Controls[0].FindControl("litExemption");
                Literal literal3 = (Literal) e.Item.Controls[0].FindControl("litoldExemption");
                Literal literal4 = (Literal) e.Item.Controls[0].FindControl("litoldTotal");
                Literal literal5 = (Literal) e.Item.Controls[0].FindControl("litTotal");
                Literal literal6 = (Literal) e.Item.Controls[0].FindControl("litbFreeShipping");
                string str = "";
                string str2 = " <div class=\"btn-group coupon\">";
                object obj2 = str2;
                object obj3 = string.Concat(new object[] { obj2, "<button type=\"button\" class=\"btn btn-default dropdown-toggle coupondropdown\" data-toggle=\"dropdown\"   id='coupondropdown", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "'>选择优惠券<span class=\"caret\"></span></button>" });
                str2 = string.Concat(new object[] { obj3, "<ul id=\"coupon", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "\" class=\"dropdown-menu\" role=\"menu\">" });
                if ((this.GetUserCoupons.Rows.Count > 0) && !this.isbargain)
                {
                    object obj4 = str;
                    str = string.Concat(new object[] { obj4, "<li><a onclick=\"Couponasetselect('", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "','不使用','0',0,'0')\"   value=\"0\">不使用</a></li>" });
                }
                if (!this.isbargain)
                {
                    for (int i = 0; i < this.GetUserCoupons.Rows.Count; i++)
                    {
                        if ((MemberProcessor.CheckCurrentMemberIsInRange(this.GetUserCoupons.Rows[i]["MemberGrades"].ToString(), this.GetUserCoupons.Rows[i]["DefualtGroup"].ToString(), this.GetUserCoupons.Rows[i]["CustomGroup"].ToString()) || (this.GetUserCoupons.Rows[i]["MemberGrades"].ToString() == "0")) || (this.GetUserCoupons.Rows[i]["MemberGrades"].ToString() == base.CurrentMemberInfo.GradeId.ToString()))
                        {
                            if (bool.Parse(this.GetUserCoupons.Rows[i]["IsAllProduct"].ToString()))
                            {
                                decimal num2 = 0M;
                                foreach (ShoppingCartItemInfo info in list)
                                {
                                    if (info.Type == 0)
                                    {
                                        num2 += info.SubTotal;
                                    }
                                }
                                if (decimal.Parse(this.GetUserCoupons.Rows[i]["ConditionValue"].ToString()) <= num2)
                                {
                                    object obj5 = str;
                                    str = string.Concat(new object[] { 
                                        obj5, "<li><a onclick=\"Couponasetselect('", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "','", this.GetUserCoupons.Rows[i]["CouponValue"], "元现金券','", this.GetUserCoupons.Rows[i]["CouponValue"], "',", this.GetUserCoupons.Rows[i]["Id"], ",'", this.GetUserCoupons.Rows[i]["CouponValue"], "元现金券|", this.GetUserCoupons.Rows[i]["Id"], "|", this.GetUserCoupons.Rows[i]["ConditionValue"], "|", 
                                        this.GetUserCoupons.Rows[i]["CouponValue"], "')\" id=\"acoupon", DataBinder.Eval(e.Item.DataItem, "TemplateId"), this.GetUserCoupons.Rows[i]["Id"], "\" value=\"", this.GetUserCoupons.Rows[i]["Id"], "\">", this.GetUserCoupons.Rows[i]["CouponValue"], "元现金券</a></li>"
                                     });
                                }
                            }
                            else
                            {
                                decimal num3 = 0M;
                                bool flag = false;
                                foreach (ShoppingCartItemInfo info2 in list)
                                {
                                    if ((info2.Type == 0) && (MemberProcessor.GetCouponByProducts(int.Parse(this.GetUserCoupons.Rows[i]["CouponId"].ToString()), info2.ProductId).Rows.Count > 0))
                                    {
                                        num3 += info2.SubTotal;
                                        flag = true;
                                    }
                                }
                                if (flag && (decimal.Parse(this.GetUserCoupons.Rows[i]["ConditionValue"].ToString()) <= num3))
                                {
                                    object obj6 = str;
                                    str = string.Concat(new object[] { 
                                        obj6, "<li><a onclick=\"Couponasetselect('", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "','", this.GetUserCoupons.Rows[i]["CouponValue"], "元现金券','", this.GetUserCoupons.Rows[i]["CouponValue"], "',", this.GetUserCoupons.Rows[i]["Id"], ",'", this.GetUserCoupons.Rows[i]["CouponValue"], "元现金券|", this.GetUserCoupons.Rows[i]["Id"], "|", this.GetUserCoupons.Rows[i]["ConditionValue"], "|", 
                                        this.GetUserCoupons.Rows[i]["CouponValue"], "')\" id=\"acoupon", DataBinder.Eval(e.Item.DataItem, "TemplateId"), this.GetUserCoupons.Rows[i]["Id"], "\" value=\"", this.GetUserCoupons.Rows[i]["Id"], "\">", this.GetUserCoupons.Rows[i]["CouponValue"], "元现金券</a></li>"
                                     });
                                }
                            }
                        }
                    }
                }
                object obj7 = str2 + str;
                str2 = string.Concat(new object[] { obj7, "</ul></div><input type=\"hidden\"  class=\"ClassCoupon\"   id=\"selectCoupon", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "\"/>  " });
                if (!string.IsNullOrEmpty(str))
                {
                    literal.Text = string.Concat(new object[] { str2, "<input type=\"hidden\"   id='selectCouponValue", DataBinder.Eval(e.Item.DataItem, "TemplateId"), "' class=\"selectCouponValue\" />" });
                }
                else
                {
                    literal.Text = "<input type=\"hidden\"   id='selectCouponValue" + DataBinder.Eval(e.Item.DataItem, "TemplateId") + "' class=\"selectCouponValue\" />";
                }
                decimal num4 = 0M;
                decimal num5 = 0M;
                decimal num6 = 0M;
                decimal num7 = 0M;
                decimal num8 = 0M;
                int num9 = 0;
                foreach (ShoppingCartItemInfo info3 in list)
                {
                    if (info3.Type == 0)
                    {
                        num8 += info3.SubTotal;
                        num9 += info3.Quantity;
                    }
                }
                num7 = num8;
                if (!this.isbargain)
                {
                    for (int j = 0; j < this.dtActivities.Rows.Count; j++)
                    {
                        if ((int.Parse(this.dtActivities.Rows[j]["attendTime"].ToString()) != 0) && (int.Parse(this.dtActivities.Rows[j]["attendTime"].ToString()) <= ActivityHelper.GetActivitiesMember(base.CurrentMemberInfo.UserId, int.Parse(this.dtActivities.Rows[j]["ActivitiesId"].ToString()))))
                        {
                            continue;
                        }
                        decimal num11 = 0M;
                        int num12 = 0;
                        DataTable table2 = ActivityHelper.GetActivities_Detail(int.Parse(this.dtActivities.Rows[j]["ActivitiesId"].ToString()));
                        foreach (ShoppingCartItemInfo info4 in list)
                        {
                            if ((info4.Type == 0) && (ActivityHelper.GetActivitiesProducts(int.Parse(this.dtActivities.Rows[j]["ActivitiesId"].ToString()), info4.ProductId).Rows.Count > 0))
                            {
                                num11 += info4.SubTotal;
                                num12 += info4.Quantity;
                            }
                        }
                        bool flag2 = false;
                        if (table2.Rows.Count > 0)
                        {
                            for (int k = 0; k < table2.Rows.Count; k++)
                            {
                                if (MemberHelper.CheckCurrentMemberIsInRange(table2.Rows[k]["MemberGrades"].ToString(), table2.Rows[k]["DefualtGroup"].ToString(), table2.Rows[k]["CustomGroup"].ToString(), base.CurrentMemberInfo.UserId))
                                {
                                    if (bool.Parse(this.dtActivities.Rows[j]["isAllProduct"].ToString()))
                                    {
                                        if (decimal.Parse(table2.Rows[k]["MeetMoney"].ToString()) > 0M)
                                        {
                                            if ((num8 != 0M) && (num8 >= decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString())))
                                            {
                                                num5 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                                num4 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                                literal6.Text = table2.Rows[table2.Rows.Count - 1]["bFreeShipping"].ToString();
                                                break;
                                            }
                                            if ((num8 != 0M) && (num8 < decimal.Parse(table2.Rows[0]["MeetMoney"].ToString())))
                                            {
                                                break;
                                            }
                                            if ((num8 != 0M) && (num8 >= decimal.Parse(table2.Rows[k]["MeetMoney"].ToString())))
                                            {
                                                num5 = decimal.Parse(table2.Rows[k]["MeetMoney"].ToString());
                                                num4 = decimal.Parse(table2.Rows[k]["ReductionMoney"].ToString());
                                                literal6.Text = table2.Rows[k]["bFreeShipping"].ToString();
                                            }
                                        }
                                        else
                                        {
                                            if ((num9 != 0) && (num9 >= int.Parse(table2.Rows[table2.Rows.Count - 1]["MeetNumber"].ToString())))
                                            {
                                                num5 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                                num6 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                                flag2 = true;
                                                literal6.Text = table2.Rows[table2.Rows.Count - 1]["bFreeShipping"].ToString();
                                                break;
                                            }
                                            if ((num9 != 0) && (num9 < int.Parse(table2.Rows[0]["MeetNumber"].ToString())))
                                            {
                                                break;
                                            }
                                            if ((num9 != 0) && (num9 >= int.Parse(table2.Rows[k]["MeetNumber"].ToString())))
                                            {
                                                num5 = decimal.Parse(table2.Rows[k]["MeetMoney"].ToString());
                                                num6 = decimal.Parse(table2.Rows[k]["ReductionMoney"].ToString());
                                                flag2 = true;
                                                literal6.Text = table2.Rows[k]["bFreeShipping"].ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        num8 = num11;
                                        num9 = num12;
                                        if (decimal.Parse(table2.Rows[k]["MeetMoney"].ToString()) > 0M)
                                        {
                                            if ((num8 != 0M) && (num8 >= decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString())))
                                            {
                                                num5 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                                num4 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                                literal6.Text = table2.Rows[table2.Rows.Count - 1]["bFreeShipping"].ToString();
                                                break;
                                            }
                                            if ((num8 != 0M) && (num8 < decimal.Parse(table2.Rows[0]["MeetMoney"].ToString())))
                                            {
                                                break;
                                            }
                                            if ((num8 != 0M) && (num8 >= decimal.Parse(table2.Rows[k]["MeetMoney"].ToString())))
                                            {
                                                num5 = decimal.Parse(table2.Rows[k]["MeetMoney"].ToString());
                                                num4 = decimal.Parse(table2.Rows[k]["ReductionMoney"].ToString());
                                                literal6.Text = table2.Rows[k]["bFreeShipping"].ToString();
                                            }
                                        }
                                        else
                                        {
                                            if ((num9 != 0) && (num9 >= int.Parse(table2.Rows[table2.Rows.Count - 1]["MeetNumber"].ToString())))
                                            {
                                                num5 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["MeetMoney"].ToString());
                                                num4 = decimal.Parse(table2.Rows[table2.Rows.Count - 1]["ReductionMoney"].ToString());
                                                flag2 = true;
                                                literal6.Text = table2.Rows[table2.Rows.Count - 1]["bFreeShipping"].ToString();
                                                break;
                                            }
                                            if ((num9 != 0) && (num9 < int.Parse(table2.Rows[0]["MeetNumber"].ToString())))
                                            {
                                                break;
                                            }
                                            if ((num9 != 0) && (num9 >= int.Parse(table2.Rows[k]["MeetNumber"].ToString())))
                                            {
                                                num5 = decimal.Parse(table2.Rows[k]["MeetMoney"].ToString());
                                                num4 = decimal.Parse(table2.Rows[k]["ReductionMoney"].ToString());
                                                flag2 = true;
                                                literal6.Text = table2.Rows[k]["bFreeShipping"].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            if (flag2)
                            {
                                if (num9 > 0)
                                {
                                    num6 += num4;
                                }
                            }
                            else if (((num8 != 0M) && (num5 != 0M)) && (num8 >= num5))
                            {
                                num6 += num4;
                            }
                        }
                    }
                }
                literal2.Text = num6.ToString("F2");
                literal3.Text = num6.ToString("F2");
                literal5.Text = (num7 - num6).ToString("F2");
                literal4.Text = (num7 - num6).ToString("F2");
            }
        }
    }
}

