namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Web.UI.WebControls;

    public class DistributorUpdateSet : AdminPage
    {
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected CheckBox cbIsAddCommission;
        protected Repeater rptList;
        protected Script Script4;
        private SiteSettings siteSettings;
        protected Hidistro.UI.Common.Controls.Style Style1;

        protected DistributorUpdateSet() : base("m05", "fxp12")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Globals.RequestFormStr("posttype") == "save")
            {
                int num = Globals.RequestFormNum("isadd");
                base.Response.ContentType = "application/json";
                string s = "{\"type\":\"0\",\"tips\":\"修改失败，请输入正确的参数！\"}";
                if (num == 1)
                {
                    string str3 = Globals.RequestFormStr("starttime");
                    string str4 = Globals.RequestFormStr("endtime");
                    JArray array = (JArray) JsonConvert.DeserializeObject(Globals.RequestFormStr("data"));
                    try
                    {
                        foreach (JObject obj2 in array)
                        {
                            int gradeid = Globals.ToNum(obj2["gradeid"].ToString());
                            decimal addcommission = decimal.Parse(obj2["addcommission"].ToString());
                            if ((gradeid > 0) && (addcommission >= 0M))
                            {
                                DistributorGradeBrower.SetAddCommission(gradeid, addcommission);
                            }
                        }
                        this.siteSettings.IsAddCommission = 1;
                        this.siteSettings.AddCommissionStartTime = str3;
                        this.siteSettings.AddCommissionEndTime = str4;
                        Globals.EntityCoding(this.siteSettings, true);
                        SettingsManager.Save(this.siteSettings);
                        s = "{\"type\":\"1\",\"tips\":\"修改成功！\"}";
                    }
                    catch
                    {
                    }
                    base.Response.Write(s);
                    base.Response.End();
                }
                else
                {
                    if (DistributorGradeBrower.ClearAddCommission())
                    {
                        this.siteSettings.IsAddCommission = 0;
                        this.siteSettings.AddCommissionStartTime = null;
                        this.siteSettings.AddCommissionEndTime = null;
                        Globals.EntityCoding(this.siteSettings, true);
                        SettingsManager.Save(this.siteSettings);
                        s = "{\"type\":\"1\",\"tips\":\"成功关闭分销商升级奖励！\"}";
                    }
                    base.Response.Write(s);
                    base.Response.End();
                }
            }
            else
            {
                bool flag = this.siteSettings.IsAddCommission == 1;
                this.cbIsAddCommission.Checked = flag;
                if (flag)
                {
                    this.calendarStartDate.Text = this.siteSettings.AddCommissionStartTime;
                    this.calendarEndDate.Text = this.siteSettings.AddCommissionEndTime;
                }
                else
                {
                    DateTime now = DateTime.Now;
                    this.calendarStartDate.Text = now.ToString("yyyy-MM-dd");
                    this.calendarEndDate.Text = now.AddMonths(2).ToString("yyyy-MM-dd");
                }
                this.rptList.DataSource = DistributorGradeBrower.GetAllDistributorGrade();
                this.rptList.DataBind();
            }
        }
    }
}

