namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class VoteItemInfo
    {
        private int _itemcount;
        private long _voteid;
        private long _voteitemid;
        private string _voteitemname;

        public int ItemCount
        {
            get
            {
                return this._itemcount;
            }
            set
            {
                this._itemcount = value;
            }
        }

        public decimal Percentage { get; set; }

        public long VoteId
        {
            get
            {
                return this._voteid;
            }
            set
            {
                this._voteid = value;
            }
        }

        public long VoteItemId
        {
            get
            {
                return this._voteitemid;
            }
            set
            {
                this._voteitemid = value;
            }
        }

        public string VoteItemName
        {
            get
            {
                return this._voteitemname;
            }
            set
            {
                this._voteitemname = value;
            }
        }
    }
}

