namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VActivity : VMemberTemplatedWebControl
    {
        private HiImage img;
        private Literal litDescription;

        protected override void AttachChildControls()
        {
            int num;
            int.TryParse(HttpContext.Current.Request.QueryString.Get("id"), out num);
            ActivityInfo activity = VshopBrowser.GetActivity(num);
            if (activity == null)
            {
                base.GotoResourceNotFound("");
            }
            this.img = (HiImage) this.FindControl("img");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.img.ImageUrl = activity.PicUrl;
            this.litDescription.Text = activity.Description;
            PageTitle.AddSiteNameTitle("微报名");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vActivity.html";
            }
            base.OnInit(e);
        }
    }
}

