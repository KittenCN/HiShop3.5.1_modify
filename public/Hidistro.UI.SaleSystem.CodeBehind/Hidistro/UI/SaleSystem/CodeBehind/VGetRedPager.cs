namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VGetRedPager : VshopTemplatedWebControl
    {
        private HtmlInputHidden hdId;
        private HtmlInputHidden hdUserId;

        protected override void AttachChildControls()
        {
            string str = HttpContext.Current.Request.QueryString.Get("id");
            string str2 = HttpContext.Current.Request.QueryString.Get("userid");
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str2))
            {
                this.hdId = (HtmlInputHidden) this.FindControl("hdID");
                this.hdUserId = (HtmlInputHidden) this.FindControl("hdUserid");
                this.hdId.Value = str;
                this.hdUserId.Value = str2;
            }
            PageTitle.AddSiteNameTitle("获取优惠券");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VGetRedPager.html";
            }
            base.OnInit(e);
        }
    }
}

