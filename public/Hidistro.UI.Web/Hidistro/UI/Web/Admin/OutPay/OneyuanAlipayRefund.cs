namespace Hidistro.UI.Web.Admin.OutPay
{
    using Hidistro.ControlPanel.OutPay.App;
    using Hidistro.Core;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;

    public class OneyuanAlipayRefund : Page
    {
        public SortedDictionary<string, string> GetRequestPost()
        {
            int index = 0;
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();
            string[] allKeys = base.Request.Form.AllKeys;
            for (index = 0; index < allKeys.Length; index++)
            {
                dictionary.Add(allKeys[index], base.Request.Form[allKeys[index]]);
            }
            return dictionary;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> requestPost = this.GetRequestPost();
            Globals.Debuglog(base.Request.Form.ToString(), "_Debuglog.txt");
            if (requestPost.Count > 0)
            {
                Notify notify = new Notify();
                if (notify.Verify(requestPost, base.Request.Form["notify_id"], base.Request.Form["sign"]))
                {
                    string str = base.Request.Form["batch_no"];
                    string text1 = base.Request.Form["success_num"];
                    string str2 = base.Request.Form["result_details"];
                    if (!OneyuanTaoHelp.IsExistAlipayRefundNUm(str) && !string.IsNullOrEmpty(str2))
                    {
                        try
                        {
                            string[] strArray = str2.Split(new char[] { '#' });
                            List<string> source = new List<string>();
                            foreach (string str3 in strArray)
                            {
                                string[] strArray2 = str3.Split(new char[] { '^' });
                                OneyuanTaoParticipantInfo info = OneyuanTaoHelp.GetAddParticipant(0, "", strArray2[0].Trim());
                                if (info != null)
                                {
                                    if (str3.Contains("SUCCESS"))
                                    {
                                        source.Add(info.ActivityId);
                                        info.Remark = "退款成功";
                                        info.RefundNum = str;
                                        OneyuanTaoHelp.SetRefundinfo(info);
                                    }
                                    else
                                    {
                                        info.Remark = "退款失败:" + str3;
                                        OneyuanTaoHelp.SetRefundinfoErr(info);
                                    }
                                }
                            }
                            source = source.Distinct<string>().ToList<string>();
                            if (source.Count > 0)
                            {
                                OneyuanTaoHelp.SetIsAllRefund(source);
                            }
                        }
                        catch (Exception exception)
                        {
                            Globals.Debuglog(base.Request.Form.ToString() + ":exception->" + exception.Message, "_Debuglog.txt");
                        }
                    }
                    base.Response.Write("success");
                }
                else
                {
                    base.Response.Write("fail");
                    Globals.Debuglog(base.Request.Form.ToString(), "alipayrefun.txt");
                }
            }
            else
            {
                base.Response.Write("无通知参数");
            }
        }
    }
}

