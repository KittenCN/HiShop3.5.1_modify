namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class GameActDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int Create(GameActInfo game, ref string msg)
        {
            msg = "未知错误";
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_Game]([GameType],[GameName],[BeginDate],[EndDate],[Decription],[MemberGrades], [usePoint],[GivePoint],[bOnlyNotWinner],[attendTimes],[ImgUrl],[status],[unWinDecrip],[CreateStep]) VALUES (@GameType,@GameName,@BeginDate,@EndDate,@Decription,@MemberGrades,@usePoint,@GivePoint,@bOnlyNotWinner, @attendTimes,@ImgUrl,@status,@unWinDecrip,@CreateStep) SELECT CAST(scope_identity() AS int)");
                this.database.AddInParameter(sqlStringCommand, "GameType", DbType.Int32, (int) game.GameType);
                this.database.AddInParameter(sqlStringCommand, "GameName", DbType.String, game.GameName);
                this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.DateTime, game.BeginDate);
                this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, game.EndDate);
                this.database.AddInParameter(sqlStringCommand, "Decription", DbType.String, game.Decription);
                this.database.AddInParameter(sqlStringCommand, "MemberGrades", DbType.String, game.MemberGrades);
                this.database.AddInParameter(sqlStringCommand, "usePoint", DbType.Int32, game.usePoint);
                this.database.AddInParameter(sqlStringCommand, "GivePoint", DbType.Int32, game.GivePoint);
                this.database.AddInParameter(sqlStringCommand, "bOnlyNotWinner", DbType.Boolean, game.bOnlyNotWinner);
                this.database.AddInParameter(sqlStringCommand, "attendTimes", DbType.Int32, game.attendTimes);
                this.database.AddInParameter(sqlStringCommand, "ImgUrl", DbType.String, game.ImgUrl);
                this.database.AddInParameter(sqlStringCommand, "status", DbType.Int32, game.status);
                this.database.AddInParameter(sqlStringCommand, "unWinDecrip", DbType.String, game.unWinDecrip);
                this.database.AddInParameter(sqlStringCommand, "CreateStep", DbType.Int32, game.CreateStep);
                int num = (int) this.database.ExecuteScalar(sqlStringCommand);
                msg = "";
                return num;
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return 0;
            }
        }

        public bool Delete(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM  Hishop_Game WHERE GameId = @ID");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Game_Prize WHERE GameId = @ID");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeletePrize(int gameid)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from Hishop_Game_Prize where [GameId]= @GameId ");
                this.database.AddInParameter(sqlStringCommand, "GameId", DbType.Int32, gameid);
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeletePrize(int gameid, int prizeId)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from Hishop_Game_Prize where [GameId]= @GameId and [ID] = @ID ");
                this.database.AddInParameter(sqlStringCommand, "GameId", DbType.Int32, gameid);
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, prizeId);
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public GameActInfo GetGame(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Game WHERE GameId = @ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, Id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<GameActInfo>(reader);
            }
        }

        public GameActPrizeInfo GetPrize(int gameId, int prizeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Game_Prize  where [GameID]= @GameID and [ID] = @ID");
            this.database.AddInParameter(sqlStringCommand, "GameID", DbType.Int32, gameId);
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, prizeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<GameActPrizeInfo>(reader);
            }
        }

        public DataTable GetPrizes(int gameId)
        {
            DataTable table2;
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_Game_Prize where [GameId]= @GameId order by sort ");
                this.database.AddInParameter(sqlStringCommand, "GameId", DbType.Int32, gameId);
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    table2 = DataHelper.ConverDataReaderToDataTable(reader);
                }
            }
            catch (Exception)
            {
                table2 = null;
            }
            return table2;
        }

        public IList<GameActPrizeInfo> GetPrizesModel(int gameId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Game_Prize  where [GameID] = @GameID order by sort");
            this.database.AddInParameter(sqlStringCommand, "GameID", DbType.Int32, gameId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<GameActPrizeInfo>(reader);
            }
        }

        public int InsertPrize(GameActPrizeInfo prize)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_Game_Prize]([GameId],[PrizeName],[sort],[PrizeType],[GrivePoint],[PointNumber],[PointRate],[GiveCouponId],[CouponNumber],[CouponRate],[GiveProductId],[ProductNumber],[ProductRate])VALUES(@GameId,@PrizeName,@sort,@PrizeType,@GrivePoint,@PointNumber ,@PointRate,@GiveCouponId,@CouponNumber,@CouponRate,@GiveProductId,@ProductNumber,@ProductRate)  SELECT CAST(scope_identity() AS int)");
                this.database.AddInParameter(sqlStringCommand, "GameId", DbType.Int32, prize.GameId);
                this.database.AddInParameter(sqlStringCommand, "PrizeName", DbType.String, prize.PrizeName);
                this.database.AddInParameter(sqlStringCommand, "sort", DbType.Int32, prize.sort);
                this.database.AddInParameter(sqlStringCommand, "PrizeType", DbType.Int64, (long) prize.PrizeType);
                this.database.AddInParameter(sqlStringCommand, "GrivePoint", DbType.Int32, prize.GrivePoint);
                this.database.AddInParameter(sqlStringCommand, "PointNumber", DbType.Int32, prize.PointNumber);
                this.database.AddInParameter(sqlStringCommand, "PointRate", DbType.Decimal, prize.PointRate);
                this.database.AddInParameter(sqlStringCommand, "GiveCouponId", DbType.Int32, prize.GiveCouponId);
                this.database.AddInParameter(sqlStringCommand, "CouponNumber", DbType.Int32, prize.CouponNumber);
                this.database.AddInParameter(sqlStringCommand, "CouponRate", DbType.Decimal, prize.CouponRate);
                this.database.AddInParameter(sqlStringCommand, "GiveProductId", DbType.Int32, prize.GiveProductId);
                this.database.AddInParameter(sqlStringCommand, "ProductNumber", DbType.Int32, prize.ProductNumber);
                this.database.AddInParameter(sqlStringCommand, "ProductRate", DbType.Decimal, prize.ProductRate);
                return (int) this.database.ExecuteScalar(sqlStringCommand);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool Update(GameActInfo game, ref string msg)
        {
            msg = "未知错误";
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE[Hishop_Game]   SET [GameType] = @GameType , [GameName] = @GameName  , [BeginDate] = @BeginDate , [EndDate] = @EndDate , [Decription] = @Decription , [MemberGrades] = @MemberGrades , [usePoint] = @usePoint , [GivePoint] = @GivePoint , [bOnlyNotWinner] = @bOnlyNotWinner , [attendTimes] = @attendTimes , [ImgUrl] = @ImgUrl , [status] = @status , [unWinDecrip] = @unWinDecrip , [CreateStep] = @CreateStep  where GameId=@ID");
                this.database.AddInParameter(sqlStringCommand, "GameType", DbType.Int32, (int) game.GameType);
                this.database.AddInParameter(sqlStringCommand, "GameName", DbType.String, game.GameName);
                this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.DateTime, game.BeginDate);
                this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, game.EndDate);
                this.database.AddInParameter(sqlStringCommand, "Decription", DbType.String, game.Decription);
                this.database.AddInParameter(sqlStringCommand, "MemberGrades", DbType.String, game.MemberGrades);
                this.database.AddInParameter(sqlStringCommand, "usePoint", DbType.Int32, game.usePoint);
                this.database.AddInParameter(sqlStringCommand, "GivePoint", DbType.Int32, game.GivePoint);
                this.database.AddInParameter(sqlStringCommand, "bOnlyNotWinner", DbType.Boolean, game.bOnlyNotWinner);
                this.database.AddInParameter(sqlStringCommand, "attendTimes", DbType.Int32, game.attendTimes);
                this.database.AddInParameter(sqlStringCommand, "ImgUrl", DbType.String, game.ImgUrl);
                this.database.AddInParameter(sqlStringCommand, "status", DbType.Int32, game.status);
                this.database.AddInParameter(sqlStringCommand, "unWinDecrip", DbType.String, game.unWinDecrip);
                this.database.AddInParameter(sqlStringCommand, "CreateStep", DbType.Int32, game.CreateStep);
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, game.GameId);
                this.database.ExecuteScalar(sqlStringCommand);
                return true;
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return false;
            }
        }

        public bool UpdatePrize(GameActPrizeInfo prize)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_Game_Prize]   SET [GameId] = @GameId , [PrizeName] = @PrizeName , [sort] = @sort , [PrizeType] = @PrizeType , [GrivePoint] = @GrivePoint , [PointNumber] = @PointNumber , [PointRate] = @PointRate , [GiveCouponId] = @GiveCouponId , [CouponNumber] = @CouponNumber , [CouponRate] = @CouponRate , [GiveProductId] = @GiveProductId , [ProductNumber] = @ProductNumber , [ProductRate] = @ProductRate  where [GameId]= @GameId and [Id] = @Id");
            this.database.AddInParameter(sqlStringCommand, "GameId", DbType.Int32, prize.GameId);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, prize.Id);
            this.database.AddInParameter(sqlStringCommand, "PrizeName", DbType.String, prize.PrizeName);
            this.database.AddInParameter(sqlStringCommand, "sort", DbType.Int32, prize.sort);
            this.database.AddInParameter(sqlStringCommand, "PrizeType", DbType.Int64, (long) prize.PrizeType);
            this.database.AddInParameter(sqlStringCommand, "GrivePoint", DbType.Int32, prize.GrivePoint);
            this.database.AddInParameter(sqlStringCommand, "PointNumber", DbType.Int32, prize.PointNumber);
            this.database.AddInParameter(sqlStringCommand, "PointRate", DbType.Decimal, prize.PointRate);
            this.database.AddInParameter(sqlStringCommand, "GiveCouponId", DbType.Int32, prize.GiveCouponId);
            this.database.AddInParameter(sqlStringCommand, "CouponNumber", DbType.Int32, prize.CouponNumber);
            this.database.AddInParameter(sqlStringCommand, "CouponRate", DbType.Decimal, prize.CouponRate);
            this.database.AddInParameter(sqlStringCommand, "GiveProductId", DbType.Int32, prize.GiveProductId);
            this.database.AddInParameter(sqlStringCommand, "ProductNumber", DbType.Int32, prize.ProductNumber);
            this.database.AddInParameter(sqlStringCommand, "ProductRate", DbType.Decimal, prize.ProductRate);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

