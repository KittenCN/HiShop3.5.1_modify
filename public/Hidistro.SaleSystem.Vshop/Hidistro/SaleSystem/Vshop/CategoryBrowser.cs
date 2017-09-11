namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.SqlDal.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Web.Caching;

    public static class CategoryBrowser
    {
        private const string MainCategoriesCachekey = "DataCache-Categories";

        public static DataTable GetAllCategories()
        {
            return new CategoryDao().GetCategories();
        }

        public static DataTable GetBrandCategories()
        {
            return new BrandCategoryDao().GetBrandCategories();
        }

        public static BrandCategoryInfo GetBrandCategory(int brandId)
        {
            return new BrandCategoryDao().GetBrandCategory(brandId);
        }

        public static DataTable GetCategories()
        {
            DataTable categories = HiCache.Get("DataCache-Categories") as DataTable;
            if (categories == null)
            {
                categories = new CategoryDao().GetCategories();
                HiCache.Insert("DataCache-Categories", categories, 360, CacheItemPriority.Normal);
            }
            return categories;
        }

        public static CategoryInfo GetCategory(int categoryId)
        {
            return new CategoryDao().GetCategory(categoryId);
        }

        public static DataSet GetCategoryList()
        {
            DataSet categoryList = HiCache.Get("DataCache-CategoryList") as DataSet;
            if (categoryList == null)
            {
                categoryList = new CategoryDao().GetCategoryList();
                HiCache.Insert("DataCache-CategoryList", categoryList, 360, CacheItemPriority.Normal);
            }
            return categoryList;
        }

        public static IList<CategoryInfo> GetMaxMainCategories(int maxNum = 0x3e8)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("Depth = 1");
            for (int i = 0; (i < maxNum) && (i < rowArray.Length); i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static IList<CategoryInfo> GetMaxSubCategories(int parentCategoryId, int maxNum = 0x3e8)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("ParentCategoryId = " + parentCategoryId);
            for (int i = 0; (i < maxNum) && (i < rowArray.Length); i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }
    }
}

