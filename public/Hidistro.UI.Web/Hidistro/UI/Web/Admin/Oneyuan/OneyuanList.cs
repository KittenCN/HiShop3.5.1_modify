namespace Hidistro.UI.Web.Admin.Oneyuan
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Threading;
    using System.Web.UI.WebControls;

    public class OneyuanList : AdminPage
    {
        private string atitle;
        protected Button btnSearchButton;
        protected Repeater Datalist;
        protected PageSize hrefPageSize;
        protected Literal Listend;
        protected Literal ListStart;
        protected Literal ListTotal;
        protected Literal ListWait;
        protected Pager pager;
        private int ReachType;
        private int state;
        protected DropDownList txtReachType;
        protected DropDownList txtState;
        protected TextBox txtTitle;

        protected OneyuanList() : base("m08", "yxp20")
        {
            this.atitle = "";
        }

        private void AjaxAction(string action)
        {
            string[] strArray;
            OneyuanTaoInfo oneyuanTaoInfoById;
            string s = "{\"state\":false,\"msg\":\"未定义操作\"}";
            NameValueCollection form = base.Request.Form;
            string str2 = "";
            switch (action)
            {
                case "Del":
                    s = "{\"state\":false,\"msg\":\"活动信息未找到失败\"}";
                    str2 = form["Aid"];
                    if (!string.IsNullOrEmpty(str2) && (OneyuanTaoHelp.GetOneyuanTaoInfoById(str2) != null))
                    {
                        if (!OneyuanTaoHelp.DeleteOneyuanTao(str2))
                        {
                            s = "{\"state\":false,\"msg\":\"该活动已有人参与，不能删除！\"}";
                            break;
                        }
                        s = "{\"state\":true,\"msg\":\"删除成功\"}";
                        OneyuanTaoHelp.DelParticipantMember(str2, true);
                    }
                    break;

                case "BatchDel":
                    s = "{\"state\":false,\"msg\":\"批量删除失败\"}";
                    str2 = form["Aids"];
                    if (!string.IsNullOrEmpty(str2))
                    {
                        strArray = str2.Split(new char[] { ',' });
                        int num = OneyuanTaoHelp.BatchDeleteOneyuanTao(strArray);
                        if (num <= 0)
                        {
                            s = "{\"state\":false,\"msg\":\"没有找到可删除的数据！\"}";
                            break;
                        }
                        string[] strArray2 = new string[] { "{\"state\":true,\"msg\":\"成功删除", num.ToString(), "条数据，失败", (strArray.Length - num).ToString(), "条！\"}" };
                        s = string.Concat(strArray2);
                        foreach (string str3 in strArray)
                        {
                            OneyuanTaoHelp.DelParticipantMember(str3, true);
                        }
                    }
                    break;

                case "EndII":
                {
                    s = "{\"state\":false,\"msg\":\"结束失败\"}";
                    str2 = form["Aid"];
                    string str4 = form["CanDraw"];
                    if (!string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str4))
                    {
                        if (str4.Trim() == "1")
                        {
                            s = "{\"state\":false,\"msg\":\"开奖错误\"}";
                            string str5 = OneyuanTaoHelp.CalculateWinner(str2);
                            if (str5 == "success")
                            {
                                s = "{\"state\":true,\"msg\":\"手动开奖成功！\"}";
                            }
                            else
                            {
                                s = "{\"state\":false,\"msg\":\"" + str5 + "\"}";
                            }
                        }
                        else
                        {
                            s = "{\"state\":false,\"msg\":\"退款错误\"}";
                            if (OneyuanTaoHelp.SetOneyuanTaoIsOn(str2, false))
                            {
                                s = "{\"state\":true,\"msg\":\"提前终止活动成功！！\"}";
                                OneyuanTaoHelp.DelParticipantMember(str2, true);
                            }
                            else
                            {
                                s = "{\"state\":false,\"msg\":\"提前终止活动失败！\"}";
                            }
                        }
                        break;
                    }
                    s = "{\"state\":false,\"msg\":\"参数错误\"}";
                    break;
                }
                case "End":
                    s = "{\"state\":false,\"msg\":\"结束失败\"}";
                    str2 = form["Aid"];
                    if (!string.IsNullOrEmpty(str2))
                    {
                        oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(str2);
                        if (oneyuanTaoInfoById != null)
                        {
                            if (this.getOneTaoState(oneyuanTaoInfoById) != OneTaoState.进行中)
                            {
                                s = "{\"state\":false,\"msg\":\"提前终止活动失败！\"}";
                                break;
                            }
                            if (!OneyuanTaoHelp.SetOneyuanTaoIsOn(str2, false))
                            {
                                s = "{\"state\":false,\"msg\":\"提前终止活动失败！\"}";
                                break;
                            }
                            s = "{\"state\":true,\"msg\":\"提前终止活动成功！！\"}";
                            OneyuanTaoHelp.DelParticipantMember(str2, true);
                        }
                    }
                    break;

                case "Start":
                    s = "{\"state\":false,\"msg\":\"操作开始失败\"}";
                    str2 = form["Aid"];
                    if (!string.IsNullOrEmpty(str2))
                    {
                        oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(str2);
                        if (oneyuanTaoInfoById != null)
                        {
                            if (this.getOneTaoState(oneyuanTaoInfoById) != OneTaoState.未开始)
                            {
                                s = "{\"state\":false,\"msg\":\"当前状态开启活动！\"}";
                                break;
                            }
                            if (!OneyuanTaoHelp.SetOneyuanTaoIsOn(str2, true))
                            {
                                s = "{\"state\":false,\"msg\":\"当前状态不能结束！\"}";
                                break;
                            }
                            s = "{\"state\":true,\"msg\":\"提前开启活动成功！！\"}";
                        }
                    }
                    break;

                case "BatchStart":
                    s = "{\"state\":false,\"msg\":\"批量操作开始失败\"}";
                    str2 = form["Aids"];
                    if (!string.IsNullOrEmpty(str2))
                    {
                        strArray = str2.Split(new char[] { ',' });
                        int num2 = OneyuanTaoHelp.BatchSetOneyuanTaoIsOn(strArray, true);
                        if (num2 <= 0)
                        {
                            s = "{\"state\":false,\"msg\":\"没有找到可开启的活动数据！\"}";
                            break;
                        }
                        string[] strArray4 = new string[] { "{\"state\":true,\"msg\":\"成功开启", num2.ToString(), "条活动，失败", (strArray.Length - num2).ToString(), "条！\"}" };
                        s = string.Concat(strArray4);
                    }
                    break;
            }
            base.Response.ClearContent();
            base.Response.ContentType = "application/json";
            base.Response.Write(s);
            base.Response.End();
        }

        private void BindData()
        {
            OneyuanTaoQuery query = new OneyuanTaoQuery {
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                title = this.atitle,
                state = this.state,
                ReachType = this.ReachType
            };
            DbQueryResult oneyuanTao = OneyuanTaoHelp.GetOneyuanTao(query);
            if (oneyuanTao.Data != null)
            {
                DataTable data = (DataTable) oneyuanTao.Data;
                data.Columns.Add("ActionBtn");
                data.Columns.Add("ASate");
                data.Columns.Add("PrizeState");
                data.Columns.Add("CanDel");
                foreach (DataRow row in data.Rows)
                {
                    OneyuanTaoInfo info = OneyuanTaoHelp.DataRowToOneyuanTaoInfo(row);
                    OneTaoPrizeState state = OneyuanTaoHelp.getPrizeState(info);
                    row["PrizeState"] = state;
                    row["CanDel"] = 0;
                    switch (state)
                    {
                        case OneTaoPrizeState.成功开奖:
                            row["PrizeState"] = "<span class='success'>" + state + "<span>";
                            break;

                        case OneTaoPrizeState.已关闭:
                            row["PrizeState"] = "<span class='normal'>" + state + "<span>";
                            break;

                        case OneTaoPrizeState.待退款:
                            if (OneyuanTaoHelp.CheckIsAll(row["ActivityId"].ToString()))
                            {
                                info.IsAllRefund = true;
                                row["PrizeState"] = "<span class='errcss'>已退款<span>";
                            }
                            else
                            {
                                row["PrizeState"] = "<span class='green'>" + state + "<span>";
                            }
                            break;

                        default:
                            row["PrizeState"] = "<span class='errcss'>" + state + "<span>";
                            break;
                    }
                    OneTaoState state2 = OneyuanTaoHelp.getOneTaoState(info);
                    row["ASate"] = state2;
                    string str2 = "<a class=\"btn btn-xs btn-primary\" onclick=\"AView('" + row["ActivityId"] + "')\" >查看</a> ";
                    switch (state2)
                    {
                        case OneTaoState.进行中:
                        case OneTaoState.未开始:
                        {
                            object obj2 = str2;
                            str2 = string.Concat(new object[] { obj2, "<a class=\"btn btn-xs btn-primary\" onclick=\"AEdit('", row["ActivityId"], "')\"  >编辑</a> " });
                            break;
                        }
                    }
                    switch (state2)
                    {
                        case OneTaoState.未开始:
                        {
                            object obj3 = str2;
                            str2 = string.Concat(new object[] { obj3, "<a class=\"btn btn-xs btn-success\" onclick=\"AStart('", row["ActivityId"], "')\"  >开启</a> " });
                            break;
                        }
                        case OneTaoState.进行中:
                        {
                            object obj4 = str2;
                            str2 = string.Concat(new object[] { obj4, "<a class=\"btn btn-xs btn-danger\" onclick=\"AEnd('", row["ActivityId"], "','", row["FinishedNum"], "','", row["ReachType"], "','", row["ReachNum"], "')\"  >结束</a> " });
                            break;
                        }
                    }
                    if (((state2 == OneTaoState.已结束) && (((int) row["FinishedNum"]) == 0)) || ((state2 == OneTaoState.未开始) || (state2 == OneTaoState.退款完成)))
                    {
                        object obj5 = str2;
                        str2 = string.Concat(new object[] { obj5, "<a class=\"btn btn-xs btn-danger\" onclick=\"ADel('", row["ActivityId"], "')\" >删除</a> " });
                        row["CanDel"] = 1;
                    }
                    if (state2 == OneTaoState.开奖失败)
                    {
                        object obj6 = str2;
                        str2 = string.Concat(new object[] { obj6, "<a class=\"btn btn-xs btn-danger\" onclick=\"BatchRefund('", row["ActivityId"], "')\" >批量退款</a> " });
                    }
                    row["ActionBtn"] = str2;
                }
                this.Datalist.DataSource = data;
                this.Datalist.DataBind();
                this.pager.TotalRecords = oneyuanTao.TotalRecords;
                int hasStart = 0;
                int waitStart = 0;
                int hasEnd = 0;
                this.ListTotal.Text = "所有夺宝(" + OneyuanTaoHelp.GetOneyuanTaoTotalNum(out hasStart, out waitStart, out hasEnd).ToString() + ")";
                this.ListStart.Text = "进行中(" + hasStart.ToString() + ")";
                this.ListWait.Text = "未开始(" + waitStart.ToString() + ")";
                this.Listend.Text = "已结束(" + hasEnd.ToString() + ")";
            }
            this.pager.TotalRecords = oneyuanTao.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private OneTaoState getOneTaoState(OneyuanTaoInfo selitem)
        {
            return OneyuanTaoHelp.getOneTaoState(selitem);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["atitle"]))
                {
                    this.atitle = base.Server.UrlDecode(this.Page.Request.QueryString["atitle"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ReachType"]))
                {
                    int.TryParse(this.Page.Request.QueryString["ReachType"], out this.ReachType);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
                {
                    int.TryParse(this.Page.Request.QueryString["state"], out this.state);
                }
                this.txtTitle.Text = this.atitle;
                this.txtReachType.SelectedValue = this.ReachType.ToString();
                this.txtState.SelectedValue = this.state.ToString();
            }
            else
            {
                this.atitle = this.txtTitle.Text;
                int.TryParse(this.txtReachType.SelectedItem.Value, out this.ReachType);
                int.TryParse(this.txtState.SelectedItem.Value, out this.state);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            new Thread(() => OneyuanTaoHelp.CalculateWinner("")).Start();
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
            queryStrings.Add("ReachType", this.ReachType.ToString());
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

