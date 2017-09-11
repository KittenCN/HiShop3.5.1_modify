namespace ControlPanel.Promotions
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Promotions;
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;

    public class VoteHelper
    {
        private static VoteDao _vote = new VoteDao();

        public static long Create(VoteInfo vote)
        {
            return _vote.CreateVote(vote);
        }

        public static bool Delete(long Id)
        {
            return (_vote.DeleteVote(Id) > 0);
        }

        public static VoteInfo GetVote(long Id)
        {
            return _vote.GetVoteById(Id);
        }

        public static int GetVoteAttends(long voteId)
        {
            return _vote.GetVoteAttends(voteId);
        }

        public static int GetVoteCounts(long voteId)
        {
            return _vote.GetVoteCounts(voteId);
        }

        public static bool IsVote(int voteId)
        {
            return new VoteDao().IsVote(voteId);
        }

        public static DbQueryResult Query(VoteSearch query)
        {
            return _vote.Query(query);
        }

        public static bool Update(VoteInfo vote, bool isUpdateItems = true)
        {
            if (isUpdateItems)
            {
                return _vote.UpdateVoteAll(vote);
            }
            return _vote.UpdateVote(vote);
        }

        public static bool Vote(int voteId, string itemIds)
        {
            if (IsVote(voteId))
            {
                throw new Exception("已投过票！");
            }
            VoteInfo vote = GetVote((long) voteId);
            if ((vote.IsMultiCheck && (vote.MaxCheck > 0)) && (vote.MaxCheck < itemIds.Split(new char[] { ',' }).Count<string>()))
            {
                throw new Exception(string.Format("对不起！您最多能选{0}项...", vote.MaxCheck));
            }
            return new VoteDao().Vote(voteId, itemIds);
        }
    }
}

