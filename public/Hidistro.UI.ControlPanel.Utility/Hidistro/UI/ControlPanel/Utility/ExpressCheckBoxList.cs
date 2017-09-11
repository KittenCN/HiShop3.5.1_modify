namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Entities.Sales;
    using Hidistro.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class ExpressCheckBoxList : CheckBoxList
    {
        private IList<string> expressCompany;

        public void BindExpressCheckBoxList()
        {
            base.Items.Clear();
            foreach (ExpressCompanyInfo info in ExpressHelper.GetAllExpress())
            {
                ListItem item = new ListItem(info.Name, info.Name);
                if (this.ExpressCompany != null)
                {
                    foreach (string str in this.ExpressCompany)
                    {
                        if (string.Compare(item.Value, str, false) == 0)
                        {
                            item.Selected = true;
                        }
                    }
                }
                base.Items.Add(item);
            }
        }

        public override void DataBind()
        {
            this.BindExpressCheckBoxList();
            base.DataBind();
        }

        public IList<string> ExpressCompany
        {
            get
            {
                if (this.expressCompany == null)
                {
                    return new List<string>();
                }
                return this.expressCompany;
            }
            set
            {
                this.expressCompany = value;
            }
        }
    }
}

