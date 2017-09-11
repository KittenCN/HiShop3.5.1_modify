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

    public class ShipperDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddShipper(ShippersInfo shipper)
        {
            StringBuilder builder = new StringBuilder();
            if (shipper.ShipperTag == "1")
            {
                builder.AppendLine("update Hishop_Shippers set ShipperTag='0' where ShipperTag='1' ;");
                builder.AppendLine("update Hishop_Shippers set ShipperTag='2' where ShipperTag='3' ;");
            }
            else if (shipper.ShipperTag == "2")
            {
                builder.AppendLine("update Hishop_Shippers set ShipperTag='0' where ShipperTag='2' ;");
                builder.AppendLine("update Hishop_Shippers set ShipperTag='1' where ShipperTag='3' ;");
            }
            else if (shipper.ShipperTag == "3")
            {
                builder.AppendLine("update Hishop_Shippers set ShipperTag='0' where ShipperTag='1' ;");
                builder.AppendLine("update Hishop_Shippers set ShipperTag='0' where ShipperTag='2' ;");
                builder.AppendLine("update Hishop_Shippers set ShipperTag='0' where ShipperTag='3' ;");
            }
            builder.AppendLine("IF EXISTS(select top 1 * from Hishop_Shippers where ShipperId=@ShipperId)").AppendLine("Begin").Append("UPDATE Hishop_Shippers SET ShipperTag=@ShipperTag, ShipperName=@ShipperName,").Append("RegionId=@RegionId, Address=@Address, CellPhone=@CellPhone,TelPhone=@TelPhone ").AppendLine("where ShipperId=@ShipperId;").AppendLine("End").AppendLine("ELSE").AppendLine("Begin").Append("INSERT INTO Hishop_Shippers (IsDefault, ShipperTag, ShipperName, RegionId, Address, CellPhone, TelPhone, Zipcode, Remark)").AppendLine(" VALUES (@IsDefault, @ShipperTag, @ShipperName, @RegionId, @Address, @CellPhone, @TelPhone, @Zipcode, @Remark);").AppendLine("End");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, shipper.IsDefault);
            this.database.AddInParameter(sqlStringCommand, "ShipperTag", DbType.String, shipper.ShipperTag);
            this.database.AddInParameter(sqlStringCommand, "ShipperName", DbType.String, shipper.ShipperName);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shipper.RegionId);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, shipper.Address);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shipper.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shipper.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shipper.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, shipper.Remark);
            this.database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int16, shipper.ShipperId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteShipper(int shipperId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Shippers WHERE ShipperId = @ShipperId");
            this.database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipperId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public ShippersInfo GetShipper(int shipperId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Shippers WHERE ShipperId = @ShipperId");
            this.database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipperId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ShippersInfo>(reader);
            }
        }

        public IList<ShippersInfo> GetShippers(bool includeDistributor)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Shippers");
            if (!includeDistributor)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " WHERE DistributorUserId = 0";
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<ShippersInfo>(reader);
            }
        }

        public void SetDefalutShipper(int shipperId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Shippers SET IsDefault = 0;UPDATE Hishop_Shippers SET IsDefault = 1 WHERE ShipperId = @ShipperId");
            this.database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipperId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool SwapShipper(int ShipperId, string ShipperTag)
        {
            string str = ShipperTag;
            if (str == "退货")
            {
                str = "发货";
            }
            else
            {
                str = "退货";
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("UPDATE Hishop_Shippers SET ShipperTag=@ShipperTag where  ShipperId != @ShipperId;").Append("UPDATE Hishop_Shippers SET ShipperTag=@NewTag where  ShipperId = @ShipperId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, ShipperId);
            this.database.AddInParameter(sqlStringCommand, "ShipperTag", DbType.String, ShipperTag);
            this.database.AddInParameter(sqlStringCommand, "NewTag", DbType.String, str);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateShipper(ShippersInfo shipper)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Shippers SET ShipperTag = @ShipperTag, ShipperName = @ShipperName, RegionId = @RegionId, Address = @Address, CellPhone = @CellPhone, TelPhone = @TelPhone, Zipcode = @Zipcode, Remark =@Remark WHERE ShipperId = @ShipperId");
            this.database.AddInParameter(sqlStringCommand, "ShipperTag", DbType.String, shipper.ShipperTag);
            this.database.AddInParameter(sqlStringCommand, "ShipperName", DbType.String, shipper.ShipperName);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shipper.RegionId);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, shipper.Address);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shipper.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shipper.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shipper.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, shipper.Remark);
            this.database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipper.ShipperId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

