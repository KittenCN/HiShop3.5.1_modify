namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI.WebControls;

    public class PaymentDropDownList : DropDownList
    {
        private bool allowNull = true;

        public override void DataBind()
        {
            base.Items.Clear();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(string.Empty, string.Empty));
            }
            if (SettingsManager.GetMasterSettings(false).EnableAlipayRequest)
            {
                foreach (PaymentModeInfo info in SalesHelper.GetPaymentModes())
                {
                    base.Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.ModeId.ToString()));
                }
            }
            if (SettingsManager.GetMasterSettings(false).EnableWeiXinRequest)
            {
                base.Items.Add(new ListItem("微信支付", "88"));
            }
            if (SettingsManager.GetMasterSettings(false).EnableOffLineRequest)
            {
                base.Items.Add(new ListItem("线下付款", "99"));
            }
            if (SettingsManager.GetMasterSettings(false).EnablePodRequest)
            {
                base.Items.Add(new ListItem("货到付款", "-1"));
            }
        }

        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }
            set
            {
                this.allowNull = value;
            }
        }

        public int? SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return new int?(int.Parse(base.SelectedValue));
            }
            set
            {
                if (!value.HasValue)
                {
                    base.SelectedValue = string.Empty;
                }
                else
                {
                    base.SelectedValue = value.ToString();
                }
            }
        }
    }
}

