namespace Hidistro.Entities.Orders
{
    using Hidistro.Entities.Promotions;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class OrderTmpInfo
    {
        public OrderTmpInfo()
        { }
        #region Model
        private string _orderid="";
        private string _ordermarking = "";
        private string _remark = "";
        private int? _managermark = 0;
        private string _managerremark;
        private decimal? _adjusteddiscount=0;
        private int _orderstatus=0;
        private string _closereason = "";
        private DateTime _orderdate= DateTime.Now;
        private DateTime? _paydate = DateTime.Now;
        private DateTime? _shippingdate = DateTime.Now;
        private DateTime? _finishdate = DateTime.Now;
        private int _userid=0;
        private string _username = "";
        private string _emailaddress = "";
        private string _realname = "";
        private string _qq = "";
        private string _wangwang = "";
        private string _msn = "";
        private string _shippingregion = "";
        private string _address = "";
        private string _zipcode = "";
        private string _shipto = "";
        private string _telphone = "";
        private string _cellphone = "";
        private string _shiptodate = "";
        private int? _shippingmodeid=0;
        private string _modename = "";
        private int? _realshippingmodeid=0;
        private string _realmodename = "";
        private int? _regionid=0;
        private decimal? _freight=0;
        private decimal? _adjustedfreight=0;
        private string _shipordernumber = "";
        private decimal? _weight=0;
        private string _expresscompanyname = "";
        private string _expresscompanyabb = "";
        private int _paymenttypeid=0;
        private string _paymenttype = "";
        private decimal? _paycharge=0;
        private int? _refundstatus=0;
        private decimal? _refundamount=0;
        private string _refundremark = "";
        private string _gateway = "";
        private decimal? _ordertotal=0;
        private int? _orderpoint=0;
        private decimal? _ordercostprice = 0;
        private decimal? _orderprofit = 0;
        private decimal? _actualfreight = 0;
        private decimal? _othercost = 0;
        private decimal? _optionprice = 0;
        private decimal? _amount = 0;
        private decimal? _discountamount = 0M;
        private string _activitiesid="";
        private string _activitiesname="";
        private int? _reducedpromotionid = 0;
        private string _reducedpromotionname="";
        private decimal? _reducedpromotionamount=0;
        private bool _isreduced=false;
        private int? _senttimespointpromotionid=0;
        private string _senttimespointpromotionname = "";
        private decimal? _timespoint = 0;
        private bool _issendtimespoint = false ;
        private int? _freightfreepromotionid = 0;
        private string _freightfreepromotionname = "";
        private bool _isfreightfree = false;
        private string _couponname = "";
        private string _couponcode = "";
        private decimal? _couponamount = 0;
        private decimal? _couponvalue = 0;
        private int? _groupbuyid = 0;
        private decimal? _needprice = 0;
        private int? _groupbuystatus = 0;
        private int? _countdownbuyid = 0;
        private int? _bundlingid = 0;
        private int? _bundlingnum = 0;
        private decimal? _bundlingprice = 0;
        private string _gatewayorderid = "";
        private bool _isprinted = false;
        private decimal? _tax = 0;
        private string _invoicetitle = "";
        private string _sender = "";
        private int? _referraluserid = 0;
        private decimal? _firstcommission = 0M;
        private decimal? _secondcommission = 0M;
        private decimal? _thirdcommission = 0M;
        private string _redpageractivityname = "";
        private int? _redpagerid = 0;
        private decimal? _redpagerorderamountcanuse = 0M;
        private decimal? _redpageramount = 0M;
        private string _oldaddress = "";
        private decimal _pointtocash = 0M;
        private int _pointexchange = 0;
        private int _splitstate = 0;
        private int _deletebeforestate = 0;
        private int _clientshorttype = 0;
        private string _referralpath = "";
        private int _bargaindetialid = 0;
        private decimal _balancepaymoneytotal = 0M;
        private decimal _balancepayfreightmoneytotal = 0M;
        private decimal _couponfreightmoneytotal = 0M;
        private DateTime _updatedate = DateTime.Now;
        private int _logisticstools = 0;
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 多个订单标示
        /// </summary>
        public string OrderMarking
        {
            set { _ordermarking = value; }
            get { return _ordermarking; }
        }
        /// <summary>
        /// 买家留言
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 管理员标记
        /// </summary>
        public int? ManagerMark
        {
            set { _managermark = value; }
            get { return _managermark; }
        }
        /// <summary>
        /// 管理员备注
        /// </summary>
        public string ManagerRemark
        {
            set { _managerremark = value; }
            get { return _managerremark; }
        }
        /// <summary>
        /// 订单折扣
        /// </summary>
        public decimal? AdjustedDiscount
        {
            set { _adjusteddiscount = value; }
            get { return _adjusteddiscount; }
        }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus
        {
            set { _orderstatus = value; }
            get { return _orderstatus; }
        }
        /// <summary>
        /// 订单关闭原因
        /// </summary>
        public string CloseReason
        {
            set { _closereason = value; }
            get { return _closereason; }
        }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderDate
        {
            set { _orderdate = value; }
            get { return _orderdate; }
        }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayDate
        {
            set { _paydate = value; }
            get { return _paydate; }
        }
        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime? ShippingDate
        {
            set { _shippingdate = value; }
            get { return _shippingdate; }
        }
        /// <summary>
        /// 交易完成时间
        /// </summary>
        public DateTime? FinishDate
        {
            set { _finishdate = value; }
            get { return _finishdate; }
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 电子邮件地址
        /// </summary>
        public string EmailAddress
        {
            set { _emailaddress = value; }
            get { return _emailaddress; }
        }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName
        {
            set { _realname = value; }
            get { return _realname; }
        }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ
        {
            set { _qq = value; }
            get { return _qq; }
        }
        /// <summary>
        /// 旺旺
        /// </summary>
        public string Wangwang
        {
            set { _wangwang = value; }
            get { return _wangwang; }
        }
        /// <summary>
        /// MSN
        /// </summary>
        public string MSN
        {
            set { _msn = value; }
            get { return _msn; }
        }
        /// <summary>
        /// 配送区域
        /// </summary>
        public string ShippingRegion
        {
            set { _shippingregion = value; }
            get { return _shippingregion; }
        }
        /// <summary>
        /// 街道地址
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode
        {
            set { _zipcode = value; }
            get { return _zipcode; }
        }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ShipTo
        {
            set { _shipto = value; }
            get { return _shipto; }
        }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string TelPhone
        {
            set { _telphone = value; }
            get { return _telphone; }
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string CellPhone
        {
            set { _cellphone = value; }
            get { return _cellphone; }
        }
        /// <summary>
        /// 收货时期段
        /// </summary>
        public string ShipToDate
        {
            set { _shiptodate = value; }
            get { return _shiptodate; }
        }
        /// <summary>
        /// 配送方式
        /// </summary>
        public int? ShippingModeId
        {
            set { _shippingmodeid = value; }
            get { return _shippingmodeid; }
        }
        /// <summary>
        /// 配送方式名称
        /// </summary>
        public string ModeName
        {
            set { _modename = value; }
            get { return _modename; }
        }
        /// <summary>
        /// 实际配送方式
        /// </summary>
        public int? RealShippingModeId
        {
            set { _realshippingmodeid = value; }
            get { return _realshippingmodeid; }
        }
        /// <summary>
        /// 实际配送方式名称
        /// </summary>
        public string RealModeName
        {
            set { _realmodename = value; }
            get { return _realmodename; }
        }
        /// <summary>
        /// 配送区域
        /// </summary>
        public int? RegionId
        {
            set { _regionid = value; }
            get { return _regionid; }
        }
        /// <summary>
        /// 原邮费
        /// </summary>
        public decimal? Freight
        {
            set { _freight = value; }
            get { return _freight; }
        }
        /// <summary>
        /// 实际邮费
        /// </summary>
        public decimal? AdjustedFreight
        {
            set { _adjustedfreight = value; }
            get { return _adjustedfreight; }
        }
        /// <summary>
        /// 发货单号
        /// </summary>
        public string ShipOrderNumber
        {
            set { _shipordernumber = value; }
            get { return _shipordernumber; }
        }
        /// <summary>
        /// 订单货物总重量
        /// </summary>
        public decimal? Weight
        {
            set { _weight = value; }
            get { return _weight; }
        }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string ExpressCompanyName
        {
            set { _expresscompanyname = value; }
            get { return _expresscompanyname; }
        }
        /// <summary>
        /// 快递公司缩写
        /// </summary>
        public string ExpressCompanyAbb
        {
            set { _expresscompanyabb = value; }
            get { return _expresscompanyabb; }
        }
        /// <summary>
        /// 支付方式编号
        /// </summary>
        public int PaymentTypeId
        {
            set { _paymenttypeid = value; }
            get { return _paymenttypeid; }
        }
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string PaymentType
        {
            set { _paymenttype = value; }
            get { return _paymenttype; }
        }
        /// <summary>
        /// 原支付手续费
        /// </summary>
        public decimal? PayCharge
        {
            set { _paycharge = value; }
            get { return _paycharge; }
        }
        /// <summary>
        /// 退款状态
        /// </summary>
        public int? RefundStatus
        {
            set { _refundstatus = value; }
            get { return _refundstatus; }
        }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal? RefundAmount
        {
            set { _refundamount = value; }
            get { return _refundamount; }
        }
        /// <summary>
        /// 退款说明
        /// </summary>
        public string RefundRemark
        {
            set { _refundremark = value; }
            get { return _refundremark; }
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string Gateway
        {
            set { _gateway = value; }
            get { return _gateway; }
        }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal? OrderTotal
        {
            set { _ordertotal = value; }
            get { return _ordertotal; }
        }
        /// <summary>
        /// 订单总积分
        /// </summary>
        public int? OrderPoint
        {
            set { _orderpoint = value; }
            get { return _orderpoint; }
        }
        /// <summary>
        /// 订单总成本
        /// </summary>
        public decimal? OrderCostPrice
        {
            set { _ordercostprice = value; }
            get { return _ordercostprice; }
        }
        /// <summary>
        /// 订单利润（订单总金额 - 订单总成本）
        /// </summary>
        public decimal? OrderProfit
        {
            set { _orderprofit = value; }
            get { return _orderprofit; }
        }
        /// <summary>
        /// 实际运费（预留给管理员填写，以便更精确的计算订单总成本）
        /// </summary>
        public decimal? ActualFreight
        {
            set { _actualfreight = value; }
            get { return _actualfreight; }
        }
        /// <summary>
        /// 其他成本（预留给管理员填写，以便更精确的计算订单总成本）
        /// </summary>
        public decimal? OtherCost
        {
            set { _othercost = value; }
            get { return _othercost; }
        }
        /// <summary>
        /// 订单可选项金额
        /// </summary>
        public decimal? OptionPrice
        {
            set { _optionprice = value; }
            get { return _optionprice; }
        }
        /// <summary>
        /// 商品总金额
        /// </summary>
        public decimal? Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 满减活动减免总金额
        /// </summary>
        public decimal? DiscountAmount
        {
            set { _discountamount = value; }
            get { return _discountamount; }
        }
        /// <summary>
        /// 满减活动ID
        /// </summary>
        public string ActivitiesId
        {
            set { _activitiesid = value; }
            get { return _activitiesid; }
        }
        /// <summary>
        /// 满减活动名称
        /// </summary>
        public string ActivitiesName
        {
            set { _activitiesname = value; }
            get { return _activitiesname; }
        }
        /// <summary>
        /// 满额或满量优惠的促销编号
        /// </summary>
        public int? ReducedPromotionId
        {
            set { _reducedpromotionid = value; }
            get { return _reducedpromotionid; }
        }
        /// <summary>
        /// 满额或满量优惠的促销名称
        /// </summary>
        public string ReducedPromotionName
        {
            set { _reducedpromotionname = value; }
            get { return _reducedpromotionname; }
        }
        /// <summary>
        /// 满额或满量优惠的金额
        /// </summary>
        public decimal? ReducedPromotionAmount
        {
            set { _reducedpromotionamount = value; }
            get { return _reducedpromotionamount; }
        }
        /// <summary>
        /// 是否有满额或满量优惠
        /// </summary>
        public bool IsReduced
        {
            set { _isreduced = value; }
            get { return _isreduced; }
        }
        /// <summary>
        /// 满额送倍数积分的促销编号
        /// </summary>
        public int? SentTimesPointPromotionId
        {
            set { _senttimespointpromotionid = value; }
            get { return _senttimespointpromotionid; }
        }
        /// <summary>
        /// 满额送倍数积分的促销名称
        /// </summary>
        public string SentTimesPointPromotionName
        {
            set { _senttimespointpromotionname = value; }
            get { return _senttimespointpromotionname; }
        }
        /// <summary>
        /// 积分倍数
        /// </summary>
        public decimal? TimesPoint
        {
            set { _timespoint = value; }
            get { return _timespoint; }
        }
        /// <summary>
        /// 是否有送倍数积分的促销
        /// </summary>
        public bool IsSendTimesPoint
        {
            set { _issendtimespoint = value; }
            get { return _issendtimespoint; }
        }
        /// <summary>
        /// 额免运费的促销编号
        /// </summary>
        public int? FreightFreePromotionId
        {
            set { _freightfreepromotionid = value; }
            get { return _freightfreepromotionid; }
        }
        /// <summary>
        /// 满额免运费的促销名称
        /// </summary>
        public string FreightFreePromotionName
        {
            set { _freightfreepromotionname = value; }
            get { return _freightfreepromotionname; }
        }
        /// <summary>
        /// 是否有免运费的促销
        /// </summary>
        public bool IsFreightFree
        {
            set { _isfreightfree = value; }
            get { return _isfreightfree; }
        }
        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string CouponName
        {
            set { _couponname = value; }
            get { return _couponname; }
        }
        /// <summary>
        /// 优惠券号码
        /// </summary>
        public string CouponCode
        {
            set { _couponcode = value; }
            get { return _couponcode; }
        }
        /// <summary>
        /// 优惠券满足金额
        /// </summary>
        public decimal? CouponAmount
        {
            set { _couponamount = value; }
            get { return _couponamount; }
        }
        /// <summary>
        /// 优惠券金额
        /// </summary>
        public decimal? CouponValue
        {
            set { _couponvalue = value; }
            get { return _couponvalue; }
        }
        /// <summary>
        /// 团购活动ID
        /// </summary>
        public int? GroupBuyId
        {
            set { _groupbuyid = value; }
            get { return _groupbuyid; }
        }
        /// <summary>
        /// 团购保证金额
        /// </summary>
        public decimal? NeedPrice
        {
            set { _needprice = value; }
            get { return _needprice; }
        }
        /// <summary>
        /// 团购状态
        /// </summary>
        public int? GroupBuyStatus
        {
            set { _groupbuystatus = value; }
            get { return _groupbuystatus; }
        }
        /// <summary>
        /// 限时抢购ID
        /// </summary>
        public int? CountDownBuyId
        {
            set { _countdownbuyid = value; }
            get { return _countdownbuyid; }
        }
        /// <summary>
        /// 捆绑商品ID
        /// </summary>
        public int? BundlingId
        {
            set { _bundlingid = value; }
            get { return _bundlingid; }
        }
        /// <summary>
        /// 捆绑商品数量
        /// </summary>
        public int? BundlingNum
        {
            set { _bundlingnum = value; }
            get { return _bundlingnum; }
        }
        /// <summary>
        /// 捆绑价格
        /// </summary>
        public decimal? BundlingPrice
        {
            set { _bundlingprice = value; }
            get { return _bundlingprice; }
        }
        /// <summary>
        /// 第三方支付公司的交易编号
        /// </summary>
        public string GatewayOrderId
        {
            set { _gatewayorderid = value; }
            get { return _gatewayorderid; }
        }
        /// <summary>
        /// 是否已经打印
        /// </summary>
        public bool IsPrinted
        {
            set { _isprinted = value; }
            get { return _isprinted; }
        }
        /// <summary>
        /// 税金
        /// </summary>
        public decimal? Tax
        {
            set { _tax = value; }
            get { return _tax; }
        }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string InvoiceTitle
        {
            set { _invoicetitle = value; }
            get { return _invoicetitle; }
        }
        /// <summary>
        /// 发货人
        /// </summary>
        public string Sender
        {
            set { _sender = value; }
            get { return _sender; }
        }
        /// <summary>
        /// 订单来源（分销商）
        /// </summary>
        public int? ReferralUserId
        {
            set { _referraluserid = value; }
            get { return _referraluserid; }
        }
        /// <summary>
        /// 上二级分销商佣金
        /// </summary>
        public decimal? FirstCommission
        {
            set { _firstcommission = value; }
            get { return _firstcommission; }
        }
        /// <summary>
        /// 上一级分销商佣金
        /// </summary>
        public decimal? SecondCommission
        {
            set { _secondcommission = value; }
            get { return _secondcommission; }
        }
        /// <summary>
        /// 本店佣金
        /// </summary>
        public decimal? ThirdCommission
        {
            set { _thirdcommission = value; }
            get { return _thirdcommission; }
        }
        /// <summary>
        /// 抵用券名称
        /// </summary>
        public string RedPagerActivityName
        {
            set { _redpageractivityname = value; }
            get { return _redpageractivityname; }
        }
        /// <summary>
        /// 抵用券ID
        /// </summary>
        public int? RedPagerID
        {
            set { _redpagerid = value; }
            get { return _redpagerid; }
        }
        /// <summary>
        /// 抵用券满足金额
        /// </summary>
        public decimal? RedPagerOrderAmountCanUse
        {
            set { _redpagerorderamountcanuse = value; }
            get { return _redpagerorderamountcanuse; }
        }
        /// <summary>
        /// 抵用券金额
        /// </summary>
        public decimal? RedPagerAmount
        {
            set { _redpageramount = value; }
            get { return _redpageramount; }
        }
        /// <summary>
        /// 旧地址
        /// </summary>
        public string OldAddress
        {
            set { _oldaddress = value; }
            get { return _oldaddress; }
        }
        /// <summary>
        /// 积分抵现金额
        /// </summary>
        public decimal PointToCash
        {
            set { _pointtocash = value; }
            get { return _pointtocash; }
        }
        /// <summary>
        /// 积分抵现点
        /// </summary>
        public int PointExchange
        {
            set { _pointexchange = value; }
            get { return _pointexchange; }
        }
        /// <summary>
        /// 拆分状态，0未拆分，1已拆分主订单，2已拆分从订单
        /// </summary>
        public int SplitState
        {
            set { _splitstate = value; }
            get { return _splitstate; }
        }
        /// <summary>
        /// 订单删除前的状态
        /// </summary>
        public int DeleteBeforeState
        {
            set { _deletebeforestate = value; }
            get { return _deletebeforestate; }
        }
        /// <summary>
        /// 客户端如支付宝服务窗2微信1,其他或者未知的浏览器：0
        /// </summary>
        public int ClientShortType
        {
            set { _clientshorttype = value; }
            get { return _clientshorttype; }
        }
        /// <summary>
        /// 记录三级分佣的用户ID，以适应调整分销商关系后的分佣问题
        /// </summary>
        public string ReferralPath
        {
            set { _referralpath = value; }
            get { return _referralpath; }
        }
        /// <summary>
        /// 用于砍价
        /// </summary>
        public int BargainDetialId
        {
            set { _bargaindetialid = value; }
            get { return _bargaindetialid; }
        }
        /// <summary>
        /// 使用余额抵扣订单详情总金额
        /// </summary>
        public decimal BalancePayMoneyTotal
        {
            set { _balancepaymoneytotal = value; }
            get { return _balancepaymoneytotal; }
        }
        /// <summary>
        /// 使用余额支付的运费部分总金额
        /// </summary>
        public decimal BalancePayFreightMoneyTotal
        {
            set { _balancepayfreightmoneytotal = value; }
            get { return _balancepayfreightmoneytotal; }
        }
        /// <summary>
        /// 优惠券抵用运费金额
        /// </summary>
        public decimal CouponFreightMoneyTotal
        {
            set { _couponfreightmoneytotal = value; }
            get { return _couponfreightmoneytotal; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            set { _updatedate = value; }
            get { return _updatedate; }
        }
        /// <summary>
        /// 物流查询工具，默认0用快递鸟
        /// </summary>
        public int LogisticsTools
        {
            set { _logisticstools = value; }
            get { return _logisticstools; }
        }
        #endregion Model
    }
}

