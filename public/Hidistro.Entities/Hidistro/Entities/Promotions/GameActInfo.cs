namespace Hidistro.Entities.Promotions
{
    using System;

    public class GameActInfo
    {
        private int _attendtimes;
        private DateTime _begindate;
        private bool _bonlynotwinner;
        private int _createstep = 1;
        private string _decription;
        private DateTime _enddate;
        private int _gameid;
        private string _gamename;
        private eGameType _gametype;
        private int _givepoint;
        private string _imgurl = "0";
        private string _membergrades = "0";
        private int _status;
        private string _unwindecrip;
        private int _usepoint;

        public int attendTimes
        {
            get
            {
                return this._attendtimes;
            }
            set
            {
                this._attendtimes = value;
            }
        }

        public DateTime BeginDate
        {
            get
            {
                return this._begindate;
            }
            set
            {
                this._begindate = value;
            }
        }

        public bool bOnlyNotWinner
        {
            get
            {
                return this._bonlynotwinner;
            }
            set
            {
                this._bonlynotwinner = value;
            }
        }

        public int CreateStep
        {
            get
            {
                return this._createstep;
            }
            set
            {
                this._createstep = value;
            }
        }

        public string Decription
        {
            get
            {
                return this._decription;
            }
            set
            {
                this._decription = value;
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

        public int GameId
        {
            get
            {
                return this._gameid;
            }
            set
            {
                this._gameid = value;
            }
        }

        public string GameName
        {
            get
            {
                return this._gamename;
            }
            set
            {
                this._gamename = value;
            }
        }

        public eGameType GameType
        {
            get
            {
                return this._gametype;
            }
            set
            {
                this._gametype = value;
            }
        }

        public int GivePoint
        {
            get
            {
                return this._givepoint;
            }
            set
            {
                this._givepoint = value;
            }
        }

        public string ImgUrl
        {
            get
            {
                return this._imgurl;
            }
            set
            {
                this._imgurl = value;
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

        public int status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
            }
        }

        public string unWinDecrip
        {
            get
            {
                return this._unwindecrip;
            }
            set
            {
                this._unwindecrip = value;
            }
        }

        public int usePoint
        {
            get
            {
                return this._usepoint;
            }
            set
            {
                this._usepoint = value;
            }
        }
    }
}

