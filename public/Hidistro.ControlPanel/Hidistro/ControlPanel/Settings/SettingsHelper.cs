namespace Hidistro.ControlPanel.Settings
{
    using Hidistro.Entities.Settings;
    using Hidistro.SqlDal.Sales;
    using Hidistro.SqlDal.Settings;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public sealed class SettingsHelper
    {
        public static string Error = "";

        public static bool CreateShippingTemplate(FreightTemplate freightTemplate)
        {
            ShippingModeDao dao = new ShippingModeDao();
            bool flag = dao.CreateShippingTemplate(freightTemplate);
            Error = dao.Error;
            return flag;
        }

        public static int DeleteExpressTemplates(string expressIds)
        {
            return new ExpressTemplateDao().DeleteExpressTemplates(expressIds);
        }

        public static bool DeleteShippingTemplate(int templateId)
        {
            return new ShippingModeDao().DeleteShippingTemplate(templateId);
        }

        public static int DeleteShippingTemplates(string templateIds)
        {
            return new ShippingModeDao().DeleteShippingTemplates(templateIds);
        }

        public static DataTable GetAllFreightItems()
        {
            return new ShippingModeDao().GetAllFreightItems();
        }

        public static string getDefaultShipText(bool IsDefault)
        {
            string str = "";
            if (IsDefault)
            {
                str = "全国";
            }
            return str;
        }

        public static string getFreeShipText(bool FreeShip)
        {
            string str = "卖家承担";
            if (FreeShip)
            {
                str = "包邮";
            }
            return str;
        }

        public static DataTable GetFreeTemplateShipping(string RegionId, int TemplateId, int ModeId)
        {
            return new ShippingModeDao().GetFreeTemplateShipping(RegionId, TemplateId, ModeId);
        }

        public static FreightTemplate GetFreightTemplate(int templateId, bool includeDetail)
        {
            return new ShippingModeDao().GetFreightTemplate(templateId, includeDetail);
        }

        public static IList<FreightTemplate> GetFreightTemplates()
        {
            return new ExpressTemplateDao().GetFreightTemplates();
        }

        public static string getMUnitText(int MUnit)
        {
            switch (MUnit)
            {
                case 1:
                    return "件";

                case 2:
                    return "KG";

                case 3:
                    return "立方";
            }
            return "件";
        }

        public static string GetShippingTemplateLinkProduct(int[] templateIds)
        {
            return new ShippingModeDao().GetShippingTemplateLinkProduct(templateIds);
        }

        public static string getShippingTypeByModeId(int ModeId)
        {
            switch (ModeId)
            {
                case 1:
                    return "快递";

                case 2:
                    return "EMS";

                case 3:
                    return "顺丰";

                case 4:
                    return "平邮";
            }
            return "未知";
        }

        public static IList<SpecifyRegionGroup> GetSpecifyRegionGroups(int templateId)
        {
            return new ShippingModeDao().GetSpecifyRegionGroups(templateId);
        }

        public static DataTable GetSpecifyRegionGroupsModeId(string TemplateIds, string RegionId)
        {
            return new ShippingModeDao().GetSpecifyRegionGroupsModeId(TemplateIds, RegionId);
        }

        public static FreightTemplate GetTemplateMessage(int TemplateId)
        {
            return new ShippingModeDao().GetTemplateMessage(TemplateId);
        }

        public static bool SetExpressIsDefault(int expressId)
        {
            return new ExpressTemplateDao().SetExpressDefault(expressId);
        }

        public static bool UpdateShippingTemplate(FreightTemplate freightTemplate, string templateName)
        {
            ShippingModeDao dao = new ShippingModeDao();
            bool flag = dao.UpdateShippingTemplate(freightTemplate, templateName);
            Error = dao.Error;
            return flag;
        }
    }
}

