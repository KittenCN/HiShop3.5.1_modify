namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.UI.WebControls;

    public class PromotionGamePrizesHandler : IHttpHandler
    {
        private string AddPrizeHtml(HttpContext context)
        {
            string str = "";
            int gameId = 0;
            int result = 0;
            PrizeGrade prizeGrade = PrizeGrade.一等奖;
            gameId = Globals.RequestQueryNum("gameId");
            if (!int.TryParse(context.Request.QueryString["index"], out result))
            {
                return str;
            }
            switch (result)
            {
                case 0:
                    prizeGrade = PrizeGrade.一等奖;
                    break;

                case 1:
                    prizeGrade = PrizeGrade.二等奖;
                    break;

                case 2:
                    prizeGrade = PrizeGrade.三等奖;
                    break;

                case 3:
                    prizeGrade = PrizeGrade.四等奖;
                    break;
            }
            return ("{\"type\":\"ok\",\"message\":\"" + Globals.String2Json(this.GetPrizeInfoHtml(prizeGrade, null, result, gameId)) + "\"}");
        }

        private List<ListItem> BindDdlCouponId()
        {
            List<ListItem> list = new List<ListItem>();
            DataTable unFinishedCoupon = CouponHelper.GetUnFinishedCoupon(DateTime.Now, (CouponType)3);
            if (unFinishedCoupon != null)
            {
                foreach (DataRow row in unFinishedCoupon.Rows)
                {
                    ListItem item = new ListItem {
                        Text = row["CouponName"].ToString(),
                        Value = row["CouponId"].ToString()
                    };
                    list.Add(item);
                }
            }
            return list;
        }

        public string DeletePrize(HttpContext context)
        {
            int result = 0;
            int num2 = 0;
            string str = "{\"type\":\"fail\",\"message\":\"删除失败，请确认活动未开始！\"}";
            if (!int.TryParse(context.Request.QueryString["gameId"], out result) || !int.TryParse(context.Request.QueryString["prizeId"], out num2))
            {
                return str;
            }
            GamePrizeInfo model = new GamePrizeInfo {
                GameId = result,
                PrizeId = num2
            };
            if (!GameHelper.DeletePromotionGamePrize(model))
            {
                return str;
            }
            bool flag2 = GameHelper.DeleteWinningPools(model.GameId);
            GameInfo gameInfoById = GameHelper.GetGameInfoById(model.GameId);
            if (gameInfoById == null)
            {
                return str;
            }
            int prizeCount = 0;
            foreach (GamePrizeInfo info3 in GameHelper.GetGamePrizeListsByGameId(model.GameId))
            {
                prizeCount += info3.PrizeCount;
            }
            if ((prizeCount > 0) && flag2)
            {
                GameHelper.CreateWinningPools(gameInfoById.PrizeRate, prizeCount, gameInfoById.GameId);
            }
            return "{\"type\":\"ok\",\"message\":\"删除成功\"}";
        }

        protected string GetPrizeInfoHtml(PrizeGrade prizeGrade, GamePrizeInfo model, int index, int gameId)
        {
            List<ListItem> list = this.BindDdlCouponId();
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class='tabContent' id='tempContentId" + index + "'>");
            builder.Append("<div class='form-horizontal clearfix'>");
            builder.Append("<div class='form-group setmargin'>");
            builder.Append("<label class='col-xs-3 pad resetSize control-label'><em>*</em>&nbsp;&nbsp;奖品类别：</label>");
            builder.Append("<div class='form-inline col-xs-9'>");
            builder.Append("<div class='resetradio selectradio pt3' >");
            builder.Append("<label class=\"mr20\">");
            if (model != null)
            {
                builder.AppendFormat(" <input type=\"radio\" id=\"rd{0}_0\" name=\"prizeType_{0}\" {1} value=\"0\" />赠送积分</label>", (int) prizeGrade, (model.PrizeType == PrizeType.赠送积分) ? "checked=\"checked\"" : "");
                builder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_1\" name=\"prizeType_{0}\" {1} value=\"1\" />赠送优惠券</label>", (int) prizeGrade, (model.PrizeType == PrizeType.赠送优惠券) ? "checked=\"checked\"" : "");
                builder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_2\" name=\"prizeType_{0}\" {1} value=\"2\" />赠送商品</label>", (int) prizeGrade, (model.PrizeType == PrizeType.赠送商品) ? "checked=\"checked\"" : "");
                builder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_3\" name=\"prizeType_{0}\" {1} value=\"3\" />其他奖品</label>", (int) prizeGrade, (model.PrizeType == PrizeType.其他奖品) ? "checked=\"checked\"" : "");
                builder.AppendFormat("<input type=\"hidden\" id=\"prizeTypeValue{0}\" value=\"{1}\" />", (int) prizeGrade, (int) model.PrizeType);
                builder.AppendFormat("<input type=\"hidden\" name=\"prizeInfoId{0}\" value=\"{1}\" />", (int) prizeGrade, model.PrizeId);
                builder.AppendFormat("<input type=\"hidden\" name=\"prizeGameId{0}\" value=\"{1}\" />", (int) prizeGrade, model.GameId);
            }
            else
            {
                builder.AppendFormat(" <input type=\"radio\" id=\"rd{0}_0\" name=\"prizeType_{0}\" checked=\"checked\" value=\"0\" />赠送积分</label>", (int) prizeGrade);
                builder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_1\" name=\"prizeType_{0}\" value=\"1\" />赠送优惠券</label>", (int) prizeGrade);
                builder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_2\" name=\"prizeType_{0}\" value=\"2\" />赠送商品</label>", (int) prizeGrade);
                builder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_3\" name=\"prizeType_{0}\" value=\"3\" />其他奖品</label>", (int) prizeGrade);
                builder.AppendFormat("<input type=\"hidden\" id=\"prizeTypeValue{0}\" value=\"{1}\" />", (int) prizeGrade, index);
                builder.AppendFormat("<input type=\"hidden\" name=\"prizeInfoId{0}\" value=\"{1}\" />", (int) prizeGrade, 0);
                builder.AppendFormat("<input type=\"hidden\" name=\"prizeGameId{0}\" value=\"{1}\" />", (int) prizeGrade, gameId);
            }
            builder.Append(" </div></div></div>");
            builder.Append("<div class=\"form-group setmargin\" style=\"display:normal\">");
            builder.Append(" <label class=\"col-xs-3 pad resetSize control-label\" for=\"Prize\"><em>*</em>&nbsp;&nbsp;奖品名称：</label> <div class=\"form-inline col-xs-9\">");
            if (model != null)
            {
                builder.AppendFormat("<input type=\"text\" name=\"txtPrize{0}\" id=\"txtPrize{0}\" class=\"form-control resetSize\" value=\"{1}\"/>", (int) prizeGrade, model.Prize);
            }
            else
            {
                builder.AppendFormat("<input type=\"text\" name=\"txtPrize{0}\" id=\"txtPrize{0}\" class=\"form-control resetSize\" value=\"\"/>", (int) prizeGrade);
            }
            builder.Append("</div></div>");
            if ((model != null) && (model.PrizeType == PrizeType.赠送积分))
            {
                builder.Append(" <div class=\"form-group setmargin give giveint\"  style=\"display:normal\">");
            }
            else
            {
                builder.Append(" <div class=\"form-group setmargin give giveint\">");
            }
            builder.Append(" <label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp");
            builder.Append("赠送积分：</label> <div class=\"form-inline col-xs-9\">");
            if (model != null)
            {
                builder.AppendFormat(" <input type=\"text\" name=\"txtGivePoint{0}\" id=\"txtGivePoint{0}\" class=\"form-control resetSize\" value=\"{1}\" />", (int) prizeGrade, model.GivePoint);
            }
            else
            {
                builder.AppendFormat(" <input type=\"text\" name=\"txtGivePoint{0}\" id=\"txtGivePoint{0}\" class=\"form-control resetSize\" value=\"0\" />", (int) prizeGrade);
            }
            if ((model != null) && (model.PrizeType == PrizeType.赠送优惠券))
            {
                builder.Append(" </div> </div><div class=\"form-group setmargin give givecop\" style=\"display:normal\">");
            }
            else
            {
                builder.Append(" </div> </div><div class=\"form-group setmargin give givecop\">");
            }
            builder.Append(" <label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp;赠送优惠券：</label> <div class=\"form-inline col-xs-9\">");
            builder.AppendFormat(" <select name=\"seletCouponId{0}\" id=\"seletCouponId{0}\" class=\"form-control resetSize\">", (int) prizeGrade);
            if (model != null)
            {
                foreach (ListItem item in list)
                {
                    if (string.Equals(model.GiveCouponId, item.Value))
                    {
                        builder.AppendFormat(" <option value=\"{0}\" selected=\"selected\">{1}</option>", item.Value, item.Text);
                    }
                    else
                    {
                        builder.AppendFormat(" <option value=\"{0}\">{1}</option>", item.Value, item.Text);
                    }
                }
            }
            else
            {
                foreach (ListItem item2 in list)
                {
                    builder.AppendFormat(" <option value=\"{0}\">{1}</option>", item2.Value, item2.Text);
                }
            }
            builder.Append(" </select> </div>  </div> ");
            if ((model != null) && (model.PrizeType == PrizeType.赠送商品))
            {
                builder.Append("<div class=\"form-group setmargin give giveshop\" style=\"display:normal\">");
            }
            else
            {
                builder.Append("<div class=\"form-group setmargin give giveshop\">");
            }
            builder.Append("<label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp;赠送商品：</label>");
            builder.Append("<div class=\"form-inline col-xs-9\"><div class=\"pt3\">");
            if (model != null)
            {
                builder.AppendFormat("<img id=\"imgProduct{0}\" style=\"width:30px; height:30px;\" name=\"imgProduct{0}\"  src=\"{1}\"onclick=\"SelectShopBookId({0});\" />", (int) prizeGrade, string.IsNullOrEmpty(model.GriveShopBookPicUrl) ? "../images/u100.png" : model.GriveShopBookPicUrl);
                builder.AppendFormat("<input type=\"hidden\" name=\"txtShopbookId{0}\" id=\"txtShopbookId{0}\"  value=\"{1}\" />", (int) prizeGrade, model.GiveShopBookId);
                builder.AppendFormat("<input type=\"hidden\" id=\"txtProductPic{0}\" name=\"txtProductPic{0}\"  value=\"{1}\" />", (int) prizeGrade, string.IsNullOrEmpty(model.GriveShopBookPicUrl) ? "../images/u100.png" : model.GriveShopBookPicUrl);
            }
            else
            {
                builder.AppendFormat("<img id=\"imgProduct{0}\" style=\"width:30px; height:30px;\" name=\"imgProduct{0}\" src=\"../images/u100.png\" onclick=\"SelectShopBookId({0});\" />", (int) prizeGrade);
                builder.AppendFormat("<input type=\"hidden\" name=\"txtShopbookId{0}\" id=\"txtShopbookId{0}\" />", (int) prizeGrade);
                builder.AppendFormat("<input type=\"hidden\" id=\"txtProductPic{0}\" name=\"txtProductPic{0}\"  />", (int) prizeGrade);
            }
            builder.Append("</div> </div></div>");
            if ((model != null) && (model.PrizeType == PrizeType.其他奖品))
            {
                builder.Append("<div class=\"form-group setmargin give other\" style=\"display:normal\">");
            }
            else
            {
                builder.Append("<div class=\"form-group setmargin give other\">");
            }
            builder.Append("<label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp;是否配送：</label>");
            builder.Append("<div class=\"form-inline col-xs-9\"><div class=\"pt3 resetradio mb5 pt3 allradio\">");
            if (model != null)
            {
                builder.AppendFormat("<label class=\"mr20\"> <input type=\"checkbox\" id=\"ckbNeed_{0}\" name=\"ckbNeed_{0}\"  {1}>是，需要配送</label>", (int) prizeGrade, (model.IsLogistics == 1) ? "checked" : "");
            }
            else
            {
                builder.AppendFormat("<label class=\"mr20\"> <input type=\"checkbox\" id=\"ckbNeed_{0}\" name=\"ckbNeed_{0}\" >是，需要配送</label>", (int) prizeGrade);
            }
            builder.Append("</div> </div></div>");
            builder.Append("<div class=\"form-group setmargin\">");
            builder.Append(" <label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp;奖品数量：</label> <div class=\"form-inline col-xs-9\">");
            if (model != null)
            {
                builder.AppendFormat("<input type=\"text\" name=\"txtPrizeCount{0}\" id=\"txtPrizeCount{0}\" class=\"form-control resetSize\" value=\"{1}\"/>", (int) prizeGrade, model.PrizeCount);
            }
            else
            {
                builder.AppendFormat("<input type=\"text\" name=\"txtPrizeCount{0}\" id=\"txtPrizeCount{0}\" class=\"form-control resetSize\" value=\"1\"/>", (int) prizeGrade);
            }
            builder.Append("  <small>奖品数量为0时不设此奖项</small> </div> </div>");
            if ((model != null) && (model.PrizeType == PrizeType.其他奖品))
            {
                builder.Append("<div class=\"form-group setmargin give other\" style=\"display:normal\">");
            }
            else
            {
                builder.Append("<div class=\"form-group setmargin give other\">");
            }
            builder.Append("<label class=\"col-xs-3 pad resetSize control-label\" for=\"PrizeImage\"><em></em>&nbsp;&nbsp;奖品图片：</label>");
            builder.Append("<div class=\"form-inline col-xs-9\"><div class=\"pt3\" style=\"vertical-align:bottom;\">");
            if (model != null)
            {
                builder.AppendFormat("<img id=\"PrizeImage{0}\" style=\"width:60px; height:60px;\" name=\"PrizeImage{0}\"  src=\"{1}\"onclick=\"SelectPrizeImage({0});\" />  <div style=\"margin-left:70px\">仅支持jpg、 png、gif，尺寸60*60px,不超过1M  </div>", (int) prizeGrade, string.IsNullOrEmpty(model.PrizeImage) ? "../images/u100.png" : model.PrizeImage);
                builder.AppendFormat("<input type=\"hidden\" id=\"hiddPrizeImage{0}\" name=\"hiddPrizeImage{0}\"  value=\"{1}\" />", (int) prizeGrade, string.IsNullOrEmpty(model.PrizeImage) ? "../images/u100.png" : model.PrizeImage);
            }
            else
            {
                builder.AppendFormat("<img id=\"PrizeImage{0}\" style=\"width:60px; height:60px;\" name=\"PrizeImage{0}\" src=\"../images/u100.png\" onclick=\"SelectPrizeImage({0});\" />  <div style=\"margin-left:70px\">仅支持jpg、 png、gif，尺寸60*60px,不超过1M</div> ", (int) prizeGrade);
                builder.AppendFormat("<input type=\"hidden\" id=\"hiddPrizeImage{0}\" name=\"hiddPrizeImage{0}\"  />", (int) prizeGrade);
            }
            builder.Append("</div> </div></div>");
            builder.Append("</div></div>");
            return builder.ToString();
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string s = "{\"type\":\"fail\",\"message\":\"删除失败\"}";
                switch (context.Request.QueryString["action"])
                {
                    case "DeletePrize":
                        s = this.DeletePrize(context);
                        break;

                    case "AddPrize":
                        s = this.AddPrizeHtml(context);
                        break;
                }
                context.Response.Write(s);
            }
            catch (Exception exception)
            {
                context.Response.Write("{\"type\":\"error\",\"message\":\"" + exception.Message + "\"}");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

