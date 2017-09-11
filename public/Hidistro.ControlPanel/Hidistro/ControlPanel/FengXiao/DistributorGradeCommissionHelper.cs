namespace Hidistro.ControlPanel.FengXiao
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.FenXiao;
    using Hidistro.SqlDal.FengXiao;
    using System;

    public class DistributorGradeCommissionHelper
    {
        public static DbQueryResult DistributorGradeCommission(DistributorGradeCommissionQuery query)
        {
            return new DistributorGradeCommissionDao().DistributorGradeCommission(query);
        }
    }
}

