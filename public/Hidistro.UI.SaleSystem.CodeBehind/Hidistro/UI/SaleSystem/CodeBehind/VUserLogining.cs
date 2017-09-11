namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VUserLogining : VMemberTemplatedWebControl
    {
        private HtmlInputHidden hidurl;

        protected override void AttachChildControls()
        {
            this.hidurl = (HtmlInputHidden) this.FindControl("hidurl");
            string str = Regex.Replace(Regex.Match(Globals.UrlDecode(HttpContext.Current.Request.QueryString.ToString()), "(returnUrl=.*)", RegexOptions.IgnoreCase).ToString(), "(returnUrl=)", "", RegexOptions.IgnoreCase);
            this.hidurl.Value = str;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VUserLogining.html";
            }
            base.OnInit(e);
        }
    }
}

