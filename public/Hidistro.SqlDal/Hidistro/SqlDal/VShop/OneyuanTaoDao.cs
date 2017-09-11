namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public class OneyuanTaoDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        public static object LuckNumObj = new object();

        public bool AddInitParticipantInfo(int creatNum = 60)
        {
            StringBuilder builder = new StringBuilder("INSERT INTO Vshop_OneyuanTao_ParticipantMember(Pid,UserId,BuyTime,BuyNum,IsPay,ActivityId,TotalPrice,SkuId,SkuIdStr,ProductPrice)VALUES");
            builder.Append("(@Pid,@UserId,@BuyTime,@BuyNum,@IsPay,@ActivityId,@TotalPrice,@SkuId,@SkuIdStr,@ProductPrice)");
            Random random = new Random();
            List<DateTime> list = new List<DateTime>();
            for (int i = 0; i < creatNum; i++)
            {
                DateTime item = DateTime.Now.AddDays(-3.0).AddHours((double) random.Next(0x48)).AddMilliseconds((double) random.Next(0x3e7));
                while (list.Contains(item))
                {
                    item = item.AddMilliseconds((double) random.Next(0x3e7));
                }
                list.Add(item);
            }
            list.Sort();
            int num2 = 0;
            foreach (DateTime time2 in list)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
                this.database.AddInParameter(sqlStringCommand, "Pid", DbType.String, "B" + time2.ToString("yyMMddHHmmssfff"));
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, "0");
                this.database.AddInParameter(sqlStringCommand, "BuyTime", DbType.DateTime, time2);
                this.database.AddInParameter(sqlStringCommand, "BuyNum", DbType.Int32, 1);
                this.database.AddInParameter(sqlStringCommand, "IsPay", DbType.Boolean, true);
                this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, "A0");
                this.database.AddInParameter(sqlStringCommand, "TotalPrice", DbType.Decimal, 1);
                this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, "");
                this.database.AddInParameter(sqlStringCommand, "SkuIdStr", DbType.String, "");
                this.database.AddInParameter(sqlStringCommand, "ProductPrice", DbType.Decimal, 1);
                this.database.ExecuteNonQuery(sqlStringCommand);
                num2++;
            }
            return true;
        }

        public bool AddLuckInfo(LuckInfo info)
        {
            StringBuilder builder = new StringBuilder("INSERT INTO Vshop_OneyuanTao_WinningRecord(UserId,ActivityId,Pid,PrizeNum,IsWin)VALUES");
            builder.Append("(@UserId,@ActivityId,@Pid,@PrizeNum,@IsWin)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, info.ActivityId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Boolean, info.UserId);
            this.database.AddInParameter(sqlStringCommand, "Pid", DbType.String, info.Pid);
            this.database.AddInParameter(sqlStringCommand, "PrizeNum", DbType.String, info.PrizeNum);
            this.database.AddInParameter(sqlStringCommand, "IsWin", DbType.Boolean, false);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool AddLuckInfo(List<LuckInfo> infoList)
        {
            if (infoList.Count == 0)
            {
                return false;
            }
            StringBuilder builder = new StringBuilder();
            foreach (LuckInfo info in infoList)
            {
                builder.AppendLine("INSERT INTO Vshop_OneyuanTao_WinningRecord(UserId,ActivityId,Pid,PrizeNum,IsWin)");
                builder.AppendLine(string.Concat(new object[] { "select  ", info.UserId, ",'", info.ActivityId, "','", info.Pid, "','", info.PrizeNum, "',0 ;" }));
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool AddOneyuanTao(OneyuanTaoInfo info)
        {
            StringBuilder builder = new StringBuilder("INSERT INTO Vshop_OneyuanTao_Detail(ActivityId,IsOn,Title,StartTime,EndTime,HeadImgage,ReachType,\r\n            ActivityDec,ProductId,ProductPrice,ProductImg,ProductTitle,SkuId,PrizeNumber,EachPrice\r\n           ,EachCanBuyNum,FitMember,DefualtGroup,CustomGroup,ReachNum,FinishedNum)VALUES");
            builder.Append("(@ActivityId,@IsOn,@Title,@StartTime,@EndTime,@HeadImgage,@ReachType,@ActivityDec,@ProductId,");
            builder.Append("@ProductPrice,@ProductImg,@ProductTitle,@SkuId,@PrizeNumber,@EachPrice,@EachCanBuyNum,");
            builder.Append("@FitMember,@DefualtGroup,@CustomGroup,@ReachNum,@FinishedNum)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, info.ActivityId);
            this.database.AddInParameter(sqlStringCommand, "IsOn", DbType.Boolean, info.IsOn);
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, info.Title);
            this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, info.StartTime);
            this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, info.EndTime);
            this.database.AddInParameter(sqlStringCommand, "HeadImgage", DbType.String, info.HeadImgage);
            this.database.AddInParameter(sqlStringCommand, "ActivityDec", DbType.String, info.ActivityDec);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, info.ProductId);
            this.database.AddInParameter(sqlStringCommand, "ProductPrice", DbType.Decimal, info.ProductPrice);
            this.database.AddInParameter(sqlStringCommand, "ProductImg", DbType.String, info.ProductImg);
            this.database.AddInParameter(sqlStringCommand, "ProductTitle", DbType.String, info.ProductTitle);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, info.SkuId);
            this.database.AddInParameter(sqlStringCommand, "PrizeNumber", DbType.Int32, info.PrizeNumber);
            this.database.AddInParameter(sqlStringCommand, "EachCanBuyNum", DbType.Int32, info.EachCanBuyNum);
            this.database.AddInParameter(sqlStringCommand, "FitMember", DbType.String, info.FitMember);
            this.database.AddInParameter(sqlStringCommand, "DefualtGroup", DbType.String, info.DefualtGroup);
            this.database.AddInParameter(sqlStringCommand, "CustomGroup", DbType.String, info.CustomGroup);
            this.database.AddInParameter(sqlStringCommand, "ReachNum", DbType.Int32, info.ReachNum);
            this.database.AddInParameter(sqlStringCommand, "FinishedNum", DbType.Int32, info.FinishedNum);
            this.database.AddInParameter(sqlStringCommand, "EachPrice", DbType.Decimal, info.EachPrice);
            this.database.AddInParameter(sqlStringCommand, "ReachType", DbType.Int32, info.ReachType);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool AddParticipant(OneyuanTaoParticipantInfo info)
        {
            StringBuilder builder = new StringBuilder("INSERT INTO Vshop_OneyuanTao_ParticipantMember(Pid,UserId,BuyTime,BuyNum,IsPay,ActivityId,TotalPrice,SkuId,SkuIdStr,ProductPrice)VALUES");
            builder.Append("(@Pid,@UserId,@BuyTime,@BuyNum,@IsPay,@ActivityId,@TotalPrice,@SkuId,@SkuIdStr,@ProductPrice)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Pid", DbType.String, info.Pid);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, info.UserId);
            this.database.AddInParameter(sqlStringCommand, "BuyTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "BuyNum", DbType.Int32, info.BuyNum);
            this.database.AddInParameter(sqlStringCommand, "IsPay", DbType.Boolean, false);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, info.ActivityId);
            this.database.AddInParameter(sqlStringCommand, "TotalPrice", DbType.Decimal, info.TotalPrice);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, info.SkuId);
            this.database.AddInParameter(sqlStringCommand, "SkuIdStr", DbType.String, info.SkuIdStr);
            this.database.AddInParameter(sqlStringCommand, "ProductPrice", DbType.Decimal, info.ProductPrice);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int BatchDeleteOneyuanTao(string[] ActivityIds)
        {
            ActivityIds = this.ChangeJoinStringArray(ActivityIds);
            string query = "delete from Vshop_OneyuanTao_Detail where FinishedNum=0 and ActivityId in(" + string.Join(",", ActivityIds) + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public int BatchSetOneyuanTaoIsOn(string[] ActivityIds, bool IsOn)
        {
            bool flag = false;
            ActivityIds = this.ChangeJoinStringArray(ActivityIds);
            string str = string.Join(",", ActivityIds);
            string str2 = "Update  Vshop_OneyuanTao_Detail set IsOn=@IsOn,Isend=@Isend where Isend=0 and ActivityId in(" + str + ")";
            if (!IsOn)
            {
                flag = true;
            }
            else
            {
                str2 = "Update  Vshop_OneyuanTao_Detail set StartTime=GETDATE() where  Isend=0 \r\n                       and StartTime>GETDATE() and ActivityId in(" + str + ")";
                DbCommand command = this.database.GetSqlStringCommand(str2.ToString());
                this.database.ExecuteNonQuery(command);
                str2 = "Update  Vshop_OneyuanTao_Detail set IsOn=@IsOn,Isend=@Isend where   Isend=0  and  ActivityId in(" + str + ")";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2.ToString());
            this.database.AddInParameter(sqlStringCommand, "IsOn", DbType.Boolean, IsOn);
            this.database.AddInParameter(sqlStringCommand, "Isend", DbType.Boolean, flag);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public string[] ChangeJoinStringArray(string[] ActivityIds)
        {
            for (int i = 0; i < ActivityIds.Length; i++)
            {
                ActivityIds[i] = "'" + ActivityIds[i] + "'";
            }
            return ActivityIds;
        }

        public bool CheckIsAll(string Aid)
        {
            string query = "select top 10000 Pid from Vshop_OneyuanTao_ParticipantMember where  ActivityId=@ActivityId and IsPay=1 and IsRefund=0";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, Aid);
            List<string> list = new List<string>();
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    list.Add(row["Pid"].ToString());
                }
            }
            if (list.Count < 1)
            {
                List<string> activivyIds = new List<string> {
                    Aid
                };
                this.SetIsAllRefund(activivyIds);
                return true;
            }
            return false;
        }

        public bool CreatLuckNum(string Pid, int UserId, string ActivityId, int BuyNum)
        {
            List<LuckInfo> infoList = new List<LuckInfo>();
            bool flag = false;
            lock (LuckNumObj)
            {
                int maxLuckNum = this.GetMaxLuckNum(ActivityId);
                for (int i = 1; i <= BuyNum; i++)
                {
                    LuckInfo info2 = new LuckInfo {
                        UserId = UserId,
                        ActivityId = ActivityId,
                        Pid = Pid
                    };
                    info2.PrizeNum = (maxLuckNum + i).ToString();
                    LuckInfo item = info2;
                    infoList.Add(item);
                }
                if ((infoList.Count > 0) && this.AddLuckInfo(infoList))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public bool DeleteOneyuanTao(string ActivityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(new StringBuilder("delete from Vshop_OneyuanTao_Detail where FinishedNum=0 and ActivityId=@ActivityId").ToString());
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DelParticipantMember(string ActivityId, bool delAll = true)
        {
            string query = "delete from Vshop_OneyuanTao_ParticipantMember where IsPay=0 and ActivityId=@ActivityId";
            if (!delAll)
            {
                if (string.IsNullOrEmpty(ActivityId))
                {
                    query = "delete from Vshop_OneyuanTao_ParticipantMember where IsPay=0 and BuyTime<@BuyTime and ActivityId<>'A0' ";
                }
                else
                {
                    query = "delete from Vshop_OneyuanTao_ParticipantMember where IsPay=0 and BuyTime<@BuyTime and ActivityId=@ActivityId";
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            this.database.AddInParameter(sqlStringCommand, "BuyTime", DbType.String, DateTime.Now.AddMinutes(-30.0));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public OneyuanTaoParticipantInfo GetAddParticipant(int UserId, string Pid = "", string payNum = "")
        {
            string query = "";
            if (UserId > 0)
            {
                query = "select top 1 * from Vshop_OneyuanTao_ParticipantMember where UserId=@UserId";
            }
            else if (Pid != "")
            {
                query = "select top 1 * from Vshop_OneyuanTao_ParticipantMember where Pid=@Pid";
            }
            else if (payNum != "")
            {
                query = "select top 1 * from Vshop_OneyuanTao_ParticipantMember where PayNum=@PayNum";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            this.database.AddInParameter(sqlStringCommand, "Pid", DbType.String, Pid);
            this.database.AddInParameter(sqlStringCommand, "PayNum", DbType.String, payNum);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<OneyuanTaoParticipantInfo>(reader);
            }
        }

        public IList<LuckInfo> getLuckInfoList(bool IsWin, string ActivityId)
        {
            string query = "select r.*,m.UserHead,m.UserName,BuyTime,BuyNum from Vshop_OneyuanTao_WinningRecord r,aspnet_Members m,Vshop_OneyuanTao_ParticipantMember p where r.UserId=m.UserId and r.Pid=p.pid  and r.IsWin=@IsWin and r.ActivityId=@ActivityId order by Pid Desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            this.database.AddInParameter(sqlStringCommand, "IsWin", DbType.Boolean, IsWin);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<LuckInfo>(reader);
            }
        }

        public IList<LuckInfo> getLuckInfoListByAId(string ActivityId, int UserId)
        {
            string query = "select r.*,m.UserHead,m.UserName,BuyTime,BuyNum from Vshop_OneyuanTao_WinningRecord r,aspnet_Members m,Vshop_OneyuanTao_ParticipantMember p where r.UserId=m.UserId and r.Pid=p.pid  and r.ActivityId=@ActivityId and r.UserId=@UserId order by Pid Desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<LuckInfo>(reader);
            }
        }

        public int GetMaxLuckNum(string ActivityId)
        {
            string query = string.Format("select MAX(PrizeNum) from Vshop_OneyuanTao_WinningRecord where ActivityId='{0}'", ActivityId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            int result = 0;
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                int.TryParse(obj2.ToString().Trim(), out result);
            }
            if (result == 0)
            {
                result = 0x989680;
            }
            return result;
        }

        public Top50ParticipantInfo GetNextParticipant(DateTime PrizeTime, string TopPid)
        {
            string query = "SELECT TOP 1 Pid,p.UserId,BuyTime,isnull(UserName,'SYSUSER') as UserName FROM Vshop_OneyuanTao_ParticipantMember p\r\n                         Left join aspnet_Members m on p.UserId=m.UserId where BuyTime<@PrizeTime and IsPay=1 and Pid<@TopPid order by Pid desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "PrizeTime", DbType.DateTime, PrizeTime);
            this.database.AddInParameter(sqlStringCommand, "TopPid", DbType.String, TopPid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<Top50ParticipantInfo>(reader);
            }
        }

        public DbQueryResult GetOneyuanPartInDataTable(OneyuanTaoPartInQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" and  UserName like '%{0}%' ", query.UserName);
            }
            if (!string.IsNullOrEmpty(query.Atitle))
            {
                builder.AppendFormat(" and  title like '%{0}%' ", query.Atitle);
            }
            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                builder.AppendFormat(" and  CellPhone like '%{0}%' ", query.CellPhone);
            }
            if (!string.IsNullOrEmpty(query.PayWay))
            {
                builder.AppendFormat(" and  PayWay = '{0}' ", query.PayWay);
            }
            if (!string.IsNullOrEmpty(query.ActivityId))
            {
                builder.AppendFormat(" and ActivityId='{0}' ", query.ActivityId);
            }
            if (!string.IsNullOrEmpty(query.Pid))
            {
                builder.AppendFormat(" and Pid='{0}' ", query.Pid);
            }
            if (query.UserId > 0)
            {
                builder.AppendFormat(" and UserId={0} ", query.UserId);
            }
            if (query.IsPay > -1)
            {
                builder.AppendFormat(" and IsPay={0} ", query.IsPay);
            }
            if (query.state > 0)
            {
                if (query.state == 1)
                {
                    builder.Append(" and (IsEnd=0 and EndTime>GETDATE()) ");
                }
                else if (query.state == 2)
                {
                    builder.Append(" and (IsEnd=1 or  EndTime<GETDATE()) ");
                }
                else if (query.state == 3)
                {
                    builder.Append(" and IsWin=1");
                }
                else if (query.state == 4)
                {
                    builder.Append(" and IsRefund=1");
                }
                else if (query.state == 5)
                {
                    builder.Append(" and (HasCalculate=1 and IsSuccess=0 and IsRefund=0)");
                }
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Vshop_OneyuanPartInList", "Pid", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DbQueryResult GetOneyuanTao(OneyuanTaoQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (!string.IsNullOrEmpty(query.title))
            {
                builder.AppendFormat(" and  Title like '%{0}%' ", query.title);
            }
            if (query.ReachType > 0)
            {
                builder.AppendFormat(" and ReachType={0} ", query.ReachType);
            }
            if (query.state > 0)
            {
                string str = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (query.state == 1)
                {
                    builder.AppendFormat(" and IsOn=1  and  StartTime<'{0}' and EndTime>='{0}' and IsEnd=0 ", str);
                }
                else if (query.state == 2)
                {
                    builder.AppendFormat(" and IsOn=1 and IsEnd=0 and StartTime>'{0}'", str);
                }
                else if (query.state == 3)
                {
                    builder.AppendFormat(" and (EndTime<'{0}' or IsEnd=1) ", str);
                }
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Vshop_OneyuanTao_Detail", "ActivityId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public OneyuanTaoInfo GetOneyuanTaoInfoById(string ActivityId)
        {
            string query = "select * from Vshop_OneyuanTao_Detail where ActivityId=@ActivityId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<OneyuanTaoInfo>(reader);
            }
        }

        public IList<OneyuanTaoInfo> GetOneyuanTaoInfoByIdList(string[] ActivityIds)
        {
            ActivityIds = this.ChangeJoinStringArray(ActivityIds);
            string query = "select * from Vshop_OneyuanTao_Detail where ActivityId in(" + string.Join(",", ActivityIds) + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<OneyuanTaoInfo>(reader);
            }
        }

        public IList<OneyuanTaoInfo> GetOneyuanTaoInfoNotCalculate()
        {
            string query = "select * from Vshop_OneyuanTao_Detail where IsEnd=0 and HasCalculate=0 and (EndTime<GETDATE() or (ReachType=1 and FinishedNum>=ReachNum )) ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<OneyuanTaoInfo>(reader);
            }
        }

        public int GetOneyuanTaoTotalNum(out int hasStart, out int waitStart, out int hasEnd)
        {
            DataTable table;
            string query = "select\r\n  (select COUNT(ActivityId) from Vshop_OneyuanTao_Detail)  total,\r\n  (select COUNT(ActivityId) from Vshop_OneyuanTao_Detail\r\n  where IsOn=1  and  StartTime<GETDATE() and EndTime>=GETDATE() and IsEnd=0) as hasStart,\r\n  (select COUNT(ActivityId) from Vshop_OneyuanTao_Detail\r\n  where  IsOn=1 and IsEnd=0 and StartTime>GETDATE()) as waitStart,\r\n  (select COUNT(ActivityId) from Vshop_OneyuanTao_Detail\r\n  where EndTime<GETDATE() or IsEnd=1) as hasEnd";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                hasStart = (int) table.Rows[0]["HasStart"];
                waitStart = (int) table.Rows[0]["waitStart"];
                hasEnd = (int) table.Rows[0]["hasEnd"];
                return (int) table.Rows[0]["total"];
            }
            hasStart = 0;
            waitStart = 0;
            hasEnd = 0;
            return 0;
        }

        public int GetParticipantCount()
        {
            string query = "SELECT count(Pid)FROM Vshop_OneyuanTao_ParticipantMember  where IsPay=1";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            int num = 0;
            if (obj2 != null)
            {
                num = (int) obj2;
            }
            return num;
        }

        public IList<OneyuanTaoParticipantInfo> GetParticipantList(string ActivityId, int[] UserIds = null, string[] PIds = null)
        {
            string query = "";
            if (!string.IsNullOrEmpty(ActivityId))
            {
                query = string.Format("select * from Vshop_OneyuanTao_ParticipantMember where ActivityId='{0}' ", ActivityId);
            }
            else if (UserIds != null)
            {
                query = "select * from Vshop_OneyuanTao_ParticipantMember where UserIds in(" + string.Join<int>(",", UserIds) + ")";
            }
            else if (PIds != null)
            {
                query = "select * from Vshop_OneyuanTao_ParticipantMember where Pid in(" + string.Join(",", PIds) + ")";
            }
            if (query == "")
            {
                return null;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<OneyuanTaoParticipantInfo>(reader);
            }
        }

        public List<string> GetParticipantPids(string ActivityId, bool IsPay = true, bool IsRefund = false, string PayWay = "alipay")
        {
            string query = "select top 10000 Pid from Vshop_OneyuanTao_ParticipantMember where  ActivityId=@ActivityId and IsPay=@IsPay and IsRefund=@IsRefund and PayWay=@PayWay";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            this.database.AddInParameter(sqlStringCommand, "PayWay", DbType.String, PayWay);
            this.database.AddInParameter(sqlStringCommand, "IsPay", DbType.Boolean, IsPay);
            this.database.AddInParameter(sqlStringCommand, "IsRefund", DbType.Boolean, IsRefund);
            List<string> list = new List<string>();
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    list.Add(row["Pid"].ToString());
                }
            }
            return list;
        }

        public string getPrizeCountInfo(string ActivityId)
        {
            string query = "select PrizeCountInfo from Vshop_OneyuanTao_Detail Where ActivityId=@ActivityId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            string str2 = "";
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                str2 = obj2.ToString();
            }
            return str2;
        }

        public IList<OneyuanTaoParticipantInfo> GetRefundParticipantList(string[] PIds)
        {
            string query = "";
            PIds = this.ChangeJoinStringArray(PIds);
            query = "select IsPay,IsRefund,TotalPrice,Pid,PayWay,IsWin,PayNum,out_refund_no from Vshop_OneyuanTao_ParticipantMember where Pid in(" + string.Join(",", PIds) + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<OneyuanTaoParticipantInfo>(reader);
            }
        }

        public int GetRefundTotalNum(out int Refundnum)
        {
            DataTable table;
            int num = 0;
            string query = " select\r\n (select count(pid) from vw_Vshop_OneyuanPartInList where  IsPay=1 and HasCalculate=1 and IsSuccess=0 and IsRefund=0) as r,\r\n  (select count(pid) from vw_Vshop_OneyuanPartInList where IsRefund=1) as f";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                num = (int) table.Rows[0]["r"];
                Refundnum = (int) table.Rows[0]["f"];
                return num;
            }
            Refundnum = 0;
            return num;
        }

        public string GetSkuStrBySkuId(string Skuid, bool ShowAttribute = true)
        {
            string query = "SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId\r\nleft join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId\r\nAND s.ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1)";
            string str2 = string.Empty;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, Skuid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    if (((ShowAttribute && (reader["AttributeName"] != DBNull.Value)) && (!string.IsNullOrEmpty((string) reader["AttributeName"]) && (reader["ValueStr"] != DBNull.Value))) && !string.IsNullOrEmpty((string) reader["ValueStr"]))
                    {
                        object obj2 = str2;
                        str2 = string.Concat(new object[] { obj2, reader["AttributeName"], "：", reader["ValueStr"], "/" });
                    }
                    else
                    {
                        str2 = str2 + reader["ValueStr"] + "/";
                    }
                }
            }
            return str2;
        }

        public IList<Top50ParticipantInfo> GetTop50ParticipantList(DateTime PrizeTime, int topnum = 50)
        {
            string query = "SELECT TOP " + topnum + " Pid,p.UserId,BuyTime,isnull(UserName,'SYSUSER') as UserName FROM Vshop_OneyuanTao_ParticipantMember p\r\n                         Left join aspnet_Members m on p.UserId=m.UserId where BuyTime<@PrizeTime and IsPay=1 order by Pid desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "PrizeTime", DbType.DateTime, PrizeTime);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<Top50ParticipantInfo>(reader);
            }
        }

        public int GetUserAlreadyBuyNum(int userId, string activityId)
        {
            string query = "select sum(BuyNum) from Vshop_OneyuanTao_ParticipantMember where UserID=@UserID and ActivityId=@ActivityId ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, activityId);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public IList<LuckInfo> getWinnerLuckInfoList(string ActivityId, string Pid = "")
        {
            string query = "";
            if (string.IsNullOrEmpty(Pid))
            {
                query = "select r.*,m.UserHead,m.UserName,BuyTime,BuyNum,1 as IsWin  from vw_Vshop_OneyuanWinnerList r,aspnet_Members m,Vshop_OneyuanTao_ParticipantMember p where r.UserId=m.UserId and r.Pid=p.pid  and r.ActivityId=@ActivityId order by Pid Desc";
            }
            else
            {
                query = "select r.*,m.UserHead,m.UserName,BuyTime,BuyNum,1 as IsWin  from vw_Vshop_OneyuanWinnerList r,aspnet_Members m,Vshop_OneyuanTao_ParticipantMember p where r.UserId=m.UserId and r.Pid=p.pid  and r.Pid=@Pid and r.ActivityId=@ActivityId order by Pid Desc";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            this.database.AddInParameter(sqlStringCommand, "Pid", DbType.String, Pid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<LuckInfo>(reader);
            }
        }

        public bool IsExistAlipayRefundNUm(string batch_no)
        {
            string query = "";
            query = "select top 1 RefundNum from Vshop_OneyuanTao_ParticipantMember where RefundNum=@RefundNum";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "RefundNum", DbType.String, batch_no);
            bool flag = false;
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                flag = true;
            }
            return flag;
        }

        public int MermberCanbuyNum(string ActivityId, int userid)
        {
            MermberCanbuyNumInfo info;
            int num = 0;
            string query = " select\r\n  (select EachCanBuyNum from Vshop_OneyuanTao_Detail where ActivityId=@ActivityId) as t,\r\n  (select isnull(SUM(BuyNum),0) from Vshop_OneyuanTao_ParticipantMember where ActivityId=@ActivityId and UserId=@UserId) as b";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<MermberCanbuyNumInfo>(reader);
            }
            if (info != null)
            {
                num = info.t - info.b;
            }
            return num;
        }

        public DataTable PrizesDeliveryRecord(string Pid)
        {
            string query = "select * from Hishop_PrizesDeliveryRecord where Pid=@Pid";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "Pid", DbType.String, Pid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public bool SetErrPrizeCountInfo(string ActivityId, string PrizeCountInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(new StringBuilder("Update  Vshop_OneyuanTao_Detail set IsEnd=1,HasCalculate=1,IsOn=0,PrizeCountInfo=@PrizeCountInfo where ActivityId=@ActivityId").ToString());
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            this.database.AddInParameter(sqlStringCommand, "PrizeCountInfo", DbType.String, PrizeCountInfo);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int SetIsAllRefund(List<string> ActivivyIds)
        {
            string[] activityIds = ActivivyIds.ToArray<string>();
            activityIds = this.ChangeJoinStringArray(activityIds);
            string query = "update Vshop_OneyuanTao_Detail set IsAllRefund=1 where IsAllRefund=0 and HasCalculate=1 ";
            query = (((query + " and IsSuccess=0 and ActivityId in( select ActivityId  from  Vshop_OneyuanTao_Detail where ") + " ActivityId in(" + string.Join(",", activityIds) + ")  ") + " and  ActivityId not in(select ActivityId from Vshop_OneyuanTao_ParticipantMember  ") + " where IsPay=1 and IsRefund=0 and ActivityId in(" + string.Join(",", activityIds) + ") ) )";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool SetOneyuanTaoFinishedNum(string ActivityId, int Addnum = 0)
        {
            StringBuilder builder = new StringBuilder();
            if (Addnum > 0)
            {
                builder.Append("Update  Vshop_OneyuanTao_Detail set FinishedNum=FinishedNum+@Addnum where ActivityId=@ActivityId");
            }
            else
            {
                builder.Append("declare @fnum int;select @fnum =isnull(SUM(BuyNum),0) from Vshop_OneyuanTao_ParticipantMember where ActivityId=@ActivityId and IsPay=1;\r\n                           Update  Vshop_OneyuanTao_Detail set FinishedNum=@fnum where ActivityId=@ActivityId;");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Addnum", DbType.Int32, Addnum);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetOneyuanTaoHasCalculate(string ActivityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(new StringBuilder("Update  Vshop_OneyuanTao_Detail set IsEnd=1,HasCalculate=1,IsOn=0 where ActivityId=@ActivityId").ToString());
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetOneyuanTaoIsOn(string ActivityId, bool IsOn)
        {
            bool flag = false;
            bool flag2 = false;
            string str = "Update  Vshop_OneyuanTao_Detail set IsOn=@IsOn,HasCalculate=@HasCalculate,Isend=@Isend where ActivityId=@ActivityId";
            if (!IsOn)
            {
                flag = true;
                flag2 = true;
            }
            else
            {
                str = "if  exists (select ActivityId from Vshop_OneyuanTao_Detail where ActivityId=@ActivityId and StartTime>GETDATE())\r\n                       Update  Vshop_OneyuanTao_Detail set StartTime=GETDATE(),IsOn=@IsOn,Isend=@Isend where ActivityId=@ActivityId\r\n                     else\r\n                       Update  Vshop_OneyuanTao_Detail set IsOn=@IsOn,Isend=@Isend where ActivityId=@ActivityId ";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str.ToString());
            this.database.AddInParameter(sqlStringCommand, "IsOn", DbType.Boolean, IsOn);
            this.database.AddInParameter(sqlStringCommand, "HasCalculate", DbType.Boolean, flag2);
            this.database.AddInParameter(sqlStringCommand, "Isend", DbType.Boolean, flag);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetOneyuanTaoPrizeTime(string ActivityId, DateTime PrizeTime, string PrizeInfoJson)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(new StringBuilder("Update  Vshop_OneyuanTao_Detail set PrizeTime=@PrizeTime,PrizeCountInfo=@PrizeInfoJson,IsEnd=1,IsSuccess=1,HasCalculate=1,IsOn=0 where ActivityId=@ActivityId").ToString());
            this.database.AddInParameter(sqlStringCommand, "PrizeTime", DbType.DateTime, PrizeTime);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, ActivityId);
            this.database.AddInParameter(sqlStringCommand, "PrizeInfoJson", DbType.String, PrizeInfoJson);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool Setout_refund_no(string Pid, string out_refund_no)
        {
            string query = "Update Vshop_OneyuanTao_ParticipantMember set out_refund_no=@out_refund_no ";
            query = query + " where Pid=@Pid";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "out_refund_no", DbType.String, out_refund_no);
            this.database.AddInParameter(sqlStringCommand, "Pid", DbType.String, Pid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetPayinfo(OneyuanTaoParticipantInfo info)
        {
            if (this.CreatLuckNum(info.Pid, info.UserId, info.ActivityId, info.BuyNum))
            {
                string query = "Update Vshop_OneyuanTao_ParticipantMember set IsPay=1, PayTime=@PayTime,PayWay=@PayWay,PayNum=@PayNum,Remark=@Remark";
                query = query + " where Pid=@Pid";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "PayWay", DbType.String, info.PayWay);
                this.database.AddInParameter(sqlStringCommand, "PayTime", DbType.DateTime, DateTime.Now);
                this.database.AddInParameter(sqlStringCommand, "PayNum", DbType.String, info.PayNum);
                this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, info.Remark);
                this.database.AddInParameter(sqlStringCommand, "Pid", DbType.String, info.Pid);
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            Globals.Debuglog("支付回调失败了！", "_Debuglog.txt");
            return false;
        }

        public bool SetRefundinfo(OneyuanTaoParticipantInfo info)
        {
            string query = "Update Vshop_OneyuanTao_ParticipantMember set IsRefund=1,RefundErr=0,RefundTime=@RefundTime,RefundNum=@RefundNum,Remark=@Remark";
            query = query + " where Pid=@Pid";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "RefundTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "RefundNum", DbType.String, info.RefundNum);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, info.Remark);
            this.database.AddInParameter(sqlStringCommand, "Pid", DbType.String, info.Pid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetRefundinfoErr(OneyuanTaoParticipantInfo info)
        {
            string query = "Update Vshop_OneyuanTao_ParticipantMember set RefundErr=1,Remark=@Remark ";
            query = query + " where Pid=@Pid ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, info.Remark);
            this.database.AddInParameter(sqlStringCommand, "Pid", DbType.String, info.Pid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool setWin(string PrizeNum, string AcitivityId)
        {
            string query = string.Format("update Vshop_OneyuanTao_WinningRecord set IsWin=1 where PrizeNum='{0}' and ActivityId='{1}'", PrizeNum, AcitivityId);
            string str2 = string.Format("update Vshop_OneyuanTao_ParticipantMember set IsWin=1 where Pid in( select pid  from  Vshop_OneyuanTao_WinningRecord where PrizeNum='{0}' and ActivityId='{1}')", PrizeNum, AcitivityId);
            query = query + "; " + str2;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateOneyuanTao(OneyuanTaoInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(new StringBuilder("Update  Vshop_OneyuanTao_Detail set IsOn=@IsOn,Title=@Title,StartTime=@StartTime,EndTime=@EndTime,\r\n           HeadImgage=@HeadImgage,ReachType=@ReachType,ActivityDec=@ActivityDec,ProductId=@ProductId,ProductPrice=@ProductPrice,\r\n           ProductImg=@ProductImg,ProductTitle=@ProductTitle,SkuId=@SkuId,PrizeNumber=@PrizeNumber,EachPrice=@EachPrice\r\n           ,EachCanBuyNum=@EachCanBuyNum,FitMember=@FitMember,DefualtGroup=@DefualtGroup,CustomGroup=@CustomGroup,ReachNum=@ReachNum,\r\n           FinishedNum=@FinishedNum where ActivityId=@ActivityId").ToString());
            this.database.AddInParameter(sqlStringCommand, "IsOn", DbType.Boolean, info.IsOn);
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, info.Title);
            this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, info.StartTime);
            this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, info.EndTime);
            this.database.AddInParameter(sqlStringCommand, "HeadImgage", DbType.String, info.HeadImgage);
            this.database.AddInParameter(sqlStringCommand, "ActivityDec", DbType.String, info.ActivityDec);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, info.ProductId);
            this.database.AddInParameter(sqlStringCommand, "ProductPrice", DbType.Decimal, info.ProductPrice);
            this.database.AddInParameter(sqlStringCommand, "ProductImg", DbType.String, info.ProductImg);
            this.database.AddInParameter(sqlStringCommand, "ProductTitle", DbType.String, info.ProductTitle);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, info.SkuId);
            this.database.AddInParameter(sqlStringCommand, "PrizeNumber", DbType.Int32, info.PrizeNumber);
            this.database.AddInParameter(sqlStringCommand, "EachCanBuyNum", DbType.Int32, info.EachCanBuyNum);
            this.database.AddInParameter(sqlStringCommand, "FitMember", DbType.String, info.FitMember);
            this.database.AddInParameter(sqlStringCommand, "DefualtGroup", DbType.String, info.DefualtGroup);
            this.database.AddInParameter(sqlStringCommand, "CustomGroup", DbType.String, info.CustomGroup);
            this.database.AddInParameter(sqlStringCommand, "ReachNum", DbType.Int32, info.ReachNum);
            this.database.AddInParameter(sqlStringCommand, "FinishedNum", DbType.Int32, info.FinishedNum);
            this.database.AddInParameter(sqlStringCommand, "EachPrice", DbType.Decimal, info.EachPrice);
            this.database.AddInParameter(sqlStringCommand, "ReachType", DbType.Int32, info.ReachType);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.String, info.ActivityId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private class MermberCanbuyNumInfo
        {
            public int b { get; set; }

            public int t { get; set; }
        }
    }
}

