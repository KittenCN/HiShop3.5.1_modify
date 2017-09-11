namespace Hidistro.UI.Web.Admin.Settings.flex
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using System;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    public class XmlData : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.Form["xmlname"];
            string s = base.Request.Form["xmldata"];
            string str3 = base.Request.Form["expressname"];
            if ((!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str3)) && (!SalesHelper.IsExistExpress(str3) && SalesHelper.AddExpressTemplate(str3, str + ".xml")))
            {
                FileStream stream = new FileStream(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}.xml", str)), FileMode.Create);
                byte[] bytes = new UTF8Encoding().GetBytes(s);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();
            }
        }
    }
}

