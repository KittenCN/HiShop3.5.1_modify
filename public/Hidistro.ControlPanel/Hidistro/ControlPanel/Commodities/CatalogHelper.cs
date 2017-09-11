namespace Hidistro.ControlPanel.Commodities
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Web.Caching;

    public sealed class CatalogHelper
    {
        private const string CategoriesCachekey = "DataCache-Categories";

        private CatalogHelper()
        {
        }

        public static bool AddBrandCategory(BrandCategoryInfo brandCategory)
        {
            int brandId = new BrandCategoryDao().AddBrandCategory(brandCategory);
            if (brandId <= 0)
            {
                return false;
            }
            if (brandCategory.ProductTypes.Count > 0)
            {
                new BrandCategoryDao().AddBrandProductTypes(brandId, brandCategory.ProductTypes);
            }
            return true;
        }

        public static CategoryActionStatus AddCategory(CategoryInfo category)
        {
            if (category == null)
            {
                return CategoryActionStatus.UnknowError;
            }
            Globals.EntityCoding(category, true);
            if (new CategoryDao().CreateCategory(category) > 0)
            {
                EventLogs.WriteOperationLog(Privilege.AddProductCategory, string.Format(CultureInfo.InvariantCulture, "创建了一个新的店铺分类:”{0}”", new object[] { category.Name }));
                HiCache.Remove("DataCache-Categories");
            }
            return CategoryActionStatus.Success;
        }

        public static bool AddProductTags(int productId, IList<int> tagsId, DbTransaction dbtran)
        {
            return new TagDao().AddProductTags(productId, tagsId, dbtran);
        }

        public static int AddTags(string tagName)
        {
            int num = 0;
            if (new TagDao().GetTags(tagName) <= 0)
            {
                num = new TagDao().AddTags(tagName);
            }
            return num;
        }


        public static int AddInsuranceCompany(string insuranceCompanyName)
        {
            int num = 0;
            if (new TagDao().GetInsuranceCompany(insuranceCompanyName) <= 0)
            {
                num = new TagDao().AddInsuranceCompany(insuranceCompanyName);
            }
            return num;
        }


        public static int AddInsuranceCompanyArea(string insuranceAreaCiteId, string insuranceAreaCiteName, string insuranceAreaProvinceId, string insuranceAreaName, string insuranceCompanyTypes,string insuranceCompanyTypesIds)
        {
            int num = 0;
            if (new TagDao().GetInsuranceCompanyArea(insuranceAreaCiteId) <= 0)
            {
                num = new TagDao().AddInsuranceCompanyArea(insuranceAreaCiteId, insuranceAreaCiteName, insuranceAreaProvinceId, insuranceAreaName, insuranceCompanyTypes, insuranceCompanyTypesIds);
            }
            return num;
        }

        public static bool BrandHvaeProducts(int brandId)
        {
            return new BrandCategoryDao().BrandHvaeProducts(brandId);
        }

        public static bool DeleteBrandCategory(int brandId)
        {
            return new BrandCategoryDao().DeleteBrandCategory(brandId);
        }

        public static bool DeleteCategory(int categoryId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductCategory);
            bool flag = new CategoryDao().DeleteCategory(categoryId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductCategory, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的店铺分类", new object[] { categoryId }));
                HiCache.Remove("DataCache-Categories");
            }
            return flag;
        }

        public static bool DeleteProductTags(int productId, DbTransaction tran)
        {
            return new TagDao().DeleteProductTags(productId, tran);
        }

        public static bool DeleteTags(int tagId)
        {
            return new TagDao().DeleteTags(tagId);
        }

        public static bool DeleteInsuranceCompany(int insuranceCompanyId)
        {
            return new TagDao().DeleteInsuranceCompany(insuranceCompanyId);
        }

        public static bool DeleteInsuranceArea(int insuranceAreaId)
        {
            return new TagDao().DeleteInsuranceArea(insuranceAreaId);
        }

        public static int DisplaceCategory(int oldCategoryId, int newCategory)
        {
            return new CategoryDao().DisplaceCategory(oldCategoryId, newCategory);
        }

        public static DataTable GetBrandCategories()
        {
            return new BrandCategoryDao().GetBrandCategories();
        }

        public static DataTable GetBrandCategories(string brandName)
        {
            return new BrandCategoryDao().GetBrandCategories(brandName);
        }

        public static BrandCategoryInfo GetBrandCategory(int brandId)
        {
            return new BrandCategoryDao().GetBrandCategory(brandId);
        }

        public static DbQueryResult GetBrandQuery(BrandQuery query)
        {
            return new BrandCategoryDao().Query(query);
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

        public static string GetFullCategory(int categoryId)
        {
            CategoryInfo category = GetCategory(categoryId);
            if (category == null)
            {
                return null;
            }
            string name = category.Name;
            while ((category != null) && category.ParentCategoryId.HasValue)
            {
                category = GetCategory(category.ParentCategoryId.Value);
                if (category != null)
                {
                    name = category.Name + " &raquo; " + name;
                }
            }
            return name;
        }

        public static IList<CategoryInfo> GetMainCategories()
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("Depth = 1");
            for (int i = 0; i < rowArray.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static IList<CategoryInfo> GetSequenceCategories()
        {
            IList<CategoryInfo> categories = new List<CategoryInfo>();
            foreach (CategoryInfo info in GetMainCategories())
            {
                categories.Add(info);
                LoadSubCategorys(info.CategoryId, categories);
            }
            return categories;
        }

        public static IList<CategoryInfo> GetSubCategories(int parentCategoryId)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            string filterExpression = "ParentCategoryId = " + parentCategoryId.ToString(CultureInfo.InvariantCulture);
            DataRow[] rowArray = GetCategories().Select(filterExpression);
            for (int i = 0; i < rowArray.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static string GetTagName(int tagId)
        {
            return new TagDao().GetTagName(tagId);
        }

        public static DataTable GetTags()
        {
            return new TagDao().GetTags();
        }

        public static DataTable GetInsuranceCompany()
        {
            return new TagDao().GetInsuranceCompany();
        }

        public static DataTable GetInsuranceArea()
        {
            return new TagDao().GetInsuranceArea();
        }

        public static DataTable GetInsuranceAreaGroupProvinceId()
        {
            return new TagDao().GetInsuranceAreaGroupProvinceId();
        }

        public static DataTable GetInsuranceCompanyByCityId(string city1,string city2)
        {
            return new TagDao().GetInsuranceCompanyByCityId(city1, city2);
        }
        
        public static DataTable GetInsuranceAreaByProvinceId(int pid)
        {
            return new TagDao().GetInsuranceAreaByProvinceId(pid);
        }


        public static bool IsExitProduct(string CategoryId)
        {
            return new CategoryDao().IsExitProduct(CategoryId);
        }

        private static void LoadSubCategorys(int parentCategoryId, IList<CategoryInfo> categories)
        {
            IList<CategoryInfo> subCategories = GetSubCategories(parentCategoryId);
            if ((subCategories != null) && (subCategories.Count > 0))
            {
                foreach (CategoryInfo info in subCategories)
                {
                    categories.Add(info);
                    LoadSubCategorys(info.CategoryId, categories);
                }
            }
        }

        public static DbQueryResult Query(CategoriesQuery query)
        {
            return new CategoryDao().Query(query);
        }

        public static bool SetBrandCategoryThemes(int brandid, string themeName)
        {
            bool flag = new BrandCategoryDao().SetBrandCategoryThemes(brandid, themeName);
            if (flag)
            {
                HiCache.Remove("DataCache-Categories");
            }
            return flag;
        }

        public static bool SetCategoryThemes(int categoryId, string themeName)
        {
            if (new CategoryDao().SetCategoryThemes(categoryId, themeName))
            {
                HiCache.Remove("DataCache-Categories");
            }
            return false;
        }

        public static bool SetProductExtendCategory(int productId, string extendCategoryPath)
        {
            return new CategoryDao().SetProductExtendCategory(productId, extendCategoryPath);
        }

        public static bool SwapCategorySequence(int categoryId, int displaysequence)
        {
            return new CategoryDao().SwapCategorySequence(categoryId, displaysequence);
        }

        public static void UpdateBrandCategorieDisplaySequence(int brandId, SortAction action)
        {
            new BrandCategoryDao().UpdateBrandCategoryDisplaySequence(brandId, action);
        }

        public static bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
        {
            bool flag = new BrandCategoryDao().UpdateBrandCategory(brandCategory);
            if (flag && new BrandCategoryDao().DeleteBrandProductTypes(brandCategory.BrandId))
            {
                new BrandCategoryDao().AddBrandProductTypes(brandCategory.BrandId, brandCategory.ProductTypes);
            }
            return flag;
        }

        public static bool UpdateBrandCategoryDisplaySequence(int barndId, int displaysequence)
        {
            return new BrandCategoryDao().UpdateBrandCategoryDisplaySequence(barndId, displaysequence);
        }

        public static CategoryActionStatus UpdateCategory(CategoryInfo category)
        {
            if (category == null)
            {
                return CategoryActionStatus.UnknowError;
            }
            Globals.EntityCoding(category, true);
            CategoryActionStatus status = new CategoryDao().UpdateCategory(category);
            if (status == CategoryActionStatus.Success)
            {
                EventLogs.WriteOperationLog(Privilege.EditProductCategory, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的店铺分类", new object[] { category.CategoryId }));
                HiCache.Remove("DataCache-Categories");
            }
            return status;
        }

        public static bool UpdateTags(int tagId, string tagName)
        {
            bool flag = false;
            int tags = new TagDao().GetTags(tagName);
            if ((tags != tagId) && (tags > 0))
            {
                return flag;
            }
            return new TagDao().UpdateTags(tagId, tagName);
        }

        public static bool UpdateInsuranceCompany(int insuranceCompanyId, string insuranceCompanyName)
        {
            bool flag = false;
            int tags = new TagDao().GetInsuranceCompany(insuranceCompanyName);
            if ((tags != insuranceCompanyId) && (tags > 0))
            {
                return flag;
            }
            return new TagDao().UpdateInsuranceCompany(insuranceCompanyId, insuranceCompanyName);
        }


        public static bool UpdateInsuranceArea(int InsuranceAreaId, string insuranceAreaCiteId, string insuranceAreaCiteName, string insuranceAreaProvinceId, string insuranceAreaName, string insuranceCompanyTypes,string insuranceCompanyTypesIds)
        {
            bool flag = false;
            int tags = new TagDao().GetInsuranceArea(insuranceAreaCiteId);
            if ((tags != InsuranceAreaId) && (tags > 0))
            {
                return flag;
            }
            return new TagDao().UpdateInsuranceArea(InsuranceAreaId, insuranceAreaCiteId, insuranceAreaCiteName, insuranceAreaProvinceId, insuranceAreaName, insuranceCompanyTypes, insuranceCompanyTypesIds);
        }
    }
}

