namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_OnlineServiceConfig : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            StringBuilder builder = new StringBuilder();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(currentMember.GradeId);
                OrderInfo userLastOrder = MemberProcessor.GetUserLastOrder(currentMember.UserId);
                string str = !string.IsNullOrEmpty(currentMember.UserBindName) ? currentMember.UserBindName : currentMember.UserName;
                string str2 = !string.IsNullOrEmpty(currentMember.OpenId) ? currentMember.UserName : string.Empty;
                int port = context.Request.Url.Port;
                string str3 = (port == 80) ? "" : (":" + port.ToString());
                string text1 = "http://" + context.Request.Url.Host + str3 + Globals.ApplicationPath + "/Admin/member/managemembers.aspx?Username=" + currentMember.UserName + "&pageSize=10";
                string str4 = currentMember.UserBindName + "【" + ((memberGrade != null) ? memberGrade.Name : "普通会员") + "】";
                string str5 = currentMember.OrderNumber.ToString() + "单（￥" + currentMember.Expenditure.ToString("F2") + "）";
                string str6 = (userLastOrder != null) ? userLastOrder.OrderDate.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                string str7 = string.Empty;
                string str8 = string.Empty;
                string str9 = string.Empty;
                string str10 = string.Empty;
                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId(false));
                if (userIdDistributors != null)
                {
                    DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(userIdDistributors.DistriGradeId);
                    string str11 = "0.00";
                    DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(userIdDistributors.UserId);
                    if (distributorInfo != null)
                    {
                        str11 = distributorInfo.ReferralBlance.ToString("F2");
                    }
                    string str12 = "0";
                    string str13 = "0";
                    DataTable distributorsSubStoreNum = VShopHelper.GetDistributorsSubStoreNum(userIdDistributors.UserId);
                    if ((distributorsSubStoreNum != null) || (distributorsSubStoreNum.Rows.Count > 0))
                    {
                        str12 = distributorsSubStoreNum.Rows[0]["firstV"].ToString();
                        str13 = distributorsSubStoreNum.Rows[0]["secondV"].ToString();
                    }
                    string text2 = "http://" + context.Request.Url.Host + str3 + Globals.ApplicationPath + "/Admin/Fenxiao/distributorlist.aspx?MicroSignal=" + currentMember.UserName + "&Status=0&pageSize=10";
                    str7 = userIdDistributors.StoreName + "【" + distributorGradeInfo.Name + "】";
                    str8 = "￥" + userIdDistributors.OrdersTotal.ToString("F2");
                    str9 = "￥" + str11 + "（待提现￥" + userIdDistributors.ReferralBlance.ToString("F2") + "，已提现￥" + userIdDistributors.ReferralRequestBalance.ToString("F2") + "）";
                    str10 = "一级分店数" + str12 + "，二级分店数" + str13;
                }
                builder.Append("<script>");
                builder.Append("var mechatMetadataInter = setInterval(function() {");
                builder.Append("if (window.mechatMetadata) {");
                builder.Append("clearInterval(mechatMetadataInter);");
                builder.Append("window.mechatMetadata({");
                builder.AppendFormat("appUserName: '{0}',", str);
                builder.AppendFormat("appNickName: '{0}',", currentMember.UserName);
                builder.AppendFormat("realName: '{0}',", currentMember.RealName);
                builder.AppendFormat("avatar: '{0}',", currentMember.UserHead);
                builder.AppendFormat("tel: '{0}',", currentMember.CellPhone);
                builder.AppendFormat("email: '{0}',", currentMember.Email);
                builder.AppendFormat("QQ: '{0}',", currentMember.QQ);
                builder.AppendFormat("weibo: '',", new object[0]);
                builder.AppendFormat("weixin: '{0}',", str2);
                builder.AppendFormat("address: '{0}',", currentMember.Address);
                builder.Append("extraParams: {");
                builder.AppendFormat("'会员帐号': '{0}',", str4);
                builder.AppendFormat("'会员订单': '{0}',", str5);
                builder.AppendFormat("'会员积分': '{0}',", currentMember.Points.ToString());
                builder.AppendFormat("'最近购买': '{0}',", str6);
                builder.AppendFormat("'店铺名称': '{0}',", str7);
                builder.AppendFormat("'销售额': '{0}',", str8);
                builder.AppendFormat("'佣金信息': '{0}',", str9);
                builder.AppendFormat("'分店数量': '{0}'", str10);
                builder.Append("}");
                builder.Append("});");
                builder.Append("}");
                builder.Append("}, 500);");
                builder.Append("</script>");
            }
            context.Response.Write(builder.ToString() + " ");
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

