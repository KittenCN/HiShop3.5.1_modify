namespace Hidistro.SqlDal.Insurance
{
    using Entities.CashBack;
    using Entities.Insurance;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public class InsuranceDao
    {
        private Database db = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(InsuranceOrderInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Hishop_InsuranceOrder(");
            strSql.Append("InsuranceOrderCity1,InsuranceOrderCity1Name,InsuranceOrderCity2,InsuranceOrderCity2Name,InsuranceOrderCompany,InsuranceOrderCompany_Name,InsuranceOrderJiaoQiangXian,InsuranceOrderShangYeSanXian,InsuranceOrderShangYeSanXianEr,InsuranceOrderCheShangRenYuanSiJiXian,InsuranceOrderCheShangRenYuanSiJiXianEr,InsuranceOrderCheShangRenYuanChengKeXian,InsuranceOrderCheShangRenYuanChengKeXianEr,InsuranceOrderCheLiangSunShiXian,InsuranceOrderDaoQiangXian,InsuranceOrderBoliXian,InsuranceOrderZiRanXian,InsuranceOrderSheShuiXian,InsuranceOrderTeYueXian,InsuranceOrderSanFangZenRenXian,InsuranceOrderHuaHenXian,InsuranceOrderCarType,InsuranceOrderJiaoZhaoZhengBen,InsuranceOrderJiaoZhaoFuBen,InsuranceOrderName,InsuranceOrderTel,InsuranceOrderCardZheng,InsuranceOrderCardFan,InsuranceOrderAddres,InsuranceOrderShouJIanName,InsuranceOrderShouJIanTel,InsuranceOrderStatu,InsuranceOrderAmount,InsuranceOrderNotes,InsuranceUserId,InsuranceOrderOpenId,InsuranceOrderCreatDate,InsuranceOrderUpdateDate,InsuranceOrderIsRenewal)");

            strSql.Append(" values (");
            strSql.Append("@InsuranceOrderCity1,@InsuranceOrderCity1Name,@InsuranceOrderCity2,@InsuranceOrderCity2Name,@InsuranceOrderCompany,@InsuranceOrderCompany_Name,@InsuranceOrderJiaoQiangXian,@InsuranceOrderShangYeSanXian,@InsuranceOrderShangYeSanXianEr,@InsuranceOrderCheShangRenYuanSiJiXian,@InsuranceOrderCheShangRenYuanSiJiXianEr,@InsuranceOrderCheShangRenYuanChengKeXian,@InsuranceOrderCheShangRenYuanChengKeXianEr,@InsuranceOrderCheLiangSunShiXian,@InsuranceOrderDaoQiangXian,@InsuranceOrderBoliXian,@InsuranceOrderZiRanXian,@InsuranceOrderSheShuiXian,@InsuranceOrderTeYueXian,@InsuranceOrderSanFangZenRenXian,@InsuranceOrderHuaHenXian,@InsuranceOrderCarType,@InsuranceOrderJiaoZhaoZhengBen,@InsuranceOrderJiaoZhaoFuBen,@InsuranceOrderName,@InsuranceOrderTel,@InsuranceOrderCardZheng,@InsuranceOrderCardFan,@InsuranceOrderAddres,@InsuranceOrderShouJIanName,@InsuranceOrderShouJIanTel,@InsuranceOrderStatu,@InsuranceOrderAmount,@InsuranceOrderNotes,@InsuranceUserId,@InsuranceOrderOpenId,@InsuranceOrderCreatDate,@InsuranceOrderUpdateDate,@InsuranceOrderIsRenewal)");
            strSql.Append(";select @@IDENTITY");

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "InsuranceOrderCity1", DbType.Int32, model.InsuranceOrderCity1);
            db.AddInParameter(dbCommand, "InsuranceOrderCity1Name", DbType.String, model.InsuranceOrderCity1Name);
            db.AddInParameter(dbCommand, "InsuranceOrderCity2", DbType.Int32, model.InsuranceOrderCity2);
            db.AddInParameter(dbCommand, "InsuranceOrderCity2Name", DbType.String, model.InsuranceOrderCity2Name);
            db.AddInParameter(dbCommand, "InsuranceOrderCompany", DbType.Int32, model.InsuranceOrderCompany);
            db.AddInParameter(dbCommand, "InsuranceOrderCompany_Name", DbType.String, model.InsuranceOrderCompany_Name);
            db.AddInParameter(dbCommand, "InsuranceOrderJiaoQiangXian", DbType.String, model.InsuranceOrderJiaoQiangXian);
            db.AddInParameter(dbCommand, "InsuranceOrderShangYeSanXian", DbType.String, model.InsuranceOrderShangYeSanXian);
            db.AddInParameter(dbCommand, "InsuranceOrderShangYeSanXianEr", DbType.String, model.InsuranceOrderShangYeSanXianEr);
            db.AddInParameter(dbCommand, "InsuranceOrderCheShangRenYuanSiJiXian", DbType.String, model.InsuranceOrderCheShangRenYuanSiJiXian);
            db.AddInParameter(dbCommand, "InsuranceOrderCheShangRenYuanSiJiXianEr", DbType.String, model.InsuranceOrderCheShangRenYuanSiJiXianEr);
            db.AddInParameter(dbCommand, "InsuranceOrderCheShangRenYuanChengKeXian", DbType.String, model.InsuranceOrderCheShangRenYuanChengKeXian);
            db.AddInParameter(dbCommand, "InsuranceOrderCheShangRenYuanChengKeXianEr", DbType.String, model.InsuranceOrderCheShangRenYuanChengKeXianEr);
            db.AddInParameter(dbCommand, "InsuranceOrderCheLiangSunShiXian", DbType.String, model.InsuranceOrderCheLiangSunShiXian);
            db.AddInParameter(dbCommand, "InsuranceOrderDaoQiangXian", DbType.String, model.InsuranceOrderDaoQiangXian);
            db.AddInParameter(dbCommand, "InsuranceOrderBoliXian", DbType.String, model.InsuranceOrderBoliXian);
            db.AddInParameter(dbCommand, "InsuranceOrderZiRanXian", DbType.String, model.InsuranceOrderZiRanXian);
            db.AddInParameter(dbCommand, "InsuranceOrderSheShuiXian", DbType.String, model.InsuranceOrderSheShuiXian);
            db.AddInParameter(dbCommand, "InsuranceOrderTeYueXian", DbType.String, model.InsuranceOrderTeYueXian);
            db.AddInParameter(dbCommand, "InsuranceOrderSanFangZenRenXian", DbType.String, model.InsuranceOrderSanFangZenRenXian);
            db.AddInParameter(dbCommand, "InsuranceOrderHuaHenXian", DbType.String, model.InsuranceOrderHuaHenXian);
            db.AddInParameter(dbCommand, "InsuranceOrderCarType", DbType.Int32, model.InsuranceOrderCarType);
            db.AddInParameter(dbCommand, "InsuranceOrderJiaoZhaoZhengBen", DbType.String, model.InsuranceOrderJiaoZhaoZhengBen);
            db.AddInParameter(dbCommand, "InsuranceOrderJiaoZhaoFuBen", DbType.String, model.InsuranceOrderJiaoZhaoFuBen);
            db.AddInParameter(dbCommand, "InsuranceOrderName", DbType.String, model.InsuranceOrderName);
            db.AddInParameter(dbCommand, "InsuranceOrderTel", DbType.String, model.InsuranceOrderTel);
            db.AddInParameter(dbCommand, "InsuranceOrderCardZheng", DbType.String, model.InsuranceOrderCardZheng);
            db.AddInParameter(dbCommand, "InsuranceOrderCardFan", DbType.String, model.InsuranceOrderCardFan);
            db.AddInParameter(dbCommand, "InsuranceOrderAddres", DbType.String, model.InsuranceOrderAddres);
            db.AddInParameter(dbCommand, "InsuranceOrderShouJIanName", DbType.String, model.InsuranceOrderShouJIanName);
            db.AddInParameter(dbCommand, "InsuranceOrderShouJIanTel", DbType.String, model.InsuranceOrderShouJIanTel);
            db.AddInParameter(dbCommand, "InsuranceOrderStatu", DbType.Int32, model.InsuranceOrderStatu);
            db.AddInParameter(dbCommand, "InsuranceOrderAmount", DbType.Decimal, model.InsuranceOrderAmount);
            db.AddInParameter(dbCommand, "InsuranceOrderNotes", DbType.String, model.InsuranceOrderNotes);
            db.AddInParameter(dbCommand, "InsuranceUserId", DbType.Int32, model.InsuranceUserId);
            db.AddInParameter(dbCommand, "InsuranceOrderOpenId", DbType.String, model.InsuranceOrderOpenId);
            db.AddInParameter(dbCommand, "InsuranceOrderCreatDate", DbType.String, model.InsuranceOrderCreatDate);
            db.AddInParameter(dbCommand, "InsuranceOrderUpdateDate", DbType.String, model.InsuranceOrderUpdateDate);
            db.AddInParameter(dbCommand, "InsuranceOrderIsRenewal", DbType.Int32, model.InsuranceOrderIsRenewal);
            int result;
            object obj = db.ExecuteScalar(dbCommand);
            if (!int.TryParse(obj.ToString(), out result))
            {
                return 0;
            }
            return result;
        }


        /// <summary>
		/// 得到一个对象实体
		/// </summary>
		public InsuranceOrderInfo GetModel(int InsuranceOrderId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select InsuranceOrderId,InsuranceOrderCity1,InsuranceOrderCity1Name,InsuranceOrderCity2,InsuranceOrderCity2Name,InsuranceOrderCompany,InsuranceOrderCompany_Name,InsuranceOrderJiaoQiangXian,InsuranceOrderShangYeSanXian,InsuranceOrderShangYeSanXianEr,InsuranceOrderCheShangRenYuanSiJiXian,InsuranceOrderCheShangRenYuanSiJiXianEr,InsuranceOrderCheShangRenYuanChengKeXian,InsuranceOrderCheShangRenYuanChengKeXianEr,InsuranceOrderCheLiangSunShiXian,InsuranceOrderDaoQiangXian,InsuranceOrderBoliXian,InsuranceOrderZiRanXian,InsuranceOrderSheShuiXian,InsuranceOrderTeYueXian,InsuranceOrderSanFangZenRenXian,InsuranceOrderHuaHenXian,InsuranceOrderCarType,InsuranceOrderJiaoZhaoZhengBen,InsuranceOrderJiaoZhaoFuBen,InsuranceOrderName,InsuranceOrderTel,InsuranceOrderCardZheng,InsuranceOrderCardFan,InsuranceOrderAddres,InsuranceOrderShouJIanName,InsuranceOrderShouJIanTel,InsuranceOrderStatu,InsuranceOrderAmount,InsuranceOrderNotes,InsuranceUserId,InsuranceOrderOpenId,InsuranceOrderCreatDate,InsuranceOrderUpdateDate,InsuranceOrderIsRenewal from Hishop_InsuranceOrder ");
            strSql.Append(" where InsuranceOrderId=@InsuranceOrderId ");
           
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "InsuranceOrderId", DbType.Int32, InsuranceOrderId);
            InsuranceOrderInfo model = null;
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }


        /// <summary>
		/// 对象实体绑定数据
		/// </summary>
		public InsuranceOrderInfo ReaderBind(IDataReader dataReader)
        {
            InsuranceOrderInfo model = new InsuranceOrderInfo();
            object ojb;
            ojb = dataReader["InsuranceOrderId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InsuranceOrderId = (int)ojb;
            }
            ojb = dataReader["InsuranceOrderCity1"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InsuranceOrderCity1 = (int)ojb;
            }
            model.InsuranceOrderCity1Name = dataReader["InsuranceOrderCity1Name"].ToString();
            ojb = dataReader["InsuranceOrderCity2"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InsuranceOrderCity2 = (int)ojb;
            }
            model.InsuranceOrderCity2Name = dataReader["InsuranceOrderCity2Name"].ToString();
            ojb = dataReader["InsuranceOrderCompany"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InsuranceOrderCompany = (int)ojb;
            }
            model.InsuranceOrderCompany_Name = dataReader["InsuranceOrderCompany_Name"].ToString();
            model.InsuranceOrderJiaoQiangXian = dataReader["InsuranceOrderJiaoQiangXian"].ToString();
            model.InsuranceOrderShangYeSanXian = dataReader["InsuranceOrderShangYeSanXian"].ToString();
            model.InsuranceOrderShangYeSanXianEr = dataReader["InsuranceOrderShangYeSanXianEr"].ToString();
            model.InsuranceOrderCheShangRenYuanSiJiXian = dataReader["InsuranceOrderCheShangRenYuanSiJiXian"].ToString();
            model.InsuranceOrderCheShangRenYuanSiJiXianEr = dataReader["InsuranceOrderCheShangRenYuanSiJiXianEr"].ToString();
            model.InsuranceOrderCheShangRenYuanChengKeXian = dataReader["InsuranceOrderCheShangRenYuanChengKeXian"].ToString();
            model.InsuranceOrderCheShangRenYuanChengKeXianEr = dataReader["InsuranceOrderCheShangRenYuanChengKeXianEr"].ToString();
            model.InsuranceOrderCheLiangSunShiXian = dataReader["InsuranceOrderCheLiangSunShiXian"].ToString();
            model.InsuranceOrderDaoQiangXian = dataReader["InsuranceOrderDaoQiangXian"].ToString();
            model.InsuranceOrderBoliXian = dataReader["InsuranceOrderBoliXian"].ToString();
            model.InsuranceOrderZiRanXian = dataReader["InsuranceOrderZiRanXian"].ToString();
            model.InsuranceOrderSheShuiXian = dataReader["InsuranceOrderSheShuiXian"].ToString();
            model.InsuranceOrderTeYueXian = dataReader["InsuranceOrderTeYueXian"].ToString();
            model.InsuranceOrderSanFangZenRenXian = dataReader["InsuranceOrderSanFangZenRenXian"].ToString();
            model.InsuranceOrderHuaHenXian = dataReader["InsuranceOrderHuaHenXian"].ToString();
            ojb = dataReader["InsuranceOrderCarType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InsuranceOrderCarType = (int)ojb;
            }
            model.InsuranceOrderJiaoZhaoZhengBen = dataReader["InsuranceOrderJiaoZhaoZhengBen"].ToString();
            model.InsuranceOrderJiaoZhaoFuBen = dataReader["InsuranceOrderJiaoZhaoFuBen"].ToString();
            model.InsuranceOrderName = dataReader["InsuranceOrderName"].ToString();
            model.InsuranceOrderTel = dataReader["InsuranceOrderTel"].ToString();
            model.InsuranceOrderCardZheng = dataReader["InsuranceOrderCardZheng"].ToString();
            model.InsuranceOrderCardFan = dataReader["InsuranceOrderCardFan"].ToString();
            model.InsuranceOrderAddres = dataReader["InsuranceOrderAddres"].ToString();
            model.InsuranceOrderShouJIanName = dataReader["InsuranceOrderShouJIanName"].ToString();
            model.InsuranceOrderShouJIanTel = dataReader["InsuranceOrderShouJIanTel"].ToString();
            ojb = dataReader["InsuranceOrderStatu"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InsuranceOrderStatu = (int)ojb;
            }
            ojb = dataReader["InsuranceOrderAmount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InsuranceOrderAmount = (decimal)ojb;
            }
            model.InsuranceOrderNotes = dataReader["InsuranceOrderNotes"].ToString();
            ojb = dataReader["InsuranceUserId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InsuranceUserId = (int)ojb;
            }
            model.InsuranceOrderOpenId = dataReader["InsuranceOrderOpenId"].ToString();
            ojb = dataReader["InsuranceOrderCreatDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InsuranceOrderCreatDate = (DateTime)ojb;
            }
            ojb = dataReader["InsuranceOrderUpdateDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InsuranceOrderUpdateDate = (DateTime)ojb;
            }
            ojb = dataReader["InsuranceOrderIsRenewal"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.InsuranceOrderIsRenewal = (int)ojb;
            }
            return model;
        }






        /// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(InsuranceOrderInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Hishop_InsuranceOrder set ");
            strSql.Append("InsuranceOrderCity1=@InsuranceOrderCity1,");
            strSql.Append("InsuranceOrderCity1Name=@InsuranceOrderCity1Name,");
            strSql.Append("InsuranceOrderCity2=@InsuranceOrderCity2,");
            strSql.Append("InsuranceOrderCity2Name=@InsuranceOrderCity2Name,");
            strSql.Append("InsuranceOrderCompany=@InsuranceOrderCompany,");
            strSql.Append("InsuranceOrderCompany_Name=@InsuranceOrderCompany_Name,");
            strSql.Append("InsuranceOrderJiaoQiangXian=@InsuranceOrderJiaoQiangXian,");
            strSql.Append("InsuranceOrderShangYeSanXian=@InsuranceOrderShangYeSanXian,");
            strSql.Append("InsuranceOrderShangYeSanXianEr=@InsuranceOrderShangYeSanXianEr,");
            strSql.Append("InsuranceOrderCheShangRenYuanSiJiXian=@InsuranceOrderCheShangRenYuanSiJiXian,");
            strSql.Append("InsuranceOrderCheShangRenYuanSiJiXianEr=@InsuranceOrderCheShangRenYuanSiJiXianEr,");
            strSql.Append("InsuranceOrderCheShangRenYuanChengKeXian=@InsuranceOrderCheShangRenYuanChengKeXian,");
            strSql.Append("InsuranceOrderCheShangRenYuanChengKeXianEr=@InsuranceOrderCheShangRenYuanChengKeXianEr,");
            strSql.Append("InsuranceOrderCheLiangSunShiXian=@InsuranceOrderCheLiangSunShiXian,");
            strSql.Append("InsuranceOrderDaoQiangXian=@InsuranceOrderDaoQiangXian,");
            strSql.Append("InsuranceOrderBoliXian=@InsuranceOrderBoliXian,");
            strSql.Append("InsuranceOrderZiRanXian=@InsuranceOrderZiRanXian,");
            strSql.Append("InsuranceOrderSheShuiXian=@InsuranceOrderSheShuiXian,");
            strSql.Append("InsuranceOrderTeYueXian=@InsuranceOrderTeYueXian,");
            strSql.Append("InsuranceOrderSanFangZenRenXian=@InsuranceOrderSanFangZenRenXian,");
            strSql.Append("InsuranceOrderHuaHenXian=@InsuranceOrderHuaHenXian,");
            strSql.Append("InsuranceOrderCarType=@InsuranceOrderCarType,");
            strSql.Append("InsuranceOrderJiaoZhaoZhengBen=@InsuranceOrderJiaoZhaoZhengBen,");
            strSql.Append("InsuranceOrderJiaoZhaoFuBen=@InsuranceOrderJiaoZhaoFuBen,");
            strSql.Append("InsuranceOrderName=@InsuranceOrderName,");
            strSql.Append("InsuranceOrderTel=@InsuranceOrderTel,");
            strSql.Append("InsuranceOrderCardZheng=@InsuranceOrderCardZheng,");
            strSql.Append("InsuranceOrderCardFan=@InsuranceOrderCardFan,");
            strSql.Append("InsuranceOrderAddres=@InsuranceOrderAddres,");
            strSql.Append("InsuranceOrderShouJIanName=@InsuranceOrderShouJIanName,");
            strSql.Append("InsuranceOrderShouJIanTel=@InsuranceOrderShouJIanTel,");
            strSql.Append("InsuranceOrderStatu=@InsuranceOrderStatu,");
            strSql.Append("InsuranceOrderAmount=@InsuranceOrderAmount,");
            strSql.Append("InsuranceOrderNotes=@InsuranceOrderNotes,");
            strSql.Append("InsuranceUserId=@InsuranceUserId,");
            strSql.Append("InsuranceOrderOpenId=@InsuranceOrderOpenId,");
            strSql.Append("InsuranceOrderCreatDate=@InsuranceOrderCreatDate,");
            strSql.Append("InsuranceOrderUpdateDate=@InsuranceOrderUpdateDate,");
            strSql.Append("InsuranceOrderIsRenewal=@InsuranceOrderIsRenewal");
            strSql.Append(" where InsuranceOrderId=@InsuranceOrderId ");
          
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "InsuranceOrderId", DbType.Int32, model.InsuranceOrderId);
            db.AddInParameter(dbCommand, "InsuranceOrderCity1", DbType.Int32, model.InsuranceOrderCity1);
            db.AddInParameter(dbCommand, "InsuranceOrderCity1Name", DbType.String, model.InsuranceOrderCity1Name);
            db.AddInParameter(dbCommand, "InsuranceOrderCity2", DbType.Int32, model.InsuranceOrderCity2);
            db.AddInParameter(dbCommand, "InsuranceOrderCity2Name", DbType.String, model.InsuranceOrderCity2Name);
            db.AddInParameter(dbCommand, "InsuranceOrderCompany", DbType.Int32, model.InsuranceOrderCompany);
            db.AddInParameter(dbCommand, "InsuranceOrderCompany_Name", DbType.String, model.InsuranceOrderCompany_Name);
            db.AddInParameter(dbCommand, "InsuranceOrderJiaoQiangXian", DbType.String, model.InsuranceOrderJiaoQiangXian);
            db.AddInParameter(dbCommand, "InsuranceOrderShangYeSanXian", DbType.String, model.InsuranceOrderShangYeSanXian);
            db.AddInParameter(dbCommand, "InsuranceOrderShangYeSanXianEr", DbType.String, model.InsuranceOrderShangYeSanXianEr);
            db.AddInParameter(dbCommand, "InsuranceOrderCheShangRenYuanSiJiXian", DbType.String, model.InsuranceOrderCheShangRenYuanSiJiXian);
            db.AddInParameter(dbCommand, "InsuranceOrderCheShangRenYuanSiJiXianEr", DbType.String, model.InsuranceOrderCheShangRenYuanSiJiXianEr);
            db.AddInParameter(dbCommand, "InsuranceOrderCheShangRenYuanChengKeXian", DbType.String, model.InsuranceOrderCheShangRenYuanChengKeXian);
            db.AddInParameter(dbCommand, "InsuranceOrderCheShangRenYuanChengKeXianEr", DbType.String, model.InsuranceOrderCheShangRenYuanChengKeXianEr);
            db.AddInParameter(dbCommand, "InsuranceOrderCheLiangSunShiXian", DbType.String, model.InsuranceOrderCheLiangSunShiXian);
            db.AddInParameter(dbCommand, "InsuranceOrderDaoQiangXian", DbType.String, model.InsuranceOrderDaoQiangXian);
            db.AddInParameter(dbCommand, "InsuranceOrderBoliXian", DbType.String, model.InsuranceOrderBoliXian);
            db.AddInParameter(dbCommand, "InsuranceOrderZiRanXian", DbType.String, model.InsuranceOrderZiRanXian);
            db.AddInParameter(dbCommand, "InsuranceOrderSheShuiXian", DbType.String, model.InsuranceOrderSheShuiXian);
            db.AddInParameter(dbCommand, "InsuranceOrderTeYueXian", DbType.String, model.InsuranceOrderTeYueXian);
            db.AddInParameter(dbCommand, "InsuranceOrderSanFangZenRenXian", DbType.String, model.InsuranceOrderSanFangZenRenXian);
            db.AddInParameter(dbCommand, "InsuranceOrderHuaHenXian", DbType.String, model.InsuranceOrderHuaHenXian);
            db.AddInParameter(dbCommand, "InsuranceOrderCarType", DbType.Int32, model.InsuranceOrderCarType);
            db.AddInParameter(dbCommand, "InsuranceOrderJiaoZhaoZhengBen", DbType.String, model.InsuranceOrderJiaoZhaoZhengBen);
            db.AddInParameter(dbCommand, "InsuranceOrderJiaoZhaoFuBen", DbType.String, model.InsuranceOrderJiaoZhaoFuBen);
            db.AddInParameter(dbCommand, "InsuranceOrderName", DbType.String, model.InsuranceOrderName);
            db.AddInParameter(dbCommand, "InsuranceOrderTel", DbType.String, model.InsuranceOrderTel);
            db.AddInParameter(dbCommand, "InsuranceOrderCardZheng", DbType.String, model.InsuranceOrderCardZheng);
            db.AddInParameter(dbCommand, "InsuranceOrderCardFan", DbType.String, model.InsuranceOrderCardFan);
            db.AddInParameter(dbCommand, "InsuranceOrderAddres", DbType.String, model.InsuranceOrderAddres);
            db.AddInParameter(dbCommand, "InsuranceOrderShouJIanName", DbType.String, model.InsuranceOrderShouJIanName);
            db.AddInParameter(dbCommand, "InsuranceOrderShouJIanTel", DbType.String, model.InsuranceOrderShouJIanTel);
            db.AddInParameter(dbCommand, "InsuranceOrderStatu", DbType.Int32, model.InsuranceOrderStatu);
            db.AddInParameter(dbCommand, "InsuranceOrderAmount", DbType.Decimal, model.InsuranceOrderAmount);
            db.AddInParameter(dbCommand, "InsuranceOrderNotes", DbType.String, model.InsuranceOrderNotes);
            db.AddInParameter(dbCommand, "InsuranceUserId", DbType.Int32, model.InsuranceUserId);
            db.AddInParameter(dbCommand, "InsuranceOrderOpenId", DbType.String, model.InsuranceOrderOpenId);
            db.AddInParameter(dbCommand, "InsuranceOrderCreatDate", DbType.String, model.InsuranceOrderCreatDate);
            db.AddInParameter(dbCommand, "InsuranceOrderUpdateDate", DbType.String, model.InsuranceOrderUpdateDate);
            db.AddInParameter(dbCommand, "InsuranceOrderIsRenewal", DbType.Int32, model.InsuranceOrderIsRenewal);
            int rows = db.ExecuteNonQuery(dbCommand);

            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int InsuranceOrderId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Hishop_InsuranceOrder ");
            strSql.Append(" where InsuranceOrderId=@InsuranceOrderId ");
          
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "InsuranceOrderId", DbType.Int32, InsuranceOrderId);
            int rows = db.ExecuteNonQuery(dbCommand);

            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DbQueryResult GetInsuranceOrdeByPager(InsuranceOrderQuery query)
        {
            string selectFields = "InsuranceOrderId,InsuranceOrderCity1Name,InsuranceOrderIsRenewal,InsuranceOrderCarType,InsuranceOrderCity2Name,InsuranceOrderCompany_Name,InsuranceOrderTel,InsuranceOrderName,InsuranceOrderStatu,InsuranceOrderCreatDate,InsuranceOrderAmount";
            string table = "Hishop_InsuranceOrder";
            string pk = "InsuranceOrderId";
            string filter = " 1=1 ";
           
            if (!string.IsNullOrWhiteSpace(query.UserName))
            {
                filter = filter + string.Format(" AND InsuranceOrderName LIKE '%{0}%'", query.UserName);
            }

            if (!string.IsNullOrWhiteSpace(query.OpenId))
            {
                filter = filter + string.Format(" AND InsuranceOrderOpenId = '{0}'", query.OpenId);
            }
            if (!string.IsNullOrWhiteSpace(query.Cellphone))
            {
                filter = filter + string.Format(" AND InsuranceOrderTel LIKE '%{0}%'", query.Cellphone);
            }
            if (query.CashBackTypes.HasValue)
            {
                filter = filter + string.Format(" AND InsuranceOrderStatu = {0}", (int)query.CashBackTypes.Value);
            }
          
           
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, pk, filter, selectFields);
        }



    }
}

