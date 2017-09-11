namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Weixin.MP.Api;
    using Hishop.Weixin.MP.Domain;
    using Hishop.Weixin.MP.Domain.Menu;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class ManageMenu : AdminPage
    {
        protected Button BtnSave;

        protected ManageMenu() : base("m06", "wxp04")
        {
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            IList<MenuInfo> initMenus = VShopHelper.GetInitMenus();
            Hishop.Weixin.MP.Domain.Menu.Menu menu = new Hishop.Weixin.MP.Domain.Menu.Menu();
            foreach (MenuInfo info in initMenus)
            {
                if ((info.Chilren == null) || (info.Chilren.Count == 0))
                {
                    menu.menu.button.Add(this.BuildMenu(info));
                }
                else
                {
                    SubMenu item = new SubMenu {
                        name = info.Name
                    };
                    foreach (MenuInfo info2 in info.Chilren)
                    {
                        item.sub_button.Add(this.BuildMenu(info2));
                    }
                    menu.menu.button.Add(item);
                }
            }
            string json = JsonConvert.SerializeObject(menu.menu);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (string.IsNullOrEmpty(masterSettings.WeixinAppId) || string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
            {
                base.Response.Write("<script>alert('您的服务号配置存在问题，请您先检查配置！');location.href='wxconfig.aspx'</script>");
            }
            else if (MenuApi.CreateMenus(JsonConvert.DeserializeObject<Token>(TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret)).access_token, json).Contains("\"ok\""))
            {
                this.ShowMsg("自定义菜单已同步到微信，24小时内生效！", true);
            }
            else
            {
                this.ShowMsg("操作失败!服务号配置信息错误或没有微信自定义菜单权限", false);
            }
        }

        private SingleButton BuildMenu(MenuInfo menu)
        {
            switch (menu.BindType)
            {
                case BindType.Key:
                    return new SingleClickButton { name = menu.Name, key = menu.MenuId.ToString() };

                case BindType.Topic:
                case BindType.HomePage:
                case BindType.ProductCategory:
                case BindType.ShoppingCar:
                case BindType.OrderCenter:
                case BindType.MemberCard:
                case BindType.Url:
                    return new SingleViewButton { name = menu.Name, url = menu.Url };

                case BindType.StoreCard:
                    return new SingleClickButton { name = menu.Name, key = menu.MenuId.ToString() };
            }
            return new SingleClickButton { name = menu.Name, key = "None" };
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.BtnSave.Click += new EventHandler(this.BtnSave_Click);
        }
    }
}

