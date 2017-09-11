namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class ExpressRadioButtonList : RadioButtonList
    {
        public override void DataBind()
        {
            IList<string> expressCompanies = this.ExpressCompanies;
            if ((expressCompanies == null) || (expressCompanies.Count == 0))
            {
                expressCompanies = ExpressHelper.GetAllExpressName();
            }
            base.Items.Clear();
            foreach (string str in expressCompanies)
            {
                ListItem item = new ListItem(str, str);
                if (string.Compare(item.Value, this.Name, false) == 0)
                {
                    item.Selected = true;
                }
                base.Items.Add(item);
            }
        }

        public IList<string> ExpressCompanies { get; set; }

        public string Name { get; set; }
    }
}

