namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
   using  global:: ControlPanel.Promotions;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ExChangeList : AdminPage
    {
        protected Button btnDelete;
        protected Button btnSeach;
        protected Button DelBtn;
        protected Repeater grdProducts;
        protected Label lblAll;
        protected Label lblEnd;
        protected Label lblIn;
        protected Label lblUnBegin;
        protected Pager pager;
        protected PageSize PageSize1;
        protected ExchangeStatus status;
        protected HtmlForm thisForm;
        protected TextBox txt_ids;
        protected TextBox txt_name;

        protected ExChangeList() : base("m08", "yxp02")
        {
        }

        private void BindData()
        {
            int total = 0;
            string text = this.txt_name.Text;
            ExChangeSearch search = new ExChangeSearch {
                status = this.status,
                ProductName = text,
                IsCount = true,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "Id",
                SortOrder = SortAction.Desc
            };
            DataTable table = PointExChangeHelper.Query(search, ref total);
            if (table != null)
            {
                table.Columns.Add("sStatus");
                table.Columns.Add("canChkStatus");
                if (table.Rows.Count > 0)
                {
                    for (int i = table.Rows.Count - 1; i >= 0; i--)
                    {
                        DateTime time = DateTime.Parse(table.Rows[i]["BeginDate"].ToString());
                        DateTime time2 = DateTime.Parse(table.Rows[i]["EndDate"].ToString());
                        if (time > DateTime.Now)
                        {
                            table.Rows[i]["sStatus"] = "未开始";
                            table.Rows[i]["canChkStatus"] = string.Empty;
                        }
                        else if ((time2 >= DateTime.Now) && (time <= DateTime.Now))
                        {
                            table.Rows[i]["sStatus"] = "进行中";
                            table.Rows[i]["canChkStatus"] = string.Empty;
                        }
                        else if (time2 < DateTime.Now)
                        {
                            table.Rows[i]["sStatus"] = "已结束";
                            table.Rows[i]["canChkStatus"] = "disabled";
                        }
                        if (table.Rows[i]["ExChangedNumber"].ToString() == "")
                        {
                            table.Rows[i]["ExChangedNumber"] = "0";
                        }
                    }
                }
            }
            this.grdProducts.DataSource = table;
            this.grdProducts.DataBind();
            this.pager.TotalRecords = total;
            this.CountTotal();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (!string.IsNullOrEmpty(str))
            {
                string[] strArray = str.Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    int num2 = 0;
                    if (!strArray[i].bInt(ref num2))
                    {
                        this.ShowMsg("选择活动出错！", false);
                        return;
                    }
                }
                for (int j = 0; j < strArray.Length; j++)
                {
                    PointExChangeHelper.Delete(int.Parse(strArray[j]));
                }
                this.ShowMsg("删除活动成功！", true);
                this.BindData();
            }
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void CountTotal()
        {
            int total = 0;
            ExChangeSearch search = new ExChangeSearch {
                status = ExchangeStatus.All,
                IsCount = true,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "Id",
                SortOrder = SortAction.Desc
            };
            DataTable table = PointExChangeHelper.Query(search, ref total);
            this.lblAll.Text = (table != null) ? table.Rows.Count.ToString() : "0";
            search.status = ExchangeStatus.In;
            table = PointExChangeHelper.Query(search, ref total);
            this.lblIn.Text = (table != null) ? table.Rows.Count.ToString() : "0";
            search.status = ExchangeStatus.End;
            table = PointExChangeHelper.Query(search, ref total);
            this.lblEnd.Text = (table != null) ? table.Rows.Count.ToString() : "0";
            search.status = ExchangeStatus.unBegin;
            table = PointExChangeHelper.Query(search, ref total);
            this.lblUnBegin.Text = (table != null) ? table.Rows.Count.ToString() : "0";
        }

        protected void DelBtn_Click(object sender, EventArgs e)
        {
            string text = this.txt_ids.Text;
            if (!string.IsNullOrEmpty(text))
            {
                string[] strArray = text.Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    int num2 = 0;
                    if (!strArray[i].bInt(ref num2))
                    {
                        this.ShowMsg("选择活动出错！", false);
                        return;
                    }
                }
                for (int j = 0; j < strArray.Length; j++)
                {
                    PointExChangeHelper.Delete(int.Parse(strArray[j]));
                }
                this.ShowMsg("删除活动成功！", true);
                this.BindData();
            }
        }

        private void grdProducts_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if ((e.CommandName == "Delete") && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                PointExChangeHelper.Delete(int.Parse(e.CommandArgument.ToString()));
                this.ShowMsg("删除活动成功！", true);
                this.BindData();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSeach.Click += new EventHandler(this.btnSeach_Click);
            this.DelBtn.Click += new EventHandler(this.DelBtn_Click);
            this.grdProducts.ItemCommand += new RepeaterCommandEventHandler(this.grdProducts_ItemCommand);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            if (base.Request.Params.AllKeys.Contains<string>("status"))
            {
                int i = 0;
                if (base.Request["status"].ToString().bInt(ref i))
                {
                    this.status = (ExchangeStatus) i;
                }
            }
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}

