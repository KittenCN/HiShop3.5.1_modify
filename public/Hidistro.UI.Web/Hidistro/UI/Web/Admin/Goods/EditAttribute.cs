namespace Hidistro.UI.Web.Admin.Goods
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Goods.ascx;
    using System;

    [PrivilegeCheck(Privilege.EditProductType)]
    public class EditAttribute : AdminPage
    {
        protected AttributeView attributeView;

        protected EditAttribute() : base("m02", "spp07")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

