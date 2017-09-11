namespace Hidistro.Messages
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hishop.Weixin.MP.Api;
    using Hishop.Weixin.MP.Util;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class WxTemplateSendHelp
    {
        public static Hidistro.Messages.AddtemplateJsonResult AddtemplateJsonResult(string accessToken, string template_id_short)
        {
            string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={0}";
            var data = new {
                template_id_short = template_id_short
            };
            return SendCommonJson<Hidistro.Messages.AddtemplateJsonResult>(accessToken, urlFormat, data);
        }

        public static string AsUrlData(this string data)
        {
            return Uri.EscapeDataString(data);
        }

        public static WxJsonResult DelPrivateTemplate(string template_id)
        {
            string accessToken = GetAccessToken();
            if (string.IsNullOrEmpty(accessToken) || accessToken.Contains("errcode"))
            {
                return new WxJsonResult { errcode = 0x9c41, errmsg = "Token获取失败" };
            }
            string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/del_private_template?access_token={0}";
            var data = new {
                template_id = template_id
            };
            return SendCommonJson<WxJsonResult>(accessToken, urlFormat, data);
        }

        public static string GetAccessToken()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            return TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
        }

        public static string GetErrorMsg(int code, string oldMsg)
        {
            string str = oldMsg;
            try
            {
                str = ((ReturnCode) code).ToString();
            }
            catch
            {
            }
            return str;
        }

        public static Hidistro.Messages.GetIndustryJsonResult GetIndustryJsonResult(string accessToken)
        {
            string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/get_industry?access_token={0}";
            return SendCommonJson<Hidistro.Messages.GetIndustryJsonResult>(accessToken, urlFormat, null);
        }

        public static Hidistro.Messages.GetPrivateTemplateJsonResult GetPrivateTemplateJsonResult(string accessToken)
        {
            string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token={0}";
            return SendCommonJson<Hidistro.Messages.GetPrivateTemplateJsonResult>(accessToken, urlFormat, null);
        }

        public static T GetResult<T>(string returnText) where T: WxJsonResult, new()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(returnText);
            }
            catch
            {
            }
            T local2 = Activator.CreateInstance<T>();
            local2.errcode = -2;
            local2.errmsg = returnText;
            return local2;
        }

        private static List<WxtemplateId> GetWxtemplateIds()
        {
            List<WxtemplateId> list = new List<WxtemplateId>();
            WxtemplateId item = new WxtemplateId {
                name = "分销商申请成功提醒",
                shortId = "OPENTM207126233",
                templateid = ""
            };
            list.Add(item);
            WxtemplateId id2 = new WxtemplateId {
                name = "账户变更提醒",
                shortId = "TM00370",
                templateid = ""
            };
            list.Add(id2);
            WxtemplateId id3 = new WxtemplateId {
                name = "提现结果提醒",
                shortId = "OPENTM207601150",
                templateid = ""
            };
            list.Add(id3);
            WxtemplateId id4 = new WxtemplateId {
                name = "订单消息提醒",
                shortId = "OPENTM205109409",
                templateid = ""
            };
            list.Add(id4);
            WxtemplateId id5 = new WxtemplateId {
                name = "退款通知",
                shortId = "TM00599",
                templateid = ""
            };
            list.Add(id5);
            WxtemplateId id6 = new WxtemplateId {
                name = "中奖结果通知",
                shortId = "OPENTM204632492",
                templateid = ""
            };
            list.Add(id6);
            WxtemplateId id7 = new WxtemplateId {
                name = "售后申请提醒",
                shortId = "OPENTM401701827",
                templateid = ""
            };
            list.Add(id7);
            WxtemplateId id8 = new WxtemplateId {
                name = "提现申请通知",
                shortId = "OPENTM401873794",
                templateid = ""
            };
            list.Add(id8);
            WxtemplateId id9 = new WxtemplateId {
                name = "商品详情通知",
                shortId = "OPENTM207331564",
                templateid = ""
            };
            list.Add(id9);
            WxtemplateId id10 = new WxtemplateId {
                name = "用户咨询提醒",
                shortId = "OPENTM202119578",
                templateid = ""
            };
            list.Add(id10);
            WxtemplateId id11 = new WxtemplateId {
                name = "会员注册成功提醒",
                shortId = "OPENTM207207788",
                templateid = ""
            };
            list.Add(id11);
            WxtemplateId id12 = new WxtemplateId {
                name = "下级会员注册提示",
                shortId = "OPENTM207777500",
                templateid = ""
            };
            list.Add(id12);

            WxtemplateId id13 = new WxtemplateId
            {
                name = "订单状态更新",
                shortId = "TM00017",
                templateid = ""
            };
            list.Add(id13);
            return list;
        }

        public static void Logwx(string msg)
        {
            Globals.Debuglog(msg, "WxTemplate.txt");
        }

        public static WxJsonResult QuickSetWeixinTemplates()
        {
            string accessToken = GetAccessToken();
            if (string.IsNullOrEmpty(accessToken) || accessToken.Contains("errcode"))
            {
                return new WxJsonResult { errcode = 0x9c41, errmsg = "Token获取失败" };
            }
            Hidistro.Messages.GetIndustryJsonResult industryJsonResult = GetIndustryJsonResult(accessToken);
            if ((industryJsonResult.errcode != 0) && (industryJsonResult.errcode != -1))
            {
                industryJsonResult.errmsg = GetErrorMsg(industryJsonResult.errcode, industryJsonResult.errmsg);
                return industryJsonResult;
            }
            if (((industryJsonResult.errcode == -1) || (industryJsonResult.primary_industry.ConvertToIndustryCode() != IndustryCode.IT科技_互联网_电子商务)) || (industryJsonResult.secondary_industry.ConvertToIndustryCode() != IndustryCode.IT科技_IT软件与服务))
            {
                WxJsonResult result3 = SetIndustry();
                if (result3.errcode != 0)
                {
                    result3.errmsg = GetErrorMsg(result3.errcode, result3.errmsg);
                    return result3;
                }
            }
            Hidistro.Messages.GetPrivateTemplateJsonResult privateTemplateJsonResult = GetPrivateTemplateJsonResult(accessToken);
            if (privateTemplateJsonResult.errcode != 0)
            {
                privateTemplateJsonResult.errmsg = GetErrorMsg(privateTemplateJsonResult.errcode, privateTemplateJsonResult.errmsg);
                return privateTemplateJsonResult;
            }
            List<GetPrivateTemplate_TemplateItem> source = privateTemplateJsonResult.template_list;
            List<WxtemplateId> wxtemplateIds = GetWxtemplateIds();
            int count = source.Count;
            using (List<WxtemplateId>.Enumerator enumerator = wxtemplateIds.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Func<GetPrivateTemplate_TemplateItem, bool> predicate = null;
                    WxtemplateId wxtempitem = enumerator.Current;
                    if (predicate == null)
                    {
                        predicate = t => (t.title == wxtempitem.name) && (t.primary_industry == "IT科技");
                    }
                    GetPrivateTemplate_TemplateItem item = source.FirstOrDefault<GetPrivateTemplate_TemplateItem>(predicate);
                    if (item != null)
                    {
                        wxtempitem.templateid = item.template_id;
                    }
                    else
                    {
                        if (count >= 0x19)
                        {
                            wxtempitem.templateid = "公众号已有模板数量越额了！";
                            continue;
                        }
                        Hidistro.Messages.AddtemplateJsonResult result5 = AddtemplateJsonResult(accessToken, wxtempitem.shortId);
                        if (result5.errcode != 0)
                        {
                            wxtempitem.templateid = result5.errmsg;
                            continue;
                        }
                        count++;
                        wxtempitem.templateid = result5.template_id;
                    }
                }
            }
            return new WxJsonResult { errcode = 0, errmsg = "设置成功", AppendData = wxtemplateIds };
        }

        public static T SendCommonJson<T>(string accessToken, string urlFormat, object data = null) where T: WxJsonResult, new()
        {
            WebUtils utils = new WebUtils();
            string str = string.IsNullOrEmpty(accessToken) ? urlFormat : string.Format(urlFormat, accessToken.AsUrlData());
            string str2 = "";
            if (data != null)
            {
                str2 = JsonConvert.SerializeObject(data);
            }
            return GetResult<T>(utils.HttpSend(str, str2));
        }

        public static WxTemplateMessageResult SendTemplateMessage(string accessTocken, TempleteModel TempleteModel)
        {
            string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            return SendCommonJson<WxTemplateMessageResult>(accessTocken, urlFormat, TempleteModel);
        }

        public static WxJsonResult SetIndustry()
        {
            string accessToken = GetAccessToken();
            if (string.IsNullOrEmpty(accessToken) || accessToken.Contains("errcode"))
            {
                return new WxJsonResult { errcode = 0x9c41, errmsg = "令牌获取失败" };
            }
            string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token={0}";
            var data = new {
                industry_id1 = "1",
                industry_id2 = "2"
            };
            return SendCommonJson<WxJsonResult>(accessToken, urlFormat, data);
        }
    }
}

