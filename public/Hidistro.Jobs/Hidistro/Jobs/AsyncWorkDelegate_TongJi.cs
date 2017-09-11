namespace Hidistro.Jobs
{
    using Hidistro.ControlPanel.VShop;
    using System;
    using System.Runtime.InteropServices;

    public class AsyncWorkDelegate_TongJi
    {
        public void CalData(string AppPath, out bool result)
        {
            string retInfo = "";
            ShopStatisticHelper.AutoStatisticsOrdersV2(AppPath, out retInfo);
            result = true;
        }
    }
}

