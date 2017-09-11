namespace ControlPanel.Settings
{
    using Hidistro.Entities.Settings;
    using Hidistro.SqlDal.Settings;
    using System;
    using System.Collections.Generic;

    public static class MenuHelper
    {
        public static bool CanAddMenu(int parentId)
        {
            IList<MenuInfo> menusByParentId = new MenuDao().GetMenusByParentId(parentId);
            if ((menusByParentId == null) || (menusByParentId.Count == 0))
            {
                return true;
            }
            if (parentId == 0)
            {
                return (menusByParentId.Count < 5);
            }
            return (menusByParentId.Count < 5);
        }

        public static bool DeleteMenu(int menuId)
        {
            return new MenuDao().DeleteMenu(menuId);
        }

        public static MenuInfo GetMenu(int menuId)
        {
            return new MenuDao().GetMenu(menuId);
        }

        public static IList<MenuInfo> GetMenus(string shopMenuStyle)
        {
            IList<MenuInfo> list = new List<MenuInfo>();
            MenuDao dao = new MenuDao();
            IList<MenuInfo> topMenus = dao.GetTopMenus();
            if (topMenus != null)
            {
                foreach (MenuInfo info in topMenus)
                {
                    IList<MenuInfo> menusByParentId = dao.GetMenusByParentId(info.MenuId);
                    if (shopMenuStyle != "1")
                    {
                        info.ShopMenuPic = "";
                    }
                    info.SubMenus = menusByParentId;
                    list.Add(info);
                }
            }
            return list;
        }

        public static IList<MenuInfo> GetMenusByParentId(int parentId)
        {
            return new MenuDao().GetMenusByParentId(parentId);
        }

        public static IList<MenuInfo> GetTopMenus()
        {
            return new MenuDao().GetTopMenus();
        }

        public static int SaveMenu(MenuInfo menu)
        {
            return new MenuDao().SaveMenu(menu);
        }

        public static bool UpdateMenu(MenuInfo menu)
        {
            return new MenuDao().UpdateMenu(menu);
        }

        public static bool UpdateMenuName(MenuInfo menu)
        {
            return new MenuDao().UpdateMenuName(menu);
        }
    }
}

