namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.SqlDal.Store;
    using System;

    public static class RedPackHelp
    {
        public static void RedPackCheckJob()
        {
            new RedPackDao().RedPackCheckJob();
        }
    }
}

