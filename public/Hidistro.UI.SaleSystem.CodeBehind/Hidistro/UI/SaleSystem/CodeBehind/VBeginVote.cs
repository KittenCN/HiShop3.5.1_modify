namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ControlPanel.Promotions;
    using global::ControlPanel.Promotions;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using System;

    public class VBeginVote : VMemberTemplatedWebControl
    {
        private string htmlTitle = string.Empty;
        private HiImage imgVoteItem;
        private HiLiteral lVoteCount;
        private HiLiteral lVoteDate;
        private HiLiteral lVoteDescription;
        private HiLiteral lVoteTitle;
        private HiLiteral lVoteType;
        private VshopTemplatedRepeater rptVoteItem;
        private long voteId = 1L;

        protected override void AttachChildControls()
        {
            VoteInfo vote = VoteHelper.GetVote(this.voteId);
            this.imgVoteItem = (HiImage) this.FindControl("imgVoteItem");
            this.lVoteTitle = (HiLiteral) this.FindControl("lVoteTitle");
            this.lVoteDescription = (HiLiteral) this.FindControl("lVoteDescription");
            this.lVoteCount = (HiLiteral) this.FindControl("lVoteCount");
            this.rptVoteItem = (VshopTemplatedRepeater) this.FindControl("rptVoteItem");
            this.lVoteDate = (HiLiteral) this.FindControl("lVoteDate");
            this.lVoteType = (HiLiteral) this.FindControl("lVoteType");
            if (vote != null)
            {
                this.htmlTitle = vote.VoteName;
                this.lVoteTitle.Text = vote.VoteName;
                this.imgVoteItem.ImageUrl = vote.ImageUrl;
                this.lVoteDescription.Text = vote.Description;
                this.lVoteCount.Text = vote.VoteAttends.ToString();
                this.lVoteDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss} 至 {1:yyyy-MM-dd  HH:mm:ss} ", vote.StartDate, vote.EndDate);
                if (vote.IsMultiCheck)
                {
                    this.lVoteType.Text = "多选";
                    this.rptVoteItem.TemplateFile = "/Tags/Skin-Common-BeginVoteItemCheckBox.ascx";
                }
                else
                {
                    this.lVoteType.Text = "单选";
                    this.rptVoteItem.TemplateFile = "/Tags/Skin-Common-BeginVoteItem.ascx";
                }
                this.rptVoteItem.DataSource = vote.VoteItems;
                this.rptVoteItem.DataBind();
            }
            else
            {
                base.GotoResourceNotFound("");
            }
            PageTitle.AddSiteNameTitle(this.htmlTitle);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VBeginVote.html";
            }
            try
            {
                this.voteId = long.Parse(this.Page.Request.QueryString["voteId"]);
            }
            catch (Exception)
            {
                base.GotoResourceNotFound("参数错误！");
            }
            base.OnInit(e);
        }
    }
}

