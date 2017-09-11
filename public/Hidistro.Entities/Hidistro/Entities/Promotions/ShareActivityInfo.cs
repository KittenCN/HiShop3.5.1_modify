namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class ShareActivityInfo
    {
        private string _ActivityName;
        private DateTime _begindate;
        private int _couponid;
        private int _couponnumber = 1;
        private string _Description;
        private DateTime _enddate;
        private int _id;
        private string _ImgUrl;
        private decimal _meetvalue;
        private string _ShareTitle;

        public string ActivityName
        {
            get
            {
                return this._ActivityName;
            }
            set
            {
                this._ActivityName = value;
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

        public int CouponId
        {
            get
            {
                return this._couponid;
            }
            set
            {
                this._couponid = value;
            }
        }

        public string CouponName { get; set; }

        public int CouponNumber
        {
            get
            {
                return this._couponnumber;
            }
            set
            {
                this._couponnumber = value;
            }
        }

        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this._Description = value;
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

        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public string ImgUrl
        {
            get
            {
                return this._ImgUrl;
            }
            set
            {
                this._ImgUrl = value;
            }
        }

        public decimal MeetValue
        {
            get
            {
                return this._meetvalue;
            }
            set
            {
                this._meetvalue = value;
            }
        }

        public string ShareTitle
        {
            get
            {
                return this._ShareTitle;
            }
            set
            {
                this._ShareTitle = value;
            }
        }
    }
}

