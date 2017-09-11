namespace Hidistro.SqlDal.Sales
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class PaymentModeDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public PaymentModeActionStatus CreateUpdateDeletePaymentMode(PaymentModeInfo paymentMode, DataProviderAction action)
        {
            if (paymentMode == null)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_PaymentType_CreateUpdateDelete");
            this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int) action);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action == DataProviderAction.Create)
            {
                this.database.AddOutParameter(storedProcCommand, "ModeId", DbType.Int32, 4);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "ModeId", DbType.Int32, paymentMode.ModeId);
            }
            if (action != DataProviderAction.Delete)
            {
                this.database.AddInParameter(storedProcCommand, "Name", DbType.String, paymentMode.Name);
                this.database.AddInParameter(storedProcCommand, "Description", DbType.String, paymentMode.Description);
                this.database.AddInParameter(storedProcCommand, "Gateway", DbType.String, paymentMode.Gateway);
                this.database.AddInParameter(storedProcCommand, "IsUseInpour", DbType.Boolean, paymentMode.IsUseInpour);
                this.database.AddInParameter(storedProcCommand, "IsUseInDistributor", DbType.Boolean, paymentMode.IsUseInDistributor);
                this.database.AddInParameter(storedProcCommand, "Charge", DbType.Currency, paymentMode.Charge);
                this.database.AddInParameter(storedProcCommand, "IsPercent", DbType.Boolean, paymentMode.IsPercent);
                this.database.AddInParameter(storedProcCommand, "Settings", DbType.String, paymentMode.Settings);
            }
            this.database.ExecuteNonQuery(storedProcCommand);
            return (PaymentModeActionStatus) ((int) this.database.GetParameterValue(storedProcCommand, "Status"));
        }

        public PaymentModeInfo GetPaymentMode(int modeId)
        {
            PaymentModeInfo info = new PaymentModeInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes WHERE ModeId = @ModeId");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePayment(reader);
                }
            }
            return info;
        }

        public PaymentModeInfo GetPaymentMode(string gateway)
        {
            PaymentModeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_PaymentTypes WHERE Gateway = @Gateway");
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, gateway);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePayment(reader);
                }
            }
            return info;
        }

        public IList<PaymentModeInfo> GetPaymentModes()
        {
            IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes Order by DisplaySequence desc");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePayment(reader));
                }
            }
            return list;
        }

        public void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_PaymentTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }
    }
}

