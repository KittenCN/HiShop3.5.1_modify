namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public sealed class ButtonManager
    {
        private ButtonManager()
        {
        }

        public static IButton Create(Control cntrl)
        {
            if (cntrl == null)
            {
                return null;
            }
            IButton button = cntrl as IButton;
            if (button == null)
            {
                if (cntrl is Button)
                {
                    return CreateButton(cntrl as Button);
                }
                if (cntrl is ImageLinkButton)
                {
                    return CreateImageLinkButton(cntrl as ImageLinkButton);
                }
                if (cntrl is LinkButton)
                {
                    button = CreateLinkButton(cntrl as LinkButton);
                }
            }
            return button;
        }

        public static IButton CreateButton(Button button)
        {
            if (button == null)
            {
                throw new ArgumentNullException("button", "The button parameter can not be null");
            }
            return new ButtonWrapper(button);
        }

        public static IButton CreateImageLinkButton(ImageLinkButton button)
        {
            if (button == null)
            {
                throw new ArgumentNullException("button", "The button parameter can not be null");
            }
            return new ImageLinkButtonWrapper(button);
        }

        public static IButton CreateLinkButton(LinkButton button)
        {
            if (button == null)
            {
                throw new ArgumentNullException("button", "The button parameter can not be null");
            }
            return new LinkButtonWrapper(button);
        }
    }
}

