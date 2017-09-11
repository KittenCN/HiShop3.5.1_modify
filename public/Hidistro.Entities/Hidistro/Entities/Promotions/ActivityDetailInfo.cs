namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class ActivityDetailInfo
    {
        private int _activitiesid;
        private bool _bfreeshipping;
        private int _couponid;
        private int _id;
        private int _integral;
        private decimal _meetmoney;
        private decimal _reductionmoney;

        public int ActivitiesId
        {
            get
            {
                return this._activitiesid;
            }
            set
            {
                this._activitiesid = value;
            }
        }

        public bool bFreeShipping
        {
            get
            {
                return this._bfreeshipping;
            }
            set
            {
                this._bfreeshipping = value;
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

        public int Integral
        {
            get
            {
                return this._integral;
            }
            set
            {
                this._integral = value;
            }
        }

        public decimal MeetMoney
        {
            get
            {
                return this._meetmoney;
            }
            set
            {
                this._meetmoney = value;
            }
        }

        public int MeetNumber { get; set; }

        public decimal ReductionMoney
        {
            get
            {
                return this._reductionmoney;
            }
            set
            {
                this._reductionmoney = value;
            }
        }
    }
}

