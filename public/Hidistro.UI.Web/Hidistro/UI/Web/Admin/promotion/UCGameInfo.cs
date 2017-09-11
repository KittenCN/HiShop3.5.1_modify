namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Web.Admin;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class UCGameInfo : UserControl
    {
        private Hidistro.Entities.Promotions.GameInfo _gameInfo;
        private IList<MemberGradeItem> _memberGrades = new List<MemberGradeItem>();
        protected CheckBox cbOnlyGiveNotPrizeMember;
        protected ucDateTimePicker dateBeginTime;
        protected ucDateTimePicker dateEndTime;
        protected HiddenField hfGameId;
        protected HiddenField hfKeyWord;
        protected bool isAllCheck;
        protected bool memberCheck;
        protected SetMemberRange memberRange;
        protected TextBox txtDescription;
        protected TextBox txtGameTitle;
        protected TextBox txtGameUrl;
        protected TextBox txtGivePoint;
        protected TextBox txtLimitEveryDay;
        protected TextBox txtMaximumDailyLimit;
        protected TextBox txtNeedPoint;

        private void BindDate()
        {
            if (this._gameInfo == null)
            {
                string keyWord = Guid.NewGuid().ToString().Replace("-", "");
                this.hfKeyWord.Value = keyWord;
                string str2 = this.CreateGameUrl(keyWord);
                this.txtGameUrl.Text = str2;
            }
            else
            {
                this.memberCheck = this._gameInfo.MemberCheck == 1;
                this.hfGameId.Value = this._gameInfo.GameId.ToString();
                this.txtGameTitle.Text = this._gameInfo.GameTitle;
                this.dateBeginTime.SelectedDate = new DateTime?(this._gameInfo.BeginTime);
                this.dateEndTime.SelectedDate = new DateTime?(this._gameInfo.EndTime);
                this.txtDescription.Text = this._gameInfo.Description.Replace("<br/>", "\n");
                this.txtNeedPoint.Text = this._gameInfo.NeedPoint.ToString();
                this.txtGivePoint.Text = this._gameInfo.GivePoint.ToString();
                this.cbOnlyGiveNotPrizeMember.Checked = this._gameInfo.OnlyGiveNotPrizeMember;
                this.txtLimitEveryDay.Text = this._gameInfo.LimitEveryDay.ToString();
                this.txtMaximumDailyLimit.Text = this._gameInfo.MaximumDailyLimit.ToString();
                HiddenField field = this.memberRange.FindControl("txt_Grades") as HiddenField;
                HiddenField field2 = this.memberRange.FindControl("txt_DefualtGroup") as HiddenField;
                HiddenField field3 = this.memberRange.FindControl("txt_CustomGroup") as HiddenField;
                this.memberRange.Grade = this._gameInfo.ApplyMembers;
                this.memberRange.CustomGroup = this._gameInfo.CustomGroup;
                this.memberRange.DefualtGroup = this._gameInfo.DefualtGroup;
                field.Value = this.memberRange.Grade;
                field2.Value = this.memberRange.DefualtGroup;
                field3.Value = this.memberRange.CustomGroup;
                switch (this._gameInfo.PlayType)
                {
                }
                if ((string.Equals(this._gameInfo.ApplyMembers, "0") && string.Equals(this._gameInfo.DefualtGroup, "0")) && string.Equals(this._gameInfo.CustomGroup, "0"))
                {
                    this.isAllCheck = true;
                }
                this.txtGameUrl.Text = this._gameInfo.GameUrl;
            }
            foreach (MemberGradeInfo info in MemberHelper.GetMemberGrades())
            {
                MemberGradeItem item = new MemberGradeItem();
                if (!this.isAllCheck)
                {
                    item.IsCheck = this.IsCheck(info.GradeId.ToString());
                }
                else
                {
                    item.IsCheck = this.isAllCheck;
                }
                item.Name = info.Name;
                item.GradeId = info.GradeId.ToString();
                this._memberGrades.Add(item);
            }
        }

        private string CreateGameUrl(string keyWord)
        {
            Uri url = HttpContext.Current.Request.Url;
            string str = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(CultureInfo.InvariantCulture));
            string str2 = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[] { url.Scheme, Globals.DomainName, str });
            return string.Format("{0}{1}/Game.aspx?gamid={2}&type={3}", new object[] { str2, Globals.ApplicationPath, keyWord, (int) this.GameType });
        }

        private void GetDate()
        {
            if (this._gameInfo == null)
            {
                this._gameInfo = new Hidistro.Entities.Promotions.GameInfo();
            }
            try
            {
                this._gameInfo.GameId = int.Parse(this.hfGameId.Value);
            }
            catch (Exception)
            {
                this._gameInfo.GameId = 0;
                this._gameInfo.GameType = this.GameType;
            }
            this._gameInfo.GameTitle = this.txtGameTitle.Text;
            try
            {
                this._gameInfo.BeginTime = this.dateBeginTime.SelectedDate.Value;
            }
            catch (InvalidOperationException)
            {
                throw new Exception("活动时间期的开始日期不能为空！");
            }
            try
            {
                this._gameInfo.EndTime = this.dateEndTime.SelectedDate.Value;
            }
            catch (InvalidOperationException)
            {
                throw new Exception("活动时间的结束日期不能为空！");
            }
            try
            {
                this._gameInfo.NeedPoint = int.Parse(this.txtNeedPoint.Text);
            }
            catch (FormatException)
            {
                throw new Exception("活动消耗积分格式不对！");
            }
            try
            {
                this._gameInfo.GivePoint = int.Parse(this.txtGivePoint.Text);
            }
            catch (FormatException)
            {
                throw new Exception("活动参与送积分格式不对！");
            }
            try
            {
                this._gameInfo.LimitEveryDay = int.Parse(this.txtLimitEveryDay.Text);
            }
            catch (FormatException)
            {
                throw new Exception("每人最多限次格式不对！");
            }
            try
            {
                this._gameInfo.MaximumDailyLimit = int.Parse(this.txtMaximumDailyLimit.Text);
            }
            catch (FormatException)
            {
                throw new Exception("每人每天限次不对！");
            }
            this._gameInfo.Description = this.txtDescription.Text.Replace("\n", "<br/>");
            this._gameInfo.OnlyGiveNotPrizeMember = this.cbOnlyGiveNotPrizeMember.Checked;
            this._gameInfo.ApplyMembers = base.Request["allmember"];
            HiddenField field = this.memberRange.FindControl("txt_Grades") as HiddenField;
            HiddenField field2 = this.memberRange.FindControl("txt_DefualtGroup") as HiddenField;
            HiddenField field3 = this.memberRange.FindControl("txt_CustomGroup") as HiddenField;
            if (string.IsNullOrEmpty(this._gameInfo.ApplyMembers))
            {
                this._gameInfo.ApplyMembers = field.Value;
            }
            this._gameInfo.CustomGroup = field3.Value;
            this._gameInfo.DefualtGroup = field2.Value;
            this._gameInfo.MemberCheck = (base.Request["MemberCheck"] == "on") ? 1 : 0;
            this._gameInfo.GameUrl = this.txtGameUrl.Text.Trim();
            this._gameInfo.KeyWork = this.hfKeyWord.Value;
        }

        private bool IsCheck(string gradeId)
        {
            if (this._gameInfo != null)
            {
                string[] source = this._gameInfo.ApplyMembers.Split(new char[] { ',' });
                for (int i = 0; i < source.Count<string>(); i++)
                {
                    if (string.Equals(source[i], gradeId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindDate();
            }
        }

        public Hidistro.Entities.Promotions.GameInfo GameInfo
        {
            get
            {
                this.GetDate();
                return this._gameInfo;
            }
            set
            {
                this._gameInfo = value;
            }
        }

        public Hidistro.Entities.Promotions.GameType GameType { get; set; }

        protected IList<MemberGradeItem> MemberGrades
        {
            get
            {
                return this._memberGrades;
            }
        }
    }
}

