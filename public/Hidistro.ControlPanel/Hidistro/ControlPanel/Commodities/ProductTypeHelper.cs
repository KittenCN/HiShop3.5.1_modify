namespace Hidistro.ControlPanel.Commodities
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web;

    public sealed class ProductTypeHelper
    {
        private ProductTypeHelper()
        {
        }

        public static bool AddAttribute(AttributeInfo attribute)
        {
            return new AttributeDao().AddAttribute(attribute);
        }

        public static bool AddAttributeName(AttributeInfo attribute)
        {
            return (new AttributeDao().AddAttributeName(attribute) > 0);
        }

        public static int AddAttributeValue(AttributeValueInfo attributeValue)
        {
            return new AttributeValueDao().AddAttributeValue(attributeValue);
        }

        public static int AddProductType(ProductTypeInfo productType)
        {
            if (productType == null)
            {
                return 0;
            }
            Globals.EntityCoding(productType, true);
            int typeId = new ProductTypeDao().AddProductType(productType);
            if (typeId > 0)
            {
                if (productType.Brands.Count > 0)
                {
                    new ProductTypeDao().AddProductTypeBrands(typeId, productType.Brands);
                }
                EventLogs.WriteOperationLog(Privilege.AddProductType, string.Format(CultureInfo.InvariantCulture, "创建了一个新的商品类型:”{0}”", new object[] { productType.TypeName }));
            }
            return typeId;
        }

        public static bool ClearAttributeValue(int attributeId)
        {
            return new AttributeValueDao().ClearAttributeValue(attributeId);
        }

        public static bool DeleteAttribute(int attriubteId)
        {
            return new AttributeDao().DeleteAttribute(attriubteId);
        }

        public static bool DeleteAttributeValue(int attributeValueId)
        {
            return new AttributeValueDao().DeleteAttributeValue(attributeValueId);
        }

        public static bool DeleteProductType(int typeId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            bool flag = new ProductTypeDao().DeleteProducType(typeId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductType, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的商品类型", new object[] { typeId }));
            }
            return flag;
        }

        public static AttributeInfo GetAttribute(int attributeId)
        {
            return new AttributeDao().GetAttribute(attributeId);
        }

        public static IList<AttributeInfo> GetAttributes(int typeId)
        {
            return new AttributeDao().GetAttributes(typeId);
        }

        public static IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode)
        {
            return new AttributeDao().GetAttributes(typeId, attributeUseageMode);
        }

        public static AttributeValueInfo GetAttributeValueInfo(int valueId)
        {
            return new AttributeValueDao().GetAttributeValueInfo(valueId);
        }

        public static DataTable GetBrandCategoriesByTypeId(int typeId)
        {
            return new ProductTypeDao().GetBrandCategoriesByTypeId(typeId);
        }

        public static string GetBrandName(int type)
        {
            return new ProductTypeDao().GetBrandName(type);
        }

        public static ProductTypeInfo GetProductType(int typeId)
        {
            return new ProductTypeDao().GetProductType(typeId);
        }

        public static IList<ProductTypeInfo> GetProductTypes()
        {
            return new ProductTypeDao().GetProductTypes();
        }

        public static DbQueryResult GetProductTypes(ProductTypeQuery query)
        {
            return new ProductTypeDao().GetProductTypes(query);
        }

        public static int GetSpecificationId(int typeId, string specificationName)
        {
            int specificationId = new AttributeDao().GetSpecificationId(typeId, specificationName);
            if (specificationId > 0)
            {
                return specificationId;
            }
            AttributeInfo attribute = new AttributeInfo {
                TypeId = typeId,
                UsageMode = AttributeUseageMode.Choose,
                UseAttributeImage = false,
                AttributeName = specificationName
            };
            return new AttributeDao().AddAttributeName(attribute);
        }

        public static int GetSpecificationValueId(int attributeId, string valueStr)
        {
            int specificationValueId = new AttributeValueDao().GetSpecificationValueId(attributeId, valueStr);
            if (specificationValueId > 0)
            {
                return specificationValueId;
            }
            AttributeValueInfo attributeValue = new AttributeValueInfo {
                AttributeId = attributeId,
                ValueStr = valueStr
            };
            return new AttributeValueDao().AddAttributeValue(attributeValue);
        }

        public static int GetTypeId(string typeName)
        {
            int typeId = new ProductTypeDao().GetTypeId(typeName);
            if (typeId > 0)
            {
                return typeId;
            }
            ProductTypeInfo productType = new ProductTypeInfo {
                TypeName = typeName
            };
            return new ProductTypeDao().AddProductType(productType);
        }

        public static void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence)
        {
            new AttributeDao().SwapAttributeSequence(attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
        }

        public static void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
        {
            new AttributeValueDao().SwapAttributeValueSequence(attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateAttribute(AttributeInfo attribute)
        {
            return new AttributeDao().UpdateAttribute(attribute);
        }

        public static bool UpdateAttributeName(AttributeInfo attribute)
        {
            return new AttributeDao().UpdateAttributeName(attribute);
        }

        public static bool UpdateAttributeValue(AttributeValueInfo attributeValue)
        {
            return new AttributeValueDao().UpdateAttributeValue(attributeValue);
        }

        public static bool UpdateProductType(ProductTypeInfo productType)
        {
            if (productType == null)
            {
                return false;
            }
            Globals.EntityCoding(productType, true);
            bool flag = new ProductTypeDao().UpdateProductType(productType);
            if (flag)
            {
                if (new ProductTypeDao().DeleteProductTypeBrands(productType.TypeId))
                {
                    new ProductTypeDao().AddProductTypeBrands(productType.TypeId, productType.Brands);
                }
                EventLogs.WriteOperationLog(Privilege.EditProductType, string.Format(CultureInfo.InvariantCulture, "修改了编号为”{0}”的商品类型", new object[] { productType.TypeId }));
            }
            return flag;
        }

        public static string UploadSKUImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = Globals.GetStoragePath() + "/sku/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            Globals.UploadFileAndCheck(postedFile, HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }
    }
}

