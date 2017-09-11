namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Web.UI;

    [ParseChildren(true)]
    public class HomePage : NewTemplatedWebControl
    {
        protected override void OnInit(EventArgs e)
        {
            this.TempFilePath = "Skin-HomePage.html";
            if (this.SkinName == null)
            {
                this.SkinName = this.TempFilePath;
            }
            base.OnInit(e);
        }

        [Bindable(true)]
        public string TempFilePath { get; set; }
    }
}

