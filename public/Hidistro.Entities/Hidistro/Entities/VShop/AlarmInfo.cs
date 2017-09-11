namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class AlarmInfo
    {
        public AlarmInfo()
        {
            this.TimeStamp = DateTime.Now;
        }

        public string AlarmContent { get; set; }

        public int AlarmNotifyId { get; set; }

        public string AppId { get; set; }

        public string Description { get; set; }

        public int ErrorType { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}

