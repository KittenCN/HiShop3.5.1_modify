namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class ProductCategoriesDropDownListNew : ProductCategoriesDropDownList
    {
        private string strDepth = "　";

        public override void DataBind()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem(base.NullToDisplay, string.Empty));
            if (base.IsTopCategory)
            {
                foreach (CategoryInfo info in CatalogHelper.GetMainCategories())
                {
                    this.Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.CategoryId.ToString()));
                }
            }
            else
            {
                IList<CategoryInfo> sequenceCategories = CatalogHelper.GetSequenceCategories();
                for (int i = 0; i < sequenceCategories.Count; i++)
                {
                    this.Items.Add(new ListItem(this.FormatDepth(sequenceCategories[i].Depth, Globals.HtmlDecode(sequenceCategories[i].Name)), sequenceCategories[i].CategoryId.ToString(CultureInfo.InvariantCulture)));
                }
            }
        }

        private string FormatDepth(int depth, string categoryName)
        {
            for (int i = 1; i < depth; i++)
            {
                categoryName = this.strDepth + categoryName;
            }
            return categoryName;
        }
    }
}

