namespace Hishop.Plugins.OpenId.Sina
{
    using Hishop.Plugins;
    using NetDimension.Weibo;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web;

    [Plugin("新浪微博登录", Sequence=4)]
    public class SinaService : OpenIdService
    {
        private readonly string returnUrl;
        private const string ReUrl = "ReturnUrl";

        public SinaService()
        {
        }

        public SinaService(string returnUrl)
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
            string url = new OAuth(this.AppKey, this.AppSecret, this.returnUrl).GetAuthorizeURL(ResponseType.Code, null, DisplayType.Default);
            this.Redirect(url);
        }

        protected override void Redirect(string url)
        {
            base.Redirect(url);
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
                return "hishop.plugins.openid.sina.sinaservice.gif";
            }
        }

        public override string ShortDescription
        {
            get
            {
                return "<div class=\"loginTabao\"><ul><li class=\"taobaoTitle\">新浪微博登录简介：</li><li class=\"li1\">用户只要使用微博账号进行登录即可注册成为网站会员，帮助您提升网站的用户注册量和提升网站访问数据。</li><li class=\"li1\">申请接口输入的回调地址为：http://域名/openid/OpenIdEntry_hishop.plugins.openid.sina.sinaservice.aspx</li><li class=\"li2\"><a target=\"_blank\" href=\"http://open.weibo.com\" title=\"在线申请\"><img src=\"../images/sinalogo.gif\"/></a></li></ul></div>";
            }
        }
    }
}

