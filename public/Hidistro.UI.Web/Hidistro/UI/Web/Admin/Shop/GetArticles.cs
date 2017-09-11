namespace Hidistro.UI.Web.Admin.Shop
{
    using ASPNET.WebControls;
    using  global:: ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Weibo;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Text;
    using System.Web.UI.WebControls;

    public class GetArticles : AdminPage
    {
        protected string ArticleTitle;
        protected int articletype;
        private int pageno;
        protected Pager pager;
        protected int recordcount;
        protected Repeater rptList;
        private string title;

        protected GetArticles() : base("m01", "dpp06")
        {
            this.ArticleTitle = string.Empty;
            this.title = string.Empty;
        }

        private void BindData(int articletype, int pageno, string title)
        {
            ArticleQuery entity = new ArticleQuery {
                Title = title,
                ArticleType = articletype
            };
            if (articletype == 1)
            {
                entity.ArticleType = 0;
                entity.IsShare = 1;
            }
            entity.SortBy = "PubTime";
            entity.SortOrder = SortAction.Desc;
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

        protected string FormatArticleShow(object articleId, object articletype, object title, object pubtime, object imgurl, object memo, object IsShare)
        {
            StringBuilder builder = new StringBuilder();
            if (articletype.ToString() == "2")
            {
                builder.AppendLine("<div class='single-mate mate-list'>");
                builder.AppendLine("    <div class='mate-inner'>");
                builder.AppendLine("          <h3>" + title + "</h3>");
                builder.AppendLine("           <span>" + DateTime.Parse(pubtime.ToString()).ToString("yyyy-MM-dd HH:mm") + "</span>");
                builder.AppendLine("         <div class='mate-img'>");
                builder.AppendLine("             <img src='" + imgurl + "' class='img-responsive'>");
                builder.AppendLine("          </div>");
                builder.AppendLine("         <p class='mate-info'>" + memo + "</p>");
                builder.AppendLine("     </div>");
                builder.AppendLine(" <div class='nav clearfix'>");
                builder.AppendLine("     <a class='one' href='../weixin/sendalledit.aspx?aid=" + articleId + "'>微信群发</a>");
                builder.AppendLine("     <a href='../weibo/letter.aspx?aid=" + articleId + "'>微博群发</a>");
                builder.AppendLine("     <a href='javascript:void(0)' onclick='ArticleView(" + articleId + ")'>预览</a>");
                builder.AppendLine(string.Concat(new object[] { "     <a href='javascript:void(0)' onclick='editOneArticle(", articleId, ",", articletype.ToString(), ")'>编辑</a>" }));
                builder.AppendLine("     <a href='javascript:void(0)' class='dropdown'>");
                builder.AppendLine("         <span id='dLabel' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>删除");
                builder.AppendLine("         </span>");
                builder.AppendLine("         <div class='dropdown-menu width' aria-labelledby='dLabel'>");
                builder.AppendLine("             <p class='dropdown-header'>确定删除吗？</p>");
                builder.AppendLine("             <button type='button' class='btn btn-danger marg' onclick='delOneArticle(" + articleId + ")'>删除</button>");
                builder.AppendLine("             <button type='button' class='btn btn-primary'>取消</button>");
                builder.AppendLine("         </div>");
                builder.AppendLine("     </a>");
                builder.AppendLine(" </div>");
                if ((bool) IsShare)
                {
                    builder.AppendLine("<p class='distributor'>分销商</p>");
                }
                builder.AppendLine("</div>");
            }
            else if (articletype.ToString() == "4")
            {
                builder.AppendLine("<div class='many-mate mate-list'>");
                builder.AppendLine("    <div class='mate-inner top'>");
                builder.AppendLine("        <span>" + DateTime.Parse(pubtime.ToString()).ToString("yyyy-MM-dd HH:mm") + "</span>");
                builder.AppendLine("        <div class='mate-img'>");
                builder.AppendLine("            <img src='" + imgurl + "' class='img-responsive'>");
                builder.AppendLine("            <div class='title'>" + title + "</div>");
                builder.AppendLine("        </div>");
                if ((bool) IsShare)
                {
                    builder.AppendLine("<p class='distributor'>分销商</p>");
                }
                builder.AppendLine("    </div>");
                foreach (ArticleItemsInfo info in ArticleHelper.GetArticleItems(int.Parse(articleId.ToString())))
                {
                    builder.AppendLine("    <div class='mate-inner'>");
                    builder.AppendLine("        <div class='child-mate'>");
                    builder.AppendLine("            <div class='child-mate-title clearfix'>");
                    builder.AppendLine("                <div class='title'>");
                    builder.AppendLine("                    <h4>" + info.Title + "</h4>");
                    builder.AppendLine("                </div>");
                    builder.AppendLine("                <div class='img'>");
                    builder.AppendLine("                    <img src='" + info.ImageUrl + "' class='img-responsive'>");
                    builder.AppendLine("                </div>");
                    builder.AppendLine("            </div>");
                    builder.AppendLine("        </div>");
                    builder.AppendLine("");
                    builder.AppendLine("    </div>");
                }
                builder.AppendLine("    <div class='nav clearfix'>");
                builder.AppendLine("        <a class='one' href='../weixin/sendalledit.aspx?aid=" + articleId + "'>微信群发</a>");
                builder.AppendLine("        <a href='../weibo/letter.aspx?aid=" + articleId + "'>微博群发</a>");
                builder.AppendLine("        <a href='javascript:void(0)' onclick='ArticleView(" + articleId + ")'>预览</a>");
                builder.AppendLine(string.Concat(new object[] { "        <a href='javascript:void(0)' onclick='editOneArticle(", articleId, ",", articletype.ToString(), ")'>编辑</a>" }));
                builder.AppendLine("        <a href='javascript:void(0)' class='dropdown'>");
                builder.AppendLine("            <span id='dLabel' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>删除");
                builder.AppendLine("            </span>");
                builder.AppendLine("            <div class='dropdown-menu width' aria-labelledby='dLabel'>");
                builder.AppendLine("                <p class='dropdown-header'>确定删除吗？</p>");
                builder.AppendLine("                <button type='button' class='btn btn-danger marg' onclick='delOneArticle(" + articleId + ")'>删除</button>");
                builder.AppendLine("                <button type='button' class='btn btn-primary'>取消</button>");
                builder.AppendLine("            </div>");
                builder.AppendLine("        </a>");
                builder.AppendLine("    </div>");
                builder.AppendLine("</div>");
            }
            return builder.ToString();
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.Form["key"]))
            {
                this.ArticleTitle = base.Server.UrlDecode(this.Page.Request.Form["key"]);
            }
            string s = base.Request.Form["type"];
            string str2 = base.Request.QueryString["pageindex"];
            int.TryParse(s, out this.articletype);
            int.TryParse(str2, out this.pageno);
            if (this.pageno < 1)
            {
                this.pageno = 1;
            }
            switch (this.articletype)
            {
                case 1:
                case 2:
                case 4:
                    return;
            }
            this.articletype = 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            this.BindData(this.articletype, this.pageno, this.ArticleTitle);
        }
    }
}

