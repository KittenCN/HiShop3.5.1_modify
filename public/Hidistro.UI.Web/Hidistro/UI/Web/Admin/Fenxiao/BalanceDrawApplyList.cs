namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.OutPay;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.OutPay;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using Hishop.Weixin.Pay;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class BalanceDrawApplyList : AdminPage
    {
        protected Button alipaySend;
        protected TextBox bankPayDate;
        protected TextBox bankPayNum;
        protected Button BankPaySave;
        public bool BatchAlipay;
        protected Button BatchPass;
        protected Button BatchPaySend;
        public bool BatchWeipay;
        protected Button btnSearchButton;
        protected Button Button1;
        protected Button Button2;
        protected Button Button3;
        protected Button Button4;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        public int DrawMinNum;
        protected string DrawPayType;
        protected LinkButton Frist;
        protected HtmlInputHidden hdredpackrecordnum;
        protected HtmlInputHidden hdreferralblance;
        protected HtmlInputHidden hduserid;
        protected HiddenField HiddenSid;
        protected HiddenField hSerialID;
        protected HiddenField HSid;
        private bool IsGetSetting;
        private int lastDay;
        private UpdateStatistics myEvent;
        private StatisticNotifier myNotifier;
        protected Pager pager;
        protected Button PassCheck;
        protected HtmlInputHidden PaybatchType;
        protected Repeater reCommissions;
        protected TextBox RefuseMks;
        protected TextBox ReqSum;
        private string RequestEndTime;
        private string RequestStartTime;
        protected LinkButton Second;
        protected TextBox SignalrefuseMks;
        private string StoreName;
        protected TextBox txtStoreName;
        protected Button weipaySend;
        protected Button WeiRedPack;

        protected BalanceDrawApplyList() : base("m05", "fxp09")
        {
            this.RequestStartTime = "";
            this.RequestEndTime = "";
            this.StoreName = "";
            this.DrawMinNum = 1;
            this.DrawPayType = "";
            this.myNotifier = new StatisticNotifier();
            this.myEvent = new UpdateStatistics();
        }

        private void alipaySend_Click(object sender, EventArgs e)
        {
            this.ShowMsg("接口暂未开通", false);
        }

        private void BankPaySave_Click(object sender, EventArgs e)
        {
            int id = Globals.ToNum(this.HiddenSid.Value);
            int userId = Globals.ToNum(this.hduserid.Value);
            string remark = "转账流水号：" + this.bankPayNum.Text;
            decimal num3 = decimal.Parse(this.hdreferralblance.Value);
            int num4 = Globals.ToNum("0" + this.hdredpackrecordnum.Value);
            if (VShopHelper.UpdateBalanceDrawRequest(id, remark))
            {
                decimal referralRequestBalance = num3;
                if (num4 > 0)
                {
                    int redPackTotalAmount = DistributorsBrower.GetRedPackTotalAmount(id, 0);
                    referralRequestBalance -= decimal.Parse(redPackTotalAmount.ToString()) / 100M;
                }
                if (VShopHelper.UpdateBalanceDistributors(userId, referralRequestBalance))
                {
                    if (referralRequestBalance > 0M)
                    {
                        BalanceDrawRequestInfo balanceDrawRequestById = DistributorsBrower.GetBalanceDrawRequestById(id.ToString());
                        if (balanceDrawRequestById != null)
                        {
                            try
                            {
                                Messenger.SendWeiXinMsg_DrawCashRelease(balanceDrawRequestById);
                            }
                            catch
                            {
                            }
                        }
                    }
                    this.ShowMsg("结算成功", true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("结算失败", false);
                }
            }
            else
            {
                this.ShowMsg("结算失败", false);
            }
        }

        protected void BatchPass_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            else
            {
                this.ShowMsg("参数错误！", false);
                return;
            }
            int[] serialids = Array.ConvertAll<string, int>(str.Split(new char[] { ',' }), s => Globals.ToNum(s));
            Dictionary<int, int> mulBalanceDrawRequestIsCheckStatus = DistributorsBrower.GetMulBalanceDrawRequestIsCheckStatus(serialids);
            ArrayList list = new ArrayList();
            foreach (int num in serialids)
            {
                if (mulBalanceDrawRequestIsCheckStatus.ContainsKey(num) && (mulBalanceDrawRequestIsCheckStatus[num] == 0))
                {
                    list.Add(num);
                }
            }
            if (list.Count == 0)
            {
                this.ShowMsg("没有未审核状态的数据，操作终止！", false);
            }
            else
            {
                serialids = (int[]) list.ToArray(typeof(int));
                if (serialids.Length > 0)
                {
                    if (DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(serialids, 1, null, null))
                    {
                        this.UpdateNotify("申请提现批量审核成功");
                        this.ShowMsg("批量审核成功！", true);
                    }
                    else
                    {
                        this.ShowMsg("批量审核失败，请再次尝试", false);
                    }
                    this.LoadParameters();
                    this.BindData();
                }
            }
        }

        private void BatchPaySend_Click(object sender, EventArgs e)
        {
            this.GetPaySetting();
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            else
            {
                this.ShowMsg("参数错误！", false);
                return;
            }
            string str2 = this.PaybatchType.Value;
            if (str2 == "0")
            {
                string[] strArray = str.Split(new char[] { ',' });
                List<OutPayWeiInfo> batchUserList = new List<OutPayWeiInfo>();
                foreach (string str3 in strArray)
                {
                    BalanceDrawRequestInfo balanceDrawRequestById = DistributorsBrower.GetBalanceDrawRequestById(str3.ToString());
                    if (((balanceDrawRequestById != null) && (balanceDrawRequestById.IsCheck == "1")) && ((this.DrawMinNum <= balanceDrawRequestById.Amount) && !string.IsNullOrEmpty(balanceDrawRequestById.RedpackId)))
                    {
                        OutPayWeiInfo item = new OutPayWeiInfo {
                            Amount = ((int) balanceDrawRequestById.Amount) * 100,
                            Partner_Trade_No = balanceDrawRequestById.RedpackId,
                            Sid = balanceDrawRequestById.SerialId,
                            Openid = balanceDrawRequestById.MerchantCode,
                            Re_User_Name = balanceDrawRequestById.AccountName,
                            Desc = " 分销商佣金发放",
                            UserId = balanceDrawRequestById.UserId,
                            Nonce_Str = OutPayHelp.GetRandomString(20)
                        };
                        batchUserList.Add(item);
                    }
                }
                if (batchUserList.Count < 1)
                {
                    this.ShowMsg("没有满足条件的提现请求！", false);
                    this.LoadParameters();
                    this.BindData();
                }
                else
                {
                    List<WeiPayResult> list2 = OutPayHelp.BatchWeiPay(batchUserList);
                    if (list2.Count < 1)
                    {
                        this.ShowMsg("系统异常，请联系管理员！", false);
                    }
                    else if (list2[0].return_code == "INITFAIL")
                    {
                        this.ShowMsg(list2[0].return_msg, false);
                    }
                    else
                    {
                        int num = 0;
                        int num2 = 0;
                        int num3 = 0;
                        int count = batchUserList.Count;
                        string str4 = "<div class='errRow'>支付失败信息如下：";
                        using (List<WeiPayResult>.Enumerator enumerator = list2.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                Func<OutPayWeiInfo, bool> predicate = null;
                                WeiPayResult rItem = enumerator.Current;
                                if (predicate == null)
                                {
                                    predicate = t => t.Partner_Trade_No == rItem.partner_trade_no;
                                }
                                int sid = batchUserList.FirstOrDefault<OutPayWeiInfo>(predicate).Sid;
                                if (rItem.result_code == "SUCCESS")
                                {
                                    VShopHelper.UpdateBalanceDrawRequest(sid, "微信企业付款：流水号" + rItem.payment_no);
                                    int userId = rItem.UserId;
                                    decimal referralRequestBalance = rItem.Amount / 100;
                                    VShopHelper.UpdateBalanceDistributors(rItem.UserId, referralRequestBalance);
                                    if ((referralRequestBalance > 0M) && (sid > 0))
                                    {
                                        BalanceDrawRequestInfo balance = DistributorsBrower.GetBalanceDrawRequestById(sid.ToString());
                                        if (balance != null)
                                        {
                                            try
                                            {
                                                Messenger.SendWeiXinMsg_DrawCashRelease(balance);
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                    num3 += rItem.Amount / 100;
                                    num++;
                                }
                                else
                                {
                                    if (((rItem.err_code == "OPENID_ERROR") || (rItem.err_code == "NAME_MISMATCH")) || (rItem.return_msg.Contains("openid字段") || (rItem.err_code == "FATAL_ERROR")))
                                    {
                                        DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(new int[] { sid }, 3, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + rItem.return_msg, null);
                                    }
                                    else
                                    {
                                        DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(new int[] { sid }, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + rItem.return_msg, null);
                                    }
                                    object obj2 = str4;
                                    str4 = string.Concat(new object[] { obj2, "<br>ID：", sid, ", 出错提示：", rItem.return_msg });
                                    num2++;
                                }
                            }
                        }
                        count = (count - num2) - num;
                        str4 = str4 + "</div>";
                        if ((num3 == num) && (num3 != 0))
                        {
                            this.ShowMsg("全部支付成功", true);
                        }
                        else
                        {
                            this.ShowMsg(string.Concat(new object[] { "本次成功支付金额", num3, "元，其中成功", num, "笔，失败", num2, "笔，异常放弃", count, "笔", str4 }), false);
                        }
                        this.LoadParameters();
                        this.BindData();
                    }
                }
            }
            else if (str2 == "1")
            {
                this.ShowMsg("接口暂未开通", false);
            }
            else
            {
                this.ShowMsg("未定义支付方式", false);
            }
        }

        private void BindData()
        {
            BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery {
                RequestTime = "",
                CheckTime = "",
                StoreName = this.StoreName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "SerialID",
                RequestEndTime = this.RequestEndTime,
                RequestStartTime = this.RequestStartTime,
                IsCheck = "",
                UserId = ""
            };
            string[] extendChecks = new string[] { "0", "1" };
            Globals.EntityCoding(entity, true);
            DbQueryResult balanceDrawRequest = DistributorsBrower.GetBalanceDrawRequest(entity, extendChecks);
            this.reCommissions.DataSource = balanceDrawRequest.Data;
            this.reCommissions.DataBind();
            this.pager.TotalRecords = balanceDrawRequest.TotalRecords;
            DataTable drawRequestNum = DistributorsBrower.GetDrawRequestNum(new int[] { 3 });
            this.Frist.Text = "待发放(" + this.pager.TotalRecords + ")";
            if (drawRequestNum.Rows.Count > 0)
            {
                this.Second.Text = "发放异常(" + drawRequestNum.Rows[0]["num"].ToString() + ")";
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                this.RequestStartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                this.RequestEndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            this.lastDay = 7;
            this.ReBind(true);
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.RequestEndTime = now.ToString("yyyy-MM-dd");
            this.RequestStartTime = now.AddDays(-6.0).ToString("yyyy-MM-dd");
            this.lastDay = 7;
            this.ReBind(true);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            else
            {
                this.ShowMsg("参数错误！", false);
                return;
            }
            int[] serialids = Array.ConvertAll<string, int>(str.Split(new char[] { ',' }), s => Globals.ToNum(s));
            Dictionary<int, int> mulBalanceDrawRequestIsCheckStatus = DistributorsBrower.GetMulBalanceDrawRequestIsCheckStatus(serialids);
            ArrayList list = new ArrayList();
            foreach (int num in serialids)
            {
                if (mulBalanceDrawRequestIsCheckStatus.ContainsKey(num) && ((mulBalanceDrawRequestIsCheckStatus[num] == 0) || (mulBalanceDrawRequestIsCheckStatus[num] == 1)))
                {
                    list.Add(num);
                }
            }
            if (list.Count == 0)
            {
                this.ShowMsg("当前选择项没有数据可以驳回，操作终止！", false);
            }
            else
            {
                serialids = (int[]) list.ToArray(typeof(int));
                if (serialids.Length > 0)
                {
                    if (DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(serialids, -1, this.RefuseMks.Text, null))
                    {
                        this.UpdateNotify("申请提现批量驳回");
                        this.ShowMsg("批量驳回成功！", true);
                    }
                    else
                    {
                        this.ShowMsg("批量驳回失败，请再次尝试", false);
                    }
                    this.LoadParameters();
                    this.BindData();
                }
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            int[] serialids = new int[] { Globals.ToNum(this.hSerialID.Value) };
            if (serialids[0] != 0)
            {
                switch (DistributorsBrower.GetBalanceDrawRequestIsCheckStatus(serialids[0]))
                {
                    case -1:
                    case 2:
                        this.ShowMsg("当前项数据不可以驳回，操作终止！", false);
                        return;
                }
                if (DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(serialids, -1, this.SignalrefuseMks.Text, null))
                {
                    this.UpdateNotify("申请提现驳回");
                    this.ShowMsg("申请已驳回！", true);
                    this.LoadParameters();
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("申请驳回失败，请再次尝试", false);
                }
            }
        }

        protected void Button4_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.RequestEndTime = now.ToString("yyyy-MM-dd");
            this.RequestStartTime = now.AddDays(-29.0).ToString("yyyy-MM-dd");
            this.lastDay = 30;
            this.ReBind(true);
        }

        private void GetPaySetting()
        {
            if (!this.IsGetSetting)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                this.BatchWeipay = masterSettings.BatchWeixinPay;
                this.BatchAlipay = masterSettings.BatchAliPay;
                this.DrawPayType = masterSettings.DrawPayType;
                this.DrawMinNum = Globals.ToNum(masterSettings.MentionNowMoney);
                this.IsGetSetting = true;
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
                {
                    this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestEndTime"]))
                {
                    this.RequestEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestEndTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestStartTime"]))
                {
                    this.RequestStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestStartTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestStartTime"]))
                {
                    this.RequestStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestStartTime"]);
                    this.calendarStartDate.SelectedDate = new DateTime?(DateTime.Parse(this.RequestStartTime));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestEndTime"]))
                {
                    this.RequestEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestEndTime"]);
                    this.calendarEndDate.SelectedDate = new DateTime?(DateTime.Parse(this.RequestEndTime));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
                {
                    int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
                    if (this.lastDay == 30)
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("#1CA47D");
                    }
                    else if (this.lastDay == 7)
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("#1CA47D");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("");
                    }
                    else
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("");
                    }
                }
                this.txtStoreName.Text = this.StoreName;
            }
            else
            {
                this.StoreName = this.txtStoreName.Text;
                if (this.calendarStartDate.SelectedDate.HasValue)
                {
                    this.RequestStartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                if (this.calendarEndDate.SelectedDate.HasValue)
                {
                    this.RequestEndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.Button3.Click += new EventHandler(this.Button3_Click);
            this.Button2.Click += new EventHandler(this.Button2_Click);
            this.BatchPass.Click += new EventHandler(this.BatchPass_Click);
            this.PassCheck.Click += new EventHandler(this.PassCheck_Click);
            this.BankPaySave.Click += new EventHandler(this.BankPaySave_Click);
            this.WeiRedPack.Click += new EventHandler(this.WeiRedPack_Click);
            this.alipaySend.Click += new EventHandler(this.alipaySend_Click);
            this.weipaySend.Click += new EventHandler(this.weipaySend_Click);
            this.BatchPaySend.Click += new EventHandler(this.BatchPaySend_Click);
            this.ReqSum.Attributes.Add("readonly", "true");
            this.GetPaySetting();
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void PassCheck_Click(object sender, EventArgs e)
        {
            decimal result = 0M;
            int num2 = 0;
            if (int.TryParse(this.HSid.Value, out num2) && decimal.TryParse(this.ReqSum.Text, out result))
            {
                int[] serialids = new int[] { num2 };
                if (DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(serialids, 1, null, result.ToString()))
                {
                    this.UpdateNotify("申请提现审核成功");
                    this.ShowMsg("审核已通过", true);
                }
                else
                {
                    this.ShowMsg("审核失败，请再次尝试", false);
                }
                this.LoadParameters();
                this.BindData();
            }
            else
            {
                this.ShowMsg("提现金额请填写数值", false);
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("StoreName", this.txtStoreName.Text);
            queryStrings.Add("RequestStartTime", this.RequestStartTime);
            queryStrings.Add("RequestEndTime", this.RequestEndTime);
            queryStrings.Add("lastDay", this.lastDay.ToString());
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                ImageLinkButton button = (ImageLinkButton) e.Item.FindControl("CheckOrGive");
                switch (Globals.ToNum(((DataRowView) e.Item.DataItem).Row["IsCheck"].ToString()))
                {
                    case 0:
                        button.Text = "审核";
                        return;

                    case 1:
                        button.Text = "发放";
                        button.CssClass = "btn btn-info btn-xs";
                        return;
                }
                button.Text = "未知";
                button.Enabled = false;
            }
        }

        protected void Second_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("BalanceDrawApplyErrorList.aspx");
        }

        public string SendRedPack(string re_openid, string sub_mch_id, string wishing, string act_name, string remark, int amount, string RedpackId)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (masterSettings.EnableWeiXinRequest)
            {
                DateTime now = DateTime.Now;
                DateTime time2 = DateTime.Parse(now.ToString("yyyy-MM-dd") + " 00:00:01");
                DateTime time3 = DateTime.Parse(now.ToString("yyyy-MM-dd") + " 08:00:00");
                if ((now > time2) && (now < time3))
                {
                    return "北京时间0：00-8：00不触发红包赠送";
                }
                if ((string.IsNullOrEmpty(masterSettings.WeixinAppId) || string.IsNullOrEmpty(masterSettings.WeixinPartnerID)) || ((string.IsNullOrEmpty(masterSettings.WeixinPartnerKey) || string.IsNullOrEmpty(masterSettings.WeixinCertPath)) || string.IsNullOrEmpty(masterSettings.WeixinCertPassword)))
                {
                    return "系统微信发红包配置接口未配置好";
                }
                if (string.IsNullOrEmpty(re_openid))
                {
                    return "用户未绑定微信";
                }
                string siteName = masterSettings.SiteName;
                string str3 = masterSettings.SiteName;
                RedPackClient client = new RedPackClient();
                try
                {
                    string iPAddress = Globals.IPAddress;
                    if (iPAddress.Length < 8)
                    {
                        iPAddress = "192.168.1.1";
                    }
                    return client.SendRedpack(masterSettings.WeixinAppId, masterSettings.WeixinPartnerID, sub_mch_id, siteName, str3, re_openid, wishing, iPAddress, act_name, remark, amount, masterSettings.WeixinPartnerKey, masterSettings.WeixinCertPath, masterSettings.WeixinCertPassword, RedpackId, masterSettings.EnableSP, masterSettings.Main_AppId, masterSettings.Main_Mch_ID, masterSettings.Main_PayKey);
                }
                catch (Exception exception)
                {
                    return exception.Message.ToString().Trim();
                }
            }
            return "管理员后台未开启微信付款！";
        }

        private void UpdateNotify(string actionDesc)
        {
            try
            {
                this.myNotifier.updateAction = UpdateAction.MemberUpdate;
                this.myNotifier.actionDesc = actionDesc;
                this.myNotifier.RecDateUpdate = DateTime.Today;
                this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                this.myNotifier.UpdateDB();
            }
            catch (Exception)
            {
            }
        }

        private void weipaySend_Click(object sender, EventArgs e)
        {
            int id = Globals.ToNum(this.HiddenSid.Value);
            decimal referralRequestBalance = decimal.Parse(this.hdreferralblance.Value);
            BalanceDrawRequestInfo balanceDrawRequestById = DistributorsBrower.GetBalanceDrawRequestById(id.ToString());
            if (balanceDrawRequestById != null)
            {
                if (balanceDrawRequestById.IsCheck == "2")
                {
                    this.ShowMsg("该申请已经支付，请检查", false);
                }
                else if (balanceDrawRequestById.IsCheck == "-1")
                {
                    this.ShowMsg("该申请已经驳回，请检查", false);
                }
                else if (string.IsNullOrEmpty(balanceDrawRequestById.RedpackId))
                {
                    this.ShowMsg("商户订单ID为空", false);
                }
                else
                {
                    WeiPayResult result = OutPayHelp.SingleWeiPay((int) (referralRequestBalance * 100M), "分销商佣金发放！", balanceDrawRequestById.MerchantCode, balanceDrawRequestById.AccountName, balanceDrawRequestById.RedpackId, balanceDrawRequestById.UserId);
                    if (result.result_code == "SUCCESS")
                    {
                        VShopHelper.UpdateBalanceDrawRequest(id, "微信企业付款");
                        int userId = balanceDrawRequestById.UserId;
                        VShopHelper.UpdateBalanceDistributors(balanceDrawRequestById.UserId, referralRequestBalance);
                        if (referralRequestBalance > 0M)
                        {
                            BalanceDrawRequestInfo balance = DistributorsBrower.GetBalanceDrawRequestById(id.ToString());
                            if (balance != null)
                            {
                                try
                                {
                                    Messenger.SendWeiXinMsg_DrawCashRelease(balance);
                                }
                                catch
                                {
                                }
                            }
                        }
                        this.LoadParameters();
                        this.BindData();
                        this.ShowMsg("支付成功！", true);
                    }
                    else
                    {
                        if (((result.err_code == "OPENID_ERROR") || (result.err_code == "NAME_MISMATCH")) || (result.return_msg.Contains("openid字段") || (result.err_code == "FATAL_ERROR")))
                        {
                            DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(new int[] { id }, 3, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + result.return_msg, referralRequestBalance.ToString());
                            this.LoadParameters();
                            this.BindData();
                        }
                        else
                        {
                            DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(new int[] { id }, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + result.return_msg, referralRequestBalance.ToString());
                        }
                        this.ShowMsg("微信企业付款失败，" + result.return_msg, false);
                    }
                }
            }
            else
            {
                this.ShowMsg("参数错误！", false);
            }
        }

        private void WeiRedPack_Click(object sender, EventArgs e)
        {
            int id = Globals.ToNum(this.HiddenSid.Value);
            if (decimal.Parse(this.hdreferralblance.Value) > 200M)
            {
                this.ShowMsg("红包金额大于200，无法发放！", false);
            }
            else
            {
                BalanceDrawRequestInfo balanceDrawRequestById = DistributorsBrower.GetBalanceDrawRequestById(id.ToString());
                if (balanceDrawRequestById == null)
                {
                    this.ShowMsg("参数错误，请求不存在", false);
                }
                else if (balanceDrawRequestById.IsCheck == "2")
                {
                    this.ShowMsg("该申请已经支付，请检查", false);
                }
                else if (!string.IsNullOrEmpty(balanceDrawRequestById.RedpackId))
                {
                    string str = this.SendRedPack(balanceDrawRequestById.MerchantCode, "", "恭喜您提现成功!", "您的提现申请已成功", "分销商发红包提现", ((int) balanceDrawRequestById.Amount) * 100, balanceDrawRequestById.RedpackId);
                    if (str == "success")
                    {
                        VShopHelper.UpdateBalanceDrawRequest(id, "微信红包付款");
                        VShopHelper.UpdateBalanceDistributors(balanceDrawRequestById.UserId, balanceDrawRequestById.Amount);
                        if (balanceDrawRequestById.Amount > 0M)
                        {
                            BalanceDrawRequestInfo balance = DistributorsBrower.GetBalanceDrawRequestById(id.ToString());
                            if (balance != null)
                            {
                                try
                                {
                                    Messenger.SendWeiXinMsg_DrawCashRelease(balance);
                                }
                                catch
                                {
                                }
                            }
                        }
                        this.ShowMsg("红包发送成功！", true);
                        this.LoadParameters();
                        this.BindData();
                    }
                    else
                    {
                        this.ShowMsg("发送失败，原因是：" + str, false);
                    }
                }
                else
                {
                    this.ShowMsg("发送失败0！", false);
                }
            }
        }
    }
}

