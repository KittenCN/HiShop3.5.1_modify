namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class CouponsList : AdminPage
    {
        protected static bool bFininshed = false;
        protected Button btnDelete;
        protected Button btnSeach;
        protected Button btnSubmit;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarEndDate2;
        protected ucDateTimePicker calendarStartDate;
        protected ucDateTimePicker calendarStartDate2;
        protected DropDownList ddl_maxNum;
        protected DropDownList ddlCouponType;
        protected Button DelBtn;
        protected Grid grdCoupondsList;
        protected PageSize hrefPageSize;
        protected HtmlInputHidden htxtRoleId;
        protected bool isFinished;
        protected static int pageIndex = 1;
        protected Pager pager1;
        protected static int pagesize = 10;
        protected string reUrl;
        protected HtmlForm thisForm;
        protected DateTime time;
        protected TextBox txt_id;
        protected TextBox txt_ids;
        protected TextBox txt_maxVal;
        protected TextBox txt_minVal;
        protected TextBox txt_name;
        protected TextBox txt_totalNum;
        protected TextBox txt_used;

        protected CouponsList() : base("m08", "yxp01")
        {
            this.time = DateTime.Now;
            this.reUrl = string.Empty;
        }

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

        private void BindCouonType()
        {
            this.ddlCouponType.Items.Clear();
            ListItem item2 = new ListItem {
                Value = "",
                Text = "优惠券类型"
            };
            this.ddlCouponType.Items.Add(item2);
            foreach (int num in Enum.GetValues(typeof(CouponType)))
            {
                ListItem item = new ListItem {
                    Value = num.ToString(),
                    Text = ((CouponType) num).ToString()
                };
                this.ddlCouponType.Items.Add(item);
            }
        }

        private void BindData()
        {
            string text = "";
            string selectedValue = "";
            decimal? nullable = null;
            decimal? nullable2 = null;
            DateTime? selectedDate = null;
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
            selectedDate = this.calendarStartDate.SelectedDate;
            nullable4 = this.calendarEndDate.SelectedDate;
            selectedValue = this.ddlCouponType.SelectedValue;
            CouponsSearch search = new CouponsSearch {
                CouponName = text,
                minValue = nullable,
                maxValue = nullable2,
                beginDate = selectedDate,
                endDate = nullable4,
                IsCount = true,
                PageIndex = this.pager1.PageIndex,
                PageSize = this.pager1.PageSize,
                SortBy = "CouponId",
                SortOrder = SortAction.Desc,
                Finished = new bool?(bFininshed),
                SearchType = new int?(string.IsNullOrEmpty(selectedValue) ? 0 : int.Parse(selectedValue))
            };
            DbQueryResult couponInfos = CouponHelper.GetCouponInfos(search);
            if (couponInfos != null)
            {
                DataTable data = (DataTable) couponInfos.Data;
                if (data.Rows.Count > 0)
                {
                    data.Columns.Add("useConditon");
                    data.Columns.Add("ReceivNum");
                    data.Columns.Add("expire");
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
                        string str3 = data.Rows[j]["maxReceivNum"].ToString();
                        if (str3 == "0")
                        {
                            data.Rows[j]["ReceivNum"] = "无限制";
                        }
                        else
                        {
                            data.Rows[j]["ReceivNum"] = str3 + "/张每人";
                        }
                        data.Rows[j]["expire"] = DateTime.Parse(data.Rows[j]["EndDate"].ToString()) <= DateTime.Now;
                    }
                }
                this.grdCoupondsList.DataSource = data;
                this.grdCoupondsList.DataBind();
            }
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int i = 0;
            int num2 = 0;
            int num3 = 1;
            DateTime now = DateTime.Now;
            DateTime time2 = DateTime.Now;
            if (!this.bInt(this.txt_id.Text, ref i))
            {
                this.ShowMsg("没有找到这张优惠券的信息！", false);
            }
            if (!this.bInt(this.txt_totalNum.Text, ref num2))
            {
                this.ShowMsg("请输入正确的发放总量！", false);
            }
            if (!this.bInt(this.ddl_maxNum.SelectedValue, ref num3))
            {
                num3 = 1;
            }
            if (!this.calendarStartDate2.SelectedDate.HasValue)
            {
                this.ShowMsg("请输入正确的生效时间！", false);
            }
            if (!this.calendarEndDate2.SelectedDate.HasValue)
            {
                this.ShowMsg("请输入正确的过期时间！", false);
            }
            if (num3 >= num2)
            {
                this.ShowMsg("每人限领数量不能大于或者等于总发放量！", false);
            }
            else
            {
                CouponEdit coupon = new CouponEdit {
                    maxReceivNum = new int?(num3),
                    totalNum = new int?(num2),
                    begin = this.calendarStartDate2.SelectedDate,
                    end = this.calendarEndDate2.SelectedDate
                };
                string msg = "";
                string str2 = CouponHelper.UpdateCoupon(i, coupon, ref msg);
                if (this.txt_used.Text == "true")
                {
                    CouponHelper.setCouponFinished(i, !bFininshed);
                    this.txt_used.Text = "";
                    this.BindData();
                    this.ShowMsg("启用优惠券成功！", true);
                }
                else if (str2 == "1")
                {
                    this.ShowMsgAndReUrl("编辑优惠券成功！", true, this.reUrl);
                }
                else
                {
                    this.ShowMsg(str2, false);
                }
            }
        }

        protected void DelBtn_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int>();
            foreach (GridViewRow row in this.grdCoupondsList.Rows)
            {
                if (row.RowIndex >= 0)
                {
                    CheckBox box = row.Cells[0].FindControl("cbId") as CheckBox;
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
                    int i = 0;
                    if (!this.bInt(num.ToString(), ref i))
                    {
                        this.ShowMsg("选择优惠券出错！", false);
                        return;
                    }
                }
                foreach (int num3 in list)
                {
                    CouponHelper.DeleteCoupon(num3);
                }
                this.ShowMsg("删除优惠券成功！", true);
                this.BindData();
            }
        }

        public string FormatCouponTypes(string types)
        {
            string str = string.Empty;
            try
            {
                foreach (string str2 in types.Split(new char[] { ',' }))
                {
                    str = str + ((CouponType) Globals.ToNum(str2)).ToString() + ",";
                }
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Trim(new char[] { ',' });
                }
            }
            catch
            {
            }
            return str;
        }

        protected string GetCouponType(string types)
        {
            if (string.IsNullOrEmpty(types))
            {
                return "--";
            }
            string[] strArray = types.Split(new char[] { ',' });
            string str = string.Empty;
            foreach (string str2 in strArray)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str = str + string.Format(",{0}", ((CouponType) int.Parse(str2)).ToString());
                }
                else
                {
                    str = str + ((CouponType) int.Parse(str2)).ToString();
                }
            }
            return str;
        }

        protected void grdCoupondsList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox box = e.Row.FindControl("cbId") as CheckBox;
                Button button = e.Row.FindControl("lkDelete") as Button;
                if (Globals.ToNum(DataBinder.Eval(e.Row.DataItem, "ReceiveNum")) > 0)
                {
                    box.Enabled = false;
                    button.Enabled = false;
                }
            }
        }

        private void grdCoupondsList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int couponId = (int) this.grdCoupondsList.DataKeys[e.RowIndex].Value;
            if (!CouponHelper.DeleteCoupon(couponId))
            {
                this.ShowMsg("未知错误", false);
            }
            else
            {
                this.BindData();
                this.ShowMsg("成功删除了选择的优惠券", true);
            }
        }

        private void grdCoupondsList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int couponId = (int) this.grdCoupondsList.DataKeys[e.RowIndex].Value;
            if (CouponHelper.GetCoupon(couponId) != null)
            {
                CouponHelper.setCouponFinished(couponId, !bFininshed);
                this.BindData();
                if (!bFininshed)
                {
                    this.ShowMsg("该优惠券已结束!", true);
                }
                else
                {
                    this.ShowMsg("该优惠券已启用!", true);
                }
            }
            else
            {
                this.ShowMsg("没有找到这张优惠券，该优惠券可能已被删除!", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdCoupondsList.RowDeleting += new GridViewDeleteEventHandler(this.grdCoupondsList_RowDeleting);
            this.grdCoupondsList.RowUpdating += new GridViewUpdateEventHandler(this.grdCoupondsList_RowUpdating);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.isFinished = Globals.RequestQueryStr("bFininshed").ToLower() == "true";
            this.reUrl = base.Request.Url.ToString();
            this.pager1.DefaultPageSize = pagesize;
            this.btnSeach.Click += new EventHandler(this.btnImagetSearch_Click);
            this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
            this.btnDelete.Click += new EventHandler(this.DelBtn_Click);
            if (!base.IsPostBack)
            {
                for (int i = 1; i <= 10; i++)
                {
                    string text = i.ToString() + "张";
                    this.ddl_maxNum.Items.Add(new ListItem(text, i.ToString()));
                }
                string[] allKeys = base.Request.Params.AllKeys;
                if (allKeys.Contains<string>("pagesize") && !this.bInt(base.Request["pagesize"].ToString(), ref pagesize))
                {
                    pagesize = 20;
                }
                this.pager1.DefaultPageSize = pagesize;
                if (allKeys.Contains<string>("pageIndex") && !this.bInt(base.Request["pageIndex"].ToString(), ref pageIndex))
                {
                    pageIndex = 1;
                }
                if (allKeys.Contains<string>("bFininshed") && !this.bBool(base.Request["bFininshed"].ToString(), ref bFininshed))
                {
                    bFininshed = false;
                }
                this.BindCouonType();
                this.BindData();
            }
        }
    }
}

