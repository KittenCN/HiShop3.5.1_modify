namespace Hidistro.Jobs
{
    using Hidistro.Core;
    using Hidistro.SaleSystem.Vshop;
    using Quartz;
    using System;

    public class OneyuanNotify : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string logname = "_Oneyuan.txt";
            try
            {
                Globals.Debuglog("进入一元夺宝开奖试算", logname);
                OneyuanTaoHelp.CalculateWinner("");
                OneyuanTaoHelp.DelParticipantMember("", false);
                Globals.Debuglog("一元夺宝开奖结束", logname);
            }
            catch (Exception exception)
            {
                Globals.Debuglog("开奖异常：" + exception.Message, logname);
            }
        }
    }
}

