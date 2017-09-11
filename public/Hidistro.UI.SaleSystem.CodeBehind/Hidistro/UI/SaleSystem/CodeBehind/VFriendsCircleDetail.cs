namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using global::ControlPanel.WeiBo;
    using global::ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Entities.Weibo;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VFriendsCircleDetail : VshopTemplatedWebControl
    {
        private HtmlInputHidden hdDesc;
        private Repeater ItemCtx;
        protected int MaterialID;
        private Repeater TopCtx;

        protected override void AttachChildControls()
        {
            this.hdDesc = (HtmlInputHidden) this.FindControl("hdDesc");
            this.MaterialID = Globals.RequestQueryNum("ID");
            if (this.MaterialID <= 0)
            {
                this.Page.Response.Redirect("/");
            }
            string title = "";
            ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(this.MaterialID);
            if (articleInfo == null)
            {
                this.Page.Response.Redirect("/");
            }
            title = articleInfo.Title;
            DateTime now = DateTime.Now;
            this.TopCtx = (Repeater) this.FindControl("TopCtx");
            this.ItemCtx = (Repeater) this.FindControl("ItemCtx");
            List<ArticleInfo> list = new List<ArticleInfo> {
                articleInfo
            };
            this.TopCtx.DataSource = list;
            this.TopCtx.DataBind();
            if (articleInfo.ArticleType == ArticleType.List)
            {
                string str2 = Globals.ReplaceHtmlTag(articleInfo.Content, 50);
                if (!string.IsNullOrEmpty(str2))
                {
                    this.hdDesc.Value = str2;
                }
                IList<ArticleItemsInfo> itemsInfo = articleInfo.ItemsInfo;
                this.ItemCtx.DataSource = itemsInfo;
                this.ItemCtx.DataBind();
            }
            else
            {
                string str3 = Globals.ReplaceHtmlTag(articleInfo.Memo, 50);
                if (!string.IsNullOrEmpty(str3))
                {
                    this.hdDesc.Value = str3;
                }
            }
            PageTitle.AddSiteNameTitle(title);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VFriendsCircleDetail.html";
            }
            base.OnInit(e);
        }
    }
}

