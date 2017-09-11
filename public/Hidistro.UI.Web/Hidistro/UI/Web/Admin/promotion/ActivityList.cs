namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
    using global::ControlPanel.Promotions;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ActivityList : AdminPage
    {
        protected int _status;
        protected Button btnDelete;
        protected Button btnSeach;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected Button DelBtn;
        protected Grid grdCoupondsList;
        protected PageSize hrefPageSize;
        protected Label lblAll;
        protected Label lblEnd;
        protected Label lblIn;
        protected Label lblUnBegin;
        protected Pager pager1;
        protected HtmlForm thisForm;
        protected TextBox txt_name;

        protected ActivityList() : base("m08", "yxp05")
        {
        }

        private void BindData()
        {
            string text = this.txt_name.Text;
            DateTime? selectedDate = null;
            DateTime? nullable2 = null;
            selectedDate = this.calendarStartDate.SelectedDate;
            nullable2 = this.calendarEndDate.SelectedDate;
            ActivitySearch query = new ActivitySearch {
                status = (ActivityStatus) this._status,
                IsCount = true,
                PageIndex = this.pager1.PageIndex,
                PageSize = this.pager1.PageSize,
                SortBy = "ActivitiesId",
                SortOrder = SortAction.Desc,
                Name = text,
                begin = selectedDate,
                end = nullable2
            };
            DbQueryResult result = ActivityHelper.Query(query);
            DataTable data = (DataTable) result.Data;
            if (data != null)
            {
                data.Columns.Add("sStatus");
                if (data.Rows.Count > 0)
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        DateTime time = DateTime.Parse(data.Rows[i]["StartTime"].ToString());
                        DateTime time2 = DateTime.Parse(data.Rows[i]["EndTime"].ToString());
                        if (time > DateTime.Now)
                        {
                            data.Rows[i]["sStatus"] = "未开始";
                        }
                        else if (time2 < DateTime.Now)
                        {
                            data.Rows[i]["sStatus"] = "已结束";
                        }
                        else
                        {
                            data.Rows[i]["sStatus"] = "进行中";
                        }
                    }
                }
            }
            this.grdCoupondsList.DataSource = data;
            this.grdCoupondsList.DataBind();
            this.pager1.TotalRecords = result.TotalRecords;
            this.CountTotal();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int>();
            foreach (GridViewRow row in this.grdCoupondsList.Rows)
            {
                if (row.RowIndex >= 0)
                {
                    CheckBox box = row.Cells[0].Controls[0] as CheckBox;
                    if (box.Checked)
                    {
                        list.Add(int.Parse(this.grdCoupondsList.DataKeys[row.RowIndex].Value.ToString()));
                    }
                }
            }
            if (list.Count <= 0)
            {
                this.ShowMsg("请至少选择一条要删除的数据！", false);
            }
            else
            {
                foreach (int num in list)
                {
                    ActivityHelper.Delete(num);
                }
                this.BindData();
                this.ShowMsg("成功删除了选择的活动", true);
            }
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void CountTotal()
        {
            ActivitySearch query = new ActivitySearch {
                status = ActivityStatus.All,
                IsCount = true,
                PageIndex = this.pager1.PageIndex,
                PageSize = this.pager1.PageSize,
                SortBy = "ActivitiesId",
                SortOrder = SortAction.Desc
            };
            DataTable data = (DataTable) ActivityHelper.Query(query).Data;
            this.lblAll.Text = (data != null) ? data.Rows.Count.ToString() : "0";
            query.status = ActivityStatus.In;
            data = (DataTable) ActivityHelper.Query(query).Data;
            this.lblIn.Text = (data != null) ? data.Rows.Count.ToString() : "0";
            query.status = ActivityStatus.End;
            data = (DataTable) ActivityHelper.Query(query).Data;
            this.lblEnd.Text = (data != null) ? data.Rows.Count.ToString() : "0";
            query.status = ActivityStatus.unBegin;
            data = (DataTable) ActivityHelper.Query(query).Data;
            this.lblUnBegin.Text = (data != null) ? data.Rows.Count.ToString() : "0";
        }

        protected void DelRows()
        {
            foreach (GridViewRow row in this.grdCoupondsList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    ActivityHelper.Delete(Convert.ToInt32(this.grdCoupondsList.DataKeys[row.RowIndex].Value));
                }
            }
            this.BindData();
            this.ShowMsg("成功删除了选择的活动", true);
        }

        private void grdCoupondsList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = (int) this.grdCoupondsList.DataKeys[e.RowIndex].Value;
            if (!ActivityHelper.Delete(id))
            {
                this.ShowMsg("未知错误", false);
            }
            else
            {
                this.BindData();
                this.ShowMsg("成功删除了选择的活动", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdCoupondsList.RowDeleting += new GridViewDeleteEventHandler(this.grdCoupondsList_RowDeleting);
            this.btnSeach.Click += new EventHandler(this.btnSeach_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            if (base.Request.Params.AllKeys.Contains<string>("status"))
            {
                if (!base.Request["status"].ToString().bInt(ref this._status))
                {
                    this._status = 0;
                }
            }
            else
            {
                this._status = 0;
            }
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            this.DelRows();
        }
    }
}

