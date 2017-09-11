namespace Hidistro.UI.Web.Admin.Oneyuan
{
    using ASPNET.WebControls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class OneTaoViewTab : UserControl
    {
        protected PageSize hrefPageSize;
        protected HtmlGenericControl LiViewTab1;
        protected HtmlGenericControl LiViewTab2;
        protected HtmlGenericControl LiViewTab3;
        protected HtmlGenericControl mytabl;
        protected HtmlGenericControl pageSizeSet;
        protected HtmlAnchor ViewTab1;
        protected HtmlAnchor ViewTab2;
        protected HtmlAnchor ViewTab3;

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString["vaid"];
            if (!string.IsNullOrEmpty(str))
            {
                string str2 = this.Page.Request.Url.ToString();
                this.pageSizeSet.Visible = false;
                if (str2.Contains(this.ViewTab1.HRef))
                {
                    this.LiViewTab1.Attributes.Add("class", "active");
                }
                else if (str2.Contains(this.ViewTab2.HRef))
                {
                    this.LiViewTab2.Attributes.Add("class", "active");
                }
                else if (str2.Contains(this.ViewTab3.HRef))
                {
                    this.LiViewTab3.Attributes.Add("class", "active");
                    this.pageSizeSet.Visible = true;
                }
                this.ViewTab1.HRef = this.ViewTab1.HRef + "?vaid=" + str;
                this.ViewTab2.HRef = this.ViewTab2.HRef + "?vaid=" + str;
                this.ViewTab3.HRef = this.ViewTab3.HRef + "?vaid=" + str;
            }
            else
            {
                this.mytabl.Visible = false;
            }
        }
    }
}

