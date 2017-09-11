using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;

namespace Hidistro.UI.Web.Admin.OutPay
{
    public partial class AliPaynotifyAmount_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {


                Hidistro.ControlPanel.OutPay.App.Notify aliNotify = new Hidistro.ControlPanel.OutPay.App.Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                if (verifyResult)//验证成功
                {
                    //批量付款数据中转账成功的详细信息
                    string success_details = Request.Form["success_details"];
                    // '1001SID号，回写数据^star_new@126.com^周新星^100^s^null^1222288888内部流水号^20150909支付时间

                    try
                    {
                        if (!string.IsNullOrEmpty(success_details))
                        {
                            string[] SuccessList = success_details.Split('|');

                            foreach (string ItemRs in SuccessList)
                            {
                                string[] ItemDetail = ItemRs.Split('^');
                                if (ItemDetail.Length >= 8)
                                {
                                    MemberAmountRequestInfo info = Hidistro.SaleSystem.Vshop.MemberAmountProcessor.GetAmountRequestDetail(Int32.Parse(ItemDetail[0])); //
                                    if (info != null && info.State != RequesState.已发放) //防止重复处理
                                    {
                                        //更新提现请求的状态为2，
                                        int[] ids = {int.Parse(ItemDetail[0])};
                                        Hidistro.SaleSystem.Vshop.MemberAmountProcessor.SetAmountRequestStatus(ids, 2, "支付宝付款：流水号" + ItemDetail[6] + ",支付时间：" + ItemDetail[7], "", ManagerHelper.GetCurrentManager().UserName); //结算请求表
                                        string detailurl = Globals.FullPath("/Vshop/MemberAmountRequestDetail.aspx?Id=" + info.Id);

                                        try
                                        {
                                            Hidistro.Messages.Messenger.SendWeiXinMsg_MemberAmountDrawCashRelease(info, detailurl);
                                        }
                                        catch { }
                                        //更新平衡表数据
                                        //Hidistro.ControlPanel.Store.VShopHelper.UpdateBalanceDistributors(info.UserId, decimal.Parse(ItemDetail[3]));                                 

                                    }
                                }
                            }
                        }


                        //批量付款数据中转账失败的详细信息
                        string fail_details = Request.Form["fail_details"];
                        if (!string.IsNullOrEmpty(fail_details))
                        {
                            string[] failList = fail_details.Split('|');

                            //处理出错支付
                            foreach (string ItemRs in failList)
                            {
                                string[] ItemDetail = ItemRs.Split('^');
                                if (ItemDetail.Length >= 8)
                                {
                                    MemberAmountRequestInfo info = Hidistro.SaleSystem.Vshop.MemberAmountProcessor.GetAmountRequestDetail(Int32.Parse(ItemDetail[0])); //
                                    if (info != null && info.State != RequesState.已发放 && info.State != RequesState.驳回) //防止重复处理
                                    {
                                        //记录一下出错信息
                                        int[] ids = { int.Parse(ItemDetail[0]) };
                                        Hidistro.SaleSystem.Vshop.MemberAmountProcessor.SetAmountRequestStatus(ids, 3, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + ItemDetail[5] + ItemDetail[6], ItemDetail[3], ManagerHelper.GetCurrentManager().UserName);
                                        //Hidistro.SaleSystem.Vshop.DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(new int[1] { int.Parse(ItemDetail[0]) }, 3, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + ItemDetail[5] + ItemDetail[6], ItemDetail[3]);

                                    }

                                }

                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        try
                        {

                            ////尝试记录日志
                            //string logFile = Globals.MapPath("/Admin/OutPay/App/") + "payLog.txt";
                            //using (StreamWriter sw = new StreamWriter(logFile, true, System.Text.Encoding.UTF8))
                            //{
                            //    sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "支付成功，写入数据库失败->" + Request.Form.ToString() + "\r\n");
                            //}
                            Globals.Debuglog(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "验证成功，写入数据库失败->" + Request.Form.ToString() + "||"+ex.ToString() +  "|| ,StackTrace --" + ex.StackTrace, "_DebugLogAlipaynotify_url.txt");


                        }
                        catch (Exception ex1)
                        {
                        }

                    }


                    Response.Write("success");  //请不要修改或删除
                }
                else//验证失败
                {
                    Response.Write("fail");

                    try
                    {

                        //尝试记录日志
                        //string logFile = Globals.MapPath("/Admin/OutPay/App/") + "payLog.txt";
                        //using (StreamWriter sw = new StreamWriter(logFile, true, System.Text.Encoding.UTF8))
                        //{
                        //    sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "支付失败->" + Request.Form.ToString() + "\r\n");
                        //}

                        Globals.Debuglog(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "验证失败1，写入数据库失败->" + Request.Form.ToString() + "", "_DebugLogAlipaynotify_url.txt");

                    }
                    catch (Exception ex)
                    {


                    }


                }


            }
            else
            {
                Response.Write("无通知参数");
            }
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;

            coll = Request.Form;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

    }
}