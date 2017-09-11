namespace Hishop.Plugins.OpenId.QQ
{
    using Hishop.Plugins;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web;

    [Plugin("QQ共享登录", Sequence=2)]
    public class QQService : OpenIdService
    {
        private readonly string returnUrl;
        private const string ReUrl = "ReturnUrl";

        public QQService()
        {
        }

        public QQService(string returnUrl)
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
            string url = "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id=" + this.Partner + "&redirect_uri=" + HttpUtility.UrlEncode(this.returnUrl) + "&state=hishop&scope = get_user_info";
            this.Redirect(url);
        }

        protected override void Redirect(string url)
        {
            base.Redirect(url);
        }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        [ConfigElement("App Key", Nullable=false)]
        public string Key { get; set; }

        public override string Logo
        {
            get
            {
                return "hishop.plugins.openid.qq.qqservice.gif";
            }
        }

        [ConfigElement("APP ID", Nullable=false)]
        public string Partner { get; set; }

        public override string ShortDescription
        {
            get
            {
                return "<div class=\"loginTabao\"><ul><li class=\"taobaoTitle\">QQ登录简介：</li><li class=\"li1\">用户使用已有的QQ号码即可登录网站，QQ一键登录更可减少登录交互操作，大大降低网站注册门槛，提升购物体验，给网站带来海量新用户。</li><li class=\"li1\">同时打通QQ空间、朋友网、腾讯微博三大平台，一站互联全线打通。</li><li class=\"li2\"><a target=\"_blank\" href=\"http://connect.opensns.qq.com/\" title=\"在线申请\"><img src=\"../images/qqlogin.gif\"/></a></li></ul></div>";
            }
        }
    }
}

