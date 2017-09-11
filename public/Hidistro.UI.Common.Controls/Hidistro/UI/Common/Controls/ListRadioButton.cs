namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ToolboxData("<{0}:ListRadioButton runat=server></{0}:ListRadioButton>")]
    public class ListRadioButton : RadioButton, IPostBackDataHandler
    {
        protected override void Render(HtmlTextWriter output)
        {
            this.RenderInputTag(output);
        }

        private void RenderInputTag(HtmlTextWriter htw)
        {
            htw.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
            htw.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
            htw.AddAttribute(HtmlTextWriterAttribute.Name, this.GroupName);
            htw.AddAttribute(HtmlTextWriterAttribute.Value, this.Value);
            if (this.Checked)
            {
                htw.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
            }
            if (!this.Enabled)
            {
                htw.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
            }
            string str = base.Attributes["onclick"];
            if (this.AutoPostBack)
            {
                if (str != null)
                {
                    str = string.Empty;
                }
                str = str + this.Page.ClientScript.GetPostBackEventReference(this, string.Empty);
                htw.AddAttribute(HtmlTextWriterAttribute.Onclick, str);
                htw.AddAttribute("language", "javascript");
            }
            else if (str != null)
            {
                htw.AddAttribute(HtmlTextWriterAttribute.Onclick, str);
            }
            if (this.AccessKey.Length > 0)
            {
                htw.AddAttribute(HtmlTextWriterAttribute.Accesskey, this.AccessKey);
            }
            if (this.TabIndex != 0)
            {
                htw.AddAttribute(HtmlTextWriterAttribute.Tabindex, this.TabIndex.ToString(NumberFormatInfo.InvariantInfo));
            }
            htw.RenderBeginTag(HtmlTextWriterTag.Input);
            htw.RenderEndTag();
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            bool flag = false;
            string str = postCollection[this.GroupName];
            if ((str != null) && (str == this.Value))
            {
                if (!this.Checked)
                {
                    this.Checked = true;
                    flag = true;
                }
                return flag;
            }
            if (this.Checked)
            {
                this.Checked = false;
            }
            return flag;
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            this.OnCheckedChanged(EventArgs.Empty);
        }

        private string Value
        {
            get
            {
                string uniqueID = base.Attributes["value"];
                if (uniqueID == null)
                {
                    uniqueID = this.UniqueID;
                }
                return uniqueID;
            }
        }
    }
}

