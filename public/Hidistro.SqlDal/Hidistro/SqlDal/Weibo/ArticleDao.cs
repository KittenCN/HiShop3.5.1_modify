namespace Hidistro.SqlDal.Weibo
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Weibo;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class ArticleDao
    {
        private string articledetailUrl = ("http://" + Globals.DomainName + Globals.ApplicationPath + "/vshop/ArticleDetail.aspx?$1");
        private Database database = DatabaseFactory.CreateDatabase();

        public int AddMultiArticle(ArticleInfo article)
        {
            int num;
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO vshop_Article(").Append("Title,ArticleType,LinkType,Content,ImageUrl,Url,Memo,PubTime,IsShare)").Append(" VALUES (@Title,@ArticleType,@LinkType,@Content,@ImageUrl,@Url,@Memo,@PubTime,@IsShare);select @@identity");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
            this.database.AddInParameter(sqlStringCommand, "ArticleType", DbType.Int32, article.ArticleType);
            this.database.AddInParameter(sqlStringCommand, "LinkType", DbType.Int32, article.LinkType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, article.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, article.Memo);
            this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "IsShare", DbType.Boolean, article.IsShare);
            if (int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(), out num))
            {
                if (article.LinkType == LinkType.ArticleDetail)
                {
                    string query = "update vshop_Article set Url=@Url where ArticleId=@ArticleId";
                    sqlStringCommand = this.database.GetSqlStringCommand(query);
                    article.ArticleId = num;
                    article.Url = this.articledetailUrl.Replace("$1", "sid=" + article.ArticleId.ToString());
                    this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
                    this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                }
                foreach (ArticleItemsInfo info in article.ItemsInfo)
                {
                    builder = new StringBuilder();
                    builder.Append("insert into vshop_ArticleItems(");
                    builder.Append("ArticleId,Title,Content,ImageUrl,Url,LinkType)");
                    builder.Append(" values (");
                    builder.Append("@ArticleId,@Title,@Content,@ImageUrl,@Url,@LinkType);select @@identity");
                    sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
                    this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, num);
                    this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, info.Title);
                    this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, info.Content);
                    this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, info.ImageUrl);
                    this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, info.Url);
                    this.database.AddInParameter(sqlStringCommand, "LinkType", DbType.Int32, info.LinkType);
                    int num2 = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
                    if (info.LinkType == LinkType.ArticleDetail)
                    {
                        string str2 = "update vshop_ArticleItems set Url=@Url where Id=@Id";
                        sqlStringCommand = this.database.GetSqlStringCommand(str2);
                        info.Id = num2;
                        info.Url = this.articledetailUrl.Replace("$1", "iid=" + info.Id.ToString());
                        this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, info.Url);
                        this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, info.Id);
                        this.database.ExecuteNonQuery(sqlStringCommand);
                    }
                }
            }
            return num;
        }

        public int AddSingerArticle(ArticleInfo article)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO vshop_Article(").Append("Title,ArticleType,LinkType,Content,ImageUrl,Url,Memo,PubTime,IsShare)").Append(" VALUES (@Title,@ArticleType,@LinkType,@Content,@ImageUrl,@Url,@Memo,@PubTime,@IsShare);select @@identity");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
            this.database.AddInParameter(sqlStringCommand, "ArticleType", DbType.Int32, article.ArticleType);
            this.database.AddInParameter(sqlStringCommand, "LinkType", DbType.Int32, article.LinkType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, article.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "IsShare", DbType.Boolean, article.IsShare);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, article.Memo);
            this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, DateTime.Now);
            int num = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
            if (article.LinkType == LinkType.ArticleDetail)
            {
                string query = "update vshop_Article set Url=@Url where ArticleId=@ArticleId";
                sqlStringCommand = this.database.GetSqlStringCommand(query);
                article.ArticleId = num;
                article.Url = this.articledetailUrl.Replace("$1", "sid=" + article.ArticleId.ToString());
                this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
                this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
            return num;
        }

        public DataSet ArticleIsInWeiXinReply(int articleId)
        {
            string query = string.Concat(new object[] { "select count(0) from vshop_Reply where ArticleID=", articleId, ";select count(0) from Weibo_Reply where ArticleID=", articleId, ";select count(0) from vshop_AliFuwuReply where ArticleID=", articleId });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public bool DeleteArticle(int articleId)
        {
            string query = "DELETE FROM vshop_Article WHERE ArticleId=@ArticleId;DELETE FROM vshop_ArticleItems WHERE ArticleId=@ArticleId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public ArticleInfo GetArticleInfo(int articleid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ArticleId,Title,ArticleType,LinkType,Content,ImageUrl,Url,Memo,PubTime,MediaId,IsShare from vshop_Article ");
            builder.Append(" where ArticleId=@ArticleId ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return this.ReaderBind(reader);
            }
        }

        public IList<ArticleItemsInfo> GetArticleItems(int articleid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select Id,ArticleId,Title,Content,ImageUrl,Url,LinkType,MediaId,PubTime from vshop_ArticleItems ");
            builder.Append(" where ArticleId=@ArticleId order by ID asc ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<ArticleItemsInfo>(reader);
            }
        }

        public ArticleItemsInfo GetArticleItemsInfo(int itemid)
        {
            StringBuilder builder = new StringBuilder();
            ArticleItemsInfo info = new ArticleItemsInfo();
            builder.Append("select Id,ArticleId,Title,Content,ImageUrl,Url,LinkType,PubTime,mediaid from vshop_ArticleItems ");
            builder.Append(" where Id=@Id order by ID asc ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, itemid);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            info.Id = int.Parse(table.Rows[0]["ID"].ToString());
            info.Title = table.Rows[0]["Title"].ToString();
            info.LinkType = (LinkType) int.Parse(table.Rows[0]["LinkType"].ToString());
            info.ArticleId = int.Parse(table.Rows[0]["ArticleId"].ToString());
            info.ImageUrl = table.Rows[0]["ImageUrl"].ToString();
            info.Url = table.Rows[0]["Url"].ToString();
            info.Content = table.Rows[0]["Content"].ToString();
            info.PubTime = DateTime.Parse(table.Rows[0]["PubTime"].ToString());
            if (table.Rows[0]["MediaId"] != null)
            {
                info.MediaId = table.Rows[0]["MediaId"].ToString();
                return info;
            }
            info.MediaId = "";
            return info;
        }

        public DbQueryResult GetArticleRequest(ArticleQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.ArticleType > 0)
            {
                builder.AppendFormat(" ArticleType = {0} ", query.ArticleType);
            }
            if (query.IsShare >= 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IsShare = {0} ", query.IsShare);
            }
            if (!string.IsNullOrEmpty(query.Title))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("( Title LIKE '%{0}%' or Memo like '%{0}%' or exists (select 1 from  vshop_ArticleItems where title like '%{0}%' and ArticleId=vshop_Article.ArticleId))", DataHelper.CleanSearchString(query.Title));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vshop_Article ", "ArticleId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DataTable GetNoImgMsgIdArticleItemList()
        {
            string query = "select top 10 ID,ImageUrl from vshop_ArticleItems  where len(MediaId)<5 or MediaId is null";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetNoImgMsgIdArticleList()
        {
            string query = "select top 10 ArticleId,ImageUrl from vshop_Article where len(MediaId)<5 or MediaId is null";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public ArticleInfo ReaderBind(IDataReader dataReader)
        {
            ArticleInfo info = null;
            if (dataReader.Read())
            {
                info = new ArticleInfo();
                object obj2 = dataReader["ArticleId"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.ArticleId = (int) obj2;
                }
                info.Title = dataReader["Title"].ToString();
                obj2 = dataReader["ArticleType"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.ArticleType = (ArticleType) obj2;
                }
                obj2 = dataReader["LinkType"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.LinkType = (LinkType) obj2;
                }
                info.Content = dataReader["Content"].ToString();
                info.ImageUrl = dataReader["ImageUrl"].ToString();
                info.Url = dataReader["Url"].ToString();
                info.Memo = dataReader["Memo"].ToString();
                info.IsShare = (bool) dataReader["IsShare"];
                obj2 = dataReader["PubTime"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.PubTime = (DateTime) obj2;
                }
                if (info.ArticleType == ArticleType.List)
                {
                    info.ItemsInfo = this.GetArticleItems(info.ArticleId);
                }
                obj2 = dataReader["MediaId"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.MediaId = obj2.ToString();
                    return info;
                }
                info.MediaId = "";
            }
            return info;
        }

        public void UpdateArticleItem(ArticleItemsInfo iteminfo)
        {
            string query = string.Empty;
            if (iteminfo.Id > 0)
            {
                query = "update vshop_ArticleItems set Title=@Title,Content=@Content,ImageUrl=@ImageUrl,Url=@Url,LinkType=@LinkType,PubTime=@PubTime where Id=@Id";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, iteminfo.Id);
                this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, iteminfo.Title);
                this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, iteminfo.Content);
                this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, iteminfo.ImageUrl);
                this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, iteminfo.Url);
                this.database.AddInParameter(sqlStringCommand, "LinkType", DbType.Int32, iteminfo.LinkType);
                this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, iteminfo.PubTime);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("insert into vshop_ArticleItems(");
                builder.Append("ArticleId,Title,Content,ImageUrl,Url,LinkType,PubTime)");
                builder.Append(" values (");
                builder.Append("@ArticleId,@Title,@Content,@ImageUrl,@Url,@LinkType,@PubTime);select @@identity");
                DbCommand command = this.database.GetSqlStringCommand(builder.ToString());
                this.database.AddInParameter(command, "ArticleId", DbType.Int32, iteminfo.ArticleId);
                this.database.AddInParameter(command, "Title", DbType.String, iteminfo.Title);
                this.database.AddInParameter(command, "Content", DbType.String, iteminfo.Content);
                this.database.AddInParameter(command, "ImageUrl", DbType.String, iteminfo.ImageUrl);
                this.database.AddInParameter(command, "Url", DbType.String, iteminfo.Url);
                this.database.AddInParameter(command, "LinkType", DbType.Int32, iteminfo.LinkType);
                this.database.AddInParameter(command, "PubTime", DbType.DateTime, iteminfo.PubTime);
                int num = int.Parse(this.database.ExecuteScalar(command).ToString());
                if (iteminfo.LinkType == LinkType.ArticleDetail)
                {
                    string str2 = "update vshop_ArticleItems set Url=@Url where Id=@Id";
                    command = this.database.GetSqlStringCommand(str2);
                    iteminfo.Id = num;
                    iteminfo.Url = this.articledetailUrl.Replace("$1", "iid=" + iteminfo.Id.ToString());
                    this.database.AddInParameter(command, "Url", DbType.String, iteminfo.Url);
                    this.database.AddInParameter(command, "Id", DbType.Int32, iteminfo.Id);
                    this.database.ExecuteNonQuery(command);
                }
            }
        }

        public bool UpdateMedia_Id(int type, int id, string mediaid)
        {
            StringBuilder builder = new StringBuilder();
            if (type == 0)
            {
                builder.Append("UPDATE vshop_Article SET mediaid=@mediaid WHERE ArticleId=@ID");
            }
            else
            {
                builder.Append("UPDATE vshop_ArticleItems SET mediaid=@mediaid WHERE Id=@ID");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, id);
            this.database.AddInParameter(sqlStringCommand, "mediaid", DbType.String, mediaid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateMultiArticle(ArticleInfo article)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE vshop_Article SET ").Append("Title=@Title,").Append("ArticleType=@ArticleType,").Append("LinkType=@LinkType,").Append("Content=@Content,").Append("ImageUrl=@ImageUrl,").Append("Url=@Url,").Append("Memo=@Memo,").Append("IsShare=@IsShare,").Append("PubTime=@PubTime").Append(" WHERE ArticleId=@ArticleId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
            this.database.AddInParameter(sqlStringCommand, "ArticleType", DbType.Int32, article.ArticleType);
            this.database.AddInParameter(sqlStringCommand, "LinkType", DbType.Int32, article.LinkType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, article.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, article.Memo);
            this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, article.PubTime);
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
            this.database.AddInParameter(sqlStringCommand, "IsShare", DbType.Boolean, article.IsShare);
            bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            if (flag)
            {
                foreach (ArticleItemsInfo info in article.ItemsInfo)
                {
                    info.ArticleId = article.ArticleId;
                    if (info.LinkType == LinkType.ArticleDetail)
                    {
                        info.Url = this.articledetailUrl.Replace("$1", "iid=" + info.Id.ToString());
                    }
                    this.UpdateArticleItem(info);
                }
                string query = "delete from vshop_ArticleItems WHERE ArticleId=@ArticleId and PubTime<>@PubTime";
                if (article.LinkType == LinkType.ArticleDetail)
                {
                    query = query + ";update vshop_Article set Url=@Url where ArticleId=@ArticleId";
                }
                sqlStringCommand = this.database.GetSqlStringCommand(query);
                article.Url = this.articledetailUrl.Replace("$1", "sid=" + article.ArticleId.ToString());
                this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
                this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
                this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, article.PubTime);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
            return flag;
        }

        public bool UpdateSingleArticle(ArticleInfo article)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE vshop_Article SET ").Append("Title=@Title,").Append("ArticleType=@ArticleType,").Append("LinkType=@LinkType,").Append("Content=@Content,").Append("ImageUrl=@ImageUrl,").Append("Url=@Url,").Append("Memo=@Memo,").Append("IsShare=@IsShare,").Append("PubTime=@PubTime").Append(" WHERE ArticleId=@ArticleId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (article.LinkType == LinkType.ArticleDetail)
            {
                article.Url = this.articledetailUrl.Replace("$1", "sid=" + article.ArticleId.ToString());
            }
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
            this.database.AddInParameter(sqlStringCommand, "ArticleType", DbType.Int32, article.ArticleType);
            this.database.AddInParameter(sqlStringCommand, "LinkType", DbType.Int32, article.LinkType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, article.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, article.Memo);
            this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, article.PubTime);
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
            this.database.AddInParameter(sqlStringCommand, "IsShare", DbType.Boolean, article.IsShare);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

