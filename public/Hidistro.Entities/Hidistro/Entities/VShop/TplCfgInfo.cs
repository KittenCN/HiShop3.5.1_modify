namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web;

    public class TplCfgInfo
    {
        public int BannerId { get; set; }

        public int DisplaySequence { get; set; }

        public int Id
        {
            get
            {
                return this.BannerId;
            }
            set
            {
                this.Id = this.BannerId;
            }
        }

        public string ImageUrl { get; set; }

        public bool IsDisable { get; set; }

        public Hidistro.Entities.VShop.LocationType LocationType { get; set; }

        public virtual string LoctionUrl
        {
            get
            {
                int port = HttpContext.Current.Request.Url.Port;
                string str = HttpContext.Current.Request.Url.Host + ((port == 80) ? "" : (":" + port.ToString()));
                string url = string.Empty;
                switch (this.LocationType)
                {
                    case Hidistro.Entities.VShop.LocationType.Vote:
                        return string.Format("http://{0}/Vshop/Vote.aspx?VoteId={1}", str, this.Url);

                    case Hidistro.Entities.VShop.LocationType.Activity:
                        {
                            string[] strArray = this.Url.Split(new char[] { ',' });
                            LotteryActivityType type = (LotteryActivityType)Enum.Parse(typeof(LotteryActivityType), strArray[0]);
                            switch (type)
                            {
                                case LotteryActivityType.SignUp:
                                    return url;
                            }
                            return string.Format("http://{0}/Vshop/Activity.aspx?id={1}", str, strArray[1]);
                        }
                    case Hidistro.Entities.VShop.LocationType.Home:
                        return string.Format("http://{0}Default.aspx", str);

                    case Hidistro.Entities.VShop.LocationType.Category:
                        return string.Format("http://{0}ProductList.aspx", str);

                    case Hidistro.Entities.VShop.LocationType.ShoppingCart:
                        return string.Format("http://{0}/Vshop/ShoppingCart.aspx", str);

                    case Hidistro.Entities.VShop.LocationType.OrderCenter:
                        return string.Format("http://{0}/Vshop/MemberCenter.aspx", str);

                    case (Hidistro.Entities.VShop.LocationType.OrderCenter | Hidistro.Entities.VShop.LocationType.Vote):
                    case (Hidistro.Entities.VShop.LocationType.Address | Hidistro.Entities.VShop.LocationType.Vote):
                        return url;

                    case Hidistro.Entities.VShop.LocationType.Link:
                        url = "http://" + this.Url;
                        if (this.Url.IndexOf("http") > -1)
                        {
                            url = this.Url;
                        }
                        return url;

                    case Hidistro.Entities.VShop.LocationType.Phone:
                        url = "tel://" + this.Url;
                        if (this.Url.IndexOf("tel") > -1)
                        {
                            url = this.Url;
                        }
                        return url;

                    case Hidistro.Entities.VShop.LocationType.Address:
                        return this.Url;

                    case Hidistro.Entities.VShop.LocationType.Brand:
                        return "/vshop/BrandList.aspx";
                }
                return url;
            }
        }

        public string ShortDesc { get; set; }

        public int Type { get; set; }

        public string Url { get; set; }
    }
}

