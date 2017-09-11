namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Entities.Members;
    using Hidistro.SqlDal.Members;
    using System;

    public class DistributorGradeCommissionBrower
    {
        public static bool AddCommission(DistributorGradeCommissionInfo info)
        {
            return new DistributorGradeCommissionDao().AddCommission(info);
        }
    }
}

