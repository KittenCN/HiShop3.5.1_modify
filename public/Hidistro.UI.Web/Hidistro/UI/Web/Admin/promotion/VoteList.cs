namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
   using  global:: ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class VoteList : AdminPage
    {
        protected Button btnSeach;
        protected Button DelBtn;
        protected Repeater grdDate;
        protected PageSize hrefPageSize;
        protected Label lblEnd;
        protected Label lblIn;
        protected Label lblUnBegin;
        protected Pager pager;
        protected VoteStatus status;
        protected HtmlForm thisForm;
        protected TextBox txt_Ids;
        protected TextBox txt_name;

        protected VoteList() : base("m08", "yxp06")
        {
            this.status = VoteStatus.In;
        }

        private void BindData()
        {
            string str = this.txt_name.Text.Trim();
            VoteSearch query = new VoteSearch {
                status = this.status,
                IsCount = true,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "VoteId",
                SortOrder = SortAction.Desc,
                Name = str
            };
            DbQueryResult result = VoteHelper.Query(query);
            DataTable data = (DataTable) result.Data;
            if (data != null)
            {
                data.Columns.Add("sStatus");
                data.Columns.Add("sType");
                data.Columns.Add("sAttend");
                string str2 = "";
                if (data.Rows.Count > 0)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        DateTime time = DateTime.Parse(row["startDate"].ToString());
                        DateTime time2 = DateTime.Parse(row["endDate"].ToString());
                        bool flag = bool.Parse(row["IsMultiCheck"].ToString());
                        int num = int.Parse(row["voteId"].ToString());
                        if (time > DateTime.Now)
                        {
                            row["sStatus"] = "未开始";
                        }
                        else if (time2 < DateTime.Now)
                        {
                            row["sStatus"] = "已结束";
                        }
                        else
                        {
                            row["sStatus"] = "进行中";
                        }
                        if (flag)
                        {
                            row["sType"] = "多选";
                        }
                        else
                        {
                            row["sType"] = "单选";
                        }
                        row["sAttend"] = VoteHelper.GetVoteAttends((long) num);
                        str2 = str2 + ", " + num.ToString();
                    }
                    if (str2.Length > 1)
                    {
                        str2 = str2.Substring(1);
                    }
                }
            }
            this.grdDate.DataSource = data;
            this.grdDate.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
            this.CountTotal();
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void CountTotal()
        {
            VoteSearch query = new VoteSearch {
                //status = this.status,
                IsCount = true,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "VoteId",
                SortOrder = SortAction.Desc,
                status = VoteStatus.In
            };
            DbQueryResult result = VoteHelper.Query(query);
            this.lblIn.Text = (result.Data != null) ? result.TotalRecords.ToString() : "0";
            query.status = VoteStatus.End;
            result = VoteHelper.Query(query);
            this.lblEnd.Text = (result.Data != null) ? result.TotalRecords.ToString() : "0";
            query.status = VoteStatus.unBegin;
            result = VoteHelper.Query(query);
            this.lblUnBegin.Text = (result.Data != null) ? result.TotalRecords.ToString() : "0";
        }

        protected void DelBtn_Click(object sender, EventArgs e)
        {
            string text = this.txt_Ids.Text;
            if (text.Length > 1)
            {
                text = text.Substring(1);
            }
            foreach (string str2 in text.Split(new char[] { ',' }))
            {
                VoteHelper.Delete((long) int.Parse(str2));
            }
            this.BindData();
            this.ShowMsg("批量删除成功！", true);
        }

        public string GetUrl(object voteId)
        {
            return string.Concat(new object[] { "http://", Globals.DomainName, Globals.ApplicationPath, "/Vshop/Vote.aspx?voteId=", voteId });
        }

        private void grdDate_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if ((e.CommandName == "Delete") && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                if (VoteHelper.Delete((long) int.Parse(e.CommandArgument.ToString())))
                {
                    this.ShowMsg("删除成功！", true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("删除失败！", false);
                }
            }
        }

        protected void lkDelete_Click(object sender, EventArgs e)
        {
            string text = this.txt_Ids.Text;
            this.txt_Ids.Text = "";
            int i = 0;
            if (text.bInt(ref i))
            {
                if (VoteHelper.Delete((long) i))
                {
                    this.ShowMsg("删除成功！", true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("删除失败！", false);
                }
            }
        }

        protected void lkStart_Click(object sender, EventArgs e)
        {
            string text = this.txt_Ids.Text;
            this.txt_Ids.Text = "";
            int i = 0;
            if (text.bInt(ref i))
            {
                VoteInfo vote = VoteHelper.GetVote((long) i);
                vote.StartDate = DateTime.Now;
                if (VoteHelper.Update(vote, false))
                {
                    this.ShowMsg("开启成功！", true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("开启失败！", false);
                }
            }
        }

        protected void lkStop_Click(object sender, EventArgs e)
        {
            string text = this.txt_Ids.Text;
            this.txt_Ids.Text = "";
            int i = 0;
            if (text.bInt(ref i))
            {
                VoteInfo vote = VoteHelper.GetVote((long) i);
                vote.EndDate = DateTime.Now;
                if (VoteHelper.Update(vote, false))
                {
                    this.ShowMsg("结束成功！", true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("结束失败！", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdDate.ItemCommand += new RepeaterCommandEventHandler(this.grdDate_ItemCommand);
            if (base.Request.Params.AllKeys.Contains<string>("status"))
            {
                int i = 0;
                if (base.Request["status"].ToString().bInt(ref i))
                {
                    this.status = (VoteStatus) i;
                }
            }
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}

