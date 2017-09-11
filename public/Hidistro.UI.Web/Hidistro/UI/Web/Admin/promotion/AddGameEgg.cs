namespace Hidistro.UI.Web.Admin.promotion
{
    using global::ControlPanel.Promotions;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddGameEgg : AdminPage
    {
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

        protected AddGameEgg() : base("m08", "yxp08")
        {
            this.gameType = GameType.疯狂砸金蛋;
        }

        private void BindViewInfo(GameInfo gameInfo, IList<GamePrizeInfo> prizeLists)
        {
            this.lbGameDescription.Text = gameInfo.Description;
            this.lbBeginTime.Text = gameInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.lbEedTime.Text = gameInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.lbPrizeGade0.Text = string.Format("{0}：{1}", PrizeGrade.一等奖, prizeLists.FirstOrDefault<GamePrizeInfo>(p => (p.PrizeGrade == PrizeGrade.一等奖)).PrizeType.ToString());
            this.lbPrizeGade1.Text = string.Format("{0}：{1}", PrizeGrade.二等奖, prizeLists.FirstOrDefault<GamePrizeInfo>(p => (p.PrizeGrade == PrizeGrade.二等奖)).PrizeType.ToString());
            this.lbPrizeGade2.Text = string.Format("{0}：{1}", PrizeGrade.三等奖, prizeLists.FirstOrDefault<GamePrizeInfo>(p => (p.PrizeGrade == PrizeGrade.三等奖)).PrizeType.ToString());
            this.lbPrizeGade3.Text = string.Format("{0}：{1}", PrizeGrade.四等奖, prizeLists.FirstOrDefault<GamePrizeInfo>(p => (p.PrizeGrade == PrizeGrade.四等奖)).PrizeType.ToString());
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.SaveDate();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.UCGameInfo.GameType = this.gameType;
        }

        private void SaveDate()
        {
            int gameId = -1;
            try
            {
                GameInfo gameInfo = this.UCGameInfo.GameInfo;
                gameInfo.PrizeRate = this.UCGamePrizeInfo.PrizeRate;
                gameInfo.NotPrzeDescription = this.UCGamePrizeInfo.NotPrzeDescription;
                IList<GamePrizeInfo> prizeLists = this.UCGamePrizeInfo.PrizeLists;
                string[] strArray = new string[] { "一等奖", "二等奖", "三等奖", "四等奖" };
                int prizeCount = 0;
                if (!GameHelper.Create(gameInfo, out gameId))
                {
                    throw new Exception("添加失败！");
                }
                for (int i = 0; i < prizeLists.Count<GamePrizeInfo>(); i++)
                {
                    GamePrizeInfo model = prizeLists[i];
                    model.GameId = gameId;
                    if (string.IsNullOrEmpty(model.PrizeName))
                    {
                        model.PrizeName = strArray[i];
                    }
                    if (model.PrizeType == PrizeType.赠送积分)
                    {
                        model.PrizeImage = "/utility/pics/jifen60.png";
                    }
                    if (model.PrizeType == PrizeType.赠送优惠券)
                    {
                        model.PrizeImage = "/utility/pics/yhq60.png";
                    }
                    if (!GameHelper.CreatePrize(model))
                    {
                        throw new Exception("添加奖品信息时失败！");
                    }
                    prizeCount += model.PrizeCount;
                }
                if (prizeCount > 0)
                {
                    GameHelper.CreateWinningPools(gameInfo.PrizeRate, prizeCount, gameId);
                }
                this.Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ShowSuccess", "<script>$(function () { ShowStep2(); })</script>");
            }
            catch (Exception exception)
            {
                if (gameId > 0)
                {
                    GameHelper.Delete(new int[] { gameId });
                }
                this.ShowMsg(exception.Message, false);
            }
        }
    }
}

