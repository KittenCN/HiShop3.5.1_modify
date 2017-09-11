namespace ControlPanel.WeiBo
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Weibo;
    using Hidistro.SqlDal.Weibo;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class ArticleHelper
    {
        public static int AddMultiArticle(ArticleInfo article)
        {
            return new ArticleDao().AddMultiArticle(article);
        }

        public static int AddSingerArticle(ArticleInfo article)
        {
            return new ArticleDao().AddSingerArticle(article);
        }

        public static DataSet ArticleIsInWeiXinReply(int articleId)
        {
            return new ArticleDao().ArticleIsInWeiXinReply(articleId);
        }

        public static bool DeleteArticle(int articleId)
        {
            return new ArticleDao().DeleteArticle(articleId);
        }

        public static ArticleInfo GetArticleInfo(int articleid)
        {
            return new ArticleDao().GetArticleInfo(articleid);
        }

        public static IList<ArticleItemsInfo> GetArticleItems(int articleid)
        {
            return new ArticleDao().GetArticleItems(articleid);
        }

        public static ArticleItemsInfo GetArticleItemsInfo(int itemid)
        {
            return new ArticleDao().GetArticleItemsInfo(itemid);
        }

        public static DbQueryResult GetArticleRequest(ArticleQuery query)
        {
            return new ArticleDao().GetArticleRequest(query);
        }

        public static DataTable GetNoImgMsgIdArticleItemList()
        {
            return new ArticleDao().GetNoImgMsgIdArticleItemList();
        }

        public static DataTable GetNoImgMsgIdArticleList()
        {
            return new ArticleDao().GetNoImgMsgIdArticleList();
        }

        public static bool UpdateMedia_Id(int type, int id, string mediaid)
        {
            return new ArticleDao().UpdateMedia_Id(type, id, mediaid);
        }

        public static bool UpdateMultiArticle(ArticleInfo article)
        {
            return new ArticleDao().UpdateMultiArticle(article);
        }

        public static bool UpdateSingleArticle(ArticleInfo article)
        {
            return new ArticleDao().UpdateSingleArticle(article);
        }
    }
}

