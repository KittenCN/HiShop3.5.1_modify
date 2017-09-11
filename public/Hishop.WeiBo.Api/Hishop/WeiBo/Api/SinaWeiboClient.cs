﻿namespace Hishop.WeiBo.Api
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    public class SinaWeiboClient : OpenAuthenticationClientBase
    {
        private const string API_URL = "https://api.weibo.com/2/";
        private const string AUTH_URL = "https://api.weibo.com/oauth2/authorize";
        private const string TOKEN_URL = "https://api.weibo.com/oauth2/access_token";

        public SinaWeiboClient(string appKey, string appSecret, string callbackUrl, string accessToken = null, string uid = null) : base(appKey, appSecret, callbackUrl, accessToken)
        {
            base.ClientName = "SinaWeibo";
            this.UID = uid;
            if (!string.IsNullOrEmpty(accessToken) || !string.IsNullOrEmpty(uid))
            {
                base.isAccessTokenSet = true;
            }
        }

        public override void GetAccessTokenByCode(string code)
        {
            HttpResponseMessage message = base.HttpPost("https://api.weibo.com/oauth2/access_token", new { client_id = base.ClientId, client_secret = base.ClientSecret, grant_type = "authorization_code", code = code, redirect_uri = base.CallbackUrl });
            if (message.StatusCode == HttpStatusCode.OK)
            {
                JObject obj2 = JObject.Parse(message.Content.ReadAsStringAsync().Result);
                if (obj2["access_token"] != null)
                {
                    base.AccessToken = obj2.Value<string>("access_token");
                    this.UID = obj2.Value<string>("uid");
                    base.isAccessTokenSet = true;
                }
            }
        }

        public override string GetAuthorizationUrl()
        {
            UriBuilder builder = new UriBuilder(this.AuthorizationCodeUrl) {
                Query = string.Format("client_id={0}&response_type=code&redirect_uri={1}", base.ClientId, Uri.EscapeDataString(base.CallbackUrl))
            };
            return builder.ToString();
        }

        public override Task<HttpResponseMessage> HttpGetAsync(string api, Dictionary<string, object> parameters)
        {
            if (base.IsAuthorized)
            {
                if (parameters == null)
                {
                    parameters = new Dictionary<string, object>();
                }
                if (!parameters.ContainsKey("source"))
                {
                    parameters["source"] = base.ClientId;
                }
                if (!parameters.ContainsKey("access_token"))
                {
                    parameters["access_token"] = base.AccessToken;
                }
            }
            return base.HttpGetAsync(api, parameters);
        }

        public override Task<HttpResponseMessage> HttpPostAsync(string api, Dictionary<string, object> parameters)
        {
            if (base.IsAuthorized)
            {
                if (parameters == null)
                {
                    parameters = new Dictionary<string, object>();
                }
                if (!parameters.ContainsKey("source"))
                {
                    parameters["source"] = base.ClientId;
                }
                if (!parameters.ContainsKey("access_token"))
                {
                    parameters["access_token"] = base.AccessToken;
                }
            }
            return base.HttpPostAsync(api, parameters);
        }

        protected override string AccessTokenUrl
        {
            get
            {
                return "https://api.weibo.com/oauth2/access_token";
            }
        }

        protected override string AuthorizationCodeUrl
        {
            get
            {
                return "https://api.weibo.com/oauth2/authorize";
            }
        }

        protected override string BaseApiUrl
        {
            get
            {
                return "https://api.weibo.com/2/";
            }
        }

        public string UID { get; set; }
    }
}

