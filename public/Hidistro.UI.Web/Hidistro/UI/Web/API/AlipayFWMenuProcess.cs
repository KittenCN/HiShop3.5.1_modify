namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.SessionState;

    public class AlipayFWMenuProcess : IHttpHandler, IRequiresSessionState
    {
        public void AddMenus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{\"status\":\"1\"}";
            MenuInfo menu = new MenuInfo {
                Content = context.Request["Content"].Trim(),
                Name = context.Request["Name"].Trim(),
                Bind = 8
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
            if (VShopHelper.CanAddFuwuMenu(menu.ParentMenuId))
            {
                if (VShopHelper.SaveFuwuMenu(menu))
                {
                    if (menu.ParentMenuId > 0)
                    {
                        MenuInfo fuwuMenu = VShopHelper.GetFuwuMenu(menu.ParentMenuId);
                        if (fuwuMenu != null)
                        {
                            fuwuMenu.Bind = 0;
                            fuwuMenu.Content = "";
                            VShopHelper.UpdateFuwuMenu(fuwuMenu);
                        }
                    }
                    s = "{\"status\":\"0\"}";
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
                if (VShopHelper.DeleteFuwuMenu(result))
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
                Type = context.Request["Type"],
                Bind = 8
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
                if (VShopHelper.UpdateFuwuMenu(menu))
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
            int result = 0;
            if (!int.TryParse(context.Request["MenuId"], out result))
            {
                s = "\"status\":\"1\"";
            }
            else
            {
                MenuInfo fuwuMenu = VShopHelper.GetFuwuMenu(result);
                if (fuwuMenu != null)
                {
                    object obj2 = s + "\"status\":\"0\",\"data\":[";
                    s = ((((string.Concat(new object[] { obj2, "{\"menuid\": \"", fuwuMenu.MenuId, "\"," }) + "\"type\": \"" + fuwuMenu.Type + "\",") + "\"name\": \"" + fuwuMenu.Name + "\",") + "\"shopmenupic\": \"\",") + "\"content\": \"" + fuwuMenu.Content + "\"}") + "]";
                }
                s = s + "}";
                context.Response.Write(s);
            }
        }

        public void GetTopMenus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{";
            IList<MenuInfo> topFuwuMenus = VShopHelper.GetTopFuwuMenus();
            if (topFuwuMenus.Count <= 0)
            {
                s = s + "\"status\":\"-1\"";
            }
            else
            {
                object obj2 = s;
                s = string.Concat(new object[] { obj2, "\"status\":\"0\",\"shopmenustyle\":\"0\",\"enableshopmenu\":\"", true, "\",\"data\":[" });
                foreach (MenuInfo info in topFuwuMenus)
                {
                    IList<MenuInfo> fuwuMenusByParentId = VShopHelper.GetFuwuMenusByParentId(info.MenuId);
                    object obj3 = s;
                    s = string.Concat(new object[] { obj3, "{\"menuid\": \"", info.MenuId, "\"," });
                    s = s + "\"childdata\":[";
                    if (fuwuMenusByParentId.Count > 0)
                    {
                        foreach (MenuInfo info2 in fuwuMenusByParentId)
                        {
                            object obj4 = s;
                            s = string.Concat(new object[] { obj4, "{\"menuid\": \"", info2.MenuId, "\"," });
                            object obj5 = s;
                            s = string.Concat(new object[] { obj5, "\"parentmenuid\": \"", info2.ParentMenuId, "\"," });
                            s = s + "\"type\": \"" + info2.Type + "\",";
                            s = s + "\"name\": \"" + info2.Name + "\",";
                            s = s + "\"content\": \"" + info2.Content + "\"},";
                        }
                        s = s.Substring(0, s.Length - 1);
                    }
                    s = s + "],";
                    s = s + "\"type\": \"" + info.Type + "\",";
                    s = s + "\"name\": \"" + info.Name + "\",";
                    s = s + "\"shopmenupic\": \"\",";
                    s = s + "\"content\": \"" + info.Content + "\"},";
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
            }
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
                    MenuInfo fuwuMenu = VShopHelper.GetFuwuMenu(result);
                    fuwuMenu.MenuId = result;
                    fuwuMenu.Name = context.Request["Name"];
                    if (VShopHelper.UpdateFuwuMenu(fuwuMenu))
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

