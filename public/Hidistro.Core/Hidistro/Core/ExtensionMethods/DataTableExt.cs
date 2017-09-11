namespace Hidistro.Core.ExtensionMethods
{
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;

    public static class DataTableExt
    {
        public static DataTable Take(this DataTable t, int Begin, int Take)
        {
            if ((Begin == Take) && (Take == 0))
            {
                return t;
            }
            DataTable table = t.Clone();
            while ((Begin < t.Rows.Count) && (Take > 0))
            {
                table.ImportRow(t.Rows[Begin]);
                Take--;
                Begin++;
            }
            return table;
        }
    }
}

