﻿namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Store;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public static class EventLogs
    {
        public static bool DeleteAllLogs()
        {
            return new LogDao().DeleteAllLogs();
        }

        public static bool DeleteLog(long logId)
        {
            return new LogDao().DeleteLog(logId);
        }

        public static int DeleteLogs(string strIds)
        {
            return new LogDao().DeleteLogs(strIds);
        }

        public static DbQueryResult GetLogs(OperationLogQuery query)
        {
            return new LogDao().GetLogs(query);
        }

        public static IList<string> GetOperationUseNames()
        {
            return new LogDao().GetOperationUserNames();
        }

        public static void WriteOperationLog(Privilege privilege, string description)
        {
            OperationLogEntry entry = new OperationLogEntry {
                AddedTime = DateTime.Now,
                Privilege = privilege,
                Description = description,
                IpAddress = Globals.IPAddress,
                PageUrl = HttpContext.Current.Request.RawUrl,
                UserName = ManagerHelper.GetCurrentManager().UserName
            };
            new LogDao().WriteOperationLogEntry(entry);
        }
    }
}

