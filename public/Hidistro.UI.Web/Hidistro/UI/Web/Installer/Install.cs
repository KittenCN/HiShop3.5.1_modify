namespace Hidistro.UI.Web.Installer
{
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Configuration;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Install : Page
    {
        private string action;
        protected Button btnInstall;
        protected CheckBox chkIsAddDemo;
        private string dbName;
        private string dbPassword;
        private string dbServer;
        private string dbUsername;
        private string email;
        private IList<string> errorMsgs;
        protected HtmlForm form1;
        protected HiddenField hdfSiteName;
        private bool isAddDemo;
        protected Label lblErrMessage;
        protected Label litSetpErrorMessage;
        private string password;
        private string password2;
        private bool testSuccessed;
        protected TextBox txtDbName;
        protected TextBox txtDbPassword;
        protected TextBox txtDbServer;
        protected TextBox txtDbUsername;
        protected TextBox txtEmail;
        protected TextBox txtPassword;
        protected TextBox txtPassword2;
        protected TextBox txtUsername;
        private string username;

        private bool AddDemoData(out string errorMsg)
        {
            string path = base.Request.MapPath("SqlScripts/SiteDemo.zh-CN.sql");
            if (!File.Exists(path))
            {
                errorMsg = "没有找到演示数据文件-SiteDemo.Sql";
                return false;
            }
            return this.ExecuteScriptFile(path, out errorMsg);
        }

        private bool AddInitData(out string errorMsg)
        {
            string path = base.Request.MapPath("SqlScripts/SiteInitData.zh-CN.Sql");
            if (!File.Exists(path))
            {
                errorMsg = "没有找到初始化数据文件-SiteInitData.Sql";
                return false;
            }
            return this.ExecuteScriptFile(path, out errorMsg);
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!this.ValidateUser(out msg))
            {
                this.ShowMsg(msg, false);
            }
            else if (!this.testSuccessed && !this.ExecuteTest())
            {
                this.ShowMsg("数据库链接信息有误", false);
            }
            else if (!this.CreateDataSchema(out msg))
            {
                this.ShowMsg(msg, false);
            }
            else if (!this.CreateAdministrator(out msg))
            {
                this.ShowMsg(msg, false);
            }
            else if (!this.AddInitData(out msg))
            {
                this.ShowMsg(msg, false);
            }
            else if (!this.isAddDemo || this.AddDemoData(out msg))
            {
                if (!this.SaveSiteSettings(out msg))
                {
                    this.ShowMsg(msg, false);
                }
                else if (!this.SaveConfig(out msg))
                {
                    this.ShowMsg(msg, false);
                }
                else
                {
                    this.Context.Response.Redirect("Succeed.aspx", true);
                }
            }
        }

        private bool CreateAdministrator(out string errorMsg)
        {
            DbConnection connection = new SqlConnection(this.GetConnectionString());
            connection.Open();
            DbCommand command = connection.CreateCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO aspnet_Roles(RoleName,IsDefault) VALUES('超级管理员',1); SELECT @@IDENTITY";
            int num = Convert.ToInt32(command.ExecuteScalar());
            command.CommandText = "INSERT INTO aspnet_Managers(RoleId, UserName, Password, Email, CreateDate) VALUES (@RoleId, @UserName, @Password, @Email, getdate())";
            command.Parameters.Add(new SqlParameter("@RoleId", num));
            command.Parameters.Add(new SqlParameter("@Username", this.username));
            command.Parameters.Add(new SqlParameter("@Password", HiCryptographer.Md5Encrypt(this.password)));
            command.Parameters.Add(new SqlParameter("@Email", this.email));
            command.ExecuteNonQuery();
            connection.Close();
            errorMsg = null;
            return true;
        }

        private bool CreateDataSchema(out string errorMsg)
        {
            string path = base.Request.MapPath("SqlScripts/Schema.sql");
            if (!File.Exists(path))
            {
                errorMsg = "没有找到数据库架构文件-Schema.sql";
                return false;
            }
            return this.ExecuteScriptFile(path, out errorMsg);
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

        private bool ExecuteScriptFile(string pathToScriptFile, out string errorMsg)
        {
            StreamReader reader = null;
            SqlConnection connection = null;
            try
            {
                string applicationPath = Globals.ApplicationPath;
                using (reader = new StreamReader(pathToScriptFile))
                {
                    using (connection = new SqlConnection(this.GetConnectionString()))
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
                errorMsg = null;
                return true;
            }
            catch (SqlException exception)
            {
                errorMsg = exception.Message;
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
                return false;
            }
        }

        private bool ExecuteTest()
        {
            string str;
            this.errorMsgs = new List<string>();
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                if (this.ValidateConnectionStrings(out str))
                {
                    using (connection = new SqlConnection(this.GetConnectionString()))
                    {
                        connection.Open();
                        DbCommand command = connection.CreateCommand();
                        transaction = connection.BeginTransaction();
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandText = "CREATE TABLE installTest(Test bit NULL)";
                        command.ExecuteNonQuery();
                        command.CommandText = "DROP TABLE installTest";
                        command.ExecuteNonQuery();
                        transaction.Commit();
                        connection.Close();
                        goto Label_00E4;
                    }
                }
                this.errorMsgs.Add(str);
            }
            catch (Exception exception)
            {
                this.errorMsgs.Add(exception.Message);
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exception2)
                    {
                        this.errorMsgs.Add(exception2.Message);
                    }
                }
                if ((connection != null) && (connection.State != ConnectionState.Closed))
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        Label_00E4:
            if (!TestFolder(base.Request.MapPath(Globals.ApplicationPath + "/config/test.txt"), out str))
            {
                this.errorMsgs.Add(str);
            }
            try
            {
                System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
                if (configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString == "none")
                {
                    configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = "required";
                }
                else
                {
                    configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = "none";
                }
                configuration.Save();
            }
            catch (Exception exception3)
            {
                this.errorMsgs.Add(exception3.Message);
            }
            if (!TestFolder(base.Request.MapPath(Globals.ApplicationPath + "/storage/test.txt"), out str))
            {
                this.errorMsgs.Add(str);
            }
            return (this.errorMsgs.Count == 0);
        }

        private string GetConnectionString()
        {
            return string.Format("server={0};uid={1};pwd={2};Trusted_Connection=no;database={3}", new object[] { this.dbServer, this.dbUsername, this.dbPassword, this.dbName });
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

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                this.action = base.Request["action"];
                this.dbServer = base.Request["DBServer"];
                this.dbName = base.Request["DBName"];
                this.dbUsername = base.Request["DBUsername"];
                this.dbPassword = base.Request["DBPassword"];
                this.username = base.Request["Username"];
                this.email = base.Request["Email"];
                this.password = base.Request["Password"];
                this.password2 = base.Request["Password2"];
                this.isAddDemo = !string.IsNullOrEmpty(base.Request["IsAddDemo"]) && (base.Request["IsAddDemo"] == "true");
                this.testSuccessed = !string.IsNullOrEmpty(base.Request["TestSuccessed"]) && (base.Request["TestSuccessed"] == "true");
            }
            else
            {
                this.dbServer = this.txtDbServer.Text;
                this.dbName = this.txtDbName.Text;
                this.dbUsername = this.txtDbUsername.Text;
                this.dbPassword = this.txtDbPassword.Text;
                this.username = this.txtUsername.Text;
                this.email = this.txtEmail.Text;
                this.password = this.txtPassword.Text;
                this.password2 = this.txtPassword2.Text;
                this.isAddDemo = this.chkIsAddDemo.Checked;
            }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            this.btnInstall.Click += new EventHandler(this.btnInstall_Click);
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                string str = "无效的操作类型：" + this.action;
                bool flag2 = false;
                if (this.action == "Test")
                {
                    flag2 = this.ExecuteTest();
                }
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                if (flag2)
                {
                    base.Response.Write("{\"Status\":\"OK\"}");
                }
                else
                {
                    string str2 = "";
                    if ((this.errorMsgs != null) && (this.errorMsgs.Count > 0))
                    {
                        foreach (string str3 in this.errorMsgs)
                        {
                            str2 = str2 + "{\"Text\":\"" + str3 + "\"},";
                        }
                        str2 = str2.Substring(0, str2.Length - 1);
                        this.errorMsgs.Clear();
                    }
                    else
                    {
                        str2 = "{\"Text\":\"" + str + "\"}";
                    }
                    base.Response.Write(string.Format("{{\"Status\":\"Fail\",\"ErrorMsgs\":[{0}]}}", str2));
                }
                base.Response.End();
            }
            else if (!this.Page.IsPostBack && (base.Request.UrlReferrer != null))
            {
                base.Request.UrlReferrer.OriginalString.IndexOf("Activation.aspx");
            }
        }

        private void RegisterERP(string appkey, string appsecret)
        {
            string url = "http://hierp.kuaidiantong.cn/api/commercialtenantregister";
            Globals.Debuglog(Globals.GetPostResult(url, "appKey=" + appkey + "&appSecret=" + appsecret + "&routeAddress=" + Globals.GetWebUrlStart() + "/OpenAPI/"), "_DebuglogERP.txt");
        }

        private bool SaveConfig(out string errorMsg)
        {
            try
            {
                System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
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
                configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = this.GetConnectionString();
                configuration.ConnectionStrings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                configuration.Save();
                errorMsg = null;
                return true;
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
                return false;
            }
        }

        private bool SaveSiteSettings(out string errorMsg)
        {
            errorMsg = null;
            try
            {
                string filename = base.Request.MapPath(Globals.ApplicationPath + "/config/SiteSettings.config");
                XmlDocument doc = new XmlDocument();
                SiteSettings settings = new SiteSettings(base.Request.Url.Host);
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine + "<Settings></Settings>");
                settings.VTheme = "t1";
                settings.SiteName = this.hdfSiteName.Value.Trim();
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
                settings.PointsRate = 1M;
                settings.ShoppingScoreUnit = 1;
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
                return true;
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
                return false;
            }
        }

        private void ShowMsg(string errorMsg, bool seccess)
        {
            this.lblErrMessage.Text = errorMsg;
        }

        private static bool TestFolder(string folderPath, out string errorMsg)
        {
            try
            {
                File.WriteAllText(folderPath, "Hi");
                File.AppendAllText(folderPath, ",This is a test file.");
                File.Delete(folderPath);
                errorMsg = null;
                return true;
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
                return false;
            }
        }

        private bool ValidateConnectionStrings(out string msg)
        {
            msg = null;
            if ((!string.IsNullOrEmpty(this.dbServer) && !string.IsNullOrEmpty(this.dbName)) && !string.IsNullOrEmpty(this.dbUsername))
            {
                return true;
            }
            msg = "数据库连接信息不完整";
            return false;
        }

        private bool ValidateUser(out string msg)
        {
            msg = null;
            if ((string.IsNullOrEmpty(this.username) || string.IsNullOrEmpty(this.email)) || (string.IsNullOrEmpty(this.password) || string.IsNullOrEmpty(this.password2)))
            {
                msg = "管理员账号信息不完整";
                return false;
            }
            HiConfiguration config = HiConfiguration.GetConfig();
            if ((this.username.Length > config.UsernameMaxLength) || (this.username.Length < config.UsernameMinLength))
            {
                msg = string.Format("管理员用户名的长度只能在{0}和{1}个字符之间", config.UsernameMinLength, config.UsernameMaxLength);
                return false;
            }
            if (string.Compare(this.username, "anonymous", true) == 0)
            {
                msg = "不能使用anonymous作为管理员用户名";
                return false;
            }
            if (!Regex.IsMatch(this.username, config.UsernameRegex))
            {
                msg = "管理员用户名的格式不符合要求，用户名一般由字母、数字、下划线和汉字组成，且必须以汉字或字母开头";
                return false;
            }
            if (this.email.Length > 0x100)
            {
                msg = "电子邮件的长度必须小于256个字符";
                return false;
            }
            if (!Regex.IsMatch(this.email, config.EmailRegex))
            {
                msg = "电子邮件的格式错误";
                return false;
            }
            if (this.password != this.password2)
            {
                msg = "管理员登录密码两次输入不一致";
                return false;
            }
            if ((this.password.Length >= Membership.Provider.MinRequiredPasswordLength) && (this.password.Length <= config.PasswordMaxLength))
            {
                return true;
            }
            msg = string.Format("管理员登录密码的长度只能在{0}和{1}个字符之间", Membership.Provider.MinRequiredPasswordLength, config.PasswordMaxLength);
            return false;
        }
    }
}

