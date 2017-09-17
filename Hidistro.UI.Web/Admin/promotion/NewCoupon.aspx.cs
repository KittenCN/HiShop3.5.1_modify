using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
    public partial class NewCoupon : AdminPage
    {
        

        protected NewCoupon() : base("m08", "yxp01")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                for (int i = 1; i <= 10; i++)
                {
                    string text = i.ToString() + "张";
                    this.ddl_maxNum.Items.Add(new ListItem(text, i.ToString()));
                }
            }
        }
    }
}