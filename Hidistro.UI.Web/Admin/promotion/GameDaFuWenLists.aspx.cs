using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.promotion
{
    public partial class GameDaFuWenLists : AdminPage
    {
        protected string isFinished;
       
       

        protected GameDaFuWenLists() : base("m08", "yxp10")
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
                return GameType.大富翁;
            }
        }
    }
}