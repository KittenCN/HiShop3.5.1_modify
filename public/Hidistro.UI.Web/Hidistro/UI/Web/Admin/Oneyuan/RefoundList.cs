namespace Hidistro.UI.Web.Admin.Oneyuan
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.OutPay;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class RefoundList : AdminPage
    {
        protected HtmlTableCell actionTd;
        private string atitle;
        protected Button btnSearchButton;
        protected Repeater Datalist;
        protected PageSize hrefPageSize;
        protected Literal ListEnd;
        protected Literal ListWait;
        protected Pager pager;
        private static object payLock = new object();
        private string PayWay;
        private string phone;
        private int state;
        protected HtmlGenericControl txtEditInfo;
        protected DropDownList txtPayWay;
        protected TextBox txtPhone;
        protected TextBox txtTitle;
        protected TextBox txtUserName;
        private string UserName;

        protected RefoundList() : base("m08", "yxp22")
        {
            this.PayWay = "";
        }

        private void AjaxAction(string action)
        {
            string s = "{\"state\":false,\"msg\":\"未定义操作\"}";
            NameValueCollection form = base.Request.Form;
            string str8 = action;
            if (str8 != null)
            {
                if (!(str8 == "WeiXinRefund"))
                {
                    if (str8 == "WeiXinBacthRefund")
                    {
                        string pids = base.Request.Form["Pids"];
                        s = this.BatchWxRefund(pids);
                    }
                    else if (str8 == "BatchRefund")
                    {
                        s = "{\"state\":false,\"msg\":\"初始化中！\"}";
                        string str7 = base.Request.Form["vaid"];
                        if (string.IsNullOrEmpty(str7))
                        {
                            s = "{\"state\":false,\"msg\":\"参数错误！\"}";
                        }
                        else
                        {
                            List<string> values = OneyuanTaoHelp.GetParticipantPids(str7, true, false, "weixin");
                            if (values.Count > 0)
                            {
                                string WxPids = string.Join(",", values);
                                new Thread(() => Globals.Debuglog(this.BatchWxRefund(WxPids), "_wxrefund.txt")).Start();
                            }
                            if (OneyuanTaoHelp.GetParticipantPids(str7, true, false, "alipay").Count > 0)
                            {
                                s = "{\"state\":true,\"msg\":\"转向支付宝退款中...！\",\"alipay\":true}";
                            }
                            else if (values.Count > 0)
                            {
                                s = "{\"state\":true,\"msg\":\"正在后台处理微信退款，请稍后刷新！\",\"alipay\":false}";
                            }
                            else
                            {
                                s = "{\"state\":false,\"msg\":\"没有退款数据可以操作！\",\"alipay\":false}";
                                OneyuanTaoHelp.SetIsAllRefund(new List<string> { str7 });
                            }
                        }
                    }
                }
                else
                {
                    string str2 = form["Pid"];
                    if (!string.IsNullOrEmpty(str2))
                    {
                        OneyuanTaoParticipantInfo info = OneyuanTaoHelp.GetAddParticipant(0, str2, "");
                        if ((info != null) && info.IsPay)
                        {
                            if (!info.IsRefund)
                            {
                                string str3 = info.out_refund_no;
                                if (string.IsNullOrEmpty(str3))
                                {
                                    str3 = RefundHelper.GenerateRefundOrderId();
                                    OneyuanTaoHelp.Setout_refund_no(str2, str3);
                                }
                                string wxRefundNum = "";
                                string str5 = RefundHelper.SendWxRefundRequest(str2, info.TotalPrice, info.TotalPrice, str3, out wxRefundNum);
                                if (str5 == "")
                                {
                                    s = "{\"state\":true,\"msg\":\"退款成功\"}";
                                    info.Remark = "退款成功";
                                    info.RefundNum = wxRefundNum;
                                    OneyuanTaoHelp.SetRefundinfo(info);
                                    OneyuanTaoHelp.SetIsAllRefund(new List<string> { info.ActivityId });
                                }
                                else
                                {
                                    info.Remark = "退款失败:" + str5.Replace("OK", "");
                                    OneyuanTaoHelp.SetRefundinfoErr(info);
                                    s = "{\"state\":false,\"msg\":\"" + info.Remark + "\"}";
                                }
                            }
                            else
                            {
                                s = "{\"state\":false,\"msg\":\"该订单已退款！\"}";
                            }
                        }
                        else
                        {
                            s = "{\"state\":false,\"msg\":\"用户记录不存在或者用户未支付！\"}";
                        }
                    }
                    else
                    {
                        s = "{\"state\":false,\"msg\":\"参数错误！\"}";
                    }
                }
            }
            base.Response.ClearContent();
            base.Response.ContentType = "application/json";
            base.Response.Write(s);
            base.Response.End();
        }

        private string BatchWxRefund(string Pids)
        {
            List<string> source = new List<string>();
            string str = "";
            if (string.IsNullOrEmpty(Pids))
            {
                str = "{\"state\":false,\"msg\":\"参数错误！\"}";
            }
            else
            {
                string message = "";
                IList<OneyuanTaoParticipantInfo> refundParticipantList = OneyuanTaoHelp.GetRefundParticipantList(Pids.Replace(" ", "").Replace("　", "").Split(new char[] { ',' }));
                int num = 0;
                foreach (OneyuanTaoParticipantInfo info in refundParticipantList)
                {
                    try
                    {
                        if (((info != null) && info.IsPay) && (info.PayWay == "weixin"))
                        {
                            source.Add(info.ActivityId);
                            if (!info.IsRefund)
                            {
                                string str3 = info.out_refund_no;
                                if (string.IsNullOrEmpty(str3))
                                {
                                    str3 = RefundHelper.GenerateRefundOrderId();
                                    OneyuanTaoHelp.Setout_refund_no(info.Pid, str3);
                                }
                                string wxRefundNum = "";
                                string str5 = RefundHelper.SendWxRefundRequest(info.Pid, info.TotalPrice, info.TotalPrice, str3, out wxRefundNum);
                                if (str5 == "")
                                {
                                    info.Remark = "退款成功";
                                    info.RefundNum = wxRefundNum;
                                    OneyuanTaoHelp.SetRefundinfo(info);
                                    num++;
                                    continue;
                                }
                                info.Remark = "退款失败:" + str5.Replace("OK", "");
                                OneyuanTaoHelp.SetRefundinfoErr(info);
                                if (((!str5.Contains("金额不足") && !str5.Contains("mch_id")) && (!str5.Contains("mch_id") && !str5.ToLower().Contains("appid"))) && ((!str5.Contains("密码") && !str5.Contains("证书")) && (!str5.Contains("签名") && !str5.ToLower().Contains("mchid"))))
                                {
                                    continue;
                                }
                                message = str5;
                                break;
                            }
                            num++;
                        }
                    }
                    catch (Exception exception)
                    {
                        message = exception.Message;
                        Globals.Debuglog("微信退款异常信息：" + exception.Message, "_wxrefund.txt");
                    }
                }
                if (num == 0)
                {
                    if (message == "")
                    {
                        str = "{\"state\":false,\"msg\":\"微信批量退款失败！\"}";
                    }
                    else
                    {
                        message = message.Replace(",", "，").Replace("\"", " ").Replace("'", " ").Replace(":", " ");
                        str = "{\"state\":false,\"msg\":\"退款中断->" + message + "\"}";
                    }
                }
                else
                {
                    string[] strArray2 = new string[] { "{\"state\":true,\"msg\":\"成功退款", num.ToString(), "笔，失败", (refundParticipantList.Count - num).ToString(), "笔\"}" };
                    str = string.Concat(strArray2);
                }
            }
            OneyuanTaoHelp.SetIsAllRefund(source.Distinct<string>().ToList<string>());
            return str;
        }

        private void BindData()
        {
            OneyuanTaoPartInQuery query = new OneyuanTaoPartInQuery {
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                IsPay = 1,
                SortBy = "Pid",
                UserName = this.UserName,
                PayWay = this.PayWay,
                CellPhone = this.phone,
                Atitle = this.atitle
            };
            if (this.state == 0)
            {
                query.state = 5;
            }
            else
            {
                query.state = 4;
            }
            DbQueryResult oneyuanPartInDataTable = OneyuanTaoHelp.GetOneyuanPartInDataTable(query);
            if (oneyuanPartInDataTable.Data != null)
            {
                DataTable data = (DataTable) oneyuanPartInDataTable.Data;
                data.Columns.Add("ActionBtn");
                data.Columns.Add("ASate");
                foreach (DataRow row in data.Rows)
                {
                    if ((bool) row["IsRefund"])
                    {
                        row["ASate"] = "<span class='green'>已退款</span>";
                    }
                    else if ((bool) row["RefundErr"])
                    {
                        row["ASate"] = "<span class='red'>退款错误</span>";
                    }
                    else
                    {
                        row["ASate"] = "<span class='red'>待退款</span>";
                    }
                    string str = string.Concat(new object[] { "<a class=\"btn btn-xs btn-danger\"  onclick=\"DoRefund('", row["Pid"], "','", row["PayWay"], "')\" >退款</a> " });
                    if ((bool) row["RefundErr"])
                    {
                        str = "<a class=\"btn btn-xs btn-primary\"  Remark='" + Globals.HtmlEncode(row["Remark"].ToString()) + "' onclick=\"AView(this)\"  >原因</a> " + str;
                    }
                    if ((bool) row["IsRefund"])
                    {
                        str = "";
                        if (row["RefundTime"] != DBNull.Value)
                        {
                            str = "<span>" + ((DateTime) row["RefundTime"]).ToString("yyyy-MM-dd") + "</span> ";
                        }
                        this.actionTd.InnerText = "退款时间";
                    }
                    row["ActionBtn"] = str;
                }
                this.Datalist.DataSource = data;
                this.Datalist.DataBind();
                this.pager.TotalRecords = oneyuanPartInDataTable.TotalRecords;
                int hasRefund = 0;
                this.ListWait.Text = "待退款(" + OneyuanTaoHelp.GetRefundTotalNum(out hasRefund).ToString() + ")";
                this.ListEnd.Text = "已退款(" + hasRefund.ToString() + ")";
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["atitle"]))
                {
                    this.atitle = base.Server.UrlDecode(this.Page.Request.QueryString["atitle"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PayWay"]))
                {
                    this.PayWay = base.Server.UrlDecode(this.Page.Request.QueryString["PayWay"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
                {
                    this.UserName = base.Server.UrlDecode(this.Page.Request.QueryString["UserName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["phone"]))
                {
                    this.phone = base.Server.UrlDecode(this.Page.Request.QueryString["phone"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
                {
                    int.TryParse(this.Page.Request.QueryString["state"], out this.state);
                }
                this.txtTitle.Text = this.atitle;
                this.txtPayWay.SelectedValue = this.PayWay;
                this.txtUserName.Text = this.UserName;
                this.txtPhone.Text = this.phone;
            }
            else
            {
                this.atitle = this.txtTitle.Text;
                int.TryParse(this.Page.Request.QueryString["state"], out this.state);
                this.PayWay = this.txtPayWay.SelectedValue;
                this.UserName = this.txtUserName.Text;
                this.phone = this.txtPhone.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.Form["action"];
            if (!string.IsNullOrEmpty(str))
            {
                this.AjaxAction(str);
                base.Response.End();
            }
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("atitle", this.atitle);
            queryStrings.Add("UserName", this.UserName);
            queryStrings.Add("PayWay", this.PayWay);
            queryStrings.Add("phone", this.phone);
            queryStrings.Add("state", this.state.ToString());
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

