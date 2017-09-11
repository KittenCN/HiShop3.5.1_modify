namespace Hidistro.Jobs
{
    using Hidistro.SaleSystem.Vshop;
    using Quartz;
    using System;

    public class OrderJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            MemberProcessor.GetAutoBatchOrdersIdList();
        }
    }
}

