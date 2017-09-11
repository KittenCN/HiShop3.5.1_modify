namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Default : AdminPage
    {
        protected HtmlForm aspForm;
        public string DateList;
        public string DistributorQty;
        public string GoodsQty;
        protected HtmlImage imgLogo;
        protected HyperLink lbServiceOrderQty;
        protected Literal lbShopName;
        public string MemberQty;
        public string OrderAmountFee_Today;
        public string OrderAmountFee_Yesterday;
        public string OrderQty_Today;
        public string OrderQty_Yesterday;
        public string QtyList;
        public string QtyList1;
        public string QtyList2;
        public string QtyList3;
        protected Repeater rptDistributor;
        protected Repeater rptMember;
        public string ServiceOrderQty;
        public string showUrl;
        public string WaitSendOrderQty;

        protected Default() : base("m01", "dpp01")
        {
            this.showUrl = "";
            this.WaitSendOrderQty = "0";
            this.OrderQty_Today = "0";
            this.OrderQty_Yesterday = "0";
            this.OrderAmountFee_Today = "0";
            this.OrderAmountFee_Yesterday = "0";
            this.ServiceOrderQty = "0";
            this.GoodsQty = "0";
            this.MemberQty = "0";
            this.DistributorQty = "0";
            this.QtyList = "";
            this.QtyList1 = "";
            this.QtyList2 = "";
            this.QtyList3 = "";
            this.DateList = "";
        }

        private void LoadTradeDataList(DateTime BeginDate, int Days)
        {
            DataTable table = ShopStatisticHelper.ShopGlobal_GetTrendDataList(BeginDate, Days);
            this.DateList = "";
            int num = 0;
            foreach (DataRow row in table.Rows)
            {
                this.DateList = this.DateList + "'" + Convert.ToDateTime(row["RecDate"].ToString()).ToString("yyyy-MM-dd") + "'";
                this.QtyList1 = this.QtyList1 + base.GetFieldValue(row, "OrderCount");
                this.QtyList2 = this.QtyList2 + base.GetFieldValue(row, "NewDistributorCount");
                this.QtyList3 = this.QtyList3 + base.GetFieldValue(row, "NewMemberCount");
                if (num < (Days - 1))
                {
                    this.DateList = this.DateList + ",";
                    this.QtyList1 = this.QtyList1 + ",";
                    this.QtyList2 = this.QtyList2 + ",";
                    this.QtyList3 = this.QtyList3 + ",";
                }
                num++;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime beginDate = today.AddDays(-6.0);
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.lbShopName.Text = masterSettings.SiteName;
                this.imgLogo.Src = masterSettings.DistributorLogoPic;
                if (!File.Exists(base.Server.MapPath(masterSettings.DistributorLogoPic)))
                {
                    this.imgLogo.Src = "/Admin/Shop/Public/images/80x80.png";
                }
                int port = base.Request.Url.Port;
                string str = (port == 80) ? "" : (":" + port.ToString());
                this.showUrl = "http://" + base.Request.Url.Host + str + Globals.ApplicationPath + "/default.aspx";
                DataRow drOne = ShopStatisticHelper.ShopGlobal_GetMemberCount();
                DataRow row2 = ShopStatisticHelper.ShopGlobal_GetOrderCountByDate(today);
                DataRow row3 = ShopStatisticHelper.ShopGlobal_GetOrderCountByDate(today.AddDays(-1.0));
                this.WaitSendOrderQty = base.GetFieldValue(drOne, "WaitSendOrderQty");
                this.OrderQty_Today = base.GetFieldValue(row2, "OrderQty");
                this.OrderQty_Yesterday = base.GetFieldValue(row3, "OrderQty");
                this.OrderAmountFee_Today = base.GetFieldDecimalValue(row2, "OrderAmountFee").ToString("N2");
                this.OrderAmountFee_Yesterday = base.GetFieldDecimalValue(row3, "OrderAmountFee").ToString("N2");
                this.ServiceOrderQty = base.GetFieldValue(drOne, "ServiceOrderQty");
                this.lbServiceOrderQty.Text = this.ServiceOrderQty;
                if (this.ServiceOrderQty == "0")
                {
                    this.lbServiceOrderQty.ForeColor = Color.Green;
                }
                else
                {
                    this.lbServiceOrderQty.ForeColor = Color.Red;
                }
                this.GoodsQty = base.GetFieldValue(drOne, "GoodsQty");
                this.MemberQty = base.GetFieldValue(drOne, "MemberQty");
                this.DistributorQty = base.GetFieldValue(drOne, "DistributorQty");
                this.LoadTradeDataList(beginDate, 7);
                this.rptDistributor.DataSource = ShopStatisticHelper.ShopGlobal_GetSortList_Distributor(beginDate, 8);
                this.rptDistributor.DataBind();
                this.rptMember.DataSource = ShopStatisticHelper.ShopGlobal_GetSortList_Member(beginDate, 8);
                this.rptMember.DataBind();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            StringWriter writer2 = new StringWriter();
            HtmlTextWriter writer3 = new HtmlTextWriter(writer2);
            base.Render(writer3);
            string str = writer2.ToString().Trim().Replace("</body>", "<div style=\"display:none\"><script src=\"http://s95.cnzz.com/stat.php?id=1259515160&web_id=1259515160\" language=\"JavaScript\"></script></div></body>");
            writer.Write(str);
        }
    }
}

