namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.Core;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SetReset : Page
    {
        private string allowWeb1 = "http://demo.xiaokeduo.com/";
        private string allowWeb2 = "http://demo.xkd.kuaidiantong.cn/";
        private string allowWeb3 = "http://qdshop.yun.kuaidiantong.cn/";
        protected Button btnReset1;
        protected Button btnReset2;
        protected Button btnReset3;
        protected Button btnReset4;
        protected Button Button1;
        private Database database;
        protected HtmlForm form1;
        protected Literal litMsg;

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                CopyFolder(base.Server.MapPath("/Storage/temp/defaultdata/"), base.Server.MapPath("/"));
                this.litMsg.Text = this.litMsg.Text + "<p>重置网站的首页模版文件以及配置文件还原成功！</p>";
            }
            catch (Exception exception)
            {
                this.litMsg.Text = exception.ToString();
            }
        }

        protected void btnReset2_Click(object sender, EventArgs e)
        {
            string query = "delete from VShop_NavMenu;SET IDENTITY_INSERT [dbo].[VShop_NavMenu] ON\r\nINSERT [dbo].[VShop_NavMenu] ( MenuId,[ParentMenuId], [Name], [Type], [DisplaySequence], [Content], [ShopMenuPic]) VALUES ( 1,0, N'店铺主页', N'click', 0, N'/Default.aspx', N'/Storage/master/ShopMenu/e6b2c1471abe42b5ae4046d8072df383.png')\r\nINSERT [dbo].[VShop_NavMenu] ( MenuId,[ParentMenuId], [Name], [Type], [DisplaySequence], [Content], [ShopMenuPic]) VALUES ( 2,0, N'购物车', N'click', 0, N'/Vshop/ShoppingCart.aspx', N'/Storage/master/ShopMenu/b6b6f69513414e03be3f601e19999616.png')\r\nINSERT [dbo].[VShop_NavMenu] ( MenuId,[ParentMenuId], [Name], [Type], [DisplaySequence], [Content], [ShopMenuPic]) VALUES ( 3,0, N'会员中心', N'click', 0, N'/Vshop/MemberCenter.aspx', N'/Storage/master/ShopMenu/29f95ea3bdf448d6a3b831ed7f07dfcb.png')\r\nINSERT [dbo].[VShop_NavMenu] ( MenuId,[ParentMenuId], [Name], [Type], [DisplaySequence], [Content], [ShopMenuPic]) VALUES ( 4,0, N'申请分销', N'click', 0, N'/Vshop/DistributorRegCheck.aspx', N'/Storage/template/20150826/6357619845391103183922574.png')\r\n SET IDENTITY_INSERT [dbo].[VShop_NavMenu] OFF";
            try
            {
                Database database = DatabaseFactory.CreateDatabase();
                DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
                sqlStringCommand = database.GetSqlStringCommand(query);
                database.ExecuteNonQuery(sqlStringCommand);
                this.litMsg.Text = this.litMsg.Text + "<p>重置页面底部导航成功！</p>";
            }
            catch (Exception exception)
            {
                this.litMsg.Text = exception.ToString();
            }
        }

        protected void btnReset3_Click(object sender, EventArgs e)
        {
            string query = "\r\ndelete from [Hishop_MessageTemplates]\r\nINSERT [dbo].[Hishop_MessageTemplates] ([IsValid], [OrderIndex], [WXOpenTM], [MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [SendWeixin], [WeixinTemplateId], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (1, 21, N'TM00370', N'AccountChangeMsg', N'账户变更提醒', 0, 0, 0, 1, N'AS5UIq3zlOgzzvn8XdbcbV7bYthHAubugGee25J2jak', N'', N'', N'', N'', N'', N'')\r\nINSERT [dbo].[Hishop_MessageTemplates] ([IsValid], [OrderIndex], [WXOpenTM], [MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [SendWeixin], [WeixinTemplateId], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (0, 6, N'TM00853', N'CouponWillExpiredMsg', N'优惠券即将到期', 0, 0, 0, 1, N'', N'', N'', N'', N'', N'', N'')\r\nINSERT [dbo].[Hishop_MessageTemplates] ([IsValid], [OrderIndex], [WXOpenTM], [MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [SendWeixin], [WeixinTemplateId], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (1, 3, N'OPENTM207126233', N'DistributorCreateMsg', N'分销商申请成功提醒', 0, 0, 0, 1, N'JY4-ZZBd5nljm0cKZ9yvOcfWLGH9-vSPNiqzegE31po', N'', N'', N'', N'', N'', N'')\r\nINSERT [dbo].[Hishop_MessageTemplates] ([IsValid], [OrderIndex], [WXOpenTM], [MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [SendWeixin], [WeixinTemplateId], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (1, 5, N'OPENTM207601150', N'DrawCashResultMsg', N'提现结果提醒', 0, 0, 0, 1, N'33Nt6lOW3ysND3EdcgMmKVWRtTxRROiPF0k4iB1CPAA', N'', N'', N'', N'', N'', N'')\r\nINSERT [dbo].[Hishop_MessageTemplates] ([IsValid], [OrderIndex], [WXOpenTM], [MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [SendWeixin], [WeixinTemplateId], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (1, 1, N'OPENTM205109409', N'OrderMsg', N'订单消息提醒', 0, 0, 0, 1, N'XtGPwEA45Kyvo6QL821rkYC3LfKYz4wQcUso5vYeaGU', N'', N'', N'', N'', N'', N'')\r\nINSERT [dbo].[Hishop_MessageTemplates] ([IsValid], [OrderIndex], [WXOpenTM], [MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [SendWeixin], [WeixinTemplateId], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (1, 22, N'OPENTM207372725', N'PersonalMsg', N'个人消息通知', 0, 0, 0, 1, N'DH4cK9SaRw0KCdQdWjKeXT-OaBFKWjSTYE24vAbuJuA', N'', N'', N'', N'', N'', N'')\r\nINSERT [dbo].[Hishop_MessageTemplates] ([IsValid], [OrderIndex], [WXOpenTM], [MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [SendWeixin], [WeixinTemplateId], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (1, 2, N'TM00599', N'RefundSuccessMsg', N'退款通知', 0, 0, 0, 1, N'bC6sAOcH_ank_0q-8TIqvXeOXwdn06ibGhm8WQz8g4o', N'', N'', N'', N'', N'', N'')\r\nINSERT [dbo].[Hishop_MessageTemplates] ([IsValid], [OrderIndex], [WXOpenTM], [MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [SendWeixin], [WeixinTemplateId], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (0, 4, N'OPENTM207572068', N'ServiceMsg', N'服务消息通知', 0, 0, 0, 1, N'5iDwah2e2KLtv365hdxdanJaQDko1kwaxpjFnC_aXt0', N'', N'', N'', N'', N'', N'')\r\n\r\n";
            if (base.Request.Url.ToString().StartsWith(this.allowWeb2))
            {
                try
                {
                    Database database = DatabaseFactory.CreateDatabase();
                    DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
                    sqlStringCommand = database.GetSqlStringCommand(query);
                    database.ExecuteNonQuery(sqlStringCommand);
                    this.litMsg.Text = this.litMsg.Text + "<p>重置微信配置信息成功！</p>";
                }
                catch (Exception exception)
                {
                    this.litMsg.Text = exception.ToString();
                }
            }
            else
            {
                this.litMsg.Text = this.litMsg.Text + "<p>不允许微信配置信息修改！</p>";
            }
        }

        protected void btnReset4_Click(object sender, EventArgs e)
        {
            string str = base.Request.Url.ToString();
            string str2 = "http://demo.xiaokeduo.com/";
            string str3 = string.Empty;
            if (str.StartsWith(str2))
            {
                str3 = "demo.xiaokeduo.com";
            }
            if (str.Contains("demo.xkd.kuaidiantong.cn/"))
            {
                str3 = "demo.xkd.kuaidiantong.cn";
            }
            if (str.Contains("qdshop.yun.kuaidiantong.cn/"))
            {
                str3 = "qdshop.yun.kuaidiantong.cn";
            }
            if (!string.IsNullOrEmpty(str3))
            {
                string path = base.Request.MapPath("~/App_Data/" + str3 + "_product.sql");
                string errorMsg = string.Empty;
                if (!File.Exists(path))
                {
                    errorMsg = "没有找到数据库架构文件-Schema.sql";
                }
                this.ExecuteScriptFile(path, out errorMsg);
                if (string.IsNullOrEmpty(errorMsg))
                {
                    this.litMsg.Text = "更新产品数据成功";
                }
                else
                {
                    this.litMsg.Text = errorMsg;
                }
            }
            else
            {
                this.litMsg.Text = "当前域名不允许更新产品数据";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                Database database = DatabaseFactory.CreateDatabase();
                string query = "delete from aspnet_RolePermissions;delete from aspnet_Roles;INSERT INTO aspnet_Roles(RoleName,IsDefault) VALUES('超级管理员',1); SELECT @@IDENTITY";
                DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
                int num = Globals.ToNum(database.ExecuteScalar(sqlStringCommand));
                if (num > 0)
                {
                    query = string.Concat(new object[] { "delete from aspnet_Managers;INSERT INTO aspnet_Managers(RoleId, UserName, Password, Email, CreateDate) VALUES (", num, ",'admin','", HiCryptographer.Md5Encrypt("admin888"), "','admin@hishop.com',getdate())" });
                    sqlStringCommand = database.GetSqlStringCommand(query);
                    database.ExecuteNonQuery(sqlStringCommand);
                }
                this.litMsg.Text = this.litMsg.Text + "<p>重置管理员密码admin888成功！</p>";
            }
            catch (Exception exception)
            {
                this.litMsg.Text = exception.ToString();
            }
        }

        public static void CopyFolder(string strFromPath, string strToPath)
        {
            if (!Directory.Exists(strFromPath))
            {
                Directory.CreateDirectory(strFromPath);
            }
            string str = strFromPath.Substring(strFromPath.LastIndexOf(@"\") + 1, (strFromPath.Length - strFromPath.LastIndexOf(@"\")) - 1);
            if (!Directory.Exists(strToPath + @"\" + str))
            {
                Directory.CreateDirectory(strToPath + @"\" + str);
            }
            string[] files = Directory.GetFiles(strFromPath);
            for (int i = 0; i < files.Length; i++)
            {
                string str2 = files[i].Substring(files[i].LastIndexOf(@"\") + 1, (files[i].Length - files[i].LastIndexOf(@"\")) - 1);
                File.Copy(files[i], strToPath + @"\" + str + @"\" + str2, true);
            }
            DirectoryInfo[] directories = new DirectoryInfo(strFromPath).GetDirectories();
            for (int j = 0; j < directories.Length; j++)
            {
                CopyFolder(strFromPath + @"\" + directories[j].ToString(), strToPath + @"\" + str);
            }
        }

        private void ExecuteScriptFile(string pathToScriptFile, out string errorMsg)
        {
            StreamReader reader = null;
            string str = "";
            try
            {
                string applicationPath = Globals.ApplicationPath;
                using (reader = new StreamReader(pathToScriptFile))
                {
                    while (!reader.EndOfStream)
                    {
                        str = NextSqlFromStream(reader);
                        if (!string.IsNullOrEmpty(str))
                        {
                            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
                            this.database.ExecuteNonQuery(sqlStringCommand);
                        }
                    }
                    errorMsg = "";
                }
            }
            catch (SqlException exception)
            {
                errorMsg = exception.Message;
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
            this.database = DatabaseFactory.CreateDatabase();
            if (Globals.RequestQueryNum("istest") != 0x75ed285)
            {
                base.Response.Write("");
                base.Response.End();
            }
            string str = base.Request.Url.ToString();
            if ((!str.StartsWith(this.allowWeb1) && !str.StartsWith(this.allowWeb2)) && !str.StartsWith(this.allowWeb3))
            {
                base.Response.Write("");
                base.Response.End();
            }
            this.litMsg.Text = "";
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
    }
}

