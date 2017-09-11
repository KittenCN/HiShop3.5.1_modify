namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Xml;

    public class RegionHandler : IHttpHandler
    {
        private void GetRegionInfo(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int result = 0;
            int.TryParse(context.Request["regionId"], out result);
            if (result <= 0)
            {
                context.Response.Write("{\"Status\":\"0\"}");
            }
            else
            {
                XmlNode region = RegionHelper.GetRegion(result);
                if (region == null)
                {
                    context.Response.Write("{\"Status\":\"0\"}");
                }
                else
                {
                    int num2 = 1;
                    if (region.Name.Equals("city"))
                    {
                        num2 = 2;
                    }
                    else if (region.Name.Equals("county"))
                    {
                        num2 = 3;
                    }
                    string str = (num2 > 1) ? RegionHelper.GetFullPath(result) : "";
                    string str2 = "";
                    if (!region.Name.Equals("province"))
                    {
                        str2 = region.ParentNode.Attributes["id"].Value;
                    }
                    string s = "{";
                    s = (((((s + "\"Status\":\"OK\",") + "\"RegionId\":\"" + result.ToString(CultureInfo.InvariantCulture) + "\",") + "\"Depth\":\"" + num2.ToString(CultureInfo.InvariantCulture) + "\",") + "\"Path\":\"" + str + "\",") + "\"ParentId\":\"" + str2 + "\"") + "}";
                    context.Response.Write(s);
                }
            }
        }

        private static void GetRegions(HttpContext context)
        {
            Dictionary<int, string> citys;
            context.Response.ContentType = "application/json";
            int result = 0;
            int.TryParse(context.Request["parentId"], out result);
            if (result > 0)
            {
                XmlNode region = RegionHelper.GetRegion(result);
                if (region == null)
                {
                    context.Response.Write("{\"Status\":\"0\"}");
                    return;
                }
                if (region.Name.Equals("province"))
                {
                    citys = RegionHelper.GetCitys(result);
                }
                else
                {
                    citys = RegionHelper.GetCountys(result);
                }
            }
            else
            {
                citys = RegionHelper.GetAllProvinces();
            }
            if (citys.Count == 0)
            {
                context.Response.Write("{\"Status\":\"0\"}");
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("{");
                builder.Append("\"Status\":\"OK\",");
                builder.Append("\"Regions\":[");
                foreach (int num2 in citys.Keys)
                {
                    builder.Append("{");
                    builder.AppendFormat("\"RegionId\":\"{0}\",", num2.ToString(CultureInfo.InvariantCulture));
                    builder.AppendFormat("\"RegionName\":\"{0}\"", citys[num2]);
                    builder.Append("},");
                }
                builder.Remove(builder.Length - 1, 1);
                builder.Append("]}");
                citys.Clear();
                context.Response.Write(builder.ToString());
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string str2 = context.Request["action"];
                if (str2 != null)
                {
                    if (!(str2 == "getregions"))
                    {
                        if (str2 == "getregioninfo")
                        {
                            goto Label_003A;
                        }
                    }
                    else
                    {
                        GetRegions(context);
                    }
                }
                return;
            Label_003A:
                this.GetRegionInfo(context);
            }
            catch
            {
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

