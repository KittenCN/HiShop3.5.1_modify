namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.Core;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.Vshop;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Xml;

    public class EditExpressTemplate : AdminPage
    {
        protected string ems;
        protected string height;
        protected string width;

        protected EditExpressTemplate() : base("m09", "szp07")
        {
            this.ems = "";
            this.width = "";
            this.height = "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int result = 0;
            string str = this.Page.Request.QueryString["ExpressName"];
            string str2 = this.Page.Request.QueryString["XmlFile"];
            if (!int.TryParse(this.Page.Request.QueryString["ExpressId"], out result))
            {
                base.GotoResourceNotFound();
            }
            else if ((string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str2)) || !str2.EndsWith(".xml"))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                DataTable expressTable = ExpressHelper.GetExpressTable();
                XmlDocument document = new XmlDocument();
                document.Load(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", str2)));
                string innerText = document.SelectSingleNode("/printer/size").InnerText;
                this.width = innerText.Split(new char[] { ':' })[0];
                this.height = innerText.Split(new char[] { ':' })[1];
                StringBuilder builder = new StringBuilder();
                foreach (DataRow row in expressTable.Rows)
                {
                    builder.AppendFormat("<option value='{0}' {1}>{0}</option>", row["Name"], row["Name"].Equals(str) ? "selected" : "");
                }
                this.ems = builder.ToString();
            }
        }
    }
}

