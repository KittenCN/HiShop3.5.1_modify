namespace Hidistro.UI.Web.Admin.Settings.flex
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using System;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    public class EditXmlData : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.Form["xmlname"];
            string s = base.Request.Form["xmldata"];
            string str3 = base.Request.Form["expressname"];
            string str4 = base.Request.Form["expressid"];
            if ((!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str3)) && !string.IsNullOrEmpty(str4))
            {
                int result = 0;
                if (int.TryParse(str4, out result) && SalesHelper.UpdateExpressTemplate(result, str3))
                {
                    FileStream stream = new FileStream(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", str)), FileMode.Create);
                    byte[] bytes = new UTF8Encoding().GetBytes(s);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                    stream.Close();
                }
            }
        }
    }
}

