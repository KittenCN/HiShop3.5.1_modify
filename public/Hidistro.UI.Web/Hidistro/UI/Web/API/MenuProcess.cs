namespace Hidistro.UI.Web.API
{
   using  global:: ControlPanel.Settings;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Settings;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.SessionState;

    public class MenuProcess : IHttpHandler, IRequiresSessionState
    {
        public void AddMenus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{\"status\":\"1\"}";
            MenuInfo menu = new MenuInfo {
                Content = context.Request["Content"].Trim(),
                Name = context.Request["Name"].Trim()
            };
            if (context.Request["ParentMenuId"] != null)
            {
                menu.ParentMenuId = (context.Request["ParentMenuId"] == "") ? 0 : int.Parse(context.Request["ParentMenuId"]);
            }
            else
            {
                menu.ParentMenuId = 0;
            }
            menu.Type = context.Request["Type"];
            menu.ShopMenuPic = context.Request["ShopMenuPic"];
            if (MenuHelper.CanAddMenu(menu.ParentMenuId))
            {
                int num = MenuHelper.SaveMenu(menu);
                if (num > 0)
                {
                    s = "{\"status\":\"0\",\"menuid\":\"" + num + "\"}";
                }
            }
            else
            {
                s = "{\"status\":\"2\"}";
            }
            context.Response.Write(s);
        }

        public void delmenu(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{\"status\":\"1\"}";
            int result = 0;
            if (!int.TryParse(context.Request["MenuId"], out result))
            {
                s = "{\"status\":\"1\"}";
            }
            else
            {
                if (MenuHelper.DeleteMenu(result))
                {
                    s = "{\"status\":\"0\"}";
                }
                context.Response.Write(s);
            }
        }

        public void EditMenus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{\"status\":\"1\"}";
            MenuInfo menu = new MenuInfo {
                Content = context.Request["Content"],
                Name = context.Request["Name"],
                Type = context.Request["Type"]
            };
            if (!string.IsNullOrEmpty(context.Request["ParentMenuId"]))
            {
                menu.ParentMenuId = int.Parse(context.Request["ParentMenuId"]);
            }
            else
            {
                menu.ParentMenuId = 0;
            }
            int result = 0;
            if (!int.TryParse(context.Request["MenuId"], out result))
            {
                s = "{\"status\":\"1\"}";
            }
            else
            {
                menu.MenuId = result;
                menu.ShopMenuPic = context.Request["ShopMenuPic"];
                if (MenuHelper.UpdateMenu(menu))
                {
                    s = "{\"status\":\"0\"}";
                }
                context.Response.Write(s);
            }
        }

        public void GetMenu(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{";
            MenuInfo menu = new MenuInfo();
            int result = 0;
            if (!int.TryParse(context.Request["MenuId"], out result))
            {
                s = "\"status\":\"1\"";
            }
            else
            {
                menu = MenuHelper.GetMenu(result);
                if (menu != null)
                {
                    object obj2 = s + "\"status\":\"0\",\"data\":[";
                    s = ((((string.Concat(new object[] { obj2, "{\"menuid\": \"", menu.MenuId, "\"," }) + "\"type\": \"" + menu.Type + "\",") + "\"name\": \"" + Globals.String2Json(menu.Name) + "\",") + "\"shopmenupic\": \"" + menu.ShopMenuPic + "\",") + "\"content\": \"" + Globals.String2Json(menu.Content) + "\"}") + "]";
                }
                s = s + "}";
                context.Response.Write(s);
            }
        }

        public void GetTopMenus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{";
            IList<MenuInfo> topMenus = MenuHelper.GetTopMenus();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (topMenus.Count <= 0)
            {
                s = s + "\"status\":\"-1\"";
            }
            else
            {
                object obj2 = s;
                s = string.Concat(new object[] { obj2, "\"status\":\"0\",\"shopmenustyle\":\"", masterSettings.ShopMenuStyle, "\",\"enableshopmenu\":\"", masterSettings.EnableShopMenu, "\",\"data\":[" });
                foreach (MenuInfo info in topMenus)
                {
                    IList<MenuInfo> menusByParentId = MenuHelper.GetMenusByParentId(info.MenuId);
                    object obj3 = s;
                    s = string.Concat(new object[] { obj3, "{\"menuid\": \"", info.MenuId, "\"," });
                    s = s + "\"childdata\":[";
                    if (menusByParentId.Count > 0)
                    {
                        foreach (MenuInfo info2 in menusByParentId)
                        {
                            object obj4 = s;
                            s = string.Concat(new object[] { obj4, "{\"menuid\": \"", info2.MenuId, "\"," });
                            object obj5 = s;
                            s = string.Concat(new object[] { obj5, "\"parentmenuid\": \"", info2.ParentMenuId, "\"," });
                            s = s + "\"type\": \"" + info2.Type + "\",";
                            s = s + "\"name\": \"" + Globals.String2Json(info2.Name) + "\",";
                            s = s + "\"content\": \"" + Globals.String2Json(info2.Content) + "\"},";
                        }
                        s = s.Substring(0, s.Length - 1);
                    }
                    s = s + "],";
                    s = s + "\"type\": \"" + info.Type + "\",";
                    s = s + "\"name\": \"" + Globals.String2Json(info.Name) + "\",";
                    s = s + "\"shopmenupic\": \"" + info.ShopMenuPic + "\",";
                    s = s + "\"content\": \"" + Globals.String2Json(info.Content) + "\"},";
                }
                s = s.Substring(0, s.Length - 1) + "]" + "}";
                context.Response.Write(s);
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request["action"])
            {
                case "gettopmenus":
                    this.GetTopMenus(context);
                    return;

                case "addmenu":
                    this.AddMenus(context);
                    return;

                case "editmenu":
                    this.EditMenus(context);
                    return;

                case "updatename":
                    this.updatename(context);
                    return;

                case "getmenu":
                    this.GetMenu(context);
                    return;

                case "delmenu":
                    this.delmenu(context);
                    return;

                case "setenable":
                    this.setenable(context);
                    return;
            }
        }

        public void setenable(HttpContext context)
        {
            string s = "{\"status\":\"1\"}";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.EnableShopMenu = bool.Parse(context.Request["enable"]);
            SettingsManager.Save(masterSettings);
            context.Response.Write(s);
        }

        public void updatename(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{\"status\":\"1\"}";
            int result = 0;
            if (!int.TryParse(context.Request["MenuId"], out result))
            {
                s = "{\"status\":\"1\"}";
            }
            else
            {
                if (result > 0)
                {
                    MenuInfo menu = MenuHelper.GetMenu(result);
                    menu.Name = context.Request["Name"];
                    menu.MenuId = result;
                    if (MenuHelper.UpdateMenu(menu))
                    {
                        s = "{\"status\":\"0\"}";
                    }
                }
                context.Response.Write(s);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

