namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class Common_Footer : SimpleTemplatedWebControl
    {
        private int isShowNav;

        protected override void AttachChildControls()
        {
            string str = string.Empty;
            if (this.Page.Request.UserAgent.ToLower().Contains("micromessenger") || (Globals.RequestQueryNum("istest") == 1))
            {
                str = "<script>WinXinShareMessage(wxinshare_title, wxinshare_desc, wxinshare_link, wxinshare_imgurl);</script>";
            }
            if (this.isShowNav == 1)
            {
                str = str + "<div style='padding-top:60px'></div><script>$(function () { jQuery.getJSON('/api/Hi_Ajax_NavMenu.ashx?" + DateTime.Now.ToString("MMddHHmm") + "', function (settingjson) {   $(_.template($('#menu').html())(settingjson)).appendTo('body'); GetUICss();});  }) </script>";
            }
            Literal literal = (Literal) this.FindControl("litJs");
            literal.Text = str;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "tags/skin-Common_Footer.html";
            }
            base.OnInit(e);
        }

        public int IsShowNav
        {
            get
            {
                return this.isShowNav;
            }
            set
            {
                this.isShowNav = value;
            }
        }
    }
}

