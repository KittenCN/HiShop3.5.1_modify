using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.SaleSystem.Vshop;
using Hidistro.Entities.VShop;
using System.Collections.Specialized;
using Hidistro.Core.Entities;
using System.Data;
using System.Globalization;
using System.Threading;

namespace Hidistro.UI.Web.Admin.Oneyuan
{
    public partial class OneyuanList : AdminPage
    {

        protected OneyuanList(): base("m08", "yxp20")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //测试处
            new Thread(delegate()
            {
                OneyuanTaoHelp.CalculateWinner(); //进入时，触发开结任务
            }).Start();


            string action = Request.Form["action"];

            if (!string.IsNullOrEmpty(action))
            {
                AjaxAction(action); //处理AJAX请求
                Response.End();
            }


            ///
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindData();
            }
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);

        }

        OneTaoState getOneTaoState(OneyuanTaoInfo selitem)
        {
            return OneyuanTaoHelp.getOneTaoState(selitem);
        }

        void AjaxAction(string action)
        {
            string rs = "{\"state\":false,\"msg\":\"未定义操作\"}";
            NameValueCollection Form = Request.Form;
            string Aid = "";
            string[] Aids ;
            OneyuanTaoInfo selitem;
            OneTaoState rowState;

            switch (action)
            {

                case "Del":
                    rs = "{\"state\":false,\"msg\":\"活动信息未找到失败\"}";

                     Aid = Form["Aid"];
                    if (!string.IsNullOrEmpty(Aid)) 
                    {
                        selitem = OneyuanTaoHelp.GetOneyuanTaoInfoById(Aid);
                        if (selitem != null)
                        {
                            if (OneyuanTaoHelp.DeleteOneyuanTao(Aid))
                            {
                                rs = "{\"state\":true,\"msg\":\"删除成功\"}";
                                OneyuanTaoHelp.DelParticipantMember(Aid); //清除未支付的用户信息
                            }

                            else
                            {
                                rs = "{\"state\":false,\"msg\":\"该活动已有人参与，不能删除！\"}";
                            }     
                        }
                    }
                    break;
                case "BatchDel":
                    rs = "{\"state\":false,\"msg\":\"批量删除失败\"}";
                    Aid = Form["Aids"];

                    if (!string.IsNullOrEmpty(Aid))
                    {
                        Aids = Aid.Split(',');
                        int delnum = OneyuanTaoHelp.BatchDeleteOneyuanTao(Aids);

                        if (delnum > 0)
                        {
                            rs = "{\"state\":true,\"msg\":\"成功删除" + delnum.ToString() + "条数据，失败" + (Aids.Length - delnum).ToString() + "条！\"}";

                            foreach (string tAid in Aids)
                            {
                                OneyuanTaoHelp.DelParticipantMember(tAid); //清除未支付的用户信息
                            }
                        
                        }
                        else
                        {
                            rs = "{\"state\":false,\"msg\":\"没有找到可删除的数据！\"}";
                        }

                    }

                    break;
                case "EndII":
                    //有参与人数的情况下，提前终结活动
                    rs = "{\"state\":false,\"msg\":\"结束失败\"}";
                    Aid = Form["Aid"];
                    string CanDraw = Form["CanDraw"];

                    if (string.IsNullOrEmpty(Aid) || string.IsNullOrEmpty(CanDraw))
                    {
                        rs = "{\"state\":false,\"msg\":\"参数错误\"}";
                    }
                    else
                    {
                        if (CanDraw.Trim() == "1")
                        {
                            rs = "{\"state\":false,\"msg\":\"开奖错误\"}";
                            string DrawRs = OneyuanTaoHelp.CalculateWinner(Aid);
                            if (DrawRs == "success")
                            {
                                rs = "{\"state\":true,\"msg\":\"手动开奖成功！\"}";
                            }
                            else
                            {
                                rs = "{\"state\":false,\"msg\":\"" + DrawRs + "\"}";
                            }
                        }
                        else
                        {
                            rs = "{\"state\":false,\"msg\":\"退款错误\"}";
                            if (OneyuanTaoHelp.SetOneyuanTaoIsOn(Aid, false))
                            {
                                rs = "{\"state\":true,\"msg\":\"提前终止活动成功！！\"}";
                                //提前终止，表示活动失败
                                OneyuanTaoHelp.DelParticipantMember(Aid); //清除未支付的用户信息
                                //没有人参与时

                            }
                            else
                            {
                                rs = "{\"state\":false,\"msg\":\"提前终止活动失败！\"}";
                            }
                        }

                    }



                    break;
                case "End":
                    //无参与人员的情况下，结束活动
                    rs = "{\"state\":false,\"msg\":\"结束失败\"}";
                     Aid = Form["Aid"];
                    if (!string.IsNullOrEmpty(Aid)) 
                    {
      
                        selitem = OneyuanTaoHelp.GetOneyuanTaoInfoById(Aid);
                        if (selitem != null)
                        {
                            rowState = getOneTaoState(selitem);

                            if (rowState == OneTaoState.进行中)
                            {
                                //关闭活动，自动结束
                                if (OneyuanTaoHelp.SetOneyuanTaoIsOn(Aid, false))
                                {
                                    rs = "{\"state\":true,\"msg\":\"提前终止活动成功！！\"}";
                                    //提前终止，表示活动失败

                                    OneyuanTaoHelp.DelParticipantMember(Aid); //清除未支付的用户信息
                                    //没有人参与时
                                 
                                }
                                else
                                {
                                    rs = "{\"state\":false,\"msg\":\"提前终止活动失败！\"}";
                                }
                            }
                            else
                            {
                                rs = "{\"state\":false,\"msg\":\"提前终止活动失败！\"}";
                            }

                        }
                    }
                    break;
                case "Start":
                    rs = "{\"state\":false,\"msg\":\"操作开始失败\"}";
                     Aid = Form["Aid"];
                    if (!string.IsNullOrEmpty(Aid)) 
                    {
                        selitem = OneyuanTaoHelp.GetOneyuanTaoInfoById(Aid);
                        if (selitem != null)
                        {
                            rowState = getOneTaoState(selitem);

                            if (rowState == OneTaoState.未开始)
                            {
                                //开启活动，如果时间没到，重设活动开始时间为当前时间
                                if (OneyuanTaoHelp.SetOneyuanTaoIsOn(Aid, true))
                                {
                                    rs = "{\"state\":true,\"msg\":\"提前开启活动成功！！\"}";
                                    //提前终止，表示活动失败



                                    //进行活动终止的相关处理
                                }
                                else
                                {
                                    rs = "{\"state\":false,\"msg\":\"当前状态不能结束！\"}";
                                }

                            }
                            else
                            {
                                rs = "{\"state\":false,\"msg\":\"当前状态开启活动！\"}";
                            }

                        }
                    }
                    break;
                case "BatchStart":
                    rs = "{\"state\":false,\"msg\":\"批量操作开始失败\"}";
                    Aid = Form["Aids"];

                    if (!string.IsNullOrEmpty(Aid))
                    {
                        Aids = Aid.Split(',');
                        int Startnum = OneyuanTaoHelp.BatchSetOneyuanTaoIsOn(Aids,true);

                        if (Startnum > 0)
                        {
                            rs = "{\"state\":true,\"msg\":\"成功开启" + Startnum.ToString() + "条活动，失败" + (Aids.Length - Startnum).ToString() + "条！\"}";
                        }
                        else
                        {
                            rs = "{\"state\":false,\"msg\":\"没有找到可开启的活动数据！\"}";
                        }

                    }
                    break;
                default:
                    break;

            }
            Response.ClearContent();
            Response.ContentType = "application/json";
            Response.Write(rs);
            Response.End();

        }


        void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }


        string atitle = "";
        int ReachType = 0;
        int state = 0;

        void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["atitle"]))
                    atitle = Server.UrlDecode(Page.Request.QueryString["atitle"]);
                if (!string.IsNullOrEmpty(Page.Request.QueryString["ReachType"]))
                    int.TryParse(Page.Request.QueryString["ReachType"], out ReachType);
                if (!string.IsNullOrEmpty(Page.Request.QueryString["state"]))
                    int.TryParse(Page.Request.QueryString["state"], out state);
                txtTitle.Text = atitle;
                txtReachType.SelectedValue = ReachType.ToString();
                txtState.SelectedValue = state.ToString();
            }
            else
            {
                atitle = txtTitle.Text;
                int.TryParse(txtReachType.SelectedItem.Value, out ReachType);
                int.TryParse(txtState.SelectedItem.Value, out state);
            }
        }


        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("atitle", atitle);
            queryStrings.Add("ReachType", ReachType.ToString());
            queryStrings.Add("state", state.ToString());
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            ReloadPage(queryStrings);
        }

        void BindData()
        {
            OneyuanTaoQuery query = new OneyuanTaoQuery();
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            query.title = atitle;
            query.state = state;
            query.ReachType = ReachType;
            DbQueryResult ListData = OneyuanTaoHelp.GetOneyuanTao(query);

            if (ListData.Data != null)
            {
                DataTable dt = (DataTable)ListData.Data;
                dt.Columns.Add("ActionBtn"); //显示的按钮
                dt.Columns.Add("ASate");  //当前状态
                dt.Columns.Add("PrizeState"); //开奖状态
                dt.Columns.Add("CanDel"); //开奖状态
                 
                foreach (DataRow Item in dt.Rows)
                {
                    OneyuanTaoInfo tempItem = OneyuanTaoHelp.DataRowToOneyuanTaoInfo(Item);
                    OneTaoState rowState = OneyuanTaoHelp.getOneTaoState(tempItem);
                    OneTaoPrizeState rowPrizeState = OneyuanTaoHelp.getPrizeState(tempItem);

                    Item["ASate"] = rowState;
                    Item["PrizeState"] = rowPrizeState;
                    Item["CanDel"] = 0;

                    if (rowPrizeState == OneTaoPrizeState.成功开奖)
                    {
                        Item["PrizeState"] ="<span class='success'>"+rowPrizeState+"<span>";
                    }
                    else if (rowPrizeState == OneTaoPrizeState.已关闭)
                    {
                        Item["PrizeState"] = "<span class='normal'>" + rowPrizeState + "<span>";
                    }
                    else if (rowPrizeState == OneTaoPrizeState.待退款)
                    {
                        Item["PrizeState"] = "<span class='green'>" + rowPrizeState + "<span>";
                    }
                    else
                    {
                        Item["PrizeState"] = "<span class='errcss'>" + rowPrizeState + "<span>";
                    }
                    

                    string btnstr = "<a class=\"btn btn-xs btn-primary\" onclick=\"AView('" + Item["ActivityId"] + "')\" >查看</a> ";

                    if (rowState == OneTaoState.进行中 || rowState == OneTaoState.未开始)
                    {
                        btnstr += "<a class=\"btn btn-xs btn-primary\" onclick=\"AEdit('" + Item["ActivityId"] + "')\"  >编辑</a> ";
                    }


                    if (  rowState == OneTaoState.未开始)
                    {
                        btnstr += "<a class=\"btn btn-xs btn-success\" onclick=\"AStart('" + Item["ActivityId"] + "')\"  >开启</a> ";
                    }

                    if (rowState == OneTaoState.进行中)
                    {
                        btnstr += "<a class=\"btn btn-xs btn-danger\" onclick=\"AEnd('" + Item["ActivityId"] + "','" + Item["FinishedNum"] + "','" + Item["ReachType"] + "','" + Item["ReachNum"] + "')\"  >结束</a> ";
                    }


                    if ((rowState == OneTaoState.已结束 && (int)Item["FinishedNum"]==0) || rowState == OneTaoState.未开始)
                    {
                        //可删除的活动，1未开始，2已结束且没有人参与的活动

                        btnstr += "<a class=\"btn btn-xs btn-danger\" onclick=\"ADel('" + Item["ActivityId"] + "')\" >删除</a> ";
                        Item["CanDel"] = 1;
                    }


                    if (rowState == OneTaoState.开奖失败)
                    {
                        btnstr += "<a class=\"btn btn-xs btn-danger\" onclick=\"BatchRefund('" + Item["ActivityId"] + "')\" >批量退款</a> ";
                    }

                    Item["ActionBtn"] = btnstr;

                }


                Datalist.DataSource = dt;
                Datalist.DataBind();
                pager.TotalRecords = ListData.TotalRecords;

                int hasStart = 0;
                int waitStart = 0;
                int Total = 0;
                int hasEnd = 0;

                Total = OneyuanTaoHelp.GetOneyuanTaoTotalNum(out hasStart, out waitStart, out hasEnd);

                ListTotal.Text = "所有夺宝("+Total.ToString()+")";
                ListStart.Text = "进行中(" + hasStart.ToString() + ")";
                ListWait.Text = "未开始(" + waitStart.ToString() + ")";
                Listend.Text = "已结束(" + hasEnd.ToString() + ")";

            }

           

            //DataTable dt = (DataTable)productSet.Data;
            //dt.Columns.Add("setStatus");

            pager.TotalRecords = ListData.TotalRecords;
        }


    }
}