namespace Hishop.WeiBo.Api
{
    using System;

    public class GetAuth
    {
        public SinaWeiboClient GetOpenAuthClient(string accessToken)
        {
            string appKey = "";
            string appSecret = "";
            string callbackUrl = "";
            return new SinaWeiboClient(appKey, appSecret, callbackUrl, accessToken, "");
        }
    }
}

