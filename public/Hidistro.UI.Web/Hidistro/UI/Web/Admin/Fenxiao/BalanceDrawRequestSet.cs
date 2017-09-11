namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class BalanceDrawRequestSet : AdminPage
    {
        protected HtmlInputCheckBox alipayCheck;
        protected HtmlGenericControl alipaypanel;
        protected Button btnSave;
        protected RadioButtonList CheckRealName;
        protected CheckBoxList DrawPayType;
        protected Script Script4;
        protected Hidistro.UI.Common.Controls.Style Style1;
        protected TextBox txtApplySet;
        protected HtmlGenericControl weipaypanel;
        protected HtmlInputCheckBox weixinPayCheck;

        protected BalanceDrawRequestSet() : base("m09", "szp16")
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string str = "";
            foreach (ListItem item in this.DrawPayType.Items)
            {
                if (item.Selected)
                {
                    str = str + "|" + item.Value;
                    if (item.Value == "0")
                    {
                        this.weipaypanel.Style["display"] = "";
                    }
                    else if (item.Value == "1")
                    {
                        this.alipaypanel.Style["display"] = "";
                    }
                }
            }
            if (str == "")
            {
                this.ShowMsg("至少选择一种提现支付方式额!", false);
            }
            else if (decimal.Parse(this.txtApplySet.Text.Trim()) < 0.01M)
            {
                this.ShowMsg("请填写适当的最低提现金额,不能小于数值0.01！!", false);
            }
            else
            {
                str = str.Remove(0, 1);
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                masterSettings.DrawPayType = str;
                masterSettings.MentionNowMoney = this.txtApplySet.Text.Trim();
                masterSettings.BatchWeixinPay = this.weixinPayCheck.Checked;
                masterSettings.BatchAliPay = this.alipayCheck.Checked;
                if (!this.DrawPayType.Items[0].Selected)
                {
                    masterSettings.BatchAliPay = false;
                }
                if (!this.DrawPayType.Items[1].Selected)
                {
                    masterSettings.BatchWeixinPay = false;
                }
                masterSettings.BatchWeixinPayCheckRealName = int.Parse(this.CheckRealName.SelectedValue);
                SettingsManager.Save(masterSettings);
                this.ShowMsg("修改成功", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.alipaypanel.Style.Add("display", "none");
                this.weipaypanel.Style.Add("display", "none");
                this.txtApplySet.Text = masterSettings.MentionNowMoney;
                string drawPayType = masterSettings.DrawPayType;
                if (drawPayType != "")
                {
                    foreach (string str2 in drawPayType.Split(new char[] { '|' }))
                    {
                        if (str2 != "")
                        {
                            foreach (ListItem item in this.DrawPayType.Items)
                            {
                                if (str2 == item.Value)
                                {
                                    item.Selected = true;
                                }
                            }
                            if (str2 == "0")
                            {
                                this.weipaypanel.Style["display"] = "";
                            }
                            else if (str2 == "1")
                            {
                                this.alipaypanel.Style["display"] = "";
                            }
                        }
                    }
                }
                this.CheckRealName.SelectedValue = masterSettings.BatchWeixinPayCheckRealName.ToString();
                if (masterSettings.BatchWeixinPay)
                {
                    this.weixinPayCheck.Checked = true;
                }
                if (masterSettings.BatchAliPay)
                {
                    this.alipayCheck.Checked = true;
                }
            }
        }
    }
}

