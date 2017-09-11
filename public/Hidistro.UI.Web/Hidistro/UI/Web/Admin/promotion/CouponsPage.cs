namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class CouponsPage : Page
    {
        protected static bool bAllProduct = false;
        protected static bool bFininshed = false;
        protected Button btnSeach;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected HtmlForm form1;
        protected Grid grdCoupondsList;
        protected static int pageIndex = 1;
        protected Pager pager1;
        protected static int pagesize = 20;
        protected TextBox txt_maxVal;
        protected TextBox txt_minVal;
        protected TextBox txt_name;

        private bool bBool(string val, ref bool i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return bool.TryParse(val, out i);
        }

        private bool bDate(string val, ref DateTime i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return DateTime.TryParse(val, out i);
        }

        private bool bDecimal(string val, ref decimal i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return decimal.TryParse(val, out i);
        }

        private void BindData()
        {
            string text = "";
            decimal? nullable = null;
            decimal? nullable2 = null;
            DateTime? nullable3 = null;
            DateTime? nullable4 = null;
            text = this.txt_name.Text;
            decimal i = 0M;
            DateTime now = DateTime.Now;
            if (this.bDecimal(this.txt_minVal.Text, ref i))
            {
                nullable = new decimal?(i);
            }
            if (this.bDecimal(this.txt_maxVal.Text, ref i))
            {
                nullable2 = new decimal?(i);
            }
            if (this.bDate(this.calendarStartDate.Text, ref now))
            {
                nullable3 = new DateTime?(now);
            }
            if (this.bDate(this.calendarEndDate.Text, ref now))
            {
                nullable4 = new DateTime?(now);
            }
            CouponsSearch search = new CouponsSearch {
                CouponName = text,
                minValue = nullable,
                maxValue = nullable2,
                beginDate = nullable3,
                endDate = nullable4,
                IsCount = true,
                PageIndex = this.pager1.PageIndex,
                PageSize = this.pager1.PageSize,
                SortBy = "CouponId",
                SortOrder = SortAction.Desc
            };
            DbQueryResult couponInfos = CouponHelper.GetCouponInfos(search);
            DataTable data = (DataTable) couponInfos.Data;
            if (data.Rows.Count > 0)
            {
                data.Columns.Add("useConditon");
                data.Columns.Add("ReceivNum");
                for (int j = 0; j < data.Rows.Count; j++)
                {
                    decimal num3 = decimal.Parse(data.Rows[j]["ConditionValue"].ToString());
                    if (num3 == 0M)
                    {
                        data.Rows[j]["useConditon"] = "不限制";
                    }
                    else
                    {
                        data.Rows[j]["useConditon"] = "满" + num3.ToString("F2") + "可使用";
                    }
                    string str2 = data.Rows[j]["maxReceivNum"].ToString();
                    if (str2 == "0")
                    {
                        data.Rows[j]["ReceivNum"] = "无限制";
                    }
                    else
                    {
                        data.Rows[j]["ReceivNum"] = str2 + "/张每人";
                    }
                }
            }
            this.grdCoupondsList.DataSource = data;
            this.grdCoupondsList.DataBind();
            this.pager1.TotalRecords = couponInfos.TotalRecords;
        }

        private bool bInt(string val, ref int i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            if (val.Contains(".") || val.Contains("-"))
            {
                return false;
            }
            return int.TryParse(val, out i);
        }

        protected void btnImagetSearch_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSeach.Click += new EventHandler(this.btnImagetSearch_Click);
            if (!base.IsPostBack)
            {
                string[] allKeys = base.Request.Params.AllKeys;
                if (allKeys.Contains<string>("pagesize") && !this.bInt(base.Request.Params["pagesize"].ToString(), ref pagesize))
                {
                    pagesize = 20;
                }
                if (allKeys.Contains<string>("pageIndex") && !this.bInt(base.Request.Params["pageIndex"].ToString(), ref pageIndex))
                {
                    pageIndex = 1;
                }
                if (allKeys.Contains<string>("bAllProduct") && !this.bBool(base.Request.Params["bAllProduct"].ToString(), ref bAllProduct))
                {
                    bAllProduct = false;
                }
                if (allKeys.Contains<string>("bFininshed") && !this.bBool(base.Request.Params["bFininshed"].ToString(), ref bFininshed))
                {
                    bFininshed = false;
                }
                this.BindData();
            }
        }
    }
}

