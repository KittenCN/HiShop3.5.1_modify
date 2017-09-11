namespace Hidistro.UI.Web.API
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Promotions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_GetActivityTopics : IHttpHandler
    {
        protected string GetLimit(object limitEveryDay, object maximumDailyLimit)
        {
            StringBuilder builder = new StringBuilder();
            int num = (int) limitEveryDay;
            int num2 = (int) maximumDailyLimit;
            if ((num == 0) && (num2 == 0))
            {
                builder.Append("不限次");
            }
            else
            {
                if (num != 0)
                {
                    builder.AppendFormat("每天参与{0}次", num);
                }
                builder.Append("<br/>");
                if (num2 != 0)
                {
                    builder.AppendFormat("参与上限{0}次", num2);
                }
            }
            return builder.ToString();
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int result = 0;
            int.TryParse(context.Request.Params["act"], out result);
            string types = "0";
            if ((result > 0) && (result < 4))
            {
                types = result.ToString();
            }
            else if (result == 4)
            {
                types = "4,5";
            }
            DataTable activityTopics = ActivityHelper.GetActivityTopics(types);
            activityTopics.Columns.Add("hasImage");
            activityTopics.Columns.Add("NewMemberGrades");
            activityTopics.Columns.Add("Url");
            activityTopics.Columns.Add("Limit");
            activityTopics.Columns.Add("Discount");
            activityTopics.Columns.Add("Description");
            activityTopics.Columns.Add("Point");
            activityTopics.Columns.Add("CouponMoeny");
            activityTopics.Columns.Add("Product");
            foreach (DataRow row in activityTopics.Rows)
            {
                int gameId = int.Parse(row["Id"].ToString());
                int num3 = int.Parse(row["ActivityType"].ToString());
                string str2 = row["MemberGrades"].ToString();
                string str3 = row["DefualtGroup"].ToString();
                string str4 = row["CustomGroup"].ToString();
                if (((num3 == 2) || (num3 == 4)) || (num3 == 5))
                {
                    row["hasImage"] = "none";
                }
                else
                {
                    row["hasImage"] = "''";
                }
                if (((str2 == "0") || (str3 == "0")) || (str4 == "0"))
                {
                    row["NewMemberGrades"] = "全部会员";
                }
                else
                {
                    row["NewMemberGrades"] = "部分会员";
                }
                if (num3 == 1)
                {
                    row["Url"] = "/ExchangeList.aspx?id=" + gameId;
                }
                else
                {
                    switch (num3)
                    {
                        case 4:
                        {
                            row["Url"] = "/BeginVote.aspx?voteId=" + gameId;
                            VoteInfo vote = VoteHelper.GetVote((long) gameId);
                            if (vote != null)
                            {
                                row["Description"] = vote.Description;
                            }
                            break;
                        }
                        case 5:
                        {
                            GameInfo modelByGameId = GameHelper.GetModelByGameId(gameId);
                            if (modelByGameId != null)
                            {
                                row["Url"] = modelByGameId.GameUrl;
                                row["Limit"] = this.GetLimit(modelByGameId.LimitEveryDay, modelByGameId.MaximumDailyLimit);
                                row["Point"] = modelByGameId.NeedPoint.ToString();
                                row["Description"] = modelByGameId.Description;
                            }
                            break;
                        }
                        case 3:
                        {
                            row["Url"] = "/VShop/CouponDetails.aspx?CouponId=" + gameId;
                            CouponInfo coupon = CouponHelper.GetCoupon(gameId);
                            if (coupon != null)
                            {
                                row["CouponMoeny"] = coupon.CouponValue.ToString();
                                if (coupon.ConditionValue > 0M)
                                {
                                    row["Limit"] = "满" + coupon.ConditionValue.ToString() + "元可用";
                                }
                                else
                                {
                                    row["Limit"] = "不限制";
                                }
                            }
                            break;
                        }
                        case 2:
                        {
                            row["Url"] = "";
                            ActivityInfo act = ActivityHelper.GetAct(gameId);
                            if (act != null)
                            {
                                row["Limit"] = "每人参与" + act.attendTime.ToString() + "次";
                                int meetType = act.MeetType;
                                DataTable table2 = ActivityHelper.GetActivities_Detail(gameId);
                                string str5 = string.Empty;
                                string str6 = "";
                                if (act.attendType == 0)
                                {
                                    foreach (DataRow row2 in table2.Rows)
                                    {
                                        if (meetType == 1)
                                        {
                                            str6 = str6 + "满" + row2["MeetNumber"].ToString() + "件";
                                        }
                                        else
                                        {
                                            str6 = str6 + "满" + row2["MeetMoney"].ToString() + "元";
                                        }
                                        if (decimal.Parse(row2["ReductionMoney"].ToString()) != 0M)
                                        {
                                            str6 = str6 + "，减" + row2["ReductionMoney"].ToString() + "元";
                                        }
                                        if (bool.Parse(row2["bFreeShipping"].ToString()))
                                        {
                                            str6 = str6 + "，免邮";
                                        }
                                        if (int.Parse(row2["Integral"].ToString()) != 0)
                                        {
                                            str6 = str6 + "，送" + row2["Integral"].ToString() + "积分";
                                        }
                                        if (int.Parse(row2["CouponId"].ToString()) != 0)
                                        {
                                            str6 = str6 + "，送优惠券";
                                        }
                                    }
                                    str5 = str5 + str6;
                                }
                                else
                                {
                                    str5 = "多级优惠（每层级优惠不累积叠加）<br/>";
                                    int num5 = 0;
                                    foreach (DataRow row3 in table2.Rows)
                                    {
                                        num5++;
                                        str6 = str6 + "层级" + num5.ToString() + "：";
                                        if (meetType == 1)
                                        {
                                            str6 = str6 + "满" + row3["MeetNumber"].ToString() + "件";
                                        }
                                        else
                                        {
                                            str6 = str6 + "满" + row3["MeetMoney"].ToString() + "元";
                                        }
                                        if (decimal.Parse(row3["ReductionMoney"].ToString()) != 0M)
                                        {
                                            str6 = str6 + "，减" + row3["ReductionMoney"].ToString() + "元";
                                        }
                                        if (bool.Parse(row3["bFreeShipping"].ToString()))
                                        {
                                            str6 = str6 + "，免邮";
                                        }
                                        if (int.Parse(row3["Integral"].ToString()) != 0)
                                        {
                                            str6 = str6 + "，送" + row3["Integral"].ToString() + "积分";
                                        }
                                        if (int.Parse(row3["CouponId"].ToString()) != 0)
                                        {
                                            str6 = str6 + "，送优惠券";
                                        }
                                        str6 = str6 + "<br/>";
                                    }
                                    str5 = str5 + str6;
                                }
                                row["Discount"] = str5;
                                if (act.isAllProduct)
                                {
                                    row["Product"] = "全部商品";
                                }
                                else
                                {
                                    string str7 = string.Empty;
                                    foreach (DataRow row4 in ActivityHelper.QueryProducts(gameId).Rows)
                                    {
                                        if (row4["status"].ToString() == "0")
                                        {
                                            str7 = str7 + row4["ProductID"].ToString() + "_";
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(str7))
                                    {
                                        row["Url"] = "/ProductList.aspx?pIds=" + str7.TrimEnd(new char[] { '_' });
                                    }
                                    row["Product"] = "部分商品";
                                }
                            }
                            break;
                        }
                    }
                }
            }
            IsoDateTimeConverter converter = new IsoDateTimeConverter {
                DateTimeFormat = "yyyy-MM-dd HH:mm"
            };
            string s = JsonConvert.SerializeObject(activityTopics, Formatting.Indented, new JsonConverter[] { converter });
            context.Response.Write(s);
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

