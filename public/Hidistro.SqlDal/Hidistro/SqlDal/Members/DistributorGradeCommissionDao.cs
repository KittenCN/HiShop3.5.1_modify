namespace Hidistro.SqlDal.Members
{
    using Hidistro.Entities.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;

    public class DistributorGradeCommissionDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddCommission(DistributorGradeCommissionInfo info)
        {
            string query = "UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance+@Commission WHERE UserId=@UserID;";
            query = query + "INSERT INTO Hishop_DistributorGradeCommission(UserID,Commission,PubTime,OperAdmin,Memo,OrderID,OldCommissionTotal)VALUES(@UserID,@Commission,@PubTime,@OperAdmin,@Memo,@OrderID,@OldCommissionTotal);" + "select @@identity;INSERT INTO Hishop_Commissions(UserId,ReferralUserId,OrderId,TradeTime,OrderTotal,CommTotal,CommType,State,CommRemark)values(@UserId,@ReferralUserId,@OrderID,@PubTime,0,@Commission,@CommType,1,@Memo);";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, info.UserId);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, info.ReferralUserId);
            this.database.AddInParameter(sqlStringCommand, "Commission", DbType.Decimal, info.Commission);
            this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, info.PubTime);
            this.database.AddInParameter(sqlStringCommand, "OperAdmin", DbType.String, info.OperAdmin);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, info.Memo);
            this.database.AddInParameter(sqlStringCommand, "OrderID", DbType.String, info.OrderID);
            this.database.AddInParameter(sqlStringCommand, "OldCommissionTotal", DbType.Decimal, info.OldCommissionTotal);
            this.database.AddInParameter(sqlStringCommand, "CommType", DbType.Int32, info.CommType);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

