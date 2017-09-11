namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Web;

    public static class HiAffiliation
    {
        public static string GetReturnUrl(string returnUrl)
        {
            if (returnUrl.IndexOf("?") > -1)
            {
                returnUrl = returnUrl.Substring(returnUrl.IndexOf("?"));
            }
            return returnUrl;
        }

        public static void LoadPage()
        {
            string str = ReturnUrl();
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("\n", "");
                HttpContext.Current.Response.Redirect(str);
            }
        }

        public static string ReturnUrl()
        {
            if (Globals.GetCurrentMemberUserId(false) > 0)
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                if (currentMember != null)
                {
                    return ReturnUrlByUser(currentMember);
                }
            }
            return ReturnUrlByQueryString();
        }

        public static string ReturnUrlByQueryString()
        {
            int userId = 0;
            string str = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("returnUrl"))
            {
                if (HttpContext.Current.Request.Url.AbsolutePath == "/logout.aspx")
                {
                    return string.Empty;
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ReferralId"]))
                {
                    userId = Globals.RequestQueryNum("ReferralId");
                    if (userId > 0)
                    {
                        if (DistributorsBrower.GetCurrentDistributors(userId, true) == null)
                        {
                            SetReferralIdCookie("0", "", false);
                            return "/Default.aspx?ReferralId=0";
                        }
                        HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                        if (cookie == null)
                        {
                            SetReferralIdCookie(userId.ToString(), "", false);
                        }
                        else if ((cookie != null) && (cookie.Value != userId.ToString()))
                        {
                            SetReferralIdCookie(userId.ToString(), "", false);
                        }
                    }
                }
                else
                {
                    HttpCookie cookie2 = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                    if ((cookie2 != null) && !string.IsNullOrEmpty(cookie2.Value))
                    {
                        if (HttpContext.Current.Request.QueryString.Count > 0)
                        {
                            if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("ReferralId"))
                            {
                                return (str + "&ReferralId=" + cookie2.Value);
                            }
                            return string.Empty;
                        }
                        if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("ReferralId"))
                        {
                            return (str + "?ReferralId=" + cookie2.Value);
                        }
                        return string.Empty;
                    }
                }
                if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("returnUrl") && (HttpContext.Current.Request.Url.AbsolutePath != "/logout.aspx"))
                {
                    if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("ReferralId") && (HttpContext.Current.Request.QueryString.Count > 0))
                    {
                        return (str + "&ReferralId=" + userId.ToString());
                    }
                    if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("ReferralId"))
                    {
                        return (str + "?ReferralId=" + userId.ToString());
                    }
                }
            }
            return string.Empty;
        }

        public static string ReturnUrlByUser(MemberInfo mInfo)
        {
            string str = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.ToNum(mInfo.UserId), true);
            if (currentDistributors != null)
            {
                SetReferralIdCookie(currentDistributors.UserId.ToString(), "", false);
            }
            else
            {
                SetReferralIdCookie(mInfo.ReferralUserId.ToString(), "", false);
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ReferralId"]))
                {
                    HiUriHelp help = new HiUriHelp(HttpContext.Current.Request.QueryString);
                    string queryString = help.GetQueryString("ReferralId");
                    if (!string.IsNullOrEmpty(queryString))
                    {
                        if (queryString == cookie.Value)
                        {
                            return string.Empty;
                        }
                        help.SetQueryString("ReferralId", cookie.Value);
                        return (HttpContext.Current.Request.Url.AbsolutePath + help.GetNewQuery());
                    }
                }
                if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("returnUrl"))
                {
                    if (HttpContext.Current.Request.Url.AbsolutePath == "/logout.aspx")
                    {
                        return string.Empty;
                    }
                    if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("ReferralId") && (HttpContext.Current.Request.QueryString.Count > 0))
                    {
                        return (str + "&ReferralId=" + cookie.Value);
                    }
                    if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("ReferralId"))
                    {
                        return (str + "?ReferralId=" + cookie.Value);
                    }
                }
            }
            return string.Empty;
        }

        public static void SetReferralIdCookie(string referralId, string url = "", bool isRedirect = false)
        {
            Globals.ClearReferralIdCookie();
            Globals.SetDistributorCookie(Globals.ToNum(referralId));
            if (isRedirect && !string.IsNullOrEmpty(url))
            {
                HttpContext.Current.Response.Redirect(url);
            }
        }
    }
}

