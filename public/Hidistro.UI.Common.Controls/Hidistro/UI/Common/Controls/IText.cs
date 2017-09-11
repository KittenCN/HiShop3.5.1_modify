namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;

    public interface IText
    {
        System.Web.UI.Control Control { get; }

        string Text { get; set; }

        bool Visible { get; set; }
    }
}

