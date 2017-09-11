namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;

    public class DistributorSummary : AdminPage
    {
        public int AgentNumber;
        public string DateList;
        public decimal FinishedDrawCommissionFee;
        public decimal FXCommissionFee;
        public decimal FXCommissionFee_Yesterday;
        public int FXOrderNumber;
        public int FXOrderNumber_Yesterday;
        public decimal FXResultPercent;
        public decimal FXResultPercent_Yesterday;
        public decimal FXValidOrderTotal;
        public decimal FXValidOrderTotal_Yesterday;
        public int NewAgentNumber_Yesterday;
        protected HtmlGenericControl OrdersTotal;
        public string QtyList1;
        public string QtyList2;
        public string QtyList3;
        protected HtmlGenericControl ReferralBlance;
        protected HtmlGenericControl ReferralOrders;
        public decimal SaleAmountFee;
        public decimal SaleAmountFee_Yesterday;
        protected HtmlGenericControl TotalReferral;
        public decimal WaitDrawCommissionFee;

        protected DistributorSummary() : base("m05", "fxp01")
        {
            this.QtyList1 = "";
            this.QtyList2 = "";
            this.QtyList3 = "";
            this.DateList = "";
        }

        private void LoadData()
        {
            DateTime today = DateTime.Today;
            DateTime recDate = today.AddDays(-1.0);
            DataRow drOne = ShopStatisticHelper.Distributor_GetGlobal(today);
            if (drOne != null)
            {
                this.SaleAmountFee = base.GetFieldDecimalValue(drOne, "ValidOrderTotal");
                this.FXValidOrderTotal = base.GetFieldDecimalValue(drOne, "FXValidOrderTotal");
                this.FXOrderNumber = base.GetFieldIntValue(drOne, "FXOrderNumber");
                this.FXCommissionFee = base.GetFieldDecimalValue(drOne, "FXSumCommission");
                this.FXResultPercent = 0M;
                if ((this.SaleAmountFee > 0M) && (this.FXValidOrderTotal > 0M))
                {
                    this.FXResultPercent = Convert.ToDecimal((decimal) ((this.FXValidOrderTotal / this.SaleAmountFee) * 100M));
                }
            }
            drOne = ShopStatisticHelper.Distributor_GetGlobal(recDate);
            if (drOne != null)
            {
                this.SaleAmountFee_Yesterday = base.GetFieldDecimalValue(drOne, "ValidOrderTotal");
                this.FXValidOrderTotal_Yesterday = base.GetFieldDecimalValue(drOne, "FXValidOrderTotal");
                this.FXOrderNumber_Yesterday = base.GetFieldIntValue(drOne, "FXOrderNumber");
                this.FXCommissionFee_Yesterday = base.GetFieldDecimalValue(drOne, "FXSumCommission");
                this.FXResultPercent_Yesterday = 0M;
                if ((this.SaleAmountFee_Yesterday > 0M) && (this.FXValidOrderTotal_Yesterday > 0M))
                {
                    this.FXResultPercent_Yesterday = Convert.ToDecimal((decimal) ((this.FXValidOrderTotal_Yesterday / this.SaleAmountFee_Yesterday) * 100M));
                }
            }
            drOne = ShopStatisticHelper.Distributor_GetGlobalTotal(recDate);
            if (drOne != null)
            {
                this.AgentNumber = base.GetFieldIntValue(drOne, "DistributorNumber");
                this.NewAgentNumber_Yesterday = base.GetFieldIntValue(drOne, "NewAgentNumber");
                this.FinishedDrawCommissionFee = base.GetFieldDecimalValue(drOne, "FinishedDrawCommissionFee");
                this.WaitDrawCommissionFee = base.GetFieldDecimalValue(drOne, "WaitDrawCommissionFee");
            }
        }

        private void LoadTradeDataList(DateTime BeginDate, int Days)
        {
            DataTable table = ShopStatisticHelper.GetTrendDataList_FX(BeginDate, Days);
            this.DateList = "";
            int num = 0;
            foreach (DataRow row in table.Rows)
            {
                this.DateList = this.DateList + "'" + Convert.ToDateTime(row["RecDate"].ToString()).ToString("yyyy-MM-dd") + "'";
                this.QtyList1 = this.QtyList1 + base.GetFieldIntValue(row, "NewAgentCount");
                this.QtyList2 = this.QtyList2 + base.GetFieldDecimalValue(row, "FXAmountFee");
                this.QtyList3 = this.QtyList3 + base.GetFieldDecimalValue(row, "FXCommisionFee");
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
            DateTime beginDate = DateTime.Today.AddDays(-6.0);
            if (!base.IsPostBack)
            {
                string retInfo = "";
                ShopStatisticHelper.StatisticsOrdersByRecDate(DateTime.Today, UpdateAction.AllUpdate, 0, out retInfo);
                this.LoadData();
                this.LoadTradeDataList(beginDate, 7);
            }
        }
    }
}

