namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class UCGamePrizeInfo : UserControl
    {
        private List<ListItem> _couponList = new List<ListItem>();
        private IList<GamePrizeInfo> _prizeLists;
        protected HiddenField hfGameId;
        protected HiddenField hfIndex;
        protected int prizeTypeValue0;
        protected int prizeTypeValue1;
        protected int prizeTypeValue2;
        protected int prizeTypeValue3;
        protected TextBox txtNotPrzeDescription;
        protected TextBox txtPrizeRate;

        private void BindDdlCouponId()
        {
            DataTable unFinishedCoupon = CouponHelper.GetUnFinishedCoupon(DateTime.Now, (CouponType)3);
            if (unFinishedCoupon != null)
            {
                foreach (DataRow row in unFinishedCoupon.Rows)
                {
                    ListItem item = new ListItem {
                        Text = row["CouponName"].ToString(),
                        Value = row["CouponId"].ToString()
                    };
                    this._couponList.Add(item);
                }
            }
        }

        private bool GetDate()
        {
            if (!this.Page.IsPostBack)
            {
                return true;
            }
            this._prizeLists = new List<GamePrizeInfo>();
            bool flag = true;
            switch (int.Parse(this.hfIndex.Value))
            {
                case 1:
                    this._prizeLists.Add(this.GetModel(PrizeGrade.一等奖));
                    return flag;

                case 2:
                    this._prizeLists.Add(this.GetModel(PrizeGrade.一等奖));
                    this._prizeLists.Add(this.GetModel(PrizeGrade.二等奖));
                    return flag;

                case 3:
                    this._prizeLists.Add(this.GetModel(PrizeGrade.一等奖));
                    this._prizeLists.Add(this.GetModel(PrizeGrade.二等奖));
                    this._prizeLists.Add(this.GetModel(PrizeGrade.三等奖));
                    return flag;

                case 4:
                    this._prizeLists.Add(this.GetModel(PrizeGrade.一等奖));
                    this._prizeLists.Add(this.GetModel(PrizeGrade.二等奖));
                    this._prizeLists.Add(this.GetModel(PrizeGrade.三等奖));
                    this._prizeLists.Add(this.GetModel(PrizeGrade.四等奖));
                    return flag;
            }
            return flag;
        }

        protected string GetGameMenu()
        {
            StringBuilder builder = new StringBuilder();
            int result = 0;
            if (int.TryParse(base.Request.QueryString["gameId"], out result))
            {
                IList<GamePrizeInfo> gamePrizeListsByGameId = GameHelper.GetGamePrizeListsByGameId(result);
                if ((gamePrizeListsByGameId != null) && (gamePrizeListsByGameId.Count<GamePrizeInfo>() > 0))
                {
                    int num2 = 0;
                    foreach (GamePrizeInfo info in gamePrizeListsByGameId)
                    {
                        string str = "";
                        if (num2 == 0)
                        {
                            str = "active";
                        }
                        if (num2 > 0)
                        {
                            builder.AppendFormat("<li class=\"{0}\" id=\"li{2}\" lival=\"{2}\">{1}<i class='' onclick='DelPrize(this)'></i></li>", str, info.PrizeName, info.PrizeId);
                        }
                        else
                        {
                            builder.AppendFormat("<li class=\"{0}\" id=\"li{2}\" lival=\"{2}\">{1}</li>", str, info.PrizeName, info.PrizeId);
                        }
                        num2++;
                    }
                }
                else
                {
                    builder.AppendFormat("<li class=\"{0}\">{1}</li>", "active", "一等奖");
                    this.hfIndex.Value = "1";
                }
            }
            else
            {
                builder.AppendFormat("<li class=\"{0}\">{1}</li>", "active", "一等奖");
                this.hfIndex.Value = "1";
            }
            return builder.ToString();
        }

        private GamePrizeInfo GetModel(PrizeGrade prizeGrade)
        {
            GamePrizeInfo info = new GamePrizeInfo {
                PrizeGrade = prizeGrade
            };
            int num = (int) prizeGrade;
            PrizeType type = PrizeType.赠送积分;
            try
            {
                type = (PrizeType) Enum.Parse(typeof(PrizeType), base.Request[string.Format("prizeType_{0}", num)]);
            }
            catch (Exception)
            {
            }
            info.PrizeType = type;
            switch (type)
            {
                case PrizeType.赠送积分:
                    try
                    {
                        info.GivePoint = int.Parse(base.Request[string.Format("txtGivePoint{0}", num)]);
                        goto Label_018B;
                    }
                    catch (Exception)
                    {
                        throw new Exception(string.Format("{0}的赠送积分格式不对!", prizeGrade.ToString()));
                    }
                    break;

                case PrizeType.赠送优惠券:
                    break;

                case PrizeType.赠送商品:
                    info.GiveShopBookId = base.Request[string.Format("txtShopbookId{0}", num)];
                    info.GriveShopBookPicUrl = base.Request[string.Format("txtProductPic{0}", num)];
                    goto Label_018B;

                case PrizeType.其他奖品:
                    try
                    {
                        info.IsLogistics = (base.Request[string.Format("ckbNeed_{0}", num)] == "on") ? 1 : 0;
                    }
                    catch (Exception)
                    {
                        throw new Exception(string.Format("{0}的是否配送格式不对!", prizeGrade.ToString()));
                    }
                    info.PrizeImage = base.Request[string.Format("hiddPrizeImage{0}", num)];
                    goto Label_018B;

                default:
                    goto Label_018B;
            }
            info.GiveCouponId = base.Request[string.Format("seletCouponId{0}", num)];
        Label_018B:
            try
            {
                info.Prize = base.Request[string.Format("txtPrize{0}", num)];
            }
            catch (Exception)
            {
                throw new Exception(string.Format("{0}的奖品名称格式不对!", prizeGrade.ToString()));
            }
            try
            {
                info.PrizeCount = int.Parse(base.Request[string.Format("txtPrizeCount{0}", num)]);
            }
            catch (Exception)
            {
                throw new Exception(string.Format("{0}的奖品数量格式不对!", prizeGrade.ToString()));
            }
            try
            {
                info.PrizeId = int.Parse(base.Request[string.Format("prizeInfoId{0}", num)]);
            }
            catch (Exception)
            {
                info.PrizeId = 0;
            }
            try
            {
                info.GameId = int.Parse(base.Request[string.Format("prizeGameId{0}", num)]);
            }
            catch (Exception)
            {
                info.GameId = 0;
            }
            return info;
        }

        protected string GetPrizeInfoHtml(PrizeGrade prizeGrade, GamePrizeInfo model)
        {
            StringBuilder builder = new StringBuilder();
            int prizeId = 0;
            if (model != null)
            {
                prizeId = model.PrizeId;
            }
            builder.Append("<div class='tabContent' id='div" + prizeId + "'>");
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
                foreach (ListItem item in this.CouponIdList)
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
                foreach (ListItem item2 in this.CouponIdList)
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                int result = 0;
                if (int.TryParse(base.Request.QueryString["gameId"], out result))
                {
                    this.hfGameId.Value = result.ToString();
                    this.hfIndex.Value = (this.PrizeLists.Count<GamePrizeInfo>() == 0) ? "1" : this.PrizeLists.Count<GamePrizeInfo>().ToString();
                    GameInfo gameInfoById = GameHelper.GetGameInfoById(result);
                    if (gameInfoById != null)
                    {
                        this.txtPrizeRate.Text = gameInfoById.PrizeRate.ToString("f2").Replace(".00", "");
                    }
                }
                this.BindDdlCouponId();
            }
        }

        protected string PrizeInfoHtml()
        {
            StringBuilder builder = new StringBuilder();
            if (this.PrizeLists != null)
            {
                switch (this.PrizeLists.Count<GamePrizeInfo>())
                {
                    case 0:
                        builder.Append(this.GetPrizeInfoHtml(PrizeGrade.一等奖, null));
                        this.hfIndex.Value = "1";
                        break;

                    case 1:
                        builder.Append(this.GetPrizeInfoHtml(PrizeGrade.一等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.一等奖)));
                        break;

                    case 2:
                        builder.Append(this.GetPrizeInfoHtml(PrizeGrade.一等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.一等奖)));
                        builder.Append(this.GetPrizeInfoHtml(PrizeGrade.二等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.二等奖)));
                        break;

                    case 3:
                        builder.Append(this.GetPrizeInfoHtml(PrizeGrade.一等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.一等奖)));
                        builder.Append(this.GetPrizeInfoHtml(PrizeGrade.二等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.二等奖)));
                        builder.Append(this.GetPrizeInfoHtml(PrizeGrade.三等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.三等奖)));
                        break;

                    case 4:
                        builder.Append(this.GetPrizeInfoHtml(PrizeGrade.一等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.一等奖)));
                        builder.Append(this.GetPrizeInfoHtml(PrizeGrade.二等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.二等奖)));
                        builder.Append(this.GetPrizeInfoHtml(PrizeGrade.三等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.三等奖)));
                        builder.Append(this.GetPrizeInfoHtml(PrizeGrade.四等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.四等奖)));
                        break;
                }
            }
            else
            {
                builder.Append(this.GetPrizeInfoHtml(PrizeGrade.一等奖, null));
            }
            return builder.ToString();
        }

        protected IList<ListItem> CouponIdList
        {
            get
            {
                return this._couponList;
            }
        }

        public string NotPrzeDescription
        {
            get
            {
                return this.txtNotPrzeDescription.Text.Trim();
            }
            set
            {
                this.txtNotPrzeDescription.Text = value;
            }
        }

        public IList<GamePrizeInfo> PrizeLists
        {
            get
            {
                this.GetDate();
                return this._prizeLists;
            }
            set
            {
                this._prizeLists = value;
            }
        }

        public float PrizeRate
        {
            get
            {
                return float.Parse(this.txtPrizeRate.Text);
            }
            set
            {
                this.txtPrizeRate.Text = value.ToString("f2");
            }
        }
    }
}

