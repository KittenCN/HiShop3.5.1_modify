namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using Hidistro.UI.Web.hieditor.ueditor.controls;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class DistributorApplySet : AdminPage
    {
        protected bool _CommissionAutoToBalance;
        protected bool _DistributorApplicationCondition;
        protected bool _EnableCommission;
        protected bool _EnableMemberAutoToDistributor;
        protected bool _IsDistributorBuyCanGetCommission;
        protected bool _IsRequestDistributor;
        protected bool _IsShowDistributorSelfStoreName;
        protected Button btnSave;
        protected Button Button1;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected HtmlInputCheckBox cbRechargeMoneyToDistributor;
        protected ucUeditor fckDescription;
        protected HtmlInputCheckBox HasConditions;
        protected HtmlInputCheckBox HasProduct;
        protected HtmlInputHidden hiddProductId;
        public string productHtml;
        protected HtmlInputCheckBox radioDistributorApplicationCondition;
        protected Script Script4;
        protected Hidistro.UI.Common.Controls.Style Style1;
        public string tabnum;
        protected HtmlForm thisForm;
        protected HtmlInputText txtRechargeMoneyToDistributor;
        protected HtmlInputText txtrequestmoney;

        protected DistributorApplySet() : base("m05", "fxp02")
        {
            this.tabnum = "0";
            this.productHtml = "";
            this._IsRequestDistributor = true;
        }

        protected void bindData()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            this.tabnum = base.Request.QueryString["tabnum"];
            if (string.IsNullOrEmpty(this.tabnum))
            {
                this.tabnum = "0";
            }
            this.txtrequestmoney.Value = masterSettings.FinishedOrderMoney.ToString();
            this.fckDescription.Text = masterSettings.DistributorDescription;
            this.radioDistributorApplicationCondition.Checked = masterSettings.DistributorApplicationCondition;
            this._DistributorApplicationCondition = masterSettings.DistributorApplicationCondition;
            this._EnableMemberAutoToDistributor = masterSettings.EnableMemberAutoToDistributor;
            this._IsShowDistributorSelfStoreName = masterSettings.IsShowDistributorSelfStoreName;
            this._IsDistributorBuyCanGetCommission = masterSettings.IsDistributorBuyCanGetCommission;
            this._IsRequestDistributor = true;
            if (!masterSettings.IsRequestDistributor)
            {
                this._IsRequestDistributor = false;
            }
            this._CommissionAutoToBalance = false;
            if (masterSettings.CommissionAutoToBalance)
            {
                this._CommissionAutoToBalance = true;
            }
            this._EnableCommission = false;
            if (masterSettings.EnableCommission)
            {
                this._EnableCommission = true;
            }
            if (masterSettings.RechargeMoneyToDistributor > 0M)
            {
                this.cbRechargeMoneyToDistributor.Checked = true;
                this.txtRechargeMoneyToDistributor.Value = masterSettings.RechargeMoneyToDistributor.ToString("F2");
            }
            if (masterSettings.FinishedOrderMoney > 0)
            {
                this.HasConditions.Checked = true;
            }
            this.HasProduct.Checked = masterSettings.EnableDistributorApplicationCondition;
            string distributorProducts = masterSettings.DistributorProducts;
            if (!string.IsNullOrEmpty(distributorProducts))
            {
                this.hiddProductId.Value = distributorProducts;
            }
            if (!string.IsNullOrEmpty(masterSettings.DistributorProductsDate) && masterSettings.DistributorProductsDate.Contains("|"))
            {
                if (!string.IsNullOrEmpty(masterSettings.DistributorProductsDate.Split(new char[] { '|' })[0]))
                {
                    this.calendarStartDate.SelectedDate = new DateTime?(Convert.ToDateTime(masterSettings.DistributorProductsDate.Split(new char[] { '|' })[0]));
                }
                if (!string.IsNullOrEmpty(masterSettings.DistributorProductsDate.Split(new char[] { '|' })[1]))
                {
                    this.calendarEndDate.SelectedDate = new DateTime?(Convert.ToDateTime(masterSettings.DistributorProductsDate.Split(new char[] { '|' })[1]));
                }
            }
            switch (Globals.RequestFormStr("type"))
            {
                case "EnableCommission":
                    try
                    {
                        base.Response.ContentType = "text/plain";
                        bool flag = bool.Parse(Globals.RequestFormStr("enable"));
                        masterSettings.EnableCommission = flag;
                        SettingsManager.Save(masterSettings);
                        base.Response.Write("保存成功");
                    }
                    catch (Exception exception)
                    {
                        base.Response.Write("保存失败！（" + exception.ToString() + ")");
                    }
                    base.Response.End();
                    return;

                case "CommissionAutoToBalance":
                    try
                    {
                        base.Response.ContentType = "text/plain";
                        bool flag2 = bool.Parse(Globals.RequestFormStr("enable"));
                        masterSettings.CommissionAutoToBalance = flag2;
                        SettingsManager.Save(masterSettings);
                        base.Response.Write("保存成功");
                    }
                    catch (Exception exception2)
                    {
                        base.Response.Write("保存失败！（" + exception2.ToString() + ")");
                    }
                    base.Response.End();
                    return;

                case "IsRequestDistributor":
                    try
                    {
                        base.Response.ContentType = "text/plain";
                        bool flag3 = bool.Parse(Globals.RequestFormStr("enable"));
                        masterSettings.IsRequestDistributor = flag3;
                        SettingsManager.Save(masterSettings);
                        base.Response.Write("保存成功");
                    }
                    catch (Exception exception3)
                    {
                        base.Response.Write("保存失败！（" + exception3.ToString() + ")");
                    }
                    base.Response.End();
                    return;

                case "IsShowDistributorSelfStoreName":
                    try
                    {
                        base.Response.ContentType = "text/plain";
                        bool flag4 = bool.Parse(Globals.RequestFormStr("enable"));
                        masterSettings.IsShowDistributorSelfStoreName = flag4;
                        SettingsManager.Save(masterSettings);
                        MemberHelper.UpdateSetCardCreatTime();
                        base.Response.Write("保存成功");
                    }
                    catch (Exception exception4)
                    {
                        base.Response.Write("保存失败！（" + exception4.ToString() + ")");
                    }
                    base.Response.End();
                    return;

                case "IsDistributorBuyCanGetCommission":
                    try
                    {
                        base.Response.ContentType = "text/plain";
                        bool flag5 = bool.Parse(Globals.RequestFormStr("enable"));
                        masterSettings.IsDistributorBuyCanGetCommission = flag5;
                        SettingsManager.Save(masterSettings);
                        base.Response.Write("保存成功");
                    }
                    catch (Exception exception5)
                    {
                        base.Response.Write("保存失败！（" + exception5.ToString() + ")");
                    }
                    base.Response.End();
                    return;

                case "DistributorApplicationCondition":
                    try
                    {
                        base.Response.ContentType = "text/plain";
                        bool flag6 = bool.Parse(Globals.RequestFormStr("enable"));
                        masterSettings.DistributorApplicationCondition = flag6;
                        SettingsManager.Save(masterSettings);
                        base.Response.Write("保存成功");
                    }
                    catch (Exception exception6)
                    {
                        base.Response.Write("保存失败！（" + exception6.ToString() + ")");
                    }
                    base.Response.End();
                    return;

                case "EnableMemberAutoToDistributor":
                    try
                    {
                        base.Response.ContentType = "text/plain";
                        bool flag7 = bool.Parse(Globals.RequestFormStr("enable"));
                        masterSettings.EnableMemberAutoToDistributor = flag7;
                        SettingsManager.Save(masterSettings);
                        base.Response.Write("保存成功");
                    }
                    catch (Exception exception7)
                    {
                        base.Response.Write("保存失败！（" + exception7.ToString() + ")");
                    }
                    base.Response.End();
                    return;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.tabnum = "0";
            int result = 0;
            this._DistributorApplicationCondition = true;
            if ((this.radioDistributorApplicationCondition.Checked && !this.HasProduct.Checked) && (!this.HasConditions.Checked && !this.cbRechargeMoneyToDistributor.Checked))
            {
                this.ShowMsg("请选择分销商申请条件", false);
            }
            else if (this.HasConditions.Checked && (!int.TryParse(this.txtrequestmoney.Value.Trim(), out result) || (result < 1)))
            {
                this.ShowMsg("累计消费金额必须为大于0的整数金额", false);
            }
            else
            {
                decimal num2 = 0M;
                if (this.cbRechargeMoneyToDistributor.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtRechargeMoneyToDistributor.Value))
                    {
                        this.ShowMsg("请填写账户单次充值金额", false);
                        return;
                    }
                    decimal num3 = 0M;
                    decimal.TryParse(this.txtRechargeMoneyToDistributor.Value, out num3);
                    if (num3 <= 0M)
                    {
                        this.ShowMsg("账户单次充值金额必须大于0", false);
                        return;
                    }
                    num2 = num3;
                }
                if (this.HasProduct.Checked && string.IsNullOrEmpty(this.hiddProductId.Value))
                {
                    this.ShowMsg("请选择指定商品", false);
                }
                else
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    masterSettings.DistributorApplicationCondition = this.radioDistributorApplicationCondition.Checked;
                    masterSettings.RechargeMoneyToDistributor = num2;
                    masterSettings.FinishedOrderMoney = result;
                    masterSettings.EnableDistributorApplicationCondition = this.HasProduct.Checked;
                    if (this.HasProduct.Checked)
                    {
                        masterSettings.DistributorProducts = this.hiddProductId.Value;
                    }
                    else
                    {
                        masterSettings.DistributorProducts = "";
                    }
                    string str = "";
                    if (this.calendarStartDate.SelectedDate.HasValue)
                    {
                        str = this.calendarStartDate.SelectedDate.Value.ToString();
                    }
                    str = str + "|";
                    if (this.calendarEndDate.SelectedDate.HasValue)
                    {
                        str = str + this.calendarEndDate.SelectedDate.Value.ToString();
                    }
                    masterSettings.DistributorProductsDate = str;
                    SettingsManager.Save(masterSettings);
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Admin-Product"];
                    if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
                    {
                        cookie.Value = null;
                        cookie.Expires = DateTime.Now.AddYears(-1);
                        HttpContext.Current.Response.Cookies.Set(cookie);
                    }
                    this.bindData();
                    this.ShowMsgAndReUrl("修改成功", true, "DistributorApplySet.aspx");
                }
            }
        }

        protected void btnSave_Description(object sender, EventArgs e)
        {
            this.tabnum = "1";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.DistributorDescription = this.fckDescription.Text.Trim();
            SettingsManager.Save(masterSettings);
            this.ShowMsg("分销说明修改成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.bindData();
            }
        }
    }
}

