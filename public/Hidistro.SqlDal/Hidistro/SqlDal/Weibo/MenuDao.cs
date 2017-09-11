namespace Hidistro.SqlDal.Weibo
{
    using Hidistro.Entities;
    using Hidistro.Entities.Weibo;
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE Weibo_Menu WHERE MenuId = @MenuId");
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private int GetAllMenusCount()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from Weibo_Menu");
            return (1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)));
        }

        public MenuInfo GetMenu(int menuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Menu WHERE MenuId = @MenuId");
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menuId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MenuInfo>(reader);
            }
        }

        public IList<MenuInfo> GetMenusByParentId(int parentId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Menu WHERE ParentMenuId = @ParentMenuId ");
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, parentId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MenuInfo>(reader);
            }
        }

        public IList<MenuInfo> GetTopMenus()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Menu WHERE ParentMenuId = 0");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MenuInfo>(reader);
            }
        }

        public bool SaveMenu(MenuInfo menu)
        {
            int allMenusCount = this.GetAllMenusCount();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Weibo_Menu (ParentMenuId, Name, Type,  DisplaySequence,  [Content]) VALUES(@ParentMenuId, @Name, @Type, @DisplaySequence, @Content)");
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, menu.ParentMenuId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, menu.Name);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, "view");
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, allMenusCount);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, menu.Content);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool UpdateMenu(MenuInfo menu)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Weibo_Menu SET ParentMenuId = @ParentMenuId, Name = @Name, Type = @Type,DisplaySequence = @DisplaySequence,  [Content] = @Content WHERE MenuId = @MenuId");
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, menu.ParentMenuId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, menu.Name);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, "view");
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, menu.DisplaySequence);
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menu.MenuId);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, menu.Content);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }
    }
}

