namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Web;

    public class SaveCouponDataHandler : IHttpHandler
    {
        private bool bDate(string val, bool flag, ref DateTime i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return DateTime.TryParse(val, out i);
        }

        private bool bDecimal(string val, ref decimal i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            i = 0M;
            if (val.Contains("-"))
            {
                return false;
            }
            return decimal.TryParse(val, out i);
        }

        private bool bInt(string val, ref int i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            i = 0;
            if (val.Contains(".") || val.Contains("-"))
            {
                return false;
            }
            return int.TryParse(val, out i);
        }

        private string GetErrMsg(string msg)
        {
            string str2;
            if (((str2 = msg) != null) && (str2 == "DuplicateName"))
            {
                return "优惠券名称重复";
            }
            return "写数据库异常";
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string str = context.Request["name"].ToString().Trim();
                string val = context.Request["val"].ToString().Trim();
                string str3 = context.Request["condition"].ToString().Trim();
                string str4 = context.Request["begin"].ToString().Trim();
                string str5 = context.Request["end"].ToString().Trim();
                string str6 = context.Request["total"].ToString().Trim();
                string str7 = context.Request["isAllMember"].ToString().Trim();
                string str8 = context.Request["memberlvl"].ToString().Trim();
                string str9 = context.Request["defualtGroup"].ToString().Trim();
                string str10 = context.Request["customGroup"].ToString().Trim();
                string str11 = context.Request["maxNum"].ToString().Trim();
                string str12 = context.Request["type"].ToString().Trim();
                string str13 = context.Request["couponType"].ToString().Trim();
                string couponName = "";
                decimal i = 0M;
                decimal num2 = 0M;
                DateTime now = DateTime.Now;
                DateTime time2 = DateTime.Now;
                int num3 = 0;
                bool flag = false;
                string str15 = "";
                int num4 = 1;
                int num5 = 0;
                if (string.IsNullOrEmpty(str) || (str.Length > 20))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的优惠券名称\"}");
                }
                else
                {
                    couponName = str;
                    if (!this.bDecimal(val, ref i))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的优惠券面值\"}");
                    }
                    else if (!this.bDecimal(str3, ref num2))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的优惠券适用最大满足额\"}");
                    }
                    else if (!this.bDate(str4, true, ref now))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的生效日期\"}");
                    }
                    else if (!this.bDate(str5, false, ref time2))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的失效日期\"}");
                    }
                    else if (!this.bInt(str6, ref num3))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的优惠券发放量\"}");
                    }
                    else
                    {
                        flag = bool.Parse(str7);
                        if (!flag)
                        {
                            if ((string.IsNullOrEmpty(str8) && string.IsNullOrEmpty(str9)) && string.IsNullOrEmpty(str10))
                            {
                                context.Response.Write("{\"type\":\"error\",\"data\":\"请选择正确的会员等级\"}");
                                return;
                            }
                            str15 = str8;
                        }
                        if (!this.bInt(str11, ref num4))
                        {
                            context.Response.Write("{\"type\":\"error\",\"data\":\"请选择正确的优惠券最大领取张数\"}");
                        }
                        else
                        {
                            this.bInt(str12, ref num5);
                            if ((num2 < i) && (num2 != 0M))
                            {
                                context.Response.Write("{\"type\":\"error\",\"data\":\"优惠券面值不能大于满足金额\"}");
                            }
                            else if (time2 < now)
                            {
                                context.Response.Write("{\"type\":\"error\",\"data\":\"优惠券失效日期不能早于生效日期\"}");
                            }
                            else
                            {
                                CouponInfo coupon = new CouponInfo {
                                    CouponName = couponName,
                                    CouponValue = i,
                                    ConditionValue = num2,
                                    BeginDate = now,
                                    EndDate = time2,
                                    StockNum = num3,
                                    IsAllProduct = (num5 == 0) ? true : false
                                };
                                if (flag)
                                {
                                    coupon.MemberGrades = "0";
                                    coupon.DefualtGroup = "0";
                                    coupon.CustomGroup = "0";
                                }
                                else
                                {
                                    coupon.MemberGrades = str15;
                                    coupon.DefualtGroup = str9;
                                    coupon.CustomGroup = str10;
                                }
                                coupon.maxReceivNum = num4;
                                coupon.CouponTypes = str13;
                                CouponActionStatus status = CouponHelper.CreateCoupon(coupon);
                                if (status == CouponActionStatus.Success)
                                {
                                    coupon = CouponHelper.GetCoupon(couponName);
                                    if (coupon != null)
                                    {
                                        context.Response.Write("{\"type\":\"success\",\"data\":\"" + coupon.CouponId.ToString() + "\"}");
                                    }
                                    else
                                    {
                                        context.Response.Write("{\"type\":\"error\",\"data\":\"写数据库异常\"}");
                                    }
                                }
                                else
                                {
                                    context.Response.Write("{\"type\":\"error\",\"data\":\"" + this.GetErrMsg(status.ToString()) + "\"}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                context.Response.Write("{\"type\":\"error\",\"data\":\"" + exception.Message + "\"}");
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

