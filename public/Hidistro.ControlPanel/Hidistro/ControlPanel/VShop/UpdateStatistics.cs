namespace Hidistro.ControlPanel.VShop
{
    using System;

    public class UpdateStatistics
    {
        public void Update(object sender, StatisticNotifier.DataUpdatedEventArgs e)
        {
            StatisticNotifier notifier = (StatisticNotifier) sender;
            string retInfo = "";
            try
            {
                ShopStatisticHelper.StatisticsOrdersByNotify(notifier.RecDateUpdate, notifier.updateAction, notifier.actionDesc, out retInfo);
            }
            catch (Exception)
            {
            }
        }
    }
}

