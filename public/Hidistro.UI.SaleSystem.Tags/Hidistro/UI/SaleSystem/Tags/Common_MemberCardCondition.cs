namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_MemberCardCondition : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            StringBuilder builder = new StringBuilder();
            if (masterSettings.VipRequireName)
            {
                builder.Append("<input id=\"txtName\" type=\"text\" class=\"mod_input\" placeholder=\"您的姓名\">");
            }
            if (masterSettings.VipRequireMobile)
            {
                builder.Append("<input id=\"txtPhone\" type=\"tel\" class=\"mod_input\" placeholder=\"您的联系电话\">");
            }
            if (masterSettings.VipRequireQQ)
            {
                builder.Append("<input id=\"txtQQ\" type=\"tel\" class=\"mod_input\" placeholder=\"您的QQ号码\">");
            }
            if (masterSettings.VipRequireAdress)
            {
                builder.Append("<input id=\"txtAddress\" type=\"text\" class=\"mod_input\" placeholder=\"您的联系地址\">");
            }
            writer.Write(builder.ToString());
        }
    }
}

