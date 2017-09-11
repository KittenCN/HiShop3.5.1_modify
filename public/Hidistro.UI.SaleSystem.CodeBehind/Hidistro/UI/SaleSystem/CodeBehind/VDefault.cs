namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDefault : VshopTemplatedWebControl
    {
        private DataTable dtpromotion;
        private HtmlImage img;
        private HiImage imglogo;
        protected int itemcount;
        private Literal litattention;
        private Literal litdescription;
        private Literal litImgae;
        private Literal litItemParams;
        private Literal litstorename;
        private Pager pager;
        private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rptProducts;

        protected override void AttachChildControls()
        {
            this.rptCategories = (VshopTemplatedRepeater) this.FindControl("rptCategories");
            this.rptProducts = (VshopTemplatedRepeater) this.FindControl("rptProducts");
            this.rptProducts.ItemDataBound += new RepeaterItemEventHandler(this.rptProducts_ItemDataBound);
            this.rptCategories.ItemDataBound += new RepeaterItemEventHandler(this.rptCategories_ItemDataBound);
            this.img = (HtmlImage) this.FindControl("imgDefaultBg");
            this.pager = (Pager) this.FindControl("pager");
            this.litstorename = (Literal) this.FindControl("litstorename");
            this.litdescription = (Literal) this.FindControl("litdescription");
            this.litattention = (Literal) this.FindControl("litattention");
            this.imglogo = (HiImage) this.FindControl("imglogo");
            this.litImgae = (Literal) this.FindControl("litImgae");
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralId"]))
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
                {
                    this.Page.Response.Redirect("Default.aspx?ReferralId=" + cookie.Value);
                }
            }
            if (this.rptCategories.Visible)
            {
                DataTable brandCategories = CategoryBrowser.GetBrandCategories();
                this.itemcount = brandCategories.Rows.Count;
                if (brandCategories.Rows.Count > 0)
                {
                    this.rptCategories.DataSource = brandCategories;
                    this.rptCategories.DataBind();
                }
            }
            this.Page.Session["stylestatus"] = "3";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            PageTitle.AddSiteNameTitle(masterSettings.SiteName);
            this.litstorename.Text = masterSettings.SiteName;
            this.litdescription.Text = masterSettings.ShopIntroduction;
            if (!string.IsNullOrEmpty(masterSettings.DistributorLogoPic))
            {
                this.imglogo.ImageUrl = masterSettings.DistributorLogoPic.Split(new char[] { '|' })[0];
            }
            if (base.referralId <= 0)
            {
                HttpCookie cookie2 = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                if ((cookie2 != null) && !string.IsNullOrEmpty(cookie2.Value))
                {
                    base.referralId = int.Parse(cookie2.Value);
                    this.Page.Response.Redirect("Default.aspx?ReferralId=" + this.referralId.ToString(), true);
                }
            }
            else
            {
                HttpCookie cookie3 = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                if (((cookie3 != null) && !string.IsNullOrEmpty(cookie3.Value)) && (this.referralId.ToString() != cookie3.Value))
                {
                    this.Page.Response.Redirect("Default.aspx?ReferralId=" + this.referralId.ToString(), true);
                }
            }
            IList<BannerInfo> allBanners = new List<BannerInfo>();
            allBanners = VshopBrowser.GetAllBanners();
            foreach (BannerInfo info in allBanners)
            {
                TplCfgInfo info2 = new NavigateInfo {
                    LocationType = info.LocationType,
                    Url = info.Url
                };
                string loctionUrl = "javascript:";
                if (!string.IsNullOrEmpty(info.Url))
                {
                    loctionUrl = info2.LoctionUrl;
                }
                string text = this.litImgae.Text;
                this.litImgae.Text = text + "<a  id=\"ahref\" href='" + loctionUrl + "'><img src=\"" + info.ImageUrl + "\" title=\"" + info.ShortDesc + "\" alt=\"" + info.ShortDesc + "\" /></a>";
            }
            if (allBanners.Count == 0)
            {
                this.litImgae.Text = "<a id=\"ahref\"  href='javascript:'><img src=\"/Utility/pics/default.jpg\" title=\"\"  /></a>";
            }
            DistributorsInfo userIdDistributors = new DistributorsInfo();
            userIdDistributors = DistributorsBrower.GetUserIdDistributors(base.referralId);
            if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
            {
                PageTitle.AddSiteNameTitle(userIdDistributors.StoreName);
                this.litdescription.Text = userIdDistributors.StoreDescription;
                this.litstorename.Text = userIdDistributors.StoreName;
                if (!string.IsNullOrEmpty(userIdDistributors.Logo))
                {
                    this.imglogo.ImageUrl = userIdDistributors.Logo;
                }
                else if (!string.IsNullOrEmpty(masterSettings.DistributorLogoPic))
                {
                    this.imglogo.ImageUrl = masterSettings.DistributorLogoPic.Split(new char[] { '|' })[0];
                }
                if (!string.IsNullOrEmpty(userIdDistributors.BackImage))
                {
                    this.litImgae.Text = "";
                    foreach (string str2 in userIdDistributors.BackImage.Split(new char[] { '|' }))
                    {
                        if (!string.IsNullOrEmpty(str2))
                        {
                            this.litImgae.Text = this.litImgae.Text + "<a ><img src=\"" + str2 + "\" title=\"\"  /></a>";
                        }
                    }
                }
            }
            this.dtpromotion = ProductBrowser.GetAllFull();
            if (this.rptProducts != null)
            {
                ProductQuery query = new ProductQuery {
                    PageSize = this.pager.PageSize,
                    PageIndex = this.pager.PageIndex,
                    SortBy = "DisplaySequence",
                    SortOrder = SortAction.Desc
                };
                DbQueryResult homeProduct = ProductBrowser.GetHomeProduct(MemberProcessor.GetCurrentMember(), query);
                this.rptProducts.DataSource = homeProduct.Data;
                this.rptProducts.DataBind();
                this.pager.TotalRecords = homeProduct.TotalRecords;
                if (this.pager.TotalRecords <= this.pager.PageSize)
                {
                    this.pager.Visible = false;
                }
            }
            if (this.img != null)
            {
                this.img.Src = new VTemplateHelper().GetDefaultBg();
            }
            if (!string.IsNullOrEmpty(masterSettings.GuidePageSet))
            {
                this.litattention.Text = masterSettings.GuidePageSet;
            }
            if ((this.Page.Request.UserAgent.ToLower().Contains("alipay") && !string.IsNullOrEmpty(masterSettings.AliPayFuwuGuidePageSet)) && !string.IsNullOrEmpty(masterSettings.GuidePageSet))
            {
                this.litattention.Text = masterSettings.AliPayFuwuGuidePageSet;
            }
            string str4 = "";
            if (!string.IsNullOrEmpty(masterSettings.ShopHomePic))
            {
                str4 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.ShopHomePic;
            }
            string str5 = "";
            string str6 = (userIdDistributors == null) ? masterSettings.SiteName : userIdDistributors.StoreName;
            if (!string.IsNullOrEmpty(masterSettings.DistributorBackgroundPic))
            {
                str5 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.DistributorBackgroundPic.Split(new char[] { '|' })[0];
            }
            this.litItemParams.Text = str4 + "|" + masterSettings.ShopHomeName + "|" + masterSettings.ShopHomeDescription + "$";
            this.litItemParams.Text = string.Concat(new object[] { this.litItemParams.Text, str5, "|好店推荐之", str6, "商城|一个购物赚钱的好去处|", HttpContext.Current.Request.Url });
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VDefault.html";
            }
            base.OnInit(e);
        }

        private void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                if (((e.Item.ItemIndex + 1) % 4) == 1)
                {
                    Literal literal = (Literal) e.Item.Controls[0].FindControl("litStart");
                    literal.Visible = true;
                }
                else if ((((e.Item.ItemIndex + 1) % 4) == 0) || ((e.Item.ItemIndex + 1) == this.itemcount))
                {
                    Literal literal2 = (Literal) e.Item.Controls[0].FindControl("litEnd");
                    literal2.Visible = true;
                }
                Literal literal3 = (Literal) e.Item.Controls[0].FindControl("litpromotion");
                if (!string.IsNullOrEmpty(literal3.Text))
                {
                    literal3.Text = "<img src='" + literal3.Text + "'/>";
                }
                else
                {
                    literal3.Text = "<img src='/Storage/master/default.png'/>";
                }
            }
        }

        private void rptProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal) e.Item.Controls[0].FindControl("litpromotion");
                string str = "";
                if (DataBinder.Eval(e.Item.DataItem, "MainCategoryPath") != null)
                {
                    str = DataBinder.Eval(e.Item.DataItem, "MainCategoryPath").ToString();
                }
                DataView defaultView = this.dtpromotion.DefaultView;
                if (!string.IsNullOrEmpty(str))
                {
                    defaultView.RowFilter = " ActivitiesType=0 ";
                    if (defaultView.Count > 0)
                    {
                        literal.Text = "<span class=\"sale-favourable\"><i>满" + decimal.Parse(defaultView[0]["MeetMoney"].ToString()).ToString("0") + "</i><i>减" + decimal.Parse(defaultView[0]["ReductionMoney"].ToString()).ToString("0") + "</i></span>";
                    }
                    else
                    {
                        defaultView.RowFilter = " ActivitiesType= " + str.Split(new char[] { '|' })[0].ToString();
                        if (defaultView.Count > 0)
                        {
                            literal.Text = "<span class=\"sale-favourable\"><i>满" + decimal.Parse(defaultView[0]["MeetMoney"].ToString()).ToString("0") + "</i><i>减" + decimal.Parse(defaultView[0]["ReductionMoney"].ToString()).ToString("0") + "</i></span>";
                        }
                    }
                }
            }
        }
    }
}

