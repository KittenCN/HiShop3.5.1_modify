namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PersistChildren(false), ParseChildren(true)]
    public abstract class CustomDraftTemplatedWebControl : WebControl, INamingContainer
    {
        private ITemplate _skinTemplate;
        private string skinName;

        protected CustomDraftTemplatedWebControl()
        {
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            this.LoadHtmlThemedControl();
        }

        protected bool LoadHtmlThemedControl()
        {
            string str = File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), Encoding.UTF8);
            if (!string.IsNullOrEmpty(str))
            {
                Control child = this.Page.ParseControl(str);
                child.ID = "_";
                this.Controls.Add(child);
                return true;
            }
            return false;
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
        }

        public override ControlCollection Controls
        {
            get
            {
                this.EnsureChildControls();
                return base.Controls;
            }
        }

        public override System.Web.UI.Page Page
        {
            get
            {
                if (base.Page == null)
                {
                    base.Page = HttpContext.Current.Handler as System.Web.UI.Page;
                }
                return base.Page;
            }
            set
            {
                base.Page = value;
            }
        }

        public virtual string SkinName
        {
            get
            {
                return this.skinName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower(CultureInfo.InvariantCulture);
                    if (value.EndsWith(".html"))
                    {
                        this.skinName = value;
                    }
                }
            }
        }

        protected virtual string SkinPath
        {
            get
            {
                return "";
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty), Browsable(false), DefaultValue((string) null)]
        public ITemplate SkinTemplate
        {
            get
            {
                return this._skinTemplate;
            }
            set
            {
                this._skinTemplate = value;
                base.ChildControlsCreated = false;
            }
        }
    }
}

