namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Entities.Commodities;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public sealed class AttributeUseageModeRadioButtonList : RadioButtonList
    {
        public AttributeUseageModeRadioButtonList()
        {
            this.Items.Clear();
            int num = 1;
            this.Items.Add(new ListItem("供客户查看", num.ToString(CultureInfo.InvariantCulture)));
            int num2 = 2;
            this.Items.Add(new ListItem("客户可选规格", num2.ToString(CultureInfo.InvariantCulture)));
            this.Items[0].Selected = true;
        }

        public AttributeUseageMode SelectedValue
        {
            get
            {
                return (AttributeUseageMode) int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
            }
            set
            {
                base.SelectedValue = ((int) value).ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}

