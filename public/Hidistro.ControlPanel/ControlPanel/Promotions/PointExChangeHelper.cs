namespace ControlPanel.Promotions
{
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Promotions;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class PointExChangeHelper
    {
        private static PointExChangeDao _exchange = new PointExChangeDao();

        public static bool AddProducts(int exchangeId, string productIds, string pNumbers, string points, string eachNumbers)
        {
            return _exchange.AddProducts(exchangeId, productIds, pNumbers, points, eachNumbers);
        }

        public static int Create(PointExChangeInfo exchange, ref string msg)
        {
            return _exchange.Create(exchange, ref msg);
        }

        public static bool Delete(int Id)
        {
            return _exchange.Delete(Id);
        }

        public static bool DeleteProduct(int exchangeId, int productId)
        {
            return _exchange.DeleteProduct(exchangeId, productId);
        }

        public static bool DeleteProducts(int exchangeId, string productIds)
        {
            return _exchange.DeleteProducts(exchangeId, productIds);
        }

        public static bool EditProducts(int exchangeId, string productIds, string pNumbers, string points, string eachNumbers)
        {
            return _exchange.EditProducts(exchangeId, productIds, pNumbers, points, eachNumbers);
        }

        public static PointExChangeInfo Get(int Id)
        {
            return _exchange.GetExChange(Id);
        }

        public static int GetProductExchangedCount(int exchangeId, int productId)
        {
            return _exchange.GetProductExchangedCount(exchangeId, productId);
        }

        public static PointExchangeProductInfo GetProductInfo(int exchangeId, int productId)
        {
            return _exchange.GetProductInfo(exchangeId, productId);
        }

        public static DataTable GetProducts(int exchangeId)
        {
            return _exchange.GetProducts(exchangeId);
        }

        public static DataTable GetProducts(int exchangeId, int pageNumber, int maxNum, out int total, string sort, string order)
        {
            return _exchange.GetProducts(exchangeId, pageNumber, maxNum, out total, sort, order == "asc");
        }

        public static int GetUserProductExchangedCount(int exchangeId, int productId, int userId)
        {
            return _exchange.GetUserProductExchangedCount(exchangeId, productId, userId);
        }

        public static bool InsertProduct(PointExchangeProductInfo product)
        {
            return _exchange.InsertProduct(product);
        }

        public static DataTable Query(ExChangeSearch search, ref int total)
        {
            return _exchange.Query(search, ref total);
        }

        public static bool SetProductsStatus(int exchangeId, int status, string productIds)
        {
            return _exchange.SetProductsStatus(exchangeId, status, productIds);
        }

        public static bool Update(PointExChangeInfo exchange, ref string msg)
        {
            return _exchange.Update(exchange, ref msg);
        }

        public static bool UpdateProduct(PointExchangeProductInfo product)
        {
            return _exchange.UpdateProduct(product);
        }

        public static bool UpdateProduct(int exchangeId, int productId)
        {
            return _exchange.DeleteProduct(exchangeId, productId);
        }
    }
}

