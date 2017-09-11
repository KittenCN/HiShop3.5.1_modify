namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class PointExchange : AdminPage
    {
        protected bool bFinished;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected int eId;
        protected HiddenField hidpic;
        protected HiddenField hidpicdel;
        protected Button saveBtn;
        protected Script Script4;
        protected Hidistro.UI.Web.Admin.SetMemberRange SetMemberRange;
        protected HtmlForm thisForm;
        protected TextBox txt_img;
        protected TextBox txt_name;

        protected PointExchange() : base("m08", "yxp02")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
            if ((base.Request.Params.AllKeys.Contains<string>("id") && base.Request["id"].ToString().bInt(ref this.eId)) && !this.Page.IsPostBack)
            {
                PointExChangeInfo info = PointExChangeHelper.Get(this.eId);
                this.txt_name.Text = info.Name;
                this.calendarStartDate.SelectedDate = new DateTime?(info.BeginDate);
                this.calendarEndDate.SelectedDate = new DateTime?(info.EndDate);
                this.txt_img.Text = info.ImgUrl;
                this.hidpic.Value = info.ImgUrl;
                HiddenField field = this.SetMemberRange.FindControl("txt_Grades") as HiddenField;
                HiddenField field2 = this.SetMemberRange.FindControl("txt_DefualtGroup") as HiddenField;
                HiddenField field3 = this.SetMemberRange.FindControl("txt_CustomGroup") as HiddenField;
                this.SetMemberRange.Grade = info.MemberGrades;
                this.SetMemberRange.DefualtGroup = info.DefualtGroup;
                this.SetMemberRange.CustomGroup = info.CustomGroup;
                field.Value = info.MemberGrades;
                field2.Value = info.DefualtGroup;
                field3.Value = info.CustomGroup;
                if (info.EndDate < DateTime.Now)
                {
                    this.bFinished = true;
                }
                else
                {
                    this.bFinished = false;
                }
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            HiddenField field = this.SetMemberRange.FindControl("txt_Grades") as HiddenField;
            HiddenField field2 = this.SetMemberRange.FindControl("txt_DefualtGroup") as HiddenField;
            HiddenField field3 = this.SetMemberRange.FindControl("txt_CustomGroup") as HiddenField;
            string str = field.Value;
            string str2 = field2.Value;
            string str3 = field3.Value;
            string text = this.txt_name.Text;
            DateTime? selectedDate = this.calendarStartDate.SelectedDate;
            DateTime? nullable2 = this.calendarEndDate.SelectedDate;
            string str5 = this.txt_img.Text;
            if (string.IsNullOrEmpty(text) || (text.Length > 30))
            {
                this.ShowMsg("请输入活动名称，长度不能超过30个字符！", false);
            }
            else if ((str.Equals("-1") && str2.Equals("-1")) && str3.Equals("-1"))
            {
                this.ShowMsg("请选择会员范围！", false);
            }
            else
            {
                DateTime? nullable3 = nullable2;
                DateTime? nullable4 = selectedDate;
                if ((nullable3.HasValue & nullable4.HasValue) ? (nullable3.GetValueOrDefault() < nullable4.GetValueOrDefault()) : false)
                {
                    this.ShowMsg("结束时间不能早于开始时间！", false);
                }
                else if (!selectedDate.HasValue || !nullable2.HasValue)
                {
                    this.ShowMsg("开始时间或者结束时间不能为空！", false);
                }
                else if (string.IsNullOrEmpty(str5))
                {
                    this.ShowMsg("请上传封面图片！", false);
                }
                else
                {
                    PointExChangeInfo exchange = new PointExChangeInfo();
                    if (this.eId != 0)
                    {
                        exchange = PointExChangeHelper.Get(this.eId);
                    }
                    exchange.BeginDate = selectedDate.Value;
                    exchange.EndDate = nullable2.Value;
                    exchange.Name = text;
                    exchange.MemberGrades = str;
                    exchange.DefualtGroup = str2;
                    exchange.CustomGroup = str3;
                    exchange.ImgUrl = str5;
                    int eId = this.eId;
                    string msg = "";
                    if (this.eId == 0)
                    {
                        exchange.ProductNumber = 0;
                        int num2 = PointExChangeHelper.Create(exchange, ref msg);
                        if (num2 == 0)
                        {
                            this.ShowMsg("保存失败(" + msg + ")", false);
                            return;
                        }
                        eId = num2;
                        this.ShowMsg("保存成功！", true);
                    }
                    else if (PointExChangeHelper.Update(exchange, ref msg))
                    {
                        this.ShowMsg("保存成功！", true);
                    }
                    else
                    {
                        this.ShowMsg("保存失败(" + msg + ")", false);
                        return;
                    }
                    base.Response.Redirect("AddProductToPointExchange.aspx?id=" + eId.ToString());
                }
            }
        }
    }
}

