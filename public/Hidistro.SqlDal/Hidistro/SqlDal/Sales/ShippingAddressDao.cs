namespace Hidistro.SqlDal.Sales
{
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class ShippingAddressDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int AddShippingAddress(ShippingAddressInfo shippingAddress)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_UserShippingAddresses(RegionId,UserId,ShipTo,Address,Zipcode,TelPhone,CellPhone,IsDefault) VALUES(@RegionId,@UserId,@ShipTo,@Address,@Zipcode,@TelPhone,@CellPhone,@IsDefault); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shippingAddress.RegionId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, shippingAddress.UserId);
            this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, shippingAddress.ShipTo);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, shippingAddress.Address);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shippingAddress.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shippingAddress.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shippingAddress.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, shippingAddress.IsDefault);
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }

        public bool DelShippingAddress(int shippingid, int userid)
        {
            StringBuilder builder = new StringBuilder("delete from Hishop_UserShippingAddresses");
            builder.Append(" where shippingId=@shippingId and UserId=@UserId ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "shippingId", DbType.Int32, shippingid);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public ShippingAddressInfo GetShippingAddress(int shippingId, int userid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE ShippingId = @ShippingId and UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "ShippingId", DbType.Int32, shippingId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            ShippingAddressInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShippingAddress(reader);
                }
            }
            return info;
        }

        public IList<ShippingAddressInfo> GetShippingAddresses(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE  UserID = @UserID");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<ShippingAddressInfo>(reader);
            }
        }

        public bool SetDefaultShippingAddress(int shippingId, int UserId)
        {
            StringBuilder builder = new StringBuilder("UPDATE  Hishop_UserShippingAddresses SET IsDefault = 0 where UserId=@UserId;");
            builder.Append("UPDATE  Hishop_UserShippingAddresses SET IsDefault = 1 WHERE ShippingId = @ShippingId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "shippingId", DbType.Int32, shippingId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateShippingAddress(ShippingAddressInfo shippingAddress)
        {
            string str = shippingAddress.Address.Replace("\n", " ").Replace("\r", "");
            StringBuilder builder = new StringBuilder();
            builder.Append("update Hishop_UserShippingAddresses");
            builder.Append(" set ShipTo=@ShipTo,");
            builder.Append("Address=@Address,");
            builder.Append("Zipcode=@Zipcode,");
            builder.Append("TelPhone=@TelPhone,");
            builder.Append("CellPhone=@CellPhone,");
            builder.Append(" RegionId=@RegionId");
            builder.Append(" where shippingId=@shippingId");
            builder.Append(" and UserId=@UserId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shippingAddress.RegionId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, shippingAddress.UserId);
            this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, shippingAddress.ShipTo);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, str);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shippingAddress.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shippingAddress.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shippingAddress.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "shippingId", DbType.Int32, shippingAddress.ShippingId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

