namespace Hidistro.UI.Web.Admin.Shop
{
    using  global:: ControlPanel.WeiBo;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.Entities.Weibo;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.hieditor.ueditor.controls;
    using System;
    using System.Web.UI.HtmlControls;

    public class ArticlesEdit : AdminPage
    {
        protected string articleJson;
        protected ucUeditor fkContent;
        protected string htmlAddJs;
        protected string htmlArticleTitle;
        protected string htmlDate;
        protected string htmlImgUrl;
        protected string htmlLinkType;
        protected string htmlLinkTypeName;
        protected string htmlMemo;
        protected string htmlOperName;
        protected string htmlUrl;
        protected HtmlInputCheckBox IsShare;
        protected int MaterialID;
        protected string ReUrl;

        protected ArticlesEdit() : base("m01", "dpp06")
        {
            this.htmlOperName = "新增";
            this.htmlArticleTitle = "单条图文标题";
            this.htmlImgUrl = string.Empty;
            this.htmlUrl = string.Empty;
            this.htmlLinkTypeName = "查看全文";
            this.htmlMemo = "摘要";
            this.htmlLinkType = "1";
            this.htmlAddJs = string.Empty;
            this.htmlDate = DateTime.Now.ToString("M月d日");
            this.ReUrl = string.Empty;
        }

        [PrivilegeCheck(Privilege.Summary)]
        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(base.Request.QueryString["id"], out this.MaterialID);
            string text1 = base.Request.QueryString["cmd"];
            this.ReUrl = base.Request.QueryString["reurl"];
            if (string.IsNullOrEmpty(this.ReUrl))
            {
                this.ReUrl = "articles.aspx";
            }
            string str = base.Request.Form["posttype"];
            if (!base.IsPostBack)
            {
                if (str == "addsinglearticle")
                {
                    base.Response.ContentType = "application/json";
                    string str2 = base.Request.Form["linkUrl"];
                    string str3 = base.Request.Form["title"];
                    string str4 = base.Request.Form["img"];
                    string str5 = base.Request.Form["memo"];
                    string str6 = base.Request.Form["content"];
                    string s = base.Request.Form["linkType"];
                    string str8 = base.Request.Form["IsShare"];
                    int result = 1;
                    int.TryParse(s, out result);
                    string str9 = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                    if (string.IsNullOrEmpty(str3))
                    {
                        str9 = "{\"type\":\"0\",\"tips\":\"请填写标题！\"}";
                        base.Response.Write(str9);
                        base.Response.End();
                    }
                    if (string.IsNullOrEmpty(str4))
                    {
                        str9 = "{\"type\":\"0\",\"tips\":\"请选择封面图片！\"}";
                        base.Response.Write(str9);
                        base.Response.End();
                    }
                    if ((result != 1) && string.IsNullOrEmpty(str2))
                    {
                        str9 = "{\"type\":\"0\",\"tips\":\"请设置链接地址！\"}";
                        base.Response.Write(str9);
                        base.Response.End();
                    }
                    ArticleInfo article = new ArticleInfo {
                        ArticleId = this.MaterialID,
                        Url = str2,
                        Title = str3
                    };
                    if (string.IsNullOrEmpty(str8))
                    {
                        article.IsShare = false;
                    }
                    else
                    {
                        article.IsShare = bool.Parse(str8);
                    }
                    article.ImageUrl = str4;
                    article.Memo = str5;
                    article.ArticleType = ArticleType.News;
                    article.PubTime = DateTime.Now;
                    article.Content = str6;
                    article.LinkType = (LinkType) result;
                    if (article.ArticleId > 0)
                    {
                        if (ArticleHelper.UpdateSingleArticle(article))
                        {
                            str9 = "{\"type\":\"1\",\"id\":\"" + article.ArticleId + "\",\"tips\":\"单图文修改成功！\"}";
                        }
                    }
                    else
                    {
                        int num2 = ArticleHelper.AddSingerArticle(article);
                        if (num2 > 0)
                        {
                            str9 = "{\"type\":\"1\",\"id\":\"" + num2 + "\",\"tips\":\"单图文新增成功！\"}";
                        }
                    }
                    base.Response.Write(str9);
                    base.Response.End();
                }
                else if (this.MaterialID > 0)
                {
                    this.htmlOperName = "编辑";
                    ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(this.MaterialID);
                    if (articleInfo != null)
                    {
                        if (articleInfo.ArticleType == ArticleType.List)
                        {
                            base.Response.Redirect("multiarticlesedit.aspx?id=" + this.MaterialID);
                            base.Response.End();
                        }
                        else
                        {
                            this.htmlArticleTitle = articleInfo.Title;
                            this.htmlImgUrl = articleInfo.ImageUrl;
                            this.htmlUrl = articleInfo.Url;
                            this.htmlMemo = articleInfo.Memo;
                            this.htmlDate = articleInfo.PubTime.ToString("M月d日");
                            this.fkContent.Text = articleInfo.Content;
                            this.IsShare.Checked = articleInfo.IsShare;
                            this.htmlAddJs = "BindPicData('" + this.htmlImgUrl + "');";
                            this.htmlLinkType = ((int) articleInfo.LinkType).ToString();
                            if (this.htmlLinkType != "1")
                            {
                                this.htmlAddJs = this.htmlAddJs + "$('#urlData').show();";
                            }
                            this.htmlLinkTypeName = articleInfo.LinkType.ToShowText();
                        }
                    }
                }
            }
        }
    }
}

