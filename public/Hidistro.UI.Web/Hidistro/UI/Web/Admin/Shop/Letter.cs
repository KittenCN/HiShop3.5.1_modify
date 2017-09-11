namespace Hidistro.UI.Web.Admin.Shop
{
    using  global:: ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Entities.Weibo;
    using Hidistro.UI.ControlPanel.Utility;
    using System;

    public class Letter : AdminPage
    {
        protected string htmlJs;
        protected int LocalArticleID;

        protected Letter() : base("m07", "wbp06")
        {
            this.htmlJs = string.Empty;
            this.LocalArticleID = Globals.RequestQueryNum("aid");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack && (this.LocalArticleID > 0))
            {
                ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(this.LocalArticleID);
                if (articleInfo != null)
                {
                    this.htmlJs = string.Concat(new object[] { "closeModal('#MyPictureIframe', 'txtContent', '", articleInfo.Url, "', '", base.Server.HtmlEncode(articleInfo.Title), "', '", base.Server.HtmlEncode(articleInfo.Memo), "', '", base.Server.HtmlEncode(articleInfo.ImageUrl), "', ", articleInfo.ArticleId, ")" });
                }
            }
        }
    }
}

