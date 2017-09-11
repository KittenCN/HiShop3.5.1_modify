namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin;
    using Hidistro.UI.Web.Admin.Ascx;
    using Hidistro.UI.Web.hieditor.ueditor.controls;
    using System;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddVote : AdminPage
    {
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected ucUeditor fkContent;
        protected HiddenField hidpic;
        protected HiddenField hidpicdel;
        protected int id;
        protected string items;
        protected Script Script4;
        protected Hidistro.UI.Web.Admin.SetMemberRange SetMemberRange;
        protected HtmlForm thisForm;
        protected VoteInfo vote;

        protected AddVote() : base("m08", "yxp06")
        {
            this.vote = new VoteInfo();
            this.items = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.Params.AllKeys.Contains<string>("id") && base.Request["id"].ToString().bInt(ref this.id))
            {
                this.vote = VoteHelper.GetVote((long) this.id);
                if (this.vote == null)
                {
                    this.ShowMsg("没有这个投票调查！", false);
                }
                this.hidpic.Value = this.vote.ImageUrl;
                this.fkContent.Text = this.vote.Description;
                this.calendarStartDate.SelectedDate = new DateTime?(this.vote.StartDate.Date);
                this.calendarEndDate.SelectedDate = new DateTime?(this.vote.EndDate.Date);
                HiddenField field = this.SetMemberRange.FindControl("txt_Grades") as HiddenField;
                HiddenField field2 = this.SetMemberRange.FindControl("txt_DefualtGroup") as HiddenField;
                HiddenField field3 = this.SetMemberRange.FindControl("txt_CustomGroup") as HiddenField;
                this.SetMemberRange.Grade = this.vote.MemberGrades;
                this.SetMemberRange.DefualtGroup = this.vote.DefualtGroup;
                this.SetMemberRange.CustomGroup = this.vote.CustomGroup;
                field.Value = this.vote.MemberGrades;
                field2.Value = this.vote.DefualtGroup;
                field3.Value = this.vote.CustomGroup;
                if ((this.vote.VoteItems != null) && (this.vote.VoteItems.Count > 0))
                {
                    foreach (VoteItemInfo info in this.vote.VoteItems)
                    {
                        this.items = this.items + info.VoteItemName + ",";
                    }
                }
                this.items = this.items.TrimEnd(new char[] { ',' }).Replace(',', '\n');
            }
        }
    }
}

