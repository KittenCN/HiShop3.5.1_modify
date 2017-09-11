namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.VShop;
    using System;

    public static class ShareMaterialBrowser
    {
        public static int AddNineImgses(NineImgsesItem info)
        {
            return new ShareMaterialDao().AddNineImgses(info);
        }

        public static bool DeleteNineImgses(int id)
        {
            return new ShareMaterialDao().DeleteActivities(id);
        }

        public static NineImgsesItem GetNineImgse(int id)
        {
            return new ShareMaterialDao().GetNineImgse(id);
        }

        public static DbQueryResult GetNineImgsesList(NineImgsesQuery query)
        {
            return new ShareMaterialDao().GetNineImgsesList(query);
        }

        public static bool UpdateNineImgses(NineImgsesItem info)
        {
            return new ShareMaterialDao().UpdateNineImgses(info);
        }
    }
}

