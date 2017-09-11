namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
   using  global:: ControlPanel.Promotions;
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

    public class ShareActList : AdminPage
    {
        protected Button btnSeach;
        protected Button DelBtn;
        protected Repeater grdDate;
        protected Label lblAll;
        protected Label lblEnd;
        protected Label lblIn;
        protected Label lblUnBegin;
        protected Button lkDelete;
        protected Pager pager1;
        protected PageSize PageSize1;
        protected ShareActivityStatus status;
        protected HtmlForm thisForm;
        protected TextBox txt_Ids;
        protected TextBox txt_name;

        protected ShareActList() : base("m08", "yxp04")
        {
        }

        private void BindData()
        {
            string name = this.txt_name.Text.Trim();
            ShareActivitySearch query = new ShareActivitySearch {
                status = this.status,
                IsCount = true,
                PageIndex = this.pager1.PageIndex,
                PageSize = this.pager1.PageSize,
                SortBy = "Id",
                SortOrder = SortAction.Desc,
                CouponName = name
            };
            DbQueryResult result = ShareActHelper.Query(query);
            DataTable data = (DataTable) result.Data;
            this.grdDate.DataSource = data;
            this.grdDate.DataBind();
            this.pager1.TotalRecords = result.TotalRecords;
            this.CountTotal(name);
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void CountTotal(string name)
        {
            ShareActivitySearch query = new ShareActivitySearch {
                status = ShareActivityStatus.All,
                IsCount = true,
                PageIndex = this.pager1.PageIndex,
                PageSize = this.pager1.PageSize,
                SortBy = "Id",
                SortOrder = SortAction.Desc,
                CouponName = name
            };
            DataTable data = (DataTable) ShareActHelper.Query(query).Data;
            this.lblAll.Text = (data != null) ? data.Rows.Count.ToString() : "0";
            query.status = ShareActivityStatus.In;
            data = (DataTable) ShareActHelper.Query(query).Data;
            this.lblIn.Text = (data != null) ? data.Rows.Count.ToString() : "0";
            query.status = ShareActivityStatus.End;
            data = (DataTable) ShareActHelper.Query(query).Data;
            this.lblEnd.Text = (data != null) ? data.Rows.Count.ToString() : "0";
            query.status = ShareActivityStatus.unBegin;
            data = (DataTable) ShareActHelper.Query(query).Data;
            this.lblUnBegin.Text = (data != null) ? data.Rows.Count.ToString() : "0";
        }

        protected void DelBtn_Click(object sender, EventArgs e)
        {
        }

        private void grdDate_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if ((e.CommandName == "Delete") && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                ShareActHelper.Delete(int.Parse(e.CommandArgument.ToString()));
                this.BindData();
            }
        }

        protected void lkDelete_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择活动！", false);
            }
            else
            {
                if (str.Length > 1)
                {
                    str = str.TrimStart(new char[] { ',' }).TrimEnd(new char[] { ',' });
                }
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    ShareActHelper.Delete(int.Parse(str2));
                }
                this.ShowMsg("批量删除成功！", true);
                this.BindData();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSeach.Click += new EventHandler(this.btnSeach_Click);
            this.DelBtn.Click += new EventHandler(this.DelBtn_Click);
            this.lkDelete.Click += new EventHandler(this.lkDelete_Click);
            this.grdDate.ItemCommand += new RepeaterCommandEventHandler(this.grdDate_ItemCommand);
            if (base.Request.Params.AllKeys.Contains<string>("status"))
            {
                int i = 0;
                if (base.Request["status"].ToString().bInt(ref i))
                {
                    this.status = (ShareActivityStatus) i;
                }
            }
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}

