namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class MemberCouponsInfo
    {
        private DateTime? _begindate;
        private decimal? _conditionvalue;
        private int _couponid;
        private string _couponname;
        private DateTime? _enddate;
        private int _id;
        private int? _memberid;
        private string _orderno;
        private DateTime? _receivedate;
        private int? _status;
        private DateTime? _useddate;

        public MemberCouponsInfo()
        {
        }

        public MemberCouponsInfo(int couponId)
        {
            this._couponid = couponId;
        }

        public DateTime? BeginDate
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

        public decimal? ConditionValue
        {
            get
            {
                return this._conditionvalue;
            }
            set
            {
                this._conditionvalue = value;
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

        public string CouponName
        {
            get
            {
                return this._couponname;
            }
            set
            {
                this._couponname = value;
            }
        }

        public decimal CouponValue { get; set; }

        public DateTime? EndDate
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

        public bool IsAllProduct { get; set; }

        public int? MemberId
        {
            get
            {
                return this._memberid;
            }
            set
            {
                this._memberid = value;
            }
        }

        public string OrderNo
        {
            get
            {
                return this._orderno;
            }
            set
            {
                this._orderno = value;
            }
        }

        public DateTime? ReceiveDate
        {
            get
            {
                return this._receivedate;
            }
            set
            {
                this._receivedate = value;
            }
        }

        public int? Status
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

        public DateTime? UsedDate
        {
            get
            {
                return this._useddate;
            }
            set
            {
                this._useddate = value;
            }
        }
    }
}

