namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class NewCoupon : AdminPage
    {
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected DropDownList ddl_maxNum;
        protected HtmlInputRadioButton rdt_1;
        protected HtmlInputRadioButton rdt_2;
        protected Hidistro.UI.Web.Admin.SetMemberRange SetMemberRange;
        protected HtmlForm thisForm;
        protected TextBox txt_conditonVal;
        protected TextBox txt_name;
        protected TextBox txt_totalNum;
        protected TextBox txt_Value;

        protected NewCoupon() : base("m08", "yxp01")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                for (int i = 1; i <= 10; i++)
                {
                    string text = i.ToString() + "张";
                    this.ddl_maxNum.Items.Add(new ListItem(text, i.ToString()));
                }
            }
        }
    }
}

