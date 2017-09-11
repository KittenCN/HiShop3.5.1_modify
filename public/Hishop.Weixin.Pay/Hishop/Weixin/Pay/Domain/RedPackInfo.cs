namespace Hishop.Weixin.Pay.Domain
{
    using Newtonsoft.Json;
    using System;
    using System.Runtime.CompilerServices;

    public class RedPackInfo
    {
        public redPackStatus Getstatus()
        {
            string str = this.status;
            redPackStatus status = redPackStatus.异常;
            string str2 = str;
            if (str2 == null)
            {
                return status;
            }
            if (!(str2 == "SENDING"))
            {
                if (str2 != "SENT")
                {
                    if (str2 == "FAILED")
                    {
                        return redPackStatus.发放失败;
                    }
                    if (str2 == "RECEIVED")
                    {
                        return redPackStatus.已领取;
                    }
                    if (str2 != "REFUND")
                    {
                        return status;
                    }
                    return redPackStatus.已退款;
                }
            }
            else
            {
                return redPackStatus.发放中;
            }
            return redPackStatus.已发放待领取;
        }

        public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string detail_id { get; set; }

        public string err_code { get; set; }

        public string mch_billno { get; set; }

        public string mch_id { get; set; }

        public string openid { get; set; }

        public DateTime? refund_time { get; set; }

        public string result_code { get; set; }

        public string return_code { get; set; }

        public string return_msg { get; set; }

        public DateTime send_time { get; set; }

        public string send_type { get; set; }

        public string sign { get; set; }

        public string status { get; set; }
    }
}

