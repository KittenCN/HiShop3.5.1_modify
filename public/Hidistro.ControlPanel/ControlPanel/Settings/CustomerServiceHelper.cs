namespace ControlPanel.Settings
{
    using Hidistro.Entities.Settings;
    using Hidistro.SqlDal.Settings;
    using System;
    using System.Data;

    public class CustomerServiceHelper
    {
        private static CustomerServiceDao dao = new CustomerServiceDao();

        public static int CreateCustomer(CustomerServiceInfo info, ref string msg)
        {
            return dao.CreateCustomer(info, ref msg);
        }

        public static bool DeletCustomer(int id)
        {
            return dao.DeletCustomer(id);
        }

        public static CustomerServiceInfo GetCustomer(int id)
        {
            return dao.GetCustomer(id);
        }

        public static DataTable GetCustomers(string unit)
        {
            return dao.GetCustomers(unit);
        }

        public static bool UpdateCustomer(CustomerServiceInfo info, ref string msg)
        {
            return dao.UpdateCustomer(info, ref msg);
        }
    }
}

