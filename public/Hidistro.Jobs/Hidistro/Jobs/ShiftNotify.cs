namespace Hidistro.Jobs
{
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Quartz;
    using System;

    public class ShiftNotify : IJob
    {
        public string AppPath = "";

        public void Execute(IJobExecutionContext context)
        {
            string logname = "_Tonji.txt";
            try
            {
                Globals.Debuglog("定时器正执行指定任务AutoStatisticsOrdersV2...", logname);
                string retInfo = "";
                ShopStatisticHelper.AutoStatisticsOrdersV2("", out retInfo);
                Globals.Debuglog("任务执行完毕。结果：" + retInfo, logname);
            }
            catch (Exception exception)
            {
                Globals.Debuglog("任务执行出错：" + exception.Message, logname);
            }
        }
    }
}

