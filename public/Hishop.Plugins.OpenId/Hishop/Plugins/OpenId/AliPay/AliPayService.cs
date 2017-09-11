namespace Hishop.Plugins.OpenId.AliPay
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;

    [Plugin("支付宝快捷登录", Sequence=1)]
    public class AliPayService : OpenIdService
    {
        private readonly string returnUrl;

        public AliPayService()
        {
        }

        public AliPayService(string returnUrl)
        {
            this.returnUrl = returnUrl;
        }

        public override void Post()
        {
            SortedDictionary<string, string> dicArrayPre = new SortedDictionary<string, string>();
            dicArrayPre.Add("service", "alipay.auth.authorize");
            dicArrayPre.Add("target_service", "user.auth.quick.login");
            dicArrayPre.Add("partner", this.Partner);
            dicArrayPre.Add("_input_charset", "utf-8");
            dicArrayPre.Add("return_url", this.returnUrl);
            Dictionary<string, string> dicArray = Globals.Parameterfilter(dicArrayPre);
            string str = Globals.BuildSign(dicArray, this.Key, "MD5", "utf-8");
            dicArray.Add("sign", str);
            dicArray.Add("sign_type", "MD5");
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in dicArray)
            {
                builder.Append(this.CreateField(pair.Key, pair.Value));
            }
            dicArrayPre.Clear();
            dicArray.Clear();
            this.Submit(this.CreateForm(builder.ToString(), "https://mapi.alipay.com/gateway.do?_input_charset=utf-8"));
        }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        [ConfigElement("安全校验码(Key)", Nullable=false)]
        public string Key { get; set; }

        public override string Logo
        {
            get
            {
                return "Hishop.Plugins.OpenId.AliPay.alipayservice.gif";
            }
        }

        [ConfigElement("合作者身份(PID)", Nullable=false)]
        public string Partner { get; set; }

        [ConfigElement("收款支付宝账号", Nullable=false)]
        public string SellerEmail { get; set; }

        public override string ShortDescription
        {
            get
            {
                return "<div class=\"loginTabao\"><ul><li class=\"taobaoTitle\">支付宝快捷登录简介：</li><li class=\"li1\">海量支付宝用户只要登录支付宝账号，即可在您网站下单购物。快捷登录简化用户购物流程 ，提升网站下单率。</li><li class=\"li2\"><a target=\"_blank\" href=\"http://act.life.alipay.com/systembiz/hishop/?src=hishop\" title=\"在线申请\"><img src=\"../images/kuaijie.gif\"/></a></li></ul><ul class=\"ul2\"><li class=\"taobaoTitle\">签约提示：</li><li>目前快捷登录必须与支付宝即时交易、担保交易、双功能接口搭配使用。您必须全新签约快登+接口套餐</li></ul></div>";
            }
        }
    }
}

