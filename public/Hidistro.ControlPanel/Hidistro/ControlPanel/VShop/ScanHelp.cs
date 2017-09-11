namespace Hidistro.ControlPanel.VShop
{
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Runtime.InteropServices;

    public static class ScanHelp
    {
        public static bool AddScanInfos(ScanInfos info)
        {
            return new ScanScenceDao().AddScanInfos(info);
        }

        public static bool ClearScanBind(string PlatForm)
        {
            return new ScanScenceDao().ClearScanBind(PlatForm);
        }

        public static bool CreatNewScan(int userId, string Platform = "WX", int type = 0)
        {
            return new ScanScenceDao().getCreatScanId(userId, Platform, type);
        }

        public static int GetMaxScenceId(int type, string Platform)
        {
            return new ScanScenceDao().GetMaxScenceId(type, Platform);
        }

        public static ScanInfos GetScanInfosById(int id)
        {
            return new ScanScenceDao().GetScanInfosById(id);
        }

        public static ScanInfos GetScanInfosByScenceId(string Sceneid)
        {
            return new ScanScenceDao().GetScanInfosByScenceId(Sceneid);
        }

        public static ScanInfos GetScanInfosByTicket(string ticket)
        {
            return new ScanScenceDao().GetScanInfosByTicket(ticket);
        }

        public static ScanInfos GetScanInfosByUserId(int Userid, int type = 0, string Platform = "WX")
        {
            return new ScanScenceDao().GetScanInfosByUserId(Userid, type, Platform);
        }

        public static bool updateScanInfos(ScanInfos info)
        {
            return new ScanScenceDao().updateScanInfos(info);
        }

        public static bool updateScanInfosCodeUrl(ScanInfos info)
        {
            return new ScanScenceDao().updateScanInfosCodeUrl(info);
        }

        public static bool updateScanInfosLastActiveTime(DateTime LastDate, string sceneId)
        {
            ScanInfos info = new ScanInfos {
                LastActiveTime = LastDate,
                Sceneid = sceneId
            };
            return new ScanScenceDao().updateScanInfosLastActiveTime(info);
        }
    }
}

