namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public static class ControlHelper
    {
        public static void SetWhenIsNotNull(this Control control, string value)
        {
            if (control != null)
            {
                if (control is ITextControl)
                {
                    ITextControl control2 = (ITextControl) control;
                    control2.Text = value;
                }
                else if (control is HtmlInputControl)
                {
                    HtmlInputControl control3 = (HtmlInputControl) control;
                    control3.Value = value;
                }
                else
                {
                    if (!(control is HyperLink))
                    {
                        throw new ApplicationException("未实现" + control.GetType().ToString() + "的SetWhenIsNotNull方法");
                    }
                    HyperLink link = (HyperLink) control;
                    link.NavigateUrl = value;
                }
            }
        }
    }
}

