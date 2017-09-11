namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Entities;
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class BannerDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool DelTplCfg(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(new StringBuilder(" delete from Hishop_Banner where BannerId=@BannerId").ToString());
            this.database.AddInParameter(sqlStringCommand, "BannerId", DbType.Int32, id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public IList<BannerInfo> GetAllBanners()
        {
            IList<BannerInfo> list = new List<BannerInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(new StringBuilder("select * from  Hishop_Banner where type=1 ORDER BY DisplaySequence ASC").ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<BannerInfo>(reader);
            }
        }

        public IList<NavigateInfo> GetAllNavigate()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(new StringBuilder("select * from  Hishop_Banner where type=2 ORDER BY DisplaySequence ASC").ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<NavigateInfo>(reader);
            }
        }

        public int GetCountBanner()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(BannerId) from Hishop_Banner where type=1");
            if (this.database.ExecuteScalar(sqlStringCommand) != DBNull.Value)
            {
                return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            }
            return 0;
        }

        private int GetMaxBannerSequence()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select max(DisplaySequence) from Hishop_Banner");
            if (this.database.ExecuteScalar(sqlStringCommand) != DBNull.Value)
            {
                return (1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)));
            }
            return 1;
        }

        public TplCfgInfo GetTplCfgById(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(new StringBuilder(" select * from Hishop_Banner where BannerId=@BannerId").ToString());
            this.database.AddInParameter(sqlStringCommand, "BannerId", DbType.Int32, id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<TplCfgInfo>(reader);
            }
        }

        public bool SaveTplCfg(TplCfgInfo info)
        {
            int maxBannerSequence = this.GetMaxBannerSequence();
            StringBuilder builder = new StringBuilder("insert into  Hishop_Banner (ShortDesc,ImageUrl,DisplaySequence,LocationType,Url,Type,IsDisable)");
            builder.Append("values (@ShortDesc,@ImageUrl,@DisplaySequence,@LocationType,@Url,@Type,@IsDisable)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ShortDesc", DbType.String, info.ShortDesc);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, info.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.String, maxBannerSequence);
            this.database.AddInParameter(sqlStringCommand, "LocationType", DbType.Int32, (int) info.LocationType);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, info.Url);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, info.Type);
            this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, info.IsDisable);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateTplCfg(TplCfgInfo info)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("update Hishop_Banner set ");
            builder.Append("ShortDesc=@ShortDesc,");
            builder.Append("ImageUrl=@ImageUrl,");
            builder.Append("DisplaySequence=@DisplaySequence,");
            builder.Append("LocationType=@LocationType,");
            builder.Append("Url=@Url,");
            builder.Append("Type=@Type,");
            builder.Append("IsDisable=@IsDisable");
            builder.Append(" where BannerId=@BannerId ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "BannerId", DbType.Int32, info.Id);
            this.database.AddInParameter(sqlStringCommand, "ShortDesc", DbType.String, info.ShortDesc);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, info.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.String, info.DisplaySequence);
            this.database.AddInParameter(sqlStringCommand, "LocationType", DbType.Int32, (int) info.LocationType);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, info.Url);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, info.Type);
            this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, info.IsDisable);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

