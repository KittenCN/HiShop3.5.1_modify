namespace Hidistro.UI.Web.Admin.promotion
{
    using global::ControlPanel.Promotions;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddGameAct : AdminPage
    {
        protected static int _gameId = 0;
        protected eGameType _gameType;
        protected string _grade;
        protected string _json;
        protected IList<GameActPrizeInfo> _lst;
        protected static int _step = 1;
        protected Button back_Step2;
        protected Button back_Step3;
        protected Button Button1;
        protected Button Button2;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected HtmlInputHidden htxtRoleId;
        protected CheckBox onlyChk;
        protected RadioButton rd1;
        protected RadioButton rd2;
        protected RadioButton rd3;
        protected RadioButton rd4;
        protected Button save_Step2;
        protected Button save_Step3;
        protected Button saveBtn;
        protected HtmlForm thisForm;
        protected TextBox txt_decrip;
        protected TextBox txt_gPoint;
        protected TextBox txt_grades;
        protected TextBox txt_json;
        protected TextBox txt_name;
        protected TextBox txt_uPoint;

        protected AddGameAct() : base("m08", "yxp07")
        {
            this._grade = "";
            this._json = "";
        }

        private int AddGame()
        {
            string str = this.txt_name.Text.Trim();
            string str2 = this.txt_decrip.Text.Trim();
            DateTime date = this.calendarStartDate.SelectedDate.Value.Date;
            DateTime time2 = this.calendarEndDate.SelectedDate.Value.Date.AddDays(1.0).AddSeconds(-1.0);
            if (string.IsNullOrEmpty(str) || (str.Length > 30))
            {
                this.ShowMsg("请输入正确的游戏名，不超过30个字符！", false);
                return 0;
            }
            if (str2.Length > 600)
            {
                this.ShowMsg("游戏说明不能超过600个字符！", false);
                return 0;
            }
            if (time2 < date)
            {
                this.ShowMsg("结束时间不能早于开始时间!", false);
                return 0;
            }
            GameActInfo game = new GameActInfo();
            if (_gameId != 0)
            {
                game = GameActHelper.Get(_gameId);
            }
            game.GameName = str;
            game.Decription = str2;
            game.BeginDate = date;
            game.EndDate = time2;
            game.CreateStep = 2;
            string msg = "";
            if (_gameId != 0)
            {
                GameActHelper.Update(game, ref msg);
                return game.GameId;
            }
            return GameActHelper.Create(game, ref msg);
        }

        protected void back_Step2_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("AddGameAct.aspx?id=" + _gameId + "&step=1");
        }

        protected void back_Step3_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("AddGameAct.aspx?id=" + _gameId + "&step=2");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
            this.save_Step2.Click += new EventHandler(this.save_Step2_Click);
            this.back_Step2.Click += new EventHandler(this.back_Step2_Click);
            this.save_Step3.Click += new EventHandler(this.save_Step3_Click);
            this.back_Step3.Click += new EventHandler(this.back_Step3_Click);
            string[] allKeys = base.Request.Params.AllKeys;
            this.txt_decrip.MaxLength = 600;
            if (allKeys.Contains<string>("id") && base.Request["id"].ToString().bInt(ref _gameId))
            {
                GameActInfo info = GameActHelper.Get(_gameId);
                if (info == null)
                {
                    this.ShowMsg("没有这个游戏~", false);
                    return;
                }
                _step = info.CreateStep;
                this._gameType = info.GameType;
                this.txt_name.Text = info.GameName;
                this.calendarStartDate.SelectedDate = new DateTime?(info.BeginDate);
                this.calendarEndDate.SelectedDate = new DateTime?(info.EndDate);
                this.txt_decrip.Text = info.Decription;
                this.txt_gPoint.Text = info.GivePoint.ToString();
                this.txt_uPoint.Text = info.usePoint.ToString();
                this._grade = info.MemberGrades;
                this.onlyChk.Checked = info.bOnlyNotWinner;
                if (info.attendTimes == 0)
                {
                    this.rd1.Checked = true;
                }
                else if (info.attendTimes == 1)
                {
                    this.rd2.Checked = true;
                }
                else if (info.attendTimes == 2)
                {
                    this.rd3.Checked = true;
                }
                else if (info.attendTimes == 3)
                {
                    this.rd4.Checked = true;
                }
            }
            if (allKeys.Contains<string>("step"))
            {
                int i = 0;
                if (base.Request["step"].ToString().bInt(ref i))
                {
                    _step = i;
                }
            }
            if (_step > 4)
            {
                _step = 4;
            }
            if (_step == 3)
            {
                IList<GameActPrizeInfo> prizesModel = GameActHelper.GetPrizesModel(_gameId);
                if ((prizesModel != null) && (prizesModel.Count > 0))
                {
                    foreach (GameActPrizeInfo info2 in prizesModel)
                    {
                        info2.PointRate = Math.Round(info2.PointRate);
                        info2.CouponRate = Math.Round(info2.CouponRate);
                        info2.ProductRate = Math.Round(info2.ProductRate);
                    }
                    this._json = JsonConvert.SerializeObject(prizesModel);
                }
            }
        }

        protected void save_Step2_Click(object sender, EventArgs e)
        {
            this.SaveGameStep2();
        }

        protected void save_Step3_Click(object sender, EventArgs e)
        {
            try
            {
                string str = this.txt_json.Text.Trim();
                if (str.Length <= 0)
                {
                    this.ShowMsg("请设定奖品信息！", false);
                }
                else
                {
                    List<GameActPrizeInfo> list = new List<GameActPrizeInfo>();
                    JArray array = (JArray) JsonConvert.DeserializeObject(str);
                    decimal num = 0M;
                    if (array.Count > 0)
                    {
                        for (int i = 0; i < array.Count; i++)
                        {
                            GameActPrizeInfo item = new GameActPrizeInfo();
                            int prizeId = int.Parse(array[i]["prizeId"].ToString());
                            if (prizeId != 0)
                            {
                                item = GameActHelper.GetPrize(_gameId, prizeId);
                            }
                            else
                            {
                                item.Id = 0;
                            }
                            item.PrizeName = array[i]["prizeName"].ToString();
                            item.PrizeType = (ePrizeType) int.Parse(array[i]["prizeType"].ToString());
                            item.GrivePoint = int.Parse(array[i]["point"].ToString());
                            item.PointNumber = int.Parse(array[i]["pointNumber"].ToString());
                            item.PointRate = int.Parse(array[i]["pointRate"].ToString());
                            item.GiveCouponId = int.Parse(array[i]["coupon"].ToString());
                            item.CouponNumber = int.Parse(array[i]["couponNumber"].ToString());
                            item.CouponRate = int.Parse(array[i]["couponRate"].ToString());
                            item.GiveProductId = int.Parse(array[i]["product"].ToString());
                            item.ProductNumber = int.Parse(array[i]["productNumber"].ToString());
                            item.ProductRate = int.Parse(array[i]["productRate"].ToString());
                            item.sort = i + 1;
                            item.GameId = _gameId;
                            num += (item.PointRate + item.CouponRate) + item.ProductRate;
                            list.Add(item);
                        }
                    }
                    if (num > 100M)
                    {
                        this.ShowMsg("中奖率总和不能大于100！", false);
                    }
                    else
                    {
                        foreach (GameActPrizeInfo info2 in list)
                        {
                            if (info2.Id != 0)
                            {
                                GameActHelper.UpdatePrize(info2);
                            }
                            else
                            {
                                GameActHelper.InsertPrize(info2);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                this.ShowMsg("保存奖品失败！", false);
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            _gameId = this.AddGame();
            base.Response.Redirect("AddGameAct.aspx?id=" + _gameId + "&step=2");
        }

        private void SaveGameStep2()
        {
            string str = this.txt_grades.Text.Trim();
            string val = this.txt_uPoint.Text.Trim();
            string str3 = this.txt_gPoint.Text.Trim();
            int i = 0;
            int num2 = 0;
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请选择适用会员等级！", false);
            }
            else if (!val.bInt(ref i))
            {
                this.ShowMsg("请输入正确的消耗积分！", false);
            }
            else
            {
                if (string.IsNullOrEmpty(str3))
                {
                    str3 = "0";
                }
                if (!str3.bInt(ref num2))
                {
                    this.ShowMsg("请输入正确的赠送积分！", false);
                }
                else
                {
                    GameActInfo game = GameActHelper.Get(_gameId);
                    game.MemberGrades = str;
                    game.usePoint = i;
                    game.GivePoint = num2;
                    game.bOnlyNotWinner = this.onlyChk.Checked;
                    if (this.rd1.Checked)
                    {
                        game.attendTimes = 0;
                    }
                    else if (this.rd2.Checked)
                    {
                        game.attendTimes = 1;
                    }
                    else if (this.rd3.Checked)
                    {
                        game.attendTimes = 2;
                    }
                    else if (this.rd4.Checked)
                    {
                        game.attendTimes = 3;
                    }
                    game.CreateStep = 3;
                    string msg = "";
                    GameActHelper.Update(game, ref msg);
                    base.Response.Redirect("AddGameAct.aspx?id=" + _gameId + "&step=3");
                }
            }
        }
    }
}

