namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class setScore_sign : AdminPage
    {
        protected static bool _continuityEnable = false;
        protected static bool _signEnable = false;
        protected static string _urlType = "sign";
        protected Button btn_signSave;
        protected HiddenField hdContinuityEnable;
        protected HtmlForm thisForm;
        protected TextBox txt_sign_RewardScore;
        protected TextBox txtEverDayScore;
        protected TextBox txtStraightDay;

        protected setScore_sign() : base("m04", "hyp07")
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

        protected void btn_signSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            int num2 = 0;
            int num3 = 0;
            string text = this.txtEverDayScore.Text;
            string str2 = this.txtStraightDay.Text;
            string str3 = this.txt_sign_RewardScore.Text;
            _urlType = "sign";
            if (!string.IsNullOrEmpty(text))
            {
                if (!this.bInt(text, ref i))
                {
                    this.ShowMsg("请输入正确的每日签到积分！", false);
                }
            }
            else
            {
                i = 0;
            }
            if (!string.IsNullOrEmpty(str2))
            {
                if (!this.bInt(str2, ref num2))
                {
                    this.ShowMsg("请输入正确的连续签到天数！", false);
                }
            }
            else
            {
                num2 = 0;
            }
            if (!string.IsNullOrEmpty(str3))
            {
                if (!this.bInt(str3, ref num3))
                {
                    this.ShowMsg("请输入正确的连续签到奖励积分！", false);
                }
            }
            else
            {
                num3 = 0;
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.SignPoint = i;
            masterSettings.SignWhere = num2;
            masterSettings.SignWherePoint = num3;
            masterSettings.open_signContinuity = this.hdContinuityEnable.Value == "1";
            SettingsManager.Save(masterSettings);
            _continuityEnable = this.hdContinuityEnable.Value == "1";
            this.ShowMsg("保存成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btn_signSave.Click += new EventHandler(this.btn_signSave_Click);
            if (base.Request.Params.AllKeys.Contains<string>("type"))
            {
                _urlType = base.Request["type"].ToString();
            }
            else
            {
                _urlType = "sign";
            }
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.txtEverDayScore.Text = masterSettings.SignPoint.ToString();
                this.txtStraightDay.Text = masterSettings.SignWhere.ToString();
                this.txt_sign_RewardScore.Text = masterSettings.SignWherePoint.ToString();
                _signEnable = masterSettings.sign_score_Enable;
                _continuityEnable = masterSettings.open_signContinuity;
            }
        }
    }
}

