namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class VoteInfo
    {
        private string _customgroup;
        private string _defualtgroup;
        private string _description;
        private DateTime _enddate;
        private string _imageurl;
        private bool _isbackup;
        private bool _isMultiCheck;
        private int _maxcheck;
        private string _membergrades;
        private DateTime _startdate;
        private long _voteid;
        private string _votename;

        public string CustomGroup
        {
            get
            {
                return this._customgroup;
            }
            set
            {
                this._customgroup = value;
            }
        }

        public string DefualtGroup
        {
            get
            {
                return this._defualtgroup;
            }
            set
            {
                this._defualtgroup = value;
            }
        }

        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this._enddate;
            }
            set
            {
                this._enddate = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                return this._imageurl;
            }
            set
            {
                this._imageurl = value;
            }
        }

        public bool IsBackup
        {
            get
            {
                return this._isbackup;
            }
            set
            {
                this._isbackup = value;
            }
        }

        public bool IsMultiCheck
        {
            get
            {
                return this._isMultiCheck;
            }
            set
            {
                this._isMultiCheck = value;
            }
        }

        public int MaxCheck
        {
            get
            {
                return this._maxcheck;
            }
            set
            {
                this._maxcheck = value;
            }
        }

        public string MemberGrades
        {
            get
            {
                return this._membergrades;
            }
            set
            {
                this._membergrades = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this._startdate;
            }
            set
            {
                this._startdate = value;
            }
        }

        public int VoteAttends { get; set; }

        public int VoteCounts { get; set; }

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

        public IList<VoteItemInfo> VoteItems { get; set; }

        public string VoteName
        {
            get
            {
                return this._votename;
            }
            set
            {
                this._votename = value;
            }
        }
    }
}

