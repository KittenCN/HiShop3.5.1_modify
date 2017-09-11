namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public sealed class TextControlHelper
    {
        private TextControlHelper()
        {
        }

        public static IText Create(Control cntrl)
        {
            if (cntrl == null)
            {
                return null;
            }
            IText text = cntrl as IText;
            if (text == null)
            {
                if (cntrl is Literal)
                {
                    return new LiteralWrapper(cntrl as Literal);
                }
                if (cntrl is Label)
                {
                    text = new LabelWrapper(cntrl as Label);
                }
            }
            return text;
        }

        public static IText CreateLabel(Label label)
        {
            return new LabelWrapper(label);
        }

        public static IText CreateLiteral(Literal lit)
        {
            return new LiteralWrapper(lit);
        }
    }
}

