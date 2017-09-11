namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class PointToCash : AdminPage
    {
        protected Button btnQuery;
        protected bool enable;
        protected Repeater grdProducts;
        protected PageSize hrefPageSize;
        protected Button saveBtn;
        protected HtmlForm thisForm;
        protected TextBox txt_MaxAmount;
        protected TextBox txt_name;
        protected TextBox txt_Rate;

        public PointToCash() : base("m08", "yxp03")
        {
        }

        private bool bDecimal(string val, ref decimal i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return decimal.TryParse(val, out i);
        }

        private void BindData()
        {
        }

        private bool bInt(string val, ref int i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            if (val.Contains(".") || val.Contains("-"))
            {
                return false;
            }
            return int.TryParse(val, out i);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            this.enable = masterSettings.PonitToCash_Enable;
            this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
            if (!base.IsPostBack)
            {
                this.txt_Rate.Text = masterSettings.PointToCashRate.ToString();
                this.txt_MaxAmount.Text = masterSettings.PonitToCash_MaxAmount.ToString("F2");
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            string text = this.txt_Rate.Text;
            string val = this.txt_MaxAmount.Text;
            int i = 0;
            decimal num2 = 0M;
            if (!this.bInt(text, ref i))
            {
                this.ShowMsg("请输入正确的抵现比例！", false);
            }
            else if (!this.bDecimal(val, ref num2))
            {
                this.ShowMsg("请输入正确的单次最高抵现金额！", false);
            }
            else
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                masterSettings.PointToCashRate = i;
                masterSettings.PonitToCash_MaxAmount = num2;
                SettingsManager.Save(masterSettings);
                this.enable = masterSettings.PonitToCash_Enable;
                this.ShowMsg("保存成功！", true);
            }
        }
    }
}

