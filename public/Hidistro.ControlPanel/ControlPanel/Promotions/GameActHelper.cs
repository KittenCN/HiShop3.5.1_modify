namespace ControlPanel.Promotions
{
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class GameActHelper
    {
        private static GameActDao _game = new GameActDao();

        public static int Create(GameActInfo game, ref string msg)
        {
            return _game.Create(game, ref msg);
        }

        public static bool Delete(int Id)
        {
            return _game.Delete(Id);
        }

        public static bool DeletePrize(int gameId)
        {
            return _game.DeletePrize(gameId);
        }

        public static bool DeletePrize(int gameId, int prizeId)
        {
            return _game.DeletePrize(gameId, prizeId);
        }

        public static GameActInfo Get(int Id)
        {
            return _game.GetGame(Id);
        }

        public static GameActPrizeInfo GetPrize(int gameId, int prizeId)
        {
            return _game.GetPrize(gameId, prizeId);
        }

        public static DataTable GetPrizes(int gameId)
        {
            return _game.GetPrizes(gameId);
        }

        public static IList<GameActPrizeInfo> GetPrizesModel(int gameId)
        {
            return _game.GetPrizesModel(gameId);
        }

        public static int InsertPrize(GameActPrizeInfo prize)
        {
            return _game.InsertPrize(prize);
        }

        public static bool Update(GameActInfo game, ref string msg)
        {
            return _game.Update(game, ref msg);
        }

        public static bool UpdatePrize(GameActPrizeInfo prize)
        {
            return _game.UpdatePrize(prize);
        }
    }
}

