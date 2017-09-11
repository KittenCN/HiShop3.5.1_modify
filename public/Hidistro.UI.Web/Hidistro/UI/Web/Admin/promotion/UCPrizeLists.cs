namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
   using  global:: ControlPanel.Promotions;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class UCPrizeLists : UserControl
    {
        protected int gameId = -1;
        protected Grid grdPrizeLists;
        protected PageSize hrefPageSize;
        protected string isFinished = "1";
        protected Label lbGameName;
        private int pageIndex = 1;
        protected Pager pager1;
        private int pageSize = 10;

        private void BindData()
        {
            PrizesDeliveQuery query = new PrizesDeliveQuery {
                SortBy = "LogId",
                GameId = new int?(this.gameId),
                PageIndex = this.pageIndex,
                PageSize = this.pageSize
            };
            string isFinished = this.isFinished;
            if (!string.IsNullOrEmpty(isFinished))
            {
                query.IsUsed = new int?(int.Parse(isFinished));
            }
            DbQueryResult prizeLogLists = GameHelper.GetPrizeLogLists(query);
            DataTable data = (DataTable) prizeLogLists.Data;
            this.grdPrizeLists.DataSource = data;
            this.grdPrizeLists.DataBind();
            this.pager1.TotalRecords = prizeLogLists.TotalRecords;
        }

        private void BindGameInfo()
        {
            GameInfo modelByGameId = GameHelper.GetModelByGameId(this.gameId);
            if (modelByGameId != null)
            {
                this.lbGameName.Text = modelByGameId.GameTitle;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.isFinished = base.Request.QueryString["isFinished"];
            if (string.IsNullOrEmpty(this.isFinished))
            {
                this.isFinished = "1";
            }
            try
            {
                this.pageIndex = int.Parse(base.Request["pageindex"]);
            }
            catch (Exception)
            {
                this.pageIndex = 1;
            }
            try
            {
                this.pageSize = int.Parse(base.Request.QueryString["pagesize"]);
            }
            catch (Exception)
            {
                this.pageSize = 10;
            }
            try
            {
                this.gameId = int.Parse(base.Request.QueryString["gameId"]);
            }
            catch (Exception)
            {
            }
            this.pager1.DefaultPageSize = this.pageSize;
            if (!base.IsPostBack)
            {
                this.BindGameInfo();
                this.BindData();
            }
        }
    }
}

