namespace Hidistro.UI.Web.Admin.OutPay
{
    using Hidistro.ControlPanel.OutPay;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class OneyungRefund : Page
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = "";
            string str2 = base.Request.QueryString["vaid"];
            if (!string.IsNullOrEmpty(str2))
            {
                List<string> values = OneyuanTaoHelp.GetParticipantPids(str2, true, false, "alipay");
                if (values.Count == 0)
                {
                    base.Response.Write("没有适合支付宝退款的活动参与记录！");
                    return;
                }
                str = string.Join(",", values);
            }
            if (str == "")
            {
                str = base.Request.QueryString["pids"];
            }
            if (string.IsNullOrEmpty(str))
            {
                base.Response.Write("非正常访问！");
            }
            else
            {
                IList<OneyuanTaoParticipantInfo> refundParticipantList = OneyuanTaoHelp.GetRefundParticipantList(str.Replace("　", "").Replace(" ", "").Trim().Split(new char[] { ',' }));
                if (refundParticipantList == null)
                {
                    base.Response.Write("获取夺宝信息失败,可能信息已删除！");
                }
                else
                {
                    List<alipayReturnInfo> refundList = new List<alipayReturnInfo>();
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    foreach (OneyuanTaoParticipantInfo info in refundParticipantList)
                    {
                        if (((info.IsPay && !info.IsRefund) && (!info.IsWin && !string.IsNullOrEmpty(info.PayNum))) && (!string.IsNullOrEmpty(info.PayWay) && !(info.PayWay == "weixin")))
                        {
                            alipayReturnInfo item = new alipayReturnInfo {
                                alipaynum = info.PayNum,
                                refundNum = info.TotalPrice,
                                Remark = masterSettings.SiteName + "退款,对应活动编码：" + info.ActivityId
                            };
                            refundList.Add(item);
                        }
                    }
                    if (refundList.Count == 0)
                    {
                        base.Response.Write("当前选择的退款记录不符号退款条件，为非支付宝付款记录！");
                    }
                    else
                    {
                        string s = RefundHelper.AlipayRefundRequest(string.Format("http://{0}/Admin/OutPay/OneyuanAlipayRefundNotify.aspx", base.Request.Url.Host), refundList);
                        base.Response.Write(s);
                        base.Response.End();
                    }
                }
            }
        }
    }
}

