namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public class MemberDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool addFuwuFollowUser(string openid)
        {
            string query = "delete from Vshop_FollowUsers where OpenId=@OpenId;insert into Vshop_FollowUsers(OpenId,FollowTime)values(@OpenId,@FollowTime);update aspnet_Members set IsFollowAlipay=1 where AlipayOpenid=@OpenId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openid);
            this.database.AddInParameter(sqlStringCommand, "FollowTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool BatchHuifu(string userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  aspnet_Members SET Status=1 WHERE UserId in (" + userId + ")");
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool BindUserName(int UserId, string UserBindName, string Password)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET UserBindName = @UserBindName, Password = @Password WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserBindName", DbType.String, UserBindName);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, Password);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CheckCurrentMemberIsInRange(string Grades, string DefualtGroup, string CustomGroup, int tuserid = 0)
        {
            bool flag = false;
            MemberDao dao = new MemberDao();
            int item = 0;
            if (tuserid <= 0)
            {
                item = Globals.GetCurrentMemberUserId(false);
            }
            else
            {
                item = tuserid;
            }
            if (!string.IsNullOrEmpty(Grades) && !Grades.Equals("-1"))
            {
                if (Grades.Equals("0"))
                {
                    return true;
                }
                flag = dao.GetGradeMemberList(Grades).Contains(item);
                if (flag)
                {
                    return true;
                }
            }
            if (!string.IsNullOrEmpty(DefualtGroup) && !DefualtGroup.Equals("-1"))
            {
                if (DefualtGroup.Equals("0"))
                {
                    return true;
                }
                flag = dao.GetDefualtGroupMemberList(DefualtGroup).Contains(item);
                if (flag)
                {
                    return true;
                }
            }
            if (!string.IsNullOrEmpty(CustomGroup))
            {
                flag = dao.GetCustomGroupMemberList(CustomGroup).Contains(item);
            }
            return flag;
        }

        public bool CheckMemberIsBuyProds(int userId, string prodIds, DateTime? startTime, DateTime? endTime)
        {
            string query = "SELECT COUNT(o.OrderId) \r\n                             FROM Hishop_Orders AS o \r\n                        LEFT JOIN Hishop_OrderItems AS oi ON o.OrderId=oi.OrderId\r\n                        where 1=1 ";
            query = query + " AND UserId=" + userId;
            if (startTime.HasValue)
            {
                object obj3 = query;
                query = string.Concat(new object[] { obj3, " AND datediff(dd,'", startTime.Value, "',o.PayDate) >= 0" });
            }
            if (endTime.HasValue)
            {
                object obj4 = query;
                query = string.Concat(new object[] { obj4, " AND datediff(dd,'", endTime.Value.AddDays(1.0), "',o.PayDate) < 0" });
            }
            query = query + " AND oi.ProductId IN (" + prodIds + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            try
            {
                return (Convert.ToInt32(obj2) > 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ClearAllOpenId(string from = "wx")
        {
            string query = "Update aspnet_Members set OpenId='',IsAuthorizeWeiXin=0 ";
            if (from == "fuwu")
            {
                query = "Update aspnet_Members set AlipayOpenid=''";
            }
            else
            {
                query = query + ";delete from WeiXin_RecentOpenID";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CreateMember(MemberInfo member)
        {
            string query = "INSERT INTO aspnet_Members(GradeId,ReferralUserId,UserName,CreateDate,OrderNumber,Expenditure,Points,TopRegionId, RegionId,OpenId, SessionId, SessionEndTime,Password,UserHead,UserBindName,Status,AlipayUserId,AlipayUsername,AlipayOpenid,AlipayLoginId,AlipayAvatar) VALUES(@GradeId,@ReferralUserId,@UserName,@CreateDate,0,0,0,0,0,@OpenId, @SessionId, @SessionEndTime,@Password,@UserHead,@UserBindName,@Status,@AlipayUserId,@AlipayUsername,@AlipayOpenid,@AlipayLoginId,@AlipayAvatar)";
            if ((member.OpenId != null) && (member.OpenId.Trim() != ""))
            {
                query = "if not exists(select top 1 userid from aspnet_Members where openid='" + member.OpenId.Trim() + "') " + query;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            if (member.AlipayOpenid == null)
            {
                member.AlipayOpenid = "";
            }
            if (member.AlipayLoginId == null)
            {
                member.AlipayLoginId = "";
            }
            if (member.AlipayUserId == null)
            {
                member.AlipayUserId = "";
            }
            if (member.AlipayUsername == null)
            {
                member.AlipayUsername = "";
            }
            if (member.AlipayAvatar == null)
            {
                member.AlipayAvatar = "";
            }
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, member.ReferralUserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, member.UserName);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, member.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, member.OpenId);
            this.database.AddInParameter(sqlStringCommand, "SessionId", DbType.String, member.SessionId);
            this.database.AddInParameter(sqlStringCommand, "SessionEndTime", DbType.DateTime, member.SessionEndTime);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, member.Password);
            this.database.AddInParameter(sqlStringCommand, "UserHead", DbType.String, member.UserHead);
            this.database.AddInParameter(sqlStringCommand, "UserBindName", DbType.String, member.UserBindName);
            this.database.AddInParameter(sqlStringCommand, "AlipayAvatar", DbType.String, member.AlipayAvatar);
            this.database.AddInParameter(sqlStringCommand, "AlipayUsername", DbType.String, member.AlipayUsername);
            this.database.AddInParameter(sqlStringCommand, "AlipayLoginId", DbType.String, member.AlipayLoginId);
            this.database.AddInParameter(sqlStringCommand, "AlipayOpenid", DbType.String, member.AlipayOpenid);
            this.database.AddInParameter(sqlStringCommand, "AlipayUserId", DbType.String, member.AlipayUserId);
            int status = Convert.ToInt32(UserStatus.Normal);
            if (member.Status == Convert.ToInt32(UserStatus.Visitor))
            {
                status = member.Status;
            }
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, status);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool Delete(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_Members WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool Delete2(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  aspnet_Members SET Status=7 WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool Deletes(string userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  aspnet_Members SET Status=7 WHERE UserId in (" + userId + ")");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DelFuwuFollowUser(string openid)
        {
            string query = "delete from Vshop_FollowUsers where OpenId=@OpenId;update aspnet_Members set IsFollowAlipay=0 where AlipayOpenid=@OpenId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DelUserMessage(MemberInfo newuser, int olduserid)
        {
            string query = "";
            object obj2 = query + "begin try  " + "  begin tran TranUpdate";
            object obj3 = string.Concat(new object[] { obj2, " DELETE FROM aspnet_Members WHERE UserId =", newuser.UserId, "; " });
            object obj4 = string.Concat(new object[] { obj3, " DELETE FROM Hishop_ShoppingCarts WHERE UserId =", newuser.UserId, "; " });
            object obj5 = string.Concat(new object[] { obj4, " DELETE FROM Hishop_UserShippingAddresses WHERE UserId =", newuser.UserId, "; " });
            query = string.Concat(new object[] { obj5, " DELETE FROM vshop_ActivitySignUp WHERE UserId =", newuser.UserId, "; " }) + " Update  aspnet_Members ";
            string str2 = "";
            if (!string.IsNullOrEmpty(newuser.OpenId))
            {
                str2 = "  set OpenId='" + newuser.OpenId + "',UserHead='" + newuser.UserHead + "'";
            }
            if (!string.IsNullOrEmpty(newuser.AlipayOpenid))
            {
                if (str2 == "")
                {
                    str2 = " set ";
                }
                else
                {
                    str2 = " ,";
                }
                string str3 = str2;
                string str4 = str3 + "AlipayUserId='" + newuser.AlipayUserId + "',AlipayOpenid='" + newuser.AlipayOpenid + "'";
                str2 = (str4 + ",AlipayLoginId='" + newuser.AlipayLoginId + "',AlipayUsername='" + newuser.AlipayUsername + "'") + ",AlipayAvatar='" + newuser.AlipayAvatar + "' ";
                if ((string.IsNullOrEmpty(newuser.UserHead) && !string.IsNullOrEmpty(newuser.AlipayAvatar)) && string.IsNullOrEmpty(newuser.OpenId))
                {
                    str2 = str2 + ",UserHead='" + newuser.AlipayAvatar + "'";
                }
            }
            object obj6 = query;
            query = string.Concat(new object[] { obj6, str2, " WHERE UserId =", olduserid, " ;" }) + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DelUserMessage(int userid, string openid, string userhead, int olduserid)
        {
            string query = "";
            object obj2 = query + "begin try  " + "  begin tran TranUpdate";
            object obj3 = string.Concat(new object[] { obj2, " DELETE FROM aspnet_Members WHERE UserId =", userid, "; " });
            object obj4 = string.Concat(new object[] { obj3, " DELETE FROM Hishop_ShoppingCarts WHERE UserId =", userid, "; " });
            object obj5 = string.Concat(new object[] { obj4, " DELETE FROM Hishop_UserShippingAddresses WHERE UserId =", userid, "; " });
            object obj6 = string.Concat(new object[] { obj5, " DELETE FROM vshop_ActivitySignUp WHERE UserId =", userid, "; " });
            query = string.Concat(new object[] { obj6, " Update  aspnet_Members set OpenId='", openid, "',UserHead='", userhead, "' WHERE UserId =", olduserid, "; " }) + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int GetActiveDay()
        {
            string commandText = string.Format("select top 1   isnull(ActiveDay,1)  from Hishop_UserGroupSet", new object[0]);
            int num = 1;
            try
            {
                num = Convert.ToInt32(this.database.ExecuteScalar(CommandType.Text, commandText));
            }
            catch
            {
            }
            return num;
        }

        public IList<int> GetActivyGroupMemberList()
        {
            IList<int> list = new List<int>();
            int activeDay = this.GetActiveDay();
            string query = string.Format("SELECT UserId FROM aspnet_Members WHERE PayOrderDate is not null and PayOrderDate >='" + DateTime.Now.AddDays((double) -activeDay).ToString("yyyy-MM-dd HH:mm:ss") + "' ", new object[0]);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    int item = Convert.ToInt32(row["UserId"]);
                    list.Add(item);
                }
            }
            return list;
        }

        public string GetAliOpenIDByUserId(int UserId)
        {
            string query = string.Format("Select top 1 AlipayOpenid  from  aspnet_Members where  UserId={0}  ", UserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (string) obj2;
            }
            return "";
        }

        public IList<int> GetAllMemberList()
        {
            IList<int> list = new List<int>();
            string query = "SELECT UserId FROM dbo.aspnet_Members where Status=1";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    int item = Convert.ToInt32(row["UserId"]);
                    list.Add(item);
                }
            }
            return list;
        }

        public int GetBindOpenIDAndNoUserNameCount()
        {
            string query = string.Format("Select COUNT(*) as SumRec from  aspnet_Members where (UserBindName is null or UserBindName='')  and Status={0};", 1);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return (int) obj2;
            }
            return 0;
        }

        public int GetBindOpenIDCount()
        {
            string query = string.Format("Select COUNT(*) as SumRec from  aspnet_Members where OpenId is not null and OpenId<>'' and Status={0};", 1);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return (int) obj2;
            }
            return 0;
        }

        public MemberInfo GetBindusernameMember(string UserBindName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserBindName = @UserBindName");
            this.database.AddInParameter(sqlStringCommand, "UserBindName", DbType.String, UserBindName);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, Convert.ToInt32(UserStatus.Normal));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MemberInfo>(reader);
            }
        }

        public IList<int> GetCreateDateIntervalMemberList(DateTime? start, DateTime? end)
        {
            IList<int> list = new List<int>();
            string query = "SELECT UserId FROM dbo.aspnet_Members WHERE 1=1 ";
            if (start.HasValue)
            {
                object obj2 = query;
                query = string.Concat(new object[] { obj2, " AND datediff(dd,'", start.Value, "',CreateDate) >= 0" });
            }
            if (end.HasValue)
            {
                object obj3 = query;
                query = string.Concat(new object[] { obj3, " AND datediff(dd,'", end.Value, "',CreateDate) <= 0" });
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    int item = Convert.ToInt32(row["UserId"]);
                    list.Add(item);
                }
            }
            return list;
        }

        public string GetCurrentParentUserId(int? userId)
        {
            string str = "";
            string query = "SELECT ReferralPath FROM aspnet_Distributors WHERE UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int64, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                str = userId.ToString();
                if (reader["ReferralUserId"].ToString() != "0")
                {
                    str = reader["ReferralUserId"].ToString() + "|" + userId.ToString();
                }
            }
            return str;
        }

        public IList<int> GetCustomGroupMemberList(string CustomGroup)
        {
            IList<int> list = new List<int>();
            string query = "";
            if (!CustomGroup.Equals("0"))
            {
                query = "SELECT UserId FROM dbo.Vshop_CustomGroupingUser WHERE GroupId in (" + CustomGroup + ")";
            }
            else
            {
                query = "SELECT UserId FROM dbo.Vshop_CustomGroupingUser";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    int item = Convert.ToInt32(row["UserId"]);
                    list.Add(item);
                }
            }
            return list;
        }

        public IList<int> GetDefualtGroupMemberList(string DefualtGroup)
        {
            IList<int> newGroupMemberList = new List<int>();
            if (DefualtGroup.Contains("1"))
            {
                newGroupMemberList = this.GetNewGroupMemberList();
            }
            if (DefualtGroup.Contains("2"))
            {
                if (newGroupMemberList.Count == 0)
                {
                    newGroupMemberList = this.GetActivyGroupMemberList();
                }
                else
                {
                    foreach (int num in this.GetActivyGroupMemberList())
                    {
                        newGroupMemberList.Add(num);
                    }
                }
            }
            if (DefualtGroup.Contains("3"))
            {
                if (newGroupMemberList.Count == 0)
                {
                    return this.GetSleepGroupMemberList();
                }
                foreach (int num2 in this.GetSleepGroupMemberList())
                {
                    newGroupMemberList.Add(num2);
                }
            }
            return newGroupMemberList;
        }

        public DistributorInfo GetDistributor(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT a.StoreName, a.CreateTime as  CreateTime_Distributor, a.DistributorGradeId,\r\n                b.* FROM aspnet_Distributors a\r\n                left join aspnet_Members b  on a.UserId=b.UserId\r\n                WHERE a.UserId = @UserId UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<DistributorInfo>(reader);
            }
        }

        public int GetDistributorNumOfTotal(int topUserId, out int topNum)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select (SELECT COUNT(UserId) \r\n  FROM aspnet_Distributors) as total,(SELECT COUNT(UserId) \r\n  FROM aspnet_Distributors where ReferralUserId=@ReferralUserId ) as topNum");
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, topUserId);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                num = (int) table.Rows[0]["total"];
                topNum = (int) table.Rows[0]["topNum"];
                return num;
            }
            topNum = 0;
            return num;
        }

        public IList<int> GetGradeMemberList(string Grades)
        {
            IList<int> list = new List<int>();
            string query = "SELECT UserId FROM dbo.aspnet_Members WHERE GradeId in (" + Grades + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    int item = Convert.ToInt32(row["UserId"]);
                    list.Add(item);
                }
            }
            return list;
        }

        public MemberInfo GetMember(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MemberInfo>(reader);
            }
        }

        public MemberInfo GetMember(string sessionId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE SessionId = @SessionId");
            this.database.AddInParameter(sqlStringCommand, "SessionId", DbType.String, sessionId);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, Convert.ToInt32(UserStatus.Normal));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MemberInfo>(reader);
            }
        }

        public Dictionary<int, MemberClientSet> GetMemberClientSet()
        {
            Dictionary<int, MemberClientSet> dictionary = new Dictionary<int, MemberClientSet>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MemberClientSet");
            MemberClientSet set = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    set = DataMapper.PopulateMemberClientSet(reader);
                    dictionary.Add(set.ClientTypeId, set);
                }
            }
            return dictionary;
        }

        public int GetMemberIdByUserNameOrNiChen(string username = "", string nich = "")
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM aspnet_Members WHERE UserBindName = @UserBindName AND Status=@Status");
                this.database.AddInParameter(sqlStringCommand, "UserBindName", DbType.String, username);
                this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, Convert.ToInt32(UserStatus.Normal));
                object obj2 = this.database.ExecuteScalar(sqlStringCommand);
                if (obj2 != null)
                {
                    return (int) obj2;
                }
            }
            else if (!string.IsNullOrWhiteSpace(nich))
            {
                DbCommand command = this.database.GetSqlStringCommand("SELECT UserId FROM aspnet_Members WHERE UserName = @UserName AND Status=@Status");
                this.database.AddInParameter(command, "UserName", DbType.String, nich);
                this.database.AddInParameter(command, "Status", DbType.Int32, Convert.ToInt32(UserStatus.Normal));
                object obj3 = this.database.ExecuteScalar(command);
                if (obj3 != null)
                {
                    return (int) obj3;
                }
            }
            return 0;
        }

        public List<MemberInfo> GetMemberInfoList(string userIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserId in (" + userIds + ")");
            List<MemberInfo> list = new List<MemberInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MemberInfo>(reader).ToList<MemberInfo>();
            }
        }

        public int GetMemberNumOfTotal(int topUserId, out int topNum)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select (SELECT COUNT(UserId) \r\n  FROM aspnet_Members) as total,(SELECT COUNT(UserId) \r\n  FROM aspnet_Members where ReferralUserId=@ReferralUserId ) as topNum");
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, topUserId);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                num = (int) table.Rows[0]["total"];
                topNum = (int) table.Rows[0]["topNum"];
                return num;
            }
            topNum = 0;
            return num;
        }

        public DbQueryResult GetMembers(MemberQuery query, bool isNotBindUserName = false)
        {
            StringBuilder builder = new StringBuilder();
            if (query.HasVipCard.HasValue)
            {
                if (query.HasVipCard.Value)
                {
                    builder.Append("VipCardNumber is not null");
                }
                else
                {
                    builder.Append("VipCardNumber is null");
                }
            }
            if (query.GradeId.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("GradeId = {0}", query.GradeId.Value);
            }
            if (query.IsApproved.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("IsApproved = '{0}'", query.IsApproved.Value);
            }
            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("CellPhone = '{0}'", query.CellPhone);
            }
            if (query.Stutas.HasValue && (Convert.ToInt32(query.Stutas) > 0))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (((UserStatus) query.Stutas) == UserStatus.Visitor)
                {
                    builder.AppendFormat(" PayOrderDate is null and (Status=1 or status=9) ", new object[0]);
                }
                else
                {
                    builder.AppendFormat("Status = {0}", Convert.ToInt32(query.Stutas));
                }
            }
            if (!string.IsNullOrEmpty(query.Username))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                if (builder.Length > 0)
                {
                    builder.AppendFormat(" AND ", new object[0]);
                }
                builder.AppendFormat("RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
            }
            if (!string.IsNullOrEmpty(query.UserBindName))
            {
                if (builder.Length > 0)
                {
                    builder.AppendFormat(" AND ", new object[0]);
                }
                builder.AppendFormat("UserBindName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserBindName));
            }
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.AppendFormat(" AND ", new object[0]);
                }
                if (query.StoreName == "主店")
                {
                    builder.AppendFormat(" ReferralUserId not in  (select userid from  dbo.aspnet_Distributors where  UserId=m.ReferralUserId)", new object[0]);
                }
                else
                {
                    builder.AppendFormat(" ReferralUserId in  (SELECT UserId  FROM aspnet_Distributors where storename='{0}' )", query.StoreName);
                }
            }
            if (isNotBindUserName)
            {
                if (builder.Length > 0)
                {
                    builder.AppendFormat(" AND ", new object[0]);
                }
                builder.AppendFormat(" (UserBindName is null or UserBindName='')", new object[0]);
            }
            this.GetActiveDay();
            if (!string.IsNullOrEmpty(query.ClientType))
            {
                string clientType = query.ClientType;
                if (clientType == null)
                {
                    goto Label_03CF;
                }
                if (!(clientType == "new"))
                {
                    int activeDay;
                    if (clientType == "activy")
                    {
                        activeDay = new MemberDao().GetActiveDay();
                        builder.AppendFormat("  and PayOrderDate is not null and PayOrderDate >='" + DateTime.Now.AddDays((double) -activeDay).ToString("yyyy-MM-dd HH:mm:ss") + "' ", new object[0]);
                        goto Label_03E1;
                    }
                    if (clientType == "sleep")
                    {
                        activeDay = new MemberDao().GetActiveDay();
                        builder.AppendFormat("  and  (PayOrderDate is null or PayOrderDate <'" + DateTime.Now.AddDays((double) -activeDay).ToString("yyyy-MM-dd HH:mm:ss") + "') ", new object[0]);
                        goto Label_03E1;
                    }
                    goto Label_03CF;
                }
                builder.AppendFormat(" AND LastOrderDate is null", new object[0]);
            }
            goto Label_03E1;
        Label_03CF:
            builder.AppendFormat(" AND LastOrderDate is null", new object[0]);
        Label_03E1:
            if (query.GroupId.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserId in (select UserId from Vshop_CustomGroupingUser where GroupId={0})", query.GroupId.Value);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Members m", "UserId", (builder.Length > 0) ? builder.ToString() : null, "*, (SELECT Name FROM aspnet_MemberGrades WHERE GradeId = m.GradeId) AS GradeName ,(select COUNT(*) from  dbo.vw_VShop_FinishOrder_Main where UserId=m.UserId) as OrderCount ,(select SUM(ValidOrderTotal) from  dbo.vw_VShop_FinishOrder_Main where UserId=m.UserId) as OrderTotal,(select StoreName from  dbo.aspnet_Distributors where UserId=m.ReferralUserId ) as StoreName");
        }

        public IList<MemberInfo> GetMembersByRank(int? gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members");
            if (gradeId.HasValue && (gradeId.Value > 0))
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" WHERE GradeId={0} AND Status={1}", gradeId.Value, Convert.ToInt32(UserStatus.Normal));
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MemberInfo>(reader);
            }
        }

        public DataTable GetMembersByUserId(int referralUserId, int pageIndex, int pageSize, out int total, string sort, string order)
        {
            DataTable table;
            total = 0;
            string query = string.Format(" SELECT count(*) FROM aspnet_Members s left join aspnet_Distributors d on s.userid=d.userid  where  s.ReferralUserId={0}  and Status in(1,9) and d.StoreName is null ", referralUserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                total = (int) obj2;
            }
            int num = ((pageIndex - 1) * pageSize) + 1;
            int num2 = (num + pageSize) - 1;
            StringBuilder builder = new StringBuilder();
            builder.Append("select * from (");
            builder.AppendFormat("select ROW_NUMBER()  OVER (ORDER BY {0} {1}) as [RowIndex],* From (", sort, order);
            builder.AppendFormat("SELECT * FROM (", new object[0]);
            builder.AppendFormat("select s.userid,username", new object[0]);
            builder.AppendFormat(",(SELECT Name FROM aspnet_MemberGrades WHERE GradeId = s.GradeId) AS GradeName", new object[0]);
            builder.AppendFormat(",UserHead,CreateDate,s.ReferralUserId,d.StoreName", new object[0]);
            builder.AppendFormat(", (select count(*) from Hishop_Orders where UserId=s.UserId and OrderStatus<>4 and PayDate is not null ) as OrderMumber", new object[0]);
            builder.AppendFormat(", LastOrderDate as OrderDate", new object[0]);
            builder.AppendFormat(", (select SUM(OrderTotal) from Hishop_Orders where UserId=s.UserId and OrderStatus<>4 and PayDate is not null ) as OrdersTotal ", new object[0]);
            builder.AppendFormat("from aspnet_Members s left join aspnet_Distributors d on s.userid=d.userid ", new object[0]);
            builder.AppendFormat("where  s.ReferralUserId={0} and Status in(1,9) and d.StoreName is null ", referralUserId);
            builder.AppendFormat(")  AS W ", new object[0]);
            builder.AppendFormat(") AS M ", new object[0]);
            builder.AppendFormat(") AS K  WHERE  RowIndex between {0} and {1}", num, num2);
            string str2 = builder.ToString();
            DbCommand command = this.database.GetSqlStringCommand(str2);
            using (IDataReader reader = this.database.ExecuteReader(command))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }

        public DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
        {
            if (fields.Count == 0)
            {
                return null;
            }
            DataTable table = null;
            string str = string.Empty;
            foreach (string str2 in fields)
            {
                str = str + str2 + ",";
            }
            str = str.Substring(0, str.Length - 1);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT {0} FROM aspnet_Members WHERE   Status={1} ", str, Convert.ToInt32(UserStatus.Normal));
            if (!string.IsNullOrEmpty(query.Username))
            {
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", query.Username);
            }
            if (query.GradeId.HasValue)
            {
                builder.AppendFormat(" AND GradeId={0}", query.GradeId);
            }
            if (query.HasVipCard.HasValue)
            {
                if (query.HasVipCard.Value)
                {
                    builder.Append(" AND VipCardNumber is not null");
                }
                else
                {
                    builder.Append(" AND VipCardNumber is null");
                }
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                builder.AppendFormat(" AND Realname LIKE '%{0}%'", query.Realname);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }

        public IList<MemberInfo> GetMemdersByCardNumbers(string cards)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_Members WHERE VipCardNumber IN ({0}) AND Status={1} ", cards, Convert.ToInt32(UserStatus.Normal)));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MemberInfo>(reader);
            }
        }

        public IList<MemberInfo> GetMemdersByOpenIds(string openids)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_Members where openid IN ({0}) AND Status={1}", openids, Convert.ToInt32(UserStatus.Normal)));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MemberInfo>(reader);
            }
        }

        public int GetMemeberNumBySearch(string gradeIds, string referralUserId, string beginCreateDate, string endCreateDate, int userType, string customGroup, int adminId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_SendCouponUser");
            this.database.AddInParameter(storedProcCommand, "@GradeIds", DbType.String, gradeIds);
            this.database.AddInParameter(storedProcCommand, "@ReferralUserId", DbType.String, referralUserId);
            this.database.AddInParameter(storedProcCommand, "@BeginCreateDate", DbType.String, beginCreateDate);
            this.database.AddInParameter(storedProcCommand, "@EndCreateDate", DbType.String, endCreateDate);
            this.database.AddInParameter(storedProcCommand, "@UserType", DbType.Int32, userType);
            this.database.AddInParameter(storedProcCommand, "@CustomGroupIds", DbType.String, customGroup);
            this.database.AddInParameter(storedProcCommand, "@AdminId", DbType.Int32, adminId);
            this.database.AddOutParameter(storedProcCommand, "@Count", DbType.Int32, 4);
            this.database.ExecuteNonQuery(storedProcCommand);
            object obj2 = storedProcCommand.Parameters["@Count"].Value;
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int) obj2;
            }
            return 0;
        }

        public IList<int> GetNewGroupMemberList()
        {
            IList<int> list = new List<int>();
            string query = "SELECT UserId FROM aspnet_Members WHERE LastOrderDate IS NULL";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    int item = Convert.ToInt32(row["UserId"]);
                    list.Add(item);
                }
            }
            return list;
        }

        public string GetOpenIDByUserId(int UserId)
        {
            string query = string.Format("Select top 1 OpenId  from  aspnet_Members where  UserId={0}  ", UserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (string) obj2;
            }
            return "";
        }

        public MemberInfo GetOpenIdMember(string openId, string From = "wx")
        {
            DbCommand sqlStringCommand = null;
            if (From == "fuwu")
            {
                if (openId.Length > 0x10)
                {
                    sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE AlipayOpenid = @openId ");
                }
                else
                {
                    sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE AlipayUserId = @openId ");
                }
            }
            else
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE openId = @openId");
            }
            this.database.AddInParameter(sqlStringCommand, "openId", DbType.String, openId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MemberInfo>(reader);
            }
        }

        public IList<int> GetPayDateIntervalMemberList(DateTime? start, DateTime? end)
        {
            IList<int> list = new List<int>();
            string query = "SELECT m.UserId FROM aspnet_Members AS m\r\n                           LEFT JOIN \r\n                          (SELECT MAX(PayDate) AS PayDate,UserId FROM dbo.vw_VShop_FinishOrder_Main GROUP BY UserId) AS v \r\n                           ON m.UserId=v.UserId WHERE 1=1 ";
            if (start.HasValue)
            {
                object obj2 = query;
                query = string.Concat(new object[] { obj2, " AND datediff(dd,'", start.Value, "',v.PayDate) >= 0" });
            }
            if (end.HasValue)
            {
                object obj3 = query;
                query = string.Concat(new object[] { obj3, " AND datediff(dd,'", end.Value, "',v.PayDate) <= 0" });
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    int item = Convert.ToInt32(row["UserId"]);
                    list.Add(item);
                }
            }
            return list;
        }

        public IList<int> GetSleepGroupMemberList()
        {
            IList<int> list = new List<int>();
            int activeDay = this.GetActiveDay();
            string query = string.Format("SELECT UserId FROM aspnet_Members WHERE PayOrderDate is null or PayOrderDate <'" + DateTime.Now.AddDays((double) -activeDay).ToString("yyyy-MM-dd HH:mm:ss") + "' ", new object[0]);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    int item = Convert.ToInt32(row["UserId"]);
                    list.Add(item);
                }
            }
            return list;
        }

        public IList<int> GetStoreNameMemberList(string StoreName)
        {
            IList<int> list = new List<int>();
            string query = "SELECT UserId FROM dbo.aspnet_Members m WHERE 1=1 ";
            if (StoreName == "主店")
            {
                query = query + " AND ReferralUserId not in (select userid from dbo.aspnet_Distributors where UserId=m.ReferralUserId)";
            }
            else
            {
                query = query + " AND ReferralUserId in (SELECT UserId  FROM aspnet_Distributors where storename=@StoreName)";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            if (StoreName != "主店")
            {
                this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, StoreName);
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    int item = Convert.ToInt32(row["UserId"]);
                    list.Add(item);
                }
            }
            return list;
        }

        public DataTable GetTop50NotTopRegionIdBind()
        {
            string query = "select UserID,regionId FROM [aspnet_Members]  where TopRegionId=0 and regionId>0";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public IList<int> GetTradeMoneyIntervalMemberList(decimal? start, decimal? end)
        {
            IList<int> list = new List<int>();
            string query = "SELECT m.UserId FROM aspnet_Members AS m\r\n                           LEFT JOIN \r\n                          (SELECT SUM(ValidOrderTotal) AS ValidOrderTotal,UserId FROM dbo.vw_VShop_FinishOrder_Main GROUP BY UserId) AS v \r\n                           ON m.UserId=v.UserId WHERE 1=1 ";
            if (start.HasValue)
            {
                query = query + " AND v.ValidOrderTotal >= " + start.Value;
            }
            if (end.HasValue)
            {
                query = query + " AND v.ValidOrderTotal <= " + end.Value;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    int item = Convert.ToInt32(row["UserId"]);
                    list.Add(item);
                }
            }
            return list;
        }

        public IList<int> GetTradeNumIntervalMemberList(int? start, int? end)
        {
            IList<int> list = new List<int>();
            string query = "SELECT m.UserId FROM aspnet_Members AS m\r\n                           LEFT JOIN \r\n                          (SELECT SUM(1) AS OrderCount,UserId FROM dbo.vw_VShop_FinishOrder_Main GROUP BY UserId) AS v \r\n                           ON m.UserId=v.UserId WHERE 1=1 ";
            if (start.HasValue)
            {
                query = query + " AND v.OrderCount >= " + start.Value;
            }
            if (end.HasValue)
            {
                query = query + " AND v.OrderCount <= " + end.Value;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    int item = Convert.ToInt32(row["UserId"]);
                    list.Add(item);
                }
            }
            return list;
        }

        public int GetUserFollowStateByUserId(int UserId, string type = "wx")
        {
            int num = 0;
            string query = "";
            if (type == "wx")
            {
                query = string.Format("Select top 1 IsFollowWeixin  from  aspnet_Members where  UserId={0}  ", UserId);
            }
            else
            {
                query = string.Format("Select top 1 IsFollowAlipay  from  aspnet_Members where  UserId={0}  ", UserId);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                num = (int) obj2;
            }
            return num;
        }

        public MemberInfo GetusernameMember(string username)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserBindName = @UserBindName ");
            this.database.AddInParameter(sqlStringCommand, "UserBindName", DbType.String, username);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MemberInfo>(reader);
            }
        }

        public bool Huifu(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  aspnet_Members SET Status=1 WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool InsertClientSet(Dictionary<int, MemberClientSet> clientsets)
        {
            StringBuilder builder = new StringBuilder("DELETE FROM  [Hishop_MemberClientSet];");
            foreach (KeyValuePair<int, MemberClientSet> pair in clientsets)
            {
                string str = "";
                string str2 = "";
                if (pair.Value.StartTime.HasValue)
                {
                    str = pair.Value.StartTime.Value.ToString("yyyy-MM-dd");
                }
                if (pair.Value.EndTime.HasValue)
                {
                    str2 = pair.Value.EndTime.Value.ToString("yyyy-MM-dd");
                }
                builder.AppendFormat(string.Concat(new object[] { "INSERT INTO Hishop_MemberClientSet(ClientTypeId,StartTime,EndTime,LastDay,ClientChar,ClientValue) VALUES (", pair.Key, ",'", str, "','", str2, "',", pair.Value.LastDay, ",'", pair.Value.ClientChar, "',", pair.Value.ClientValue, ");" }), new object[0]);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool IsExitOpenId(string openId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(*) FROM aspnet_Members WHERE OpenId = @OpenId");
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public bool IsFuwuFollowUser(string openid)
        {
            string query = "select top 1 openid from Vshop_FollowUsers where OpenId=@OpenId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openid);
            return (this.database.ExecuteScalar(sqlStringCommand) != null);
        }

        public bool ReSetUserHead(string userid, string wxName, string wxHead, string Openid = "")
        {
            string str = "";
            if (!string.IsNullOrEmpty(Openid.Trim()))
            {
                str = ",OpenId=@OpenId ";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET UserName = @UserName,UserHead = @UserHead " + str + " WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, wxName);
            this.database.AddInParameter(sqlStringCommand, "UserHead", DbType.String, wxHead);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            if (!string.IsNullOrEmpty(Openid.Trim()))
            {
                this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, Openid);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SaveMemberInfoByAddress(ShippingAddressInfo info)
        {
            MemberInfo member = null;
            if (info != null)
            {
                member = this.GetMember(info.UserId);
            }
            if (member == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(member.RealName) && !string.IsNullOrEmpty(info.ShipTo))
            {
                member.RealName = info.ShipTo;
                member.CellPhone = info.CellPhone;
                member.Address = info.Address;
                member.RegionId = info.RegionId;
                member.TopRegionId = RegionHelper.GetTopRegionId(info.RegionId);
            }
            return this.Update(member);
        }

        public int SetAlipayInfos(MemberInfo user)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET AlipayLoginId = @AlipayLoginId,AlipayOpenid = @AlipayOpenid,AlipayUserId = @AlipayUserId,AlipayUsername = @AlipayUsername,AlipayAvatar = @AlipayAvatar  WHERE UserId=@UserId ");
            this.database.AddInParameter(sqlStringCommand, "AlipayUserId", DbType.String, user.AlipayUserId);
            this.database.AddInParameter(sqlStringCommand, "AlipayOpenid", DbType.String, user.AlipayOpenid);
            this.database.AddInParameter(sqlStringCommand, "AlipayLoginId", DbType.String, user.AlipayLoginId);
            this.database.AddInParameter(sqlStringCommand, "AlipayUsername", DbType.String, user.AlipayUsername);
            this.database.AddInParameter(sqlStringCommand, "AlipayAvatar", DbType.String, user.AlipayAvatar);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, user.UserId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool SetMemberSessionId(string sessionId, DateTime sessionEndTime, string openId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET SessionId = @SessionId, SessionEndTime = @SessionEndTime WHERE OpenId = @OpenId");
            this.database.AddInParameter(sqlStringCommand, "SessionId", DbType.String, sessionId);
            this.database.AddInParameter(sqlStringCommand, "SessionEndTime", DbType.DateTime, sessionEndTime);
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int SetMultiplePwd(string userids, string pwd)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Password = @Password  WHERE UserId in(" + userids + ")");
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, pwd);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public int SetOrderDate(int userID, int orderType)
        {
            string str = string.Empty;
            switch (orderType)
            {
                case 0:
                    str = "LastOrderDate";
                    break;

                case 1:
                    str = "PayOrderDate";
                    break;

                case 2:
                    str = "FinishOrderDate";
                    break;

                default:
                    str = "LastOrderDate";
                    break;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[] { "UPDATE aspnet_Members SET ", str, " = getdate()  WHERE UserId =", userID }));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool SetPoint(int userid, int point)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Points = Points+@Points  WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, point);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetPwd(string userid, string pwd)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Password = @Password  WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, pwd);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int SetRegion(string userID, int regionId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET ReferralUserId = @ReferralUserId  WHERE UserId =" + userID);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, regionId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public int SetRegions(string userIDs, int regionId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET ReferralUserId = @ReferralUserId  WHERE UserId in(" + userIDs + ")");
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, regionId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool SetUserHeadAndUserName(string OpenId, string UserHead, string UserName, int IsAuthorizeWeiXin = 1)
        {
            string str = string.Empty;
            switch (IsAuthorizeWeiXin)
            {
                case -1:
                    str = "UPDATE  aspnet_Members SET IsAuthorizeWeiXin=0 where OpenId = @OpenId ";
                    break;

                case 0:
                    str = "UPDATE  aspnet_Members SET UserName = @UserName,UserHead=@UserHead where OpenId = @OpenId ";
                    break;

                case 1:
                    str = "UPDATE  aspnet_Members SET UserName = @UserName,UserHead=@UserHead,IsAuthorizeWeiXin=1 where OpenId = @OpenId ";
                    break;

                case 2:
                    str = "UPDATE  aspnet_Members SET IsAuthorizeWeiXin=1 where OpenId = @OpenId ";
                    break;
            }
            if (!string.IsNullOrEmpty(str))
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
                this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, OpenId);
                this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, UserName);
                this.database.AddInParameter(sqlStringCommand, "UserHead", DbType.String, UserHead);
                bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
                str = "UPDATE  aspnet_Members SET Status=1 where OpenId = @OpenId and Status=@Status";
                sqlStringCommand = this.database.GetSqlStringCommand(str);
                this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, OpenId);
                this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, Convert.ToInt32(UserStatus.Visitor));
                this.database.ExecuteNonQuery(sqlStringCommand);
                return flag;
            }
            return false;
        }

        public int SetUsersGradeId(string userId, int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[] { "UPDATE  aspnet_Members SET GradeId=", gradeId, "  WHERE UserId in (", userId, ")" }));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool Update(MemberInfo member)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(member.OpenId))
            {
                str = ",OpenId = @OpenId";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET GradeId = @GradeId" + str + ",UserName = @UserName, RealName = @RealName, TopRegionId = @TopRegionId, RegionId = @RegionId,VipCardNumber = @VipCardNumber, VipCardDate = @VipCardDate, Email = @Email, CellPhone = @CellPhone, QQ = @QQ, Address = @Address, Expenditure = @Expenditure, OrderNumber = @OrderNumber,MicroSignal=@MicroSignal,UserHead=@UserHead,CardID=@CardID WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, member.OpenId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, member.UserName);
            this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, member.RealName);
            this.database.AddInParameter(sqlStringCommand, "TopRegionId", DbType.Int32, member.TopRegionId);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, member.RegionId);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, member.Email);
            this.database.AddInParameter(sqlStringCommand, "VipCardNumber", DbType.String, member.VipCardNumber);
            this.database.AddInParameter(sqlStringCommand, "VipCardDate", DbType.DateTime, member.VipCardDate);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, member.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "QQ", DbType.String, member.QQ);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, member.Address);
            this.database.AddInParameter(sqlStringCommand, "Expenditure", DbType.Currency, member.Expenditure);
            this.database.AddInParameter(sqlStringCommand, "OrderNumber", DbType.Int32, member.OrderNumber);
            this.database.AddInParameter(sqlStringCommand, "MicroSignal", DbType.String, member.MicroSignal);
            this.database.AddInParameter(sqlStringCommand, "UserHead", DbType.String, member.UserHead);
            this.database.AddInParameter(sqlStringCommand, "CardID", DbType.String, member.CardID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateUserFollowStateByOpenId(string openid, int state)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(openid))
            {
                string query = "update aspnet_Members set IsFollowWeixin=" + state + " where  OpenId=@OpenId";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openid);
                flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            }
            return flag;
        }

        public bool UpdateUserFollowStateByUserId(int UserId, int state, string type = "wx")
        {
            string query = "";
            if (type == "wx")
            {
                query = string.Format(" set  IsFollowWeixin={0} ", state);
            }
            else
            {
                query = string.Format(" set  IsFollowAlipay={0} ", state);
            }
            query = "update  aspnet_Members " + query + string.Format(" where  UserId={0}  ", UserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

