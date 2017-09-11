namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class PointExchangeProductInfo
    {
        private int _exchangeid;
        private int _pointnumber;
        private int _productid;
        private int _productnumber;
        private int _status;

        public int ChangedNumber { get; set; }

        public int EachMaxNumber { get; set; }

        public int exChangeId
        {
            get
            {
                return this._exchangeid;
            }
            set
            {
                this._exchangeid = value;
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

        public int ProductId
        {
            get
            {
                return this._productid;
            }
            set
            {
                this._productid = value;
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
    }
}

