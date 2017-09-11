namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;

    public class testWX : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = HiCryptographer.Md5Encrypt("888999");
        }
    }
}

