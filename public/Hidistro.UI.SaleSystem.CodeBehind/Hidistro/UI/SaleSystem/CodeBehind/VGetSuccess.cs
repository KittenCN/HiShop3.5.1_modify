namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ControlPanel.Promotions;
    using global::ControlPanel.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VGetSuccess : VshopTemplatedWebControl
    {
        private Panel divError;
        private Panel divNoLogin;
        private Panel divNoNum;
        private Panel divSuccess;
        private HtmlInputHidden hdCondition;
        private HyperLink hlinkLogin;
        private Literal ltErrorMessage;
        private Literal ltExpiryTime;
        private Literal ltGetTotal;
        private Literal ltOrderAmountCanUse;
        private Literal ltRedPagerActivityName;
        private Literal ltRedPagerActivityNameForOrders;
        private Literal ltRedPagerLimit;

        protected override void AttachChildControls()
        {
            string s = HttpContext.Current.Request.QueryString.Get("m");
            string str2 = HttpContext.Current.Request.QueryString.Get("type");
            this.ltGetTotal = (Literal) this.FindControl("ltGetTotal");
            this.ltOrderAmountCanUse = (Literal) this.FindControl("ltOrderAmountCanUse");
            this.ltExpiryTime = (Literal) this.FindControl("ltExpiryTime");
            this.ltRedPagerActivityName = (Literal) this.FindControl("ltRedPagerActivityName");
            this.ltRedPagerActivityNameForOrders = (Literal) this.FindControl("ltRedPagerActivityNameForOrders");
            this.ltRedPagerLimit = (Literal) this.FindControl("ltRedPagerLimit");
            this.ltErrorMessage = (Literal) this.FindControl("ltErrorMessage");
            this.divNoLogin = (Panel) this.FindControl("divNoLogin");
            this.divNoNum = (Panel) this.FindControl("divNoNum");
            this.divSuccess = (Panel) this.FindControl("divSuccess");
            this.divError = (Panel) this.FindControl("divError");
            this.hdCondition = (HtmlInputHidden) this.FindControl("hdCondition");
            this.hlinkLogin = (HyperLink) this.FindControl("hlinkLogin");
            switch (str2)
            {
                case "1":
                case "5":
                {
                    int result = 0;
                    int.TryParse(s, out result);
                    if (result > 0)
                    {
                        int num2 = 0;
                        int.TryParse(HttpContext.Current.Request["id"], out num2);
                        ShareActivityInfo act = ShareActHelper.GetAct(num2);
                        if (act != null)
                        {
                            CouponInfo coupon = CouponHelper.GetCoupon(act.CouponId);
                            this.ltGetTotal.Text = coupon.CouponValue.ToString("F2").Replace(".00", "");
                            this.ltOrderAmountCanUse.Text = coupon.ConditionValue.ToString("F2").Replace(".00", "");
                            this.hdCondition.SetWhenIsNotNull(coupon.ConditionValue.ToString("F2").Replace(".00", ""));
                            this.ltExpiryTime.Text = coupon.EndDate.ToString("yyyy-MM-dd");
                            if (str2 == "5")
                            {
                                this.ltRedPagerActivityName.Text = "该券已经到你的钱包了</div><div class='get-red-explain'><a href='/Vshop/MyCouponLists.aspx'>点击查看</a>";
                            }
                            else
                            {
                                this.ltRedPagerActivityName.Text = coupon.CouponName ?? "";
                            }
                            if (coupon.IsAllProduct)
                            {
                                this.ltRedPagerLimit.Text = "该券可用于任意商品的抵扣";
                            }
                            else
                            {
                                string couponProductIds = CouponHelper.GetCouponProductIds(act.CouponId);
                                this.ltRedPagerLimit.Text = "该券可用于部分商品的抵扣</div><div class='get-red-explain'><a href='/ProductList.aspx?pIds='" + couponProductIds + ">查看商品</a>";
                            }
                            this.divSuccess.Visible = true;
                        }
                    }
                    PageTitle.AddSiteNameTitle("成功获取优惠券");
                    return;
                }
                default:
                {
                    string str5 = str2;
                    if (str5 != null)
                    {
                        if (!(str5 == "-1"))
                        {
                            if (str5 == "-2")
                            {
                                this.ltErrorMessage.Text = s;
                                this.divError.Visible = true;
                                break;
                            }
                            if (str5 == "-4")
                            {
                                this.divNoLogin.Visible = true;
                                break;
                            }
                            if (str5 == "-3")
                            {
                                this.divNoNum.Visible = true;
                                break;
                            }
                        }
                        else
                        {
                            int num3 = 0;
                            int.TryParse(HttpContext.Current.Request["id"], out num3);
                            ShareActivityInfo info3 = ShareActHelper.GetAct(num3);
                            if (info3 != null)
                            {
                                CouponInfo info4 = CouponHelper.GetCoupon(info3.CouponId);
                                if (info4 != null)
                                {
                                    this.ltRedPagerActivityNameForOrders.Text = info4.CouponName;
                                    string str4 = string.Concat(new object[] { Globals.GetWebUrlStart(), "/Vshop/GetRedPager.aspx?id=", num3.ToString(), "&userid=", Globals.GetCurrentMemberUserId(false), "&ReferralId=", Globals.GetCurrentDistributorId() });
                                    this.hlinkLogin.NavigateUrl = "/UserLogining.aspx?returnUrl=" + HttpContext.Current.Server.UrlEncode(str4);
                                    this.divNoLogin.Visible = true;
                                }
                                else
                                {
                                    HttpContext.Current.Response.Redirect("/default.aspx");
                                    HttpContext.Current.Response.End();
                                }
                            }
                            else
                            {
                                HttpContext.Current.Response.Redirect("/default.aspx");
                                HttpContext.Current.Response.End();
                            }
                        }
                    }
                    break;
                }
            }
            PageTitle.AddSiteNameTitle("获取优惠券");
        }

        public string getopenid()
        {
            return "";
        }

        private string GetResponseResult(string url)
        {
            using (HttpWebResponse response = (HttpWebResponse) WebRequest.Create(url).GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VGetSuccess.html";
            }
            base.OnInit(e);
        }
    }
}

