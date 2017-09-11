namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditGameGuaGuaLe : AdminPage
    {
        private int _gameId;
        protected Button btnSubmit1;
        protected GameType gameType;
        protected Label lbBeginTime;
        protected Label lbEedTime;
        protected Label lbGameDescription;
        protected Label lbGameNum;
        protected Label lbPrizeGade0;
        protected Label lbPrizeGade1;
        protected Label lbPrizeGade2;
        protected Label lbPrizeGade3;
        protected HtmlForm thisForm;
        protected Hidistro.UI.Web.Admin.promotion.UCGameInfo UCGameInfo;
        protected Hidistro.UI.Web.Admin.promotion.UCGamePrizeInfo UCGamePrizeInfo;

        protected EditGameGuaGuaLe() : base("m08", "yxp11")
        {
            this._gameId = -1;
            this.gameType = GameType.刮刮乐;
        }

        private void BindDate()
        {
            GameInfo modelByGameId = GameHelper.GetModelByGameId(this._gameId);
            if (modelByGameId != null)
            {
                this.UCGameInfo.GameInfo = modelByGameId;
                IList<GamePrizeInfo> gamePrizeListsByGameId = GameHelper.GetGamePrizeListsByGameId(this._gameId);
                this.UCGamePrizeInfo.PrizeLists = gamePrizeListsByGameId;
                this.UCGamePrizeInfo.NotPrzeDescription = modelByGameId.NotPrzeDescription;
                GamePrizeInfo info2 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.一等奖);
                if (info2 != null)
                {
                    this.lbPrizeGade0.Text = "一等奖：" + info2.PrizeType.ToString();
                }
                info2 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.二等奖);
                if (info2 != null)
                {
                    this.lbPrizeGade1.Text = "二等奖：" + info2.PrizeType.ToString();
                }
                info2 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.三等奖);
                if (info2 != null)
                {
                    this.lbPrizeGade2.Text = "三等奖：" + info2.PrizeType.ToString();
                }
                info2 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.四等奖);
                if (info2 != null)
                {
                    this.lbPrizeGade3.Text = "四等奖：" + info2.PrizeType.ToString();
                }
                this.lbBeginTime.Text = modelByGameId.BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                this.lbEedTime.Text = modelByGameId.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                this.lbGameDescription.Text = Globals.HtmlDecode(modelByGameId.Description);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.SaveDate();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this._gameId = int.Parse(base.Request.QueryString["gameId"]);
            }
            catch (Exception)
            {
                base.GotoResourceNotFound();
                return;
            }
            if (!this.Page.IsPostBack)
            {
                this.BindDate();
            }
        }

        private void SaveDate()
        {
            try
            {
                GameInfo gameInfo = this.UCGameInfo.GameInfo;
                gameInfo.NotPrzeDescription = this.UCGamePrizeInfo.NotPrzeDescription;
                IList<GamePrizeInfo> prizeLists = this.UCGamePrizeInfo.PrizeLists;
                int prizeCount = 0;
                if (!GameHelper.Update(gameInfo))
                {
                    throw new Exception("更新失败！");
                }
                foreach (GamePrizeInfo info2 in prizeLists)
                {
                    if (info2.PrizeType == PrizeType.赠送积分)
                    {
                        info2.PrizeImage = "/utility/pics/jifen60.png";
                    }
                    if (info2.PrizeType == PrizeType.赠送优惠券)
                    {
                        info2.PrizeImage = "/utility/pics/yhq60.png";
                    }
                    if (!GameHelper.UpdatePrize(info2))
                    {
                        throw new Exception("更新奖品信息时失败！");
                    }
                    prizeCount += info2.PrizeCount;
                }
                bool flag2 = GameHelper.DeleteWinningPools(gameInfo.GameId);
                if ((prizeCount > 0) && flag2)
                {
                    GameHelper.CreateWinningPools(gameInfo.PrizeRate, prizeCount, gameInfo.GameId);
                }
                this.Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ShowSuccess", "<script>$(function () { ShowStep2(); })</script>");
            }
            catch (Exception exception)
            {
                this.ShowMsg(exception.Message, false);
            }
        }
    }
}

