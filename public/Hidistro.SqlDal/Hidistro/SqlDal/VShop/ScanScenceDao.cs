namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Entities;
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;

    public class ScanScenceDao
    {
        private static object CreatLockObj = new object();
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddScanInfos(ScanInfos info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into Vshop_ScanSceneInfos(Platform,Sceneid,BindUserId,DescInfo,type,CreateTime, CodeUrl,LastActiveTime)\r\n            VALUES(@Platform,@Sceneid,@BindUserId,@DescInfo,@type,@CreateTime, @CodeUrl,@LastActiveTime)");
            this.database.AddInParameter(sqlStringCommand, "BindUserId", DbType.Int32, info.BindUserId);
            this.database.AddInParameter(sqlStringCommand, "Sceneid", DbType.String, info.Sceneid);
            this.database.AddInParameter(sqlStringCommand, "Platform", DbType.String, info.Platform);
            this.database.AddInParameter(sqlStringCommand, "type", DbType.Int16, info.type);
            this.database.AddInParameter(sqlStringCommand, "DescInfo", DbType.String, info.DescInfo);
            this.database.AddInParameter(sqlStringCommand, "CodeUrl", DbType.String, info.CodeUrl);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.Date, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "LastActiveTime", DbType.Date, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool ClearScanBind(string Platform)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from Vshop_ScanSceneInfos where Platform=@Platform");
            this.database.AddInParameter(sqlStringCommand, "Platform", DbType.String, Platform);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool getCreatScanId(int userId, string Platform = "WX", int type = 0)
        {
            lock (CreatLockObj)
            {
                int num = this.GetMaxScenceId(type, Platform) + 1;
                ScanInfos info = new ScanInfos {
                    BindUserId = userId,
                    Sceneid = num.ToString(),
                    CreateTime = DateTime.Now,
                    DescInfo = "分销商关注公众号",
                    Platform = Platform,
                    type = type,
                    LastActiveTime = DateTime.Now,
                    CodeUrl = ""
                };
                return this.AddScanInfos(info);
            }
        }

        public int GetMaxScenceId(int type, string Platform)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT max(Sceneid*1) FROM Vshop_ScanSceneInfos WHERE type=@type and  Platform=@Platform");
            this.database.AddInParameter(sqlStringCommand, "type", DbType.Int32, type);
            this.database.AddInParameter(sqlStringCommand, "Platform", DbType.String, Platform);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                num = Convert.ToInt32(obj2);
            }
            if (num == 0)
            {
                num = 1;
            }
            return num;
        }

        public ScanInfos GetScanInfosById(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Vshop_ScanSceneInfos WHERE Id={0}", id));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ScanInfos>(reader);
            }
        }

        public ScanInfos GetScanInfosByScenceId(string Sceneid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Vshop_ScanSceneInfos WHERE Sceneid='{0}'", Sceneid));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ScanInfos>(reader);
            }
        }

        public ScanInfos GetScanInfosByTicket(string Ticket)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Vshop_ScanSceneInfos WHERE CodeUrl=@CodeUrl");
            this.database.AddInParameter(sqlStringCommand, "CodeUrl", DbType.String, Ticket);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ScanInfos>(reader);
            }
        }

        public ScanInfos GetScanInfosByUserId(int Userid, int type = 0, string Platform = "WX")
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Vshop_ScanSceneInfos WHERE Platform=@Platform and BindUserId=@BindUserId and type=@type");
            this.database.AddInParameter(sqlStringCommand, "BindUserId", DbType.Int32, Userid);
            this.database.AddInParameter(sqlStringCommand, "type", DbType.Int16, type);
            this.database.AddInParameter(sqlStringCommand, "Platform", DbType.String, Platform);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ScanInfos>(reader);
            }
        }

        public bool updateScanInfos(ScanInfos info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Vshop_ScanSceneInfos set Platform=@Platform,Sceneid=@Sceneid,BindUserId=@BindUserId,\r\n             DescInfo=@DescInfo,type=@type,CreateTime=@CreateTime,LastActiveTime=@LastActiveTime, CodeUrl=@CodeUrl where Id=@Id");
            this.database.AddInParameter(sqlStringCommand, "BindUserId", DbType.Int32, info.BindUserId);
            this.database.AddInParameter(sqlStringCommand, "Sceneid", DbType.String, info.Sceneid);
            this.database.AddInParameter(sqlStringCommand, "Platform", DbType.String, info.Platform);
            this.database.AddInParameter(sqlStringCommand, "type", DbType.Int16, info.type);
            this.database.AddInParameter(sqlStringCommand, "DescInfo", DbType.String, info.DescInfo);
            this.database.AddInParameter(sqlStringCommand, "CodeUrl", DbType.String, info.CodeUrl);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.Date, info.CreateTime);
            this.database.AddInParameter(sqlStringCommand, "LastActiveTime", DbType.Date, info.LastActiveTime);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, info.id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool updateScanInfosCodeUrl(ScanInfos info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Vshop_ScanSceneInfos set CreateTime=@CreateTime,LastActiveTime=@LastActiveTime,CodeUrl=@CodeUrl where Id=@Id");
            this.database.AddInParameter(sqlStringCommand, "CodeUrl", DbType.String, info.CodeUrl);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.Date, info.CreateTime);
            this.database.AddInParameter(sqlStringCommand, "LastActiveTime", DbType.Date, info.LastActiveTime);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, info.id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool updateScanInfosLastActiveTime(ScanInfos info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Vshop_ScanSceneInfos set LastActiveTime=@LastActiveTime where Sceneid=@Sceneid");
            this.database.AddInParameter(sqlStringCommand, "Sceneid", DbType.String, info.Sceneid);
            this.database.AddInParameter(sqlStringCommand, "LastActiveTime", DbType.Date, info.LastActiveTime);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

