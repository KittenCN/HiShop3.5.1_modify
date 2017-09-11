namespace Hidistro.UI.Web.Admin.promotion
{
    using Entities.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SetRegisterSendCoupon : AdminPage
    {
        protected Button btnSave;
        protected DropDownList ddlCouponList;
        protected HtmlForm thisForm;
        protected ucDateTimePicker ucDateBegin;
        protected ucDateTimePicker ucDateEnd;

        protected SetRegisterSendCoupon() : base("m08", "yxp17")
        {
        }

        private void BindDate()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            this.IsEnble = masterSettings.IsRegisterSendCoupon;
            try
            {
                this.ddlCouponList.SelectedValue = masterSettings.RegisterSendCouponId.ToString();
            }
            catch (Exception)
            {
            }
            this.ucDateBegin.Text = masterSettings.RegisterSendCouponBeginTime.HasValue ? masterSettings.RegisterSendCouponBeginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            this.ucDateEnd.Text = masterSettings.RegisterSendCouponEndTime.HasValue ? masterSettings.RegisterSendCouponEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
        }

        private void BindDdlCouponList()
        {
            DataTable unFinishedCoupon = CouponHelper.GetUnFinishedCoupon(DateTime.Now, (CouponType)3);
            if (unFinishedCoupon != null)
            {
                foreach (DataRow row in unFinishedCoupon.Rows)
                {
                    ListItem item = new ListItem {
                        Text = row["CouponName"].ToString(),
                        Value = row["CouponId"].ToString()
                    };
                    this.ddlCouponList.Items.Add(item);
                }
            }
        }

        protected void BtnSave(object sender, EventArgs e)
        {
            this.IsEnble = bool.Parse(base.Request["txtIsEnble"]);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.IsRegisterSendCoupon = this.IsEnble;
            masterSettings.RegisterSendCouponId = int.Parse(this.ddlCouponList.SelectedValue);
            masterSettings.RegisterSendCouponBeginTime = this.ucDateBegin.SelectedDate;
            masterSettings.RegisterSendCouponEndTime = this.ucDateEnd.SelectedDate;
            try
            {
                SettingsManager.Save(masterSettings);
                this.ShowMsg("保存成功！", true);
            }
            catch (Exception)
            {
                this.ShowMsg("保存失败！", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.BtnSave);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindDdlCouponList();
                this.BindDate();
            }
        }

        protected bool IsEnble { get; private set; }
    }
}

