namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;

    public class GameEggLists : AdminPage
    {
        protected string isFinished;
        protected HtmlForm thisForm;
        protected UCGameLists UCGameLists1;

        protected GameEggLists() : base("m08", "yxp08")
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
                return GameType.疯狂砸金蛋;
            }
        }
    }
}

