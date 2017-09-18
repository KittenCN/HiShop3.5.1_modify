using Hidistro.Core;
using System;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin
{
    public partial class testWX : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = HiCryptographer.Md5Encrypt("888999");

        }
    }
}