namespace Hishop.AlipayFuwu.Api.Model
{
    using Aop.Api;
    using Aop.Api.Request;
    using Aop.Api.Response;
    using Aop.Api.Util;
    using System;
    using System.Runtime.InteropServices;
    using System.Web;

    public class AlipayOHClient
    {
        private const string ACCESS_TOKEN = "auth_token";
        private string aliPubKey;
        private const string APP_ID = "app_id";
        private string appId;
        private const string BIZ_CONTENT = "biz_content";
        private string charset;
        private const string CHARSET = "charset";
        private const string CONTENT = "biz_content";
        private HttpContext context;
        private string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        private string format;
        private const string FORMAT = "format";
        private const string METHOD = "method";
        private string privateKey;
        private const string PROD_CODE = "prod_code";
        private string pubKey;
        private string serverUrl;
        private const string SERVICE = "service";
        private const string SIGN = "sign";
        private const string SIGN_TYPE = "sign_type";
        private string signType = "RSA";
        private const string SING = "sign";
        private const string SING_TYPE = "sign_type";
        private const string TERMINAL_INFO = "terminal_info";
        private const string TERMINAL_TYPE = "terminal_type";
        private const string TIMESTAMP = "timestamp";
        private string version;
        private const string VERSION = "version";
        private WebUtils webUtils = new WebUtils();

        public AlipayOHClient(string url, string appId, string aliPubKey, string priKey, string pubKey, string charset = "UTF-8")
        {
            this.serverUrl = url;
            this.appId = appId;
            this.privateKey = priKey;
            this.charset = charset;
            this.pubKey = pubKey;
            this.aliPubKey = aliPubKey;
        }

        public AlipayUserUserinfoShareResponse GetAliUserInfo(string accessToken)
        {
            AlipayUserUserinfoShareRequest request = new AlipayUserUserinfoShareRequest();
            IAopClient client = new DefaultAopClient(this.serverUrl, this.appId, this.privateKey);
            return client.Execute<AlipayUserUserinfoShareResponse>(request, accessToken);
        }

        public AlipaySystemOauthTokenResponse OauthTokenRequest(string authCode)
        {
            AlipaySystemOauthTokenRequest request = new AlipaySystemOauthTokenRequest {
                GrantType = "authorization_code",
                Code = authCode
            };
            AlipaySystemOauthTokenResponse response = null;
            try
            {
                IAopClient client = new DefaultAopClient(this.serverUrl, this.appId, this.privateKey);
                response = client.Execute<AlipaySystemOauthTokenResponse>(request);
            }
            catch (AopException)
            {
            }
            return response;
        }
    }
}

