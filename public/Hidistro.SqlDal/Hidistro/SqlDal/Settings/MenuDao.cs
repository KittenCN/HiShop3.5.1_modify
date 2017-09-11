namespace Hidistro.SqlDal.Settings
{
    using Hidistro.Entities;
    using Hidistro.Entities.Settings;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class MenuDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool DeleteMenu(int menuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE VShop_NavMenu WHERE (MenuId = @MenuId or ParentMenuId=@ParentMenuId)");
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menuId);
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, menuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private int GetAllMenusCount()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from VShop_NavMenu");
            return (1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)));
        }

        public MenuInfo GetMenu(int menuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM VShop_NavMenu WHERE MenuId = @MenuId");
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menuId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MenuInfo>(reader);
            }
        }

        public IList<MenuInfo> GetMenusByParentId(int parentId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM VShop_NavMenu WHERE ParentMenuId = @ParentMenuId");
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, parentId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MenuInfo>(reader);
            }
        }

        public IList<MenuInfo> GetTopMenus()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM VShop_NavMenu WHERE ParentMenuId = 0");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MenuInfo>(reader);
            }
        }

        public int SaveMenu(MenuInfo menu)
        {
            int allMenusCount = this.GetAllMenusCount();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO VShop_NavMenu (ParentMenuId, Name, Type,  DisplaySequence,  [Content],ShopMenuPic) VALUES(@ParentMenuId, @Name, @Type, @DisplaySequence, @Content,@ShopMenuPic);select @@IDENTITY ;");
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, menu.ParentMenuId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, menu.Name);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, menu.Type);
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, allMenusCount);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, menu.Content);
            this.database.AddInParameter(sqlStringCommand, "ShopMenuPic", DbType.String, menu.ShopMenuPic);
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }

        public bool UpdateMenu(MenuInfo menu)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE VShop_NavMenu SET ParentMenuId = @ParentMenuId, Name = @Name, Type = @Type,DisplaySequence = @DisplaySequence,  [Content] = @Content,ShopMenuPic=@ShopMenuPic WHERE MenuId = @MenuId");
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, menu.ParentMenuId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, menu.Name);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, menu.Type);
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, menu.DisplaySequence);
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menu.MenuId);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, menu.Content);
            this.database.AddInParameter(sqlStringCommand, "ShopMenuPic", DbType.String, menu.ShopMenuPic);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool UpdateMenuName(MenuInfo menu)
        {
            this.GetAllMenusCount();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update VShop_NavMenu  set Name=@Name  where MenuId=@MenuId");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, menu.Name);
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menu.MenuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }
    }
}

