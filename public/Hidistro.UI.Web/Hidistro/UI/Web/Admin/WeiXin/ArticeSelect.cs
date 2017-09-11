namespace Hidistro.UI.Web.Admin.WeiXin
{
    using ASPNET.WebControls;
    using  global:: ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Weibo;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ArticeSelect : AdminPage
    {
        protected string ArticleTitle;
        protected int articletype;
        protected Button btnSearch;
        protected HtmlForm form1;
        protected string htmlMenuTitleAdd;
        private int pageno;
        protected Pager pager;
        protected int recordcount;
        protected Repeater rptList;
        private string title;
        protected TextBox txtSearchText;

        protected ArticeSelect() : base("m06", "00000")
        {
            this.htmlMenuTitleAdd = string.Empty;
            this.ArticleTitle = string.Empty;
            this.title = string.Empty;
        }

        private void BindData(int articletype, int pageno, string title)
        {
            ArticleQuery entity = new ArticleQuery {
                Title = title,
                ArticleType = articletype,
                SortBy = "PubTime",
                SortOrder = SortAction.Desc
            };
            Globals.EntityCoding(entity, true);
            entity.PageIndex = pageno;
            entity.PageSize = this.pager.PageSize;
            DbQueryResult articleRequest = ArticleHelper.GetArticleRequest(entity);
            this.rptList.DataSource = articleRequest.Data;
            this.rptList.DataBind();
            int totalRecords = articleRequest.TotalRecords;
            this.pager.TotalRecords = totalRecords;
            this.recordcount = totalRecords;
            if (this.pager.TotalRecords <= this.pager.PageSize)
            {
                this.pager.Visible = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string url = "ArticeSelect.aspx";
            string str2 = string.Empty;
            if (this.articletype > 0)
            {
                str2 = str2 + "&type=" + this.articletype;
            }
            string str3 = this.txtSearchText.Text.Trim();
            if (!string.IsNullOrEmpty(str3))
            {
                str2 = str2 + "&key=" + base.Server.UrlEncode(str3);
            }
            if (!string.IsNullOrEmpty(str2))
            {
                url = url + "?" + str2.Trim(new char[] { '&' });
            }
            base.Response.Redirect(url);
            base.Response.End();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.articletype = Globals.RequestQueryNum("type");
            switch (this.articletype)
            {
                case 2:
                    this.htmlMenuTitleAdd = "单";
                    break;

                case 4:
                    this.htmlMenuTitleAdd = "多";
                    break;

                default:
                    this.articletype = 0;
                    break;
            }
            if (!base.IsPostBack)
            {
                this.pageno = Globals.RequestQueryNum("pageindex");
                if (this.pageno < 1)
                {
                    this.pageno = 1;
                }
                string str = Globals.RequestQueryStr("key");
                if (!string.IsNullOrEmpty(str))
                {
                    this.ArticleTitle = str;
                    this.txtSearchText.Text = str;
                }
                this.articletype = Globals.RequestQueryNum("type");
                switch (this.articletype)
                {
                    case 2:
                    case 4:
                        break;

                    default:
                        this.articletype = 0;
                        break;
                }
                this.BindData(this.articletype, this.pageno, this.ArticleTitle);
            }
        }
    }
}

