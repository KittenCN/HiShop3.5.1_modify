namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public interface IButton : IText
    {
        event EventHandler Click;

        event CommandEventHandler Command;

        AttributeCollection Attributes { get; }

        bool CausesValidation { get; set; }

        string CommandArgument { get; set; }

        string CommandName { get; set; }
    }
}

