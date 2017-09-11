namespace Hidistro.UI.Web.Admin.AliFuwu
{
    using Aop.Api.Response;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.AlipayFuwu.Api.Model;
    using Hishop.AlipayFuwu.Api.Util;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class ManageAlipayMenu : AdminPage
    {
        protected Button BtnSave;

        protected ManageAlipayMenu() : base("m11", "fwp02")
        {
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            IList<MenuInfo> initFuwuMenus = VShopHelper.GetInitFuwuMenus();
            FWMenu menu = new FWMenu {
                button = new List<FWButton>()
            };
            List<FWButton> list2 = menu.button as List<FWButton>;
            foreach (MenuInfo info in initFuwuMenus)
            {
                FWButton item = this.BuildMenu(info);
                if ((info.Chilren != null) && (info.Chilren.Count > 0))
                {
                    item.subButton = new List<FWButton>();
                    foreach (MenuInfo info2 in info.Chilren)
                    {
                        (item.subButton as List<FWButton>).Add(this.BuildMenu(info2));
                    }
                }
                list2.Add(item);
            }
            if (!AlipayFuwuConfig.CommSetConfig(SettingsManager.GetMasterSettings(true).AlipayAppid, base.Server.MapPath("~/"), "GBK"))
            {
                base.Response.Write("<script>alert('您的服务窗配置信息错误，请您先检查配置！');location.href='AliFuwuConfig.aspx'</script>");
            }
            else
            {
                AlipayMobilePublicMenuUpdateResponse response = AliOHHelper.MenuUpdate(menu);
                if (((response != null) && !response.IsError) && (response.Code == "200"))
                {
                    this.ShowMsg("自定义菜单已同步到支付宝服务窗！", true);
                }
                else
                {
                    this.ShowMsg("操作失败!" + response.Msg, false);
                }
            }
        }

        private FWButton BuildMenu(MenuInfo menu)
        {
            switch (menu.BindType)
            {
                case BindType.Key:
                    return new FWButton { name = menu.Name, actionParam = menu.MenuId.ToString(), actionType = "out" };

                case BindType.Topic:
                case BindType.HomePage:
                case BindType.ProductCategory:
                case BindType.ShoppingCar:
                case BindType.OrderCenter:
                case BindType.MemberCard:
                    return new FWButton { name = menu.Name, actionParam = menu.Url, actionType = "link" };

                case BindType.Url:
                    return new FWButton { name = menu.Name, actionParam = menu.Content, actionType = "link" };
            }
            return new FWButton { name = menu.Name };
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AlipayFuwuConfig.appId.Length < 0x10)
            {
                this.ShowMsgAndReUrl("请先绑定服务窗", false, "AliFuwuConfig.aspx");
            }
            else
            {
                this.BtnSave.Click += new EventHandler(this.BtnSave_Click);
            }
        }
    }
}

