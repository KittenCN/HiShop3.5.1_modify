namespace Hidistro.Core
{
    using System;

    public static class ProductTempSQLADD
    {
        public static string ReturnShowOrder(ProductShowOrderPriority show)
        {
            if (show == ProductShowOrderPriority.IDDESC)
            {
                return " DisplaySequence DESC";
            }
            if (show == ProductShowOrderPriority.AddedDateDESC)
            {
                return " AddedDate DESC";
            }
            if (show == ProductShowOrderPriority.AddedDateASC)
            {
                return " AddedDate ASC";
            }
            if (show == ProductShowOrderPriority.ShowSaleCountsDESC)
            {
                return " ShowSaleCounts DESC";
            }
            return "";
        }
    }
}

