namespace Hidistro.UI.Web.Admin.CashBack
{
    using Hidistro.ControlPanel.CashBack;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Entities.CashBack;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddCashBack : AdminPage
    {
        protected Button btnAddCashBack;
        private int cashBackId;
        protected HtmlInputCheckBox cbIsDefault;
        protected CashBackTypesDropDownList dropCashBackTypes;
        protected HiddenField hidAction;
        protected string opt;
        protected Script Script4;
        protected HtmlForm thisForm;
        protected TextBox txtPercentage;
        protected TextBox txtRechargeAmount;
        protected HiddenField txtUserId;
        protected TextBox txtUserName;

        public AddCashBack() : base("m99", "fxp02")
        {
            this.opt = "添加";
        }

        protected void btnAddCashBack_Click(object sender, EventArgs e)
        {
            string str = this.hidAction.Value;
            if (str != null)
            {
                if (!(str == "ADD"))
                {
                    if (!(str == "EDIT"))
                    {
                        return;
                    }
                }
                else
                {
                    CashBackInfo info = new CashBackInfo {
                        UserId = int.Parse(this.txtUserId.Value),
                        RechargeAmount = decimal.Parse(this.txtRechargeAmount.Text),
                        CashBackAmount = 0M,
                        Percentage = decimal.Parse(this.txtPercentage.Text),
                        IsValid = this.cbIsDefault.Checked,
                        IsFinished = false,
                        FinishedDate = null,
                        CreateDate = DateTime.Now,
                        CashBackType =(CashBackTypes)this.dropCashBackTypes.SelectedValue.Value
                    };
                    if (CashBackHelper.AddCashBack(info))
                    {
                        this.ShowMsg("增加充值返现成功", true);
                        return;
                    }
                    this.ShowMsg("增加充值返现失败", false);
                    return;
                }
                CashBackInfo cashBackInfo = CashBackHelper.GetCashBackInfo(this.cashBackId);
                cashBackInfo.UserId = int.Parse(this.txtUserId.Value);
                cashBackInfo.RechargeAmount = decimal.Parse(this.txtRechargeAmount.Text);
                cashBackInfo.Percentage = decimal.Parse(this.txtPercentage.Text);
                cashBackInfo.IsValid = this.cbIsDefault.Checked;
                if (CashBackHelper.UpdateCashBack(cashBackInfo, null))
                {
                    this.ShowMsg("修改充值返现成功", true);
                }
                else
                {
                    this.ShowMsg("修改充值返现失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.dropCashBackTypes.DataBind();
            }
            string s = base.Request.QueryString["CashBackId"];
            if (int.TryParse(s, out this.cashBackId))
            {
                this.opt = "修改";
                if (!this.Page.IsPostBack)
                {
                    CashBackInfo cashBackInfo = CashBackHelper.GetCashBackInfo(this.cashBackId);
                    if (cashBackInfo != null)
                    {
                        this.hidAction.Value = "EDIT";
                        this.txtUserName.Text = MemberHelper.GetMember(cashBackInfo.UserId).UserName;
                        this.txtUserId.Value = cashBackInfo.UserId.ToString();
                        this.txtRechargeAmount.Text = cashBackInfo.RechargeAmount.ToString("f2");
                        this.txtPercentage.Text = cashBackInfo.Percentage.ToString("f2");
                        this.cbIsDefault.Checked = cashBackInfo.IsValid;
                        this.dropCashBackTypes.SelectedValue = new int?((int) cashBackInfo.CashBackType);
                        this.dropCashBackTypes.Enabled = false;
                        this.txtPercentage.Text = cashBackInfo.Percentage.ToString("F2");
                    }
                }
            }
        }
    }
}

