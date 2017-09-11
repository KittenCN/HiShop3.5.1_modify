namespace Hidistro.ControlPanel.VShop
{
    using Hidistro.Entities.StatisticsReport;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class StatisticNotifier
    {
        public string actionDesc = "";
        public DateTime RecDateUpdate;
        public UpdateAction updateAction;

        public event DataUpdatedEventHandler DataUpdated;

        public virtual void OnDataUpdated(DataUpdatedEventArgs e)
        {
            if (this.DataUpdated != null)
            {
                this.DataUpdated(this, e);
            }
        }

        public void UpdateDB()
        {
            DataUpdatedEventArgs e = new DataUpdatedEventArgs();
            this.OnDataUpdated(e);
        }

        public class DataUpdatedEventArgs : EventArgs
        {
            public readonly int temperature;
        }

        public delegate void DataUpdatedEventHandler(object sender, StatisticNotifier.DataUpdatedEventArgs e);
    }
}

