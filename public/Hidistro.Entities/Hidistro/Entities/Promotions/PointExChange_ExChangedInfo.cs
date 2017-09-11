namespace Hidistro.Entities.Promotions
{
    using System;

    public class PointExChange_ExChangedInfo
    {
        private DateTime _date;
        private int _exchangeid;
        private string _exchangename;
        private int _id;
        private string _membergrades = "0";
        private int _memberid;
        private int _pointnumber;
        private int _productid;

        public DateTime Date
        {
            get
            {
                return this._date;
            }
            set
            {
                this._date = value;
            }
        }

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

        public string exChangeName
        {
            get
            {
                return this._exchangename;
            }
            set
            {
                this._exchangename = value;
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

        public int MemberID
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
    }
}

