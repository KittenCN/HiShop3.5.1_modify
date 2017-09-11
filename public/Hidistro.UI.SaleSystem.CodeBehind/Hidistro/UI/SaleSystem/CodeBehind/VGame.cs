namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ControlPanel.Promotions;
    using global::ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class VGame : VMemberTemplatedWebControl
    {
        private string htmlTitle = string.Empty;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle(this.htmlTitle);
            Literal literal = (Literal) this.FindControl("litJs");
            GameInfo modelByGameId = GameHelper.GetModelByGameId(Globals.RequestQueryStr("gamid"));
            string s = string.Empty;
            if (modelByGameId == null)
            {
                this.Context.Response.Redirect("/default.aspx");
                this.Context.Response.End();
            }
            else
            {
                s = modelByGameId.NotPrzeDescription;
                string gameTitle = modelByGameId.GameTitle;
                string description = modelByGameId.Description;
                Uri url = this.Context.Request.Url;
                string str5 = url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
                string str6 = string.Empty;
                GameType type = GameType.幸运大转盘;
                try
                {
                    type = (GameType) Enum.Parse(typeof(GameType), Globals.RequestQueryNum("type").ToString());
                }
                catch
                {
                }
                string str7 = string.Empty;
                switch (type)
                {
                    case GameType.幸运大转盘:
                        str6 = "/Utility/pics/game_dzp.png";
                        foreach (GamePrizeInfo info2 in GameHelper.GetGamePrizeListsByGameId(modelByGameId.GameId))
                        {
                            object obj2 = str7;
                            str7 = string.Concat(new object[] { obj2, "<img src='", info2.PrizeImage, "' id='price", info2.PrizeId, "' style='display:none;' />" });
                        }
                        break;

                    case GameType.疯狂砸金蛋:
                        str6 = "/Utility/pics/game_zjd.png";
                        break;

                    case GameType.好运翻翻看:
                        str6 = "/Utility/pics/game_ffk.png";
                        break;

                    case GameType.大富翁:
                        str6 = "/Utility/pics/game_dfw.png";
                        break;

                    case GameType.刮刮乐:
                        str6 = "/Utility/pics/game_ggk.png";
                        break;

                    default:
                        str6 = "/Utility/pics/game_dzp.png";
                        break;
                }
                literal.Text = str7 + "<script>thanksTips=\"" + this.Context.Server.HtmlEncode(s) + "\";wxinshare_title=\"" + this.Context.Server.HtmlEncode(gameTitle) + "\";wxinshare_desc=\"" + this.Context.Server.HtmlEncode(description.Replace("\n", " ").Replace("\r", "")) + "\";wxinshare_link=location.href;wxinshare_imgurl=\"" + str5 + str6 + "\"</script>";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            GameType type = GameType.幸运大转盘;
            try
            {
                type = (GameType) Enum.Parse(typeof(GameType), this.Page.Request.QueryString["type"]);
            }
            catch (Exception)
            {
                base.GotoResourceNotFound("");
            }
            this.htmlTitle = type.ToString();
            if (this.SkinName == null)
            {
                switch (type)
                {
                    case GameType.幸运大转盘:
                        this.SkinName = "skin-vGameZhuangPan.html";
                        goto Label_00C4;

                    case GameType.疯狂砸金蛋:
                        this.SkinName = "skin-vGameEgg.html";
                        goto Label_00C4;

                    case GameType.好运翻翻看:
                        this.SkinName = "skin-vGameHaoYun.html";
                        goto Label_00C4;

                    case GameType.大富翁:
                        this.SkinName = "skin-vGameDaFuWen.html";
                        goto Label_00C4;

                    case GameType.刮刮乐:
                        this.SkinName = "skin-vGameGuaGuaLe.html";
                        goto Label_00C4;
                }
                base.GotoResourceNotFound("");
            }
        Label_00C4:
            base.OnInit(e);
        }
    }
}

