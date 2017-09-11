namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Collections.Generic;

    public class AliFuwuReplyHelper
    {
        public static bool DeleteReply(int id)
        {
            return new AliFuwuReplyDao().DeleteReply(id);
        }

        public static IList<ReplyInfo> GetAllFuwuReply()
        {
            return new AliFuwuReplyDao().GetAllReply();
        }

        public static IList<ReplyInfo> GetAllReply()
        {
            return new AliFuwuReplyDao().GetAllReply();
        }

        public static int GetArticleIDByOldArticle(int replyid, MessageType msgtype)
        {
            return new AliFuwuReplyDao().GetArticleIDByOldArticle(replyid, msgtype);
        }

        public static ReplyInfo GetMismatchReply()
        {
            IList<ReplyInfo> replies = new AliFuwuReplyDao().GetReplies(ReplyType.NoMatch);
            if ((replies != null) && (replies.Count > 0))
            {
                return replies[0];
            }
            return null;
        }

        public static int GetNoMatchReplyID(int compareid)
        {
            return new AliFuwuReplyDao().GetNoMatchReplyID(compareid);
        }

        public static IList<ReplyInfo> GetReplies(ReplyType type)
        {
            return new AliFuwuReplyDao().GetReplies(type);
        }

        public static ReplyInfo GetReply(int id)
        {
            return new AliFuwuReplyDao().GetReply(id);
        }

        public static int GetSubscribeID(int compareid)
        {
            return new AliFuwuReplyDao().GetSubscribeID(compareid);
        }

        public static ReplyInfo GetSubscribeReply()
        {
            IList<ReplyInfo> replies = new AliFuwuReplyDao().GetReplies(ReplyType.Subscribe);
            if ((replies != null) && (replies.Count > 0))
            {
                return replies[0];
            }
            return null;
        }

        public static bool HasReplyKey(string key)
        {
            return new AliFuwuReplyDao().HasReplyKey(key);
        }

        public static bool HasReplyKey(string key, int replyid)
        {
            return new AliFuwuReplyDao().HasReplyKey(key, replyid);
        }

        public static bool SaveReply(ReplyInfo reply)
        {
            reply.LastEditDate = DateTime.Now;
            reply.LastEditor = ManagerHelper.GetCurrentManager().UserName;
            return new AliFuwuReplyDao().SaveReply(reply);
        }

        public static bool UpdateReply(ReplyInfo reply)
        {
            reply.LastEditDate = DateTime.Now;
            reply.LastEditor = ManagerHelper.GetCurrentManager().UserName;
            return new AliFuwuReplyDao().UpdateReply(reply);
        }

        public static bool UpdateReplyRelease(int id)
        {
            return new AliFuwuReplyDao().UpdateReplyRelease(id);
        }
    }
}

