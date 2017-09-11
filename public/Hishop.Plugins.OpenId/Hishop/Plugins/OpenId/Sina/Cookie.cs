namespace Hishop.Plugins.OpenId.Sina
{
    using System;
    using System.Reflection;
    using System.Web;

    public class Cookie
    {
        private bool _encrypt;
        private int _expires = 15;
        private DateTime _expiresTime;
        private string _name = string.Empty;
        private Hishop.Plugins.OpenId.Sina.TimeUnit _unit;

        public Cookie(string cookieName, int expires, Hishop.Plugins.OpenId.Sina.TimeUnit unit)
        {
            this.Name = cookieName;
            this.Expires = expires;
            this.TimeUnit = unit;
        }

        private DateTime CacluteExpiresTime()
        {
            DateTime now = DateTime.Now;
            switch (this.TimeUnit)
            {
                case Hishop.Plugins.OpenId.Sina.TimeUnit.Minute:
                    return now.AddMinutes((double) this.Expires);

                case Hishop.Plugins.OpenId.Sina.TimeUnit.Hour:
                    return now.AddHours((double) this.Expires);

                case Hishop.Plugins.OpenId.Sina.TimeUnit.Day:
                    return now.AddDays((double) this.Expires);

                case Hishop.Plugins.OpenId.Sina.TimeUnit.Month:
                    return now.AddMonths(this.Expires);

                case Hishop.Plugins.OpenId.Sina.TimeUnit.Year:
                    return now.AddYears(this.Expires);
            }
            return now;
        }

        private string GetCookieValue(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(this.Name);
            if (cookie == null)
            {
                return null;
            }
            if (this.Encrypt)
            {
                return DESC.Decrypt(cookie.Values[key]);
            }
            return cookie.Values[key];
        }

        public static string GetValue(string key)
        {
            if (HttpContext.Current.Request.Cookies.Get(key) == null)
            {
                return string.Empty;
            }
            return HttpContext.Current.Request.Cookies[key].Value;
        }

        private void SetCookieValue(string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(this.Name);
            if (cookie == null)
            {
                cookie = new HttpCookie(this.Name);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
            if (string.IsNullOrEmpty(value))
            {
                cookie.Values.Remove(key);
            }
            else if (this.Encrypt)
            {
                cookie.Values[key] = DESC.Encrypt(value);
            }
            else
            {
                cookie.Values[key] = value;
            }
            if (this.Expires != 0)
            {
                cookie.Expires = this.CacluteExpiresTime();
            }
            HttpContext.Current.Response.SetCookie(cookie);
        }

        public static void SetValue(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key) {
                Value = value
            };
            if (HttpContext.Current.Response.Cookies.Get(key) != null)
            {
                HttpContext.Current.Response.SetCookie(cookie);
            }
            else
            {
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        public static void SetValue(string key, string value, DateTime expiresTime)
        {
            HttpCookie cookie = new HttpCookie(key) {
                Value = value,
                Expires = expiresTime
            };
            if (HttpContext.Current.Response.Cookies.Get(key) != null)
            {
                HttpContext.Current.Response.SetCookie(cookie);
            }
            else
            {
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        public static void SetValue(string key, string value, int minutes)
        {
            HttpCookie cookie = new HttpCookie(key) {
                Value = value,
                Expires = DateTime.Now.AddMinutes((double) minutes)
            };
            if (HttpContext.Current.Response.Cookies.Get(key) != null)
            {
                HttpContext.Current.Response.SetCookie(cookie);
            }
            else
            {
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        public bool Encrypt
        {
            get
            {
                return this._encrypt;
            }
            set
            {
                this._encrypt = value;
            }
        }

        public int Expires
        {
            get
            {
                return this._expires;
            }
            set
            {
                this._expires = value;
            }
        }

        public DateTime ExpiresTime
        {
            get
            {
                return this._expiresTime;
            }
            set
            {
                this._expiresTime = value;
            }
        }

        public string this[string key]
        {
            get
            {
                return this.GetCookieValue(key);
            }
            set
            {
                this.SetCookieValue(key, value);
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public Hishop.Plugins.OpenId.Sina.TimeUnit TimeUnit
        {
            get
            {
                return this._unit;
            }
            set
            {
                this._unit = value;
            }
        }
    }
}

