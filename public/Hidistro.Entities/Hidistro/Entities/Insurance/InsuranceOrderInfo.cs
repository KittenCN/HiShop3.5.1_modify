namespace Hidistro.Entities.Insurance
{
    using System;
    using System.Runtime.CompilerServices;

    public class InsuranceOrderInfo
    {
        #region Model
        private int _insuranceorderid;
        private int? _insuranceordercity1;
        private string _insuranceordercity1name;
        private int? _insuranceordercity2;
        private string _insuranceordercity2name;
        private int? _insuranceordercompany;
        private string _insuranceordercompany_name;
        private string _insuranceorderjiaoqiangxian;
        private string _insuranceordershangyesanxian;
        private string _insuranceordershangyesanxianer;
        private string _insuranceordercheshangrenyuansijixian;
        private string _insuranceordercheshangrenyuansijixianer;
        private string _insuranceordercheshangrenyuanchengkexian;
        private string _insuranceordercheshangrenyuanchengkexianer;
        private string _insuranceordercheliangsunshixian;
        private string _insuranceorderdaoqiangxian;
        private string _insuranceorderbolixian;
        private string _insuranceorderziranxian;
        private string _insuranceordersheshuixian;
        private string _insuranceorderteyuexian;
        private string _insuranceordersanfangzenrenxian;
        private string _insuranceorderhuahenxian;
        private int? _insuranceordercartype;
        private string _insuranceorderjiaozhaozhengben;
        private string _insuranceorderjiaozhaofuben;
        private string _insuranceordername;
        private string _insuranceordertel;
        private string _insuranceordercardzheng;
        private string _insuranceordercardfan;
        private string _insuranceorderaddres;
        private string _insuranceordershoujianname, _insuranceorderpayid;
        private string _insuranceordershoujiantel;
        private int? _insuranceorderstatu;
        private decimal? _insuranceorderamount;
        private string _insuranceordernotes;
        private int? _insuranceuserid;
        private string _insuranceorderopenid;

        private DateTime? _insuranceordercreatdate;
        private DateTime? _insuranceorderupdatedate;
        private int? _insuranceorderisrenewal;
        /// <summary>
        /// 
        /// </summary>
        public int InsuranceOrderId
        {
            set { _insuranceorderid = value; }
            get { return _insuranceorderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? InsuranceOrderCity1
        {
            set { _insuranceordercity1 = value; }
            get { return _insuranceordercity1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderCity1Name
        {
            set { _insuranceordercity1name = value; }
            get { return _insuranceordercity1name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? InsuranceOrderCity2
        {
            set { _insuranceordercity2 = value; }
            get { return _insuranceordercity2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderCity2Name
        {
            set { _insuranceordercity2name = value; }
            get { return _insuranceordercity2name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? InsuranceOrderCompany
        {
            set { _insuranceordercompany = value; }
            get { return _insuranceordercompany; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderCompany_Name
        {
            set { _insuranceordercompany_name = value; }
            get { return _insuranceordercompany_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderJiaoQiangXian
        {
            set { _insuranceorderjiaoqiangxian = value; }
            get { return _insuranceorderjiaoqiangxian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderShangYeSanXian
        {
            set { _insuranceordershangyesanxian = value; }
            get { return _insuranceordershangyesanxian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderShangYeSanXianEr
        {
            set { _insuranceordershangyesanxianer = value; }
            get { return _insuranceordershangyesanxianer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderCheShangRenYuanSiJiXian
        {
            set { _insuranceordercheshangrenyuansijixian = value; }
            get { return _insuranceordercheshangrenyuansijixian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderCheShangRenYuanSiJiXianEr
        {
            set { _insuranceordercheshangrenyuansijixianer = value; }
            get { return _insuranceordercheshangrenyuansijixianer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderCheShangRenYuanChengKeXian
        {
            set { _insuranceordercheshangrenyuanchengkexian = value; }
            get { return _insuranceordercheshangrenyuanchengkexian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderCheShangRenYuanChengKeXianEr
        {
            set { _insuranceordercheshangrenyuanchengkexianer = value; }
            get { return _insuranceordercheshangrenyuanchengkexianer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderCheLiangSunShiXian
        {
            set { _insuranceordercheliangsunshixian = value; }
            get { return _insuranceordercheliangsunshixian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderDaoQiangXian
        {
            set { _insuranceorderdaoqiangxian = value; }
            get { return _insuranceorderdaoqiangxian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderBoliXian
        {
            set { _insuranceorderbolixian = value; }
            get { return _insuranceorderbolixian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderZiRanXian
        {
            set { _insuranceorderziranxian = value; }
            get { return _insuranceorderziranxian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderSheShuiXian
        {
            set { _insuranceordersheshuixian = value; }
            get { return _insuranceordersheshuixian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderTeYueXian
        {
            set { _insuranceorderteyuexian = value; }
            get { return _insuranceorderteyuexian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderSanFangZenRenXian
        {
            set { _insuranceordersanfangzenrenxian = value; }
            get { return _insuranceordersanfangzenrenxian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderHuaHenXian
        {
            set { _insuranceorderhuahenxian = value; }
            get { return _insuranceorderhuahenxian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? InsuranceOrderCarType
        {
            set { _insuranceordercartype = value; }
            get { return _insuranceordercartype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderJiaoZhaoZhengBen
        {
            set { _insuranceorderjiaozhaozhengben = value; }
            get { return _insuranceorderjiaozhaozhengben; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderJiaoZhaoFuBen
        {
            set { _insuranceorderjiaozhaofuben = value; }
            get { return _insuranceorderjiaozhaofuben; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderName
        {
            set { _insuranceordername = value; }
            get { return _insuranceordername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderTel
        {
            set { _insuranceordertel = value; }
            get { return _insuranceordertel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderCardZheng
        {
            set { _insuranceordercardzheng = value; }
            get { return _insuranceordercardzheng; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderCardFan
        {
            set { _insuranceordercardfan = value; }
            get { return _insuranceordercardfan; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderAddres
        {
            set { _insuranceorderaddres = value; }
            get { return _insuranceorderaddres; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderShouJIanName
        {
            set { _insuranceordershoujianname = value; }
            get { return _insuranceordershoujianname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderShouJIanTel
        {
            set { _insuranceordershoujiantel = value; }
            get { return _insuranceordershoujiantel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? InsuranceOrderStatu
        {
            set { _insuranceorderstatu = value; }
            get { return _insuranceorderstatu; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? InsuranceOrderAmount
        {
            set { _insuranceorderamount = value; }
            get { return _insuranceorderamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderNotes
        {
            set { _insuranceordernotes = value; }
            get { return _insuranceordernotes; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? InsuranceUserId
        {
            set { _insuranceuserid = value; }
            get { return _insuranceuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InsuranceOrderOpenId
        {
            set { _insuranceorderopenid = value; }
            get { return _insuranceorderopenid; }
        }

        /// <summary>
		/// 
		/// </summary>
		public DateTime? InsuranceOrderCreatDate
        {
            set { _insuranceordercreatdate = value; }
            get { return _insuranceordercreatdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? InsuranceOrderUpdateDate
        {
            set { _insuranceorderupdatedate = value; }
            get { return _insuranceorderupdatedate; }
        }

        /// <summary>
		/// 
		/// </summary>
		public int? InsuranceOrderIsRenewal
        {
            set { _insuranceorderisrenewal = value; }
            get { return _insuranceorderisrenewal; }
        }

       
        #endregion Model
    }
}

