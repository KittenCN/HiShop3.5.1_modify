namespace Hidistro.Jobs
{
    using Hidistro.Core;
    using Hidistro.SaleSystem.Vshop;
    using Quartz;
    using System;

    public class RedPachCheckJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                RedPackHelp.RedPackCheckJob();
            }
            catch (Exception exception)
            {
                Globals.Debuglog("RedPachCheckJob任务出错了:" + exception.Message, "_Debuglog.txt");
            }
        }
    }
}

