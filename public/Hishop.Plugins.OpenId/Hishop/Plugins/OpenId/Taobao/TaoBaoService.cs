namespace Hishop.Plugins.OpenId.Taobao
{
    using Hishop.Plugins;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web;

    [Plugin("淘宝信任登录", Sequence=3)]
    public class TaoBaoService : OpenIdService
    {
        private readonly string returnUrl;
        private const string ReUrl = "ReturnUrl";

        public TaoBaoService()
        {
        }

        public TaoBaoService(string returnUrl)
        {
            this.returnUrl = returnUrl;
        }

        public override void Post()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["ReturnUrl"];
            if (cookie == null)
            {
                cookie = new HttpCookie("ReturnUrl");
            }
            cookie.Value = this.returnUrl;
            cookie.Expires = DateTime.Now.AddHours(1.0);
            HttpContext.Current.Response.Cookies.Add(cookie);
            base.Redirect("https://oauth.taobao.com/authorize?response_type=code&client_id=" + this.AppKey + "&redirect_uri=" + this.returnUrl + "&state=hishop");
        }

        [ConfigElement("App Key", Nullable=false)]
        public string AppKey { get; set; }

        [ConfigElement("AppSecret", Nullable=false)]
        public string AppSecret { get; set; }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        public override string Logo
        {
            get
            {
                return "hishop.plugins.openid.taobao.taobaoservice.gif";
            }
        }

        public override string ShortDescription
        {
            get
            {
                return "<div class=\"loginTabao\"><ul><li class=\"taobaoTitle\">淘宝信任登录简介：</li><li class=\"li1\">海量淘宝用户只要登录淘宝账号，即可在您网站下单购物。快捷登录简化用户购物流程 ，提升网站下单率。</li><li class=\"li2\"><a target=\"_blank\" href=\"http://open.taobao.com/index.htm\" title=\"在线申请\"><img src=\"../images/kuaijie.gif\"/></a></li></ul></div>";
            }
        }
    }
}

