namespace Hidistro.UI.Web.Admin.Shop
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_SaveTemplate : IHttpHandler
    {
        public string Base64Code(string message)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        }

        public string Base64Decode(string message)
        {
            byte[] bytes = Convert.FromBase64String(message);
            return Encoding.UTF8.GetString(bytes);
        }

        public string GetGoodGroupTag(HttpContext context, string path, JToken data)
        {
            try
            {
                return string.Concat(new object[] { 
                    "<Hi:GoodsListModule runat=\"server\"  Type=\"goodGroup\" Layout=\"", data["content"]["layout"], "\" ShowName=\"", data["content"]["showName"], "\" ShowIco=\"", data["content"]["showIco"], "\" ShowPrice=\"", data["content"]["showPrice"], "\" DataUrl=\"", context.Request.Form["getGoodGroupUrl"], "\" ID=\"group_", Guid.NewGuid().ToString("N"), "\" TemplateFile=\"", path, "\"  GoodListSize=\"", data["content"]["goodsize"], 
                    "\" FirstPriority=\"", data["content"]["firstPriority"], "\"  SecondPriority=\"", data["content"]["secondPriority"], "\"  ShowMaketPrice=\"", data["content"]["showMaketPrice"], "\"  />"
                 });
            }
            catch
            {
                return "";
            }
        }

        public string GetGoodTag(HttpContext context, JToken data)
        {
            try
            {
                string str = "";
                foreach (JToken token in (IEnumerable<JToken>) data["content"]["goodslist"])
                {
                    str = str + token["item_id"] + ",";
                }
                str = str.TrimEnd(new char[] { ',' });
                string str2 = "";
                if (!string.IsNullOrEmpty(str))
                {
                    string str3 = "/admin/shop/Modules/GoodGroup" + data["content"]["layout"] + ".cshtml";
                    str2 = string.Concat(new object[] { 
                        "<Hi:GoodsMobule runat=\"server\" Layout=\"", data["content"]["layout"], "\" ShowName=\"", data["content"]["showName"], "\" IDs=\"", str, "\" ShowIco=\"", data["content"]["showIco"], "\" ShowPrice=\"", data["content"]["showPrice"], "\" DataUrl=\"", context.Request.Form["getGoodUrl"], "\" ID=\"goods_", Guid.NewGuid().ToString("N"), "\" TemplateFile=\"", str3, 
                        "\"    />"
                     });
                }
                else
                {
                    str2 = str2 + this.Base64Decode(data["dom_conitem"].ToString());
                }
                return str2;
            }
            catch
            {
                return "";
            }
        }

        public string GetLModulesHtml(HttpContext context, JObject jo)
        {
            string str = "";
            foreach (JToken token in (IEnumerable<JToken>) jo["LModules"])
            {
                if (token["type"].ToString() == "5")
                {
                    str = str + this.GetGoodGroupTag(context, this.Base64Decode(token["dom_conitem"].ToString()), token);
                }
                else if (token["type"].ToString() == "4")
                {
                    str = str + this.GetGoodTag(context, token);
                }
                else
                {
                    str = str + this.Base64Decode(token["dom_conitem"].ToString());
                }
            }
            return str;
        }

        public string GetPModulesHtml(HttpContext context, JObject jo)
        {
            string str = "";
            foreach (JToken token in (IEnumerable<JToken>) jo["PModules"])
            {
                str = str + this.Base64Decode(token["dom_conitem"].ToString());
            }
            return str;
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string str = context.Request.Form["content"];
            string str2 = context.Request.Form["id"];
            JObject jo = (JObject) JsonConvert.DeserializeObject(str);
            string message = "保存成功";
            string str4 = "1";
            try
            {
                StreamWriter writer = new StreamWriter(context.Server.MapPath("/Templates/vshop/" + str2.Trim() + "/data/default.json"), false, Encoding.UTF8);
                foreach (char ch in str)
                {
                    writer.Write(ch);
                }
                writer.Close();
                string str5 = "<%@ Control Language=\"C#\" %>\n<%@ Register TagPrefix=\"Hi\" Namespace=\"HiTemplate\" Assembly=\"HiTemplate\" %>";
                str5 = str5 + this.GetPModulesHtml(context, jo);
                string lModulesHtml = this.GetLModulesHtml(context, jo);
                str5 = str5 + lModulesHtml;
                string path = "/Templates/vshop/" + str2;
                if (!Directory.Exists(context.Server.MapPath(path)))
                {
                    Directory.CreateDirectory(context.Server.MapPath(path));
                }
                StreamWriter writer2 = new StreamWriter(context.Server.MapPath(path + "/Skin-HomePage.html"), false, Encoding.UTF8);
                foreach (char ch2 in str5)
                {
                    writer2.Write(ch2);
                }
                writer2.Close();
            }
            catch (Exception exception)
            {
                message = exception.Message;
                str4 = "0";
            }
            if (context.Request.Form["is_preview"] == "1")
            {
                context.Response.Write("{\"status\":" + str4 + ",\"msg\":\"" + message + "\",\"link\":\"default.aspx\"}");
            }
            else
            {
                context.Response.Write("{\"status\":" + str4 + ",\"msg\":\"" + message + "\"}");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

