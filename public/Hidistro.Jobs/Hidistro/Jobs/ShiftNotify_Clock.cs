namespace Hidistro.Jobs
{
    using Hidistro.Core;
    using Quartz;
    using System;

    public class ShiftNotify_Clock : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Globals.Debuglog("打卡定时器定时打卡成功...", "_TonjiClock.txt");
            }
            catch (Exception exception)
            {
                Globals.Debuglog("打卡定时器定时打卡出错：" + exception.Message, "_TonjiClock.txt");
            }
        }
    }
}

