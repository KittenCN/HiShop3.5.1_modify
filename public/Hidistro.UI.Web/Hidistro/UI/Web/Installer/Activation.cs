namespace Hidistro.UI.Web.Installer
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Activation : Page
    {
        protected Button btnInstall;
        protected HtmlForm form1;
        protected Label lblErrMessage;
        protected HtmlInputText txtcode;

        protected void btnInstall_Click(object sender, EventArgs e)
        {
            string str = this.txtcode.Value;
            if (!string.IsNullOrEmpty(str) && this.CheckCode(str))
            {
                base.Response.Redirect("Install.aspx");
            }
            else
            {
                this.lblErrMessage.Text = "对不起，您的激活码错误！";
            }
        }

        internal bool CheckCode(string code)
        {
            if (!string.IsNullOrEmpty(code) && (code.Length == 6))
            {
                string path = HttpContext.Current.Server.MapPath("~/config/code.db3");
                try
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        string str2 = reader.ReadToEnd();
                        reader.Close();
                        return str2.Contains(code);
                    }
                }
                catch
                {
                }
            }
            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

