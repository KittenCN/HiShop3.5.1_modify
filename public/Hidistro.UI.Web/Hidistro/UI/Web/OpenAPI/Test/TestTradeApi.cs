namespace Hidistro.UI.Web.OpenAPI.Test
{
    using global::Hishop.Open.Api;
    using Hidistro.Core;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.OpenAPI.Impl;
    using Hishop.Open.Api;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class TestTradeApi : AdminPage
    {
        protected Button btnTestChangLogistics;
        protected Button btnTestGetIncrementSoldTrades;
        protected Button btnTestGetSoldTrades;
        protected Button btnTestGetTrade;
        protected Button btnTestSendLogistic;
        protected Button btnTestUpdateTradeMemo;
        protected DropDownList ddlStatus1;
        protected DropDownList ddlStatus2;
        protected HtmlForm form1;
        private ITrade tradeApi;
        protected TextBox txtBuyerName1;
        protected TextBox txtBuyerName2;
        protected TextBox txtCompanyName1;
        protected TextBox txtCompanyName4;
        protected TextBox txtRemark1;
        protected TextBox txtSendLogistic;
        protected TextBox txtTag;
        protected TextBox txtTestChangLogistics;
        protected TextBox txtTestGetIncrementSoldTrades;
        protected TextBox txtTestGetSoldTrades;
        protected TextBox txtTestGetTrade;
        protected TextBox txtTestUpdateTradeMemo;
        protected TextBox txtTradeId1;
        protected TextBox txtTradeId2;
        protected TextBox txtTradeId3;
        protected TextBox txtTradeId4;
        protected TextBox txtTransId1;
        protected TextBox txtTransId4;

        protected TestTradeApi() : base("m03", "00000")
        {
            this.tradeApi = new TradeApi();
        }

        protected void btnTestChangLogistics_Click(object sender, EventArgs e)
        {
            string text = this.txtTradeId1.Text;
            string str2 = this.txtCompanyName1.Text;
            string str3 = this.txtTransId1.Text;
            string str4 = this.tradeApi.ChangLogistics(text, str2, str3);
            this.txtTestChangLogistics.Text = str4;
        }

        protected void btnTestGetIncrementSoldTrades_Click(object sender, EventArgs e)
        {
            string status = (this.ddlStatus2.SelectedValue == "all") ? "" : this.ddlStatus1.SelectedValue;
            string text = this.txtBuyerName2.Text;
            string str3 = this.tradeApi.GetIncrementSoldTrades(DateTime.Now.AddDays(-1.0), DateTime.Now, status, text, 1, 10);
            this.txtTestGetIncrementSoldTrades.Text = str3;
        }

        protected void btnTestGetSoldTrades_Click(object sender, EventArgs e)
        {
            string status = (this.ddlStatus1.SelectedValue == "all") ? "" : this.ddlStatus1.SelectedValue;
            string text = this.txtBuyerName1.Text;
            string str3 = this.tradeApi.GetSoldTrades(new DateTime?(DateTime.Now.AddMonths(-1)), new DateTime?(DateTime.Now), status, text, 1, 10);
            this.txtTestGetSoldTrades.Text = str3;
        }

        protected void btnTestGetTrade_Click(object sender, EventArgs e)
        {
            string text = this.txtTradeId2.Text;
            string trade = this.tradeApi.GetTrade(text);
            this.txtTestGetTrade.Text = trade;
        }

        protected void btnTestSendLogistic_Click(object sender, EventArgs e)
        {
            string text = this.txtTradeId4.Text;
            string str2 = this.txtCompanyName4.Text;
            string str3 = this.txtTransId4.Text;
            string str4 = this.tradeApi.SendLogistic(text, str2, str3);
            this.txtSendLogistic.Text = str4;
        }

        protected void btnTestUpdateTradeMemo_Click(object sender, EventArgs e)
        {
            string text = this.txtTradeId3.Text;
            string memo = this.txtRemark1.Text;
            int flag = Globals.ToNum(this.txtTag.Text);
            string str3 = this.tradeApi.UpdateTradeMemo(text, memo, flag);
            this.txtTestUpdateTradeMemo.Text = str3;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

