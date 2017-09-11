namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditLimitedTimeDiscount : AdminPage
    {
        protected Button btnSave;
        protected Button btnSaveAndNext;
        protected ucDateTimePicker dateBeginTime;
        protected ucDateTimePicker dateEndTime;
        protected TextBox discount;
        public int id;
        protected CheckBox IsCommision;
        protected SetMemberRange memberRange;
        protected HtmlForm thisForm;
        protected TextBox txtActivityName;
        protected TextBox txtDescription;
        protected TextBox txtLimitNumber;

        protected EditLimitedTimeDiscount() : base("m08", "yxp24")
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string text = this.txtActivityName.Text;
            DateTime? textToDate = this.dateBeginTime.TextToDate;
            DateTime? nullable2 = this.dateEndTime.TextToDate;
            string str2 = this.txtDescription.Text;
            int result = 0;
            bool flag = this.IsCommision.Checked;
            string str3 = this.discount.Text;
            HiddenField field = this.memberRange.FindControl("txt_Grades") as HiddenField;
            HiddenField field2 = this.memberRange.FindControl("txt_DefualtGroup") as HiddenField;
            HiddenField field3 = this.memberRange.FindControl("txt_CustomGroup") as HiddenField;
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("活动名称不能为空！", false);
            }
            else if (!textToDate.HasValue || !nullable2.HasValue)
            {
                this.ShowMsg("开始时间和结束时间都不能为空！", false);
            }
            else if (textToDate.Value >= nullable2.Value)
            {
                this.ShowMsg("开始时间不能大于或等于结束时间！", false);
            }
            else if (!int.TryParse(this.txtLimitNumber.Text, out result))
            {
                this.ShowMsg("每人限购格式不对！", false);
            }
            else if (((field.Value == "-1") && (field2.Value == "-1")) && (field3.Value == "-1"))
            {
                this.ShowMsg("请选择适用会员！", false);
            }
            else if (flag && string.IsNullOrEmpty(str3))
            {
                this.ShowMsg("请填写佣金折扣值！", false);
            }
            else if (flag && (str3 == "0"))
            {
                this.ShowMsg("佣金折扣值必须大于0！", false);
            }
            else
            {
                LimitedTimeDiscountInfo info = new LimitedTimeDiscountInfo {
                    ActivityName = text,
                    BeginTime = textToDate.Value,
                    EndTime = nullable2.Value,
                    Description = str2,
                    LimitNumber = result,
                    ApplyMembers = field.Value,
                    DefualtGroup = field2.Value,
                    CustomGroup = field3.Value,
                    CreateTime = DateTime.Now
                };
                info.Status = 1.ToString();
                info.IsCommission = flag;
                info.CommissionDiscount = flag ? ((int) (float.Parse(str3) * 10f)) : 0;
                int num2 = Globals.RequestQueryNum("id");
                if (num2 > 0)
                {
                    info.LimitedTimeDiscountId = num2;
                    LimitedTimeDiscountHelper.UpdateLimitedTimeDiscount(info);
                }
                this.ShowMsgAndReUrl("保存成功！", true, "EditLimitedTimeDiscount.aspx?id=" + Globals.RequestQueryNum("id"));
            }
        }

        protected void btnSaveAndNext_Click(object sender, EventArgs e)
        {
            string text = this.txtActivityName.Text;
            DateTime? textToDate = this.dateBeginTime.TextToDate;
            DateTime? nullable2 = this.dateEndTime.TextToDate;
            string str2 = this.txtDescription.Text;
            int result = 0;
            bool flag = this.IsCommision.Checked;
            string str3 = this.discount.Text;
            HiddenField field = this.memberRange.FindControl("txt_Grades") as HiddenField;
            HiddenField field2 = this.memberRange.FindControl("txt_DefualtGroup") as HiddenField;
            HiddenField field3 = this.memberRange.FindControl("txt_CustomGroup") as HiddenField;
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("活动名称不能为空！", false);
            }
            else if (!textToDate.HasValue || !nullable2.HasValue)
            {
                this.ShowMsg("开始时间和结束时间都不能为空！", false);
            }
            else if (textToDate.Value >= nullable2.Value)
            {
                this.ShowMsg("开始时间不能大于或等于结束时间！", false);
            }
            else if (!int.TryParse(this.txtLimitNumber.Text, out result))
            {
                this.ShowMsg("每人限购格式不对！", false);
            }
            else if (((field.Value == "-1") && (field2.Value == "-1")) && (field3.Value == "-1"))
            {
                this.ShowMsg("请选择适用会员！", false);
            }
            else if (flag && string.IsNullOrEmpty(str3))
            {
                this.ShowMsg("请填写商品佣金折数！", false);
            }
            else if (flag && (str3 == "0"))
            {
                this.ShowMsg("佣金折扣值必须大于0！", false);
            }
            else
            {
                LimitedTimeDiscountInfo info = new LimitedTimeDiscountInfo {
                    ActivityName = text,
                    BeginTime = textToDate.Value,
                    EndTime = nullable2.Value,
                    Description = str2,
                    LimitNumber = result,
                    ApplyMembers = field.Value,
                    DefualtGroup = field2.Value,
                    CustomGroup = field3.Value,
                    CreateTime = DateTime.Now
                };
                info.Status = 1.ToString();
                info.IsCommission = flag;
                info.CommissionDiscount = flag ? ((int) (float.Parse(str3) * 10f)) : 0;
                int num2 = LimitedTimeDiscountHelper.AddLimitedTimeDiscount(info);
                if (num2 > 0)
                {
                    this.ShowMsgAndReUrl("添加成功", true, "LimitedTimeDiscountAddProduct.aspx?id=" + num2);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.id = Globals.RequestQueryNum("id");
                if (this.id > 0)
                {
                    LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(this.id);
                    if (discountInfo != null)
                    {
                        this.IsCommision.Checked = discountInfo.IsCommission;
                        this.discount.Text = (((double) discountInfo.CommissionDiscount) / 10.0).ToString();
                        this.txtActivityName.Text = discountInfo.ActivityName;
                        this.dateBeginTime.Text = discountInfo.BeginTime.ToString();
                        this.dateEndTime.Text = discountInfo.EndTime.ToString();
                        this.txtDescription.Text = discountInfo.Description;
                        this.txtLimitNumber.Text = discountInfo.LimitNumber.ToString();
                        HiddenField field = this.memberRange.FindControl("txt_Grades") as HiddenField;
                        HiddenField field2 = this.memberRange.FindControl("txt_DefualtGroup") as HiddenField;
                        HiddenField field3 = this.memberRange.FindControl("txt_CustomGroup") as HiddenField;
                        this.memberRange.Grade = discountInfo.ApplyMembers;
                        this.memberRange.CustomGroup = discountInfo.CustomGroup;
                        this.memberRange.DefualtGroup = discountInfo.DefualtGroup;
                        field.Value = discountInfo.ApplyMembers;
                        field2.Value = discountInfo.DefualtGroup;
                        field3.Value = discountInfo.CustomGroup;
                    }
                }
            }
        }
    }
}

