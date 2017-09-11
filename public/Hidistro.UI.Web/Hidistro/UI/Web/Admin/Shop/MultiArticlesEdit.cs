namespace Hidistro.UI.Web.Admin.Shop
{
    using  global:: ControlPanel.WeiBo;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.Entities.Weibo;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.hieditor.ueditor.controls;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;

    public class MultiArticlesEdit : AdminPage
    {
        protected string articleJson;
        protected ucUeditor fkContent;
        protected string htmlAddJs;
        protected string htmlLinkType;
        protected string htmlLinkTypeName;
        protected string htmlOperName;
        protected string htmlUrl;
        protected HtmlInputCheckBox IsShare;
        protected int MaterialID;
        protected string ReUrl;

        protected MultiArticlesEdit() : base("m01", "dpp06")
        {
            this.htmlOperName = "新增";
            this.htmlLinkType = "1";
            this.htmlUrl = string.Empty;
            this.htmlLinkTypeName = "阅读原文";
            this.ReUrl = string.Empty;
            this.htmlAddJs = string.Empty;
        }

        [PrivilegeCheck(Privilege.Summary)]
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ReUrl = Globals.RequestQueryStr("reurl");
            if (string.IsNullOrEmpty(this.ReUrl))
            {
                this.ReUrl = "articles.aspx";
            }
            int.TryParse(Globals.RequestQueryStr("id"), out this.MaterialID);
            if (!(Globals.RequestQueryStr("cmd") == "add"))
            {
                if (this.MaterialID > 0)
                {
                    this.htmlOperName = "编辑";
                    ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(this.MaterialID);
                    if (articleInfo != null)
                    {
                        if (articleInfo.ArticleType == ArticleType.News)
                        {
                            base.Response.Redirect("articlesedit.aspx?id=" + this.MaterialID);
                            base.Response.End();
                        }
                        else
                        {
                            IList<ArticleItemsInfo> itemsInfo = articleInfo.ItemsInfo;
                            ArticleItemsInfo item = new ArticleItemsInfo {
                                ArticleId = this.MaterialID,
                                Title = articleInfo.Title,
                                ImageUrl = articleInfo.ImageUrl,
                                Url = articleInfo.Url,
                                Content = articleInfo.Content,
                                LinkType = articleInfo.LinkType,
                                Id = 0,
                                IsShare = articleInfo.IsShare
                            };
                            itemsInfo.Insert(0, item);
                            this.IsShare.Checked = articleInfo.IsShare;
                            this.htmlLinkType = ((int) articleInfo.LinkType).ToString();
                            if (this.htmlLinkType != "1")
                            {
                                this.htmlAddJs = "$('#urlData').show();";
                            }
                            this.htmlLinkTypeName = articleInfo.LinkType.ToShowText();
                            List<ArticleList> list5 = new List<ArticleList>();
                            int num3 = 1;
                            foreach (ArticleItemsInfo info5 in itemsInfo)
                            {
                                ArticleList list6 = new ArticleList {
                                    Id = info5.Id,
                                    Title = info5.Title,
                                    Url = info5.Url,
                                    ImageUrl = info5.ImageUrl,
                                    Content = info5.Content
                                };
                                list6.BoxId = num3++.ToString();
                                list6.LinkType = info5.LinkType;
                                list6.Status = "";
                                list6.IsShare = info5.IsShare;
                                list5.Add(list6);
                            }
                            this.articleJson = JsonConvert.SerializeObject(list5);
                        }
                    }
                }
                else
                {
                    this.articleJson = "''";
                }
            }
            else
            {
                base.Response.ContentType = "application/json";
                string str2 = Globals.RequestFormStr("MultiArticle");
                string s = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
                List<ArticleList> list = JsonConvert.DeserializeObject<List<ArticleList>>(str2);
                if ((list != null) && (list.Count > 0))
                {
                    int num = 0;
                    ArticleInfo article = new ArticleInfo();
                    List<ArticleItemsInfo> list2 = new List<ArticleItemsInfo>();
                    DateTime now = DateTime.Now;
                    string str4 = string.Empty;
                    foreach (ArticleList list3 in list)
                    {
                        if (list3.Title == "")
                        {
                            str4 = "标题不能为空!";
                            break;
                        }
                        if (list3.ImageUrl == "")
                        {
                            str4 = "请选择一张封面!";
                            break;
                        }
                        if ((list3.LinkType == LinkType.ArticleDetail) && (list3.Content == ""))
                        {
                            str4 = "请输入内容!";
                            break;
                        }
                        if ((list3.LinkType != LinkType.ArticleDetail) && (list3.Url == ""))
                        {
                            str4 = "请选择或输入自定义链接!";
                            break;
                        }
                        if (list3.Status != "del")
                        {
                            if (num == 0)
                            {
                                article.Title = list3.Title;
                                article.ArticleType = ArticleType.List;
                                article.Content = list3.Content;
                                article.ImageUrl = list3.ImageUrl;
                                article.Url = list3.Url;
                                article.LinkType = list3.LinkType;
                                article.Memo = "";
                                article.ArticleId = this.MaterialID;
                                article.PubTime = now;
                                article.IsShare = list3.IsShare;
                                num++;
                            }
                            else
                            {
                                ArticleItemsInfo info2 = list3;
                                info2.PubTime = now;
                                list2.Add(info2);
                                num++;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(str4))
                    {
                        s = "{\"type\":\"0\",\"tips\":\"" + str4 + "\"}";
                        base.Response.Write(s);
                        base.Response.End();
                    }
                    article.ItemsInfo = list2;
                    if (article.ArticleId > 0)
                    {
                        if (ArticleHelper.UpdateMultiArticle(article))
                        {
                            s = "{\"type\":\"1\",\"id\":\"" + article.ArticleId + "\",\"tips\":\"多图素材修改成功！\"}";
                        }
                    }
                    else
                    {
                        int num2 = ArticleHelper.AddMultiArticle(article);
                        if (num2 > 0)
                        {
                            s = "{\"type\":\"1\",\"id\":\"" + num2 + "\",\"tips\":\"多图素材新增成功！\"}";
                        }
                    }
                    base.Response.Write(s);
                    base.Response.End();
                }
            }
        }
    }
}

