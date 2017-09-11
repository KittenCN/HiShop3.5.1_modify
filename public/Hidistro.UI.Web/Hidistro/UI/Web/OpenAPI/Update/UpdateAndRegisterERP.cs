namespace Hidistro.UI.Web.OpenAPI.Update
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UpdateAndRegisterERP : Page
    {
        protected Button btnUpdateAndRegisterERP;
        protected HtmlForm form1;

        protected void btnUpdateAndRegisterERP_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string appKey = masterSettings.AppKey;
            string checkCode = masterSettings.CheckCode;
            if (!string.IsNullOrEmpty(appKey) && !string.IsNullOrEmpty(checkCode))
            {
                str = "版本已更新，无需重复更新";
            }
            else
            {
                appKey = this.CreateAppKey();
                checkCode = CreateKey(20);
                try
                {
                    this.RegisterERP(appKey, checkCode);
                    masterSettings.AppKey = appKey;
                    masterSettings.CheckCode = checkCode;
                    SettingsManager.Save(masterSettings);
                    str = "更新版本及注册成功";
                }
                catch (Exception exception)
                {
                    str = "注册失败，错误信息：" + exception.Message;
                }
            }
            base.Response.Write("<script language='javascript'>alert('" + str + "');</script>");
        }

        private string CreateAppKey()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str = str + ((char) (0x30 + ((ushort) (num % 10)))).ToString();
            }
            return (DateTime.Now.ToString("yyyyMMdd") + str);
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

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void RegisterERP(string appkey, string appsecret)
        {
            string url = "http://hierp.kuaidiantong.cn/api/commercialtenantregister";
            Globals.Debuglog(Globals.GetPostResult(url, "appKey=" + appkey + "&appSecret=" + appsecret + "&routeAddress=" + Globals.GetWebUrlStart() + "/OpenAPI/"), "_DebuglogERP.txt");
        }
    }
}

