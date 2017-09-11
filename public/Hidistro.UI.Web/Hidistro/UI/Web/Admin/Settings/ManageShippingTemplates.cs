namespace Hidistro.UI.Web.Admin.Settings
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Settings;
    using Hidistro.Entities;
    using Hidistro.Entities.Settings;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ManageShippingTemplates : AdminPage
    {
        private Dictionary<int, string> AllRegionIds;
        protected Repeater ListTemplates;
        protected Button lkbDeleteCheck;
        protected Pager pager1;
        protected HtmlGenericControl TablefFooter;
        protected HtmlForm thisForm;

        protected ManageShippingTemplates() : base("m09", "szp06")
        {
        }

        public void BindTemplates()
        {
            if (this.AllRegionIds == null)
            {
                this.AllRegionIds = RegionHelper.GetAllCitys();
            }
            IList<FreightTemplate> freightTemplates = SettingsHelper.GetFreightTemplates();
            this.ListTemplates.DataSource = freightTemplates;
            this.ListTemplates.DataBind();
            if (freightTemplates.Count < 1)
            {
                this.TablefFooter.Visible = false;
            }
        }

        private void DeleteCheck()
        {
            string templateIds = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                templateIds = base.Request["CheckBoxGroup"];
            }
            if (templateIds.Length <= 0)
            {
                this.ShowMsg("请先选择要删除的运费模板", false);
            }
            else
            {
                int num = SettingsHelper.DeleteShippingTemplates(templateIds);
                this.ShowMsg(string.Format("成功删除了{0}条模板关联数据", num), true);
                this.BindTemplates();
            }
        }

        protected void DeleteShiper(object sender, EventArgs e)
        {
            int result = 0;
            if (int.TryParse(((Button) sender).CommandArgument, out result))
            {
                string shippingTemplateLinkProduct = SettingsHelper.GetShippingTemplateLinkProduct(new int[] { result });
                if (shippingTemplateLinkProduct.StartsWith(result.ToString() + "|"))
                {
                    this.ShowMsg("您选择的模板已有商品使用，不能删除！", false);
                }
                else if (SettingsHelper.DeleteShippingTemplate(result))
                {
                    this.BindTemplates();
                    this.ShowMsg("已经成功删除选择的模板信息", true);
                }
                else
                {
                    this.ShowMsg("非正常删除！", true);
                }
            }
            else
            {
                this.ShowMsg("非正常删除！", true);
            }
        }

        private string getRegionNameById(int RegionId)
        {
            string str = "未知";
            if (this.AllRegionIds.ContainsKey(RegionId))
            {
                str = this.AllRegionIds[RegionId];
            }
            return str;
        }

        public string getRegionNamesByIds(string RegionIds)
        {
            string str = "";
            if (!string.IsNullOrEmpty(RegionIds))
            {
                string[] strArray = RegionIds.Split(new char[] { ',' });
                str = "";
                foreach (string str2 in strArray)
                {
                    int result = 0;
                    if (int.TryParse(str2.Trim(), out result) && (result > 0))
                    {
                        str = str + this.getRegionNameById(result) + "，";
                    }
                }
            }
            return str;
        }

        private void lkbDeleteCheck_Click(object sender, EventArgs e)
        {
            this.DeleteCheck();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.lkbDeleteCheck.Click += new EventHandler(this.lkbDeleteCheck_Click);
            if (!this.Page.IsPostBack)
            {
                this.TablefFooter.Visible = true;
                this.BindTemplates();
            }
        }

        protected void rptypelist_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater repeater = e.Item.FindControl("ListShipper") as Repeater;
                FreightTemplate dataItem = (FreightTemplate) e.Item.DataItem;
                IList<SpecifyRegionGroup> specifyRegionGroups = SettingsHelper.GetSpecifyRegionGroups(Convert.ToInt32(dataItem.TemplateId));
                if (specifyRegionGroups.Count < 1)
                {
                    SpecifyRegionGroup item = new SpecifyRegionGroup {
                        RegionIds = "",
                        ModeId = 1,
                        FristNumber = 1M,
                        FristPrice = 0M,
                        AddNumber = 1M,
                        AddPrice = 0M,
                        IsDefault = true
                    };
                    specifyRegionGroups.Add(item);
                }
                repeater.DataSource = specifyRegionGroups;
                repeater.DataBind();
            }
        }
    }
}

