namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    public class GetMemberGradesHandler : IHttpHandler
    {
        private void GetCouponInfo(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            int result = 0;
            if (!int.TryParse(context.Request["id"], out result))
            {
                builder.Append("\"Status\":\"0\"}");
                context.Response.Write(builder.ToString());
            }
            CouponInfo coupon = CouponHelper.GetCoupon(result);
            if (coupon == null)
            {
                builder.Append("\"Status\":\"1\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                var type = new {
                    Status = 2,
                    Count = coupon.StockNum - coupon.ReceiveNum,
                    BeginTime = coupon.BeginDate,
                    EndTime = coupon.EndDate
                };
                IsoDateTimeConverter converter = new IsoDateTimeConverter {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                };
                context.Response.Write(JsonConvert.SerializeObject(type, Formatting.Indented, new JsonConverter[] { converter }));
            }
        }

        private void GetCouponType(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            IList<CouponTypes> list = new List<CouponTypes>();
            foreach (int num in Enum.GetValues(typeof(CouponType)))
            {
                CouponTypes item = new CouponTypes {
                    id = num,
                    Name = ((CouponType) num).ToString()
                };
                list.Add(item);
            }
            var type = new {
                type = "success",
                data = list
            };
            string s = JsonConvert.SerializeObject(type);
            context.Response.Write(s);
        }

        private void GetMemberGrade(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                new StringBuilder();
                IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
                List<SimpleGradeClass> list2 = new List<SimpleGradeClass>();
                if (memberGrades.Count > 0)
                {
                    foreach (MemberGradeInfo info in memberGrades)
                    {
                        SimpleGradeClass item = new SimpleGradeClass {
                            GradeId = info.GradeId,
                            Name = info.Name
                        };
                        list2.Add(item);
                    }
                }
                var type = new {
                    type = "success",
                    data = list2
                };
                string s = JsonConvert.SerializeObject(type);
                context.Response.Write(s);
            }
            catch (Exception exception)
            {
                context.Response.Write("{\"type\":\"error\",data:\"" + exception.Message + "\"}");
            }
        }

        private void GetMemberGroup(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            int result = 0;
            if (!int.TryParse(context.Request["userId"], out result))
            {
                builder.Append("\"Status\":\"ok\"}");
            }
            else
            {
                builder.Append("\"Status\":\"" + CustomGroupingHelper.GetMemberGroupList(result) + "\"}");
            }
            context.Response.Write(builder);
        }

        private void GetSearchUserCount(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var type = new {
                status = "err",
                msg = "参数错误！"
            };
            string str = context.Request["gId"];
            if (string.IsNullOrWhiteSpace(str))
            {
                context.Response.Write(JsonConvert.SerializeObject(type));
            }
            else
            {
                string str2 = context.Request["rId"];
                if (!string.IsNullOrWhiteSpace(str2))
                {
                    try
                    {
                        int.Parse(str2);
                    }
                    catch (Exception)
                    {
                        context.Response.Write(JsonConvert.SerializeObject(type));
                        return;
                    }
                }
                string str3 = context.Request["bDate"];
                if (!string.IsNullOrWhiteSpace(str3))
                {
                    try
                    {
                        DateTime.Parse(str3);
                    }
                    catch (Exception)
                    {
                        context.Response.Write(JsonConvert.SerializeObject(type));
                        return;
                    }
                }
                string str4 = context.Request["eDate"];
                if (!string.IsNullOrWhiteSpace(str4))
                {
                    try
                    {
                        DateTime.Parse(str4);
                    }
                    catch (Exception)
                    {
                        context.Response.Write(JsonConvert.SerializeObject(type));
                        return;
                    }
                }
                string str5 = context.Request["uType"];
                if (!string.IsNullOrWhiteSpace(str5))
                {
                    string str6 = context.Request["cGroup"];
                    if (string.IsNullOrWhiteSpace(str6))
                    {
                        context.Response.Write(JsonConvert.SerializeObject(type));
                    }
                    else
                    {
                        try
                        {
                            int.Parse(str5);
                        }
                        catch (Exception)
                        {
                            context.Response.Write(JsonConvert.SerializeObject(type));
                            return;
                        }
                        try
                        {
                            int num = CouponHelper.GetMemeberNumBySearch(str, str2, str3, str4, int.Parse(str5), str6);
                            var type2 = new {
                                status = "ok",
                                msg = "",
                                count = num
                            };
                            context.Response.Write(JsonConvert.SerializeObject(type2));
                        }
                        catch (Exception)
                        {
                            context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "获取会员数出错！" }));
                            return;
                        }
                    }
                }
                else
                {
                    context.Response.Write(JsonConvert.SerializeObject(type));
                }
            }
        }

        private void GetStoreIdByStoreName(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string str = context.Request["storeName"];
            if (!string.IsNullOrWhiteSpace(str))
            {
                int num = MemberHelper.IsExiteDistributorNames(str);
                if (num > 0)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { status = "ok", storeId = num }));
                }
                else
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { status = "err", description = "上级店铺名称不存在!" }));
                }
            }
            else
            {
                context.Response.Write(JsonConvert.SerializeObject(new { status = "err", description = "参数错误!" }));
            }
        }

        private void GetUserCustomGroup(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                new StringBuilder();
                IList<CustomGroupingInfo> customGroupingList = CustomGroupingHelper.GetCustomGroupingList();
                List<CustomGroup> list2 = new List<CustomGroup>();
                if (customGroupingList.Count > 0)
                {
                    foreach (CustomGroupingInfo info in customGroupingList)
                    {
                        CustomGroup item = new CustomGroup {
                            id = info.Id,
                            Name = info.GroupName
                        };
                        list2.Add(item);
                    }
                }
                var type = new {
                    type = "success",
                    data = list2
                };
                string s = JsonConvert.SerializeObject(type);
                context.Response.Write(s);
            }
            catch (Exception exception)
            {
                context.Response.Write("{\"type\":\"error\",data:\"" + exception.Message + "\"}");
            }
        }

        private void GetUserCustomGroupAndGrade(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                new StringBuilder();
                IList<CustomGroupingInfo> customGroupingList = CustomGroupingHelper.GetCustomGroupingList();
                List<CustomGroup> list2 = new List<CustomGroup>();
                if (customGroupingList.Count > 0)
                {
                    foreach (CustomGroupingInfo info in customGroupingList)
                    {
                        CustomGroup item = new CustomGroup {
                            id = info.Id,
                            Name = info.GroupName
                        };
                        list2.Add(item);
                    }
                }
                new StringBuilder();
                IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
                List<SimpleGradeClass> list4 = new List<SimpleGradeClass>();
                if (memberGrades.Count > 0)
                {
                    foreach (MemberGradeInfo info2 in memberGrades)
                    {
                        SimpleGradeClass class2 = new SimpleGradeClass {
                            GradeId = info2.GradeId,
                            Name = info2.Name
                        };
                        list4.Add(class2);
                    }
                }
                var type = new {
                    type = "success",
                    data = list2,
                    gradedata = list4
                };
                string s = JsonConvert.SerializeObject(type);
                context.Response.Write(s);
            }
            catch (Exception exception)
            {
                context.Response.Write("{\"type\":\"error\",data:\"" + exception.Message + "\"}");
            }
        }

        private void GetUserIdByNiCh(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string str = context.Request["nich"];
            if (string.IsNullOrWhiteSpace(str))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "参数错误！" }));
            }
            else
            {
                try
                {
                    string[] strArray = str.Split(new char[] { '_' });
                    List<int> values = new List<int>();
                    StringBuilder builder = new StringBuilder();
                    foreach (string str2 in strArray)
                    {
                        int memberIdByUserNameOrNiChen = MemberHelper.GetMemberIdByUserNameOrNiChen(null, str2);
                        if (memberIdByUserNameOrNiChen > 0)
                        {
                            values.Add(memberIdByUserNameOrNiChen);
                        }
                        else if (builder.Length <= 0)
                        {
                            builder.AppendFormat("{0}", str2);
                        }
                        else
                        {
                            builder.AppendFormat(",{0}", str2);
                        }
                    }
                    if (builder.Length > 0)
                    {
                        builder.Append(" 无效！");
                        context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "微信昵称 " + builder.ToString() }));
                    }
                    else
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new { status = "ok", msg = "", ids = string.Join<int>("_", values), count = values.Count }));
                    }
                }
                catch (Exception)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "程序出错！" }));
                }
            }
        }

        private void GetUserIdByUserName(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string str = context.Request["uname"];
            if (string.IsNullOrWhiteSpace(str))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "参数错误！" }));
            }
            else
            {
                try
                {
                    string[] strArray = str.Split(new char[] { '_' });
                    List<int> values = new List<int>();
                    StringBuilder builder = new StringBuilder();
                    foreach (string str2 in strArray)
                    {
                        int memberIdByUserNameOrNiChen = MemberHelper.GetMemberIdByUserNameOrNiChen(str2, null);
                        if (memberIdByUserNameOrNiChen > 0)
                        {
                            values.Add(memberIdByUserNameOrNiChen);
                        }
                        else if (builder.Length <= 0)
                        {
                            builder.AppendFormat("{0}", str2);
                        }
                        else
                        {
                            builder.AppendFormat(",{0}", str2);
                        }
                    }
                    if (builder.Length > 0)
                    {
                        builder.Append(" 无效！");
                        context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "用户名 " + builder.ToString() }));
                    }
                    else
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new { status = "ok", msg = "", ids = string.Join<int>("_", values), count = values.Count }));
                    }
                }
                catch (Exception)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "程序出错！" }));
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (Globals.GetCurrentManagerUserId() <= 0)
            {
                context.Response.Write("{\"type\":\"error\",\"data\":\"请先登录\"}");
                context.Response.End();
            }
            string str = context.Request.QueryString["action"];
            if (string.IsNullOrEmpty(str))
            {
                this.GetMemberGrade(context);
            }
            else
            {
                switch (str.ToLower())
                {
                    case "getmembergrade":
                        this.GetMemberGrade(context);
                        return;

                    case "getcoupontype":
                        this.GetCouponType(context);
                        return;

                    case "getcouponinfo":
                        this.GetCouponInfo(context);
                        return;

                    case "setregisternosendcoupon":
                        this.SetRegisterNoSendCoupon(context);
                        return;

                    case "getstroeidbystorename":
                        this.GetStoreIdByStoreName(context);
                        return;

                    case "getsearchusercount":
                        this.GetSearchUserCount(context);
                        return;

                    case "sendcoupontosearchuser":
                        this.SendCouponToSearchUser(context);
                        return;

                    case "getuseridbynich":
                        this.GetUserIdByNiCh(context);
                        return;

                    case "getuseridbyusername":
                        this.GetUserIdByUserName(context);
                        return;

                    case "sendcoupontousers":
                        this.SendCouponToUser(context);
                        return;

                    case "getusercustomgroup":
                        this.GetUserCustomGroup(context);
                        return;

                    case "getusercustomgroupandgrade":
                        this.GetUserCustomGroupAndGrade(context);
                        return;

                    case "getmembergroup":
                        this.GetMemberGroup(context);
                        return;
                }
            }
        }

        private void SendCouponToSearchUser(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int result = 0;
            if (!int.TryParse(context.Request["couponId"], out result))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "参数错误！" }));
            }
            else
            {
                try
                {
                    if (CouponHelper.SendCouponToMemebers(result))
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new { status = "ok", msg = "发送成功！" }));
                    }
                    else
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "发送失败！" }));
                    }
                }
                catch (Exception)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "发送出错！" }));
                }
            }
        }

        private void SendCouponToUser(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string str = context.Request["ids"];
            if (string.IsNullOrWhiteSpace(str))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "参数错误！" }));
            }
            else
            {
                int result = 0;
                if (!int.TryParse(context.Request["cId"], out result))
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "参数错误！" }));
                }
                else
                {
                    try
                    {
                        if (CouponHelper.SendCouponToMemebers(result, str))
                        {
                            context.Response.Write(JsonConvert.SerializeObject(new { status = "ok", msg = "发送成功！" }));
                        }
                        else
                        {
                            context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "发送失败！" }));
                        }
                    }
                    catch (Exception)
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new { status = "err", msg = "程序出错！" }));
                    }
                }
            }
        }

        private void SetRegisterNoSendCoupon(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.IsRegisterSendCoupon = false;
            try
            {
                SettingsManager.Save(masterSettings);
                builder.Append("\"Status\":\"ok\"}");
            }
            catch (Exception)
            {
                builder.Append("\"Status\":\"err\"}");
            }
            context.Response.Write(builder);
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

