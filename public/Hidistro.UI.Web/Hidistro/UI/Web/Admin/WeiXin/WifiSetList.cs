namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.IO;
    using System.Web;
    using System.Web.UI.WebControls;

    public class WifiSetList : AdminPage
    {
        protected Repeater rptWifiSetList;
        protected Script Script5;
        protected Script Script6;

        protected WifiSetList() : base("m06", "wxp12")
        {
        }

        public void BindData()
        {
            DataSet set = new DataSet();
            string path = HttpContext.Current.Server.MapPath("/config/WifiConfig.xml");
            if (File.Exists(path))
            {
                set.ReadXml(path);
                if ((set != null) && (set.Tables.Count > 0))
                {
                    this.rptWifiSetList.DataSource = set;
                    this.rptWifiSetList.DataBind();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}

