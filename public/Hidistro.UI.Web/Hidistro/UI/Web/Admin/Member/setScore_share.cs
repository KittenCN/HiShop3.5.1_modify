namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class setScore_share : AdminPage
    {
        protected static bool _shareEnable = false;
        protected static string _urlType = "share";
        protected Button btn_shareSave;
        protected HtmlForm thisForm;
        protected TextBox txt_ShareScore;

        protected setScore_share() : base("m04", "hyp08")
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

        protected void btn_shareSave_Click(object sender, EventArgs e)
        {
            _urlType = "share";
            int i = 0;
            string text = this.txt_ShareScore.Text;
            if (!string.IsNullOrEmpty(text))
            {
                if (!this.bInt(text, ref i))
                {
                    this.ShowMsg("请输入正确的分享积分！", false);
                }
            }
            else
            {
                i = 0;
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.share_Score = i;
            SettingsManager.Save(masterSettings);
            this.ShowMsg("保存成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btn_shareSave.Click += new EventHandler(this.btn_shareSave_Click);
            if (base.Request.Params.AllKeys.Contains<string>("type"))
            {
                _urlType = base.Request["type"].ToString();
            }
            else
            {
                _urlType = "share";
            }
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.txt_ShareScore.Text = masterSettings.share_Score.ToString();
                _shareEnable = masterSettings.share_score_Enable;
            }
        }
    }
}

