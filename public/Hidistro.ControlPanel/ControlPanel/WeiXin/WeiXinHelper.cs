namespace ControlPanel.WeiXin
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.WeiXin;
    using Hidistro.SqlDal.Weibo;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public class WeiXinHelper
    {
        public static Dictionary<string, string> BindAdminOpenId = new Dictionary<string, string>();

        public static string ClearWeiXinMediaID()
        {
            return new SendAllDao().ClearWeiXinMediaID();
        }

        public static bool DeleteOldQRCode(string AppID)
        {
            return new SendAllDao().DeleteOldQRCode(AppID);
        }

        public static bool DelOldSendAllList()
        {
            return new SendAllDao().DelOldSendAllList();
        }

        public static int getAlypayUserNum()
        {
            return new SendAllDao().getAlypayUserNum();
        }

        public static bool GetQRCodeScanInfo(string AppID, bool IsClearAfterRead, out string SCannerUserOpenID, out string SCannerUserNickName, out string UserHead)
        {
            return new SendAllDao().GetQRCodeScanInfo(AppID, IsClearAfterRead, out SCannerUserOpenID, out SCannerUserNickName, out UserHead);
        }

        public static DataTable GetRencentAliOpenID()
        {
            return new SendAllDao().GetRencentAliOpenID();
        }

        public static DataTable GetRencentOpenID(int topnum)
        {
            return new SendAllDao().GetRencentOpenID(topnum);
        }

        public static SendAllInfo GetSendAllInfo(int sendID)
        {
            return new SendAllDao().GetSendAllInfo(sendID);
        }

        public static DbQueryResult GetSendAllRequest(SendAllQuery query, int platform = 0)
        {
            return new SendAllDao().GetSendAllRequest(query, platform);
        }

        public static bool SaveQRCodeScanInfo(string AppID, string SCannerUserOpenID, string SCannerUserNickName)
        {
            return new SendAllDao().SaveQRCodeScanInfo(AppID, SCannerUserOpenID, SCannerUserNickName);
        }

        public static string SaveSendAllInfo(SendAllInfo sendAllInfo, int platform = 0)
        {
            return new SendAllDao().SaveSendAllInfo(sendAllInfo, platform);
        }

        public static bool UpdateAddSendCount(int id, int addcount, int SendState = -1)
        {
            return new SendAllDao().UpdateAddSendCount(id, addcount, SendState);
        }

        public static bool UpdateMsgId(int id, string msgid, int sendstate, int sendcount, int totalcount, string returnjsondata)
        {
            return new SendAllDao().UpdateMsgId(id, msgid, sendstate, sendcount, totalcount, returnjsondata);
        }

        public static int UpdateRencentAliOpenID(string openid)
        {
            return new SendAllDao().UpdateRencentAliOpenID(openid);
        }

        public static int UpdateRencentOpenID(string openid)
        {
            return new SendAllDao().UpdateRencentOpenID(openid);
        }
    }
}

