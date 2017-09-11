namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class UserSign
    {
        public UserSign()
        {
            this.SignDay = DateTime.Now;
        }

        public int Continued { get; set; }

        public int ID { get; set; }

        public DateTime SignDay { get; set; }

        public int Stage { get; set; }

        public int UserID { get; set; }
    }
}

