namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;

    public class GameXingYunLists : AdminPage
    {
        protected string isFinished;
        protected HtmlForm thisForm;
        protected UCGameLists UCGameLists1;

        protected GameXingYunLists() : base("m08", "yxp09")
        {
            this.isFinished = "0";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.UCGameLists1.PGameType = new GameType?(this.PGameType);
            this.isFinished = base.Request.QueryString["isFinished"];
            if (string.IsNullOrEmpty(this.isFinished))
            {
                this.isFinished = "0";
            }
        }

        protected GameType PGameType
        {
            get
            {
                return GameType.好运翻翻看;
            }
        }
    }
}

