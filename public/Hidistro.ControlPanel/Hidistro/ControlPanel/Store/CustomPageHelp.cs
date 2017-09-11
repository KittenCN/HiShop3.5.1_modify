namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Store;
    using System;

    public static class CustomPageHelp
    {
        public static int Create(CustomPage page)
        {
            return new CustomPageDao().Create(page);
        }

        public static bool DeletePage(int Id)
        {
            return new CustomPageDao().DeletePage(Id);
        }

        public static CustomPage GetCustomDraftPageByPath(string path)
        {
            return new CustomPageDao().GetCustomDraftPageByPath(path);
        }

        public static CustomPage GetCustomPageByID(int id)
        {
            return new CustomPageDao().GetCustomPageByID(id);
        }

        public static CustomPage GetCustomPageByPath(string path)
        {
            return new CustomPageDao().GetCustomPageByPath(path);
        }

        public static DbQueryResult GetPages(CustomPageQuery query)
        {
            return new CustomPageDao().GetPages(query);
        }

        public static bool Update(CustomPage page)
        {
            return new CustomPageDao().Update(page);
        }
    }
}

