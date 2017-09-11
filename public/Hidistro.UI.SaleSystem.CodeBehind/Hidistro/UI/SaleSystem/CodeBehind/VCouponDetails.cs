namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;

    public class VCouponDetails : VMemberTemplatedWebControl
    {
        private int couponId;
        private HtmlInputHidden hideDesc;
        private HtmlInputHidden hideImgUrl;
        private string htmlTitle = string.Empty;
        private HiLiteral lbBeginTime;
        private HiLiteral lbCounponValue1;
        private HiLiteral lbCouponDateTime;
        private HiLiteral lbCouponName;
        private HiLiteral lbCouponTj;
        private HiLiteral lbCouponTj1;
        private HiLiteral lbCouponUseCount;
        private HiLiteral lbCouponUsedShopBook;
        private HiLiteral lbCouponValue;
        private HiLiteral lbEndTime;
        private HiLiteral lbLeftCount;

        protected override void AttachChildControls()
        {
            this.lbCouponValue = (HiLiteral) this.FindControl("lbCouponValue");
            this.lbCouponTj = (HiLiteral) this.FindControl("lbCouponTj");
            this.lbBeginTime = (HiLiteral) this.FindControl("lbBeginTime");
            this.lbEndTime = (HiLiteral) this.FindControl("lbEndTime");
            this.lbCounponValue1 = (HiLiteral) this.FindControl("lbCounponValue1");
            this.lbCouponTj1 = (HiLiteral) this.FindControl("lbCouponTj1");
            this.lbLeftCount = (HiLiteral) this.FindControl("lbLeftCount");
            this.lbCouponUseCount = (HiLiteral) this.FindControl("lbCouponUseCount");
            this.lbCouponUsedShopBook = (HiLiteral) this.FindControl("lbCouponUsedShopBook");
            this.lbCouponDateTime = (HiLiteral) this.FindControl("lbCouponDateTime");
            this.lbCouponName = (HiLiteral) this.FindControl("lbCouponName");
            this.hideImgUrl = (HtmlInputHidden) this.FindControl("hideImgUrl");
            this.hideDesc = (HtmlInputHidden) this.FindControl("hideDesc");
            CouponInfo coupon = CouponHelper.GetCoupon(this.couponId);
            if (coupon != null)
            {
                Uri url = HttpContext.Current.Request.Url;
                string str = url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
                this.hideImgUrl.Value = str + "/Utility/pics/coupon.png";
                this.hideDesc.Value = "面值:" + coupon.CouponValue.ToString("n0") + "元,活动时间：" + string.Format("{0:yyyy-MM-dd}~{1:yyyy-MM-dd}", coupon.BeginDate, coupon.EndDate);
                this.htmlTitle = coupon.CouponName;
                this.lbCouponValue.Text = coupon.CouponValue.ToString("n0");
                this.lbCounponValue1.Text = coupon.CouponValue.ToString("n0");
                this.lbBeginTime.Text = coupon.BeginDate.ToString("yyyy-MM-dd HH:mm:ss");
                this.lbEndTime.Text = coupon.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
                this.lbLeftCount.Text = (coupon.StockNum - coupon.ReceiveNum).ToString();
                this.lbCouponUseCount.Text = coupon.maxReceivNum.ToString();
                this.lbCouponName.Text = coupon.CouponName;
                if (coupon.IsAllProduct)
                {
                    this.lbCouponUsedShopBook.Text = "适应任意商品";
                }
                else
                {
                    this.lbCouponUsedShopBook.Text = string.Format("部分商品参与<a href=\"../productList.aspx?pIds={0}\">查看活动商品</a>", CouponHelper.GetCouponProductIds(coupon.CouponId));
                }
                string str2 = "";
                if (coupon.ConditionValue > 0M)
                {
                    str2 = string.Format("订单满{0:n0}", coupon.ConditionValue);
                }
                else
                {
                    str2 = "直";
                }
                str2 = string.Format("{0}减{1:n0}", str2, coupon.CouponValue);
                this.lbCouponTj.Text = str2;
                this.lbCouponTj1.Text = str2;
                this.lbCouponDateTime.Text = string.Format("{0:yyyy-MM-dd}~{1:yyyy-MM-dd}", coupon.BeginDate, coupon.EndDate);
            }
            else
            {
                base.GotoResourceNotFound("");
            }
            PageTitle.AddSiteNameTitle(this.htmlTitle);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VCouponDetails.html";
            }
            try
            {
                this.couponId = int.Parse(this.Page.Request.QueryString["CouponId"]);
            }
            catch (Exception)
            {
                base.GotoResourceNotFound("参数错误！");
            }
            base.OnInit(e);
        }
    }
}

