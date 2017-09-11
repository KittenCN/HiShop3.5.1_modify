namespace Hidistro.UI.Web.Vshop
{
    using  global:: ControlPanel.WeiBo;
    using Hidistro.Entities.Weibo;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ArticleShow : Page
    {
        protected int articleID;
        protected string ArticleType = string.Empty;
        protected string htmlImageUrl = string.Empty;
        protected string htmlMemo = string.Empty;
        protected string htmlPubTime = string.Empty;
        protected string htmlTitle = string.Empty;
        protected string htmlUrl = string.Empty;
        protected Repeater rptList;

        private void GoBack()
        {
            base.Response.Write("<script>history.go(-1)</script>");
            base.Response.End();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string s = base.Request.QueryString["ID"];
            int.TryParse(s, out this.articleID);
            if (this.articleID > 0)
            {
                ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(this.articleID);
                if (articleInfo == null)
                {
                    this.GoBack();
                }
                else
                {
                    this.htmlTitle = articleInfo.Title;
                    this.ArticleType = articleInfo.ArticleType.ToString().ToLower();
                    this.htmlImageUrl = articleInfo.ImageUrl;
                    this.htmlUrl = articleInfo.Url;
                    this.htmlPubTime = articleInfo.PubTime.ToString("yyyy-MM-dd HH:mm");
                    this.htmlMemo = articleInfo.Memo;
                    if (this.ArticleType == "list")
                    {
                        this.rptList.DataSource = articleInfo.ItemsInfo;
                        this.rptList.DataBind();
                    }
                }
            }
            else
            {
                this.GoBack();
            }
        }
    }
}

