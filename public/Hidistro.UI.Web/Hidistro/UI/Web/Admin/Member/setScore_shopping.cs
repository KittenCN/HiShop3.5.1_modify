namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class setScore_shopping : AdminPage
    {
        protected static bool _shoppingEnable = false;
        protected static string _urlType = "shopping";
        protected Button btn_shoppingSave;
        protected CheckBox chk;
        protected HtmlForm thisForm;
        protected TextBox txt_OrderValue;
        protected TextBox txt_shopping_RewardScore;
        protected TextBox txt_ShoppingScore;
        protected TextBox txt_ShoppingScoreUnit;

        protected setScore_shopping() : base("m04", "hyp06")
        {
        }

        private bool bDouble(string val, ref double i)
        {
            i = 0.0;
            if (val.Contains("-"))
            {
                return false;
            }
            return double.TryParse(val, out i);
        }

        private bool bInt(string val, ref int i)
        {
            i = 0;
            if (val.Contains("-"))
            {
                return false;
            }
            return int.TryParse(val, out i);
        }

        protected void btn_shoppingSave_Click(object sender, EventArgs e)
        {
            _urlType = "shopping";
            int i = 0;
            bool flag = false;
            double num2 = 0.0;
            int num3 = 0;
            int num4 = 0;
            flag = this.chk.Checked;
            string text = this.txt_ShoppingScore.Text;
            string str2 = this.txt_OrderValue.Text;
            string str3 = this.txt_shopping_RewardScore.Text;
            string str4 = this.txt_ShoppingScoreUnit.Text;
            if (!string.IsNullOrEmpty(text))
            {
                if (!this.bInt(text, ref i))
                {
                    this.ShowMsg("请输入正确的购物积分！", false);
                }
            }
            else
            {
                i = 0;
            }
            if (i <= 0)
            {
                this.ShowMsg("请输入大于0的购物金额基数！", false);
            }
            else
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    if (!this.bDouble(str2, ref num2))
                    {
                        this.ShowMsg("请输入正确的单笔订单价值！", false);
                        return;
                    }
                }
                else
                {
                    num2 = 0.0;
                }
                if (!string.IsNullOrEmpty(str3) && !this.bInt(str3, ref num3))
                {
                    this.ShowMsg("奖励积分为大于0的整数！", false);
                }
                else if (num3 <= 0)
                {
                    this.ShowMsg("奖励积分为大于0的整数！", false);
                }
                else if (!string.IsNullOrEmpty(str4) && !this.bInt(str4, ref num4))
                {
                    this.ShowMsg("请输入正确的积分基数！", false);
                }
                else if (num4 <= 0)
                {
                    this.ShowMsg("请输入大于0的积分基数！", false);
                }
                else
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    masterSettings.PointsRate = i;
                    masterSettings.shopping_reward_Enable = flag;
                    masterSettings.shopping_reward_OrderValue = num2;
                    masterSettings.shopping_reward_Score = num3;
                    masterSettings.ShoppingScoreUnit = num4;
                    SettingsManager.Save(masterSettings);
                    this.ShowMsgAndReUrl("保存成功", true, "/Admin/member/setScore_shopping.aspx");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btn_shoppingSave.Click += new EventHandler(this.btn_shoppingSave_Click);
            if (base.Request.Params.AllKeys.Contains<string>("type"))
            {
                _urlType = base.Request["type"].ToString();
            }
            else
            {
                _urlType = "shopping";
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            _shoppingEnable = masterSettings.shopping_score_Enable;
            if (!base.IsPostBack)
            {
                this.txt_ShoppingScore.Text = masterSettings.PointsRate.ToString("f0");
                this.chk.Checked = masterSettings.shopping_reward_Enable;
                this.txt_OrderValue.Text = masterSettings.shopping_reward_OrderValue.ToString("F2");
                this.txt_shopping_RewardScore.Text = masterSettings.shopping_reward_Score.ToString();
                this.txt_ShoppingScoreUnit.Text = masterSettings.ShoppingScoreUnit.ToString();
            }
        }
    }
}

