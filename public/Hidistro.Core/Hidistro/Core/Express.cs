namespace Hidistro.Core
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using System.Xml;

    public static class Express
    {
        public static string GetDataByKuaidi100(string computer, string expressNo, int show = 0)
        {
            string str = "29833628d495d7a5";
            string str2 = "";
            string str3 = null;
            string str4 = "暂时没有此快递单号的信息";
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                str3 = current.Request.MapPath("~/config/Express.xml");
            }
            XmlDocument document = new XmlDocument();
            if (!string.IsNullOrEmpty(str3))
            {
                document.Load(str3);
                XmlNode node = document.SelectSingleNode("companys");
                if (node != null)
                {
                    string str5 = node.Attributes["Kuaidi100NewKey"].Value;
                    if (!string.IsNullOrEmpty(str5))
                    {
                        str2 = str5;
                    }
                }
            }
            if (string.IsNullOrEmpty(str2))
            {
                HttpWebResponse response;
                string expressCode = GetExpressCode(computer);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://api.kuaidi100.com/api?id=" + str + "&com=" + expressCode + "&nu=" + expressNo + "&show=0&muti=1&order=asc");
                request.Timeout = 0x1f40;
                try
                {
                    response = (HttpWebResponse) request.GetResponse();
                }
                catch
                {
                    return str4;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                    str4 = reader.ReadToEnd().Replace("&amp;", "").Replace("&nbsp;", "").Replace("&", "");
                }
            }
            return str4;
        }

        public static string GetDataByTaobaoTop(string computer, string expressNo)
        {
            return "暂时没有此快递单号的信息";
        }

        public static string GetExpressCode(string computer)
        {
            string str = computer;
            switch (str)
            {
                case "AAE全球专递":
                    return "aae";

                case "安捷快递":
                    return "anjiekuaidi";

                case "安信达快递":
                    return "anxindakuaixi";

                case "百福东方":
                    return "baifudongfang";

                case "彪记快递":
                    return "biaojikuaidi";

                case "BHT":
                    return "bht";

                case "希伊艾斯快递":
                    return "cces";

                case "中国东方":
                    return "Coe";

                case "长宇物流":
                    return "changyuwuliu";

                case "大田物流":
                    return "datianwuliu";

                case "德邦物流":
                    return "debangwuliu";

                case "DPEX":
                    return "dpex";

                case "DHL":
                    return "dhl";

                case "D速快递":
                    return "dsukuaidi";

                case "fedex":
                    return "fedex";

                case "飞康达物流":
                    return "feikangda";

                case "凤凰快递":
                    return "fenghuangkuaidi";

                case "港中能达物流":
                    return "ganzhongnengda";

                case "广东邮政物流":
                    return "guangdongyouzhengwuliu";

                case "汇通快运":
                    return "huitongkuaidi";

                case "恒路物流":
                    return "hengluwuliu";

                case "华夏龙物流":
                    return "huaxialongwuliu";

                case "佳怡物流":
                    return "jiayiwuliu";

                case "京广速递":
                    return "jinguangsudikuaijian";

                case "急先达":
                    return "jixianda";

                case "佳吉物流":
                    return "jiajiwuliu";

                case "加运美":
                    return "jiayunmeiwuliu";

                case "快捷速递":
                    return "kuaijiesudi";

                case "联昊通物流":
                    return "lianhaowuliu";

                case "龙邦物流":
                    return "longbanwuliu";

                case "民航快递":
                    return "minghangkuaidi";

                case "配思货运":
                    return "peisihuoyunkuaidi";

                case "全晨快递":
                    return "quanchenkuaidi";

                case "全际通物流":
                    return "quanjitong";

                case "全日通快递":
                    return "quanritongkuaidi";

                case "全一快递":
                    return "quanyikuaidi";

                case "盛辉物流":
                    return "shenghuiwuliu";

                case "速尔物流":
                    return "suer";

                case "盛丰物流":
                    return "shengfengwuliu";

                case "天地华宇":
                    return "tiandihuayu";

                case "天天快递":
                    return "tiantian";

                case "TNT":
                    return "tnt";

                case "UPS":
                    return "ups";

                case "万家物流":
                    return "wanjiawuliu";

                case "文捷航空速递":
                    return "wenjiesudi";

                case "伍圆速递":
                    return "wuyuansudi";

                case "万象物流":
                    return "wanxiangwuliu";

                case "新邦物流":
                    return "xinbangwuliu";

                case "信丰物流":
                    return "xinfengwuliu";

                case "星晨急便":
                    return "xingchengjibian";

                case "鑫飞鸿物流":
                    return "xinhongyukuaidi";

                case "亚风速递":
                    return "yafengsudi";

                case "一邦速递":
                    return "yibangwuliu";

                case "优速物流":
                    return "youshuwuliu";

                case "远成物流":
                    return "yuanchengwuliu";

                case "圆通速递":
                    return "yuantong";

                case "源伟丰快递":
                    return "yuanweifeng";

                case "元智捷诚快递":
                    return "yuanzhijiecheng";

                case "越丰物流":
                    return "yuefengwuliu";

                case "韵达快递":
                    return "yunda";

                case "源安达":
                    return "yuananda";

                case "运通快递":
                    return "yuntongkuaidi";

                case "宅急送":
                    return "zhaijisong";

                case "中铁快运":
                    return "zhongtiewuliu";

                case "中通速递":
                    return "zhongtong";

                case "中邮物流":
                    return "zhongyouwuliu";

                case "顺丰物流":
                    return "shunfeng";
            }
            return str;
        }

        public static string GetExpressData(string computer, string expressNo, int show = 0)
        {
            if (GetExpressType() == "kuaidi100")
            {
                return GetDataByKuaidi100(computer, expressNo, show);
            }
            return GetDataByTaobaoTop(computer, expressNo);
        }

        public static string GetExpressType()
        {
            string innerText = "kuaidi100";
            string str2 = null;
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                str2 = current.Request.MapPath("~/config/Express.xml");
            }
            XmlDocument document = new XmlDocument();
            if (!string.IsNullOrEmpty(str2))
            {
                document.Load(str2);
                XmlNode node = document.SelectSingleNode("expressapi");
                if (node != null)
                {
                    innerText = node.Attributes["usetype"].InnerText;
                }
            }
            return innerText;
        }

        public static string GetNewKey()
        {
            string str = "";
            string str2 = null;
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                str2 = current.Request.MapPath("~/config/Express.xml");
            }
            XmlDocument document = new XmlDocument();
            if (!string.IsNullOrEmpty(str2))
            {
                document.Load(str2);
                XmlNode node = document.SelectSingleNode("companys");
                if (node != null)
                {
                    string str3 = node.Attributes["Kuaidi100NewKey"].Value;
                    if (!string.IsNullOrEmpty(str3))
                    {
                        str = str3;
                    }
                }
            }
            return str;
        }

        public static void SubscribeExpress100(string companyCode, string number)
        {
            string newKey = GetNewKey();
            if (!string.IsNullOrEmpty(newKey) && ((companyCode != "") && (number != "")))
            {
                WebClient client = new WebClient();
                NameValueCollection data = new NameValueCollection();
                StringBuilder builder = new StringBuilder();
                builder.Append("{");
                builder.AppendFormat("\"company\":\"{0}\",", companyCode);
                builder.AppendFormat("\"number\":\"{0}\",", number);
                builder.AppendFormat("\"key\":\"{0}\",", newKey);
                string str2 = "http://" + HttpContext.Current.Request.Url.Host + "/API/ExpressReturn.ashx?action=SaveExpressData";
                builder.AppendFormat("\"parameters\":{0}\"callbackurl\":\"{1}\"{2}", "{", str2, "}");
                builder.Append("}");
                data.Add("schema", "json");
                data.Add("param", builder.ToString());
                try
                {
                    byte[] bytes = client.UploadValues("http://www.kuaidi100.com/poll", "POST", data);
                    string log = Encoding.UTF8.GetString(bytes);
                    Globals.Debuglog("returnUrl:" + str2, "_Debuglog.txt");
                    Globals.Debuglog(log, "_Debuglog.txt");
                }
                catch (Exception)
                {
                }
            }
        }
    }
}

