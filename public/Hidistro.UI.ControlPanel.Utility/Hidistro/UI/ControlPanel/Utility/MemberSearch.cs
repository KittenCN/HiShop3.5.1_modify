namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Entities.Members;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class MemberSearch : TemplatedWebControl
    {
        private IButton btnSearchButton;
        private MemberGradeDropDownList dropMemberRank;
        private PageSizeDropDownList dropPageSize;
        private TextBox txtSearchText;

        public event ReSearchEventHandler ReSearch;

        protected override void AttachChildControls()
        {
            this.txtSearchText = (TextBox) this.FindControl("txtSearchText");
            this.dropPageSize = (PageSizeDropDownList) this.FindControl("dropPageSize");
            this.btnSearchButton = ButtonManager.Create(this.FindControl("btnSearchButton"));
            this.dropMemberRank = (MemberGradeDropDownList) this.FindControl("rankList");
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            if (!this.Page.IsPostBack)
            {
                this.InitializeControls();
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.OnReSearch(sender, e);
        }

        protected void InitializeControls()
        {
            int num;
            MemberQuery query = new MemberQuery();
            if (this.dropPageSize != null)
            {
                this.dropPageSize.SelectedValue = query.PageSize;
            }
            if (this.dropMemberRank != null)
            {
                this.dropMemberRank.DataBind();
            }
            if ((this.Page.Request.QueryString["rankId"] != null) && int.TryParse(this.Page.Request.QueryString["rankId"], out num))
            {
                this.dropMemberRank.SelectedValue = new int?(num);
            }
        }

        public void OnReSearch(object sender, EventArgs e)
        {
            if (this.ReSearch != null)
            {
                this.ReSearch(sender, e);
            }
        }

        public MemberQuery Item
        {
            get
            {
                MemberQuery query = new MemberQuery();
                if (this.txtSearchText != null)
                {
                    query.Username = this.txtSearchText.Text.Trim();
                }
                if (this.dropPageSize != null)
                {
                    query.PageSize = this.dropPageSize.SelectedValue;
                }
                if (this.dropMemberRank != null)
                {
                    query.GradeId = new int?(this.dropMemberRank.SelectedValue.Value);
                }
                return query;
            }
        }

        public delegate void ReSearchEventHandler(object sender, EventArgs e);
    }
}

