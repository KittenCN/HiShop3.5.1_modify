namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.Entities.Weibo;
    using Hidistro.SqlDal.Weibo;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class ReplyDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public void DeleteNewsMsg(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(new StringBuilder(" delete from vshop_Message where MsgID=@MsgID").ToString());
            this.database.AddInParameter(sqlStringCommand, "MsgID", DbType.Int32, id);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool DeleteReply(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE vshop_Reply WHERE ReplyId = @ReplyId;DELETE vshop_Message WHERE ReplyId = @ReplyId");
            this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public IList<Hidistro.Entities.VShop.ReplyInfo> GetAllReply()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ReplyId,Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ActivityId,ArticleID");
            builder.Append(" FROM vshop_Reply order by Replyid desc ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            List<Hidistro.Entities.VShop.ReplyInfo> list = new List<Hidistro.Entities.VShop.ReplyInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    object obj2;
                    Hidistro.Entities.VShop.ReplyInfo info = this.ReaderBind(reader);
                    switch (info.MessageType)
                    {
                        case MessageType.Text:
                        {
                            TextReplyInfo info3 = info as TextReplyInfo;
                            obj2 = reader["Content"];
                            if ((obj2 != null) && (obj2 != DBNull.Value))
                            {
                                info3.Text = obj2.ToString();
                            }
                            list.Add(info3);
                            continue;
                        }
                        case MessageType.News:
                        case MessageType.List:
                        {
                            NewsReplyInfo info2 = info as NewsReplyInfo;
                            info2.NewsMsg = this.GetNewsReplyInfo(info2.Id);
                            list.Add(info2);
                            continue;
                        }
                    }
                    TextReplyInfo item = info as TextReplyInfo;
                    obj2 = reader["Content"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        item.Text = obj2.ToString();
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public int GetArticleIDByOldArticle(int replyid, MessageType msgtype)
        {
            NewsReplyInfo reply;
            int num = 0;
            ArticleInfo article = new ArticleInfo();
            switch (msgtype)
            {
                case MessageType.News:
                    reply = this.GetReply(replyid) as NewsReplyInfo;
                    if (((reply == null) || (reply.NewsMsg == null)) || (reply.NewsMsg.Count == 0))
                    {
                        return num;
                    }
                    article.Title = reply.NewsMsg[0].Title;
                    article.ArticleType = ArticleType.News;
                    article.Content = reply.NewsMsg[0].Content;
                    article.ImageUrl = reply.NewsMsg[0].PicUrl;
                    article.Url = reply.NewsMsg[0].Url;
                    article.Memo = reply.NewsMsg[0].Description;
                    article.PubTime = DateTime.Now;
                    if (article.Url.Length <= 10)
                    {
                        article.LinkType = LinkType.ArticleDetail;
                    }
                    else
                    {
                        article.LinkType = LinkType.Userdefined;
                    }
                    return new ArticleDao().AddSingerArticle(article);

                case (MessageType.News | MessageType.Text):
                    return num;

                case MessageType.List:
                    reply = this.GetReply(replyid) as NewsReplyInfo;
                    article.Title = reply.NewsMsg[0].Title;
                    article.ArticleType = ArticleType.List;
                    article.Content = reply.NewsMsg[0].Content;
                    article.ImageUrl = reply.NewsMsg[0].PicUrl;
                    article.Url = reply.NewsMsg[0].Url;
                    article.Memo = reply.NewsMsg[0].Description;
                    article.PubTime = DateTime.Now;
                    if (article.Url.Length <= 10)
                    {
                        article.LinkType = LinkType.ArticleDetail;
                        break;
                    }
                    article.LinkType = LinkType.Userdefined;
                    break;

                default:
                    return num;
            }
            List<ArticleItemsInfo> list = new List<ArticleItemsInfo>();
            if ((reply.NewsMsg != null) && (reply.NewsMsg.Count > 0))
            {
                int num2 = 0;
                foreach (NewsMsgInfo info3 in reply.NewsMsg)
                {
                    num2++;
                    if (num2 != 1)
                    {
                        ArticleItemsInfo item = new ArticleItemsInfo {
                            Title = info3.Title,
                            Content = info3.Content,
                            ImageUrl = info3.PicUrl,
                            Url = info3.Url,
                            ArticleId = 0
                        };
                        if (item.Url.Length > 10)
                        {
                            item.LinkType = LinkType.Userdefined;
                        }
                        else
                        {
                            item.LinkType = LinkType.ArticleDetail;
                        }
                        list.Add(item);
                    }
                }
            }
            article.ItemsInfo = list;
            return new ArticleDao().AddMultiArticle(article);
        }

        public Hidistro.Entities.VShop.MessageInfo GetMessage(int messageId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Vshop_Message WHERE MsgID =@MsgID");
            this.database.AddInParameter(sqlStringCommand, "MsgID", DbType.Int32, messageId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<Hidistro.Entities.VShop.MessageInfo>(reader);
            }
        }

        public IList<NewsMsgInfo> GetNewsReplyInfo(int replyid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ReplyId,MsgID,Title,ImageUrl,Url,Description,Content from vshop_Message ");
            builder.Append(" where ReplyId=@ReplyId ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, replyid);
            List<NewsMsgInfo> list = new List<NewsMsgInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(this.ReaderBindNewsRelpy(reader));
                }
            }
            return list;
        }

        public int GetNoMatchReplyID(int compareid)
        {
            string str = string.Empty;
            if (compareid > 0)
            {
                str = " and ReplyId<>" + compareid;
            }
            string query = "select top 1 ReplyId from vshop_Reply where ReplyType=@ReplyType " + str + "  order by ReplyId desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 4);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public IList<Hidistro.Entities.VShop.ReplyInfo> GetReplies(ReplyType type)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ReplyId,Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ActivityId,ArticleID ");
            builder.Append(" FROM vshop_Reply ");
            builder.Append(" where ReplyType = @ReplyType and IsDisable=0");
            builder.Append(" order by replyid desc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int) type);
            List<Hidistro.Entities.VShop.ReplyInfo> list = new List<Hidistro.Entities.VShop.ReplyInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    TextReplyInfo info3;
                    object obj2;
                    Hidistro.Entities.VShop.ReplyInfo info = this.ReaderBind(reader);
                    switch (info.MessageType)
                    {
                        case MessageType.Text:
                        {
                            info3 = info as TextReplyInfo;
                            obj2 = reader["Content"];
                            if ((obj2 != null) && (obj2 != DBNull.Value))
                            {
                                info3.Text = Globals.FormatWXReplyContent(obj2.ToString());
                            }
                            list.Add(info3);
                            continue;
                        }
                        case MessageType.News:
                        case MessageType.List:
                        {
                            NewsReplyInfo item = info as NewsReplyInfo;
                            item.NewsMsg = this.GetNewsReplyInfo(item.Id);
                            list.Add(item);
                            continue;
                        }
                    }
                    info3 = info as TextReplyInfo;
                    obj2 = reader["Content"];
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        info3.Text = Globals.FormatWXReplyContent(obj2.ToString());
                    }
                    list.Add(info3);
                }
            }
            return list;
        }

        public Hidistro.Entities.VShop.ReplyInfo GetReply(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Reply WHERE ReplyId = @ReplyId");
            this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, id);
            Hidistro.Entities.VShop.ReplyInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                TextReplyInfo info3;
                if (reader.Read())
                {
                    info = this.ReaderBind(reader);
                    switch (info.MessageType)
                    {
                        case MessageType.Text:
                            goto Label_0089;

                        case MessageType.News:
                        case MessageType.List:
                        {
                            NewsReplyInfo info2 = info as NewsReplyInfo;
                            info2.NewsMsg = this.GetNewsReplyInfo(info2.Id);
                            return info2;
                        }
                        case (MessageType.News | MessageType.Text):
                            return info;
                    }
                }
                return info;
            Label_0089:
                info3 = info as TextReplyInfo;
                object obj2 = reader["Content"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info3.Text = obj2.ToString();
                }
                return info3;
            }
        }

        public int GetSubscribeID(int compareid)
        {
            string str = string.Empty;
            if (compareid > 0)
            {
                str = " and ReplyId<>" + compareid;
            }
            string query = "select top 1 ReplyId from vshop_Reply where ReplyType=@ReplyType " + str + " order by ReplyId desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 1);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public bool HasReplyKey(string key)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM vshop_Reply WHERE Keys = @Keys");
            this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, key);
            return (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public bool HasReplyKey(string key, int replyid)
        {
            string str = string.Empty;
            if (replyid > 0)
            {
                str = " and ReplyId<>" + replyid;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM vshop_Reply WHERE Keys = @Keys " + str);
            this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, key);
            return (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public Hidistro.Entities.VShop.ReplyInfo ReaderBind(IDataReader dataReader)
        {
            Hidistro.Entities.VShop.ReplyInfo info = null;
            object obj2 = dataReader["MessageType"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                if (((MessageType) obj2) == MessageType.Text)
                {
                    info = new TextReplyInfo();
                }
                else
                {
                    info = new NewsReplyInfo();
                }
            }
            obj2 = dataReader["ReplyId"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Id = (int) obj2;
            }
            info.Keys = dataReader["Keys"].ToString();
            obj2 = dataReader["MatchType"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.MatchType = (MatchType) obj2;
            }
            obj2 = dataReader["ReplyType"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.ReplyType = (ReplyType) obj2;
            }
            obj2 = dataReader["MessageType"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.MessageType = (MessageType) obj2;
            }
            obj2 = dataReader["IsDisable"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.IsDisable = (bool) obj2;
            }
            obj2 = dataReader["LastEditDate"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.LastEditDate = (DateTime) obj2;
            }
            info.LastEditor = dataReader["LastEditor"].ToString();
            obj2 = dataReader["ActivityId"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.ActivityId = (int) obj2;
            }
            obj2 = dataReader["ArticleID"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.ArticleID = (int) obj2;
            }
            return info;
        }

        private NewsMsgInfo ReaderBindNewsRelpy(IDataReader dataReader)
        {
            NewsMsgInfo info = new NewsMsgInfo();
            object obj2 = dataReader["MsgID"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Id = (int) obj2;
            }
            obj2 = dataReader["Title"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Title = dataReader["Title"].ToString();
            }
            obj2 = dataReader["ImageUrl"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.PicUrl = dataReader["ImageUrl"].ToString();
            }
            obj2 = dataReader["Url"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Url = dataReader["Url"].ToString();
            }
            obj2 = dataReader["Description"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Description = dataReader["Description"].ToString();
            }
            obj2 = dataReader["Content"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Content = dataReader["Content"].ToString();
            }
            return info;
        }

        private bool SaveNewsReply(NewsReplyInfo model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("insert into vshop_Reply(");
            builder.Append("Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ArticleID)");
            builder.Append(" values (");
            builder.Append("@Keys,@MatchType,@ReplyType,@MessageType,@IsDisable,@LastEditDate,@LastEditor,@Content,@Type,@ArticleID)");
            builder.Append(";select @@IDENTITY");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, model.Keys);
            this.database.AddInParameter(sqlStringCommand, "MatchType", DbType.Int32, (int) model.MatchType);
            this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int) model.ReplyType);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int) model.MessageType);
            this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, model.IsDisable);
            this.database.AddInParameter(sqlStringCommand, "LastEditDate", DbType.DateTime, model.LastEditDate);
            this.database.AddInParameter(sqlStringCommand, "LastEditor", DbType.String, model.LastEditor);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, "");
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, 2);
            this.database.AddInParameter(sqlStringCommand, "ArticleID", DbType.Int32, model.ArticleID);
            this.database.ExecuteScalar(sqlStringCommand);
            return true;
        }

        public bool SaveReply(Hidistro.Entities.VShop.ReplyInfo reply)
        {
            bool flag = false;
            switch (reply.MessageType)
            {
                case MessageType.Text:
                    return this.SaveTextReply(reply as TextReplyInfo);

                case MessageType.News:
                case MessageType.List:
                    return this.SaveNewsReply(reply as NewsReplyInfo);

                case (MessageType.News | MessageType.Text):
                    return flag;
            }
            return flag;
        }

        private bool SaveTextReply(TextReplyInfo model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("insert into vshop_Reply(");
            builder.Append("Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ActivityId,ArticleID)");
            builder.Append(" values (");
            builder.Append("@Keys,@MatchType,@ReplyType,@MessageType,@IsDisable,@LastEditDate,@LastEditor,@Content,@Type,@ActivityId,@ArticleID)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, model.Keys);
            this.database.AddInParameter(sqlStringCommand, "MatchType", DbType.Int32, (int) model.MatchType);
            this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int) model.ReplyType);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int) model.MessageType);
            this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, model.IsDisable);
            this.database.AddInParameter(sqlStringCommand, "LastEditDate", DbType.DateTime, model.LastEditDate);
            this.database.AddInParameter(sqlStringCommand, "LastEditor", DbType.String, model.LastEditor);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, model.Text);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, model.ActivityId);
            this.database.AddInParameter(sqlStringCommand, "ArticleID", DbType.Int32, model.ArticleID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private bool UpdateNewsReply(NewsReplyInfo reply)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("update vshop_Reply set ");
            builder.Append("Keys=@Keys,");
            builder.Append("MatchType=@MatchType,");
            builder.Append("ReplyType=@ReplyType,");
            builder.Append("MessageType=@MessageType,");
            builder.Append("IsDisable=@IsDisable,");
            builder.Append("LastEditDate=@LastEditDate,");
            builder.Append("LastEditor=@LastEditor,");
            builder.Append("Content=@Content,");
            builder.Append("ArticleID=@ArticleID,");
            builder.Append("Type=@Type");
            builder.Append(" where ReplyId=@ReplyId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, reply.Keys);
            this.database.AddInParameter(sqlStringCommand, "MatchType", DbType.Int32, (int) reply.MatchType);
            this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int) reply.ReplyType);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int) reply.MessageType);
            this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, reply.IsDisable);
            this.database.AddInParameter(sqlStringCommand, "LastEditDate", DbType.DateTime, reply.LastEditDate);
            this.database.AddInParameter(sqlStringCommand, "LastEditor", DbType.String, reply.LastEditor);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, "");
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, 2);
            this.database.AddInParameter(sqlStringCommand, "ArticleID", DbType.Int32, reply.ArticleID);
            this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, reply.Id);
            this.database.ExecuteNonQuery(sqlStringCommand);
            return true;
        }

        public bool UpdateReply(Hidistro.Entities.VShop.ReplyInfo reply)
        {
            switch (reply.MessageType)
            {
                case MessageType.Text:
                    return this.UpdateTextReply(reply as TextReplyInfo);

                case MessageType.News:
                case MessageType.List:
                    return this.UpdateNewsReply(reply as NewsReplyInfo);
            }
            return this.UpdateTextReply(reply as TextReplyInfo);
        }

        public bool UpdateReplyRelease(int id)
        {
            Hidistro.Entities.VShop.ReplyInfo reply = this.GetReply(id);
            StringBuilder builder = new StringBuilder();
            if (reply.IsDisable)
            {
                if ((reply.ReplyType & ReplyType.NoMatch) == ReplyType.NoMatch)
                {
                    builder.AppendFormat("update  vshop_Reply set IsDisable = 1 where ReplyType&{0}>0;", 4);
                }
                if ((reply.ReplyType & ReplyType.Subscribe) == ReplyType.Subscribe)
                {
                    builder.AppendFormat("update  vshop_Reply set IsDisable = 1 where ReplyType&{0}>0;", 1);
                }
            }
            builder.Append("update vshop_Reply set ");
            builder.Append("IsDisable=~IsDisable");
            builder.Append(" where ReplyId=@ReplyId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private bool UpdateTextReply(TextReplyInfo reply)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("update vshop_Reply set ");
            builder.Append("Keys=@Keys,");
            builder.Append("MatchType=@MatchType,");
            builder.Append("ReplyType=@ReplyType,");
            builder.Append("MessageType=@MessageType,");
            builder.Append("IsDisable=@IsDisable,");
            builder.Append("LastEditDate=@LastEditDate,");
            builder.Append("LastEditor=@LastEditor,");
            builder.Append("Content=@Content,");
            builder.Append("Type=@Type,");
            builder.Append("ActivityId=@ActivityId,");
            builder.Append("ArticleID=@ArticleID ");
            builder.Append(" where ReplyId=@ReplyId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, reply.Keys);
            this.database.AddInParameter(sqlStringCommand, "MatchType", DbType.Int32, (int) reply.MatchType);
            this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int) reply.ReplyType);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int) reply.MessageType);
            this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, reply.IsDisable);
            this.database.AddInParameter(sqlStringCommand, "LastEditDate", DbType.DateTime, reply.LastEditDate);
            this.database.AddInParameter(sqlStringCommand, "LastEditor", DbType.String, reply.LastEditor);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, reply.Text);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, 2);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, reply.ActivityId);
            this.database.AddInParameter(sqlStringCommand, "ArticleID", DbType.Int32, reply.ArticleID);
            this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, reply.Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

