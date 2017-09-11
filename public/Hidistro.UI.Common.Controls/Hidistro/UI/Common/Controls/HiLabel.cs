namespace Hidistro.UI.Common.Controls
{
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class HiLabel : Label, IText
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

