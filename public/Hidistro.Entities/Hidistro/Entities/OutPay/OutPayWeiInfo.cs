namespace Hidistro.Entities.OutPay
{
    using System;
    using System.Runtime.CompilerServices;

    public class OutPayWeiInfo
    {
        public int Amount { get; set; }

        public string Desc { get; set; }

        public string device_info { get; set; }

        public string Nonce_Str { get; set; }

        public string Openid { get; set; }

        public string Partner_Trade_No { get; set; }

        public string Re_User_Name { get; set; }

        public int Sid { get; set; }

        public string Sign { get; set; }

        public int UserId { get; set; }
    }
}

