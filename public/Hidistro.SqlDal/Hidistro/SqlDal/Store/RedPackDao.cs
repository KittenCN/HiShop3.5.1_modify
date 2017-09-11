namespace Hidistro.SqlDal.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.SqlDal.Members;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Domain;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class RedPackDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public void RedPackCheckJob()
        {
            new StringBuilder();
            DateTime time = DateTime.Now.Date.AddDays(-3.0);
            DataTable table = new DataTable();
            string query = "select UserId,SerialID,RedpackId,Amount from Hishop_BalanceDrawRequest WHERE IsCheck=2 AND RequestType=3 AND CheckTime>=@CheckTime";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "CheckTime", DbType.DateTime, time);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            RedPackClient client = new RedPackClient();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            DistributorsDao dao = new DistributorsDao();
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    string str2 = row["RedpackId"].ToString();
                    RedPackInfo info = client.GetRedpackInfo(masterSettings.WeixinAppId, masterSettings.WeixinPartnerID, str2, masterSettings.WeixinPartnerKey, masterSettings.WeixinCertPath, masterSettings.WeixinCertPassword);
                    if (info != null)
                    {
                        redPackStatus status = info.Getstatus();
                        if (((int)status == 4) || ((int)status == 2))
                        {
                            int id = int.Parse(row["SerialID"].ToString());
                            Globals.Debuglog(string.Concat(new object[] { "BalanceDrawRequest-", id, ":", info.ToString() }), "RedPackCheck.txt");
                            decimal num2 = decimal.Parse(row["Amount"].ToString());
                            int userId = int.Parse(row["UserId"].ToString());
                            dao.UpdateBalanceDistributors(userId, -1M * num2);
                            dao.UpdateRedPackStatus(id, "红包" + status.ToString(), null);
                        }
                    }
                }
            }
            query = "select UserId,Id,RedpackId,Amount from Hishop_MemberAmountRequest WHERE State=2 AND RequestType=3 AND CheckTime>=@CheckTime";
            AmountDao dao2 = new AmountDao();
            sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "CheckTime", DbType.DateTime, time);
            using (IDataReader reader2 = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader2);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row2 in table.Rows)
                {
                    string str3 = row2["RedpackId"].ToString();
                    RedPackInfo info2 = client.GetRedpackInfo(masterSettings.WeixinAppId, masterSettings.WeixinPartnerID, str3, masterSettings.WeixinPartnerKey, masterSettings.WeixinCertPath, masterSettings.WeixinCertPassword);
                    if (info2 != null)
                    {
                        redPackStatus status2 = info2.Getstatus();
                        if (((int)status2 == 4) || ((int)status2 == 2))
                        {
                            int num4 = int.Parse(row2["Id"].ToString());
                            Globals.Debuglog(string.Concat(new object[] { "MemberAmountRequest-", num4, ":", info2.ToString() }), "RedPackCheck.txt");
                            decimal.Parse(row2["Amount"].ToString());
                            int.Parse(row2["UserId"].ToString());
                            dao2.SetAmountRequestStatus(new int[] { num4 }, 3, "红包" + status2.ToString(), "", "SYSJOB");
                        }
                    }
                }
            }
        }
    }
}

