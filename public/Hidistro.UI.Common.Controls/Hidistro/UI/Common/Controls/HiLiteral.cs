namespace Hidistro.UI.Common.Controls
{
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class HiLiteral : Literal, IText
    {
        public System.Web.UI.Control Control
        {
            get
            {
                return this;
            }
        }
    }
}

