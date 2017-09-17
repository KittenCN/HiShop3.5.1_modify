﻿using global::ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
    public partial class EditGameXingYun : AdminPage
    {
        private int _gameId;
       
        protected GameType gameType;
       

        protected EditGameXingYun() : base("m08", "yxp09")
        {
            this._gameId = -1;
            this.gameType = GameType.好运翻翻看;
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
                gameInfo.PrizeRate = this.UCGamePrizeInfo.PrizeRate;
                gameInfo.NotPrzeDescription = this.UCGamePrizeInfo.NotPrzeDescription;
                IList<GamePrizeInfo> prizeLists = this.UCGamePrizeInfo.PrizeLists;
                string[] strArray = new string[] { "一等奖", "二等奖", "三等奖", "四等奖" };
                int prizeCount = 0;
                if (!GameHelper.Update(gameInfo))
                {
                    throw new Exception("更新失败！");
                }
                for (int i = 0; i < prizeLists.Count<GamePrizeInfo>(); i++)
                {
                    GamePrizeInfo model = prizeLists[i];
                    model.GameId = gameInfo.GameId;
                    if (model.PrizeId > 0)
                    {
                        if (model.PrizeType == PrizeType.赠送积分)
                        {
                            model.PrizeImage = "/utility/pics/jifen60.png";
                        }
                        if (model.PrizeType == PrizeType.赠送优惠券)
                        {
                            model.PrizeImage = "/utility/pics/yhq60.png";
                        }
                        if (!GameHelper.UpdatePrize(model))
                        {
                            throw new Exception("更新奖品信息时失败！");
                        }
                        prizeCount += model.PrizeCount;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(model.PrizeName))
                        {
                            if (model.PrizeType == PrizeType.赠送积分)
                            {
                                model.PrizeImage = "/utility/pics/jifen60.png";
                            }
                            if (model.PrizeType == PrizeType.赠送优惠券)
                            {
                                model.PrizeImage = "/utility/pics/yhq60.png";
                            }
                            model.PrizeName = strArray[i];
                        }
                        if (!GameHelper.CreatePrize(model))
                        {
                            throw new Exception("添加奖品信息时失败！");
                        }
                        prizeCount += model.PrizeCount;
                    }
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