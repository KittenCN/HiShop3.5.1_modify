namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web.UI;

    public class test : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.Response.Write("");
        }

        public string Read(string path)
        {
            string str;
            StreamReader reader = new StreamReader(path, Encoding.Default);
            StringBuilder builder = new StringBuilder();
            while ((str = reader.ReadLine()) != null)
            {
                builder.Append(str);
            }
            reader.Close();
            reader.Dispose();
            return builder.ToString();
        }
    }
}

