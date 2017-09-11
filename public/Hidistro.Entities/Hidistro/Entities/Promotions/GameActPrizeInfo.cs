namespace Hidistro.Entities.Promotions
{
    using System;

    public class GameActPrizeInfo
    {
        private int _couponnumber;
        private decimal _couponrate;
        private int _gameid;
        private int _givecouponid;
        private int _giveproductid;
        private int _grivepoint;
        private int _id;
        private int _pointnumber;
        private decimal _pointrate;
        private string _prizename;
        private ePrizeType _prizetype = ePrizeType.point;
        private int _productnumber;
        private decimal _productrate;
        private int _sort;

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

        public decimal CouponRate
        {
            get
            {
                return this._couponrate;
            }
            set
            {
                this._couponrate = value;
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

        public int GiveCouponId
        {
            get
            {
                return this._givecouponid;
            }
            set
            {
                this._givecouponid = value;
            }
        }

        public int GiveProductId
        {
            get
            {
                return this._giveproductid;
            }
            set
            {
                this._giveproductid = value;
            }
        }

        public int GrivePoint
        {
            get
            {
                return this._grivepoint;
            }
            set
            {
                this._grivepoint = value;
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

        public int PointNumber
        {
            get
            {
                return this._pointnumber;
            }
            set
            {
                this._pointnumber = value;
            }
        }

        public decimal PointRate
        {
            get
            {
                return this._pointrate;
            }
            set
            {
                this._pointrate = value;
            }
        }

        public string PrizeName
        {
            get
            {
                return this._prizename;
            }
            set
            {
                this._prizename = value;
            }
        }

        public ePrizeType PrizeType
        {
            get
            {
                return this._prizetype;
            }
            set
            {
                this._prizetype = value;
            }
        }

        public int ProductNumber
        {
            get
            {
                return this._productnumber;
            }
            set
            {
                this._productnumber = value;
            }
        }

        public decimal ProductRate
        {
            get
            {
                return this._productrate;
            }
            set
            {
                this._productrate = value;
            }
        }

        public int sort
        {
            get
            {
                return this._sort;
            }
            set
            {
                this._sort = value;
            }
        }
    }
}

