﻿using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Goods.ascx;
using System;


namespace Hidistro.UI.Web.Admin.Goods
{
    [PrivilegeCheck(Privilege.EditProductType)]
    public partial class EditAttribute : AdminPage
    {
        protected EditAttribute() : base("m02", "spp07")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}