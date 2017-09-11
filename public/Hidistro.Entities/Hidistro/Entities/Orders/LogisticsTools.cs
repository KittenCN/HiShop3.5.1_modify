namespace Hidistro.Entities.Orders
{
    using System;
    using System.ComponentModel;

    public enum LogisticsTools
    {
        [Description("快递100")]
        Kuaidi100 = 1,
        [Description("快递鸟")]
        Kuaidiniao = 0
    }
}

