namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddShareAct : AdminPage
    {
        protected int actId;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected DropDownList cmb_CouponList;
        protected HiddenField hidpic;
        protected HiddenField hidpicdel;
        protected Button saveBtn;
        protected HiddenField shareActId;
        protected HtmlForm thisForm;
        protected TextBox txt_des;
        protected TextBox txt_img;
        protected TextBox txt_MeetValue;
        protected TextBox txt_name;
        protected TextBox txt_Number;
        protected TextBox txt_title;

        protected AddShareAct() : base("m08", "yxp04")
        {
        }

        private DataTable GetCouponList()
        {
            return CouponHelper.GetUnFinishedCoupon(DateTime.Now, (CouponType)3);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.saveBtn.Click += new EventHandler(this.Unnamed_Click);
            if (!base.IsPostBack)
            {
                DataTable couponList = this.GetCouponList();
                if ((couponList != null) && (couponList.Rows.Count > 0))
                {
                    for (int i = 0; i < couponList.Rows.Count; i++)
                    {
                        ListItem item = new ListItem(couponList.Rows[i]["CouponName"].ToString(), couponList.Rows[i]["CouponId"].ToString());
                        this.cmb_CouponList.Items.Add(item);
                    }
                }
                ListItem item2 = new ListItem("请选择", "0");
                this.cmb_CouponList.Items.Insert(0, item2);
                if (base.Request.Params.AllKeys.Contains<string>("id") && base.Request["id"].ToString().bInt(ref this.actId))
                {
                    this.shareActId.Value = this.actId.ToString();
                    ShareActivityInfo act = new ShareActivityInfo();
                    act = ShareActHelper.GetAct(this.actId);
                    this.txt_MeetValue.Text = act.MeetValue.ToString("F2");
                    this.txt_Number.Text = act.CouponNumber.ToString();
                    this.cmb_CouponList.SelectedValue = act.CouponId.ToString();
                    this.calendarStartDate.SelectedDate = new DateTime?(act.BeginDate);
                    this.calendarEndDate.SelectedDate = new DateTime?(act.EndDate);
                    this.txt_name.Text = act.ActivityName;
                    this.txt_img.Text = act.ImgUrl;
                    this.hidpic.Value = act.ImgUrl;
                    this.txt_title.Text = act.ShareTitle;
                    this.txt_des.Text = act.Description;
                }
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.cmb_CouponList.SelectedValue) || (this.cmb_CouponList.SelectedValue == "0"))
            {
                this.ShowMsg("请选择优惠券！", false);
            }
            else
            {
                int couponId = int.Parse(this.cmb_CouponList.SelectedValue);
                DateTime time = this.calendarStartDate.SelectedDate.Value;
                DateTime time2 = this.calendarEndDate.SelectedDate.Value;
                string text = this.txt_MeetValue.Text;
                string val = this.txt_Number.Text;
                decimal i = 0M;
                int num3 = 0;
                if (string.IsNullOrEmpty(this.txt_name.Text.Trim()))
                {
                    this.ShowMsg("活动名称不能为空！", false);
                }
                else if (time2 < time)
                {
                    this.ShowMsg("活动结束时间不能小于开始时间！", false);
                }
                else
                {
                    CouponInfo coupon = CouponHelper.GetCoupon(couponId);
                    if (coupon == null)
                    {
                        this.ShowMsg("优惠券不存在！", false);
                    }
                    else if (time2 > coupon.EndDate)
                    {
                        this.ShowMsg(string.Concat(new object[] { "活动结束时间", time2, "不能大于优惠券的结束时间", coupon.EndDate, "！" }), false);
                    }
                    else if (!text.bDecimal(ref i))
                    {
                        this.ShowMsg("订单满足金额输入错误！", false);
                    }
                    else if (!val.bInt(ref num3))
                    {
                        this.ShowMsg("优惠券张数输入错误！", false);
                    }
                    else if (string.IsNullOrEmpty(this.txt_img.Text))
                    {
                        this.ShowMsg("请上传朋友圈显示图片！", false);
                    }
                    else if (string.IsNullOrEmpty(this.txt_title.Text.Trim()))
                    {
                        this.ShowMsg("朋友圈分享标题不能为空！", false);
                    }
                    else if (string.IsNullOrEmpty(this.txt_des.Text.Trim()))
                    {
                        this.ShowMsg("活动介绍不能为空！", false);
                    }
                    else
                    {
                        ShareActivityInfo act = new ShareActivityInfo();
                        this.actId = int.Parse(this.shareActId.Value);
                        if (this.actId != 0)
                        {
                            act = ShareActHelper.GetAct(this.actId);
                        }
                        act.BeginDate = time;
                        act.EndDate = time2;
                        act.CouponId = couponId;
                        act.CouponNumber = num3;
                        act.CouponName = coupon.CouponName;
                        act.MeetValue = i;
                        act.ActivityName = this.txt_name.Text;
                        act.ImgUrl = this.txt_img.Text;
                        act.ShareTitle = this.txt_title.Text;
                        act.Description = this.txt_des.Text;
                        if (this.actId != 0)
                        {
                            string msg = "";
                            ShareActHelper.Update(act, ref msg);
                            this.ShowMsg("修改成功！", true);
                        }
                        else
                        {
                            string str4 = "";
                            ShareActHelper.Create(act, ref str4);
                            this.ShowMsg("保存成功！", true);
                        }
                        base.Response.Redirect("ShareActList.aspx");
                    }
                }
            }
        }
    }
}

