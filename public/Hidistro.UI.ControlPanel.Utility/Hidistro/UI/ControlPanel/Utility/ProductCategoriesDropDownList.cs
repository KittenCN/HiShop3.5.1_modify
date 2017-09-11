namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class ProductCategoriesDropDownList : DropDownList
    {
        private bool isTopCategory;
        private bool m_AutoDataBind;
        private string m_NullToDisplay = "";
        private string strDepth = "　";

        public override void DataBind()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            if (this.IsTopCategory)
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

        protected override void OnLoad(EventArgs e)
        {
            if (this.AutoDataBind && !this.Page.IsPostBack)
            {
                this.DataBind();
            }
        }

        public bool AutoDataBind
        {
            get
            {
                return this.m_AutoDataBind;
            }
            set
            {
                this.m_AutoDataBind = value;
            }
        }

        public bool IsTopCategory
        {
            get
            {
                return this.isTopCategory;
            }
            set
            {
                this.isTopCategory = value;
            }
        }

        public bool IsUnclassified { get; set; }

        public string NullToDisplay
        {
            get
            {
                return this.m_NullToDisplay;
            }
            set
            {
                this.m_NullToDisplay = value;
            }
        }

        public int? SelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(base.SelectedValue))
                {
                    return new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    base.SelectedIndex = -1;
                }
            }
        }
    }
}

