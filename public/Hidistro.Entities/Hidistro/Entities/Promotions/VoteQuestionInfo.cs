namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class VoteQuestionInfo
    {
        private long _questionid;
        private string _title;
        private long _voteid;

        public IList<VoteItemInfo> Items { get; set; }

        public int MaxCheck { get; set; }

        public long QuestionId
        {
            get
            {
                return this._questionid;
            }
            set
            {
                this._questionid = value;
            }
        }

        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

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
    }
}

