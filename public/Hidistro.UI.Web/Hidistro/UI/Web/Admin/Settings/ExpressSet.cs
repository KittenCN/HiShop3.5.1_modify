namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.Entities.Sales;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.Vshop;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ExpressSet : AdminPage
    {
        protected Button btnSave;
        protected HtmlForm thisForm;
        protected TextBox txtKey;
        protected TextBox txtUrl;

        protected ExpressSet() : base("m09", "szp13")
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string key = this.txtKey.Text.Trim();
            string str2 = this.txtUrl.Text.Trim();
            if (string.IsNullOrEmpty(str2))
            {
                this.ShowMsg("物理查询地址不允许为空！", false);
            }
            else
            {
                ExpressHelper.UpdateExpressUrlAndKey(key, str2);
                this.ShowMsg("修改物流查询地址和快递100 key成功！", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                Hidistro.Entities.Sales.ExpressSet expressSet = ExpressHelper.GetExpressSet();
                if (expressSet != null)
                {
                    this.txtKey.Text = expressSet.NewKey;
                    this.txtUrl.Text = expressSet.Url;
                }
            }
        }
    }
}

