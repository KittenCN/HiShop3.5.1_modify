namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using global::ControlPanel.WeiBo;
    using global::ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Entities.Weibo;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VArticleDetail : VshopTemplatedWebControl
    {
        protected string htmlTitle = string.Empty;
        private HiImage imgUrl;
        protected int itemID;
        private Literal litContent;
        private Literal LitJs;
        protected int singleID;

        protected override void AttachChildControls()
        {
            this.itemID = Globals.RequestQueryNum("iid");
            this.singleID = Globals.RequestQueryNum("sid");
            this.imgUrl = (HiImage) this.FindControl("imgUrl");
            this.litContent = (Literal) this.FindControl("litContent");
            this.LitJs = (Literal) this.FindControl("LitJs");
            if (this.singleID > 0)
            {
                ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(this.singleID);
                if (articleInfo != null)
                {
                    this.htmlTitle = articleInfo.Title;
                    this.imgUrl.ImageUrl = articleInfo.ImageUrl;
                    this.litContent.Text = articleInfo.Content;
                    string imageUrl = articleInfo.ImageUrl;
                    if (!imageUrl.ToLower().StartsWith("http"))
                    {
                        imageUrl = Globals.GetWebUrlStart() + imageUrl;
                    }
                    string str2 = Globals.ReplaceHtmlTag(articleInfo.Memo, 50);
                    this.LitJs.Text = "<script>wxinshare_title ='" + this.Page.Server.HtmlEncode(this.htmlTitle.Replace("\n", "").Replace("\r", "")) + "';wxinshare_desc = '" + this.Page.Server.HtmlEncode(str2.Replace("\n", "").Replace("\r", "")) + "';wxinshare_link = location.href;wxinshare_imgurl = '" + imageUrl + "';</script>";
                }
                else
                {
                    base.GotoResourceNotFound("");
                }
            }
            else if (this.itemID > 0)
            {
                ArticleItemsInfo articleItemsInfo = ArticleHelper.GetArticleItemsInfo(this.itemID);
                if (articleItemsInfo != null)
                {
                    this.htmlTitle = articleItemsInfo.Title;
                    this.imgUrl.ImageUrl = articleItemsInfo.ImageUrl;
                    this.litContent.Text = articleItemsInfo.Content;
                    string str3 = articleItemsInfo.ImageUrl;
                    if (!str3.ToLower().StartsWith("http"))
                    {
                        str3 = Globals.GetWebUrlStart() + str3;
                    }
                    string str4 = Globals.ReplaceHtmlTag(articleItemsInfo.Content, 50);
                    this.LitJs.Text = "<script>wxinshare_title ='" + this.Page.Server.HtmlEncode(this.htmlTitle.Replace("\n", "").Replace("\r", "")) + "';wxinshare_desc = '" + this.Page.Server.HtmlEncode(str4.Replace("\n", "").Replace("\r", "")) + "';wxinshare_link = location.href;wxinshare_imgurl = '" + str3 + "';</script>";
                }
                else
                {
                    base.GotoResourceNotFound("");
                }
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
                this.SkinName = "Skin-VArticleDetails.html";
            }
            base.OnInit(e);
        }
    }
}

