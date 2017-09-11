namespace Hidistro.Jobs
{
    using Hidistro.Core;
    using Newtonsoft.Json;
    using Quartz;
    using Quartz.Impl;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public static class JobsHelp
    {
        private static string ConfigFile = "";
        private static IScheduler sched = null;

        public static void start(string _ConfigFile)
        {
            ConfigFile = _ConfigFile;
            List<jobinfo> list = new List<jobinfo>();
            try
            {
                if (sched != null)
                {
                    stop();
                    sched = null;
                }
                sched = new StdSchedulerFactory().GetScheduler();
                XmlDocument document = new XmlDocument();
                document.Load(ConfigFile);
                XmlNode node = document.SelectSingleNode("Jobs");
                if (node.ChildNodes.Count > 0)
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        jobinfo item = new jobinfo {
                            name = node2.Attributes["name"].Value,
                            type = node2.Attributes["type"].Value,
                            CronExpression = node2.Attributes["CronExpression"].Value,
                            enabled = bool.Parse(node2.Attributes["enabled"].Value)
                        };
                        if (item.enabled)
                        {
                            list.Add(item);
                            IJobDetail jobDetail = JobBuilder.Create(Type.GetType(item.type)).WithIdentity(item.name, item.name + "Group").Build();
                            ITrigger trigger = TriggerBuilder.Create().WithIdentity(item.name, item.name + "Group").WithCronSchedule(item.CronExpression).Build();
                            sched.ScheduleJob(jobDetail, trigger);
                        }
                    }
                    if (list.Count > 0)
                    {
                        sched.Start();
                    }
                    else
                    {
                        Globals.Debuglog("暂未有计划任务开启1", "_Debuglog.txt");
                    }
                }
                else
                {
                    Globals.Debuglog("暂未有计划任务开启", "_Debuglog.txt");
                }
            }
            catch (Exception exception)
            {
                Globals.Debuglog(JsonConvert.SerializeObject(list), "_Debuglog.txt");
                Globals.Debuglog("启动计划任务失败：" + exception.Message, "_Debuglog.txt");
            }
        }

        public static void stop()
        {
            try
            {
                if (sched != null)
                {
                    sched.Shutdown(false);
                    sched.Clear();
                }
            }
            catch (Exception exception)
            {
                Globals.Debuglog("关闭计划任务失败：" + exception.Message, "_Debuglog.txt");
            }
        }

        private class jobinfo
        {
            public string CronExpression { get; set; }

            public bool enabled { get; set; }

            public string name { get; set; }

            public string type { get; set; }
        }
    }
}

