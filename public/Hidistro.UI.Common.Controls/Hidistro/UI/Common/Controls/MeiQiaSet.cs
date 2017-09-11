namespace Hidistro.UI.Common.Controls
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class MeiQiaSet : Literal
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.Text = "";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            string str = string.Empty;
            string str2 = string.Empty;
            if (masterSettings.EnableSaleService)
            {
                if (!string.IsNullOrEmpty(masterSettings.MeiQiaEntId))
                {
                    flag = true;
                    string str3 = "name: '游客'";
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    if (currentMember == null)
                    {
                        string getCurrentWXOpenId = Globals.GetCurrentWXOpenId;
                        if (!string.IsNullOrEmpty(getCurrentWXOpenId))
                        {
                            currentMember = MemberProcessor.GetOpenIdMember(getCurrentWXOpenId, "wx");
                        }
                    }
                    if (currentMember != null)
                    {
                        StringBuilder builder2 = new StringBuilder();
                        builder2.Append("name: '" + (string.IsNullOrEmpty(currentMember.RealName) ? currentMember.UserName : currentMember.RealName) + "'");
                        if (!string.IsNullOrEmpty(currentMember.Email))
                        {
                            builder2.Append(",email: '" + currentMember.Email + "'");
                        }
                        if (!string.IsNullOrEmpty(currentMember.Address))
                        {
                            builder2.Append(",address: '" + currentMember.Address.Replace("'", "’") + "'");
                        }
                        if (!string.IsNullOrEmpty(currentMember.CellPhone))
                        {
                            builder2.Append(",tel: '" + currentMember.CellPhone + "'");
                        }
                        if (!string.IsNullOrEmpty(currentMember.QQ))
                        {
                            builder2.Append(",qq: '" + currentMember.QQ + "'");
                        }
                        MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(currentMember.GradeId);
                        builder2.Append(",会员帐号: '" + currentMember.UserBindName + "【" + ((memberGrade != null) ? memberGrade.Name : "普通会员") + "】'");
                        builder2.Append(",注册日期: '" + currentMember.CreateDate.ToString("yyyy-MM-dd") + "'");
                        builder2.Append(",订单量: '" + currentMember.OrderNumber + "'");
                        builder2.Append(",积分: '" + currentMember.Points + "'");
                        if (currentMember.LastOrderDate.HasValue)
                        {
                            builder2.Append(",最近下单: '" + currentMember.LastOrderDate.Value.ToString("yyyy-MM-dd") + "'");
                        }
                        DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMember.UserId);
                        if (userIdDistributors != null)
                        {
                            DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(userIdDistributors.DistriGradeId);
                            string str5 = "0.00";
                            DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(userIdDistributors.UserId);
                            if (distributorInfo != null)
                            {
                                str5 = distributorInfo.ReferralBlance.ToString("F2");
                            }
                            string str6 = "0";
                            string str7 = "0";
                            DataTable distributorsSubStoreNum = VShopHelper.GetDistributorsSubStoreNum(userIdDistributors.UserId);
                            if ((distributorsSubStoreNum != null) || (distributorsSubStoreNum.Rows.Count > 0))
                            {
                                str6 = distributorsSubStoreNum.Rows[0]["firstV"].ToString();
                                str7 = distributorsSubStoreNum.Rows[0]["secondV"].ToString();
                            }
                            builder2.Append(",店铺名称: '" + userIdDistributors.StoreName + "【" + distributorGradeInfo.Name + "】'");
                            builder2.Append(",销售额: '￥" + userIdDistributors.OrdersTotal.ToString("F2") + "'");
                            builder2.Append(",佣金信息: '￥" + str5 + "（待提现￥" + userIdDistributors.ReferralBlance.ToString("F2") + "，已提现￥" + userIdDistributors.ReferralRequestBalance.ToString("F2") + "）'");
                            builder2.Append(",comment: '" + ("一级分店数" + str6 + "，二级分店数" + str7) + "'");
                        }
                        str3 = builder2.ToString();
                    }
                    str = "<script>function MeiQiaInit() {$('#meiqia_serviceico').show();}(function(m, ei, q, i, a, j, s) {m[a] = m[a] || function() {(m[a].a = m[a].a || []).push(arguments)};j = ei.createElement(q),s = ei.getElementsByTagName(q)[0];j.async = true;j.charset = 'UTF-8';j.src = i + '?v=' + new Date().getUTCDate();s.parentNode.insertBefore(j, s);})(window, document, 'script', '//static.meiqia.com/dist/meiqia.js', '_MEIQIA');_MEIQIA('entId', " + masterSettings.MeiQiaEntId + ");_MEIQIA('withoutBtn');_MEIQIA('metadata', {" + str3 + "});</script><script>_MEIQIA('allSet', MeiQiaInit);</script>";
                    str2 = "<!-- 在线客服 -->\n<div class=\"customer-service\" id=\"meiqia_serviceico\" style=\"position:fixed;bottom:100px;right:10%;width:38px;height:38px;background:url(/Utility/pics/service.png?v1026) no-repeat;background-size:100%;cursor:pointer;display:none\" onclick=\"javascript:_MEIQIA._SHOWPANEL();\"></div>";
                }
                else
                {
                    CustomerServiceSettings settings2 = CustomerServiceManager.GetMasterSettings(false);
                    if ((!string.IsNullOrEmpty(settings2.unitid) && !string.IsNullOrEmpty(settings2.unit)) && !string.IsNullOrEmpty(settings2.password))
                    {
                        str = string.Format("<script src='//meiqia.com/js/mechat.js?unitid={0}&btn=hide' charset='UTF-8' async='async'></script>", settings2.unitid);
                        flag = true;
                        builder.Append("<script type=\"text/javascript\">");
                        builder.Append("function mechatFuc()");
                        builder.Append("{");
                        builder.Append("$.get(\"/Api/Hi_Ajax_OnlineServiceConfig.ashx\", function (data) {");
                        builder.Append("if (data != \"\") {");
                        builder.Append("$(data).appendTo('head');");
                        builder.Append("}");
                        builder.Append("mechatClick();");
                        builder.Append("});");
                        builder.Append("}");
                        builder.Append("</script>");
                        str2 = "<!-- 在线客服 -->\n<div class=\"customer-service\" style=\"position:fixed;bottom:100px;right:10%;width:38px;height:38px;background:url(/Utility/pics/service.png?v1026) no-repeat;background-size:100%;cursor:pointer;\" onclick=\"javascript:mechatFuc();\"></div>";
                    }
                }
                if (flag)
                {
                    base.Text = str + "\n" + builder.ToString() + "\n" + str2;
                }
            }
            base.Render(writer);
        }
    }
}

