namespace Hidistro.UI.Web.Admin.OutPay
{
    using Hidistro.ControlPanel.OutPay.App;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;

    public class AliPaynotify_url : Page
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
            if (requestPost.Count > 0)
            {
                Notify notify = new Notify();
                if (notify.Verify(requestPost, base.Request.Form["notify_id"], base.Request.Form["sign"]))
                {
                    string str = base.Request.Form["success_details"];
                    try
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            foreach (string str2 in str.Split(new char[] { '|' }))
                            {
                                string[] strArray2 = str2.Split(new char[] { '^' });
                                if (strArray2.Length >= 8)
                                {
                                    BalanceDrawRequestInfo balanceDrawRequestById = DistributorsBrower.GetBalanceDrawRequestById(strArray2[0]);
                                    if ((balanceDrawRequestById != null) && (balanceDrawRequestById.IsCheck != "2"))
                                    {
                                        VShopHelper.UpdateBalanceDrawRequest(int.Parse(strArray2[0]), "支付宝付款：流水号" + strArray2[6] + ",支付时间：" + strArray2[7]);
                                        VShopHelper.UpdateBalanceDistributors(balanceDrawRequestById.UserId, decimal.Parse(strArray2[3]));
                                    }
                                }
                            }
                        }
                        string str3 = base.Request.Form["fail_details"];
                        if (!string.IsNullOrEmpty(str3))
                        {
                            foreach (string str4 in str3.Split(new char[] { '|' }))
                            {
                                string[] strArray4 = str4.Split(new char[] { '^' });
                                if (strArray4.Length >= 8)
                                {
                                    BalanceDrawRequestInfo info2 = DistributorsBrower.GetBalanceDrawRequestById(strArray4[0]);
                                    if (((info2 != null) && (info2.IsCheck != "2")) && (info2.IsCheck != "-1"))
                                    {
                                        DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(new int[] { int.Parse(strArray4[0]) }, 3, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + strArray4[5] + strArray4[6], strArray4[3]);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        try
                        {
                            Globals.Debuglog(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "验证成功，写入数据库失败->" + base.Request.Form.ToString() + "||" + exception.ToString(), "_DebugLogAlipaynotify_url.txt");
                        }
                        catch (Exception)
                        {
                        }
                    }
                    base.Response.Write("success");
                }
                else
                {
                    base.Response.Write("fail");
                    try
                    {
                        Globals.Debuglog(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "验证失败2，写入数据库失败->" + base.Request.Form.ToString(), "_DebugLogAlipaynotify_url.txt");
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else
            {
                base.Response.Write("无通知参数");
            }
        }
    }
}

