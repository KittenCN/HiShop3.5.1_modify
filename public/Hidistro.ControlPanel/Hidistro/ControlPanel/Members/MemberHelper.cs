namespace Hidistro.ControlPanel.Members
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Members;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;

    public static class MemberHelper
    {
        public static bool BacthHuifu(string userId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            return new MemberDao().BatchHuifu(userId);
        }

        public static IList<string> BatchCreateMembers(IList<string> distributornames, int referruserId, string createtype = "1")
        {
            string referralPath = string.Empty;
            IList<string> list = new List<string>();
            DistributorGrade threeDistributor = DistributorGrade.ThreeDistributor;
            if (referruserId > 0)
            {
                referralPath = new DistributorsDao().GetDistributorInfo(referruserId).ReferralPath;
                if (string.IsNullOrEmpty(referralPath))
                {
                    referralPath = referruserId.ToString();
                    threeDistributor = DistributorGrade.TowDistributor;
                }
                else if (referralPath.Contains("|"))
                {
                    referralPath = referralPath.Split(new char[] { '|' })[1] + "|" + referruserId.ToString();
                }
                else
                {
                    referralPath = referralPath + "|" + referruserId.ToString();
                }
            }
            foreach (string str2 in distributornames)
            {
                MemberInfo member = new MemberInfo();
                string generateId = Globals.GetGenerateId();
                member.GradeId = new MemberGradeDao().GetDefaultMemberGrade();
                member.UserName = str2;
                member.CreateDate = DateTime.Now;
                member.UserBindName = str2;
                member.SessionId = generateId;
                member.ReferralUserId = Convert.ToInt32(referruserId);
                member.SessionEndTime = DateTime.Now.AddYears(10);
                member.Password = HiCryptographer.Md5Encrypt("888888");
                member.UserHead = "/templates/common/images/user.png";
                if ((new MemberDao().GetusernameMember(str2) == null) && new MemberDao().CreateMember(member))
                {
                    DistributorsInfo distributor = new DistributorsInfo {
                        UserId = new MemberDao().GetusernameMember(str2).UserId,
                        RequestAccount = "",
                        StoreName = str2,
                        StoreDescription = "",
                        BackImage = "",
                        Logo = "",
                        DistributorGradeId = threeDistributor
                    };
                    distributor.UserId.ToString();
                    distributor.ReferralPath = referralPath;
                    distributor.ParentUserId = new int?(Convert.ToInt32(referruserId));
                    DistributorGradeInfo isDefaultDistributorGradeInfo = new DistributorsDao().GetIsDefaultDistributorGradeInfo();
                    distributor.DistriGradeId = isDefaultDistributorGradeInfo.GradeId;
                    if (new DistributorsDao().CreateDistributor(distributor) && (createtype == "1"))
                    {
                        list.Add(str2);
                    }
                }
                else if (createtype == "2")
                {
                    list.Add(str2);
                }
            }
            return list;
        }

        public static bool BindUserName(int UserId, string UserBindName, string Password)
        {
            MemberDao dao = new MemberDao();
            return dao.BindUserName(UserId, UserBindName, Password);
        }

        public static int CanChangeBindWeixin()
        {
            if (GetBindOpenIDAndNoUserNameCount() > 0)
            {
                return 1;
            }
            if (GetBindOpenIDCount() > 0)
            {
                return 3;
            }
            return 2;
        }

        public static bool CheckCurrentMemberIsInRange(string Grades, string DefualtGroup, string CustomGroup, int userid)
        {
            return new MemberDao().CheckCurrentMemberIsInRange(Grades, DefualtGroup, CustomGroup, userid);
        }

        public static bool ClearAllAlipayopenId()
        {
            return new MemberDao().ClearAllOpenId("fuwu");
        }

        public static bool ClearAllOpenId()
        {
            return new MemberDao().ClearAllOpenId("wx");
        }

        public static bool CreateDistributorByUserIds(string userids, ref string msg)
        {
            bool flag = false;
            userids = userids.Trim(new char[] { ',' });
            string[] strArray = userids.Split(new char[] { ',' });
            if (strArray.Length == 0)
            {
                msg = "没有会员被选择！";
                return flag;
            }
            DistributorGradeInfo isDefaultDistributorGradeInfo = new DistributorGradeDao().GetIsDefaultDistributorGradeInfo();
            if (isDefaultDistributorGradeInfo == null)
            {
                msg = "默认分销商等级未设置,无法生成分销商!";
                return flag;
            }
            Dictionary<int, bool> existDistributorList = new DistributorsDao().GetExistDistributorList(userids);
            List<int> list = new List<int>();
            foreach (string str in strArray)
            {
                int result = 0;
                if (int.TryParse(str, out result) && !existDistributorList.ContainsKey(result))
                {
                    list.Add(result);
                }
            }
            if (list.Count == 0)
            {
                msg = "选择的会员已经是分销商，操作终止！";
                return flag;
            }
            int num2 = 0;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            foreach (int num3 in list)
            {
                int userId = num3;
                MemberInfo member = GetMember(userId);
                int referralUserId = member.ReferralUserId;
                string str2 = string.Empty;
                DistributorsInfo distributor = new DistributorsInfo {
                    DistributorGradeId = DistributorGrade.OneDistributor
                };
                if (referralUserId > 0)
                {
                    DistributorsInfo distributorInfo = new DistributorsDao().GetDistributorInfo(referralUserId);
                    if (distributorInfo != null)
                    {
                        if (!string.IsNullOrEmpty(distributorInfo.ReferralPath) && !distributorInfo.ReferralPath.Contains("|"))
                        {
                            str2 = distributorInfo.ReferralPath + "|" + distributorInfo.UserId.ToString();
                        }
                        else if (!string.IsNullOrEmpty(distributorInfo.ReferralPath) && distributorInfo.ReferralPath.Contains("|"))
                        {
                            str2 = distributorInfo.ReferralPath.Split(new char[] { '|' })[1] + "|" + distributorInfo.UserId.ToString();
                        }
                        else
                        {
                            str2 = distributorInfo.UserId.ToString();
                        }
                        if (!string.IsNullOrEmpty(distributorInfo.Logo))
                        {
                            distributor.Logo = distributorInfo.Logo;
                        }
                        if (distributorInfo.DistributorGradeId == DistributorGrade.OneDistributor)
                        {
                            distributor.DistributorGradeId = DistributorGrade.TowDistributor;
                        }
                        else if (distributorInfo.DistributorGradeId == DistributorGrade.TowDistributor)
                        {
                            distributor.DistributorGradeId = DistributorGrade.ThreeDistributor;
                        }
                        else
                        {
                            distributor.DistributorGradeId = DistributorGrade.ThreeDistributor;
                        }
                    }
                }
                if (string.IsNullOrEmpty(distributor.Logo))
                {
                    distributor.Logo = masterSettings.DistributorLogoPic;
                }
                distributor.UserId = member.UserId;
                distributor.RequestAccount = "";
                distributor.StoreName = Globals.GetStoreNameByUserIDAndName(member.UserId, member.UserName, member.OpenId, masterSettings.SiteName);
                distributor.StoreDescription = "";
                distributor.BackImage = "";
                distributor.DistriGradeId = isDefaultDistributorGradeInfo.GradeId;
                distributor.ReferralPath = str2;
                distributor.ParentUserId = new int?(Convert.ToInt32(referralUserId));
                if (new DistributorsDao().CreateDistributor(distributor))
                {
                    num2++;
                }
            }
            if (num2 > 0)
            {
                msg = "成功生成" + num2.ToString() + "位分销商，请检查！";
                return true;
            }
            msg = "生成分销商失败！";
            return false;
        }

        public static bool CreateMemberGrade(MemberGradeInfo memberGrade)
        {
            if (memberGrade == null)
            {
                return false;
            }
            Globals.EntityCoding(memberGrade, true);
            if (!IsCanSetThisGrade(memberGrade))
            {
                throw new Exception("交易次数的上下级别，与交易额的上下级别不是同一个！");
            }
            bool flag = new MemberGradeDao().CreateMemberGrade(memberGrade);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.AddMemberGrade, string.Format(CultureInfo.InvariantCulture, "添加了名为 “{0}” 的会员等级", new object[] { memberGrade.Name }));
            }
            return flag;
        }

        public static bool Delete(int userId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            bool flag = new MemberDao().Delete(userId);
            if (flag)
            {
                HiCache.Remove(string.Format("DataCache-Member-{0}", userId));
                EventLogs.WriteOperationLog(Privilege.DeleteMember, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的会员", new object[] { userId }));
            }
            return flag;
        }

        public static bool Delete2(int userId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            bool flag = new MemberDao().Delete2(userId);
            if (flag)
            {
                HiCache.Remove(string.Format("DataCache-Member-{0}", userId));
                EventLogs.WriteOperationLog(Privilege.DeleteMember, string.Format(CultureInfo.InvariantCulture, "逻辑删除了编号为 “{0}” 的会员", new object[] { userId }));
            }
            return flag;
        }

        public static bool DeleteMemberGrade(int gradeId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMemberGrade);
            bool flag = new MemberGradeDao().DeleteMemberGrade(gradeId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteMemberGrade, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的会员等级", new object[] { gradeId }));
            }
            return flag;
        }

        public static bool Deletes(string userId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            return new MemberDao().Deletes(userId);
        }

        public static int GetActiveDay()
        {
            return new MemberDao().GetActiveDay();
        }

        public static string GetAliUserOpenIdByUserId(int UserId)
        {
            MemberDao dao = new MemberDao();
            return dao.GetAliOpenIDByUserId(UserId);
        }

        public static string GetAllDistributorsName(string keysearch)
        {
            string str = "";
            foreach (DataRow row in new DistributorsDao().GetAllDistributorsName(keysearch).Rows)
            {
                string str2 = str;
                str = str2 + "{\"title\":\"" + Globals.HtmlEncode(row[0].ToString()) + "\",\"result\":\"" + row[1].ToString() + "\"},";
            }
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        public static int GetBindOpenIDAndNoUserNameCount()
        {
            return new MemberDao().GetBindOpenIDAndNoUserNameCount();
        }

        public static int GetBindOpenIDCount()
        {
            return new MemberDao().GetBindOpenIDCount();
        }

        public static string GetEnumDescription(Enum enumValue)
        {
            string name = enumValue.ToString();
            object[] customAttributes = enumValue.GetType().GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((customAttributes == null) || (customAttributes.Length == 0))
            {
                return name;
            }
            DescriptionAttribute attribute = (DescriptionAttribute) customAttributes[0];
            return attribute.Description;
        }

        public static Dictionary<int, string> GetEnumValueAndDescription(Type enumtype)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            foreach (object obj2 in Enum.GetValues(enumtype))
            {
                object[] customAttributes = obj2.GetType().GetField(obj2.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    DescriptionAttribute attribute = customAttributes[0] as DescriptionAttribute;
                    dictionary.Add(Convert.ToInt32(obj2), attribute.Description);
                }
            }
            return dictionary;
        }

        public static DbQueryResult GetIntegralDetail(IntegralDetailQuery query)
        {
            return new IntegralDetailDao().GetIntegralDetail(query);
        }

        public static MemberInfo GetMember(int userId)
        {
            return new MemberDao().GetMember(userId);
        }

        public static Dictionary<int, MemberClientSet> GetMemberClientSet()
        {
            return new MemberDao().GetMemberClientSet();
        }

        public static MemberGradeInfo GetMemberGrade(int gradeId)
        {
            return new MemberGradeDao().GetMemberGrade(gradeId);
        }

        public static IList<MemberGradeInfo> GetMemberGrades()
        {
            return new MemberGradeDao().GetMemberGrades("");
        }

        public static IList<MemberGradeInfo> GetMemberGrades(string GradeIds = "")
        {
            return new MemberGradeDao().GetMemberGrades(GradeIds);
        }

        public static int GetMemberIdByUserNameOrNiChen(string username = "", string nich = "")
        {
            return new MemberDao().GetMemberIdByUserNameOrNiChen(username, nich);
        }

        public static DbQueryResult GetMembers(MemberQuery query, bool isNotBindUserName = false)
        {
            return new MemberDao().GetMembers(query, isNotBindUserName);
        }

        public static IList<MemberInfo> GetMembersByRank(int? gradeId)
        {
            return new MemberDao().GetMembersByRank(gradeId);
        }

        public static DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
        {
            return new MemberDao().GetMembersNopage(query, fields);
        }

        public static IList<MemberInfo> GetMemdersByCardNumbers(string cards)
        {
            return new MemberDao().GetMemdersByCardNumbers(cards);
        }

        public static int GetSystemDistributorsCount()
        {
            return new DistributorsDao().GetSystemDistributorsCount();
        }

        public static DataTable GetTop50NotTopRegionIdBind()
        {
            return new MemberDao().GetTop50NotTopRegionIdBind();
        }

        public static string GetUserOpenIdByUserId(int UserId)
        {
            MemberDao dao = new MemberDao();
            return dao.GetOpenIDByUserId(UserId);
        }

        public static bool HasSameMemberGrade(MemberGradeInfo memberGrade)
        {
            return new MemberGradeDao().HasSameMemberGrade(memberGrade);
        }

        public static bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
        {
            return new MemberGradeDao().HasSamePointMemberGrade(memberGrade);
        }

        public static bool huifu(int userId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            bool flag = new MemberDao().Huifu(userId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteMember, string.Format(CultureInfo.InvariantCulture, "恢复了编号为 “{0}” 的会员", new object[] { userId }));
            }
            return flag;
        }

        public static bool InsertClientSet(Dictionary<int, MemberClientSet> clientset)
        {
            return new MemberDao().InsertClientSet(clientset);
        }

        private static bool IsCanSetThisGrade(MemberGradeInfo memberGrade)
        {
            List<MemberGradeInfo> list = GetMemberGrades().ToList<MemberGradeInfo>();
            int num = 0;
            int num2 = 0;
            foreach (MemberGradeInfo info in list)
            {
                if (info.GradeId != memberGrade.GradeId)
                {
                    if ((info.TranVol.HasValue && memberGrade.TranVol.HasValue) && (info.TranVol.Value > memberGrade.TranVol.Value))
                    {
                        num++;
                    }
                    if ((info.TranTimes.HasValue && memberGrade.TranTimes.HasValue) && (info.TranTimes.Value > memberGrade.TranTimes.Value))
                    {
                        num2++;
                    }
                }
            }
            return (num == num2);
        }

        public static bool IsExist(string name)
        {
            return new MemberGradeDao().IsExist(name);
        }

        public static bool IsExistUserBindName(string userBindName)
        {
            return (new MemberDao().GetMemberIdByUserNameOrNiChen(userBindName, null) > 0);
        }

        public static int IsExiteDistributorNames(string distributorname)
        {
            return new DistributorsDao().IsExiteDistributorsByStoreName(distributorname);
        }

        public static int SelectUserCountGrades(int gid)
        {
            return new MemberGradeDao().SelectUserCountGrades(gid);
        }

        public static int SelectUserGroupSet()
        {
            return new MemberGradeDao().SelectUserGroupSet();
        }

        public static void SetDefalutMemberGrade(int gradeId)
        {
            new MemberGradeDao().SetDefalutMemberGrade(gradeId);
        }

        public static int SetOrderDate(int userID, int orderType)
        {
            return new MemberDao().SetOrderDate(userID, orderType);
        }

        public static int SetRegion(string userID, int regionId)
        {
            return new MemberDao().SetRegion(userID, regionId);
        }

        public static int SetRegions(string userIDs, int regionId)
        {
            return new MemberDao().SetRegions(userIDs, regionId);
        }

        public static int SetUserGroup(int day)
        {
            return new MemberGradeDao().SetUserGroup(day);
        }

        public static bool SetUserHeadAndUserName(string OpenId, string HUserHead, string UserName, int IsAuthorizeWeiXin = 1)
        {
            return new MemberDao().SetUserHeadAndUserName(OpenId, HUserHead, UserName, IsAuthorizeWeiXin);
        }

        public static int SetUsersGradeId(string userId, int gradeId)
        {
            return new MemberDao().SetUsersGradeId(userId, gradeId);
        }

        public static string StringToTradeType(string tradeType)
        {
            try
            {
                return GetEnumDescription((TradeType) int.Parse(tradeType));
            }
            catch
            {
                return "其他交易类型";
            }
        }

        public static string StringToTradeWays(string tradeWays)
        {
            try
            {
                return GetEnumDescription((TradeWays) int.Parse(tradeWays));
            }
            catch
            {
                return "其他交易方式";
            }
        }

        public static bool Update(MemberInfo member)
        {
            bool flag = new MemberDao().Update(member);
            if (flag)
            {
                HiCache.Remove(string.Format("DataCache-Member-{0}", member.UserId));
                EventLogs.WriteOperationLog(Privilege.EditMember, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的会员", new object[] { member.UserId }));
            }
            return flag;
        }

        public static bool UpdateMemberGrade(MemberGradeInfo memberGrade)
        {
            if (memberGrade == null)
            {
                return false;
            }
            Globals.EntityCoding(memberGrade, true);
            if (!IsCanSetThisGrade(memberGrade))
            {
                throw new Exception("交易次数的上下级别，与交易额的上下级别不是同一个！");
            }
            bool flag = new MemberGradeDao().UpdateMemberGrade(memberGrade);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditMemberGrade, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的会员等级", new object[] { memberGrade.GradeId }));
            }
            return flag;
        }

        public static void UpdateSetCardCreatTime()
        {
            new DistributorsDao().UpdateSetCardCreatTime();
        }
    }
}

