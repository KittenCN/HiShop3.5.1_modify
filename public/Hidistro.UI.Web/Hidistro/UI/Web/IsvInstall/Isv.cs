namespace Hidistro.UI.Web.IsvInstall
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.Configuration;
    using System.Xml;
    using System.Xml.Linq;

    public class Isv : IHttpHandler
    {
        private string AddInitData(string connectionstr)
        {
            string path = HttpContext.Current.Request.MapPath("~/Installer/SqlScripts/SiteInitData.zh-CN.Sql");
            if (!File.Exists(path))
            {
                return "没有找到初始化数据文件-SiteInitData.Sql";
            }
            return this.ExecuteScriptFile(path, connectionstr);
        }

        private string CreateAdministrator(string connectionstr)
        {
            DbConnection connection = new SqlConnection(connectionstr);
            connection.Open();
            DbCommand command = connection.CreateCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO aspnet_Roles(RoleName,IsDefault) VALUES('超级管理员',1); SELECT @@IDENTITY";
            int num = Convert.ToInt32(command.ExecuteScalar());
            command.CommandText = "INSERT INTO aspnet_Managers(RoleId, UserName, Password, Email, CreateDate) VALUES (@RoleId, @UserName, @Password, @Email, getdate())";
            command.Parameters.Add(new SqlParameter("@RoleId", num));
            command.Parameters.Add(new SqlParameter("@Username", "admin"));
            command.Parameters.Add(new SqlParameter("@Password", HiCryptographer.Md5Encrypt("123456")));
            command.Parameters.Add(new SqlParameter("@Email", "test@hishop.com"));
            command.ExecuteNonQuery();
            connection.Close();
            return "";
        }

        private string CreateDataSchema(string connectionstr)
        {
            string path = HttpContext.Current.Request.MapPath("~/Installer/SqlScripts/Schema.sql");
            if (!File.Exists(path))
            {
                return "没有找到数据库架构文件-Schema.sql";
            }
            return this.ExecuteScriptFile(path, connectionstr);
        }

        private static string CreateKey(int len)
        {
            byte[] data = new byte[len];
            new RNGCryptoServiceProvider().GetBytes(data);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(string.Format("{0:X2}", data[i]));
            }
            return builder.ToString();
        }

        private string ExecuteScriptFile(string pathToScriptFile, string connectionstr)
        {
            StreamReader reader = null;
            SqlConnection connection = null;
            try
            {
                string applicationPath = Globals.ApplicationPath;
                using (reader = new StreamReader(pathToScriptFile))
                {
                    using (connection = new SqlConnection(connectionstr))
                    {
                        SqlCommand command2 = new SqlCommand {
                            Connection = connection,
                            CommandType = CommandType.Text,
                            CommandTimeout = 60
                        };
                        DbCommand command = command2;
                        connection.Open();
                        while (!reader.EndOfStream)
                        {
                            string str = NextSqlFromStream(reader);
                            if (!string.IsNullOrEmpty(str))
                            {
                                command.CommandText = str.Replace("$VirsualPath$", applicationPath);
                                command.ExecuteNonQuery();
                            }
                        }
                        connection.Close();
                    }
                    reader.Close();
                }
                return "";
            }
            catch (SqlException exception)
            {
                if ((connection != null) && (connection.State != ConnectionState.Closed))
                {
                    connection.Close();
                    connection.Dispose();
                }
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                return exception.Message;
            }
        }

        private RijndaelManaged GetCryptographer()
        {
            RijndaelManaged managed = new RijndaelManaged {
                KeySize = 0x80
            };
            managed.GenerateIV();
            managed.GenerateKey();
            return managed;
        }

        private static string NextSqlFromStream(StreamReader reader)
        {
            StringBuilder builder = new StringBuilder();
            string strA = reader.ReadLine().Trim();
            while (!reader.EndOfStream && (string.Compare(strA, "GO", true, CultureInfo.InvariantCulture) != 0))
            {
                builder.Append(strA + Environment.NewLine);
                strA = reader.ReadLine();
            }
            if (string.Compare(strA, "GO", true, CultureInfo.InvariantCulture) != 0)
            {
                builder.Append(strA + Environment.NewLine);
            }
            return builder.ToString();
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.Expires = -1;
            switch (Globals.RequestQueryStr("action"))
            {
                case "setup":
                    this.SetUP();
                    return;

                case "addadmin":
                    this.WriteReturnCode(200, "ok");
                    return;
            }
            this.WriteReturnCode(200, "默认ok");
        }

        private string SaveConfig(string connectionstr)
        {
            try
            {
                System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                using (RijndaelManaged managed = this.GetCryptographer())
                {
                    configuration.AppSettings.Settings["IV"].Value = Convert.ToBase64String(managed.IV);
                    configuration.AppSettings.Settings["Key"].Value = Convert.ToBase64String(managed.Key);
                }
                MachineKeySection section = (MachineKeySection) configuration.GetSection("system.web/machineKey");
                section.ValidationKey = CreateKey(20);
                section.DecryptionKey = CreateKey(0x18);
                section.Validation = MachineKeyValidation.SHA1;
                section.Decryption = "3DES";
                configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = connectionstr;
                configuration.ConnectionStrings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                configuration.Save();
                configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                configuration.AppSettings.Settings.Remove("Installer");
                configuration.Save();
                return "";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        private string SaveSiteSettings()
        {
            try
            {
                string filename = HttpContext.Current.Request.MapPath("~/config/SiteSettings.config");
                XmlDocument doc = new XmlDocument();
                SiteSettings settings = new SiteSettings(HttpContext.Current.Request.Url.Host);
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine + "<Settings></Settings>");
                settings.VTheme = "t1";
                settings.SiteName = "微信分销大师";
                settings.CheckCode = CreateKey(20);
                settings.DistributorLogoPic = "/utility/pics/headLogo.jpg";
                settings.DistributorsMenu = true;
                settings.EnableShopMenu = true;
                settings.ShopDefault = true;
                settings.MemberDefault = true;
                settings.GoodsType = true;
                settings.GoodsCheck = true;
                settings.ActivityMenu = true;
                settings.ActivityMenu = true;
                settings.BrandMenu = true;
                settings.GoodsListMenu = true;
                settings.OrderShowDays = 7;
                settings.CloseOrderDays = 3;
                settings.FinishOrderDays = 7;
                settings.MaxReturnedDays = 15;
                settings.TaxRate = 0M;
                settings.PointsRate = 1M;
                settings.IsValidationService = true;
                settings.SMSSender = "";
                settings.SMSSettings = "";
                settings.ShopMenuStyle = "1";
                settings.EnablePodRequest = false;
                settings.EnableCommission = true;
                settings.EnableAlipayRequest = false;
                settings.EnableWeiXinRequest = false;
                settings.EnableOffLineRequest = true;
                settings.EnableWapShengPay = false;
                settings.OffLinePayContent = "<p>请填写在线支付帮助内容</p>";
                settings.DistributorDescription = "<p><img src=\"/utility/pics/fxs.png\" title=\"fenxiao.png\" alt=\"fenxiao.png\"/></p>";
                settings.DistributorBackgroundPic = "/Storage/data/DistributorBackgroundPic/default.jpg|";
                settings.SaleService = "<p>请填写售后服务内容</p>";
                settings.MentionNowMoney = "1";
                settings.Disabled = false;
                settings.ShareAct_Enable = true;
                settings.SignPoint = 10;
                settings.SignWherePoint = 10;
                settings.SignWhere = 10;
                settings.ActiveDay = 1;
                settings.sign_EverDayScore = 50;
                settings.sign_StraightDay = 2;
                settings.sign_RewardScore = 20;
                settings.sign_score_Enable = true;
                settings.shopping_reward_Enable = true;
                settings.shopping_score_Enable = true;
                settings.shopping_Score = 100;
                settings.shopping_reward_Score = 1;
                settings.shopping_reward_OrderValue = 100.0;
                settings.share_score_Enable = true;
                settings.share_Score = 100;
                settings.PointToCashRate = 100;
                settings.PonitToCash_Enable = true;
                settings.PonitToCash_MaxAmount = 1000M;
                settings.DrawPayType = "1|0|2|3";
                settings.BatchAliPay = true;
                settings.BatchWeixinPay = true;
                settings.BatchWeixinPayCheckRealName = 0;
                settings.EnableSaleService = false;
                settings.ServiceMeiQia = "";
                settings.App_Secret = "836e49139e90c64f21251a6dec9c2cca";
                settings.WriteToXml(doc);
                doc.Save(filename);
                return "";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        private void SetUP()
        {
            if (ConfigurationManager.AppSettings["Installer"] == null)
            {
                this.WriteReturnCode(700, "已完成安装");
            }
            else
            {
                string connectionstr = string.Format("server={0};uid={1};pwd={2};Trusted_Connection=no;database={3}", new object[] { Globals.RequestQueryStr("dbhost"), Globals.RequestQueryStr("dbuser"), Globals.RequestQueryStr("dbpassword"), Globals.RequestQueryStr("dbname") });
                string str2 = string.Empty;
                str2 = this.CreateDataSchema(connectionstr);
                if (!string.IsNullOrEmpty(str2))
                {
                    this.WriteReturnCode(100, str2);
                }
                str2 = this.CreateAdministrator(connectionstr);
                if (!string.IsNullOrEmpty(str2))
                {
                    this.WriteReturnCode(300, str2);
                }
                str2 = this.AddInitData(connectionstr);
                if (!string.IsNullOrEmpty(str2))
                {
                    this.WriteReturnCode(400, str2);
                }
                str2 = this.SaveSiteSettings();
                if (!string.IsNullOrEmpty(str2))
                {
                    this.WriteReturnCode(500, str2);
                }
                str2 = this.SaveConfig(connectionstr);
                if (!string.IsNullOrEmpty(str2))
                {
                    this.WriteReturnCode(600, str2);
                }
                else
                {
                    this.WriteReturnCode(200, "安装成功");
                }
            }
        }

        private void WriteReturnCode(int status, string msg)
        {
            XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", "no"), new object[] { new XElement("rsp", new object[] { new XElement("code", status.ToString()), new XElement("msg", msg) }) });
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(document.ToString());
            HttpContext.Current.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

