namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.UI.ControlPanel.Utility;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Web.UI.HtmlControls;

    public class StoreCardSet : AdminPage
    {
        protected HtmlImage idImg;

        protected StoreCardSet() : base("m05", "fxp13")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.Form["action"];
            if (str == "Edit")
            {
                base.Response.ContentType = "application/json";
                string s = "{\"success\":\"false\",\"Desciption\":\"保存设置失败!\"}";
                string path = base.Server.MapPath("~/Storage/Utility/StoreCardSet.js");
                string str4 = base.Request.Form["SotreCardJson"];
                if (!string.IsNullOrEmpty(str4))
                {
                    JObject obj2 = JsonConvert.DeserializeObject(str4) as JObject;
                    if (((obj2["posList"] != null) && (obj2["DefaultHead"] != null)) && ((obj2["myusername"] != null) && (obj2["shopname"] != null)))
                    {
                        try
                        {
                            obj2["writeDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            File.WriteAllText(path, JsonConvert.SerializeObject(obj2));
                            s = "{\"success\":\"true\",\"Desciption\":\"保存设置成功!\"}";
                        }
                        catch (Exception exception)
                        {
                            s = "{\"success\":\"false\",\"Desciption\":\"保存设置失败!" + exception.Message + "\"}";
                        }
                    }
                }
                base.Response.Write(s);
                base.Response.End();
            }
        }
    }
}

