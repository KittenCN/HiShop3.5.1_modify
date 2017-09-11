namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class SaveActivityHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (Globals.GetCurrentManagerUserId() <= 0)
            {
                context.Response.Write("请先登录");
                context.Response.End();
            }
            string str = Globals.RequestFormStr("action");
            if (!string.IsNullOrEmpty(str))
            {
                if (str == "End")
                {
                    int aid = Globals.RequestFormNum("delId");
                    if (aid > 0)
                    {
                        if (ActivityHelper.EndAct(aid))
                        {
                            context.Response.Write("{\"state\":\"true\",\"msg\":\"活动成功结束\"}");
                        }
                        else
                        {
                            context.Response.Write("{\"state\":\"false\",\"msg\":\"活动结束失败\"}");
                        }
                    }
                    else
                    {
                        context.Response.Write("{\"state\":\"false\",\"msg\":\"参数不正确\"}");
                    }
                }
            }
            else
            {
                try
                {
                    int id = int.Parse(context.Request["id"].ToString());
                    string str2 = context.Request["name"].ToString();
                    string val = context.Request["begin"].ToString();
                    string str4 = context.Request["end"].ToString();
                    string str5 = context.Request["memberlvl"].ToString();
                    string str6 = context.Request["defualtGroup"].ToString();
                    string str7 = context.Request["customGroup"].ToString();
                    string str8 = context.Request["maxNum"].ToString();
                    string str9 = context.Request["type"].ToString();
                    string str10 = context.Request["attendType"].ToString();
                    string str11 = context.Request["meetType"].ToString();
                    int i = 0;
                    int num4 = 0;
                    DateTime now = DateTime.Now;
                    DateTime time2 = DateTime.Now;
                    int num5 = 0;
                    int num6 = 0;
                    if (str2.Length > 30)
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"活动名称不能超过30个字符\"}");
                    }
                    else if (!val.bDate(ref now))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的开始时间\"}");
                    }
                    else if (!str4.bDate(ref time2))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的结束时间\"}");
                    }
                    else if (time2 < now)
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"结束时间不能早于开始时间\"}");
                    }
                    else if ((string.IsNullOrEmpty(str5) && string.IsNullOrEmpty(str6)) && string.IsNullOrEmpty(str7))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请选择适用会员等级\"}");
                    }
                    else if (!str8.bInt(ref num5))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请选择参与次数\"}");
                    }
                    else if (!str9.bInt(ref num6))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请选择参与商品类型\"}");
                    }
                    else if (!str10.bInt(ref i))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请选择优惠类型\"}");
                    }
                    else if (!str11.bInt(ref num4))
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"请选择优惠条件\"}");
                    }
                    else
                    {
                        List<ActivityDetailInfo> list = new List<ActivityDetailInfo>();
                        JArray array = (JArray) JsonConvert.DeserializeObject(context.Request.Form["stk"].ToString());
                        if (array.Count > 1)
                        {
                            for (int j = 0; j < (array.Count - 1); j++)
                            {
                                JToken token = array[j]["meet"];
                                for (int k = j + 1; k < array.Count; k++)
                                {
                                    if (array[k]["meet"] == token)
                                    {
                                        context.Response.Write("{\"type\":\"error\",\"data\":\"多级优惠的各级满足金额不能相同\"}");
                                        return;
                                    }
                                }
                            }
                        }
                        if (array.Count > 0)
                        {
                            for (int m = 0; m < array.Count; m++)
                            {
                                ActivityDetailInfo item = new ActivityDetailInfo();
                                string str12 = array[m]["meet"].ToString();
                                string str13 = array[m]["meetNumber"].ToString();
                                string str14 = array[m]["redus"].ToString();
                                string str15 = array[m]["free"].ToString();
                                string str16 = array[m]["point"].ToString();
                                string str17 = array[m]["coupon"].ToString();
                                decimal num10 = 0M;
                                int num11 = 0;
                                decimal num12 = 0M;
                                bool flag = false;
                                int num13 = 0;
                                int num14 = 0;
                                int num15 = 0;
                                if (!str13.bInt(ref num11))
                                {
                                    int num17 = m + 1;
                                    context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num17.ToString() + "级优惠满足次数输入错误！\"}");
                                    return;
                                }
                                if (!str12.bDecimal(ref num10))
                                {
                                    int num18 = m + 1;
                                    context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num18.ToString() + "级优惠满足金额输入错误！\"}");
                                    return;
                                }
                                if (!str14.bDecimal(ref num12))
                                {
                                    int num19 = m + 1;
                                    context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num19.ToString() + "级优惠减免金额输入错误！\"}");
                                    return;
                                }
                                if (!str15.bInt(ref num15))
                                {
                                    int num20 = m + 1;
                                    context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num20.ToString() + "级优惠免邮选择错误！\"}");
                                    return;
                                }
                                flag = num15 != 0;
                                if (!str16.bInt(ref num13))
                                {
                                    int num21 = m + 1;
                                    context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num21.ToString() + "级优惠积分输入错误！\"}");
                                    return;
                                }
                                if (!str16.bInt(ref num13))
                                {
                                    int num22 = m + 1;
                                    context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num22.ToString() + "级优惠积分输入错误！\"}");
                                    return;
                                }
                                if (!str17.bInt(ref num14))
                                {
                                    int num23 = m + 1;
                                    context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num23.ToString() + "级优惠优惠券选择错误！\"}");
                                    return;
                                }
                                if ((num11 == 0) && (num12 > num10))
                                {
                                    int num24 = m + 1;
                                    context.Response.Write("{\"type\":\"error\",\"data\":\"第" + num24.ToString() + "级优惠减免金额不能大于满足金额！\"}");
                                    return;
                                }
                                item.ActivitiesId = 0;
                                item.bFreeShipping = flag;
                                item.CouponId = num14;
                                item.MeetMoney = num10;
                                item.MeetNumber = num11;
                                item.ReductionMoney = num12;
                                item.Integral = num13;
                                list.Add(item);
                            }
                        }
                        ActivityInfo act = new ActivityInfo();
                        if (id != 0)
                        {
                            act = ActivityHelper.GetAct(id);
                            if (act == null)
                            {
                                context.Response.Write("{\"type\":\"error\",\"data\":\"没有找到这个活动\"}");
                                return;
                            }
                        }
                        act.ActivitiesName = str2;
                        act.EndTime = time2.Date.AddDays(1.0).AddSeconds(-1.0);
                        act.StartTime = now.Date;
                        act.attendTime = num5;
                        act.attendType = i;
                        act.isAllProduct = num6 == 0;
                        act.MemberGrades = str5;
                        act.DefualtGroup = str6;
                        act.CustomGroup = str7;
                        act.Type = 0;
                        act.MeetMoney = 0M;
                        act.ReductionMoney = 0M;
                        act.Details = list;
                        act.MeetType = num4;
                        string msg = "";
                        int activitiesId = 0;
                        if (id == 0)
                        {
                            activitiesId = ActivityHelper.Create(act, ref msg);
                        }
                        else
                        {
                            activitiesId = act.ActivitiesId;
                            if (!ActivityHelper.Update(act, ref msg))
                            {
                                activitiesId = 0;
                            }
                        }
                        if (activitiesId > 0)
                        {
                            context.Response.Write("{\"type\":\"success\",\"data\":\"" + activitiesId.ToString() + "\"}");
                        }
                        else
                        {
                            context.Response.Write("{\"type\":\"error\",\"data\":\"" + msg + "\"}");
                        }
                    }
                }
                catch (Exception exception)
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"" + exception.Message + "\"}");
                }
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

