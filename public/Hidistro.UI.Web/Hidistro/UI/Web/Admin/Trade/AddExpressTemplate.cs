namespace Hidistro.UI.Web.Admin.Trade
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.Vshop;
    using System;
    using System.Data;
    using System.Text;

    public class AddExpressTemplate : AdminPage
    {
        protected string ems;

        protected AddExpressTemplate() : base("m03", "ddp11")
        {
            this.ems = "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request["task"];
            string str2 = base.Request["ExpressName"];
            if ((!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str2)) && (str == "IsExist"))
            {
                if (SalesHelper.IsExistExpress(str2))
                {
                    base.Response.Write("Y");
                }
                else
                {
                    base.Response.Write("N");
                }
                base.Response.End();
            }
            if (!base.IsPostBack)
            {
                DataTable expressTable = ExpressHelper.GetExpressTable();
                StringBuilder builder = new StringBuilder();
                foreach (DataRow row in expressTable.Rows)
                {
                    builder.AppendFormat("<option value='{0}'>{0}</option>", row["Name"]);
                }
                this.ems = builder.ToString();
            }
        }
    }
}

