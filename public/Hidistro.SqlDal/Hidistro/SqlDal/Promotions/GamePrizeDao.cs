namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class GamePrizeDao
    {
        private Database _db = DatabaseFactory.CreateDatabase();

        public bool Create(GamePrizeInfo model)
        {
            string query = "INSERT INTO [Hishop_PromotionGamePrizes]\r\n                    ([GameId]\r\n                    ,[PrizeGrade]\r\n                    ,[PrizeType]\r\n                    ,[GivePoint]\r\n                    ,[GiveCouponId]\r\n                    ,[GiveShopBookId]\r\n                    ,[PrizeCount]\r\n                    ,[PrizeRate]\r\n                    ,[GriveShopBookPicUrl]\r\n                    ,[PrizeName]\r\n                    ,[Prize]\r\n                    ,IsLogistics\r\n                    ,PrizeImage\r\n                   )\r\n                  VALUES\r\n                    ( @GameId\r\n                    ,@PrizeGrade\r\n                    ,@PrizeType\r\n                    ,@GivePoint\r\n                    ,@GiveCouponId\r\n                    ,@GiveShopBookId\r\n                    ,@PrizeCount\r\n                    ,@PrizeRate\r\n                    ,@GriveShopBookPicUrl\r\n                    ,@PrizeName\r\n                    ,@Prize\r\n                    ,@IsLogistics\r\n                    ,@PrizeImage\r\n                    );";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
            this._db.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, model.GameId);
            this._db.AddInParameter(sqlStringCommand, "@PrizeGrade", DbType.Int32, (int) model.PrizeGrade);
            this._db.AddInParameter(sqlStringCommand, "@PrizeType", DbType.Int32, (int) model.PrizeType);
            this._db.AddInParameter(sqlStringCommand, "@GivePoint", DbType.Decimal, model.GivePoint);
            this._db.AddInParameter(sqlStringCommand, "@GiveCouponId", DbType.String, model.GiveCouponId);
            this._db.AddInParameter(sqlStringCommand, "@GiveShopBookId", DbType.String, model.GiveShopBookId);
            this._db.AddInParameter(sqlStringCommand, "@PrizeCount", DbType.Int32, model.PrizeCount);
            this._db.AddInParameter(sqlStringCommand, "@PrizeRate", DbType.Int32, model.PrizeRate);
            this._db.AddInParameter(sqlStringCommand, "@GriveShopBookPicUrl", DbType.String, model.GriveShopBookPicUrl);
            this._db.AddInParameter(sqlStringCommand, "@PrizeName", DbType.String, model.PrizeName);
            this._db.AddInParameter(sqlStringCommand, "@Prize", DbType.String, model.Prize);
            this._db.AddInParameter(sqlStringCommand, "@IsLogistics", DbType.Int32, model.IsLogistics);
            this._db.AddInParameter(sqlStringCommand, "@PrizeImage", DbType.String, string.IsNullOrEmpty(model.PrizeImage) ? "/utility/pics/lipin60.png" : model.PrizeImage);
            return (this._db.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CreateWinningPool(GameWinningPoolInfo model)
        {
            string query = "INSERT INTO [Hishop_PromotionWinningPool]\r\n                    ([GameId]\r\n                    ,[Number]\r\n                    ,[GamePrizeId]\r\n                    ,[IsReceive])\r\n                  VALUES\r\n                    (@GameId\r\n                    ,@Number\r\n                    ,@GamePrizeId\r\n                    ,@IsReceive);";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
            this._db.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, model.GameId);
            this._db.AddInParameter(sqlStringCommand, "@Number", DbType.Int32, model.Number);
            this._db.AddInParameter(sqlStringCommand, "@GamePrizeId", DbType.Int32, model.GamePrizeId);
            this._db.AddInParameter(sqlStringCommand, "@IsReceive", DbType.Decimal, model.IsReceive);
            return (this._db.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool Delete(GamePrizeInfo model)
        {
            string query = "delete [Hishop_PromotionGamePrizes] \r\n                    Where GameId=@GameId and PrizeId=@PrizeId and GameId in(select GameId from Hishop_PromotionGame where getdate()< BeginTime);";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
            this._db.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, model.GameId);
            this._db.AddInParameter(sqlStringCommand, "@PrizeId", DbType.Int32, model.PrizeId);
            return (this._db.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteWinningPools(int GameId)
        {
            string query = "delete [Hishop_PromotionWinningPool] \r\n                    Where GameId=@GameId";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
            this._db.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, GameId);
            return (this._db.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public GamePrizeInfo GetGamePrizeInfoById(int id)
        {
            string query = "Select * From [Hishop_PromotionGamePrizes] where PrizeId=@PrizeId ";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
            this._db.AddInParameter(sqlStringCommand, "@PrizeId", DbType.Int32, id);
            using (IDataReader reader = this._db.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<GamePrizeInfo>(reader);
            }
        }

        public IList<GamePrizeInfo> GetGamePrizeListsByGameId(int gameId)
        {
            string query = "Select * From [Hishop_PromotionGamePrizes] where GameId=@GameId ";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
            this._db.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, gameId);
            using (IDataReader reader = this._db.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<GamePrizeInfo>(reader);
            }
        }

        public GamePrizeInfo GetModelByPrizeGradeAndGameId(PrizeGrade grade, int gameId)
        {
            string query = "Select * From [Hishop_PromotionGamePrizes] where PrizeGrade=@PrizeGrade and GameId=@GameId ";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
            this._db.AddInParameter(sqlStringCommand, "@PrizeGrade", DbType.Int32, (int) grade);
            this._db.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, gameId);
            using (IDataReader reader = this._db.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<GamePrizeInfo>(reader);
            }
        }

        public int GetOppNumber(int userId, int gameId)
        {
            string query = "select COUNT(*) from Hishop_PromotionGameResultMembersLog where UserId=@UserId AND GameId=@GameId ";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
            this._db.AddInParameter(sqlStringCommand, "@UserId", DbType.Int32, userId);
            this._db.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, gameId);
            return (int) this._db.ExecuteScalar(sqlStringCommand);
        }

        public int GetOppNumberByToday(int userId, int gameId)
        {
            string query = "select COUNT(*) from Hishop_PromotionGameResultMembersLog where UserId=@UserId AND GameId=@GameId And PlayTime>=CONVERT(varchar(100), GETDATE(), 23)";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
            this._db.AddInParameter(sqlStringCommand, "@UserId", DbType.Int32, userId);
            this._db.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, gameId);
            return (int) this._db.ExecuteScalar(sqlStringCommand);
        }

        public bool Update(GamePrizeInfo model)
        {
            string query = "Update [Hishop_PromotionGamePrizes] set [PrizeType]=@PrizeType\r\n                    ,[GivePoint]=@GivePoint\r\n                    ,[GiveCouponId]=@GiveCouponId\r\n                    ,[GiveShopBookId]=@GiveShopBookId\r\n                    ,[PrizeCount]=@PrizeCount\r\n                    ,[GriveShopBookPicUrl]=@GriveShopBookPicUrl\r\n                    ,[IsLogistics]=@IsLogistics\r\n                    ,[PrizeImage]=@PrizeImage\r\n                    Where PrizeId=@PrizeId ;";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
            this._db.AddInParameter(sqlStringCommand, "@PrizeType", DbType.Int32, (int) model.PrizeType);
            this._db.AddInParameter(sqlStringCommand, "@GivePoint", DbType.Decimal, model.GivePoint);
            this._db.AddInParameter(sqlStringCommand, "@GiveCouponId", DbType.String, model.GiveCouponId);
            this._db.AddInParameter(sqlStringCommand, "@GiveShopBookId", DbType.String, model.GiveShopBookId);
            this._db.AddInParameter(sqlStringCommand, "@PrizeCount", DbType.Int32, model.PrizeCount);
            this._db.AddInParameter(sqlStringCommand, "@GriveShopBookPicUrl", DbType.String, model.GriveShopBookPicUrl);
            this._db.AddInParameter(sqlStringCommand, "@IsLogistics", DbType.Int32, model.IsLogistics);
            this._db.AddInParameter(sqlStringCommand, "@PrizeImage", DbType.String, string.IsNullOrEmpty(model.PrizeImage) ? "/utility/pics/lipin60.png" : model.PrizeImage);
            this._db.AddInParameter(sqlStringCommand, "@PrizeId", DbType.Int32, model.PrizeId);
            return (this._db.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

