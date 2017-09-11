namespace Hidistro.ControlPanel.Bargain
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Bargain;
    using Hidistro.SqlDal.Bargain;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class BargainHelper
    {
        public static bool ActionIsEnd(int bargainDetialId)
        {
            return new BargainDao().ActionIsEnd(bargainDetialId);
        }

        public static bool DeleteBargainById(string ids)
        {
            return new BargainDao().DeleteBargainById(ids);
        }

        public static bool ExistsHelpBargainDetial(HelpBargainDetialInfo helpBargainDetial)
        {
            return new BargainDao().ExistsHelpBargainDetial(helpBargainDetial);
        }

        public static HelpBargainDetialInfo GeHelpBargainDetialInfo(int bargainDetialId, int userId)
        {
            return new BargainDao().GeHelpBargainDetialInfo(bargainDetialId, userId);
        }

        public static DataTable GetAllBargain()
        {
            return new BargainDao().GetAllBargain();
        }

        public static DataTable GetBargainById(string ids)
        {
            return new BargainDao().GetBargainById(ids);
        }

        public static BargainDetialInfo GetBargainDetialInfo(int id)
        {
            return new BargainDao().GetBargainDetialInfo(id);
        }

        public static BargainDetialInfo GetBargainDetialInfo(int bargainId, int userId)
        {
            return new BargainDao().GetBargainDetialInfo(bargainId, userId);
        }

        public static BargainInfo GetBargainInfo(int id)
        {
            return new BargainDao().GetBargainInfo(id);
        }

        public static BargainInfo GetBargainInfoByDetialId(int bargainDetialId)
        {
            return new BargainDao().GetBargainInfoByDetialId(bargainDetialId);
        }

        public static DbQueryResult GetBargainList(BargainQuery query)
        {
            return new BargainDao().GetBargainList(query);
        }

        public static BargainStatisticalData GetBargainStatisticalDataInfo(int bargainId)
        {
            return new BargainDao().GetBargainStatisticalDataInfo(bargainId);
        }

        public static string GetDay(object hour)
        {
            int num = (int) hour;
            if (num < 0x18)
            {
                return "即将结束";
            }
            int num2 = ((num % 0x18) > 0) ? ((num / 0x18) + 1) : (num / 0x18);
            return ("还剩：" + num2.ToString() + "天");
        }

        public static int GetHelpBargainDetialCount(int bargainDetialId)
        {
            return new BargainDao().GetHelpBargainDetialCount(bargainDetialId);
        }

        public static DataTable GetHelpBargainDetials(int bargainDetialId)
        {
            return new BargainDao().GetHelpBargainDetials(bargainDetialId);
        }

        public static string GetLinkHtml(string id, string status, string stockNumber = "0", string tranNumber = "0")
        {
            StringBuilder builder = new StringBuilder();
            string str = status;
            if (str != null)
            {
                if (!(str == "进行中"))
                {
                    if (str == "未开始")
                    {
                        builder.Append("<a class='btn btn-danger btn-xs end' href='javascript:void(0)'>未开始</a>");
                    }
                    else if (str == "已结束")
                    {
                        builder.Append("<a class='btn btn-danger btn-xs end' href='javascript:void(0)'>已结束</a>");
                    }
                }
                else if (Globals.ToNum(stockNumber) <= Globals.ToNum(tranNumber))
                {
                    builder.Append("<a class='btn btn-danger btn-xs end' href='javascript:void(0)'>卖光了</a>");
                }
                else
                {
                    builder.Append("<a class='btn btn-danger btn-xs' href='BargainDetial.aspx?id=" + id + "'>马上参与</a>");
                }
            }
            return builder.ToString();
        }

        public static DbQueryResult GetMyBargainList(BargainQuery query)
        {
            return new BargainDao().GetMyBargainList(query);
        }

        public static int GetTotal(BargainQuery query)
        {
            return new BargainDao().GetTotal(query);
        }

        public static int HelpBargainCount(int bargainId)
        {
            return new BargainDao().HelpBargainCount(bargainId);
        }

        public static bool InsertBargain(BargainInfo bargain)
        {
            return new BargainDao().InsertBargain(bargain);
        }

        public static bool InsertBargainDetial(BargainDetialInfo bargainDetial, out int bargainDetialId)
        {
            return new BargainDao().InsertBargainDetial(bargainDetial, out bargainDetialId);
        }

        public static string InsertHelpBargainDetial(HelpBargainDetialInfo helpBargainDetial)
        {
            string str2 = new BargainDao().IsCanBuyByBarginId(helpBargainDetial.BargainId);
            if (str2 != "1")
            {
                return str2;
            }
            if (new BargainDao().InsertHelpBargainDetial(helpBargainDetial))
            {
                new BargainDao().UpdateBargainDetial(helpBargainDetial);
            }
            return "1";
        }

        public static string IsCanBuyByBarginDetailId(int bargainDetailId)
        {
            return new BargainDao().IsCanBuyByBarginDetailId(bargainDetailId);
        }

        public static string IsCanBuyByBarginId(int bargainId)
        {
            return new BargainDao().IsCanBuyByBarginId(bargainId);
        }

        public static bool UpdateBargain(BargainInfo bargain)
        {
            return new BargainDao().UpdateBargain(bargain);
        }

        public static bool UpdateBargain(int bargainId, DateTime endDate)
        {
            return new BargainDao().UpdateBargain(bargainId, endDate);
        }

        public static bool UpdateBargainDetial(HelpBargainDetialInfo helpBargainDetial)
        {
            return new BargainDao().UpdateBargainDetial(helpBargainDetial);
        }

        public static bool UpdateNumberById(int bargainDetialId, int number, out int relNumber)
        {
            bool flag = false;
            relNumber = number;
            BargainInfo bargainInfoByDetialId = GetBargainInfoByDetialId(bargainDetialId);
            if (bargainInfoByDetialId != null)
            {
                int purchaseNumber = bargainInfoByDetialId.PurchaseNumber;
                int num2 = bargainInfoByDetialId.ActivityStock - bargainInfoByDetialId.TranNumber;
                if (num2 < relNumber)
                {
                    relNumber = num2;
                }
                if (purchaseNumber < relNumber)
                {
                    relNumber = purchaseNumber;
                }
                if (relNumber > 0)
                {
                    flag = new BargainDao().UpdateNumberById(bargainDetialId, relNumber);
                }
            }
            return flag;
        }
    }
}

