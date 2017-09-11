namespace Hidistro.Entities.VShop
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Web;

    public class MenuInfo
    {
        public int Bind { get; set; }

        public Hidistro.Entities.VShop.BindType BindType
        {
            get
            {
                switch (this.Bind)
                {
                    case 0:
                        return Hidistro.Entities.VShop.BindType.None;

                    case 1:
                        return Hidistro.Entities.VShop.BindType.Key;

                    case 2:
                        return Hidistro.Entities.VShop.BindType.Topic;

                    case 3:
                        return Hidistro.Entities.VShop.BindType.HomePage;

                    case 4:
                        return Hidistro.Entities.VShop.BindType.ProductCategory;

                    case 5:
                        return Hidistro.Entities.VShop.BindType.ShoppingCar;

                    case 6:
                        return Hidistro.Entities.VShop.BindType.OrderCenter;

                    case 7:
                        return Hidistro.Entities.VShop.BindType.MemberCard;

                    case 8:
                        return Hidistro.Entities.VShop.BindType.Url;

                    case 9:
                        return Hidistro.Entities.VShop.BindType.StoreCard;
                }
                return Hidistro.Entities.VShop.BindType.None;
            }
        }

        public virtual string BindTypeName
        {
            get
            {
                switch (this.BindType)
                {
                    case Hidistro.Entities.VShop.BindType.Key:
                        return "关键字";

                    case Hidistro.Entities.VShop.BindType.Topic:
                        return "专题";

                    case Hidistro.Entities.VShop.BindType.HomePage:
                        return "首页";

                    case Hidistro.Entities.VShop.BindType.ProductCategory:
                        return "分类页";

                    case Hidistro.Entities.VShop.BindType.ShoppingCar:
                        return "购物车";

                    case Hidistro.Entities.VShop.BindType.OrderCenter:
                        return "会员中心";

                    case Hidistro.Entities.VShop.BindType.MemberCard:
                        return "会员卡";

                    case Hidistro.Entities.VShop.BindType.Url:
                        return "自定义链接";

                    case Hidistro.Entities.VShop.BindType.StoreCard:
                        return "分销商名片";
                }
                return string.Empty;
            }
        }

        public IList<MenuInfo> Chilren { get; set; }

        public string Content { get; set; }

        public int DisplaySequence { get; set; }

        public int MenuId { get; set; }

        public string Name { get; set; }

        public int ParentMenuId { get; set; }

        public int ReplyId { get; set; }

        public string Type { get; set; }

        public virtual string Url
        {
            get
            {
                string host = HttpContext.Current.Request.Url.Host;
                switch (this.BindType)
                {
                    case Hidistro.Entities.VShop.BindType.Key:
                        return this.ReplyId.ToString();

                    case Hidistro.Entities.VShop.BindType.Topic:
                        return string.Format("http://{0}/Vshop/Topics.aspx?TopicId={1}", host, this.Content);

                    case Hidistro.Entities.VShop.BindType.HomePage:
                        return string.Format("http://{0}/Default.aspx", host);

                    case Hidistro.Entities.VShop.BindType.ProductCategory:
                        return string.Format("http://{0}/ProductSearch.aspx", host);

                    case Hidistro.Entities.VShop.BindType.ShoppingCar:
                        return string.Format("http://{0}/Vshop/ShoppingCart.aspx", host);

                    case Hidistro.Entities.VShop.BindType.OrderCenter:
                        return string.Format("http://{0}/Vshop/MemberCenter.aspx", host);

                    case Hidistro.Entities.VShop.BindType.MemberCard:
                        return string.Format("http://{0}/Vshop/MemberCard.aspx", host);

                    case Hidistro.Entities.VShop.BindType.Url:
                        return this.Content;
                }
                return string.Empty;
            }
        }
    }
}

