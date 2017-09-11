namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VVote : VMemberTemplatedWebControl
    {
        private HtmlGenericControl divVoteOk;
        private HtmlInputHidden hidCheckNum;
        private Literal litVoteName;
        private Literal litVoteNum;
        private VshopTemplatedRepeater rptVoteItems;
        private int voteId;
        private int voteNum;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["voteId"], out this.voteId))
            {
                base.GotoResourceNotFound("");
            }
            this.litVoteName = (Literal) this.FindControl("litVoteName");
            this.litVoteNum = (Literal) this.FindControl("litVoteNum");
            this.rptVoteItems = (VshopTemplatedRepeater) this.FindControl("rptVoteItems");
            this.hidCheckNum = (HtmlInputHidden) this.FindControl("hidCheckNum");
            this.divVoteOk = (HtmlGenericControl) this.FindControl("divVoteOk");
            string voteName = string.Empty;
            int checkNum = 1;
            DataTable table = VshopBrowser.GetVote(this.voteId, out voteName, out checkNum, out this.voteNum);
            if (table == null)
            {
                base.GotoResourceNotFound("");
            }
            this.LoadVoteItemTable(table);
            this.rptVoteItems.DataSource = table;
            this.rptVoteItems.DataBind();
            this.litVoteName.Text = voteName;
            this.hidCheckNum.Value = checkNum.ToString();
            this.litVoteNum.Text = string.Format("共有{0}人参与投票", this.voteNum);
            if (VshopBrowser.IsVote(this.voteId))
            {
                this.litVoteNum.Text = this.litVoteNum.Text + "(您已投票)";
                this.divVoteOk.Visible = false;
            }
            PageTitle.AddSiteNameTitle("投票调查");
        }

        private string GetVoteItemCountString(int num)
        {
            string str = string.Empty;
            if (this.voteNum != 0)
            {
                int num2 = (num * 30) / this.voteNum;
                for (int i = 0; i < num2; i++)
                {
                    str = str + "&nbsp;";
                }
            }
            return str;
        }

        private void LoadVoteItemTable(DataTable table)
        {
            table.Columns.Add("Lenth");
            table.Columns.Add("Percentage");
            foreach (DataRow row in table.Rows)
            {
                row["Lenth"] = this.GetVoteItemCountString((int) row["ItemCount"]);
                if (this.voteNum != 0)
                {
                    row["Percentage"] = ((decimal.Parse(row["ItemCount"].ToString()) * 100M) / decimal.Parse(this.voteNum.ToString())).ToString("F2");
                }
                else
                {
                    row["Percentage"] = 0.0;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VVote.html";
            }
            base.OnInit(e);
        }
    }
}

