
CREATE FUNCTION [dbo].[F_SplitToInt]
(
	@str nvarchar(4000), 
	@spliter nvarchar(2)
)
RETURNS @returntable TABLE (UnitInt INT)
AS
BEGIN
	WHILE(CHARINDEX(@spliter,@str)<>0)  
	BEGIN  
		INSERT INTO @returntable(UnitInt) SELECT CAST (SUBSTRING(@str,1,CHARINDEX(@spliter,@str)-1) AS INT)
		SET @str = STUFF(@str,1,CHARINDEX(@spliter,@str),'')  
	END
	
	INSERT INTO @returntable(UnitInt) SELECT CAST (@str AS INT) 
      
	RETURN 
END

GO
/****** Object:  UserDefinedFunction [dbo].[F_SplitToString]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[F_SplitToString]
(
	@str nvarchar(4000), 
	@spliter nvarchar(2)
)
RETURNS @returntable TABLE (UnitString nvarchar(50))
AS
BEGIN
	WHILE(CHARINDEX(@spliter,@str)<>0)  
	BEGIN  
		INSERT INTO @returntable(UnitString) VALUES (SUBSTRING(@str,1,CHARINDEX(@spliter,@str)-1))  
		SET @str = STUFF(@str,1,CHARINDEX(@spliter,@str),'')  
	END
	
	INSERT INTO @returntable(UnitString) VALUES (@str) 
      
	RETURN 
END

GO
/****** Object:  Table [dbo].[aspnet_DistributorGrade]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[aspnet_DistributorGrade](
	[GradeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](500) NULL,
	[CommissionsLimit] [money] NOT NULL,
	[FirstCommissionRise] [decimal](18, 2) NOT NULL DEFAULT ((0)),
	[SecondCommissionRise] [decimal](18, 2) NOT NULL DEFAULT ((0)),
	[ThirdCommissionRise] [decimal](18, 2) NOT NULL DEFAULT ((0)),
	[IsDefault] [bit] NULL,
	[Ico] [varchar](255) NOT NULL DEFAULT ('/utility/pics/grade.png'),
	[AddCommission] [decimal](18, 2) NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_aspnet_DistributorGrade] PRIMARY KEY CLUSTERED 
(
	[GradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[aspnet_Distributors]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[aspnet_Distributors](
	[UserId] [int] NOT NULL,
	[StoreName] [varchar](255) NOT NULL,
	[Logo] [varchar](255) NULL,
	[BackImage] [varchar](500) NOT NULL,
	[RequestAccount] [varchar](500) NULL,
	[AccountTime] [datetime] NULL,
	[GradeId] [int] NOT NULL,
	[ReferralUserId] [int] NOT NULL,
	[ReferralPath] [varchar](50) NULL,
	[OrdersTotal] [money] NOT NULL CONSTRAINT [DF_aspnet_Distributors_OrdersTotal]  DEFAULT ((0)),
	[ReferralOrders] [int] NOT NULL CONSTRAINT [DF_aspnet_Distributors_ReferralOrders]  DEFAULT ((0)),
	[ReferralBlance] [money] NOT NULL CONSTRAINT [DF_aspnet_Distributors_ReferralBlance]  DEFAULT ((0)),
	[ReferralRequestBalance] [money] NOT NULL CONSTRAINT [DF_aspnet_Distributors_ReferralRequestBalance]  DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_aspnet_Distributors_CreateTime]  DEFAULT (getdate()),
	[ReferralStatus] [int] NOT NULL CONSTRAINT [DF_aspnet_Distributors_ReferralStatus]  DEFAULT ((1)),
	[StoreDescription] [ntext] NOT NULL,
	[DistributorGradeId] [int] NULL,
	[StoreCard] [varchar](255) NULL,
	[CardCreatTime] [datetime] NULL,
 CONSTRAINT [PK_aspnet_Distributors] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[aspnet_Managers]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Managers](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Password] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_aspnet_Managers] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_MemberGrades]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_MemberGrades](
	[GradeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Points] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[Discount] [int] NOT NULL,
	[TranVol] [decimal](18, 2) NULL,
	[TranTimes] [int] NULL,
 CONSTRAINT [PK_aspnet_MemberGrades] PRIMARY KEY CLUSTERED 
(
	[GradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Members]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[aspnet_Members](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[GradeId] [int] NOT NULL,
	[ReferralUserId] [int] NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[OrderNumber] [int] NOT NULL CONSTRAINT [DF_aspnet_Members_OrderNumber]  DEFAULT ((0)),
	[Expenditure] [money] NOT NULL CONSTRAINT [DF_aspnet_Members_Expenditure]  DEFAULT ((0)),
	[Points] [int] NOT NULL CONSTRAINT [DF_aspnet_Members_Points]  DEFAULT ((0)),
	[TopRegionId] [int] NULL,
	[RegionId] [int] NULL,
	[RealName] [nvarchar](50) NULL,
	[Email] [nvarchar](256) NULL,
	[CellPhone] [nvarchar](50) NULL,
	[QQ] [nvarchar](20) NULL,
	[Address] [nvarchar](300) NULL,
	[VipCardNumber] [nvarchar](150) NULL,
	[VipCardDate] [datetime] NULL,
	[OpenId] [nvarchar](128) NULL,
	[SessionId] [nvarchar](128) NULL,
	[SessionEndTime] [datetime] NULL,
	[Password] [nvarchar](256) NULL,
	[MicroSignal] [nvarchar](50) NULL,
	[UserHead] [nvarchar](500) NULL,
	[UserBindName] [nvarchar](50) NULL,
	[Status] [int] NOT NULL DEFAULT ((1)),
	[AlipayUserId] [varchar](100) NULL,
	[AlipayOpenid] [varchar](100) NULL,
	[AlipayLoginId] [varchar](100) NULL,
	[AlipayUsername] [nvarchar](50) NULL,
	[AlipayAvatar] [varchar](255) NULL,
	[CardID] [varchar](50) NULL,
	[IsFollowWeixin] [bit] NULL DEFAULT ((0)),
	[IsFollowAlipay] [bit] NULL DEFAULT ((0)),
	[LastOrderDate] [datetime] NULL,
	[PayOrderDate] [datetime] NULL,
	[FinishOrderDate] [datetime] NULL,
	[IsAuthorizeWeiXin] [int] NOT NULL DEFAULT ((0)),
	[TotalAmount] [money] NOT NULL DEFAULT ((0)),
	[AvailableAmount] [money] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_aspnet_Members] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[aspnet_RolePermissions]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[aspnet_RolePermissions](
	[PermissionId] [varchar](100) NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_aspnet_RolePermissions] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[aspnet_Roles]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
	[IsDefault] [bit] NOT NULL CONSTRAINT [DF_aspnet_Roles_IsDefault]  DEFAULT ((1)),
 CONSTRAINT [PK_aspnet_Roles] PRIMARY KEY NONCLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomDistributor]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CustomDistributor](
	[Id] [int] NOT NULL,
	[storename] [varchar](100) NULL,
	[ordernums] [int] NULL,
	[commtotalsum] [money] NULL,
	[rank] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_Activities]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_Activities](
	[ActivitiesId] [int] IDENTITY(1,1) NOT NULL,
	[ActivitiesName] [nvarchar](50) NOT NULL,
	[ActivitiesType] [int] NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[ActivitiesDescription] [text] NULL,
	[TakeEffect] [int] NULL,
	[Type] [int] NULL,
	[MemberGrades] [varchar](500) NOT NULL,
	[DefualtGroup] [varchar](50) NOT NULL,
	[CustomGroup] [varchar](500) NOT NULL,
	[attendTime] [int] NOT NULL,
	[attendType] [int] NOT NULL,
	[isAllProduct] [bit] NOT NULL,
	[MeetMoney] [decimal](10, 2) NOT NULL,
	[ReductionMoney] [decimal](10, 2) NOT NULL,
	[MeetType] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ActivitiesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_Activities_Detail]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Activities_Detail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ActivitiesId] [int] NOT NULL,
	[MeetMoney] [decimal](10, 2) NOT NULL,
	[ReductionMoney] [decimal](10, 2) NOT NULL,
	[bFreeShipping] [bit] NOT NULL,
	[Integral] [int] NOT NULL,
	[CouponId] [int] NOT NULL,
	[MeetNumber] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Activities_Product]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Activities_Product](
	[ActivitiesId] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[status] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_ActivitiesMember]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_ActivitiesMember](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ActivitiesId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Hishop_ActivitiesMember] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_AliFuWuMessageTemplates]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_AliFuWuMessageTemplates](
	[IsValid] [int] NULL DEFAULT ((1)),
	[OrderIndex] [int] NULL DEFAULT ((99)),
	[WXOpenTM] [varchar](50) NULL DEFAULT (''),
	[MessageType] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[SendEmail] [bit] NULL,
	[SendSMS] [bit] NULL,
	[SendInnerMessage] [bit] NULL,
	[SendWeixin] [bit] NULL,
	[WeixinTemplateId] [varchar](150) NULL,
	[TagDescription] [nvarchar](500) NULL,
	[EmailSubject] [nvarchar](1024) NULL,
	[EmailBody] [ntext] NULL,
	[InnerMessageSubject] [nvarchar](1024) NULL,
	[InnerMessageBody] [ntext] NULL,
	[SMSBody] [nvarchar](1024) NULL,
PRIMARY KEY CLUSTERED 
(
	[MessageType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_AliFuWuMessageTemplatesDetail]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_AliFuWuMessageTemplatesDetail](
	[AutoID] [int] IDENTITY(1,1) NOT NULL,
	[MessageType] [varchar](50) NULL,
	[DetailType] [varchar](50) NULL,
	[DetailName] [varchar](50) NULL,
	[AllowToAdmin] [int] NULL DEFAULT ((1)),
	[AllowToDistributor] [int] NULL DEFAULT ((0)),
	[AllowToMember] [int] NULL DEFAULT ((0)),
	[IsSelectedByDistributor] [int] NULL DEFAULT ((1)),
	[IsSelectedByMember] [int] NULL DEFAULT ((1)),
PRIMARY KEY CLUSTERED 
(
	[AutoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_Attributes]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Attributes](
	[AttributeId] [int] IDENTITY(1,1) NOT NULL,
	[AttributeName] [nvarchar](50) NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[TypeId] [int] NOT NULL,
	[UsageMode] [int] NOT NULL,
	[UseAttributeImage] [bit] NOT NULL,
 CONSTRAINT [PK_Hishop_Attributes] PRIMARY KEY CLUSTERED 
(
	[AttributeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_AttributeValues]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_AttributeValues](
	[ValueId] [int] IDENTITY(1,1) NOT NULL,
	[AttributeId] [int] NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[ValueStr] [nvarchar](200) NOT NULL,
	[ImageUrl] [nvarchar](255) NULL,
 CONSTRAINT [PK_Hishop_AttributeValues] PRIMARY KEY CLUSTERED 
(
	[ValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_BalanceDrawRequest]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_BalanceDrawRequest](
	[SerialID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RequestType] [int] NOT NULL CONSTRAINT [DF_Hishop_BalanceDrawRequest_RequestType]  DEFAULT ((1)),
	[UserName] [nvarchar](256) NOT NULL,
	[RequestTime] [datetime] NOT NULL CONSTRAINT [DF_RequestTime]  DEFAULT (getdate()),
	[Amount] [money] NOT NULL,
	[AccountName] [nvarchar](50) NOT NULL,
	[CellPhone] [nvarchar](50) NOT NULL,
	[MerchantCode] [nvarchar](300) NOT NULL,
	[Remark] [nvarchar](2000) NULL,
	[RedpackRecordNum] [int] NOT NULL CONSTRAINT [DF_Hishop_BalanceDrawRequest_RedpackRecordNum]  DEFAULT ((0)),
	[IsCheck] [int] NOT NULL CONSTRAINT [DF_Hishop_BalanceDrawRequest_IsCheck]  DEFAULT ((0)),
	[CheckTime] [datetime] NULL,
	[bankName] [nvarchar](50) NULL,
	[RedpackId] [varchar](50) NULL,
 CONSTRAINT [PK_Hishop_BalanceDrawRequest] PRIMARY KEY CLUSTERED 
(
	[SerialID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_Banner]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Banner](
	[BannerId] [int] IDENTITY(1,1) NOT NULL,
	[ShortDesc] [nvarchar](200) NULL,
	[ImageUrl] [nvarchar](350) NULL,
	[DisplaySequence] [int] NULL,
	[LocationType] [int] NULL,
	[Url] [nvarchar](350) NOT NULL,
	[Type] [int] NULL,
	[IsDisable] [bit] NOT NULL,
 CONSTRAINT [PK_Hishop_Banner] PRIMARY KEY NONCLUSTERED 
(
	[BannerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Bargain]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_Bargain](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](255) NOT NULL,
	[ActivityCover] [varchar](255) NOT NULL,
	[BeginDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Remarks] [varchar](255) NULL,
	[Status] [int] NOT NULL DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL DEFAULT (getdate()),
	[ProductId] [int] NOT NULL,
	[ActivityStock] [int] NOT NULL,
	[PurchaseNumber] [int] NOT NULL,
	[BargainType] [int] NOT NULL,
	[BargainTypeMaxVlue] [float] NOT NULL DEFAULT ((0)),
	[BargainTypeMinVlue] [float] NOT NULL DEFAULT ((0)),
	[InitialPrice] [money] NOT NULL,
	[IsCommission] [bit] NOT NULL DEFAULT ((0)),
	[FloorPrice] [money] NOT NULL,
	[TranNumber] [int] NOT NULL DEFAULT ((0)),
	[IsDelete] [int] NOT NULL DEFAULT ((0)),
	[CommissionDiscount] [int] NOT NULL DEFAULT ((100)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_BargainDetial]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_BargainDetial](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[BargainId] [int] NOT NULL,
	[Number] [int] NOT NULL DEFAULT ((0)),
	[Price] [money] NOT NULL,
	[NumberOfParticipants] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[Sku] [varchar](50) NULL,
	[IsDelete] [int] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_BrandCategories]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_BrandCategories](
	[BrandId] [int] IDENTITY(1,1) NOT NULL,
	[BrandName] [nvarchar](50) NOT NULL,
	[Logo] [nvarchar](255) NULL,
	[CompanyUrl] [nvarchar](255) NULL,
	[RewriteName] [nvarchar](50) NULL,
	[MetaKeywords] [nvarchar](100) NULL,
	[MetaDescription] [nvarchar](100) NULL,
	[Description] [ntext] NULL,
	[DisplaySequence] [int] NOT NULL,
	[Theme] [nvarchar](100) NULL,
 CONSTRAINT [PK_Hishop_BrandCategories] PRIMARY KEY CLUSTERED 
(
	[BrandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_CashBack]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_CashBack](
	[CashBackId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RechargeAmount] [money] NOT NULL,
	[CashBackAmount] [money] NOT NULL,
	[Percentage] [decimal](18, 2) NOT NULL,
	[CashBackType] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Hishop_CashBack_AddDate]  DEFAULT (getdate()),
	[IsValid] [bit] NOT NULL,
	[IsFinished] [bit] NOT NULL CONSTRAINT [DF_Hishop_CashBack_IsFinished]  DEFAULT ((0)),
	[FinishedDate] [datetime] NULL,
 CONSTRAINT [PK_Hishop_CashBack] PRIMARY KEY CLUSTERED 
(
	[CashBackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_CashBackDetails]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_CashBackDetails](
	[CashBackDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[CashBackId] [int] NOT NULL,
	[CashBackAmount] [money] NOT NULL,
	[CashBackDate] [datetime] NOT NULL CONSTRAINT [DF_Hishop_CashBackDetails_CashBackDate]  DEFAULT (getdate()),
	[CashBackType] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_CashBackDetails_1] PRIMARY KEY CLUSTERED 
(
	[CashBackDetailsId] ASC,
	[CashBackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Categories]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_Categories](
	[CategoryId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[IconUrl] [nvarchar](255) NULL,
	[Meta_Title] [nvarchar](1000) NULL,
	[Meta_Description] [nvarchar](1000) NULL,
	[Meta_Keywords] [nvarchar](1000) NULL,
	[ParentCategoryId] [int] NULL,
	[Depth] [int] NOT NULL,
	[Path] [varchar](4000) NOT NULL,
	[RewriteName] [nvarchar](50) NULL,
	[SKUPrefix] [nvarchar](10) NULL,
	[AssociatedProductType] [int] NULL,
	[Notes1] [ntext] NULL,
	[Notes2] [ntext] NULL,
	[Notes3] [ntext] NULL,
	[Notes4] [ntext] NULL,
	[Notes5] [ntext] NULL,
	[Theme] [varchar](100) NULL,
	[HasChildren] [bit] NOT NULL CONSTRAINT [DF_Hishop_Categories_HasChildren]  DEFAULT ((0)),
	[FirstCommission] [varchar](50) NULL,
	[SecondCommission] [varchar](50) NULL,
	[ThirdCommission] [varchar](50) NULL,
 CONSTRAINT [PK_Hishop_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_Commissions]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Commissions](
	[CommId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ReferralUserId] [int] NOT NULL,
	[OrderId] [nvarchar](50) NOT NULL,
	[TradeTime] [datetime] NOT NULL CONSTRAINT [DF_Hishop_Commissions_TradeTime]  DEFAULT (getdate()),
	[OrderTotal] [money] NOT NULL,
	[CommTotal] [money] NOT NULL,
	[CommType] [int] NOT NULL,
	[State] [bit] NULL,
	[CommRemark] [nvarchar](255) NULL,
 CONSTRAINT [PK_Hishop_Commissions] PRIMARY KEY CLUSTERED 
(
	[CommId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Coupon_Coupons]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_Coupon_Coupons](
	[CouponId] [int] IDENTITY(1,1) NOT NULL,
	[CouponName] [nvarchar](20) NOT NULL,
	[CouponValue] [decimal](10, 2) NOT NULL,
	[ConditionValue] [decimal](10, 2) NOT NULL CONSTRAINT [DF__Hishop_Co__Condi__1E6F845E]  DEFAULT ((0)),
	[BeginDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[StockNum] [int] NOT NULL CONSTRAINT [DF__Hishop_Co__Stock__1F63A897]  DEFAULT ((0)),
	[ReceiveNum] [int] NOT NULL CONSTRAINT [DF__Hishop_Co__Recei__2057CCD0]  DEFAULT ((0)),
	[UsedNum] [int] NOT NULL CONSTRAINT [DF__Hishop_Co__UsedN__214BF109]  DEFAULT ((0)),
	[MemberGrades] [varchar](500) NOT NULL CONSTRAINT [DF__Hishop_Co__Membe__22401542]  DEFAULT ('0'),
	[DefualtGroup] [varchar](50) NOT NULL DEFAULT ('-1'),
	[CustomGroup] [varchar](500) NOT NULL DEFAULT ('-1'),
	[ImgUrl] [nvarchar](500) NULL,
	[ProductNumber] [int] NOT NULL CONSTRAINT [DF__Hishop_Co__Produ__2334397B]  DEFAULT ((0)),
	[Finished] [bit] NULL CONSTRAINT [DF__Hishop_Co__Finis__24285DB4]  DEFAULT ((0)),
	[IsAllProduct] [bit] NOT NULL,
	[maxReceivNum] [int] NOT NULL CONSTRAINT [DF__Hishop_Co__maxRe__251C81ED]  DEFAULT ((1)),
	[CouponTypes] [varchar](50) NULL,
	[ExpiredPromptTimes] [nchar](10) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_Coupon_MemberCoupons]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Coupon_MemberCoupons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CouponId] [int] NOT NULL,
	[MemberId] [int] NULL,
	[ReceiveDate] [datetime] NULL,
	[UsedDate] [datetime] NULL,
	[OrderNo] [nvarchar](50) NULL,
	[Status] [int] NULL,
	[CouponName] [nvarchar](20) NOT NULL,
	[ConditionValue] [decimal](10, 2) NULL,
	[BeginDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[CouponValue] [decimal](18, 0) NULL,
 CONSTRAINT [PK_Hishop_Coupon_slave] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Coupon_Products]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Coupon_Products](
	[CouponId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Hishop_Coupon_Good] PRIMARY KEY CLUSTERED 
(
	[CouponId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_CouponItems]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_CouponItems](
	[CouponId] [int] NOT NULL,
	[LotNumber] [uniqueidentifier] NOT NULL,
	[ClaimCode] [nvarchar](50) NOT NULL,
	[UserId] [int] NULL,
	[UserName] [nvarchar](256) NULL,
	[EmailAddress] [nvarchar](255) NULL,
	[GenerateTime] [datetime] NOT NULL,
	[CouponStatus] [int] NOT NULL,
	[UsedTime] [datetime] NULL,
	[OrderId] [nvarchar](60) NULL,
 CONSTRAINT [PK_Hishop_CouponItems] PRIMARY KEY CLUSTERED 
(
	[ClaimCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Coupons]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Coupons](
	[CouponId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[ClosingTime] [datetime] NOT NULL,
	[Description] [ntext] NULL,
	[Amount] [money] NULL,
	[DiscountValue] [money] NOT NULL,
	[SentCount] [int] NOT NULL,
	[UsedCount] [int] NOT NULL,
	[NeedPoint] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_Coupons] PRIMARY KEY CLUSTERED 
(
	[CouponId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_CustomDistributor]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_CustomDistributor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[storename] [varchar](100) NULL,
	[ordernums] [int] NULL,
	[commtotalsum] [money] NULL,
	[logo] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_CustomPage]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_CustomPage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[FormalJson] [varchar](max) NULL,
	[DraftJson] [varchar](max) NULL,
	[Status] [int] NULL,
	[TempIndexName] [varchar](50) NULL,
	[PageUrl] [varchar](255) NULL,
	[IsShowMenu] [bigint] NULL,
	[Details] [varchar](200) NULL,
	[CreateTime] [datetime] NULL,
	[PV] [int] NULL DEFAULT ((0)),
	[DraftName] [varchar](50) NULL,
	[DraftDetails] [varchar](200) NULL,
	[DraftPageUrl] [varchar](255) NULL,
	[DraftIsShowMenu] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_DistributorGradeCommission]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_DistributorGradeCommission](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Commission] [decimal](18, 2) NOT NULL,
	[PubTime] [datetime] NOT NULL DEFAULT (getdate()),
	[OperAdmin] [nvarchar](50) NOT NULL,
	[Memo] [nvarchar](2000) NULL,
	[OrderID] [nvarchar](50) NULL,
	[OldCommissionTotal] [decimal](18, 2) NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_DistributorProducts]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_DistributorProducts](
	[ProductId] [int] NOT NULL,
	[UserId] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_ExpressTemplates]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_ExpressTemplates](
	[ExpressId] [int] IDENTITY(1,1) NOT NULL,
	[ExpressName] [nvarchar](50) NOT NULL,
	[XmlFile] [nvarchar](50) NOT NULL,
	[IsUse] [bit] NOT NULL,
	[IsDefault] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Hishop_ExpressTemplates] PRIMARY KEY CLUSTERED 
(
	[ExpressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Favorite]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Favorite](
	[FavoriteId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Tags] [nvarchar](100) NOT NULL,
	[Remark] [nvarchar](500) NULL,
 CONSTRAINT [PK_Hishop_Favorite] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_FreightTemplate_FreeShipping]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_FreightTemplate_FreeShipping](
	[TemplateId] [int] NOT NULL,
	[ModeId] [tinyint] NOT NULL,
	[ConditionType] [tinyint] NOT NULL,
	[ConditionNumber] [varchar](30) NOT NULL,
	[FreeId] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Hishop_FreeShipping] PRIMARY KEY CLUSTERED 
(
	[FreeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_FreightTemplate_FreeShippingRegions]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_FreightTemplate_FreeShippingRegions](
	[TemplateId] [int] NOT NULL,
	[RegionId] [varchar](10) NOT NULL,
	[FreeId] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_Hishop_FreeShippingRegions] PRIMARY KEY CLUSTERED 
(
	[RegionId] ASC,
	[FreeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_FreightTemplate_SpecifyRegionGroups]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_FreightTemplate_SpecifyRegionGroups](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[TemplateId] [int] NOT NULL,
	[ModeId] [tinyint] NOT NULL,
	[FristNumber] [decimal](18, 2) NOT NULL,
	[FristPrice] [money] NOT NULL,
	[AddNumber] [decimal](18, 2) NOT NULL,
	[AddPrice] [money] NOT NULL,
	[IsDefault] [bit] NOT NULL,
 CONSTRAINT [PK_Hishop_SpecifyRegionGroups] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_FreightTemplate_SpecifyRegions]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_FreightTemplate_SpecifyRegions](
	[TemplateId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
	[RegionId] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_SpecifyRegions] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC,
	[RegionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_FreightTemplate_Templates]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_FreightTemplate_Templates](
	[TemplateId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[FreeShip] [bit] NOT NULL DEFAULT ((0)),
	[MUnit] [tinyint] NOT NULL,
	[HasFree] [bit] NOT NULL,
 CONSTRAINT [PK_Hishop_FreightTemplates] PRIMARY KEY CLUSTERED 
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_HelpBargainDetial]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_HelpBargainDetial](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BargainDetialId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[BargainPrice] [money] NOT NULL DEFAULT ((0)),
	[CreateDate] [datetime] NULL,
	[BargainId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_InsuranceArea]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_InsuranceArea](
	[InsuranceAreaId] [int] IDENTITY(1,1) NOT NULL,
	[InsuranceAreaName] [nvarchar](500) NULL,
	[InsuranceAreaCiteId] [int] NULL,
	[InsuranceAreaProvinceId] [int] NULL,
	[InsuranceAreaCiteName] [nvarchar](500) NULL,
	[InsuranceCompanyTypes] [nvarchar](500) NULL,
	[InsuranceCompanyTypesIds] [nvarchar](50) NULL,
 CONSTRAINT [PK_Hishop_InsuranceArea] PRIMARY KEY CLUSTERED 
(
	[InsuranceAreaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_InsuranceAreaCompany]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_InsuranceAreaCompany](
	[InsuranceCompanyID] [int] NOT NULL,
	[InsuranceAreaId] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_InsuranceAreaCompany] PRIMARY KEY CLUSTERED 
(
	[InsuranceCompanyID] ASC,
	[InsuranceAreaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_InsuranceCompany]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_InsuranceCompany](
	[InsuranceCompanyID] [int] IDENTITY(1,1) NOT NULL,
	[InsuranceCompanyName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Hishop_InsuranceCompany] PRIMARY KEY NONCLUSTERED 
(
	[InsuranceCompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_InsuranceOrder]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_InsuranceOrder](
	[InsuranceOrderId] [int] IDENTITY(1,1) NOT NULL,
	[InsuranceOrderCity1] [int] NULL,
	[InsuranceOrderCity1Name] [nvarchar](50) NULL,
	[InsuranceOrderCity2] [int] NULL,
	[InsuranceOrderCity2Name] [nvarchar](50) NULL,
	[InsuranceOrderCompany] [int] NULL,
	[InsuranceOrderCompany_Name] [nvarchar](100) NULL,
	[InsuranceOrderJiaoQiangXian] [nvarchar](50) NULL,
	[InsuranceOrderShangYeSanXian] [nvarchar](50) NULL,
	[InsuranceOrderShangYeSanXianEr] [nvarchar](50) NULL,
	[InsuranceOrderCheShangRenYuanSiJiXian] [nvarchar](50) NULL,
	[InsuranceOrderCheShangRenYuanSiJiXianEr] [nvarchar](50) NULL,
	[InsuranceOrderCheShangRenYuanChengKeXian] [nvarchar](50) NULL,
	[InsuranceOrderCheShangRenYuanChengKeXianEr] [nvarchar](50) NULL,
	[InsuranceOrderCheLiangSunShiXian] [nvarchar](50) NULL,
	[InsuranceOrderDaoQiangXian] [nvarchar](50) NULL,
	[InsuranceOrderBoliXian] [nvarchar](50) NULL,
	[InsuranceOrderZiRanXian] [nvarchar](50) NULL,
	[InsuranceOrderSheShuiXian] [nvarchar](50) NULL,
	[InsuranceOrderTeYueXian] [nvarchar](50) NULL,
	[InsuranceOrderSanFangZenRenXian] [nvarchar](50) NULL,
	[InsuranceOrderHuaHenXian] [nvarchar](50) NULL,
	[InsuranceOrderCarType] [int] NULL,
	[InsuranceOrderJiaoZhaoZhengBen] [nvarchar](50) NULL,
	[InsuranceOrderJiaoZhaoFuBen] [nvarchar](50) NULL,
	[InsuranceOrderName] [nvarchar](50) NULL,
	[InsuranceOrderTel] [nvarchar](50) NULL,
	[InsuranceOrderCardZheng] [nvarchar](50) NULL,
	[InsuranceOrderCardFan] [nvarchar](50) NULL,
	[InsuranceOrderAddres] [nvarchar](200) NULL,
	[InsuranceOrderShouJIanName] [nvarchar](50) NULL,
	[InsuranceOrderShouJIanTel] [nvarchar](50) NULL,
	[InsuranceOrderStatu] [int] NULL CONSTRAINT [DF_Hishop_InsuranceOrder_InsuranceOrderStatu]  DEFAULT ((0)),
	[InsuranceOrderAmount] [decimal](18, 2) NULL,
	[InsuranceOrderNotes] [nvarchar](1000) NULL,
	[InsuranceUserId] [int] NULL,
	[InsuranceOrderOpenId] [nvarchar](100) NULL,
	[InsuranceOrderCreatDate] [datetime] NULL,
	[InsuranceOrderUpdateDate] [datetime] NULL,
	[InsuranceOrderIsRenewal] [int] NULL,
	[InsuranceDisCountAmount] [decimal](18, 2) NULL CONSTRAINT [DF_Hishop_InsuranceOrder_InsuranceDisCountAmount]  DEFAULT ((0)),
 CONSTRAINT [PK_Hishop_InsuranceOrder] PRIMARY KEY CLUSTERED 
(
	[InsuranceOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_LimitedTimeDiscount]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_LimitedTimeDiscount](
	[LimitedTimeDiscountId] [int] IDENTITY(1,1) NOT NULL,
	[ActivityName] [nvarchar](100) NOT NULL,
	[BeginTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Description] [nvarchar](100) NULL,
	[LimitNumber] [int] NOT NULL,
	[ApplyMembers] [varchar](50) NOT NULL,
	[DefualtGroup] [varchar](50) NOT NULL,
	[CustomGroup] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NULL,
	[Status] [int] NULL,
	[IsCommission] [bit] NOT NULL,
	[CommissionDiscount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LimitedTimeDiscountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_LimitedTimeDiscountProduct]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_LimitedTimeDiscountProduct](
	[LimitedTimeDiscountProductId] [int] IDENTITY(1,1) NOT NULL,
	[LimitedTimeDiscountId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Discount] [decimal](18, 2) NULL,
	[Minus] [decimal](18, 2) NULL,
	[IsDehorned] [int] NULL,
	[IsChamferPoint] [int] NULL,
	[FinalPrice] [decimal](18, 2) NOT NULL,
	[CreateTime] [datetime] NULL,
	[BeginTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LimitedTimeDiscountProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Logs]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Logs](
	[LogId] [bigint] IDENTITY(1,1) NOT NULL,
	[PageUrl] [nvarchar](1000) NOT NULL,
	[AddedTime] [datetime] NOT NULL,
	[UserName] [nvarchar](64) NOT NULL,
	[IPAddress] [nvarchar](50) NOT NULL,
	[Privilege] [int] NOT NULL,
	[Description] [ntext] NULL,
 CONSTRAINT [PK_Hishop_Logs] PRIMARY KEY NONCLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_MemberAmountDetailed]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_MemberAmountDetailed](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[PayId] [nvarchar](256) NOT NULL,
	[TradeAmount] [money] NOT NULL,
	[AvailableAmount] [money] NOT NULL DEFAULT ((0)),
	[TradeType] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[TradeWays] [int] NOT NULL,
	[TradeTime] [datetime] NOT NULL,
	[Remark] [text] NULL,
	[State] [int] NOT NULL DEFAULT ((0)),
	[GatewayPayId] [nvarchar](256) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_MemberAmountRequest]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_MemberAmountRequest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[RequestTime] [datetime] NOT NULL,
	[Amount] [money] NOT NULL,
	[RequestType] [int] NOT NULL,
	[AccountCode] [text] NOT NULL,
	[AccountName] [nvarchar](256) NOT NULL,
	[BankName] [text] NOT NULL,
	[Remark] [text] NULL,
	[State] [int] NOT NULL,
	[CheckTime] [datetime] NULL,
	[CellPhone] [text] NULL,
	[Operator] [nvarchar](256) NULL,
	[RedpackId] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_MemberClientSet]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_MemberClientSet](
	[ClientTypeId] [int] NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[LastDay] [int] NULL,
	[ClientChar] [nvarchar](2) NULL,
	[ClientValue] [decimal](18, 0) NULL,
 CONSTRAINT [PK_Hishop_MemberClientSet] PRIMARY KEY NONCLUSTERED 
(
	[ClientTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_MessageAdminUserMsgList]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_MessageAdminUserMsgList](
	[AutoID] [int] IDENTITY(1,1) NOT NULL,
	[UserOpenId] [varchar](50) NOT NULL,
	[RealName] [varchar](50) NULL,
	[RoleName] [varchar](50) NULL,
	[Msg1] [int] NULL DEFAULT ((0)),
	[Msg2] [int] NULL DEFAULT ((0)),
	[Msg3] [int] NULL DEFAULT ((0)),
	[Msg4] [int] NULL DEFAULT ((0)),
	[Msg5] [int] NULL DEFAULT ((0)),
	[Msg6] [int] NULL DEFAULT ((0)),
	[Type] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Hishop_MessageAdminUserList] PRIMARY KEY CLUSTERED 
(
	[UserOpenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_MessageTemplates]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_MessageTemplates](
	[IsValid] [int] NULL DEFAULT ((1)),
	[OrderIndex] [int] NULL DEFAULT ((99)),
	[WXOpenTM] [varchar](50) NULL DEFAULT (''),
	[MessageType] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[SendEmail] [bit] NULL,
	[SendSMS] [bit] NULL,
	[SendInnerMessage] [bit] NULL,
	[SendWeixin] [bit] NULL,
	[WeixinTemplateId] [varchar](150) NULL,
	[TagDescription] [nvarchar](500) NULL,
	[EmailSubject] [nvarchar](1024) NULL,
	[EmailBody] [ntext] NULL,
	[InnerMessageSubject] [nvarchar](1024) NULL,
	[InnerMessageBody] [ntext] NULL,
	[SMSBody] [nvarchar](1024) NULL,
 CONSTRAINT [PK_Hishop_MessageTemplates] PRIMARY KEY CLUSTERED 
(
	[MessageType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_MessageTemplatesDetail]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_MessageTemplatesDetail](
	[AutoID] [int] IDENTITY(1,1) NOT NULL,
	[MessageType] [varchar](50) NOT NULL,
	[DetailType] [varchar](50) NULL,
	[DetailName] [varchar](50) NULL,
	[AllowToAdmin] [int] NULL CONSTRAINT [DF_Hishop_MessageTemplatesDetail_AllowToAdmin]  DEFAULT ((1)),
	[AllowToDistributor] [int] NULL CONSTRAINT [DF_Hishop_MessageTemplatesDetail_AllowToDistributor]  DEFAULT ((0)),
	[AllowToMember] [int] NULL CONSTRAINT [DF_Table_1_AllowToDistributor1]  DEFAULT ((0)),
	[IsSelectedByDistributor] [int] NULL CONSTRAINT [DF_Hishop_MessageTemplatesDetail_IsSelectedByAdmin1]  DEFAULT ((1)),
	[IsSelectedByMember] [int] NULL CONSTRAINT [DF_Hishop_MessageTemplatesDetail_IsSelectedByAdmin2]  DEFAULT ((1))
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_Notice]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Notice](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Memo] [ntext] NOT NULL,
	[Author] [nvarchar](50) NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsPub] [int] NOT NULL,
	[PubTime] [datetime] NULL,
	[SendType] [int] NOT NULL,
	[SendTo] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_NoticeRead]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_NoticeRead](
	[NoticeId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[ReadTime] [datetime] NOT NULL,
	[NoticeIsDel] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_NoticeRead] PRIMARY KEY CLUSTERED 
(
	[NoticeId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_NoticeTempUser]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_NoticeTempUser](
	[LoginName] [varchar](50) NOT NULL,
	[UserId] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_NoticeUser]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_NoticeUser](
	[NoticeId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[NoticeId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_OrderDebitNote]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_OrderDebitNote](
	[NoteId] [nvarchar](50) NOT NULL,
	[OrderId] [nvarchar](50) NOT NULL,
	[Operator] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Hishop_OrderDebitNote] PRIMARY KEY CLUSTERED 
(
	[NoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_OrderExpressData]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_OrderExpressData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [nvarchar](20) NOT NULL,
	[ExpressNumber] [nvarchar](50) NOT NULL,
	[DataContent] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_OrderItems]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_OrderItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [nvarchar](50) NOT NULL,
	[SkuId] [nvarchar](100) NOT NULL,
	[ProductId] [int] NOT NULL,
	[SKU] [nvarchar](50) NULL,
	[Quantity] [int] NOT NULL,
	[ShipmentQuantity] [int] NOT NULL,
	[CostPrice] [money] NOT NULL,
	[ItemListPrice] [money] NOT NULL,
	[ItemAdjustedPrice] [money] NOT NULL,
	[ItemAdjustedCommssion] [money] NOT NULL CONSTRAINT [DF_Hishop_Orders_ItemAdjustedCommssion]  DEFAULT ((0)),
	[ItemDescription] [nvarchar](200) NOT NULL,
	[ThumbnailsUrl] [nvarchar](255) NULL,
	[Weight] [money] NULL,
	[SKUContent] [nvarchar](4000) NULL,
	[PromotionId] [int] NULL,
	[PromotionName] [nvarchar](100) NULL,
	[OrderItemsStatus] [int] NOT NULL,
	[ItemsCommission] [money] NULL CONSTRAINT [DF_Hishop_Orders_ItemsCommission]  DEFAULT ((0)),
	[SecondItemsCommission] [money] NULL CONSTRAINT [DF_Hishop_Orders_SecondItemsCommission]  DEFAULT ((0)),
	[ThirdItemsCommission] [money] NULL CONSTRAINT [DF_Hishop_Orders_ThirdItemsCommission]  DEFAULT ((0)),
	[PointNumber] [int] NULL CONSTRAINT [DF_Hishop_Orders_PointNumber]  DEFAULT ((0)),
	[Type] [int] NULL CONSTRAINT [DF_Hishop_Orders_Type]  DEFAULT ((0)),
	[IsHandled] [bit] NOT NULL DEFAULT ((0)),
	[ReturnMoney] [money] NOT NULL DEFAULT ((0)),
	[DiscountAverage] [decimal](18, 2) NOT NULL CONSTRAINT [DF_Hishop_OrderItems_DiscountAverage]  DEFAULT ((0)),
	[IsAdminModify] [bit] NOT NULL DEFAULT ((0)),
	[DeleteBeforeState] [int] NULL DEFAULT ((0)),
	[LimitedTimeDiscountId] [int] NOT NULL DEFAULT ((0)),
	[ItemsCommissionScale] [decimal](18, 2) NOT NULL DEFAULT ((0)),
	[SecondItemsCommissionScale] [decimal](18, 2) NOT NULL DEFAULT ((0)),
	[ThirdItemsCommissionScale] [decimal](18, 2) NOT NULL DEFAULT ((0)),
	[BalancePayMoney] [decimal](18, 2) NOT NULL DEFAULT ((0)),
	[CommissionDiscount] [int] NOT NULL DEFAULT ((100)),
 CONSTRAINT [PK_Hishop_OrderItems] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_OrderRefund]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_OrderRefund](
	[RefundId] [int] IDENTITY(10000,1) NOT NULL,
	[OrderId] [nvarchar](50) NOT NULL,
	[ApplyForTime] [datetime] NOT NULL,
	[RefundType] [int] NULL,
	[RefundRemark] [ntext] NULL,
	[AdminRemark] [ntext] NULL,
	[HandleStatus] [int] NOT NULL,
	[HandleTime] [datetime] NULL,
	[Operator] [nvarchar](50) NULL,
 CONSTRAINT [PK_Hishop_Refund] PRIMARY KEY NONCLUSTERED 
(
	[RefundId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_OrderReplace]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_OrderReplace](
	[ReplaceId] [int] IDENTITY(10000,1) NOT NULL,
	[OrderId] [nvarchar](50) NOT NULL,
	[ApplyForTime] [datetime] NOT NULL,
	[Comments] [ntext] NULL,
	[HandleStatus] [int] NOT NULL,
	[HandleTime] [datetime] NULL,
	[AdminRemark] [ntext] NULL,
 CONSTRAINT [PK_Hishop_OrderReplace] PRIMARY KEY NONCLUSTERED 
(
	[ReplaceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_OrderReturns]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_OrderReturns](
	[ReturnsId] [int] IDENTITY(10000,1) NOT NULL,
	[OrderId] [nvarchar](50) NOT NULL,
	[ApplyForTime] [datetime] NOT NULL,
	[RefundType] [int] NOT NULL,
	[RefundMoney] [money] NOT NULL,
	[Comments] [ntext] NULL,
	[HandleStatus] [int] NOT NULL,
	[HandleTime] [datetime] NULL,
	[AdminRemark] [ntext] NULL,
	[Operator] [nvarchar](50) NULL,
	[Account] [nvarchar](50) NULL,
	[ProductId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[AuditTime] [nvarchar](50) NULL,
	[RefundTime] [nvarchar](50) NULL,
	[SkuId] [nvarchar](100) NULL,
	[DeleteBeforeState] [int] NULL,
	[OrderItemID] [int] NOT NULL,
	[BalanceReturnMoney] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Hishop_Returns] PRIMARY KEY NONCLUSTERED 
(
	[ReturnsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Orders]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_Orders](
	[OrderId] [nvarchar](50) NOT NULL,
	[OrderMarking] [nvarchar](50) NULL,
	[Remark] [nvarchar](4000) NULL,
	[ManagerMark] [int] NULL,
	[ManagerRemark] [nvarchar](4000) NULL,
	[AdjustedDiscount] [money] NULL,
	[OrderStatus] [int] NOT NULL,
	[CloseReason] [nvarchar](4000) NULL,
	[OrderDate] [datetime] NOT NULL,
	[PayDate] [datetime] NULL,
	[ShippingDate] [datetime] NULL,
	[FinishDate] [datetime] NULL,
	[UserId] [int] NOT NULL,
	[Username] [nvarchar](64) NOT NULL,
	[EmailAddress] [nvarchar](255) NULL,
	[RealName] [nvarchar](50) NULL,
	[QQ] [nvarchar](20) NULL,
	[Wangwang] [nvarchar](20) NULL,
	[MSN] [nvarchar](128) NULL,
	[ShippingRegion] [nvarchar](300) NULL,
	[Address] [nvarchar](300) NULL,
	[ZipCode] [nvarchar](20) NULL,
	[ShipTo] [nvarchar](50) NULL,
	[TelPhone] [nvarchar](50) NULL,
	[CellPhone] [nvarchar](50) NULL,
	[ShipToDate] [nvarchar](50) NULL,
	[ShippingModeId] [int] NULL,
	[ModeName] [nvarchar](50) NULL,
	[RealShippingModeId] [int] NULL,
	[RealModeName] [nvarchar](50) NULL,
	[RegionId] [int] NULL,
	[Freight] [money] NULL,
	[AdjustedFreight] [money] NULL,
	[ShipOrderNumber] [nvarchar](50) NULL,
	[Weight] [money] NULL,
	[ExpressCompanyName] [nvarchar](500) NULL,
	[ExpressCompanyAbb] [nvarchar](500) NULL,
	[PaymentTypeId] [int] NULL,
	[PaymentType] [nvarchar](100) NULL,
	[PayCharge] [money] NULL,
	[RefundStatus] [int] NULL,
	[RefundAmount] [money] NULL,
	[RefundRemark] [nvarchar](4000) NULL,
	[Gateway] [nvarchar](200) NULL,
	[OrderTotal] [money] NULL,
	[OrderPoint] [int] NULL,
	[OrderCostPrice] [money] NULL,
	[OrderProfit] [money] NULL,
	[ActualFreight] [money] NULL,
	[OtherCost] [money] NULL,
	[OptionPrice] [money] NULL,
	[Amount] [money] NULL,
	[DiscountAmount] [money] NULL CONSTRAINT [DF_Hishop_Orders_DiscountAmount]  DEFAULT ((0)),
	[ActivitiesId] [nvarchar](50) NULL,
	[ActivitiesName] [nvarchar](200) NULL,
	[ReducedPromotionId] [int] NULL,
	[ReducedPromotionName] [nvarchar](100) NULL,
	[ReducedPromotionAmount] [money] NULL,
	[IsReduced] [bit] NULL,
	[SentTimesPointPromotionId] [int] NULL,
	[SentTimesPointPromotionName] [nvarchar](100) NULL,
	[TimesPoint] [money] NULL,
	[IsSendTimesPoint] [bit] NULL,
	[FreightFreePromotionId] [int] NULL,
	[FreightFreePromotionName] [nvarchar](100) NULL,
	[IsFreightFree] [bit] NULL,
	[CouponName] [nvarchar](100) NULL,
	[CouponCode] [nvarchar](50) NULL,
	[CouponAmount] [money] NULL,
	[CouponValue] [money] NULL,
	[GroupBuyId] [int] NULL,
	[NeedPrice] [money] NULL,
	[GroupBuyStatus] [int] NULL,
	[CountDownBuyId] [int] NULL,
	[BundlingId] [int] NULL,
	[BundlingNum] [int] NULL,
	[BundlingPrice] [money] NULL,
	[GatewayOrderId] [nvarchar](100) NULL,
	[IsPrinted] [bit] NULL,
	[Tax] [money] NULL,
	[InvoiceTitle] [nvarchar](50) NULL,
	[Sender] [nvarchar](50) NULL,
	[ReferralUserId] [int] NULL,
	[FirstCommission] [money] NULL CONSTRAINT [DF_Hishop_Orders_FirstCommission]  DEFAULT ((0)),
	[SecondCommission] [money] NULL CONSTRAINT [DF_Hishop_Orders_SecondCommission]  DEFAULT ((0)),
	[ThirdCommission] [money] NULL CONSTRAINT [DF_Hishop_Orders_ThirdCommission]  DEFAULT ((0)),
	[RedPagerActivityName] [nvarchar](100) NULL,
	[RedPagerID] [int] NULL,
	[RedPagerOrderAmountCanUse] [money] NULL CONSTRAINT [DF_RedPagerOrderAmountCanUse]  DEFAULT ((0)),
	[RedPagerAmount] [money] NULL CONSTRAINT [DF_RedPagerAmount]  DEFAULT ((0)),
	[OldAddress] [nvarchar](200) NULL,
	[PointToCash] [money] NOT NULL CONSTRAINT [DF_PointToCash]  DEFAULT ((0)),
	[PointExchange] [int] NOT NULL CONSTRAINT [DF_PointExchange]  DEFAULT ((0)),
	[SplitState] [int] NOT NULL DEFAULT ((0)),
	[DeleteBeforeState] [int] NOT NULL DEFAULT ((0)),
	[ClientShortType] [int] NOT NULL DEFAULT ((0)),
	[ReferralPath] [varchar](50) NULL,
	[BargainDetialId] [int] NOT NULL DEFAULT ((0)),
	[BalancePayMoneyTotal] [decimal](18, 2) NOT NULL DEFAULT ((0)),
	[BalancePayFreightMoneyTotal] [decimal](18, 2) NOT NULL DEFAULT ((0)),
	[CouponFreightMoneyTotal] [decimal](18, 2) NOT NULL DEFAULT ((0)),
	[UpdateDate] [datetime] NOT NULL DEFAULT (getdate()),
	[LogisticsTools] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Hishop_Orders] PRIMARY KEY NONCLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [Hishop_Orders_Index]    Script Date: 2017/3/14 21:10:56 ******/
CREATE CLUSTERED INDEX [Hishop_Orders_Index] ON [dbo].[Hishop_Orders]
(
	[OrderDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hishop_OrderSendNote]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_OrderSendNote](
	[NoteId] [nvarchar](50) NOT NULL,
	[OrderId] [nvarchar](50) NOT NULL,
	[Operator] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Hishop_OrderSendNote] PRIMARY KEY CLUSTERED 
(
	[NoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_PaymentTypes]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_PaymentTypes](
	[ModeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [ntext] NULL,
	[Gateway] [nvarchar](200) NULL,
	[DisplaySequence] [int] NOT NULL,
	[IsUseInpour] [bit] NOT NULL CONSTRAINT [DF_Hishop_PaymentTypes_IsUseInpour]  DEFAULT ('true'),
	[IsUseInDistributor] [bit] NOT NULL CONSTRAINT [DF_Hishop_PaymentTypes_IsUseInDistributor]  DEFAULT ('true'),
	[Charge] [money] NOT NULL,
	[IsPercent] [bit] NOT NULL,
	[Settings] [ntext] NULL,
 CONSTRAINT [PK_Hishop_PaymentTypes] PRIMARY KEY CLUSTERED 
(
	[ModeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_PhotoCategories]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_PhotoCategories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[TypeId] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_distro_PhotoCategories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_PhotoGallery]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_PhotoGallery](
	[PhotoId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[PhotoName] [nvarchar](100) NULL,
	[PhotoPath] [varchar](300) NOT NULL,
	[FileSize] [int] NOT NULL,
	[UploadTime] [datetime] NOT NULL,
	[LastUpdateTime] [datetime] NOT NULL,
	[TypeId] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_distro_PhotoGallery] PRIMARY KEY CLUSTERED 
(
	[PhotoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_PointExchange_Changed]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_PointExchange_Changed](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[exChangeId] [int] NOT NULL,
	[exChangeName] [nvarchar](50) NOT NULL,
	[ProductId] [int] NOT NULL,
	[PointNumber] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[MemberID] [int] NOT NULL,
	[MemberGrades] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_PointExChange_PointExChanges]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_PointExChange_PointExChanges](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[MemberGrades] [varchar](500) NOT NULL,
	[DefualtGroup] [varchar](50) NOT NULL,
	[CustomGroup] [varchar](500) NOT NULL,
	[BeginDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[ProductNumber] [int] NOT NULL,
	[ImgUrl] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_PointExChange_Products]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_PointExChange_Products](
	[exChangeId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[status] [int] NOT NULL,
	[ProductNumber] [int] NOT NULL,
	[PointNumber] [int] NOT NULL,
	[ChangedNumber] [int] NOT NULL,
	[eachMaxNumber] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_PointExChange_Products] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[exChangeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_PrivilegeInRoles]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_PrivilegeInRoles](
	[RoleId] [int] NOT NULL,
	[Privilege] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_PrivilegeInRoles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[Privilege] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_PrizesDeliveryRecord]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_PrizesDeliveryRecord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Receiver] [nvarchar](50) NULL,
	[Tel] [varchar](50) NULL,
	[status] [int] NOT NULL,
	[LogId] [int] NOT NULL,
	[DeliveryTime] [datetime] NULL,
	[ReceiveTime] [datetime] NULL,
	[ExpressName] [nvarchar](50) NULL,
	[CourierNumber] [varchar](50) NULL,
	[ReggionPath] [varchar](50) NULL,
	[Address] [nvarchar](100) NULL,
	[Pid] [varchar](50) NOT NULL,
	[RecordType] [int] NOT NULL,
	[IsPrinted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_ProductAttributes]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_ProductAttributes](
	[ProductId] [int] NOT NULL,
	[AttributeId] [int] NOT NULL,
	[ValueId] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_ProductAttributes] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[AttributeId] ASC,
	[ValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_ProductConsultations]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_ProductConsultations](
	[ConsultationId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](100) NULL,
	[UserEmail] [nvarchar](100) NULL,
	[ConsultationText] [nvarchar](1000) NOT NULL,
	[ConsultationDate] [datetime] NOT NULL,
	[ReplyText] [ntext] NULL,
	[ReplyDate] [datetime] NULL,
	[ReplyUserId] [int] NULL,
	[ViewDate] [datetime] NULL,
 CONSTRAINT [PK_Hishop_ProductConsultations] PRIMARY KEY NONCLUSTERED 
(
	[ConsultationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_ProductReviews]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_ProductReviews](
	[ReviewId] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[UserEmail] [nvarchar](100) NULL,
	[ReviewText] [nvarchar](3000) NOT NULL,
	[ReviewDate] [datetime] NOT NULL,
	[OrderID] [nvarchar](50) NULL,
	[SkuID] [nvarchar](100) NULL,
	[OrderItemID] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Hishop_ProductReviews] PRIMARY KEY NONCLUSTERED 
(
	[ReviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Products]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_Products](
	[CategoryId] [int] NOT NULL,
	[TypeId] [int] NULL,
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](200) NOT NULL,
	[ProductCode] [nvarchar](50) NULL,
	[ShortDescription] [nvarchar](2000) NULL,
	[Unit] [nvarchar](50) NULL,
	[Description] [ntext] NULL,
	[SaleStatus] [int] NOT NULL,
	[AddedDate] [datetime] NOT NULL,
	[VistiCounts] [int] NOT NULL CONSTRAINT [DF_Hishop_Products_VistiCounts]  DEFAULT ((0)),
	[SaleCounts] [int] NOT NULL CONSTRAINT [DF_Hishop_Products_SaleCounts]  DEFAULT ((0)),
	[ShowSaleCounts] [int] NOT NULL CONSTRAINT [DF_Hishop_Products_ShowSaleCounts]  DEFAULT ((0)),
	[DisplaySequence] [int] NOT NULL CONSTRAINT [DF_Hishop_Products_DisplaySequence]  DEFAULT ((0)),
	[ImageUrl1] [nvarchar](255) NULL,
	[ImageUrl2] [nvarchar](255) NULL,
	[ImageUrl3] [nvarchar](255) NULL,
	[ImageUrl4] [nvarchar](255) NULL,
	[ImageUrl5] [nvarchar](255) NULL,
	[ThumbnailUrl40] [nvarchar](255) NULL,
	[ThumbnailUrl60] [nvarchar](255) NULL,
	[ThumbnailUrl100] [nvarchar](255) NULL,
	[ThumbnailUrl160] [nvarchar](255) NULL,
	[ThumbnailUrl180] [nvarchar](255) NULL,
	[ThumbnailUrl220] [nvarchar](255) NULL,
	[ThumbnailUrl310] [nvarchar](255) NULL,
	[ThumbnailUrl410] [nvarchar](255) NULL,
	[MarketPrice] [money] NULL,
	[BrandId] [int] NULL,
	[MainCategoryPath] [nvarchar](256) NULL,
	[ExtendCategoryPath] [nvarchar](256) NULL,
	[HasSKU] [bit] NOT NULL,
	[IsfreeShipping] [bit] NULL,
	[TaobaoProductId] [bigint] NULL,
	[Source] [varchar](1) NULL,
	[MinShowPrice] [money] NOT NULL CONSTRAINT [DF_Hishop_Products_MinShowPrice]  DEFAULT ((0)),
	[MaxShowPrice] [money] NOT NULL CONSTRAINT [DF_Hishop_Products_MaxShowPrice]  DEFAULT ((0)),
	[FreightTemplateId] [int] NOT NULL CONSTRAINT [DF_Hishop_Products_FreightTemplateId]  DEFAULT ((0)),
	[FirstCommission] [decimal](18, 2) NOT NULL CONSTRAINT [DF_Hishop_Products_FirstCommission]  DEFAULT ((0)),
	[SecondCommission] [decimal](18, 2) NOT NULL CONSTRAINT [DF_Hishop_Products_SecondCommission]  DEFAULT ((0)),
	[ThirdCommission] [decimal](18, 2) NOT NULL CONSTRAINT [DF_Hishop_Products_ThirdCommission]  DEFAULT ((0)),
	[IsSetCommission] [bit] NOT NULL CONSTRAINT [DF_Hishop_Products_IsSetCommission]  DEFAULT ((0)),
	[CubicMeter] [decimal](18, 2) NOT NULL CONSTRAINT [DF_Hishop_Products_CubicMeter]  DEFAULT ((0)),
	[FreightWeight] [decimal](18, 2) NOT NULL CONSTRAINT [DF_Hishop_Products_FreightWeight]  DEFAULT ((0)),
	[ProductShortName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Hishop_Products] PRIMARY KEY NONCLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [Hishop_Products_Index]    Script Date: 2017/3/14 21:10:56 ******/
CREATE CLUSTERED INDEX [Hishop_Products_Index] ON [dbo].[Hishop_Products]
(
	[DisplaySequence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hishop_ProductTag]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_ProductTag](
	[TagId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_ProductTag] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_ProductTypeBrands]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_ProductTypeBrands](
	[ProductTypeId] [int] NOT NULL,
	[BrandId] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_ProductTypeBrands] PRIMARY KEY CLUSTERED 
(
	[ProductTypeId] ASC,
	[BrandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_ProductTypes]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_ProductTypes](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
 CONSTRAINT [PK_Hishop_ProductTypes] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_PromotionGame]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_PromotionGame](
	[GameId] [int] IDENTITY(1,1) NOT NULL,
	[GameType] [int] NOT NULL,
	[GameTitle] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[BeginTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[ApplyMembers] [varchar](50) NOT NULL,
	[NeedPoint] [int] NOT NULL,
	[GivePoint] [int] NOT NULL,
	[OnlyGiveNotPrizeMember] [bit] NOT NULL,
	[PlayType] [int] NOT NULL,
	[NotPrzeDescription] [nvarchar](1000) NULL,
	[GameUrl] [nvarchar](150) NULL,
	[GameQRCodeAddress] [nvarchar](150) NULL,
	[Status] [int] NOT NULL,
	[KeyWork] [varchar](100) NULL,
	[LimitEveryDay] [int] NOT NULL,
	[MaximumDailyLimit] [int] NOT NULL,
	[PrizeRate] [real] NOT NULL,
	[MemberCheck] [int] NOT NULL,
	[DefualtGroup] [varchar](50) NOT NULL,
	[CustomGroup] [varchar](500) NOT NULL,
 CONSTRAINT [PK_Hishop_PromotionGame] PRIMARY KEY CLUSTERED 
(
	[GameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_PromotionGamePrizes]    Script Date: 2017/3/14 21:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_PromotionGamePrizes](
	[PrizeId] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[PrizeGrade] [int] NOT NULL,
	[PrizeType] [int] NOT NULL,
	[GivePoint] [int] NULL,
	[GiveCouponId] [varchar](100) NULL,
	[GiveShopBookId] [varchar](100) NULL,
	[GriveShopBookPicUrl] [varchar](200) NULL,
	[PrizeCount] [int] NOT NULL,
	[PrizeRate] [int] NOT NULL,
	[PrizeName] [nvarchar](100) NOT NULL,
	[Prize] [nvarchar](100) NOT NULL,
	[IsLogistics] [int] NOT NULL,
	[PrizeImage] [varchar](500) NOT NULL,
 CONSTRAINT [PK_Hishop_PromotionGamePrizes] PRIMARY KEY CLUSTERED 
(
	[PrizeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_PromotionGameResultMembersLog]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_PromotionGameResultMembersLog](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[PrizeId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[PlayTime] [datetime] NOT NULL,
	[IsUsed] [bit] NOT NULL,
 CONSTRAINT [PK_Hishop_PromotionGameResultMembersLog] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_PromotionWinningPool]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_PromotionWinningPool](
	[WinningPoolId] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[GamePrizeId] [int] NOT NULL,
	[IsReceive] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_PromotionWinningPool] PRIMARY KEY CLUSTERED 
(
	[WinningPoolId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_ShareActivity]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_ShareActivity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CouponId] [int] NOT NULL,
	[BeginDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[MeetValue] [decimal](10, 2) NOT NULL,
	[CouponNumber] [int] NOT NULL,
	[CouponName] [nvarchar](50) NOT NULL,
	[ActivityName] [nvarchar](50) NULL,
	[ImgUrl] [varchar](500) NULL,
	[ShareTitle] [nvarchar](50) NULL,
	[Description] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_ShareActivity_Record]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_ShareActivity_Record](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[shareId] [int] NOT NULL,
	[shareUser] [int] NOT NULL,
	[attendUser] [int] NULL,
 CONSTRAINT [PK_Hishop_ShareActivity_Record] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Shippers]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Shippers](
	[ShipperId] [int] IDENTITY(1,1) NOT NULL,
	[DistributorUserId] [int] NOT NULL CONSTRAINT [DF_Hishop_Shippers_DistributorUserId]  DEFAULT ((0)),
	[IsDefault] [bit] NOT NULL,
	[ShipperTag] [nvarchar](50) NOT NULL,
	[ShipperName] [nvarchar](50) NOT NULL,
	[RegionId] [int] NOT NULL,
	[Address] [nvarchar](300) NOT NULL,
	[CellPhone] [nvarchar](20) NULL,
	[TelPhone] [nvarchar](20) NULL,
	[Zipcode] [nvarchar](20) NOT NULL,
	[Remark] [nvarchar](300) NULL,
 CONSTRAINT [PK_Hishop_Shippers] PRIMARY KEY CLUSTERED 
(
	[ShipperId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_ShoppingCarts]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_ShoppingCarts](
	[UserId] [int] NOT NULL,
	[SkuId] [nvarchar](100) NOT NULL,
	[Quantity] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[CategoryId] [int] NULL,
	[Templateid] [int] NULL,
	[Type] [int] NOT NULL,
	[ExchangeId] [int] NULL,
	[LimitedTimeDiscountId] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_ShoppingCarts] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[SkuId] ASC,
	[LimitedTimeDiscountId] ASC,
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_SKUItems]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_SKUItems](
	[SkuId] [nvarchar](100) NOT NULL,
	[AttributeId] [int] NOT NULL,
	[ValueId] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_SKUItems] PRIMARY KEY CLUSTERED 
(
	[SkuId] ASC,
	[AttributeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_SKUMemberPrice]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_SKUMemberPrice](
	[SkuId] [nvarchar](100) NOT NULL,
	[GradeId] [int] NOT NULL,
	[MemberSalePrice] [money] NOT NULL,
 CONSTRAINT [PK_Hishop_SKUMemberPrice] PRIMARY KEY CLUSTERED 
(
	[SkuId] ASC,
	[GradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_SKUs]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_SKUs](
	[SkuId] [nvarchar](100) NOT NULL,
	[ProductId] [int] NOT NULL,
	[SKU] [nvarchar](50) NULL,
	[Weight] [money] NULL,
	[Stock] [int] NOT NULL,
	[CostPrice] [money] NULL,
	[SalePrice] [money] NOT NULL,
 CONSTRAINT [PK_Hishop_SKUs] PRIMARY KEY CLUSTERED 
(
	[SkuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Tags]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Tags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[TagName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Hishop_Tags] PRIMARY KEY NONCLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_TemplateRelatedShipping]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_TemplateRelatedShipping](
	[ModeId] [int] NOT NULL,
	[ExpressCompanyName] [nvarchar](500) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_TempSendCouponUserLists]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_TempSendCouponUserLists](
	[UserId] [int] NULL,
	[AdminId] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_UserGroupSet]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_UserGroupSet](
	[ActiveDay] [int] NOT NULL DEFAULT ((1))
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_UserShippingAddresses]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_UserShippingAddresses](
	[RegionId] [int] NOT NULL,
	[ShippingId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ShipTo] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](500) NOT NULL,
	[Zipcode] [nvarchar](20) NOT NULL,
	[TelPhone] [nvarchar](20) NULL,
	[CellPhone] [nvarchar](20) NULL,
	[IsDefault] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Hishop_UserShippingAddresses] PRIMARY KEY CLUSTERED 
(
	[ShippingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_UserSign]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_UserSign](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[SignDay] [datetime] NOT NULL,
	[Continued] [int] NULL,
	[Stage] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_VoteItems]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_VoteItems](
	[VoteId] [bigint] NOT NULL,
	[VoteItemId] [bigint] IDENTITY(1,1) NOT NULL,
	[VoteItemName] [nvarchar](100) NOT NULL,
	[ItemCount] [int] NOT NULL,
 CONSTRAINT [PK_Hishop_VoteItems] PRIMARY KEY NONCLUSTERED 
(
	[VoteItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_VoteRecord]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_VoteRecord](
	[UserId] [int] NOT NULL,
	[VoteId] [bigint] NOT NULL,
 CONSTRAINT [PK_Hishop_VoteRecord] PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC,
	[VoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hishop_Votes]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hishop_Votes](
	[VoteId] [bigint] IDENTITY(1,1) NOT NULL,
	[VoteName] [nvarchar](100) NOT NULL,
	[IsBackup] [bit] NOT NULL,
	[MaxCheck] [int] NOT NULL,
	[ImageUrl] [nvarchar](500) NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Description] [ntext] NULL,
	[MemberGrades] [varchar](500) NULL,
	[DefualtGroup] [varchar](50) NOT NULL,
	[CustomGroup] [varchar](500) NOT NULL,
	[IsMultiCheck] [bit] NULL,
 CONSTRAINT [PK_Hishop_Votes] PRIMARY KEY NONCLUSTERED 
(
	[VoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hishop_Votes_Question]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hishop_Votes_Question](
	[QuestionId] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](500) NOT NULL,
	[VoteId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[QuestionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MeiQia_Userver]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MeiQia_Userver](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[unit] [nvarchar](50) NULL,
	[userver] [nvarchar](50) NULL,
	[password] [varchar](50) NULL,
	[nickname] [nvarchar](20) NULL,
	[realname] [nvarchar](20) NULL,
	[level] [varchar](10) NULL,
	[tel] [varchar](20) NULL,
 CONSTRAINT [PK_MeiQia_Userver] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Taobao_Products]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Taobao_Products](
	[Cid] [bigint] NOT NULL,
	[StuffStatus] [nvarchar](10) NULL,
	[ProductId] [int] NOT NULL,
	[ProTitle] [nvarchar](200) NOT NULL,
	[Num] [bigint] NOT NULL,
	[LocationState] [nvarchar](10) NOT NULL,
	[LocationCity] [nvarchar](10) NOT NULL,
	[FreightPayer] [nvarchar](10) NOT NULL,
	[PostFee] [money] NULL,
	[ExpressFee] [money] NULL,
	[EMSFee] [money] NULL,
	[HasInvoice] [bit] NOT NULL,
	[HasWarranty] [bit] NOT NULL,
	[HasDiscount] [bit] NOT NULL,
	[ValidThru] [bigint] NOT NULL,
	[ListTime] [datetime] NULL,
	[PropertyAlias] [nvarchar](4000) NULL,
	[InputPids] [nvarchar](2000) NULL,
	[InputStr] [nvarchar](2000) NULL,
	[SkuProperties] [nvarchar](4000) NULL,
	[SkuQuantities] [nvarchar](2000) NULL,
	[SkuPrices] [nvarchar](2000) NULL,
	[SkuOuterIds] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Taobao_Products] PRIMARY KEY NONCLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_Activity]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_Activity](
	[ActivityId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Description] [nvarchar](500) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[CloseRemark] [nvarchar](1000) NULL,
	[Keys] [nvarchar](50) NULL,
	[MaxValue] [nchar](10) NULL,
	[PicUrl] [nvarchar](300) NULL,
	[Item1] [nvarchar](50) NULL,
	[Item2] [nvarchar](50) NULL,
	[Item3] [nvarchar](50) NULL,
	[Item4] [nvarchar](50) NULL,
	[Item5] [nvarchar](50) NULL,
 CONSTRAINT [PK_vshop_Activity] PRIMARY KEY CLUSTERED 
(
	[ActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_ActivitySignUp]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_ActivitySignUp](
	[ActivitySignUpId] [int] IDENTITY(1,1) NOT NULL,
	[ActivityId] [int] NULL,
	[UserId] [int] NULL,
	[UserName] [nvarchar](256) NULL,
	[RealName] [nvarchar](50) NULL,
	[SignUpDate] [datetime] NULL,
	[Item1] [nvarchar](200) NULL,
	[Item2] [nvarchar](200) NULL,
	[Item3] [nvarchar](200) NULL,
	[Item4] [nvarchar](200) NULL,
	[Item5] [nvarchar](200) NULL,
 CONSTRAINT [PK_vshop_ActivitySignUp] PRIMARY KEY CLUSTERED 
(
	[ActivitySignUpId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_AlarmNotify]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_AlarmNotify](
	[AlarmNotifyId] [int] IDENTITY(1,1) NOT NULL,
	[AppId] [nvarchar](100) NULL,
	[ErrorType] [int] NULL,
	[Description] [nvarchar](500) NULL,
	[AlarmContent] [nvarchar](2000) NULL,
	[TimeStamp] [datetime] NULL,
 CONSTRAINT [PK_Hishop_AlarmNotify] PRIMARY KEY NONCLUSTERED 
(
	[AlarmNotifyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_AliFuwuReply]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_AliFuwuReply](
	[ReplyId] [int] IDENTITY(1,1) NOT NULL,
	[Keys] [nvarchar](200) NULL,
	[MatchType] [int] NULL,
	[ReplyType] [int] NULL,
	[MessageType] [int] NULL,
	[IsDisable] [bit] NULL,
	[LastEditDate] [datetime] NOT NULL,
	[LastEditor] [nvarchar](255) NULL,
	[Content] [ntext] NULL,
	[Type] [int] NULL,
	[ActivityId] [int] NULL,
	[ArticleID] [int] NOT NULL,
 CONSTRAINT [PK_vshopAliFuwuReply] PRIMARY KEY CLUSTERED 
(
	[ReplyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_AlipayActiveOpendId]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[vshop_AlipayActiveOpendId](
	[AliOpenID] [varchar](128) NOT NULL,
	[PubTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AliOpenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[vshop_Article]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[vshop_Article](
	[ArticleId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NULL,
	[ArticleType] [int] NOT NULL CONSTRAINT [DF_vshop_Article_ArticleType]  DEFAULT ((0)),
	[LinkType] [int] NOT NULL CONSTRAINT [DF_vshop_Article_LinkType]  DEFAULT ((0)),
	[Content] [ntext] NULL,
	[ImageUrl] [nvarchar](255) NULL,
	[Url] [nvarchar](255) NULL,
	[Memo] [nvarchar](255) NULL,
	[PubTime] [datetime] NOT NULL DEFAULT (getdate()),
	[mediaid] [varchar](100) NULL,
	[IsShare] [bit] NOT NULL DEFAULT ((1)),
 CONSTRAINT [PK_vshop_Article] PRIMARY KEY CLUSTERED 
(
	[ArticleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[vshop_ArticleItems]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[vshop_ArticleItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ArticleId] [int] NOT NULL,
	[Title] [nvarchar](50) NULL,
	[Content] [ntext] NULL,
	[ImageUrl] [nvarchar](255) NULL,
	[Url] [nvarchar](255) NULL,
	[LinkType] [int] NOT NULL,
	[PubTime] [datetime] NOT NULL,
	[mediaid] [varchar](100) NULL,
 CONSTRAINT [PK_vshop_ArticleItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Vshop_CustomGrouping]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vshop_CustomGrouping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](50) NOT NULL,
	[UserCount] [int] NOT NULL DEFAULT ((0)),
	[Memo] [nvarchar](2000) NULL,
	[UpdateTime] [datetime] NOT NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vshop_CustomGroupingUser]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vshop_CustomGroupingUser](
	[GroupId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_FeedBackNotify]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_FeedBackNotify](
	[FeedBackNotifyID] [int] IDENTITY(1,1) NOT NULL,
	[AppId] [nvarchar](100) NULL,
	[TimeStamp] [datetime] NULL,
	[OpenId] [nvarchar](100) NULL,
	[MsgType] [nvarchar](20) NULL,
	[FeedBackId] [nvarchar](255) NULL,
	[TransId] [nvarchar](255) NULL,
	[Reason] [nvarchar](1000) NULL,
	[Solution] [nvarchar](1000) NULL,
	[ExtInfo] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Hishop_FeedBackNotify] PRIMARY KEY NONCLUSTERED 
(
	[FeedBackNotifyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vshop_FollowUsers]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Vshop_FollowUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OpenId] [varchar](100) NOT NULL,
	[FollowTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Vshop_HomeProducts]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vshop_HomeProducts](
	[ProductId] [int] NOT NULL,
	[DisplaySequence] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vshop_HomeTopics]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vshop_HomeTopics](
	[TopicId] [int] NOT NULL,
	[DisplaySequence] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_IntegralDetail]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[vshop_IntegralDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IntegralSourceType] [int] NOT NULL,
	[IntegralSource] [varchar](100) NOT NULL,
	[IntegralChange] [money] NOT NULL,
	[Remark] [varchar](500) NULL,
	[Userid] [int] NOT NULL,
	[GoToUrl] [varchar](100) NULL,
	[TrateTime] [datetime] NOT NULL CONSTRAINT [DF_TrateTime]  DEFAULT (getdate()),
	[IntegralStatus] [int] NULL CONSTRAINT [DF_vshop_IntegralDetail_IntegralStatus]  DEFAULT ((0)),
 CONSTRAINT [PK_IntegralDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[vshop_Menu]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_Menu](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[ParentMenuId] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[ReplyId] [int] NULL,
	[DisplaySequence] [int] NULL,
	[Bind] [int] NULL,
	[Content] [nvarchar](300) NULL,
 CONSTRAINT [PK_vshop_Menu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_Menu_Fuwu]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_Menu_Fuwu](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[ParentMenuId] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[ReplyId] [int] NULL,
	[DisplaySequence] [int] NULL,
	[Bind] [int] NULL,
	[Content] [nvarchar](300) NULL,
 CONSTRAINT [PK_vshop_Menu_fuwu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_Message]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_Message](
	[ReplyId] [int] NULL,
	[MsgID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[ImageUrl] [nvarchar](255) NULL,
	[Url] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Content] [ntext] NULL,
 CONSTRAINT [PK_Hishop_Message] PRIMARY KEY NONCLUSTERED 
(
	[MsgID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VShop_NavMenu]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VShop_NavMenu](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[ParentMenuId] [int] NULL,
	[Name] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[DisplaySequence] [int] NULL,
	[Content] [varchar](500) NULL,
	[ShopMenuPic] [varchar](100) NULL,
 CONSTRAINT [PK_VShop_NavMenu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Vshop_OneyuanTao_Detail]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Vshop_OneyuanTao_Detail](
	[ActivityId] [varchar](50) NOT NULL,
	[IsOn] [bit] NOT NULL DEFAULT ((1)),
	[Title] [nvarchar](30) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[HeadImgage] [varchar](300) NOT NULL,
	[ReachType] [int] NOT NULL,
	[ActivityDec] [nvarchar](100) NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductPrice] [money] NOT NULL,
	[ProductImg] [varchar](300) NOT NULL,
	[ProductTitle] [nvarchar](200) NOT NULL,
	[SkuId] [nvarchar](100) NOT NULL,
	[PrizeNumber] [int] NOT NULL,
	[EachPrice] [money] NOT NULL,
	[EachCanBuyNum] [int] NOT NULL,
	[FitMember] [varchar](500) NOT NULL DEFAULT ('-1'),
	[DefualtGroup] [varchar](50) NOT NULL DEFAULT ('-1'),
	[CustomGroup] [varchar](500) NOT NULL DEFAULT ('-1'),
	[ReachNum] [int] NOT NULL,
	[PrizeTime] [datetime] NULL,
	[FinishedNum] [int] NOT NULL DEFAULT ((0)),
	[IsEnd] [bit] NOT NULL DEFAULT ((0)),
	[IsAllRefund] [bit] NOT NULL DEFAULT ((0)),
	[PrizeCountInfo] [text] NULL,
	[HasCalculate] [bit] NOT NULL DEFAULT ((0)),
	[IsSuccess] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK__tmp_ms_x__45F4A7916EAB62A3] PRIMARY KEY CLUSTERED 
(
	[ActivityId] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Vshop_OneyuanTao_ParticipantMember]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Vshop_OneyuanTao_ParticipantMember](
	[Pid] [varchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
	[BuyTime] [datetime] NOT NULL,
	[BuyNum] [int] NOT NULL DEFAULT ((0)),
	[IsPay] [bit] NOT NULL DEFAULT ((0)),
	[ActivityId] [varchar](50) NOT NULL,
	[SkuId] [varchar](100) NOT NULL DEFAULT ((0)),
	[SkuIdStr] [varchar](100) NULL,
	[ProductPrice] [decimal](18, 0) NOT NULL DEFAULT ((0)),
	[PayTime] [datetime] NULL,
	[PayWay] [varchar](50) NULL,
	[PayNum] [varchar](50) NULL,
	[Remark] [nvarchar](300) NULL,
	[TotalPrice] [money] NOT NULL,
	[IsRefund] [bit] NOT NULL DEFAULT ((0)),
	[RefundNum] [varchar](50) NULL,
	[RefundTime] [datetime] NULL,
	[RefundErr] [bit] NOT NULL DEFAULT ((0)),
	[IsWin] [bit] NOT NULL DEFAULT ((0)),
	[WinIds] [varchar](100) NULL,
	[WinTime] [datetime] NULL,
	[out_refund_no] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Pid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Vshop_OneyuanTao_WinningRecord]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Vshop_OneyuanTao_WinningRecord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ActivityId] [varchar](50) NOT NULL,
	[Pid] [varchar](50) NOT NULL,
	[PrizeNum] [varchar](20) NOT NULL,
	[IsWin] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[vshop_OrderRedPager]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_OrderRedPager](
	[OrderID] [nvarchar](50) NOT NULL,
	[RedPagerActivityId] [int] NOT NULL,
	[RedPagerActivityName] [nvarchar](100) NOT NULL,
	[MaxGetTimes] [int] NOT NULL,
	[AlreadyGetTimes] [int] NOT NULL,
	[ItemAmountLimit] [money] NOT NULL,
	[OrderAmountCanUse] [money] NOT NULL,
	[ExpiryDays] [int] NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_vshop_OrderRedPager] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_OrderSplit]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[vshop_OrderSplit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderIDNum] [int] NOT NULL,
	[OldOrderID] [nvarchar](50) NOT NULL,
	[ItemList] [varchar](2000) NOT NULL,
	[UpdateTime] [datetime] NOT NULL DEFAULT (getdate()),
	[AdjustedFreight] [money] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VShop_Region]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VShop_Region](
	[RegionId] [int] NOT NULL,
	[RegionName] [varchar](50) NULL,
 CONSTRAINT [PK_VShop_Region] PRIMARY KEY CLUSTERED 
(
	[RegionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[vshop_Reply]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_Reply](
	[ReplyId] [int] IDENTITY(1,1) NOT NULL,
	[Keys] [nvarchar](200) NULL,
	[MatchType] [int] NULL,
	[ReplyType] [int] NULL,
	[MessageType] [int] NULL,
	[IsDisable] [bit] NULL,
	[LastEditDate] [datetime] NOT NULL,
	[LastEditor] [nvarchar](255) NULL,
	[Content] [ntext] NULL,
	[Type] [int] NULL,
	[ActivityId] [int] NULL,
	[ArticleID] [int] NOT NULL CONSTRAINT [DF_vshop_Reply_ArticleID]  DEFAULT ((0)),
 CONSTRAINT [PK_vshop_Reply] PRIMARY KEY CLUSTERED 
(
	[ReplyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_ScanOpenID]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[vshop_ScanOpenID](
	[AutoID] [int] IDENTITY(1,1) NOT NULL,
	[ScanDate] [datetime] NULL,
	[AppID] [varchar](200) NULL,
	[SCannerUserOpenID] [varchar](200) NULL,
	[SCannerUserNickName] [varchar](200) NULL,
 CONSTRAINT [PK_vshop_ScanOpenID] PRIMARY KEY CLUSTERED 
(
	[AutoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Vshop_ScanSceneInfos]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Vshop_ScanSceneInfos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Platform] [varchar](10) NOT NULL,
	[Sceneid] [nchar](10) NOT NULL,
	[BindUserId] [varchar](50) NOT NULL,
	[DescInfo] [nvarchar](100) NOT NULL DEFAULT ('推广二维码'),
	[type] [int] NOT NULL DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL,
	[CodeUrl] [varchar](300) NULL,
	[LastActiveTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[vshop_SendRedpackRecord]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[vshop_SendRedpackRecord](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BalanceDrawRequestID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[OpenId] [nvarchar](128) NOT NULL,
	[Amount] [int] NOT NULL,
	[ActName] [nvarchar](32) NULL,
	[Wishing] [nvarchar](128) NULL,
	[ClientIP] [varchar](20) NULL,
	[IsSend] [bit] NOT NULL DEFAULT ((0)),
	[SendTime] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Vshop_ShareMaterial_NineImages]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vshop_ShareMaterial_NineImages](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[ShareDesc] [nvarchar](100) NULL,
	[CreatTime] [datetime] NOT NULL,
	[image1] [nvarchar](255) NOT NULL,
	[image2] [nvarchar](255) NOT NULL,
	[image3] [nvarchar](255) NOT NULL,
	[image4] [nvarchar](255) NOT NULL,
	[image5] [nvarchar](255) NOT NULL,
	[image6] [nvarchar](255) NOT NULL,
	[image7] [nvarchar](255) NOT NULL,
	[image8] [nvarchar](255) NOT NULL,
	[image9] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Vshop_ShareMaterial_NineImages] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_Statistics_Distributors]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_Statistics_Distributors](
	[RecDate] [datetime] NOT NULL,
	[AgentID] [int] NOT NULL,
	[OrderNumber] [int] NOT NULL CONSTRAINT [DF_vshop_Statistics_Distributors_OrderNumber]  DEFAULT ((0)),
	[SaleAmountFee] [money] NOT NULL CONSTRAINT [DF_vshop_Statistics_Distributors_SaleAmountFee]  DEFAULT ((0)),
	[BuyerNumber] [int] NOT NULL CONSTRAINT [DF_vshop_Statistics_Distributors_BuyerNumber]  DEFAULT ((0)),
	[BuyerAvgPrice] [money] NOT NULL CONSTRAINT [DF_vshop_Statistics_Distributors_BuyerAvgPrice]  DEFAULT ((0)),
	[CommissionAmountFee] [money] NOT NULL CONSTRAINT [DF_vshop_Statistics_Distributors_CommissionAmountFee]  DEFAULT ((0)),
 CONSTRAINT [PK_vshop_Statistics_Distributors] PRIMARY KEY CLUSTERED 
(
	[RecDate] ASC,
	[AgentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_Statistics_Globals]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_Statistics_Globals](
	[RecDate] [datetime] NOT NULL,
	[OrderNumber] [int] NULL,
	[SaleAmountFee] [money] NULL,
	[BuyerNumber] [int] NULL,
	[UserAvgPrice] [money] NULL,
	[FXOrderNumber] [int] NULL,
	[FXSaleAmountFee] [money] NULL,
	[FXResultPercent] [numeric](18, 3) NULL,
	[CommissionFee] [money] NULL,
	[NewMemberNumber] [int] NULL,
	[NewAgentNumber] [int] NULL,
	[FinishedDrawCommissionFee] [money] NULL DEFAULT ((0)),
	[WaitDrawCommissionFee] [money] NULL DEFAULT ((0)),
 CONSTRAINT [PK_vshop_Statistics_Globals] PRIMARY KEY CLUSTERED 
(
	[RecDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_Statistics_Log]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[vshop_Statistics_Log](
	[RecDate] [datetime] NOT NULL,
	[FirstCountTime] [datetime] NULL,
	[BeginTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[IsSuccess] [int] NULL CONSTRAINT [DF_vshop_Statistics_Log_IsSuccess]  DEFAULT ((0)),
	[ErrorStep] [varchar](256) NULL,
	[LogInfo] [varchar](4000) NULL,
 CONSTRAINT [PK_vshop_Statistics_Log] PRIMARY KEY CLUSTERED 
(
	[RecDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[vshop_Statistics_NotifyLog]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[vshop_Statistics_NotifyLog](
	[AutoID] [int] IDENTITY(1,1) NOT NULL,
	[RecDate] [datetime] NULL,
	[TaskTime] [datetime] NULL,
	[FuncAction] [int] NULL,
	[ActionDesc] [varchar](255) NULL,
 CONSTRAINT [PK_vshop_Statistics_NotifyLog] PRIMARY KEY CLUSTERED 
(
	[AutoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[vshop_Statistics_Products]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_Statistics_Products](
	[RecDate] [datetime] NOT NULL,
	[ProductID] [int] NOT NULL,
	[SaleQty] [int] NOT NULL CONSTRAINT [DF_vshop_Statistics_Goods_SaleQty]  DEFAULT ((0)),
	[SaleAmountFee] [money] NOT NULL CONSTRAINT [DF_vshop_Statistics_Goods_SaleAmountFee]  DEFAULT ((0)),
	[TotalVisits] [int] NOT NULL CONSTRAINT [DF_vshop_Statistics_Goods_TotalVisits]  DEFAULT ((0)),
	[BuyerNumber] [int] NOT NULL CONSTRAINT [DF_vshop_Statistics_Goods_BuyerNumber]  DEFAULT ((0)),
	[ConversionRate] [numeric](18, 3) NULL CONSTRAINT [DF_vshop_Statistics_Goods_ConversionRate]  DEFAULT ((0)),
 CONSTRAINT [PK_vshop_Statistics_Goods] PRIMARY KEY CLUSTERED 
(
	[RecDate] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vshop_Statistics_ProductsVisits]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vshop_Statistics_ProductsVisits](
	[RecDate] [datetime] NOT NULL,
	[ProductID] [int] NOT NULL,
	[TotalVisits] [int] NOT NULL CONSTRAINT [DF_vshop_Statistics_GoodsVisits_TotalVisits]  DEFAULT ((0)),
 CONSTRAINT [PK_vshop_Statistics_GoodsVisits] PRIMARY KEY CLUSTERED 
(
	[RecDate] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Weibo_Menu]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Weibo_Menu](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[ParentMenuId] [int] NULL,
	[Name] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[DisplaySequence] [int] NULL,
	[Content] [varchar](500) NULL,
 CONSTRAINT [PK_Weibo_Menu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Weibo_Message]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Weibo_Message](
	[MessageId] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Receiver_id] [varchar](50) NOT NULL,
	[Sender_id] [varchar](50) NOT NULL,
	[Created_at] [datetime] NOT NULL,
	[Text] [varchar](1000) NULL,
	[Vfid] [varchar](50) NULL,
	[Tovfid] [varchar](50) NULL,
	[Status] [int] NOT NULL,
	[SenderMessage] [varchar](1000) NULL,
	[SenderDate] [datetime] NULL,
	[Display_Name] [varchar](100) NULL,
	[Summary] [varchar](1000) NULL,
	[Image] [varchar](255) NULL,
	[Url] [varchar](255) NULL,
	[ArticleId] [int] NULL,
	[Access_Token] [varchar](70) NULL,
 CONSTRAINT [PK_Weibo_Message] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Weibo_Reply]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Weibo_Reply](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReplyKeyId] [int] NULL,
	[ArticleId] [int] NULL,
	[IsDisable] [bit] NOT NULL,
	[EditDate] [datetime] NULL,
	[Content] [ntext] NULL,
	[Type] [int] NOT NULL,
	[ReceiverType] [varchar](50) NOT NULL,
	[Displayname] [varchar](500) NULL,
	[Summary] [varchar](500) NULL,
	[Image] [varchar](500) NULL,
	[Url] [varchar](500) NULL,
 CONSTRAINT [PK_Weibo_Reply] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Weibo_ReplyKeys]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Weibo_ReplyKeys](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Keys] [nvarchar](200) NOT NULL,
	[Type] [int] NOT NULL,
	[Matching] [int] NOT NULL,
 CONSTRAINT [PK_Weibo_ReplyKeys] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WeiXin_RecentOpenID]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WeiXin_RecentOpenID](
	[OpenID] [varchar](128) NOT NULL,
	[PubTime] [datetime] NOT NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[OpenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WeiXin_SendAll]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WeiXin_SendAll](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NULL,
	[MessageType] [int] NOT NULL,
	[ArticleID] [int] NOT NULL,
	[Content] [nvarchar](4000) NULL,
	[SendState] [int] NOT NULL,
	[SendTime] [datetime] NOT NULL,
	[SendCount] [int] NOT NULL,
	[msgid] [varchar](50) NULL,
	[TotalCount] [int] NOT NULL,
	[ReturnJsonData] [nvarchar](1000) NULL,
	[platform] [int] NOT NULL,
 CONSTRAINT [PK_WeiXin_SendAll] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[vw_Vshop_OneyuanWinnerList]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Vshop_OneyuanWinnerList]
AS
SELECT     A.Pid,A.ActivityId,A.UserId,
                          (SELECT     cast(PrizeNum AS varchar(10)) + ','
                            FROM          Vshop_OneyuanTao_WinningRecord
                            WHERE      Pid = A.Pid and IsWin=1
                            ORDER BY Pid,ActivityId,UserId FOR XML PATH('')) AS PrizeNums
FROM         Vshop_OneyuanTao_WinningRecord A where A.IsWin=1
GROUP BY A.Pid,A.ActivityId,UserId

GO
/****** Object:  View [dbo].[vw_Vshop_ProductPrizeLIst]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 


 CREATE VIEW [dbo].[vw_Vshop_ProductPrizeLIst]
AS
SELECT  '' AS PrizeNums,p.PrizeGrade,g.GameType,  0 as Ptype, m.LogId,'0' as Pid,PlayTime as WinTime,g.GameTitle as Title,ps.ProductName,' ' as SkuIdStr,p.GriveShopBookPicUrl as ThumbnailUrl100 ,p.IsLogistics,p.PrizeType
FROM         dbo.Hishop_PromotionGameResultMembersLog AS m INNER JOIN
                      dbo.Hishop_PromotionGamePrizes AS p ON m.PrizeId = p.PrizeId INNER JOIN
                      dbo.Hishop_PromotionGame AS g ON m.GameId = g.GameId INNER JOIN
                      dbo.Hishop_Products AS ps ON p.GiveShopBookId = ps.ProductId  and p.PrizeType=2      
Union 
SELECT   w.PrizeNums,5 as PrizeGrade, 5 as GameType,  1 as Ptype,  0 as LogId,w.Pid,d.PrizeTime as WinTime,d.Title,d.ProductTitle,p.SkuIdStr,d.ProductImg  as ThumbnailUrl100, '0' as IsLogistics,'0' as PrizeType FROM  vw_Vshop_OneyuanWinnerList  w, 
 dbo.Vshop_OneyuanTao_ParticipantMember p ,Vshop_OneyuanTao_Detail D 
 where  w.Pid = p.Pid and w.ActivityId=d.ActivityId 
 Union 
 SELECT  '' AS PrizeNums,p.PrizeGrade,g.GameType,  0 as Ptype, m.LogId,'0' as Pid,PlayTime as WinTime,g.GameTitle as Title,p.Prize as ProductName,' ' as SkuIdStr,p.PrizeImage as ThumbnailUrl100 ,p.IsLogistics,p.PrizeType
FROM         dbo.Hishop_PromotionGameResultMembersLog AS m INNER JOIN
                      dbo.Hishop_PromotionGamePrizes AS p ON m.PrizeId = p.PrizeId INNER JOIN
                      dbo.Hishop_PromotionGame AS g ON m.GameId = g.GameId   and  p.PrizeType =3 and p.IsLogistics=1

GO
/****** Object:  View [dbo].[vw_Vshop_ProductPrizeLIst_WithDelievelyInfo]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Vshop_ProductPrizeLIst_WithDelievelyInfo]
	AS 
SELECT v.*,Isnull(r.Id,0) as Id,Receiver,r.Tel,Isnull(r.status,0) as status,r.DeliveryTime,r.ReceiveTime,r.ExpressName,r.CourierNumber,r.ReggionPath,r.Address,r.RecordType
FROM vw_Vshop_ProductPrizeLIst v left join 
Hishop_PrizesDeliveryRecord r on (v.LogId = r.LogId and v.LogId!=0)  or  (v.LogId=0 and v.Pid=r.Pid)

GO
/****** Object:  View [dbo].[vw_BuyerMember]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[vw_BuyerMember]
as

select a.* from aspnet_Members a
	left join aspnet_Distributors D on a.UserId= D.UserId
	where  1=1
	and D.UserId is null

GO
/****** Object:  View [dbo].[vw_Hishop_Activities_Detail]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_Activities_Detail]
AS
SELECT     d.Id, d.ActivitiesId, d.MeetMoney, d.ReductionMoney, d.bFreeShipping, d.Integral, d.CouponId, d.MeetNumber, a.ActivitiesName, a.MemberGrades,a.CustomGroup,a.DefualtGroup, a.StartTime, 
                      a.EndTIme, a.isAllProduct
FROM         dbo.Hishop_Activities_Detail AS d LEFT OUTER JOIN
                      dbo.Hishop_Activities AS a ON a.ActivitiesId = d.ActivitiesId

GO
/****** Object:  View [dbo].[vw_Hishop_BalanceDrawRequesDistributors]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_BalanceDrawRequesDistributors]
AS
SELECT     dbo.Hishop_BalanceDrawRequest.SerialID, dbo.Hishop_BalanceDrawRequest.UserId, dbo.Hishop_BalanceDrawRequest.RequestType, 
                      dbo.Hishop_BalanceDrawRequest.UserName, dbo.Hishop_BalanceDrawRequest.RequestTime, dbo.Hishop_BalanceDrawRequest.Amount, 
                      dbo.Hishop_BalanceDrawRequest.AccountName, dbo.Hishop_BalanceDrawRequest.CellPhone, dbo.Hishop_BalanceDrawRequest.MerchantCode, 
                      dbo.Hishop_BalanceDrawRequest.Remark, dbo.Hishop_BalanceDrawRequest.RedpackRecordNum, dbo.Hishop_BalanceDrawRequest.IsCheck, 
                      dbo.Hishop_BalanceDrawRequest.CheckTime, dbo.aspnet_Distributors.StoreName, dbo.aspnet_Distributors.ReferralBlance, dbo.Hishop_BalanceDrawRequest.bankName
FROM         dbo.aspnet_Distributors INNER JOIN
                      dbo.Hishop_BalanceDrawRequest ON dbo.aspnet_Distributors.UserId = dbo.Hishop_BalanceDrawRequest.UserId

GO
/****** Object:  View [dbo].[vw_Hishop_BargainList]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_BargainList]
	AS select a.ProductName,a.MarketPrice,a.ThumbnailUrl60,a.SaleStatus, (SELECT   MIN(SalePrice) AS Expr1
                     FROM      dbo.Hishop_SKUs
                     WHERE   (ProductId = b.ProductId)) AS SalePrice,CASE WHEN BeginDate <GETDATE() and GETDATE()<EndDate  THEN '进行中'
WHEN BeginDate >GETDATE()  THEN '未开始'
WHEN EndDate < GETDATE()  THEN '已结束'
ELSE NULL END bargainstatus,b.*  from Hishop_Products a join Hishop_Bargain b on a.ProductId=b.ProductId

GO
/****** Object:  View [dbo].[vw_Hishop_BrandTypeAndBrandCategories]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_BrandTypeAndBrandCategories]
AS
SELECT     dbo.Hishop_BrandCategories.*, dbo.Hishop_ProductTypeBrands.ProductTypeId
FROM         dbo.Hishop_BrandCategories INNER JOIN
                      dbo.Hishop_ProductTypeBrands ON dbo.Hishop_BrandCategories.BrandId = dbo.Hishop_ProductTypeBrands.BrandId

GO
/****** Object:  View [dbo].[vw_Hishop_BrowseProductImgList]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_BrowseProductImgList]
AS


SELECT CategoryId, TypeId,BrandId,ProductId,ProductName,SaleStatus,MainCategoryPath,ExtendCategoryPath,AddedDate,ImageUrl1 as img FROM Hishop_Products where ImageUrl1!=''
union 
SELECT CategoryId, TypeId,BrandId,ProductId,ProductName,SaleStatus,MainCategoryPath,ExtendCategoryPath,AddedDate,ImageUrl2 as img FROM Hishop_Products where ImageUrl2!=''
union 
SELECT CategoryId, TypeId,BrandId,ProductId,ProductName,SaleStatus,MainCategoryPath,ExtendCategoryPath,AddedDate,ImageUrl3 as img FROM Hishop_Products where ImageUrl3!=''
union 
SELECT CategoryId, TypeId,BrandId,ProductId,ProductName,SaleStatus,MainCategoryPath,ExtendCategoryPath,AddedDate,ImageUrl4 as img FROM Hishop_Products where ImageUrl4!=''
union 
SELECT CategoryId, TypeId,BrandId,ProductId,ProductName,SaleStatus,MainCategoryPath,ExtendCategoryPath,AddedDate,ImageUrl5 as img FROM Hishop_Products where ImageUrl5!=''

GO
/****** Object:  View [dbo].[vw_Hishop_BrowseProductList]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_BrowseProductList]
AS
SELECT CategoryId, TypeId,BrandId,ProductId,ProductName,ProductShortName,ProductCode,ShortDescription,MarketPrice,
ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310,
 SaleStatus, DisplaySequence, MainCategoryPath, ExtendCategoryPath, SaleCounts, ShowSaleCounts, AddedDate, VistiCounts, MaxShowPrice,
 ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,
MinShowPrice AS SalePrice,
(SELECT TOP 1 SkuId FROM Hishop_SKUs WHERE ProductId = p.ProductId ORDER BY SalePrice) AS SkuId,
(SELECT SUM(Stock) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Stock,
(SELECT TOP 1 [Weight] FROM Hishop_SKUs WHERE ProductId = p.ProductId ORDER BY SalePrice) AS [Weight],
(SELECT COUNT(*) FROM Taobao_Products WHERE ProductId = p.ProductId) AS IsMakeTaobao
FROM Hishop_Products p

GO
/****** Object:  View [dbo].[vw_Hishop_CommissionDistributors]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_CommissionDistributors]
AS
SELECT     dbo.Hishop_Commissions.CommId, dbo.Hishop_Commissions.UserId, dbo.Hishop_Commissions.ReferralUserId, dbo.Hishop_Commissions.OrderId, 
                      dbo.Hishop_Commissions.TradeTime, dbo.Hishop_Commissions.OrderTotal, dbo.Hishop_Commissions.CommTotal, dbo.Hishop_Commissions.CommType, 
                      dbo.Hishop_Commissions.CommRemark, dbo.aspnet_Distributors.StoreName, dbo.aspnet_Distributors.ReferralBlance, 
                      dbo.aspnet_Distributors.ReferralRequestBalance,dbo.aspnet_Distributors.GradeId,dbo.Hishop_Commissions.State

FROM         dbo.Hishop_Commissions INNER JOIN 
                      dbo.aspnet_Distributors ON dbo.Hishop_Commissions.UserId = dbo.aspnet_Distributors.UserId

GO
/****** Object:  View [dbo].[vw_Hishop_CommissionDistributorsAddFromStore]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_CommissionDistributorsAddFromStore]
AS
SELECT     dbo.Hishop_Commissions.CommId, dbo.Hishop_Commissions.UserId, dbo.Hishop_Commissions.ReferralUserId, dbo.Hishop_Commissions.OrderId, 
                      dbo.Hishop_Commissions.TradeTime, dbo.Hishop_Commissions.OrderTotal, dbo.Hishop_Commissions.CommTotal, dbo.Hishop_Commissions.CommType, 
                      dbo.Hishop_Commissions.CommRemark, dbo.aspnet_Distributors.StoreName AS fromStoreName, dbo.aspnet_Distributors.ReferralBlance, 
                      dbo.aspnet_Distributors.ReferralRequestBalance, dbo.aspnet_Distributors.GradeId, dbo.Hishop_Commissions.State, dbo.aspnet_Distributors.ReferralPath, 
                      dbo.Hishop_Orders.RealName, dbo.Hishop_Orders.Username, aspnet_Distributors_1.StoreName
FROM         dbo.Hishop_Commissions INNER JOIN
                      dbo.aspnet_Distributors ON dbo.Hishop_Commissions.ReferralUserId = dbo.aspnet_Distributors.UserId LEFT JOIN
                      dbo.Hishop_Orders ON dbo.Hishop_Commissions.OrderId = dbo.Hishop_Orders.OrderId INNER JOIN
                      dbo.aspnet_Distributors AS aspnet_Distributors_1 ON dbo.Hishop_Commissions.UserId = aspnet_Distributors_1.UserId

GO
/****** Object:  View [dbo].[vw_Hishop_CommissionDistributorsOnlyForStoreName]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_CommissionDistributorsOnlyForStoreName]
AS
SELECT     dbo.Hishop_Commissions.CommId, dbo.Hishop_Commissions.UserId, dbo.Hishop_Commissions.ReferralUserId, dbo.Hishop_Commissions.OrderId, 
                      dbo.Hishop_Commissions.TradeTime, dbo.Hishop_Commissions.OrderTotal, dbo.Hishop_Commissions.CommTotal, dbo.Hishop_Commissions.CommType, 
                      dbo.Hishop_Commissions.CommRemark, dbo.aspnet_Distributors.StoreName AS fromStoreName, dbo.aspnet_Distributors.ReferralBlance, 
                      dbo.aspnet_Distributors.ReferralRequestBalance, dbo.aspnet_Distributors.GradeId, dbo.Hishop_Commissions.State, dbo.aspnet_Distributors.ReferralPath, 
                      dbo.Hishop_Orders.RealName, dbo.Hishop_Orders.Username, aspnet_Distributors.StoreName
FROM         dbo.Hishop_Commissions left JOIN
                      dbo.aspnet_Distributors ON dbo.Hishop_Commissions.UserID = dbo.aspnet_Distributors.UserId LEFT JOIN
                      dbo.Hishop_Orders ON dbo.Hishop_Commissions.OrderId = dbo.Hishop_Orders.OrderId

GO
/****** Object:  View [dbo].[vw_Hishop_CommissionRanking]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_CommissionRanking]
	AS 
	SELECT     d.StoreName, d.ReferralBlance, d.ReferralRequestBalance, d.GradeId, d.ReferralPath, d.Logo, c.CommId, d.UserId, ISNULL(c.ReferralUserId, d.UserId) 
                      AS ReferralUserId, c.OrderId, c.TradeTime, ISNULL(c.OrderTotal, 0) AS OrderTotal, ISNULL(c.CommTotal, 0) AS CommTotal, c.CommType, c.CommRemark, c.State, 
                      ISNULL(o.UserId, 0) AS BuyUserId, o.Username AS BuyUsername
FROM         dbo.aspnet_Distributors AS d LEFT OUTER JOIN
                      dbo.Hishop_Commissions AS c ON d.UserId = c.UserId LEFT OUTER JOIN
                      dbo.Hishop_Orders AS o ON o.OrderId = c.OrderId
 UNION
SELECT     d .StoreName, d .ReferralBlance, d .ReferralRequestBalance, d .GradeId, d .ReferralPath, d .Logo, c.CommId, d .UserId, d .UserId AS ReferralUserId, c.OrderId, 
                      c.TradeTime, ISNULL(c.OrderTotal, 0) AS OrderTotal, ISNULL(c.CommTotal, 0) AS CommTotal, c.CommType, c.CommRemark, c.State, ISNULL(c.UserId, 0) 
                      AS BuyUserId, c.Username AS BuyUsername
FROM         dbo.aspnet_Distributors AS d LEFT OUTER JOIN
                          (SELECT     0 AS UserId, NULL AS OrderId, NULL AS TradeTime, 0 AS OrderTotal, 0 AS CommTotal, NULL AS CommType, NULL AS CommRemark, NULL AS State, 
                                                   0 AS BuyUserId, NULL AS BuyUsername, NULL AS Username, NULL AS CommId) c ON d .UserId = c.UserId

GO
/****** Object:  View [dbo].[vw_Hishop_CommissionWithBuyUserId]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_CommissionWithBuyUserId]
AS
SELECT     dbo.Hishop_Commissions.CommId, dbo.Hishop_Commissions.UserId, dbo.Hishop_Commissions.ReferralUserId, dbo.Hishop_Commissions.OrderId, 
                      dbo.Hishop_Commissions.TradeTime, dbo.Hishop_Commissions.OrderTotal, dbo.Hishop_Commissions.CommTotal, dbo.Hishop_Commissions.CommType, 
                      dbo.Hishop_Commissions.CommRemark, dbo.aspnet_Distributors.StoreName, dbo.aspnet_Distributors.ReferralBlance, 
                      dbo.aspnet_Distributors.ReferralRequestBalance, dbo.aspnet_Distributors.GradeId, dbo.Hishop_Commissions.State, dbo.Hishop_Orders.UserId AS BuyUserId, 
                      dbo.Hishop_Orders.Username AS BuyUsername, dbo.aspnet_Distributors.ReferralPath, dbo.aspnet_Distributors.Logo
FROM         dbo.Hishop_Commissions INNER JOIN
                      dbo.aspnet_Distributors ON dbo.Hishop_Commissions.UserId = dbo.aspnet_Distributors.UserId AND dbo.aspnet_Distributors.ReferralStatus = 0 Left JOIN
                      dbo.Hishop_Orders ON dbo.Hishop_Orders.OrderId = dbo.Hishop_Commissions.OrderId

GO
/****** Object:  View [dbo].[vw_Hishop_CommissionWithReferralPath]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_CommissionWithReferralPath]
	AS 
	SELECT     c.UserId AS CUserId, d .StoreName, d .ReferralBlance, d .ReferralRequestBalance, d .GradeId, d .ReferralPath, d .Logo, c.CommId, d .UserId, ISNULL(c.ReferralUserId, 
                      d .UserId) AS ReferralUserId, c.OrderId, c.TradeTime, ISNULL(c.OrderTotal, 0) AS OrderTotal, ISNULL(c.CommTotal, 0) AS CommTotal, c.CommType, c.CommRemark, 
                      c.State, ISNULL(o.UserId, 0) AS BuyUserId, o.Username AS BuyUsername
FROM         dbo.aspnet_Distributors AS d LEFT OUTER JOIN
                      dbo.Hishop_Commissions AS c ON d .UserId = c.ReferralUserId LEFT OUTER JOIN
                      dbo.Hishop_Orders AS o ON o.OrderId = c.OrderId
UNION
SELECT     c.UserId AS CUserId, d .StoreName, d .ReferralBlance, d .ReferralRequestBalance, d .GradeId, d .ReferralPath, d .Logo, c.CommId, d .UserId, 
                      d .UserId AS ReferralUserId, c.OrderId, c.TradeTime, ISNULL(c.OrderTotal, 0) AS OrderTotal, ISNULL(c.CommTotal, 0) AS CommTotal, c.CommType, c.CommRemark, 
                      c.State, ISNULL(c.UserId, 0) AS BuyUserId, c.Username AS BuyUsername
FROM         dbo.aspnet_Distributors AS d LEFT OUTER JOIN
                          (SELECT     0 AS UserId, NULL AS OrderId, NULL AS TradeTime, 0 AS OrderTotal, 0 AS CommTotal, NULL AS CommType, NULL AS CommRemark, NULL AS State, 
                                                   0 AS BuyUserId, NULL AS BuyUsername, NULL AS Username, NULL AS CommId) c ON d .UserId = c.UserId

GO
/****** Object:  View [dbo].[vw_Hishop_CouponInfo]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_CouponInfo]
	AS 
	SELECT     dbo.Hishop_CouponItems.LotNumber, dbo.Hishop_CouponItems.ClaimCode, dbo.Hishop_CouponItems.UserId, dbo.Hishop_CouponItems.UserName, 
                      dbo.Hishop_CouponItems.EmailAddress, dbo.Hishop_CouponItems.GenerateTime, dbo.Hishop_CouponItems.CouponStatus, dbo.Hishop_CouponItems.UsedTime, 
                      dbo.Hishop_CouponItems.OrderId, dbo.Hishop_CouponItems.CouponId, dbo.Hishop_Coupons.Name,dbo.Hishop_Coupons.ClosingTime
FROM         dbo.Hishop_CouponItems INNER JOIN
                      dbo.Hishop_Coupons ON dbo.Hishop_CouponItems.CouponId = dbo.Hishop_Coupons.CouponId

GO
/****** Object:  View [dbo].[vw_hishop_DiscountList]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_hishop_DiscountList]
	AS   select *,(select count(*) from Hishop_LimitedTimeDiscountProduct where LimitedTimeDiscountId=dis.LimitedTimeDiscountId and Status!=2) as productCount from Hishop_LimitedTimeDiscount as dis where dis.Status!=2

GO
/****** Object:  View [dbo].[vw_Hishop_DiscountProducted]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_DiscountProducted]
AS 
 select  CategoryId, TypeId,BrandId,ProductName,ProductShortName,ProductCode,ShortDescription,MarketPrice,ThumbnailUrl40,ThumbnailUrl60, 
         SaleStatus, DisplaySequence, MainCategoryPath, ExtendCategoryPath, SaleCounts, ShowSaleCounts, AddedDate, VistiCounts, MaxShowPrice,
		 (SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice,l.*
  from Hishop_LimitedTimeDiscount d 
  join Hishop_LimitedTimeDiscountProduct l on d.LimitedTimeDiscountId=l.LimitedTimeDiscountId
  join Hishop_Products p on l.ProductId=p.ProductId

GO
/****** Object:  View [dbo].[vw_Hishop_DiscountProductList]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_DiscountProductList]
	AS 
SELECT CategoryId, TypeId,BrandId,ProductName,ProductShortName,ProductCode,ShortDescription,MarketPrice,
ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310,
SaleStatus, DisplaySequence, MainCategoryPath, ExtendCategoryPath, SaleCounts, ShowSaleCounts, AddedDate, VistiCounts, MinShowPrice, MaxShowPrice,p.ProductId as productws,
(SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice,
(SELECT COUNT(SkuId)  FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SkuNum,
(SELECT TOP 1 SkuId FROM Hishop_SKUs WHERE ProductId = p.ProductId ORDER BY SalePrice) AS SkuId,
(SELECT SUM(Stock) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Stock,
(SELECT TOP 1 [Weight] FROM Hishop_SKUs WHERE ProductId = p.ProductId ORDER BY SalePrice) AS [Weight],
(SELECT COUNT(*) FROM Taobao_Products WHERE ProductId = p.ProductId) AS IsMakeTaobao,
(select ActivityName from Hishop_LimitedTimeDiscount WHERE LimitedTimeDiscountId=d.LimitedTimeDiscountId and status!=2 and d.Status<>2) as ActivityName,
d.*
FROM Hishop_Products p left join Hishop_LimitedTimeDiscountProduct d on p.ProductId=d.ProductId

GO
/****** Object:  View [dbo].[vw_Hishop_DistributorGradeCommission]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_DistributorGradeCommission]
	AS SELECT a.Id,a.UserId,a.Commission,a.PubTime,a.OperAdmin,a.Memo,a.OrderID,a.OldCommissionTotal,b.StoreName FROM Hishop_DistributorGradeCommission a left join aspnet_Distributors b 
	on a.UserID=b.UserId

GO
/****** Object:  View [dbo].[vw_Hishop_DistributorsMembers]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_DistributorsMembers]
AS
SELECT     dbo.aspnet_Distributors.UserId, dbo.aspnet_Distributors.StoreName, dbo.aspnet_Distributors.Logo, dbo.aspnet_Distributors.BackImage, 
                      dbo.aspnet_Distributors.RequestAccount, dbo.aspnet_Distributors.AccountTime, dbo.aspnet_Distributors.GradeId, dbo.aspnet_Distributors.ReferralUserId, 
                      dbo.aspnet_Distributors.ReferralPath, dbo.aspnet_Distributors.OrdersTotal, dbo.aspnet_Distributors.ReferralOrders, dbo.aspnet_Distributors.ReferralBlance, 
                      dbo.aspnet_Distributors.ReferralRequestBalance, dbo.aspnet_Distributors.CreateTime, dbo.aspnet_Distributors.ReferralStatus, 
                      dbo.aspnet_Distributors.StoreDescription, dbo.aspnet_Distributors.DistributorGradeId, dbo.aspnet_Members.QQ, dbo.aspnet_Members.CellPhone, 
                      dbo.aspnet_Members.MicroSignal, dbo.aspnet_Members.RealName, dbo.aspnet_Members.UserHead, dbo.aspnet_Members.UserName, dbo.aspnet_Members.UserBindName, 
                      dbo.aspnet_DistributorGrade.Name
FROM         dbo.aspnet_Distributors INNER JOIN

                      dbo.aspnet_Members ON dbo.aspnet_Distributors.UserId = dbo.aspnet_Members.UserId LEFT JOIN
                      dbo.aspnet_DistributorGrade ON dbo.aspnet_Distributors.DistributorGradeId = dbo.aspnet_DistributorGrade.GradeId

GO
/****** Object:  View [dbo].[vw_Hishop_FreightTemplate_FreeShippingRegions]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_FreightTemplate_FreeShippingRegions]
AS
SELECT A.FreeId,
(SELECT cast(RegionId AS varchar(10))+',' FROM Hishop_FreightTemplate_FreeShippingRegions WHERE FreeId=A.FreeId ORDER BY FreeId FOR XML PATH('')) AS RegionIds
FROM Hishop_FreightTemplate_FreeShippingRegions A 
GROUP BY A.FreeId

GO
/****** Object:  View [dbo].[vw_Hishop_FreightTemplate_SpecifyRegions]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_FreightTemplate_SpecifyRegions]
	AS
SELECT A.GroupId,
(SELECT cast(RegionId AS varchar(10))+',' FROM Hishop_FreightTemplate_SpecifyRegions WHERE GroupId=A.GroupId ORDER BY GroupId FOR XML PATH('')) AS RegionIds
FROM Hishop_FreightTemplate_SpecifyRegions A 
GROUP BY A.GroupId

GO
/****** Object:  View [dbo].[vw_Hishop_MyBargainList]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_MyBargainList]
	AS select * from (select a.ProductName,a.MarketPrice,a.ThumbnailUrl60, (SELECT   MIN(SalePrice) AS Expr1
                     FROM      dbo.Hishop_SKUs
                     WHERE   (ProductId = b.ProductId)) AS SalePrice,CASE WHEN BeginDate <GETDATE() and GETDATE()<EndDate  THEN '进行中'
WHEN BeginDate >GETDATE()  THEN '未开始'
WHEN EndDate < GETDATE()  THEN '已结束'
ELSE NULL END bargainstatus,b.*,c.UserId,c.Price,c.id as bargainDetialID,((b.InitialPrice-c.Price)/(b.InitialPrice-b.FloorPrice))*100 as process
,(DATEDIFF(hh, GETDATE(),EndDate))   AS hou
from Hishop_Products a join Hishop_Bargain b on a.ProductId=b.ProductId join Hishop_BargainDetial c on b.id=c.BargainId) as w

GO
/****** Object:  View [dbo].[vw_Hishop_Notice]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_Notice]
AS
SELECT     ID,Title,Author,IsPub,AddTime,PubTime,SendType,SendTo,0 as UserId 
FROM         Hishop_Notice  where SendTo=0
union 
SELECT     ID,Title,Author,IsPub,AddTime,PubTime,SendType,SendTo,0 as UserId
FROM         Hishop_Notice where SendTo=1

union

SELECT     ID,Title,Author,IsPub,AddTime,PubTime,SendType,SendTo,b.UserId
FROM        Hishop_NoticeUser b left Join Hishop_Notice a on b.NoticeId=a.id

GO
/****** Object:  View [dbo].[vw_Hishop_Order]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_Order]
	AS 

	SELECT     o.OrderId, o.Remark, o.ManagerMark, o.ManagerRemark, o.AdjustedDiscount, o.OrderStatus, o.CloseReason, o.OrderDate, o.PayDate, o.ShippingDate, o.FinishDate, 
                      o.UserId, o.Username, o.EmailAddress, o.RealName, o.QQ, o.Wangwang, o.MSN, o.ShippingRegion, o.Address, o.ZipCode, o.ShipTo, o.TelPhone, o.CellPhone, 
                      o.ShipToDate, o.ShippingModeId, o.ModeName, o.RealShippingModeId, o.RealModeName, o.RegionId, o.Freight, o.AdjustedFreight, o.ShipOrderNumber, o.Weight, 
                      o.ExpressCompanyName, o.ExpressCompanyAbb, o.PaymentTypeId, o.PaymentType, o.PayCharge, o.RefundStatus, o.RefundAmount, o.RefundRemark, o.Gateway, 
                      o.OrderTotal, o.OrderPoint, o.OrderCostPrice, o.OrderProfit, o.ActualFreight, o.OtherCost, o.OptionPrice, o.Amount, o.DiscountAmount, o.ReducedPromotionId, 
                      o.ReducedPromotionName, o.ReducedPromotionAmount, o.IsReduced, o.SentTimesPointPromotionId, o.SentTimesPointPromotionName, o.TimesPoint, 
                      o.IsSendTimesPoint, o.FreightFreePromotionId, o.FreightFreePromotionName, o.IsFreightFree, o.CouponName, o.CouponCode, o.CouponAmount, o.CouponValue, 
                      o.GroupBuyId, o.NeedPrice, o.GroupBuyStatus, o.CountDownBuyId, o.BundlingId, o.BundlingNum, o.BundlingPrice, o.GatewayOrderId, o.IsPrinted, o.Tax, 
                      o.InvoiceTitle, o.Sender, o.ReferralUserId,o.SplitState,o.DeleteBeforeState,o.BargainDetialId, d.StoreName
FROM         dbo.aspnet_Distributors AS d RIGHT OUTER JOIN
                      dbo.Hishop_Orders AS o ON d.UserId = o.ReferralUserId

GO
/****** Object:  View [dbo].[vw_Hishop_OrderDebitNote]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_OrderDebitNote]
as
select a.NoteId,a.OrderId,b.Username,b.OrderTotal,b.PayCharge,b.PaymentType,a.Operator,b.PayDate,a.Remark
from Hishop_OrderDebitNote a inner join Hishop_Orders b on a.OrderId=b.OrderId

GO
/****** Object:  View [dbo].[vw_Hishop_OrderItem]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_OrderItem]
AS
select top 100 percent items.*,orders.PayDate,orders.Username,orders.ShipTo from dbo.Hishop_OrderItems as items
left join Hishop_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!=1 and orders.OrderStatus!=4
order by orders.PayDate desc

GO
/****** Object:  View [dbo].[vw_Hishop_OrderRefund]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_OrderRefund]
AS
SELECT a.RefundId,a.OrderId,a.Operator,a.RefundRemark,b.Username,b.OrderTotal,a.ApplyForTime,a.HandleTime,
a.AdminRemark,a.HandleStatus,b.OrderStatus,b.UserId,CASE a.RefundType WHEN 1 THEN '预存款' ELSE '银行转帐' END AS RefundType
FROM Hishop_OrderRefund a INNER JOIN Hishop_Orders b on a.OrderId=b.OrderId;

GO
/****** Object:  View [dbo].[vw_Hishop_OrderReplace]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_OrderReplace]
AS
SELECT a.ReplaceId,a.OrderId,b.Username,b.OrderTotal,a.ApplyForTime,a.Comments,a.HandleStatus,b.OrderStatus,b.UserId,a.HandleTime, a.AdminRemark
FROM Hishop_OrderReplace a INNER JOIN Hishop_Orders b on a.OrderId=b.OrderId;

GO
/****** Object:  View [dbo].[vw_Hishop_OrderReturns]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_OrderReturns]
AS
SELECT a.ReturnsId,a.OrderId,a.Operator,b.Username,a.RefundMoney,a.ApplyForTime,a.AdminRemark,a.Comments,a.HandleStatus, OrderStatus,b.UserId,
CASE a.RefundType WHEN 1 THEN '预存款' ELSE '银行转帐' END AS RefundType, a.HandleTime
FROM Hishop_OrderReturns a INNER JOIN Hishop_Orders b on a.OrderId=b.OrderId;

GO
/****** Object:  View [dbo].[vw_Hishop_OrderSendNote]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_OrderSendNote]
as
select a.NoteId,b.OrderTotal,a.OrderId,b.PaymentType,b.ShippingDate,b.ExpressCompanyName,b.ZipCode,b.TelPhone,b.CellPhone,
b.ShipOrderNumber,b.ShipTo,b.ShippingRegion,a.Operator,b.Username,a.Remark
from Hishop_OrderSendNote a inner join Hishop_Orders b on a.OrderId=b.OrderId

GO
/****** Object:  View [dbo].[vw_Hishop_PointExchange_Products]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_PointExchange_Products]
AS
SELECT     dbo.Hishop_Products.ProductName, dbo.Hishop_PointExChange_Products.exChangeId, dbo.Hishop_PointExChange_Products.ProductId, 
                      dbo.Hishop_PointExChange_Products.status, dbo.Hishop_PointExChange_Products.ProductNumber, dbo.Hishop_PointExChange_Products.PointNumber, 
                      dbo.Hishop_PointExChange_Products.ChangedNumber, dbo.Hishop_PointExChange_Products.eachMaxNumber, dbo.Hishop_Products.ImageUrl1, 
                      dbo.Hishop_Products.ImageUrl2, dbo.Hishop_Products.ImageUrl3, dbo.Hishop_Products.ImageUrl4, dbo.Hishop_Products.ImageUrl5, 
                      dbo.Hishop_Products.ThumbnailUrl40, dbo.Hishop_Products.ThumbnailUrl60, dbo.Hishop_Products.ThumbnailUrl100, dbo.Hishop_Products.ThumbnailUrl160, 
                      dbo.Hishop_Products.ThumbnailUrl180, dbo.Hishop_Products.ThumbnailUrl220, dbo.Hishop_Products.ThumbnailUrl310, dbo.Hishop_Products.ThumbnailUrl410, 
                      dbo.Hishop_Products.MarketPrice, dbo.Hishop_Products.CategoryId, dbo.Hishop_Products.TypeId, dbo.Hishop_Products.ProductCode, 
                      dbo.Hishop_Products.ShortDescription, dbo.Hishop_Products.Unit, dbo.Hishop_Products.Description, dbo.Hishop_Products.SaleStatus, 
                      dbo.Hishop_Products.AddedDate, dbo.Hishop_Products.VistiCounts, dbo.Hishop_Products.SaleCounts, dbo.Hishop_Products.ShowSaleCounts, 
                      dbo.Hishop_Products.DisplaySequence, dbo.Hishop_Products.BrandId, dbo.Hishop_Products.MainCategoryPath, dbo.Hishop_Products.ExtendCategoryPath, 
                      dbo.Hishop_Products.HasSKU, dbo.Hishop_Products.IsfreeShipping, dbo.Hishop_Products.TaobaoProductId, dbo.Hishop_Products.Source, 
                      dbo.Hishop_Products.MinShowPrice, dbo.Hishop_Products.MaxShowPrice, dbo.Hishop_Products.FreightTemplateId, dbo.Hishop_Products.FirstCommission, 
                      dbo.Hishop_Products.SecondCommission, dbo.Hishop_Products.ThirdCommission, dbo.Hishop_Products.IsSetCommission, dbo.Hishop_Products.CubicMeter, 
                      dbo.Hishop_Products.FreightWeight, dbo.Hishop_Products.ProductShortName,dbo.Hishop_PointExChange_PointExChanges.MemberGrades
FROM         dbo.Hishop_Products INNER JOIN
                      dbo.Hishop_PointExChange_Products ON dbo.Hishop_Products.ProductId = dbo.Hishop_PointExChange_Products.ProductId INNER JOIN
					   dbo.Hishop_PointExChange_PointExChanges ON 
                dbo.Hishop_PointExChange_PointExChanges.Id = dbo.Hishop_PointExChange_Products.exChangeId

GO
/****** Object:  View [dbo].[vw_Hishop_PrizesDeliveryRecord]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--奖品发货记录视图
CREATE VIEW [dbo].[vw_Hishop_PrizesDeliveryRecord]
AS
SELECT     p.PrizeId, g.GameId, m.LogId, r.Receiver, r.Tel, CASE p.PrizeType WHEN 2  THEN ISNULL(r.status, 0) WHEN 3  THEN ISNULL(r.status, 0) ELSE 4 END AS status, r.DeliveryTime, r.ReceiveTime, 
                      r.ExpressName, r.CourierNumber, r.ReggionPath, r.Address, m.UserId, m.PlayTime, m.IsUsed, g.GameType, g.GameTitle, g.BeginTime, g.EndTime, g.ApplyMembers, 
                      g.OnlyGiveNotPrizeMember, g.PlayType, g.NotPrzeDescription, g.GameUrl, g.GameQRCodeAddress, p.PrizeGrade, p.PrizeType, p.GiveCouponId, p.GiveShopBookId,p.Prize,p.IsLogistics, 
                      p.GriveShopBookPicUrl, p.PrizeCount, p.PrizeRate, u.UserName, u.RealName, u.CellPhone, u.Address AS UAddress, ps.ProductId, ps.ProductName, 
                      ps.ThumbnailUrl100, ISNULL(r.Id, 0) AS Id, p.GivePoint
FROM         dbo.Hishop_PromotionGameResultMembersLog AS m INNER JOIN
                      dbo.Hishop_PromotionGamePrizes AS p ON m.PrizeId = p.PrizeId INNER JOIN
                      dbo.Hishop_PromotionGame AS g ON m.GameId = g.GameId INNER JOIN
                      dbo.aspnet_Members AS u ON m.UserId = u.UserId LEFT OUTER JOIN
                      dbo.Hishop_Products AS ps ON p.GiveShopBookId = ps.ProductId LEFT OUTER JOIN
                      dbo.Hishop_PrizesDeliveryRecord AS r ON m.LogId = r.LogId

GO
/****** Object:  View [dbo].[vw_Hishop_PrizesRecord]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_PrizesRecord]
	AS 
SELECT	 
l.LogId,l.PlayTime,
l.UserId,m.UserName,
p.PrizeGrade,p.PrizeType,
p.GivePoint,p.GiveCouponId,
p.GiveShopBookId,l.GameId,l.IsUsed,p.Prize,l.PrizeId,p.IsLogistics
From Hishop_PromotionGameResultMembersLog as l inner join
 Hishop_PromotionGamePrizes as p on l.PrizeId=p.PrizeId 
left join aspnet_Members as m on l.UserId=m.UserId

GO
/****** Object:  View [dbo].[vw_Hishop_ProductConsultations]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_ProductConsultations]
AS
SELECT
	  p.[ProductId]
      ,p.[ProductName]     
	  ,p.[ProductCode]
      ,p.[ThumbnailUrl40]
      ,p.[ThumbnailUrl60]  
	  ,p.[CategoryId]
      ,c.[ConsultationId]
	  ,c.[ConsultationText]
      ,c.[ConsultationDate]
      ,c.[ReplyText]
      ,c.[UserName]
      ,c.[ReplyUserId]
      ,c.[Userid]
	  ,c.[ReplyDate]
FROM Hishop_Products p inner join Hishop_ProductConsultations c ON p.productId=c.ProductId

GO
/****** Object:  View [dbo].[vw_Hishop_ProductReviews]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_ProductReviews]
AS
SELECT
	p.[ProductId]
	,p.ProductCode
	,p.[ProductName]
	,p.[CategoryId]
	,p.[ThumbnailUrl60]
    ,r.ReviewId
    ,r.ReviewText
    ,r.ReviewDate
    ,r.UserId
    ,r.UserName
FROM 
	Hishop_Products p (nolock)inner join Hishop_ProductReviews r on (r.productId=p.ProductId)

GO
/****** Object:  View [dbo].[vw_Hishop_ProductSkuList]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_ProductSkuList]
AS
SELECT CategoryId, BrandId,ProductName,ProductCode,MarketPrice,ThumbnailUrl40, ThumbnailUrl60,
 SaleStatus, DisplaySequence, MainCategoryPath, ExtendCategoryPath, AddedDate, VistiCounts, s.*
FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId

GO
/****** Object:  View [dbo].[vw_Hishop_PromotionGame]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_PromotionGame]
as
select g.GameID,g.GameType,g.GameTitle,g.BeginTime,g.EndTime,g.PlayType,g.GameUrl,g.GameQRCodeAddress,g.Status,isnull(v1.TotalCount,0) as TotalCount,
isnull(v2.PrizeCount,0) as PrizeCount, g.LimitEveryDay,g.MaximumDailyLimit
from Hishop_PromotionGame as g left join(
select GameId, COUNT(UserId) as TotalCount  from Hishop_PromotionGameResultMembersLog group by GameId
)as v1 on g.GameId= v1.GameId left join (
select GameId, COUNT(UserId) as PrizeCount  from Hishop_PromotionGameResultMembersLog where PrizeId>=1 group by GameId 
) as v2 on g.GameId=v2.GameId

GO
/****** Object:  View [dbo].[vw_Hishop_ReplyKeysReply]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_ReplyKeysReply]
AS
SELECT     dbo.Weibo_Reply.Id, dbo.Weibo_Reply.ReplyKeyId, dbo.Weibo_Reply.IsDisable, dbo.Weibo_Reply.EditDate, dbo.Weibo_Reply.[Content], dbo.Weibo_Reply.Type, 
                      dbo.Weibo_Reply.ReceiverType, dbo.Weibo_Reply.Displayname AS Display_name, dbo.Weibo_Reply.Summary, dbo.Weibo_Reply.Image, dbo.Weibo_Reply.Url, 
                      dbo.Weibo_Reply.ArticleId, dbo.Weibo_ReplyKeys.Matching, dbo.Weibo_ReplyKeys.Keys
FROM         dbo.Weibo_Reply INNER JOIN
                      dbo.Weibo_ReplyKeys ON dbo.Weibo_Reply.ReplyKeyId = dbo.Weibo_ReplyKeys.Id

GO
/****** Object:  View [dbo].[vw_Hishop_SaleDetails]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_SaleDetails]
AS
SELECT oi.OrderId, oi.ItemDescription AS ProductName, oi.Quantity, oi.ItemAdjustedPrice, o.orderDate, o.OrderStatus 
FROM Hishop_OrderItems oi join Hishop_orders o on oi.OrderId = o.OrderId

GO
/****** Object:  View [dbo].[vw_OrderMmemberComment]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---视图[vw_OrderMmemberComment]在2005数据库下面有问题，需要把userid移动到a.*的前面。

CREATE view [dbo].[vw_OrderMmemberComment]
as
 select a.*,c.UserId from Hishop_OrderItems a 
 left join Hishop_ProductReviews b on a.skuid= b.skuid and a.orderid=b.orderid 
 left join Hishop_Orders c on a.orderid=c.orderid 
 where   b.orderid is null  

GO
/****** Object:  View [dbo].[vw_UserOrderByPage]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[vw_UserOrderByPage]
as

SELECT UserId,ThumbnailsUrl, ItemDescription, SKUContent, SKU,OrderItemsStatus, ProductId,Quantity,ReturnMoney,SkuID,o.OrderId,OrderMarking, OrderDate, OrderStatus,PaymentTypeId, OrderTotal,   Gateway,(SELECT count(0) FROM vshop_OrderRedPager WHERE OrderId = o.OrderId and ExpiryDays<getdate() and AlreadyGetTimes<MaxGetTimes) as HasRedPage,(SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum 
FROM Hishop_Orders o join Hishop_OrderItems b on o.OrderId=b.OrderId

GO
/****** Object:  View [dbo].[vw_VShop_FinishOrder_Detail]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[vw_VShop_FinishOrder_Detail]
as
/*
订单子表明细，核算有效的（已成功且未退款）订单。
要考虑主表已设置为退款或关闭的情况

select * from vw_VShop_FinishOrder_Detail

*/
SELECT     a.OrderStatus, a.Gateway, a.OrderDate, a.PayDate, a.ReferralUserId, a.UserId, b.Id, b.OrderId, b.SkuId, b.ProductId, b.SKU, b.Quantity, b.ShipmentQuantity, 
                      b.CostPrice, b.ItemListPrice, b.ItemAdjustedPrice, b.ItemAdjustedCommssion, b.ItemDescription, b.ThumbnailsUrl, b.Weight, b.SKUContent, b.PromotionId, 
                      b.PromotionName, b.OrderItemsStatus, b.ItemsCommission, b.SecondItemsCommission, b.ThirdItemsCommission, b.PointNumber, b.Type, b.IsHandled, 
                      b.ReturnMoney, b.DiscountAverage, b.IsAdminModify
FROM         dbo.Hishop_OrderItems AS b INNER JOIN
                      dbo.Hishop_Orders AS a ON a.OrderId = b.OrderId
WHERE     (a.OrderStatus <> 4) AND (a.OrderStatus <> 1) AND (a.Gateway <> 'hishop.plugins.payment.podrequest') OR
                      (a.OrderStatus <> 4) AND (a.Gateway = 'hishop.plugins.payment.podrequest')

GO
/****** Object:  View [dbo].[vw_VShop_FinishOrder_Detail_All]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vw_VShop_FinishOrder_Detail_All]
as
/*
订单子表明细  2=待发货  3=已发货待确认  5=已完成

select * from vw_VShop_FinishOrder_Detail

*/
	select a.PayDate, a.ReferralUserId, a.UserId,a.OrderStatus, b.*
				from Hishop_OrderItems b
				left join Hishop_Orders a on a.OrderId= b.OrderId 
				where 1=1

GO
/****** Object:  View [dbo].[vw_VShop_FinishOrder_Main]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[vw_VShop_FinishOrder_Main]
as
SELECT     a.OrderId, a.OrderStatus, a.UserId, a.Gateway, a.Username, a.ReferralUserId, a.OrderDate, a.PayDate, a.OrderTotal AS ValidOrderTotal, ISNULL(T1.SumCommission, 
                      0) AS SumCommission
FROM         dbo.Hishop_Orders AS a LEFT OUTER JOIN
                          (SELECT     OrderId AS OrderId_Item, SUM(ItemsCommission + SecondItemsCommission + ThirdItemsCommission) AS SumCommission
                            FROM          dbo.Hishop_OrderItems AS b
                            WHERE      (OrderItemsStatus <> 4)
                            GROUP BY OrderId) AS T1 ON a.OrderId = T1.OrderId_Item
WHERE     (a.OrderStatus <> 4) AND(a.OrderStatus <> 12) AND (a.OrderStatus <> 1) AND (a.Gateway <> 'hishop.plugins.payment.podrequest') OR
                      (a.OrderStatus <> 4) AND (a.Gateway = 'hishop.plugins.payment.podrequest')
					  --增加了(a.OrderStatus <> 12)判断，订单回收站的商品不要统计进去

GO
/****** Object:  View [dbo].[vw_VShop_FinishOrder_Main_Payed]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vw_VShop_FinishOrder_Main_Payed]
as
/*
对订单主表统计，每个单号一条记录。核算状态任意的订单（2=已付款未发货  3=已发货待收货  5=已完成）
select * from [vw_VShop_FinishOrder_Main_Payed]

select * from vw_VShop_FinishOrder_Detail

--update Hishop_Orders set paydate= finishDate where paydate is null
sp_refreshview   [vw_VShop_FinishOrder_Main]

alter table  Hishop_OrderItems add ReturnMoney money default 0


*/

--select  * from Hishop_OrderItems


		select  a.OrderId, a.UserId,a.Username, a.ReferralUserId, a.PayDate, 
				--T1.SumItemAmountFee - isnull(r.RefundMoney,0)  as ValidOrderTotal
				
				--a.OrderTotal - isnull(r.RefundMoney,0)  as ValidOrderTotal ,
				a.OrderTotal  as  OrderTotal ,
				--a.OrderTotal - isnull(T1.ReturnMoney,0)  as ValidOrderTotal ,
				a.OrderTotal    as ValidOrderTotal ,   --2015-9-19经与蒯工沟通，以后退款时，会在明细表中保存退款金额，主表的OrderTotal减去相应的退款，
				 isnull(T1.ReturnMoney,0)  as ReturnMoney ,
				
				isnull(T1.SumCommission,0) as SumCommission
				from Hishop_Orders a 
				left join 
				(
					select  OrderId as OrderId_Item, 
					   SUM( (ItemsCommission + SecondItemsCommission + ThirdItemsCommission))  as SumCommission,
					   sum(isnull(ReturnMoney,0)) as ReturnMoney
						from Hishop_OrderItems b  
						where 
						1=1 
						and ( OrderItemsStatus =2 or OrderItemsStatus = 3   or OrderItemsStatus =5)
						group by OrderId
				) T1  on a.OrderId = T1.OrderId_Item				
				where 1=1
				and ( OrderStatus =2 or OrderStatus = 3   or OrderStatus =5)

GO
/****** Object:  View [dbo].[vw_VShop_FinishOrder_Main_PayedAndNoClosed]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[vw_VShop_FinishOrder_Main_PayedAndNoClosed]
as
/*
对订单主表统计，每个单号一条记录。核算状态任意的订单（2=已付款未发货  3=已发货待收货  5=已完成）

select * from [vw_VShop_FinishOrder_Main_PayedAndNoClosed]
select  ItemAdjustedPrice * Quantity - DiscountAverage - ReturnMoney,
select * from Hishop_OrderItems where orderid='201509199691596'

select  * from Hishop_Orders where orderid='201509199691596'
 
 select * from Hishop_OrderItems where orderid in 
( select  orderid from Hishop_Orders where  Gateway='hishop.plugins.payment.podrequest'  )

201509223162229

*/

--select  * from Hishop_OrderItems
-- select  * from Hishop_Orders where orderid='201509199691596'


		select  a.OrderId, a.UserId,a.Username, a.ReferralUserId, 
				case
					when ( Gateway='hishop.plugins.payment.podrequest' and PayDate is null ) then OrderDate
					else PayDate
				end as PayDate, --a.PayDate, 
				a.OrderTotal  as  OrderTotal ,
				--a.OrderTotal    as ValidOrderTotal ,   --2015-9-19经与蒯工沟通，以后退款时，会在明细表中保存退款金额，主表的OrderTotal减去相应的退款， 20151023其表示现不能确定OrderTotal是否会扣减
				isnull(ItemTotal,0) as  ValidOrderTotal,
				isnull(T1.ReturnMoney,0)  as ReturnMoney ,			
				isnull(T1.SumCommission,0) as SumCommission,
				a.OrderStatus
				from Hishop_Orders a 
				left join 
				(
					select  b.OrderId as OrderId_Item, 
					   sum(ItemAdjustedPrice * Quantity - DiscountAverage - ReturnMoney) as ItemTotal,
					   
					   SUM( (ItemsCommission + SecondItemsCommission + ThirdItemsCommission))  as SumCommission,
					   sum(isnull(ReturnMoney,0)) as ReturnMoney
						from Hishop_OrderItems b  
						left join Hishop_Orders m on b.OrderId=m.OrderId
						where 1=1 
						and ( OrderItemsStatus<>1 or m.Gateway='hishop.plugins.payment.podrequest' )
						and  OrderItemsStatus<>4 and OrderItemsStatus<>9 and OrderItemsStatus<>10
						group by b.OrderId
				) T1  on a.OrderId = T1.OrderId_Item				
				where 1=1
				and  ( OrderStatus<>1 or Gateway='hishop.plugins.payment.podrequest')
				and   OrderStatus<>4 and OrderStatus<>9 and OrderStatus<>10

GO
/****** Object:  View [dbo].[vw_Vshop_OneyuanPartInList]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Vshop_OneyuanPartInList]
AS
SELECT     dbo.Vshop_OneyuanTao_ParticipantMember.Pid, dbo.Vshop_OneyuanTao_ParticipantMember.UserId, dbo.Vshop_OneyuanTao_ParticipantMember.BuyTime, 
                      dbo.Vshop_OneyuanTao_ParticipantMember.BuyNum, dbo.Vshop_OneyuanTao_ParticipantMember.IsPay, dbo.Vshop_OneyuanTao_ParticipantMember.ActivityId, 
                      dbo.Vshop_OneyuanTao_ParticipantMember.SkuId, dbo.Vshop_OneyuanTao_ParticipantMember.SkuIdStr, dbo.Vshop_OneyuanTao_ParticipantMember.ProductPrice, 
                      dbo.Vshop_OneyuanTao_ParticipantMember.PayTime, dbo.Vshop_OneyuanTao_ParticipantMember.PayWay, dbo.Vshop_OneyuanTao_ParticipantMember.PayNum, 
                      dbo.Vshop_OneyuanTao_ParticipantMember.Remark, dbo.Vshop_OneyuanTao_ParticipantMember.TotalPrice, dbo.Vshop_OneyuanTao_ParticipantMember.IsRefund, 
                      dbo.Vshop_OneyuanTao_ParticipantMember.RefundNum, dbo.Vshop_OneyuanTao_ParticipantMember.RefundTime, dbo.aspnet_MemberGrades.Name, 
                      dbo.aspnet_Members.UserName, dbo.Vshop_OneyuanTao_Detail.IsOn, dbo.Vshop_OneyuanTao_Detail.Title, dbo.Vshop_OneyuanTao_Detail.StartTime, 
                      dbo.Vshop_OneyuanTao_Detail.EndTime, dbo.Vshop_OneyuanTao_Detail.HeadImgage, dbo.Vshop_OneyuanTao_Detail.ReachType, 
                      dbo.Vshop_OneyuanTao_Detail.ActivityDec, dbo.Vshop_OneyuanTao_Detail.IsEnd, dbo.Vshop_OneyuanTao_Detail.FinishedNum, dbo.aspnet_Members.UserHead, 
                      dbo.Vshop_OneyuanTao_Detail.ProductImg, dbo.Vshop_OneyuanTao_Detail.ProductTitle, dbo.Vshop_OneyuanTao_Detail.PrizeNumber, 
                      dbo.Vshop_OneyuanTao_ParticipantMember.IsWin, dbo.Vshop_OneyuanTao_ParticipantMember.WinIds, dbo.Vshop_OneyuanTao_ParticipantMember.WinTime, 
                      dbo.Vshop_OneyuanTao_Detail.PrizeTime, dbo.aspnet_Members.CellPhone, dbo.Vshop_OneyuanTao_Detail.HasCalculate, 
                      dbo.Vshop_OneyuanTao_Detail.IsSuccess, dbo.Vshop_OneyuanTao_ParticipantMember.RefundErr, dbo.Vshop_OneyuanTao_Detail.ReachNum


FROM         dbo.aspnet_Members INNER JOIN
                      dbo.Vshop_OneyuanTao_ParticipantMember ON dbo.aspnet_Members.UserId = dbo.Vshop_OneyuanTao_ParticipantMember.UserId INNER JOIN
                      dbo.Vshop_OneyuanTao_Detail ON dbo.Vshop_OneyuanTao_Detail.ActivityId = dbo.Vshop_OneyuanTao_ParticipantMember.ActivityId LEFT OUTER JOIN
                      dbo.aspnet_MemberGrades ON dbo.aspnet_Members.GradeId = dbo.aspnet_MemberGrades.GradeId

GO
/****** Object:  View [dbo].[vw_VShop_Order_Detail_PayedAndNoClosed]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[vw_VShop_Order_Detail_PayedAndNoClosed]
as
/*
订单子表明细，核算有效的（付款了且未退款成功）订单。
要考虑主表已设置为退款或关闭的情况

select * from vw_VShop_Order_Detail_PayedAndNoClosed

*/
	select --a.PayDate, 
				case
					when ( Gateway='hishop.plugins.payment.podrequest' and PayDate is null ) then OrderDate
					else PayDate
				end as PayDate,
					
			a.ReferralUserId, a.UserId, b.*
				from Hishop_OrderItems b
				left join Hishop_Orders a on a.OrderId= b.OrderId 
						where 1=1 
						and ( OrderItemsStatus<>1 or a.Gateway='hishop.plugins.payment.podrequest' )
						and  OrderItemsStatus<>4 and OrderItemsStatus<>9 and OrderItemsStatus<>10

GO
/****** Object:  Index [IX_Hishop_FreeShipping]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [IX_Hishop_FreeShipping] ON [dbo].[Hishop_FreightTemplate_FreeShipping]
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Hishop_FreeShippingRegions]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [IX_Hishop_FreeShippingRegions] ON [dbo].[Hishop_FreightTemplate_FreeShippingRegions]
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Hishop_SpecifyRegionGroups]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [IX_Hishop_SpecifyRegionGroups] ON [dbo].[Hishop_FreightTemplate_SpecifyRegionGroups]
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Hishop_FreightTemplate_SpecifyRegions]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [IX_Hishop_FreightTemplate_SpecifyRegions] ON [dbo].[Hishop_FreightTemplate_SpecifyRegions]
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Hishop_Orders_Index2]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [Hishop_Orders_Index2] ON [dbo].[Hishop_Orders]
(
	[PaymentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Hishop_Orders_Index3]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [Hishop_Orders_Index3] ON [dbo].[Hishop_Orders]
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Hishop_Orders_Index4]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [Hishop_Orders_Index4] ON [dbo].[Hishop_Orders]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Hishop_PrizesDeliveryRecord_Column]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [IX_Hishop_PrizesDeliveryRecord_Column] ON [dbo].[Hishop_PrizesDeliveryRecord]
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Hishop_Products_Index2]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [Hishop_Products_Index2] ON [dbo].[Hishop_Products]
(
	[MainCategoryPath] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Hishop_Products_Index3]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [Hishop_Products_Index3] ON [dbo].[Hishop_Products]
(
	[ExtendCategoryPath] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Hishop_SKUs_Index2]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [Hishop_SKUs_Index2] ON [dbo].[Hishop_SKUs]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Vshop_OneyuanTao_ParticipantMember_Column]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [IX_Vshop_OneyuanTao_ParticipantMember_Column] ON [dbo].[Vshop_OneyuanTao_ParticipantMember]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Vshop_OneyuanTao_ParticipantMember_Column_1]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [IX_Vshop_OneyuanTao_ParticipantMember_Column_1] ON [dbo].[Vshop_OneyuanTao_ParticipantMember]
(
	[ActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Vshop_OneyuanTao_WinningRecord_Column]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [IX_Vshop_OneyuanTao_WinningRecord_Column] ON [dbo].[Vshop_OneyuanTao_WinningRecord]
(
	[ActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Vshop_OneyuanTao_WinningRecord_Column_1]    Script Date: 2017/3/14 21:10:57 ******/
CREATE NONCLUSTERED INDEX [IX_Vshop_OneyuanTao_WinningRecord_Column_1] ON [dbo].[Vshop_OneyuanTao_WinningRecord]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Hishop_Activities] ADD  DEFAULT ((0)) FOR [ActivitiesType]
GO
ALTER TABLE [dbo].[Hishop_Activities] ADD  DEFAULT ((0)) FOR [TakeEffect]
GO
ALTER TABLE [dbo].[Hishop_Activities] ADD  DEFAULT ((0)) FOR [Type]
GO
ALTER TABLE [dbo].[Hishop_Activities] ADD  DEFAULT ('0') FOR [MemberGrades]
GO
ALTER TABLE [dbo].[Hishop_Activities] ADD  DEFAULT ('-1') FOR [DefualtGroup]
GO
ALTER TABLE [dbo].[Hishop_Activities] ADD  DEFAULT ('-1') FOR [CustomGroup]
GO
ALTER TABLE [dbo].[Hishop_Activities] ADD  DEFAULT ((0)) FOR [attendTime]
GO
ALTER TABLE [dbo].[Hishop_Activities] ADD  DEFAULT ((0)) FOR [attendType]
GO
ALTER TABLE [dbo].[Hishop_Activities] ADD  DEFAULT ((1)) FOR [isAllProduct]
GO
ALTER TABLE [dbo].[Hishop_Activities] ADD  DEFAULT ((0)) FOR [MeetMoney]
GO
ALTER TABLE [dbo].[Hishop_Activities] ADD  DEFAULT ((0)) FOR [ReductionMoney]
GO
ALTER TABLE [dbo].[Hishop_Activities_Detail] ADD  DEFAULT ((0)) FOR [MeetMoney]
GO
ALTER TABLE [dbo].[Hishop_Activities_Detail] ADD  DEFAULT ((0)) FOR [ReductionMoney]
GO
ALTER TABLE [dbo].[Hishop_Activities_Detail] ADD  DEFAULT ((0)) FOR [Integral]
GO
ALTER TABLE [dbo].[Hishop_Activities_Detail] ADD  DEFAULT ((0)) FOR [MeetNumber]
GO
ALTER TABLE [dbo].[Hishop_Activities_Product] ADD  DEFAULT ((0)) FOR [status]
GO
ALTER TABLE [dbo].[Hishop_Coupon_Products] ADD  CONSTRAINT [DF_Hishop_Coupon_Products_status]  DEFAULT ((0)) FOR [status]
GO
ALTER TABLE [dbo].[Hishop_CouponItems] ADD  CONSTRAINT [DF_Hishop_CouponItems_CouponStatus]  DEFAULT ((0)) FOR [CouponStatus]
GO
ALTER TABLE [dbo].[Hishop_Coupons] ADD  CONSTRAINT [DF_Hishop_Coupons_SentCount]  DEFAULT ((0)) FOR [SentCount]
GO
ALTER TABLE [dbo].[Hishop_Coupons] ADD  CONSTRAINT [DF_Hishop_Coupons_UsedCount]  DEFAULT ((0)) FOR [UsedCount]
GO
ALTER TABLE [dbo].[Hishop_Coupons] ADD  CONSTRAINT [DF_Hishop_Coupons_NeedPoint]  DEFAULT ((0)) FOR [NeedPoint]
GO
ALTER TABLE [dbo].[Hishop_LimitedTimeDiscount] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Hishop_LimitedTimeDiscount] ADD  DEFAULT ((0)) FOR [IsCommission]
GO
ALTER TABLE [dbo].[Hishop_LimitedTimeDiscount] ADD  DEFAULT ((100)) FOR [CommissionDiscount]
GO
ALTER TABLE [dbo].[Hishop_LimitedTimeDiscountProduct] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Hishop_Notice] ADD  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[Hishop_Notice] ADD  DEFAULT ((0)) FOR [IsPub]
GO
ALTER TABLE [dbo].[Hishop_Notice] ADD  DEFAULT ((0)) FOR [SendType]
GO
ALTER TABLE [dbo].[Hishop_Notice] ADD  DEFAULT ((0)) FOR [SendTo]
GO
ALTER TABLE [dbo].[Hishop_NoticeRead] ADD  DEFAULT (getdate()) FOR [ReadTime]
GO
ALTER TABLE [dbo].[Hishop_NoticeRead] ADD  DEFAULT ((0)) FOR [NoticeIsDel]
GO
ALTER TABLE [dbo].[Hishop_OrderReturns] ADD  DEFAULT ((0)) FOR [DeleteBeforeState]
GO
ALTER TABLE [dbo].[Hishop_OrderReturns] ADD  DEFAULT ((0)) FOR [OrderItemID]
GO
ALTER TABLE [dbo].[Hishop_OrderReturns] ADD  DEFAULT ((0)) FOR [BalanceReturnMoney]
GO
ALTER TABLE [dbo].[Hishop_PointExchange_Changed] ADD  DEFAULT ((0)) FOR [PointNumber]
GO
ALTER TABLE [dbo].[Hishop_PointExchange_Changed] ADD  DEFAULT ('0') FOR [MemberGrades]
GO
ALTER TABLE [dbo].[Hishop_PointExChange_PointExChanges] ADD  DEFAULT ('0') FOR [MemberGrades]
GO
ALTER TABLE [dbo].[Hishop_PointExChange_PointExChanges] ADD  DEFAULT ('0') FOR [DefualtGroup]
GO
ALTER TABLE [dbo].[Hishop_PointExChange_PointExChanges] ADD  DEFAULT ('0') FOR [CustomGroup]
GO
ALTER TABLE [dbo].[Hishop_PointExChange_PointExChanges] ADD  DEFAULT ((0)) FOR [ProductNumber]
GO
ALTER TABLE [dbo].[Hishop_PointExChange_Products] ADD  DEFAULT ((0)) FOR [ProductNumber]
GO
ALTER TABLE [dbo].[Hishop_PointExChange_Products] ADD  DEFAULT ((0)) FOR [PointNumber]
GO
ALTER TABLE [dbo].[Hishop_PointExChange_Products] ADD  DEFAULT ((0)) FOR [ChangedNumber]
GO
ALTER TABLE [dbo].[Hishop_PointExChange_Products] ADD  DEFAULT ((0)) FOR [eachMaxNumber]
GO
ALTER TABLE [dbo].[Hishop_PrizesDeliveryRecord] ADD  DEFAULT ((0)) FOR [status]
GO
ALTER TABLE [dbo].[Hishop_PrizesDeliveryRecord] ADD  DEFAULT ((0)) FOR [Pid]
GO
ALTER TABLE [dbo].[Hishop_PrizesDeliveryRecord] ADD  DEFAULT ((0)) FOR [RecordType]
GO
ALTER TABLE [dbo].[Hishop_PromotionGame] ADD  CONSTRAINT [DF_Hishop_PromotionGame_NeedPoint]  DEFAULT ((0)) FOR [NeedPoint]
GO
ALTER TABLE [dbo].[Hishop_PromotionGame] ADD  CONSTRAINT [DF_Hishop_PromotionGame_GivePoint]  DEFAULT ((0)) FOR [GivePoint]
GO
ALTER TABLE [dbo].[Hishop_PromotionGame] ADD  CONSTRAINT [DF_Hishop_PromotionGame_OnlyGiveNotPrizeMember]  DEFAULT ((0)) FOR [OnlyGiveNotPrizeMember]
GO
ALTER TABLE [dbo].[Hishop_PromotionGame] ADD  DEFAULT ((0)) FOR [LimitEveryDay]
GO
ALTER TABLE [dbo].[Hishop_PromotionGame] ADD  DEFAULT ((0)) FOR [MaximumDailyLimit]
GO
ALTER TABLE [dbo].[Hishop_PromotionGame] ADD  DEFAULT ((0)) FOR [PrizeRate]
GO
ALTER TABLE [dbo].[Hishop_PromotionGame] ADD  DEFAULT ((0)) FOR [MemberCheck]
GO
ALTER TABLE [dbo].[Hishop_PromotionGame] ADD  DEFAULT ('0') FOR [DefualtGroup]
GO
ALTER TABLE [dbo].[Hishop_PromotionGame] ADD  DEFAULT ('0') FOR [CustomGroup]
GO
ALTER TABLE [dbo].[Hishop_PromotionGamePrizes] ADD  CONSTRAINT [DF_Hishop_PromotionGamePrizes_GivePoint]  DEFAULT ((0)) FOR [GivePoint]
GO
ALTER TABLE [dbo].[Hishop_PromotionGamePrizes] ADD  CONSTRAINT [DF_Hishop_PromotionGamePrizes_PrizeCount]  DEFAULT ((0)) FOR [PrizeCount]
GO
ALTER TABLE [dbo].[Hishop_PromotionGamePrizes] ADD  CONSTRAINT [DF_Hishop_PromotionGamePrizes_PrizeRate]  DEFAULT ((0)) FOR [PrizeRate]
GO
ALTER TABLE [dbo].[Hishop_PromotionGamePrizes] ADD  DEFAULT ('奖项名称') FOR [PrizeName]
GO
ALTER TABLE [dbo].[Hishop_PromotionGamePrizes] ADD  DEFAULT ('奖品名称') FOR [Prize]
GO
ALTER TABLE [dbo].[Hishop_PromotionGamePrizes] ADD  DEFAULT ((0)) FOR [IsLogistics]
GO
ALTER TABLE [dbo].[Hishop_PromotionGamePrizes] ADD  DEFAULT ('/utility/pics/lipin30.png') FOR [PrizeImage]
GO
ALTER TABLE [dbo].[Hishop_PromotionGameResultMembersLog] ADD  CONSTRAINT [DF_Hishop_PromotionGameResultMembersLog_PrizeId]  DEFAULT ((0)) FOR [PrizeId]
GO
ALTER TABLE [dbo].[Hishop_PromotionGameResultMembersLog] ADD  CONSTRAINT [DF_Hishop_PromotionGameResultMembersLog_PlayTime]  DEFAULT (getdate()) FOR [PlayTime]
GO
ALTER TABLE [dbo].[Hishop_PromotionGameResultMembersLog] ADD  CONSTRAINT [DF_Hishop_PromotionGameResultMembersLog_IsUsed]  DEFAULT ((0)) FOR [IsUsed]
GO
ALTER TABLE [dbo].[Hishop_ShareActivity] ADD  DEFAULT ((1)) FOR [CouponNumber]
GO
ALTER TABLE [dbo].[Hishop_ShoppingCarts] ADD  CONSTRAINT [DF_Hishop_ShoppingCarts_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[Hishop_ShoppingCarts] ADD  CONSTRAINT [DF_Hishop_ShoppingCarts_CategoryId]  DEFAULT ((0)) FOR [CategoryId]
GO
ALTER TABLE [dbo].[Hishop_ShoppingCarts] ADD  CONSTRAINT [DF_Hishop_ShoppingCarts_Templateid]  DEFAULT ((0)) FOR [Templateid]
GO
ALTER TABLE [dbo].[Hishop_ShoppingCarts] ADD  CONSTRAINT [DF_Hishop_ShoppingCarts_Type]  DEFAULT ((0)) FOR [Type]
GO
ALTER TABLE [dbo].[Hishop_ShoppingCarts] ADD  DEFAULT ((0)) FOR [LimitedTimeDiscountId]
GO
ALTER TABLE [dbo].[Hishop_Votes] ADD  DEFAULT ('-1') FOR [MemberGrades]
GO
ALTER TABLE [dbo].[Hishop_Votes] ADD  DEFAULT ('-1') FOR [DefualtGroup]
GO
ALTER TABLE [dbo].[Hishop_Votes] ADD  DEFAULT ('-1') FOR [CustomGroup]
GO
ALTER TABLE [dbo].[vshop_AliFuwuReply] ADD  CONSTRAINT [vshop_AliFuwuReply_ArticleID]  DEFAULT ((0)) FOR [ArticleID]
GO
ALTER TABLE [dbo].[vshop_AlipayActiveOpendId] ADD  DEFAULT (getdate()) FOR [PubTime]
GO
ALTER TABLE [dbo].[vshop_ArticleItems] ADD  CONSTRAINT [DF_vshop_ArticleItems_ArticleId]  DEFAULT ((0)) FOR [ArticleId]
GO
ALTER TABLE [dbo].[vshop_ArticleItems] ADD  CONSTRAINT [DF_vshop_ArticleItems_LinkType]  DEFAULT ((0)) FOR [LinkType]
GO
ALTER TABLE [dbo].[vshop_ArticleItems] ADD  CONSTRAINT [DF_vshop_ArticleItems_PubTime]  DEFAULT (getdate()) FOR [PubTime]
GO
ALTER TABLE [dbo].[Vshop_CustomGroupingUser] ADD  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[Vshop_ShareMaterial_NineImages] ADD  DEFAULT ('') FOR [CreatTime]
GO
ALTER TABLE [dbo].[Vshop_ShareMaterial_NineImages] ADD  DEFAULT ('') FOR [image1]
GO
ALTER TABLE [dbo].[Vshop_ShareMaterial_NineImages] ADD  DEFAULT ('') FOR [image2]
GO
ALTER TABLE [dbo].[Vshop_ShareMaterial_NineImages] ADD  DEFAULT ('') FOR [image3]
GO
ALTER TABLE [dbo].[Vshop_ShareMaterial_NineImages] ADD  DEFAULT ('') FOR [image4]
GO
ALTER TABLE [dbo].[Vshop_ShareMaterial_NineImages] ADD  DEFAULT ('') FOR [image5]
GO
ALTER TABLE [dbo].[Vshop_ShareMaterial_NineImages] ADD  DEFAULT ('') FOR [image6]
GO
ALTER TABLE [dbo].[Vshop_ShareMaterial_NineImages] ADD  DEFAULT ('') FOR [image7]
GO
ALTER TABLE [dbo].[Vshop_ShareMaterial_NineImages] ADD  DEFAULT ('') FOR [image8]
GO
ALTER TABLE [dbo].[Vshop_ShareMaterial_NineImages] ADD  DEFAULT ('') FOR [image9]
GO
ALTER TABLE [dbo].[WeiXin_SendAll] ADD  CONSTRAINT [DF_WeiXin_SendAll_MessageType]  DEFAULT ((0)) FOR [MessageType]
GO
ALTER TABLE [dbo].[WeiXin_SendAll] ADD  CONSTRAINT [DF_WeiXin_SendAll_ArticleID]  DEFAULT ((0)) FOR [ArticleID]
GO
ALTER TABLE [dbo].[WeiXin_SendAll] ADD  CONSTRAINT [DF_WeiXin_SendAll_SendState]  DEFAULT ((0)) FOR [SendState]
GO
ALTER TABLE [dbo].[WeiXin_SendAll] ADD  DEFAULT (getdate()) FOR [SendTime]
GO
ALTER TABLE [dbo].[WeiXin_SendAll] ADD  DEFAULT ((0)) FOR [SendCount]
GO
ALTER TABLE [dbo].[WeiXin_SendAll] ADD  DEFAULT ((0)) FOR [TotalCount]
GO
ALTER TABLE [dbo].[WeiXin_SendAll] ADD  DEFAULT ((0)) FOR [platform]
GO
ALTER TABLE [dbo].[aspnet_RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_aspnet_RolePermissions_aspnet_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[aspnet_RolePermissions] CHECK CONSTRAINT [FK_aspnet_RolePermissions_aspnet_Roles]
GO
ALTER TABLE [dbo].[Hishop_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_Attributes_ProductTypes] FOREIGN KEY([TypeId])
REFERENCES [dbo].[Hishop_ProductTypes] ([TypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_Attributes] CHECK CONSTRAINT [FK_Hishop_Attributes_ProductTypes]
GO
ALTER TABLE [dbo].[Hishop_AttributeValues]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_AttributeValues_Attributes] FOREIGN KEY([AttributeId])
REFERENCES [dbo].[Hishop_Attributes] ([AttributeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_AttributeValues] CHECK CONSTRAINT [FK_Hishop_AttributeValues_Attributes]
GO
ALTER TABLE [dbo].[Hishop_CashBack]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_CashBack_aspnet_Members] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Members] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_CashBack] CHECK CONSTRAINT [FK_Hishop_CashBack_aspnet_Members]
GO
ALTER TABLE [dbo].[Hishop_CashBackDetails]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_CashBackDetails_Hishop_CashBack] FOREIGN KEY([CashBackId])
REFERENCES [dbo].[Hishop_CashBack] ([CashBackId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_CashBackDetails] CHECK CONSTRAINT [FK_Hishop_CashBackDetails_Hishop_CashBack]
GO
ALTER TABLE [dbo].[Hishop_CouponItems]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_CouponItems__Coupons] FOREIGN KEY([CouponId])
REFERENCES [dbo].[Hishop_Coupons] ([CouponId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_CouponItems] CHECK CONSTRAINT [FK_Hishop_CouponItems__Coupons]
GO
ALTER TABLE [dbo].[Hishop_InsuranceAreaCompany]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_InsuranceAreaCompany_Hishop_InsuranceArea] FOREIGN KEY([InsuranceAreaId])
REFERENCES [dbo].[Hishop_InsuranceArea] ([InsuranceAreaId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_InsuranceAreaCompany] CHECK CONSTRAINT [FK_Hishop_InsuranceAreaCompany_Hishop_InsuranceArea]
GO
ALTER TABLE [dbo].[Hishop_OrderDebitNote]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_OrderDebitNote_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Hishop_Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_OrderDebitNote] CHECK CONSTRAINT [FK_Hishop_OrderDebitNote_Orders]
GO
ALTER TABLE [dbo].[Hishop_OrderRefund]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_OrderRefund_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Hishop_Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_OrderRefund] CHECK CONSTRAINT [FK_Hishop_OrderRefund_Orders]
GO
ALTER TABLE [dbo].[Hishop_OrderReplace]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_OrderReplace_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Hishop_Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_OrderReplace] CHECK CONSTRAINT [FK_Hishop_OrderReplace_Orders]
GO
ALTER TABLE [dbo].[Hishop_OrderReturns]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_OrderReturns_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Hishop_Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_OrderReturns] CHECK CONSTRAINT [FK_Hishop_OrderReturns_Orders]
GO
ALTER TABLE [dbo].[Hishop_OrderSendNote]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_OrderSendNote_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Hishop_Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_OrderSendNote] CHECK CONSTRAINT [FK_Hishop_OrderSendNote_Orders]
GO
ALTER TABLE [dbo].[Hishop_ProductAttributes]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_ProductAttributes_Attributes] FOREIGN KEY([AttributeId])
REFERENCES [dbo].[Hishop_Attributes] ([AttributeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_ProductAttributes] CHECK CONSTRAINT [FK_Hishop_ProductAttributes_Attributes]
GO
ALTER TABLE [dbo].[Hishop_ProductAttributes]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_ProductAttributes_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Hishop_Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_ProductAttributes] CHECK CONSTRAINT [FK_Hishop_ProductAttributes_Products]
GO
ALTER TABLE [dbo].[Hishop_ProductTag]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_ProductTag_Hishop_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Hishop_Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_ProductTag] CHECK CONSTRAINT [FK_Hishop_ProductTag_Hishop_Products]
GO
ALTER TABLE [dbo].[Hishop_ProductTypeBrands]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_ProductTypeBrands_Hishop_BrandCategories] FOREIGN KEY([BrandId])
REFERENCES [dbo].[Hishop_BrandCategories] ([BrandId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_ProductTypeBrands] CHECK CONSTRAINT [FK_Hishop_ProductTypeBrands_Hishop_BrandCategories]
GO
ALTER TABLE [dbo].[Hishop_ProductTypeBrands]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_ProductTypeBrands_Hishop_ProductTypes] FOREIGN KEY([ProductTypeId])
REFERENCES [dbo].[Hishop_ProductTypes] ([TypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_ProductTypeBrands] CHECK CONSTRAINT [FK_Hishop_ProductTypeBrands_Hishop_ProductTypes]
GO
ALTER TABLE [dbo].[Hishop_PromotionGameResultMembersLog]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_PromotionGameResultMembersLog_Hishop_PromotionGame] FOREIGN KEY([GameId])
REFERENCES [dbo].[Hishop_PromotionGame] ([GameId])
GO
ALTER TABLE [dbo].[Hishop_PromotionGameResultMembersLog] CHECK CONSTRAINT [FK_Hishop_PromotionGameResultMembersLog_Hishop_PromotionGame]
GO
ALTER TABLE [dbo].[Hishop_ShoppingCarts]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_ShoppingCarts_aspnet_Members] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Members] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_ShoppingCarts] CHECK CONSTRAINT [FK_Hishop_ShoppingCarts_aspnet_Members]
GO
ALTER TABLE [dbo].[Hishop_SKUItems]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_SKUItems_SKUs] FOREIGN KEY([SkuId])
REFERENCES [dbo].[Hishop_SKUs] ([SkuId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_SKUItems] CHECK CONSTRAINT [FK_Hishop_SKUItems_SKUs]
GO
ALTER TABLE [dbo].[Hishop_SKUMemberPrice]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_SKUMemberPrice_aspnet_MemberGrades] FOREIGN KEY([GradeId])
REFERENCES [dbo].[aspnet_MemberGrades] ([GradeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_SKUMemberPrice] CHECK CONSTRAINT [FK_Hishop_SKUMemberPrice_aspnet_MemberGrades]
GO
ALTER TABLE [dbo].[Hishop_SKUMemberPrice]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_SKUMemberPrice_SKUs] FOREIGN KEY([SkuId])
REFERENCES [dbo].[Hishop_SKUs] ([SkuId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_SKUMemberPrice] CHECK CONSTRAINT [FK_Hishop_SKUMemberPrice_SKUs]
GO
ALTER TABLE [dbo].[Hishop_SKUs]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_SKUs_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Hishop_Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_SKUs] CHECK CONSTRAINT [FK_Hishop_SKUs_Products]
GO
ALTER TABLE [dbo].[Hishop_UserShippingAddresses]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_UserShippingAddresses_aspnet_Memberss] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Members] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_UserShippingAddresses] CHECK CONSTRAINT [FK_Hishop_UserShippingAddresses_aspnet_Memberss]
GO
ALTER TABLE [dbo].[Hishop_VoteItems]  WITH CHECK ADD  CONSTRAINT [FK_Hishop_VoteItems_Votes] FOREIGN KEY([VoteId])
REFERENCES [dbo].[Hishop_Votes] ([VoteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hishop_VoteItems] CHECK CONSTRAINT [FK_Hishop_VoteItems_Votes]
GO
ALTER TABLE [dbo].[Taobao_Products]  WITH CHECK ADD  CONSTRAINT [FK_Taobao_Products_Hishop_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Hishop_Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Taobao_Products] CHECK CONSTRAINT [FK_Taobao_Products_Hishop_Products]
GO
/****** Object:  StoredProcedure [dbo].[cp_AddIntegralDetail]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		刘继先
-- Description:	写入积分明显表
-- =============================================
CREATE PROCEDURE [dbo].[cp_AddIntegralDetail]
	@IntegralSourceType int,
	@IntegralSource varchar(100),
	@IntegralChange int,
	@Remark varchar(500),
	@Userid int,
	@GoToUrl varchar(100),
	@IntegralStatus int
AS
BEGIN
	if @IntegralChange <>0
	Begin
		Insert into vshop_IntegralDetail(IntegralSourceType,IntegralSource,IntegralChange,Remark,Userid,GoToUrl,TrateTime,IntegralStatus) 
		values(@IntegralSourceType,@IntegralSource,@IntegralChange,@Remark,@Userid,@GoToUrl,GETDATE(),@IntegralStatus);
	End
END

GO
/****** Object:  StoredProcedure [dbo].[cp_API_Orders_Get]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[cp_API_Orders_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@TotalOrders int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForOrders
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		OrderId nvarchar(50)
	)	

	INSERT INTO #PageIndexForOrders(OrderId)

	exec sp_executesql @sqlPopulate

	SET @TotalOrders = @@rowcount
	
	SELECT o.OrderId, 0 as SellerUid,Username,EmailAddress,ShipTo,
ShippingRegion,RegionId,Address,ZipCode,CellPhone,TelPhone,Remark,ManagerMark,ManagerRemark,RealName,
(select sum(Quantity) from Hishop_OrderItems where Hishop_OrderItems.OrderId=o.OrderId) as Nums,
 OrderTotal,Amount,AdjustedFreight,ReducedPromotionAmount as DiscountValue,AdjustedDiscount,Tax,InvoiceTitle,PayCharge,CouponValue,PaymentTypeId,PaymentType,PayDate,ShippingDate,OrderDate,UpdateDate,FinishDate,
ReFundStatus,RefundAmount,RefundRemark,OrderStatus,CloseReason,ModeName,Gateway,[Weight] from Hishop_Orders as o, #PageIndexForOrders
	WHERE 
		o.OrderId = #PageIndexForOrders.OrderId AND
		#PageIndexForOrders.IndexId > @PageLowerBound AND
		#PageIndexForOrders.IndexId < @PageUpperBound 
	ORDER BY #PageIndexForOrders.IndexId;
	SELECT 0 as Tid,OrderId,ProductId,ItemDescription,SKU,SkuId,SKUContent,Quantity,ItemListPrice,ItemAdjustedPrice,
	'0.00' as DiscountFee,'0.00' as Fee,ThumbnailsUrl,OrderItemsStatus from 
	Hishop_OrderItems where OrderId in (SELECT OrderId FROM #PageIndexForOrders WHERE IndexId > @PageLowerBound AND
		IndexId < @PageUpperBound)

	drop table #PageIndexForOrders
END

GO
/****** Object:  StoredProcedure [dbo].[cp_Category_Create]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_Category_Create]
	(
		@IconUrl NVARCHAR(255) = NULL,
		@Name NVARCHAR(100) = NULL,
		@DisplaySequence INT = NULL,
		@Meta_Title NVARCHAR(1000)=NULL,
		@Meta_Description NVARCHAR(1000) = NULL,
		@Meta_Keywords NVARCHAR(1000) = NULL,
		@ParentCategoryId INT = NULL,
		@RewriteName NVARCHAR(50) = NULL,
		@SKUPrefix NVARCHAR(10) = NULL,
		@AssociatedProductType INT = NULL,
		@Notes1 NTEXT = NULL,
		@Notes2 NTEXT = NULL,
		@Notes3 NTEXT = NULL,
		@Notes4 NTEXT = NULL,
		@Notes5 NTEXT = NULL,
		@FirstCommission NVARCHAR(50),
		@SecondCommission NVARCHAR(50),
		@ThirdCommission NVARCHAR(50),
		@CategoryId INT OUTPUT
	)
AS
Declare @Err As int
SELECT @Err=0

SET XACT_ABORT ON
Begin Tran
	
IF @ParentCategoryId IS NULL OR @ParentCategoryId < 0
	SET @ParentCategoryId = 0
	
--通过现有记录获取栏目ID


Select @CategoryId = ISNULL(Max(CategoryId),0) From Hishop_Categories
IF @CategoryId Is Not Null
	Set @CategoryId = @CategoryId+1
Else
	Set @CategoryId = 1

--判断是否是顶级栏目，设置其Path和Depth
Declare @Depth As int
Declare @Path As nvarchar(4000)

IF @ParentCategoryId = 0
Begin
	Select @DisplaySequence = ISNULL(MAX(DisplaySequence),0) + 1 from Hishop_Categories where ParentCategoryId = 0
	Set @Path =Ltrim(RTRIM(Str(@CategoryId)))
	Set @Depth = 1
End
Else
Begin
	--获取父节点的路径和深度
	Select @Path = [Path] ,@Depth = Depth From Hishop_Categories Where CategoryId=@ParentCategoryId
	Select @DisplaySequence = ISNULL(MAX(DisplaySequence),0) + 1 from Hishop_Categories where ParentCategoryId = @ParentCategoryId
	IF @Path Is Null
	Begin
		Set @Err = 1
		Goto theEnd
	End
	
	Set @Path = @Path + '|' + Ltrim(RTRIM(Str(@CategoryId)))
	Set @Depth = @Depth+1
End

Insert Into Hishop_Categories(
	CategoryId, [Name], DisplaySequence,IconUrl,Meta_Title, Meta_Description, Meta_Keywords, SKUPrefix,AssociatedProductType,
	ParentCategoryId, Depth, Path, RewriteName, Notes1, Notes2, Notes3, Notes4, Notes5,FirstCommission,SecondCommission,ThirdCommission
) 
Values(
	@CategoryId, @Name, @DisplaySequence,@IconUrl,@Meta_Title, @Meta_Description, @Meta_Keywords, @SKUPrefix,@AssociatedProductType,
	@ParentCategoryId, @Depth, @Path, @RewriteName, @Notes1, @Notes2, @Notes3, @Notes4, @Notes5,@FirstCommission,@SecondCommission,@ThirdCommission
)

IF @@Error<>0 
Begin
	Set @Err=1
	Goto theEnd
End

theEnd:
IF @Err=0
Begin
	Commit Tran
	Return @CategoryId
End
Else
Begin
    Rollback Tran
	Return 0
End

GO
/****** Object:  StoredProcedure [dbo].[cp_Category_Delete]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_Category_Delete]
(
	@CategoryId INT
)
AS
Declare @Err As int, @Count INT, @DisplaySequence INT
DECLARE @Path nvarchar(4000)

SET XACT_ABORT ON
Begin Tran

CREATE TABLE #tempTable (CategoryId INT)

SELECT @Err = 0, @Path = Path, @DisplaySequence = DisplaySequence FROM Hishop_Categories WHERE CategoryId = @CategoryId
INSERT INTO #tempTable SELECT CategoryId FROM Hishop_Categories WHERE CategoryId = @CategoryId OR Path LIKE '' + @Path + '|%'

SET @Count = @@ROWCOUNT

-- 删除自身和所有子分类
DELETE From Hishop_Categories Where CategoryId IN (SELECT CategoryId FROM #tempTable)

-- 修改商品分类的编号为0，表示未分类
UPDATE Hishop_Products SET CategoryId = 0, MainCategoryPath = null WHERE CategoryId IN (SELECT CategoryId FROM #tempTable)


DROP TABLE #tempTable

  Commit Tran

GO
/****** Object:  StoredProcedure [dbo].[cp_ClaimCode_Create]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_ClaimCode_Create]
	(
		@row int,
        @CouponId int,
        @UserId int,
        @EmailAddress nvarchar(255),
		@UserName nvarchar(256),
        @ReturnLotNumber nvarchar(300) OUTPUT
	)
AS
declare @LotNumber uniqueidentifier
set  @LotNumber=newid()
 WHILE(@row>0)
BEGIN
SET ROWCOUNT @row
    INSERT Hishop_CouponItems SELECT
         CouponId=@CouponId,
         LotNumber=@LotNumber,
        ClaimCode =SUBSTRING(REPLACE(newid(),'-',''),1,15),
        UserId=@UserId,UserName=@UserName,EmailAddress=@EmailAddress,GenerateTime=getdate(),CouponStatus=0,UsedTime=null,OrderId=null
    FROM syscolumns c1, syscolumns c2   
 SET @row = @row - @@ROWCOUNT
END
SET @ReturnLotNumber=CONVERT(NVARCHAR(300),@LotNumber)

GO
/****** Object:  StoredProcedure [dbo].[cp_GamePrize]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_GamePrize]
	@GameId int,
	@PrizeId int,
	@UserId int,
	@Result int output
AS
BEGIN
	Set @Result=0;
	Declare @IsUseD bit;
	Set @IsUseD=1;
	--玩本次抽奖需要的积分与赠送的积分
	Declare @NeedPoint int;
	Declare @GivePoint int;
	Declare @IsAllGivePoint bit;
	Declare @GameUrl varchar(100);
	Select @NeedPoint=-1*NeedPoint,@GivePoint=GivePoint,@IsAllGivePoint=OnlyGiveNotPrizeMember,@GameUrl=GameUrl from Hishop_PromotionGame Where GameId=@GameId; 
	
	if @PrizeId>0 --表示中奖了的
	Begin
		Declare @PrizeType int;
		Declare @PrizeGivePoint int;
		Declare @CouponsId varchar(100);
		Select @PrizeType=PrizeType,@PrizeGivePoint=GivePoint,@CouponsId=GiveCouponId From Hishop_PromotionGamePrizes where PrizeId=@PrizeId;
		if @PrizeType=0--赠送积分
		Begin
			Declare @Point int;
			Set @Point=0;
			if @IsAllGivePoint=0
			Begin
				set @Point=@GivePoint+@NeedPoint;
				exec cp_AddIntegralDetail 1,'抽奖赠送',@GivePoint,'',@UserId,@GameUrl,3;
			End
			else
			Begin
				set @Point=@NeedPoint;
			End
			set @Point=@Point+@PrizeGivePoint;
			Update aspnet_Members set Points=Points+@Point where  UserId=@UserId;
			exec cp_AddIntegralDetail 1,'积分奖品',@PrizeGivePoint,'',@UserId,@GameUrl,3;
		End
		if @PrizeType=1 --赠送优惠劵
		Begin
			Declare @Point1 int;
			SET @Point1=0;
			if @IsAllGivePoint=0
			Begin
				set @Point1=@GivePoint+@NeedPoint;
				exec cp_AddIntegralDetail 1,'抽奖赠送',@GivePoint,'',@UserId,@GameUrl,3;
			End
			else
			Begin
				set @Point1=@NeedPoint;
			End
			if @Point1 <>0 --玩游戏使用的积分与赠送的积分
			Begin
				Update aspnet_Members set Points=Points+@Point1 where  UserId=@UserId;
			End
			--把优惠劵写入红包中
			--把优惠劵用户表中
			Declare @ResultTemp int;
			Exec cp_SendCouponToMember @CouponsId,@UserId,@ResultTemp output;
		End
		if @PrizeType=2 or @PrizeType=3  --赠送商品
		Begin
			Set @IsUseD=0;
			Declare @Point2 int;
			SET @Point2=0;
			if @IsAllGivePoint=0
			Begin
				set @Point2=@GivePoint+@NeedPoint;
				exec cp_AddIntegralDetail 1,'抽奖赠送',@GivePoint,'',@UserId,@GameUrl,3;
			End
			else
			Begin
				set @Point2=@NeedPoint;
			End
			if @Point2 <>0 --玩游戏使用的积分与赠送的积分
			Begin
				Update aspnet_Members set Points=Points+@Point2 where  UserId=@UserId;
			End
		End
		--抽奖一次,会把奖品数量减1
		--Update Hishop_PromotionGamePrizes set PrizeCount=PrizeCount-1 where PrizeId=@PrizeId;
	End
	else
	Begin--表示没有中奖的
		--当需要积分与赠送积分
		Declare @Point0 int;
		set @Point0=@GivePoint+@NeedPoint;
		if @GivePoint>0
		Begin
			exec cp_AddIntegralDetail 1,'抽奖赠送',@GivePoint,'',@UserId,@GameUrl,3;
		End
		if @Point0 <>0
		Begin
			Update aspnet_Members set Points=Points+@Point0 where  UserId=@UserId;
		End
	End
	exec cp_AddIntegralDetail 2,'抽奖消耗',@NeedPoint,'',@UserId,@GameUrl,6;
	--插入记录(是否中奖，都会插入记录)
	INSERT INTO [Hishop_PromotionGameResultMembersLog]
           ([GameId]
           ,[PrizeId]
           ,[UserId]
           ,[PlayTime]
           ,[IsUsed])
		VALUES(@GameId,@PrizeId,@UserId,GETDATE(),@IsUseD);
	Set @Result=1;	
END

GO
/****** Object:  StoredProcedure [dbo].[cp_IsCanPrize]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_IsCanPrize]
	@GameId int,
	@UserId int,
	@Result int output
AS
BEGIN
	set @Result=0;
	--判断游戏是否正常
	Declare @Count int;
	Declare @EveryDayCount int;
	Declare @PayCount int;
	Set @Count=0;
	Set @EveryDayCount=0;
	Set @PayCount=0;
	Select @EveryDayCount=LimitEveryDay,@PayCount=MaximumDailyLimit from Hishop_PromotionGame Where GameId=@GameId And Status=0;
	Select @Count=COUNT(*) From Hishop_PromotionGame where GameId=@GameId And Status=0;
	if @Count<=0
	Begin
		set @Result=7;--表示当前游戏已结束
		return;
	End
	Select @Count=COUNT(*) From Hishop_PromotionGame where GameId=@GameId And Status=0 And BeginTime<=GETDATE();
	if @Count<=0
	Begin
		set @Result=8;--表示当前游戏还没开始
		return;
	End
	Select @Count=COUNT(*) From Hishop_PromotionGame where GameId=@GameId And BeginTime<=GETDATE() And EndTime>=GETDATE();
	if @Count<=0
	Begin
		set @Result=7;--表示当前游戏已结束
		return;
	End
	--获取游戏的参与方式与当前积分是否足够
	Declare @PlayType int;
	Declare @NeedPoint int;
	Select @PlayType=PlayType,@NeedPoint=NeedPoint from Hishop_PromotionGame Where GameId=@GameId;
	 
	--判断当前积分是否足够
	if @NeedPoint >0
	Begin
		Declare @UserPoint int;
		select @UserPoint=Points from dbo.aspnet_Members where UserId=@UserId;
		if @UserPoint < @NeedPoint
		Begin
			set @Result=2;--表示当前用户积分不够
			return;
		End
	End
	--判断会员今天是否能参数
	Set @Count=0;
	select @Count=COUNT(*) from Hishop_PromotionGameResultMembersLog where UserId=@UserId AND GameId=@GameId And PlayTime>=CONVERT(varchar(100), GETDATE(), 23)
	if  @Count >=@EveryDayCount and @EveryDayCount>0
	Begin
	   set @Result=3;--表示当前用户当天不能玩了
	   return;
	End
	select @Count=COUNT(*) from Hishop_PromotionGameResultMembersLog where UserId=@UserId AND GameId=@GameId
	if @Count >=@PayCount and @PayCount>0
	Begin
	   set @Result=4;--表示当前用户不能玩了
	   return;
	End
	--if @PlayType=0 --一天一次
	--Begin
	--	select @Count=COUNT(*) from Hishop_PromotionGameResultMembersLog where UserId=@UserId AND GameId=@GameId And PlayTime>=CONVERT(varchar(100), GETDATE(), 23)
	--	if @Count>=1
	--	Begin
	--		set @Result=3;--表示当前用户当天已玩过了
	--		return;
	--	End
	--End 
	--else if @PlayType=1 --一人一次
	--Begin
	--	select @Count=COUNT(*) from Hishop_PromotionGameResultMembersLog where UserId=@UserId AND GameId=@GameId ;
	--	if @Count>=1
	--	Begin
	--		set @Result=4;--表示当前用户已玩过一次
	--		return;
	--	End
	--End
	--else if @PlayType=2 --一天两次
	--Begin
	--	select @Count=COUNT(*) from Hishop_PromotionGameResultMembersLog where UserId=@UserId AND GameId=@GameId And PlayTime>=CONVERT(varchar(100), GETDATE(), 23)
	--	if @Count>=2
	--	Begin
	--		set @Result=5;--表示当前用户当天已玩过两次了
	--		return;
	--	End
	--End
	--else if @PlayType=3 --一人两次
	--Begin
	--	select @Count=COUNT(*) from Hishop_PromotionGameResultMembersLog where UserId=@UserId AND GameId=@GameId ;
	--	if @Count>=2
	--	Begin
	--		set @Result=6;--表示当前用户已玩过两次
	--		return;
	--	End
	--End
END

GO
/****** Object:  StoredProcedure [dbo].[cp_MemberCanReceiveCoupon]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--刘继先
--判断客户是否可以领取此优惠劵
CREATE PROCEDURE [dbo].[cp_MemberCanReceiveCoupon]
	@CouponsId int,
	@UserId int,
	@Result int output
AS
Begin
	Declare @Count int;
	Set @Count=0;
	Select @Count=COUNT(*) From Hishop_Coupon_Coupons Where Finished=0 And EndDate>=GETDATE() And CouponId=@CouponsId;
	if @Count >0
	Begin
		Declare @MaxReceivNum int;
		Declare @StockNum int;
		Declare @ReceiveNum int;
		Declare @MemberGrades varchar(100);
		Set @MaxReceivNum=0;
		Set @StockNum=0;
		Set @ReceiveNum=0;
		Select @MaxReceivNum=maxReceivNum,@StockNum=StockNum,@ReceiveNum=ReceiveNum,@MemberGrades=MemberGrades From Hishop_Coupon_Coupons Where  CouponId=@CouponsId;

		--判断会员等级是否可以参数
		--if @MemberGrades!='0'
		--	Begin
		--	Declare @UserGrade varchar(50);
		--	select @UserGrade=GradeId from dbo.aspnet_Members where UserId=@UserId;
		--	Set @MemberGrades=@MemberGrades+',';
		--   if charindex(@UserGrade+',',@MemberGrades+',')<1
		--   Begin	
		--		set @Result=2;--表示当前用户会员等级不在此活动范内
		--		return;
		--   End
		--End
		
		--判断是否已领完
		if @ReceiveNum+1>@StockNum
		Begin
			set @Result=3;--表示优惠劵已领完
			return;
		End
		
		--判断是否还可以领取
		Set @Count=0;
		Select @Count=COUNT(*) From Hishop_Coupon_MemberCoupons where MemberId=@UserId And CouponId=@CouponsId;
		if @Count>=@MaxReceivNum
		Begin
			set @Result=4;--表示此用户不能再领取此优惠劵
			return;
		End
		
	End
	Else
	Begin
		Set @Result=1;--表示优惠劵已结束
		Return;
	End
	Set @Result=0;
End

GO
/****** Object:  StoredProcedure [dbo].[cp_MemberStatistics_Get]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_MemberStatistics_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@TotalProductSales int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForProductSales
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		UserName nvarchar(256) NOT NULL,
		UserId int,
		SaleTotals money default(0),
		OrderCount int default(0)
	)	

	INSERT INTO #PageIndexForProductSales(UserId, UserName, SaleTotals, OrderCount)
	Exec sp_executesql @sqlPopulate

	SET @TotalProductSales = @@rowcount
	
	SELECT S.IndexId,
		S.Username, ISNULL(S.SaleTotals, 0) as SaleTotals , ISNULL(S.OrderCount, 0) as OrderCount
	FROM   #PageIndexForProductSales S
	WHERE 
			S.IndexId > @PageLowerBound AND
			S.IndexId < @PageUpperBound 
	ORDER BY S.IndexId
	
	drop table #PageIndexForProductSales
END

GO
/****** Object:  StoredProcedure [dbo].[cp_Menu_SwapDisplaySequence]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_Menu_SwapDisplaySequence]
	(
		@MenuId INT,
		@ZIndex INT
	)
AS
	
	DECLARE @ParentMenuId INT, @DisplaySequence INT
	DECLARE @swap_MenuId INT, @swap_DisplaySequence INT
	
	SELECT @ParentMenuId = ParentMenuId, @DisplaySequence = DisplaySequence FROM vshop_Menu WHERE MenuId = @MenuId

	IF @ZIndex = 0
		SELECT TOP 1 @swap_MenuId = MenuId, @swap_DisplaySequence = DisplaySequence FROM vshop_Menu WHERE ParentMenuId = @ParentMenuId AND DisplaySequence < @DisplaySequence ORDER BY DisplaySequence DESC
	ELSE
		SELECT TOP 1 @swap_MenuId = MenuId, @swap_DisplaySequence = DisplaySequence FROM vshop_Menu WHERE ParentMenuId = @ParentMenuId AND DisplaySequence > @DisplaySequence ORDER BY DisplaySequence ASC
	
	IF @swap_MenuId IS NULL
		RETURN;

	SET XACT_ABORT ON
	BEGIN TRAN
	
	UPDATE vshop_Menu SET DisplaySequence = @swap_DisplaySequence WHERE MenuId = @MenuId
	UPDATE vshop_Menu SET DisplaySequence = @DisplaySequence WHERE MenuId = @swap_MenuId
	
	COMMIT TRAN

GO
/****** Object:  StoredProcedure [dbo].[cp_OrderStatistics_Get]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[cp_OrderStatistics_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@TotalUserOrders int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForUserOrders
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		OrderId nvarchar(50)
	)	

	INSERT INTO #PageIndexForUserOrders(OrderId)
	Exec sp_executesql @sqlPopulate

	SET @TotalUserOrders = @@rowcount
	
	SELECT O.OrderId, OrderDate, Isnull(OrderTotal,0) as Total, Username, ShipTo,Isnull(OrderProfit,0) As Profits
	FROM Hishop_Orders O, #PageIndexForUserOrders UO 
	WHERE 
			O.OrderId = UO.OrderId AND
			UO.IndexId > @PageLowerBound AND
			UO.IndexId < @PageUpperBound
	ORDER BY UO.IndexId 
    ------------------------------------------------------------
    -- ��ҳ�������,�ܽ��,����
    SELECT 
		Isnull(SUM(OrderTotal),0) AS OrderTotal, -- �ܽ��
        Isnull(SUM(OrderProfit),0) AS Profits --����
	FROM Hishop_Orders o,#PageIndexForUserOrders
    where
       o.OrderId = #PageIndexForUserOrders.OrderId and
       #PageIndexForUserOrders.IndexId > @PageLowerBound and  
	   #PageIndexForUserOrders.IndexId < @PageUpperBound
   
    -- �����������,�ܽ��,����
    SELECT 
		Isnull(SUM(OrderTotal),0) AS OrderTotal, -- �ܽ��
        Isnull(SUM(OrderProfit),0) AS Profits --����
	FROM Hishop_Orders o,#PageIndexForUserOrders
    where
       o.OrderId = #PageIndexForUserOrders.OrderId  
      
    drop table #PageIndexForUserOrders
END

GO
/****** Object:  StoredProcedure [dbo].[cp_OrderStatisticsNoPage_Get]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_OrderStatisticsNoPage_Get]
(
	@sqlPopulate ntext,
	@TotalUserOrders int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN

	CREATE TABLE #PageIndexForUserOrders
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		OrderId nvarchar(50)
	)	

	INSERT INTO #PageIndexForUserOrders(OrderId)
	Exec sp_executesql @sqlPopulate

	SET @TotalUserOrders = @@rowcount
	
	SELECT O.OrderId, OrderDate, Isnull(OrderTotal,0) as Total, Username, ShipTo, Isnull(OrderProfit,0) As Profits
	FROM Hishop_Orders O, #PageIndexForUserOrders UO 
	WHERE 
			O.OrderId = UO.OrderId
	ORDER BY UO.IndexId 
    ------------------------------------------------------------
    -- 当次搜索结果,总金额,利润
    SELECT 
		Isnull(SUM(OrderTotal),0) AS OrderTotal, -- 总金额
        Isnull(SUM(OrderProfit),0) AS Profits --利润
	FROM Hishop_Orders o,#PageIndexForUserOrders
    where
       o.OrderId = #PageIndexForUserOrders.OrderId
   drop table #PageIndexForUserOrders
END

GO
/****** Object:  StoredProcedure [dbo].[cp_PaymentType_CreateUpdateDelete]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[cp_PaymentType_CreateUpdateDelete]
(
@ModeId INT = NULL OUTPUT,
@Name NVARCHAR(100) = null,
@Description NVARCHAR(4000) = NULL,
@Gateway NVARCHAR(200) = NULL,
@IsUseInpour BIT = NULL,
@IsUseInDistributor BIT = NULL,
@Charge MONEY = NULL,
@IsPercent BIT = NULL,
@Settings NTEXT = NULL,
@Action INT,
@Status INT OUTPUT
)
AS

DECLARE @DisplaySequence INT

SET @Status = 99

IF @Action = 2 -- 删除
BEGIN
DELETE FROM Hishop_PaymentTypes WHERE ModeId = @ModeId
IF @@ROWCOUNT = 1
BEGIN
SET @Status = 0
END

RETURN
END

IF @Action = 0 -- 创建
BEGIN

--如果取最大序号为空 则直接将序号设置为1
IF (Select Max(DisplaySequence) From Hishop_PaymentTypes) IS NULL
SET @DisplaySequence=1
-- 如果不为空则将 将序号设置为表中现有的最大值加1
ELSE
SELECT @DisplaySequence = MAX(DisplaySequence)+1 FROM Hishop_PaymentTypes

INSERT INTO
Hishop_PaymentTypes([Name], Description, Gateway, DisplaySequence, IsUseInpour,IsUseInDistributor, Charge, IsPercent, Settings)
VALUES
(@Name, @Description, @Gateway, @DisplaySequence, @IsUseInpour, @IsUseInDistributor,@Charge, @IsPercent, @Settings)

SELECT @ModeId = @@IDENTITY
IF @@ROWCOUNT = 1
SET @Status = 0

RETURN
END

IF @Action = 1 -- 修改
BEGIN

UPDATE
Hishop_PaymentTypes
SET
[Name] = @Name,
Description = @Description,
IsUseInpour = @IsUseInpour,
IsUseInDistributor = @IsUseInDistributor,
Charge = @Charge,
IsPercent = @IsPercent,
Settings = @Settings
WHERE ModeId = @ModeId
IF @@ROWCOUNT = 1
SET @Status = 0

RETURN
END

GO
/****** Object:  StoredProcedure [dbo].[cp_Product_Create]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_Product_Create]
(
@CategoryId INT,
@MainCategoryPath NVARCHAR(256),
@TypeId INT = NULL,
@ProductName NVARCHAR(200),
@ProductShortName NVARCHAR(50),
@ProductCode [nvarchar] (50),
@ShortDescription NVARCHAR(2000) = NULL,
@Unit NVARCHAR(10) = NULL,
@Description NTEXT = NULL,
@Title NVARCHAR(100) = NULL,
@Meta_Description NVARCHAR(1000) = NULL,
@Meta_Keywords NVARCHAR(1000) = NULL,
@SaleStatus INT,
@AddedDate DATETIME,
@ImageUrl1 [nvarchar] (255) = NULL,
@ImageUrl2 [nvarchar] (255) = NULL,
@ImageUrl3 [nvarchar] (255) = NULL,
@ImageUrl4 [nvarchar] (255) = NULL,
@ImageUrl5 [nvarchar] (255) = NULL,
@ThumbnailUrl40 [nvarchar] (255) = NULL,
@ThumbnailUrl60 [nvarchar] (255) = NULL,
@ThumbnailUrl100 [nvarchar] (255) = NULL,
@ThumbnailUrl160 [nvarchar] (255) = NULL,
@ThumbnailUrl180 [nvarchar] (255) = NULL,
@ThumbnailUrl220 [nvarchar] (255) = NULL,
@ThumbnailUrl310 [nvarchar] (255) = NULL,
@ThumbnailUrl410 [nvarchar] (255) = NULL,
@MarketPrice MONEY = NULL,
@BrandId [int],
@HasSKU [BIT],
@IsfreeShipping [bit],
@TaobaoProductId [bigint],
@ShowSaleCounts int,

@MinShowPrice MONEY =0,
@MaxShowPrice MONEY =0,
@FreightTemplateId INT =0,
@FirstCommission DECIMAL(8,2) =0,
@SecondCommission DECIMAL(8,2) =0,
@ThirdCommission DECIMAL(8,2) =0,
@IsSetCommission BIT=0,
@CubicMeter DECIMAL(8,2)=0,
@FreightWeight  DECIMAL(8,2)=0,


@ProductId INT OUTPUT
)
AS

SET @ProductId = 0

--商品的顺序号取当前的最大值加1
DECLARE @DisplaySequence INT
SELECT @DisplaySequence = MAX(DisplaySequence) + 1 FROM  Hishop_Products
if @DisplaySequence is null
 	set @DisplaySequence = 1

INSERT INTO Hishop_Products
(CategoryId, MainCategoryPath, TypeId, ProductName,ProductShortName, ProductCode, ShortDescription, Unit, [Description], SaleStatus, AddedDate, DisplaySequence,
ImageUrl1, ImageUrl2, ImageUrl3, ImageUrl4, ImageUrl5, ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100, ThumbnailUrl160, ThumbnailUrl180,
ThumbnailUrl220, ThumbnailUrl310, ThumbnailUrl410,
MarketPrice, BrandId, HasSKU,IsfreeShipping,TaobaoProductId,MinShowPrice,MaxShowPrice,FreightTemplateId,FirstCommission,SecondCommission,ThirdCommission,IsSetCommission,CubicMeter,FreightWeight,ShowSaleCounts)
Values
(@CategoryId, @MainCategoryPath, @TypeId, @ProductName,@ProductShortName, @ProductCode, @ShortDescription, @Unit,  @Description,@SaleStatus, @AddedDate, @DisplaySequence,
@ImageUrl1, @ImageUrl2, @ImageUrl3, @ImageUrl4, @ImageUrl5, @ThumbnailUrl40, @ThumbnailUrl60, @ThumbnailUrl100, @ThumbnailUrl160, @ThumbnailUrl180,
@ThumbnailUrl220, @ThumbnailUrl310, @ThumbnailUrl410,
@MarketPrice, @BrandId, @HasSKU,@IsfreeShipping,@TaobaoProductId,@MinShowPrice,@MaxShowPrice,@FreightTemplateId,@FirstCommission,@SecondCommission,@ThirdCommission,@IsSetCommission,@CubicMeter,@FreightWeight,@ShowSaleCounts)
SET @ProductId = @@IDENTITY;

GO
/****** Object:  StoredProcedure [dbo].[cp_Product_GetExportList]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_Product_GetExportList]
	@sqlPopulate ntext
AS
	CREATE TABLE #Products
	(
		[ProductId] int,
		[TypeId] int,
		[ProductName] [nvarchar] (200),
		[ProductCode] [nvarchar] (50),
		[ShortDescription] [nvarchar] (2000),
		[Unit] [nvarchar] (50),
		[Description] [ntext],		
		[SaleStatus] [int],
		[ImageUrl1] [nvarchar] (255),
		[ImageUrl2] [nvarchar] (255),
		[ImageUrl3] [nvarchar] (255),
		[ImageUrl4] [nvarchar] (255),
		[ImageUrl5] [nvarchar] (255),
		[MarketPrice] [money],
		[HasSKU] [BIT]
	)

	-- 商品
	INSERT INTO #Products ([ProductId], [TypeId], [ProductName], [ProductCode], [ShortDescription], [Unit], [Description],
		[SaleStatus], [ImageUrl1], [ImageUrl2], [ImageUrl3], [ImageUrl4], [ImageUrl5], [MarketPrice], [HasSKU]) 
    Exec sp_executesql @sqlPopulate
	-- 类型
	SELECT TypeId, TypeName, Remark INTO  #Types FROM Hishop_ProductTypes WHERE TypeId IN (SELECT DISTINCT([TypeId]) FROM #Products WHERE #Products.TypeId IS NOT NULL)
	-- 规格
	SELECT [SkuId], [ProductId], [SKU], [Weight], [Stock], [CostPrice], [SalePrice] INTO #Skus
		FROM Hishop_SKUs WHERE ProductId IN (SELECT ProductId FROM #Products)
	-- 规格项
	SELECT [SkuId], [AttributeId], [ValueId] INTO #SKUItems FROM Hishop_SKUItems WHERE SkuId IN (SELECT SkuId FROM #Skus)
	-- 商品属性
	SELECT [ProductId], [AttributeId], [ValueId] INTO #ProductAttributes FROM Hishop_ProductAttributes WHERE ProductId IN (SELECT ProductId FROM #Products)
	-- 属性
	SELECT [AttributeId], [AttributeName], [DisplaySequence], [TypeId], [UsageMode], [UseAttributeImage] INTO #Attributes 
		FROM Hishop_Attributes WHERE [AttributeId] IN (SELECT DISTINCT([AttributeId]) FROM #SKUItems UNION SELECT DISTINCT([AttributeId]) FROM #ProductAttributes)
	-- 属性值
	SELECT [ValueId], [AttributeId], [DisplaySequence], [ValueStr], [ImageUrl] INTO #Values 
		FROM Hishop_AttributeValues WHERE [ValueId] IN (SELECT DISTINCT([ValueId]) FROM #SKUItems UNION SELECT DISTINCT([ValueId]) FROM #ProductAttributes)
		
	---淘宝属性值
	SELECT  *
		INTO #TaoBaoSKU FROM dbo.Taobao_Products WHERE [ProductId] IN (SELECT ProductId FROM #Products) 


	-- 输出商品类型
	SELECT * FROM #Types
	-- 输出类型的属性
	SELECT * FROM #Attributes
	--输出属性值
	SELECT * FROM #Values
	--输出商品信息
	SELECT * FROM #Products
	-- 输出商品规格信息
	SELECT * FROM #Skus
	-- 输出商品规格的字段值
	SELECT * FROM #SKUItems
	-- 输出商品属性
	SELECT * FROM #ProductAttributes
	-- 输出淘宝属性值
	SELECT * FROM #TaoBaoSKU

	DROP TABLE #Types
	DROP TABLE #Attributes
	DROP TABLE #Values
	DROP TABLE #Products
	DROP TABLE #Skus
	DROP TABLE #SKUItems
	DROP TABLE #ProductAttributes
	DROP TABLE #TaoBaoSKU

GO
/****** Object:  StoredProcedure [dbo].[cp_Product_Update]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_Product_Update]
(
@CategoryId INT,
@MainCategoryPath NVARCHAR(256),
@TypeId INT = NULL,
@ProductName NVARCHAR(200),
@ProductShortName NVARCHAR(50),
@ProductCode [nvarchar] (50),
@ShortDescription NVARCHAR(2000) = NULL,
@Unit NVARCHAR(10) = NULL,
@Description NTEXT = NULL,
@SaleStatus INT,
@DisplaySequence INT,
@ImageUrl1 [nvarchar] (255) = NULL,
@ImageUrl2 [nvarchar] (255) = NULL,
@ImageUrl3 [nvarchar] (255) = NULL,
@ImageUrl4 [nvarchar] (255) = NULL,
@ImageUrl5 [nvarchar] (255) = NULL,
@ThumbnailUrl40 [nvarchar] (255) = NULL,
@ThumbnailUrl60 [nvarchar] (255) = NULL,
@ThumbnailUrl100 [nvarchar] (255) = NULL,
@ThumbnailUrl160 [nvarchar] (255) = NULL,
@ThumbnailUrl180 [nvarchar] (255) = NULL,
@ThumbnailUrl220 [nvarchar] (255) = NULL,
@ThumbnailUrl310 [nvarchar] (255) = NULL,
@ThumbnailUrl410 [nvarchar] (255) = NULL,
@MarketPrice MONEY = NULL,
@BrandId INT,
@HasSKU BIT,
@IsfreeShipping [bit],
@SaleCounts INT,
@ShowSaleCounts INT,
@ProductId INT,
@MinShowPrice MONEY =0,
@MaxShowPrice MONEY =0,
@FreightTemplateId INT =0,
@FirstCommission DECIMAL(8,2) =0,
@SecondCommission DECIMAL(8,2) =0,
@ThirdCommission DECIMAL(8,2)=0,
@IsSetCommission BIT=0,
@CubicMeter DECIMAL(8,2)=0,
@FreightWeight  DECIMAL(8,2)=0
)
AS

--如果商品显示顺序存在，则所有这个商品后台的顺序加一
IF (SELECT DisplaySequence FROM Hishop_Products WHERE ProductId = @ProductId) != @DisplaySequence AND EXISTS(SELECT ProductId FROM Hishop_Products WHERE DisplaySequence = @DisplaySequence)
UPDATE Hishop_Products SET DisplaySequence = DisplaySequence + 1 WHERE DisplaySequence >= @DisplaySequence

UPDATE Hishop_Products SET
CategoryId = @CategoryId, MainCategoryPath = @MainCategoryPath, TypeId = @TypeId, ProductName = @ProductName,ProductShortName=@ProductShortName, ProductCode = @ProductCode,
ShortDescription = @ShortDescription, Unit = @Unit, [Description] = @Description, MarketPrice = @MarketPrice, SaleStatus = @SaleStatus, DisplaySequence = @DisplaySequence,
ImageUrl1 = @ImageUrl1, ImageUrl2 = @ImageUrl2, ImageUrl3 = @ImageUrl3, ImageUrl4 = @ImageUrl4, ImageUrl5 = @ImageUrl5,
ThumbnailUrl40 = @ThumbnailUrl40, ThumbnailUrl60 = @ThumbnailUrl60, ThumbnailUrl100 = @ThumbnailUrl100, ThumbnailUrl160 = @ThumbnailUrl160, ThumbnailUrl180 = @ThumbnailUrl180,
ThumbnailUrl220 = @ThumbnailUrl220, ThumbnailUrl310 = @ThumbnailUrl310, ThumbnailUrl410 = @ThumbnailUrl410, 
BrandId = @BrandId, HasSKU = @HasSKU,IsfreeShipping=@IsfreeShipping,SaleCounts = @SaleCounts, ShowSaleCounts = @ShowSaleCounts,
MinShowPrice=@MinShowPrice,MaxShowPrice=@MaxShowPrice,FreightTemplateId=@FreightTemplateId,FirstCommission=@FirstCommission,SecondCommission=@SecondCommission,ThirdCommission=@ThirdCommission,IsSetCommission=@IsSetCommission,CubicMeter=@CubicMeter,FreightWeight=@FreightWeight
WHERE ProductId = @ProductId

GO
/****** Object:  StoredProcedure [dbo].[cp_ProductSales_Get]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[cp_ProductSales_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@TotalProductSales int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForProductSales
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		ProductId int,
		ProductSaleCounts int,
		ProductSaleTotals money,
		ProductProfitsTotals money
	)	

	INSERT INTO #PageIndexForProductSales(ProductId, ProductSaleCounts, ProductSaleTotals, ProductProfitsTotals)
	Exec sp_executesql @sqlPopulate

	SET @TotalProductSales = @@rowcount
	
	SELECT IDOfSaleTotals=(select count(1)+1 from #PageIndexForProductSales where ProductSaleCounts>s.ProductSaleCounts),
		P.ProductName,P.ProductCode as SKU,
		case when S.ProductSaleCounts is null then 0 else S.ProductSaleCounts end as ProductSaleCounts , 
		case when S.ProductSaleTotals is null then 0 else S.ProductSaleTotals end as ProductSaleTotals , 
		case when S.ProductProfitsTotals is null then 0 else S.ProductProfitsTotals end as ProductProfitsTotals
	FROM   Hishop_Products P, #PageIndexForProductSales S
	WHERE 
			P.ProductId = S.ProductId AND
			S.IndexId > @PageLowerBound AND
			S.IndexId < @PageUpperBound 
	ORDER BY S.IndexId
	
	drop table #PageIndexForProductSales
END

GO
/****** Object:  StoredProcedure [dbo].[cp_ProductSalesNoPage_Get]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_ProductSalesNoPage_Get]
(
	@sqlPopulate ntext,
	@TotalProductSales int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	CREATE TABLE #PageIndexForProductSales
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		ProductId int,
		ProductSaleCounts int,
		ProductSaleTotals money,
		ProductProfitsTotals money
	)	

	INSERT INTO #PageIndexForProductSales(ProductId, ProductSaleCounts, ProductSaleTotals, ProductProfitsTotals)
	Exec sp_executesql @sqlPopulate

	SET @TotalProductSales = @@rowcount
	
	SELECT IDOfSaleTotals=(select count(1)+1 from #PageIndexForProductSales where ProductSaleCounts>s.ProductSaleCounts),
		P.ProductName,P.ProductCode as SKU,
		case when S.ProductSaleCounts is null then 0 else S.ProductSaleCounts end as ProductSaleCounts , 
		case when S.ProductSaleTotals is null then 0 else S.ProductSaleTotals end as ProductSaleTotals , 
		case when S.ProductProfitsTotals is null then 0 else S.ProductProfitsTotals end as ProductProfitsTotals
	FROM   Hishop_Products P, #PageIndexForProductSales S
	WHERE 
			P.ProductId = S.ProductId
	ORDER BY S.IndexId
END

GO
/****** Object:  StoredProcedure [dbo].[cp_ProductVisitAndBuyStatistics_Get]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[cp_ProductVisitAndBuyStatistics_Get]
(
	@PageIndex int,
	@PageSize int,
	@sqlPopulate ntext,
	@TotalProductSales int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForProductSales
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		ProductId int,
		BuyPercentage decimal(18, 0)		
	)	

	INSERT INTO #PageIndexForProductSales(ProductId,BuyPercentage)
	Exec sp_executesql @sqlPopulate

	SET @TotalProductSales = @@rowcount
	
	SELECT S.IndexId,P.ProductName,P.VistiCounts,P.SaleCounts as BuyCount ,S.BuyPercentage    
	FROM   Hishop_Products P, #PageIndexForProductSales S
	WHERE 
			P.ProductId = S.ProductId AND
			S.IndexId > @PageLowerBound AND
			S.IndexId < @PageUpperBound 
	ORDER BY S.IndexId
	
	drop table #PageIndexForProductSales
END

GO
/****** Object:  StoredProcedure [dbo].[cp_RegionsUsers_Get]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[cp_RegionsUsers_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@TotalRegionsUsers int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForRegionsUsers
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		RegionId int,
		RegionName char(100),
		UserCounts int,
		AllUserCounts int
	)	

	INSERT INTO #PageIndexForRegionsUsers(RegionId, RegionName, UserCounts, AllUserCounts)
	Exec sp_executesql @sqlPopulate

	SET @TotalRegionsUsers = @@rowcount
	
	SELECT RU.RegionId, RU.RegionName, RU.UserCounts, RU.AllUserCounts
	FROM   #PageIndexForRegionsUsers RU
	WHERE 
			RU.IndexId > @PageLowerBound AND
			RU.IndexId < @PageUpperBound
	ORDER BY RU.IndexId
	
	drop table #PageIndexForRegionsUsers
END

GO
/****** Object:  StoredProcedure [dbo].[cp_SendCouponToMember]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--刘继先
--发送优惠劵给客户
CREATE PROCEDURE [dbo].[cp_SendCouponToMember]
	@CouponsId int,
	@UserId int,
	@Result int output
AS
BEGIN
	--判断是否可以领取此劵
	Set @Result=0;
	Exec cp_MemberCanReceiveCoupon @CouponsId,@UserId,@Result output;
	if @Result>0
	Begin
		return;
	End
	--把优惠劵写用用户优惠劵表
	Insert into Hishop_Coupon_MemberCoupons(CouponId,MemberId,ReceiveDate,[Status],CouponName,ConditionValue,BeginDate,EndDate,CouponValue)
	select CouponId,@UserId as MemberId,GETDATE() as ReceiveDate,0 as [Status], CouponName,ConditionValue,BeginDate,EndDate,CouponValue
	from Hishop_Coupon_Coupons where CouponId=@CouponsId;
	--修改优惠劵领取数量
	Update Hishop_Coupon_Coupons set ReceiveNum=ReceiveNum+1 where CouponId=@CouponsId;
END

GO
/****** Object:  StoredProcedure [dbo].[cp_SendCouponUser]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_SendCouponUser]
	@GradeIds varchar(50),
	@ReferralUserId varchar(20),
	@BeginCreateDate varchar(50),
	@EndCreateDate varchar(50),
	@UserType int,
	@CustomGroupIds varchar(50),
	@AdminId int,
	@Count int output
AS
BEGIN
	Declare @Where varchar(4000);
	Set @Where=' 1=1 ';
	if @ReferralUserId !=''
	Begin
		Set @Where=@Where+' And ReferralUserId='+@ReferralUserId
	End
	if @BeginCreateDate !=''
	Begin
		Set @Where=@Where+' And CreateDate >= '''+@BeginCreateDate+''' '
	End
	if @EndCreateDate !=''
	Begin
		Declare @TempD DateTime;
		Set @TempD = DATEADD(d,1,@EndCreateDate);
		set @EndCreateDate= Convert(varchar(10),@TempD,120);
		Set @Where=@Where+' And CreateDate < '''+@EndCreateDate+''' '
	End
	--删除上次临时存储表中的数据
	Delete From Hishop_TempSendCouponUserLists where AdminId=@AdminId;
	--上级店铺名，注册时间 查询出会员
	Declare @UserBySelect table(UserId int);
	Insert into @UserBySelect(UserId)
	Exec('select UserId from dbo.aspnet_Members where '+@Where);
	
	--查询出会员等级的会员
	Declare @TypeUser table(UserId int);
	Declare @dates int;
	Declare @SelectDate datetime;
	Declare @ActivyUser table(Userid int);--保存临时的活跃会员
	Declare @SleepUser table(UserId int);--保存临时的沉睡会员
	set @dates=0;
	if @UserType=1 --新会员
	Begin
		Insert into @TypeUser(UserId)
		Select u.UserId From @UserBySelect as u left join
		(SELECT UserId FROM  dbo.Hishop_Orders group by UserId) as v
		on u.Userid = v.UserId where v.UserId is null
	End
	if @UserType=2 --活跃会员
	Begin
		select top 1 @dates = -1*isnull(ActiveDay,1) from Hishop_UserGroupSet;
		set @SelectDate = DATEADD(d,@dates,Convert(varchar(10),getdate(),120));
		Insert into @TypeUser(UserId)
		Select u.UserId From @UserBySelect as u inner join
		(select a.UserId from vw_VShop_FinishOrder_Main as a 
		where PayDate>=@SelectDate group by UserId) as v
		on u.Userid = v.UserId;
	End
	if @UserType=3 --沉睡会员
	Begin
		--活跃会员
		select top 1 @dates = -1*isnull(ActiveDay,1) from Hishop_UserGroupSet;
		set @SelectDate = DATEADD(d,@dates,Convert(varchar(10),getdate(),120));
		Insert into @ActivyUser(Userid)
		select a.UserId from vw_VShop_FinishOrder_Main as a 
		where PayDate>=@SelectDate group by UserId;

		--沉睡会员
		with allSaleCustomer
		as(
		select a.UserId from vw_VShop_FinishOrder_Main as a group by UserId)
		Insert into @TypeUser(UserId)
		Select u.UserId From @UserBySelect as u inner join(
		select us.UserId from allSaleCustomer us left join @ActivyUser as au
		on us.UserId = au.Userid
		where au.Userid is null) as v
		on u.Userid = v.UserId;
	End
	if @UserType=12 --新会员与活跃会员
	Begin
		Insert into @TypeUser(UserId)
		Select u.UserId From @UserBySelect as u left join
		(SELECT UserId FROM  dbo.Hishop_Orders group by UserId) as v
		on u.Userid = v.UserId where v.UserId is null;
		
		select top 1 @dates = -1*isnull(ActiveDay,1) from Hishop_UserGroupSet;
		set @SelectDate = DATEADD(d,@dates,Convert(varchar(10),getdate(),120));
		Insert into @TypeUser(UserId)
		Select u.UserId From @UserBySelect as u inner join
		(select a.UserId from vw_VShop_FinishOrder_Main as a 
		where PayDate>=@SelectDate group by UserId) as v
		on u.Userid = v.UserId where v.UserId is null;
		
	End
	if @UserType=13 --新会员与沉睡会员
	Begin
		Insert into @TypeUser(UserId)
		Select u.UserId From @UserBySelect as u left join
		(SELECT UserId FROM  dbo.Hishop_Orders group by UserId) as v
		on u.Userid = v.UserId where v.UserId is null;
		
		--活跃会员
		select top 1 @dates = -1*isnull(ActiveDay,1) from Hishop_UserGroupSet;
		set @SelectDate = DATEADD(d,@dates,Convert(varchar(10),getdate(),120));
		Insert into @ActivyUser(Userid)
		select a.UserId from vw_VShop_FinishOrder_Main as a 
		where PayDate>=@SelectDate group by UserId;

		--沉睡会员
		with allSaleCustomer
		as(
		select a.UserId from vw_VShop_FinishOrder_Main as a group by UserId)
		Insert into @TypeUser(UserId)
		Select u.UserId From @UserBySelect as u inner join(
		select us.UserId from allSaleCustomer us left join @ActivyUser as au
		on us.UserId = au.Userid
		where au.Userid is null) as v
		on u.Userid = v.UserId;
		
	End
	if @UserType=23 --活跃会员与沉睡会员
	Begin
		--活跃会员
		select top 1 @dates = -1*isnull(ActiveDay,1) from Hishop_UserGroupSet;
		set @SelectDate = DATEADD(d,@dates,Convert(varchar(10),getdate(),120));
		Insert into @ActivyUser(Userid)
		select a.UserId from vw_VShop_FinishOrder_Main as a 
		where PayDate>=@SelectDate group by UserId;
		Insert into @TypeUser(UserId)
		Select u.UserId From @UserBySelect as u inner join
		@ActivyUser as v
		on u.Userid = v.UserId;
		
		--沉睡会员
		with allSaleCustomer
		as(
		select a.UserId from vw_VShop_FinishOrder_Main as a group by UserId)
		Insert into @TypeUser(UserId)
		Select u.UserId From @UserBySelect as u inner join(
		select us.UserId from allSaleCustomer us left join @ActivyUser as au
		on us.UserId = au.Userid
		where au.Userid is null) as v
		on u.Userid = v.UserId;
	End
	
	--查询出自定义分组的会员
	Declare @CustomGroupUser table(UserId int);
	if @CustomGroupIds !='0'--自定义分组
	Begin
		Declare @GroupUser table(UserId int);
		Insert into @GroupUser
		Exec('select UserId from dbo.Vshop_CustomGroupingUser where GroupId in('+@CustomGroupIds+')');

		Insert into @CustomGroupUser(UserId)
		Select u.UserId From @UserBySelect as u inner join
		(SELECT UserId FROM @GroupUser group by UserId) as v on u.Userid = v.UserId
	End

	if @GradeIds='0'
	Begin
		if @UserType=0
		Begin
			if @CustomGroupIds='0'
			Begin--所有会员等级、所有会员分组、未选择自定义分组
				Insert Into Hishop_TempSendCouponUserLists(UserId,AdminId)
				Select UserId,@AdminId From @UserBySelect;
			End
			else
			Begin--所有会员等级、所有会员分组、自定义分组
				Insert Into Hishop_TempSendCouponUserLists(UserId,AdminId)
				Select UserId,@AdminId From @CustomGroupUser;
			End
		End
		else
		Begin
			if @CustomGroupIds='0'
			Begin--所有会员等级、会员分组、未选择自定义分组
				Insert Into Hishop_TempSendCouponUserLists(UserId,AdminId)
				Select UserId,@AdminId From @TypeUser;
			End
			else
			Begin--所有会员等级、会员分组、自定义分组
				Insert Into Hishop_TempSendCouponUserLists(UserId,AdminId)
				select UserId,@AdminId from(
				Select UserId From @TypeUser
				union
				Select UserId From @CustomGroupUser) as v group by v.UserId;
			End
		End
	End
	Else
	Begin--不是所有会员等级
		Declare @GradeUser table(UserId int);
		Insert into @GradeUser
		Exec('select UserId from dbo.aspnet_Members where '+@Where+' and GradeId in('+@GradeIds+')');
		if @UserType=0 --所有会员分组
		Begin
			if @CustomGroupIds='0'
			Begin--会员等级、所有会员分组、未选择自定义分组
				Insert Into Hishop_TempSendCouponUserLists(UserId,AdminId)
				Select UserId,@AdminId From @GradeUser group by UserId;
			End
			else
			Begin--会员等级、所有会员分组、自定义分组
				Insert Into Hishop_TempSendCouponUserLists(UserId,AdminId)
				select UserId,@AdminId from(
				Select UserId From @GradeUser
				union
				Select UserId From @CustomGroupUser) as v group by v.UserId;
			End
		End
		else
		Begin
			if @CustomGroupIds='0'
			Begin--会员等级、会员分组、未选择自定义分组
				Insert Into Hishop_TempSendCouponUserLists(UserId,AdminId)
				select UserId,@AdminId from(
				Select UserId From @GradeUser
				union
				Select UserId From @TypeUser) as v group by v.UserId;
			End
			else
			Begin--会员等级、会员分组、自定义分组
				Insert Into Hishop_TempSendCouponUserLists(UserId,AdminId)
				select UserId,@AdminId from(
				Select UserId From @GradeUser
				union
				Select UserId From @TypeUser
				union
				Select UserId From @CustomGroupUser) as v group by v.UserId;
			End
		End
	End

	Select @Count=COUNT(*) From Hishop_TempSendCouponUserLists where AdminId=@AdminId;
END

GO
/****** Object:  StoredProcedure [dbo].[cp_ShippingMode_Create]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[cp_ShippingMode_Create]
(
 @ModeId int output,
 @Name nvarchar(100),
 @TemplateId int,	
 @Description nvarchar(4000)=null,
 @Status int Output
)
as 
DECLARE @DisplaySequence INT
SET @Status = 99
BEGIN TRAN
--如果取最大序号为空 则直接将序号设置为1
   IF  (Select Max(DisplaySequence) From Hishop_ShippingTypes) IS NUll
      SET @DisplaySequence = 1
   -- 如果不为空则将 将序号设置为表中现有的最大值加1
   ELSE
      SET @DisplaySequence = (Select Max(DisplaySequence) From Hishop_ShippingTypes) + 1
 INSERT INTO Hishop_ShippingTypes ([Name],TemplateId,[Description],DisplaySequence)
 VALUES (@Name,@TemplateId,@Description,@DisplaySequence)
  SET @ModeId = @@IDENTITY
     IF @@ROWCOUNT = 1
	   SET @Status = 0
	 
COMMIT

GO
/****** Object:  StoredProcedure [dbo].[cp_ShippingMode_Update]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[cp_ShippingMode_Update]
(
 @ModeId INT =null,
 @Name nvarchar(100),
 @TemplateId INT,
 @Description nvarchar(4000)=null,
 @Status INT Output
)
as 
DECLARE @DisplaySequence INT
DECLARE @intErrorCode INT
	SELECT @Status = 99, @intErrorCode = 0
BEGIN TRAN
  --直接取出原来的序号   
   SET @DisplaySequence = (Select DisplaySequence  From Hishop_ShippingTypes where ModeId=@ModeId)
     
   Update Hishop_ShippingTypes 
   SET [Name]=@Name,TemplateId=@TemplateId , Description =@Description
   Where ModeId=@ModeId
   SET @intErrorCode = @intErrorCode + @@ERROR
      
   IF @intErrorCode = 0
    BEGIN
       DELETE FROM Hishop_TemplateRelatedShipping Where ModeId=@ModeId
   END
  IF @intErrorCode = 0
	BEGIN
		COMMIT TRAN
		SET @Status = 0
	END
	ELSE
		ROLLBACK TRAN

GO
/****** Object:  StoredProcedure [dbo].[cp_Votes_Create]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cp_Votes_Create]
	(
		@VoteName NVARCHAR(100),
		@IsBackup BIT,
		@MaxCheck INT,
		@ImageUrl NVARCHAR(255),
		@StartDate DateTime,
		@EndDate DateTime,
		@Keys NVARCHAR(200),
		@VoteId int OUTPUT
	)
AS

IF @IsBackup = 1
	BEGIN
		UPDATE Hishop_Votes SET IsBackup = 0
	END

INSERT INTO Hishop_Votes (VoteName, IsBackup, MaxCheck, ImageUrl, StartDate, EndDate)
 VALUES (@VoteName, @IsBackup, @MaxCheck, @ImageUrl, @StartDate, @EndDate)
SET @VoteId = @@IDENTITY ;

GO
/****** Object:  StoredProcedure [dbo].[Hishop_InsuranceOrder_ADD]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------
--用途：增加一条记录 
--项目名称：
--说明：
--时间：2016/12/18 16:15:50
------------------------------------
CREATE PROCEDURE [dbo].[Hishop_InsuranceOrder_ADD]
@InsuranceOrderId int output,
@InsuranceOrderCity1 int,
@InsuranceOrderCity1Name nvarchar(50),
@InsuranceOrderCity2 int,
@InsuranceOrderCity2Name nvarchar(50),
@InsuranceOrderCompany int,
@InsuranceOrderCompany_Name nvarchar(100),
@InsuranceOrderJiaoQiangXian nvarchar(50),
@InsuranceOrderShangYeSanXian nvarchar(50),
@InsuranceOrderShangYeSanXianEr nvarchar(50),
@InsuranceOrderCheShangRenYuanSiJiXian nvarchar(50),
@InsuranceOrderCheShangRenYuanSiJiXianEr nvarchar(50),
@InsuranceOrderCheShangRenYuanChengKeXian nvarchar(50),
@InsuranceOrderCheShangRenYuanChengKeXianEr nvarchar(50),
@InsuranceOrderCheLiangSunShiXian nvarchar(50),
@InsuranceOrderDaoQiangXian nvarchar(50),
@InsuranceOrderBoliXian nvarchar(50),
@InsuranceOrderZiRanXian nvarchar(50),
@InsuranceOrderSheShuiXian nvarchar(50),
@InsuranceOrderTeYueXian nvarchar(50),
@InsuranceOrderSanFangZenRenXian nvarchar(50),
@InsuranceOrderHuaHenXian nvarchar(50),
@InsuranceOrderCarType int,
@InsuranceOrderJiaoZhaoZhengBen nvarchar(50),
@InsuranceOrderJiaoZhaoFuBen nvarchar(50),
@InsuranceOrderName nvarchar(50),
@InsuranceOrderTel nvarchar(50),
@InsuranceOrderCardZheng nvarchar(50),
@InsuranceOrderCardFan nvarchar(50),
@InsuranceOrderAddres nvarchar(200),
@InsuranceOrderShouJIanName nvarchar(50),
@InsuranceOrderShouJIanTel nvarchar(50),
@InsuranceOrderStatu int,
@InsuranceOrderAmount decimal(18,2),
@InsuranceOrderNotes nvarchar(1000),
@InsuranceUserId int,
@InsuranceOrderOpenId nvarchar(100)

 AS 
	INSERT INTO [Hishop_InsuranceOrder](
	[InsuranceOrderCity1],[InsuranceOrderCity1Name],[InsuranceOrderCity2],[InsuranceOrderCity2Name],[InsuranceOrderCompany],[InsuranceOrderCompany_Name],[InsuranceOrderJiaoQiangXian],[InsuranceOrderShangYeSanXian],[InsuranceOrderShangYeSanXianEr],[InsuranceOrderCheShangRenYuanSiJiXian],[InsuranceOrderCheShangRenYuanSiJiXianEr],[InsuranceOrderCheShangRenYuanChengKeXian],[InsuranceOrderCheShangRenYuanChengKeXianEr],[InsuranceOrderCheLiangSunShiXian],[InsuranceOrderDaoQiangXian],[InsuranceOrderBoliXian],[InsuranceOrderZiRanXian],[InsuranceOrderSheShuiXian],[InsuranceOrderTeYueXian],[InsuranceOrderSanFangZenRenXian],[InsuranceOrderHuaHenXian],[InsuranceOrderCarType],[InsuranceOrderJiaoZhaoZhengBen],[InsuranceOrderJiaoZhaoFuBen],[InsuranceOrderName],[InsuranceOrderTel],[InsuranceOrderCardZheng],[InsuranceOrderCardFan],[InsuranceOrderAddres],[InsuranceOrderShouJIanName],[InsuranceOrderShouJIanTel],[InsuranceOrderStatu],[InsuranceOrderAmount],[InsuranceOrderNotes],[InsuranceUserId],[InsuranceOrderOpenId]
	)VALUES(
	@InsuranceOrderCity1,@InsuranceOrderCity1Name,@InsuranceOrderCity2,@InsuranceOrderCity2Name,@InsuranceOrderCompany,@InsuranceOrderCompany_Name,@InsuranceOrderJiaoQiangXian,@InsuranceOrderShangYeSanXian,@InsuranceOrderShangYeSanXianEr,@InsuranceOrderCheShangRenYuanSiJiXian,@InsuranceOrderCheShangRenYuanSiJiXianEr,@InsuranceOrderCheShangRenYuanChengKeXian,@InsuranceOrderCheShangRenYuanChengKeXianEr,@InsuranceOrderCheLiangSunShiXian,@InsuranceOrderDaoQiangXian,@InsuranceOrderBoliXian,@InsuranceOrderZiRanXian,@InsuranceOrderSheShuiXian,@InsuranceOrderTeYueXian,@InsuranceOrderSanFangZenRenXian,@InsuranceOrderHuaHenXian,@InsuranceOrderCarType,@InsuranceOrderJiaoZhaoZhengBen,@InsuranceOrderJiaoZhaoFuBen,@InsuranceOrderName,@InsuranceOrderTel,@InsuranceOrderCardZheng,@InsuranceOrderCardFan,@InsuranceOrderAddres,@InsuranceOrderShouJIanName,@InsuranceOrderShouJIanTel,@InsuranceOrderStatu,@InsuranceOrderAmount,@InsuranceOrderNotes,@InsuranceUserId,@InsuranceOrderOpenId
	)
	SET @InsuranceOrderId = @@IDENTITY


GO
/****** Object:  StoredProcedure [dbo].[Hishop_InsuranceOrder_Delete]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------
--用途：删除一条记录 
--项目名称：
--说明：
--时间：2016/12/18 16:15:50
------------------------------------
CREATE PROCEDURE [dbo].[Hishop_InsuranceOrder_Delete]
@InsuranceOrderId int
 AS 
	DELETE [Hishop_InsuranceOrder]
	 WHERE InsuranceOrderId=@InsuranceOrderId 


GO
/****** Object:  StoredProcedure [dbo].[Hishop_InsuranceOrder_Exists]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------
--用途：是否已经存在 
--项目名称：
--说明：
--时间：2016/12/18 16:15:50
------------------------------------
CREATE PROCEDURE [dbo].[Hishop_InsuranceOrder_Exists]
@InsuranceOrderId int
AS
	DECLARE @TempID int
	SELECT @TempID = count(1) FROM [Hishop_InsuranceOrder] WHERE InsuranceOrderId=@InsuranceOrderId 
	IF @TempID = 0
		RETURN 0
	ELSE
		RETURN 1


GO
/****** Object:  StoredProcedure [dbo].[Hishop_InsuranceOrder_GetList]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------
--用途：查询记录信息 
--项目名称：
--说明：
--时间：2016/12/18 16:15:50
------------------------------------
CREATE PROCEDURE [dbo].[Hishop_InsuranceOrder_GetList]
 AS 
	SELECT 
	InsuranceOrderId,InsuranceOrderCity1,InsuranceOrderCity1Name,InsuranceOrderCity2,InsuranceOrderCity2Name,InsuranceOrderCompany,InsuranceOrderCompany_Name,InsuranceOrderJiaoQiangXian,InsuranceOrderShangYeSanXian,InsuranceOrderShangYeSanXianEr,InsuranceOrderCheShangRenYuanSiJiXian,InsuranceOrderCheShangRenYuanSiJiXianEr,InsuranceOrderCheShangRenYuanChengKeXian,InsuranceOrderCheShangRenYuanChengKeXianEr,InsuranceOrderCheLiangSunShiXian,InsuranceOrderDaoQiangXian,InsuranceOrderBoliXian,InsuranceOrderZiRanXian,InsuranceOrderSheShuiXian,InsuranceOrderTeYueXian,InsuranceOrderSanFangZenRenXian,InsuranceOrderHuaHenXian,InsuranceOrderCarType,InsuranceOrderJiaoZhaoZhengBen,InsuranceOrderJiaoZhaoFuBen,InsuranceOrderName,InsuranceOrderTel,InsuranceOrderCardZheng,InsuranceOrderCardFan,InsuranceOrderAddres,InsuranceOrderShouJIanName,InsuranceOrderShouJIanTel,InsuranceOrderStatu,InsuranceOrderAmount,InsuranceOrderNotes,InsuranceUserId,InsuranceOrderOpenId
	 FROM [Hishop_InsuranceOrder]


GO
/****** Object:  StoredProcedure [dbo].[Hishop_InsuranceOrder_GetModel]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------
--用途：得到实体对象的详细信息 
--项目名称：
--说明：
--时间：2016/12/18 16:15:50
------------------------------------
CREATE PROCEDURE [dbo].[Hishop_InsuranceOrder_GetModel]
@InsuranceOrderId int
 AS 
	SELECT 
	InsuranceOrderId,InsuranceOrderCity1,InsuranceOrderCity1Name,InsuranceOrderCity2,InsuranceOrderCity2Name,InsuranceOrderCompany,InsuranceOrderCompany_Name,InsuranceOrderJiaoQiangXian,InsuranceOrderShangYeSanXian,InsuranceOrderShangYeSanXianEr,InsuranceOrderCheShangRenYuanSiJiXian,InsuranceOrderCheShangRenYuanSiJiXianEr,InsuranceOrderCheShangRenYuanChengKeXian,InsuranceOrderCheShangRenYuanChengKeXianEr,InsuranceOrderCheLiangSunShiXian,InsuranceOrderDaoQiangXian,InsuranceOrderBoliXian,InsuranceOrderZiRanXian,InsuranceOrderSheShuiXian,InsuranceOrderTeYueXian,InsuranceOrderSanFangZenRenXian,InsuranceOrderHuaHenXian,InsuranceOrderCarType,InsuranceOrderJiaoZhaoZhengBen,InsuranceOrderJiaoZhaoFuBen,InsuranceOrderName,InsuranceOrderTel,InsuranceOrderCardZheng,InsuranceOrderCardFan,InsuranceOrderAddres,InsuranceOrderShouJIanName,InsuranceOrderShouJIanTel,InsuranceOrderStatu,InsuranceOrderAmount,InsuranceOrderNotes,InsuranceUserId,InsuranceOrderOpenId
	 FROM [Hishop_InsuranceOrder]
	 WHERE InsuranceOrderId=@InsuranceOrderId 


GO
/****** Object:  StoredProcedure [dbo].[Hishop_InsuranceOrder_Update]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------
--用途：修改一条记录 
--项目名称：
--说明：
--时间：2016/12/18 16:15:50
------------------------------------
CREATE PROCEDURE [dbo].[Hishop_InsuranceOrder_Update]
@InsuranceOrderId int,
@InsuranceOrderCity1 int,
@InsuranceOrderCity1Name nvarchar(50),
@InsuranceOrderCity2 int,
@InsuranceOrderCity2Name nvarchar(50),
@InsuranceOrderCompany int,
@InsuranceOrderCompany_Name nvarchar(100),
@InsuranceOrderJiaoQiangXian nvarchar(50),
@InsuranceOrderShangYeSanXian nvarchar(50),
@InsuranceOrderShangYeSanXianEr nvarchar(50),
@InsuranceOrderCheShangRenYuanSiJiXian nvarchar(50),
@InsuranceOrderCheShangRenYuanSiJiXianEr nvarchar(50),
@InsuranceOrderCheShangRenYuanChengKeXian nvarchar(50),
@InsuranceOrderCheShangRenYuanChengKeXianEr nvarchar(50),
@InsuranceOrderCheLiangSunShiXian nvarchar(50),
@InsuranceOrderDaoQiangXian nvarchar(50),
@InsuranceOrderBoliXian nvarchar(50),
@InsuranceOrderZiRanXian nvarchar(50),
@InsuranceOrderSheShuiXian nvarchar(50),
@InsuranceOrderTeYueXian nvarchar(50),
@InsuranceOrderSanFangZenRenXian nvarchar(50),
@InsuranceOrderHuaHenXian nvarchar(50),
@InsuranceOrderCarType int,
@InsuranceOrderJiaoZhaoZhengBen nvarchar(50),
@InsuranceOrderJiaoZhaoFuBen nvarchar(50),
@InsuranceOrderName nvarchar(50),
@InsuranceOrderTel nvarchar(50),
@InsuranceOrderCardZheng nvarchar(50),
@InsuranceOrderCardFan nvarchar(50),
@InsuranceOrderAddres nvarchar(200),
@InsuranceOrderShouJIanName nvarchar(50),
@InsuranceOrderShouJIanTel nvarchar(50),
@InsuranceOrderStatu int,
@InsuranceOrderAmount decimal(18,2),
@InsuranceOrderNotes nvarchar(1000),
@InsuranceUserId int,
@InsuranceOrderOpenId nvarchar(100)
 AS 
	UPDATE [Hishop_InsuranceOrder] SET 
	[InsuranceOrderCity1] = @InsuranceOrderCity1,[InsuranceOrderCity1Name] = @InsuranceOrderCity1Name,[InsuranceOrderCity2] = @InsuranceOrderCity2,[InsuranceOrderCity2Name] = @InsuranceOrderCity2Name,[InsuranceOrderCompany] = @InsuranceOrderCompany,[InsuranceOrderCompany_Name] = @InsuranceOrderCompany_Name,[InsuranceOrderJiaoQiangXian] = @InsuranceOrderJiaoQiangXian,[InsuranceOrderShangYeSanXian] = @InsuranceOrderShangYeSanXian,[InsuranceOrderShangYeSanXianEr] = @InsuranceOrderShangYeSanXianEr,[InsuranceOrderCheShangRenYuanSiJiXian] = @InsuranceOrderCheShangRenYuanSiJiXian,[InsuranceOrderCheShangRenYuanSiJiXianEr] = @InsuranceOrderCheShangRenYuanSiJiXianEr,[InsuranceOrderCheShangRenYuanChengKeXian] = @InsuranceOrderCheShangRenYuanChengKeXian,[InsuranceOrderCheShangRenYuanChengKeXianEr] = @InsuranceOrderCheShangRenYuanChengKeXianEr,[InsuranceOrderCheLiangSunShiXian] = @InsuranceOrderCheLiangSunShiXian,[InsuranceOrderDaoQiangXian] = @InsuranceOrderDaoQiangXian,[InsuranceOrderBoliXian] = @InsuranceOrderBoliXian,[InsuranceOrderZiRanXian] = @InsuranceOrderZiRanXian,[InsuranceOrderSheShuiXian] = @InsuranceOrderSheShuiXian,[InsuranceOrderTeYueXian] = @InsuranceOrderTeYueXian,[InsuranceOrderSanFangZenRenXian] = @InsuranceOrderSanFangZenRenXian,[InsuranceOrderHuaHenXian] = @InsuranceOrderHuaHenXian,[InsuranceOrderCarType] = @InsuranceOrderCarType,[InsuranceOrderJiaoZhaoZhengBen] = @InsuranceOrderJiaoZhaoZhengBen,[InsuranceOrderJiaoZhaoFuBen] = @InsuranceOrderJiaoZhaoFuBen,[InsuranceOrderName] = @InsuranceOrderName,[InsuranceOrderTel] = @InsuranceOrderTel,[InsuranceOrderCardZheng] = @InsuranceOrderCardZheng,[InsuranceOrderCardFan] = @InsuranceOrderCardFan,[InsuranceOrderAddres] = @InsuranceOrderAddres,[InsuranceOrderShouJIanName] = @InsuranceOrderShouJIanName,[InsuranceOrderShouJIanTel] = @InsuranceOrderShouJIanTel,[InsuranceOrderStatu] = @InsuranceOrderStatu,[InsuranceOrderAmount] = @InsuranceOrderAmount,[InsuranceOrderNotes] = @InsuranceOrderNotes,[InsuranceUserId] = @InsuranceUserId,[InsuranceOrderOpenId] = @InsuranceOrderOpenId
	WHERE InsuranceOrderId=@InsuranceOrderId 


GO
/****** Object:  StoredProcedure [dbo].[sp_Statistics_Member]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_Statistics_Member]
as
begin
/*
exec sp_Statistics_Member

统计会员概况。本模块的会员，包含购买会员和分销商

统计原则：会员被删除时,会员数量不再计算，但产生的金额要计算		

*/
    --select 
    --(select COUNT(*) from  vw_BuyerMember where 1=1) as 'ActiveUserQty',  
    --(select COUNT(*) from  vw_BuyerMember where 1=1) as 'SleepUserQty',
    --(select COUNT(*) from  vw_BuyerMember where 1=1) as 'SuccessTradeUserQty',
    --(select COUNT(*) from  vw_BuyerMember where 1=1) as 'SuccessTradeUserQty_Yesterday',
    --(select COUNT(*) from  vw_BuyerMember where 1=1) as 'PotentialUserQty',
    --(select COUNT(*) from  vw_BuyerMember where 1=1) as 'PotentialUserQty_Yesterday',
    --(select COUNT(*) from  vw_BuyerMember where 1=1) as 'CollectUserQty',
    --(select COUNT(*) from  vw_BuyerMember where 1=1) as 'CartUserQty',
    --(select COUNT(*) from  aspnet_Members where 1=1) as 'MemberQty'
                    
        
     
        
    declare 
			@NewUserQty int, --新会员
			@ActiveUserQty int,
			@SleepUserQty int,
			
			@SuccessTradeUserQty int,  --总成交客户（不限时间）
			@SuccessTradeUserQty_Yesterday int	,
			
			@PotentialUserQty	int,  --潜在客户(不限时间)
			@PotentialUserQty_Yesterday int,
			
			@CollectUserQty int , --收藏会员数
			@CartUserQty int, --购物车会员数
			
			@MemberQty int  --总会员数
			
	select @ActiveUserQty=0, @SleepUserQty=0
                   

	declare @ActiveDay int,
			@StartTime datetime, @EndTime datetime
			


	--分析哪些是 新客户  的配置
	select top 1 @ActiveDay= ActiveDay  from Hishop_UserGroupSet 
	set @ActiveDay=ISNULL(@ActiveDay,1)			
	

	 
	 /*
	 注册以后没有购买过的会员
	 活跃会员：最近*天内有成交的会员
	 沉睡会员：最近*天内无成交的会员，但至少有过一次交易的
	 
	 */
	 
	--总会员
	select @MemberQty=COUNT(*) from aspnet_Members where Status=1
			
	--新会员
	select @NewUserQty=COUNT(*) from aspnet_Members
		where  LastOrderDate is null and   Status=1
	
	--成交会员(总)： 成功购买过的会员(不限时间，退款的不算)
	select @SuccessTradeUserQty = count(*)
	from 
	(
		select a.UserID from vw_VShop_FinishOrder_Main	 a
			inner join aspnet_Members b on a.UserId= b.UserId 
			where b.Status=1
			group by a.UserId			
	) T1
		
					
	--活跃会员
	select @ActiveUserQty=COUNT(*)
	from 
	(
	select a.UserId
		from vw_VShop_FinishOrder_Main	 a
		inner join aspnet_Members b on a.UserId= b.UserId 
		where b.Status=1
		and 
		(
		(CONVERT(varchar(10), PayDate + @ActiveDay , 120 ) >= CONVERT(varchar(10), GETDATE() , 120 )
		 and Gateway<>'hishop.plugins.payment.podrequest'
		 )
		 or(
		 Gateway='hishop.plugins.payment.podrequest' and
		 CONVERT(varchar(10), OrderDate+ @ActiveDay , 120 ) = CONVERT(varchar(10), GETDATE() , 120 )
		 )
		)
		group by a.UserId  
	) T1
	
	set @SleepUserQty=@SuccessTradeUserQty - @ActiveUserQty
	set @SleepUserQty=ISNULL(@SleepUserQty,0)
	 
	

	
	--昨日激活会员，即以前未有交易记录，昨天有交易记录的用户
    select @SuccessTradeUserQty_Yesterday=count(c1.UserId) from  (
    select distinct UserId from
		vw_VShop_FinishOrder_Main  where
		 (CONVERT(varchar(10), PayDate , 120 ) = CONVERT(varchar(10), GETDATE()-1 , 120 )
		 and Gateway<>'hishop.plugins.payment.podrequest'
		 )
		 or(
		 Gateway='hishop.plugins.payment.podrequest' and
		 CONVERT(varchar(10), OrderDate , 120 ) = CONVERT(varchar(10), GETDATE()-1 , 120 )
		 )
     ) as c1
    where 
    c1.UserId not in (select distinct UserId from
		vw_VShop_FinishOrder_Main  where
		 (CONVERT(varchar(10), PayDate , 120 ) < CONVERT(varchar(10), GETDATE()-1 , 120 )
		 and Gateway<>'hishop.plugins.payment.podrequest'
		 )
		 or(
		 Gateway='hishop.plugins.payment.podrequest' and
		 CONVERT(varchar(10), OrderDate , 120 ) < CONVERT(varchar(10), GETDATE()-1 , 120 )
		 )
		 )
    
  
	--总潜在客户(只注册没有任何交易的会员)
	select @PotentialUserQty = COUNT(*)
		from aspnet_Members 
		where PayOrderDate is null and (Status=1 or status=9) 
		
	
	--昨日潜在客户
    select @PotentialUserQty_Yesterday = COUNT(*) from aspnet_Members a
		where  CONVERT(varchar(10), CreateDate , 120 ) = CONVERT(varchar(10), GETDATE()-1 , 120 )
		and PayOrderDate is null and (Status=1 or status=9) and UserId not in(select UserId from
		vw_VShop_FinishOrder_Main  where
		 (CONVERT(varchar(10), PayDate , 120 ) >= CONVERT(varchar(10), GETDATE()-1 , 120 )
		 and Gateway<>'hishop.plugins.payment.podrequest'
		 )
		 or(
		 Gateway='hishop.plugins.payment.podrequest' and
		 CONVERT(varchar(10), OrderDate , 120 ) >= CONVERT(varchar(10), GETDATE()-1 , 120 )
		 )
		 )
		

	--收藏会员数
	--select  @CollectUserQty = COUNT(*) from (select UserId from Hishop_Favorite group by UserId ) T1
	select  @CollectUserQty = COUNT(*) from (
		select a.UserID from Hishop_Favorite	 a
			left join aspnet_Members b on a.UserId= b.UserId 
			where b.Status=1
			group by a.UserId	
	) T1	
	
			
	
	--购物车会员数
	--select @CartUserQty = COUNT(*)
	--from
	--(
	--	select userid
	--	from Hishop_ShoppingCarts
	--	group by UserId
	--) t1
	select @CartUserQty = COUNT(*)
	from
	(
		select a.UserID from Hishop_ShoppingCarts	 a
			left join aspnet_Members b on a.UserId= b.UserId 
			where b.Status=1
			group by a.UserId	
	) t1	
	
 
	
 
	
    select 
		@NewUserQty as 'NewUserQty', 
		@ActiveUserQty as 'ActiveUserQty',  
		@SleepUserQty as 'SleepUserQty',
		@SuccessTradeUserQty as 'SuccessTradeUserQty',
		@SuccessTradeUserQty_Yesterday as 'SuccessTradeUserQty_Yesterday',
		@PotentialUserQty as 'PotentialUserQty',
		@PotentialUserQty_Yesterday as 'PotentialUserQty_Yesterday',
		@CollectUserQty as 'CollectUserQty',
		@CartUserQty as 'CartUserQty',
		@MemberQty as 'MemberQty'
    	
	
 
        
end







GO
/****** Object:  StoredProcedure [dbo].[sp_vshop_Statistics_Auto]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_vshop_Statistics_Auto]
@RecDate datetime=null,
@RetCode int output,  --仅当返回 1 才表示核算成功
@RetInfo varchar(256) output
as
begin

/*
功能：统计每日订单情况，包括：
按天统计_店铺常规数据
按天统计_分销商数据
按天统计_商品数据

订单日期以付款日期为标准

exec [sp_vshop_Statistics_Auto] '2015-10-08', 0, '原因'

修改日期：2015-10-09  
修改：当日数据不统计

*/

	declare @DEF_IsShowDebugInfo int
	set @DEF_IsShowDebugInfo =-1

	select @RetCode = -1, @RetInfo ='原因未知。'

	if @RecDate is null
		set @RecDate= GETDATE()
		 
	
	--select @RecDate = CONVERT(varchar(10), @RecDate, 120 )	
	--select @RecDate  '@RecDate'
	
	 
--select 999
	
--insert into dbo.vshop_Statistics_Log(RecDate,FirstCountTime) 
--	values ('2015-01-01'	, getdate() )
		
--	waitfor delay '00:00:25'  
	

--insert into dbo.vshop_Statistics_Log(RecDate,FirstCountTime) 
--	values ('2015-01-02'	, getdate() )
--return

	begin try
			
			declare @i int
			set @i=30
			while @i>=0   --最多补30天的记录
			begin
				declare @CountDate datetime
				set @CountDate = CONVERT(varchar(10), @RecDate - @i , 120 )
				
				if 	 @CountDate>= CONVERT(varchar(10),  GETDATE()  , 120 )  --当日的也不统计
					break   -- 2015-10-09
									
				if not exists (  
						select top 1 * from vshop_Statistics_Log  
							where IsSuccess= 1	
							and  RecDate= @CountDate
							)
				begin
					print @CountDate
					exec [sp_vshop_Statistics_Daily]  @CountDate , 999,  1, --需要更新日志
						0, '原因'
				end
				 
				set @i=@i-1
			end
					


			select @RetCode =  1, @RetInfo ='核算成功。'
	 
	end try
	BEGIN CATCH 
 
			DECLARE @ErrorMessage NVARCHAR(4000); 
			DECLARE @ErrorSeverity INT; 
			DECLARE @ErrorState INT; 

			SELECT 
			@ErrorMessage = ERROR_MESSAGE(), 
			@ErrorSeverity = ERROR_SEVERITY(), 
			@ErrorState = ERROR_STATE(); 

			--RAISERROR (@ErrorMessage, -- Message text. 
			--@ErrorSeverity, -- Severity. 
			--@ErrorState -- State. 
			--); 
			select @ErrorState '@ErrorState'
		 
	 
			
			select @RetCode =  -1, @RetInfo ='核算出错。'
	END CATCH; 
end

GO
/****** Object:  StoredProcedure [dbo].[sp_vshop_Statistics_Daily]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_vshop_Statistics_Daily]
@RecDate datetime=null,
@FuncAction int =999,
@IsUpdateLog int=1,  --是否需要更新日志 2015-10-12

@RetCode int output,  --仅当返回 1 才表示核算成功
@RetInfo varchar(256) output
as
begin

/*
@FuncAction取值:
  OrderUpdate = 100,
  CommissionsDraw=101,
  MemberUpdate = 200,
  ProductUpdate = 300,
  AllUpdate = 999
  
  
功能：统计每日订单情况，包括：
按天统计_店铺常规数据
按天统计_分销商数据
按天统计_商品数据

订单日期以付款日期为标准

调用方法
declare @RetCode int 
declare  @RetInfo varchar(256) 
exec sp_vshop_Statistics_Daily '2015-10-28', 999, 0, @RetCode,@RetInfo

sp_refreshview vw_VShop_FinishOrder_Detail

逻辑删除会员
UPDATE  aspnet_Members SET Status=7 WHERE UserId = @UserId

*/
	declare @DEF_IsShowDebugInfo int  --是否显示调试数据， 1=是  其它=否
	set @DEF_IsShowDebugInfo = -1

	select @RetCode = -1, @RetInfo ='原因未知。', @FuncAction=isnull(@FuncAction,999)

	if @RecDate is null
		set @RecDate= GETDATE()

	select @RecDate = CONVERT(varchar(10), @RecDate, 120 )


	declare @IsFirstCount int,  --是否第1次运行
			@ErrorStep varchar(256)
	set @IsFirstCount =0
	set @ErrorStep =''
	
	set @IsUpdateLog =ISNULL(@IsUpdateLog,1)
	
	if @IsUpdateLog=1
	begin
		if exists(Select top 1 * from vshop_Statistics_Log where RecDate= @RecDate )
		begin
			Update vshop_Statistics_Log
				set BeginTime= GETDATE(), EndTime=null, IsSuccess= -1, ErrorStep='', LogInfo=''
				where  RecDate= @RecDate
		end
		else
		begin
			insert into vshop_Statistics_Log (
				RecDate, FirstCountTime, BeginTime, EndTime, IsSuccess, ErrorStep, LogInfo
				)
				values (
				@RecDate, GETDATE(),	GETDATE(),  null,	 0,			'',			  ''
				)
			set @IsFirstCount =1
		end
	end
	
	
/*			

等待买家付款=1
等待发货=2
已发货=3
成功订单=5
已关闭=4(包括退款退货的)
退款中=6
已退款=9
已退货=10
*/	
	
	declare @DEF_Status_RefundFee int,  --退款成功的状态定义
			@DEF_Status_Finish int  --交易成功的状态定义
	select @DEF_Status_RefundFee =3,
			@DEF_Status_Finish=5
			
	/*
	根据8.29沟通结果：
	1、子表全部退款成功，主表才变更为成功
	2、子表部分退款成功，主表仍是成功订单
	3、退款中的订单，状态不变
	4、主订单设置为退款成功，子订单状态也同步成已退款
	*/
		 
	
	begin try
			if @FuncAction in ( 100, 101, 999,99 )
			begin			
					set @ErrorStep ='按天统计_店铺常规数据'
			
					if ( @FuncAction=99 or @FuncAction=999)  and @IsUpdateLog=1
						Update vshop_Statistics_Log
							set  ErrorStep= @ErrorStep
							where  RecDate= @RecDate			
					/*****************************************************
					按天统计_店铺常规数据 vshop_Statistics_Globals
					*****************************************************/
					if exists(Select top 1 * from vshop_Statistics_Globals where RecDate= @RecDate )
						delete from  vshop_Statistics_Globals where RecDate= @RecDate
					 

					declare @OrderNumber int,	
							@SaleAmountFee money,
							@BuyerNumber int,
							@UserAvgPrice money,
							
							@FXOrderNumber int,
							@FXSaleAmountFee money,
							@FXResultPercent numeric(19,3),
							@CommissionFee  money,
							@FinishedDrawCommissionFee  money,  --已完成提现
							@WaitDrawCommissionFee  money,		--待提现
							
							@NewMemberNumber int,
							@NewAgentNumber int
							 
						
					select 	@OrderNumber =0,	@SaleAmountFee =0,	@BuyerNumber =0,@UserAvgPrice =0,
							@FXOrderNumber=0,	@FXSaleAmountFee=0, @FXResultPercent=0,@CommissionFee=0,
							@FinishedDrawCommissionFee=0,			@WaitDrawCommissionFee=0,
							
							@NewMemberNumber=0, @NewAgentNumber =0

					--所有订单数( 要考虑主从表退款标识不一样的情形)	、 交易额度(不计退款的)
					select @OrderNumber = COUNT(*) , @SaleAmountFee = SUM (  ValidOrderTotal)
						from  vw_VShop_FinishOrder_Main
						where    
						(OrderStatus<>1 and CONVERT(varchar(10), PayDate , 120 ) = @RecDate	 and Gateway <> 'hishop.plugins.payment.podrequest')
							or
							( CONVERT(varchar(10), OrderDate , 120 ) = @RecDate	 and Gateway = 'hishop.plugins.payment.podrequest')
					  
						
					--购买者人数(不计退款的，不计整个订单都退款了的)
					select @BuyerNumber =  count(*)
					from 
					(
						select  UserName  -- @BuyerNumber =  count(*)  
							from vw_VShop_FinishOrder_Main  a
							where 
							(OrderStatus<>1 and CONVERT(varchar(10), PayDate , 120 ) = @RecDate	 and Gateway <> 'hishop.plugins.payment.podrequest')
							or
							( CONVERT(varchar(10), OrderDate , 120 ) = @RecDate	 and Gateway = 'hishop.plugins.payment.podrequest')
							group by a.Username 
					) T1
					
					--客单价
					set @UserAvgPrice=0
					if @BuyerNumber>0
						set @UserAvgPrice =( @SaleAmountFee * 1.0 / @BuyerNumber )

                    --分销商订单数
					select @FXOrderNumber = COUNT(*) 
						from vw_VShop_FinishOrder_Main a
						where ReferralUserId>0 and(
						    (OrderStatus<>1 and CONVERT(varchar(10), PayDate , 120 ) = @RecDate	 and Gateway <> 'hishop.plugins.payment.podrequest')
							or
							( CONVERT(varchar(10), OrderDate , 120 ) = @RecDate	 and Gateway = 'hishop.plugins.payment.podrequest')
							)
				
 				
						
					--分销商：交易额度(不计退款的)
					select @FXSaleAmountFee =  SUM( ValidOrderTotal ) 
						from vw_VShop_FinishOrder_Main a
						where ReferralUserId>0 and(
						    (OrderStatus<>1 and CONVERT(varchar(10), PayDate , 120 ) = @RecDate	 and Gateway <> 'hishop.plugins.payment.podrequest')
							or
							( CONVERT(varchar(10), OrderDate , 120 ) = @RecDate	 and Gateway = 'hishop.plugins.payment.podrequest')
							)		

					--分销业绩占比：分销金额/交易额的百分比
					set @FXResultPercent=0
					if @FXSaleAmountFee>0
						set @FXResultPercent = @FXSaleAmountFee  * 1.0 / @SaleAmountFee   * 100
					set @FXSaleAmountFee = ISNULL(@FXSaleAmountFee,0)
						
					
					select @CommissionFee = SUM(ISNULL(SumCommission,0))
						from vw_VShop_FinishOrder_Main a
						where ReferralUserId>0 and(
						    (OrderStatus<>1 and CONVERT(varchar(10), PayDate , 120 ) = @RecDate	 and Gateway <> 'hishop.plugins.payment.podrequest')
							or
							( CONVERT(varchar(10), OrderDate , 120 ) = @RecDate	 and Gateway = 'hishop.plugins.payment.podrequest')
							)							
					set @CommissionFee = ISNULL(@CommissionFee,0)	
					
					--已提现及待提现				
					SELECT @FinishedDrawCommissionFee= SUM(Amount) from Hishop_BalanceDrawRequest   
						where isnull(IsCheck,0)=2 and CONVERT( varchar(10), CheckTime, 120) =  CONVERT( varchar(10), @RecDate, 120)
					SELECT @WaitDrawCommissionFee=SUM(Amount) from Hishop_BalanceDrawRequest   
						where isnull(IsCheck,0) in(0,1) and CONVERT( varchar(10), RequestTime, 120) =  CONVERT( varchar(10), @RecDate, 120)
						
				 
					--新增会员数量(未开店的会员)
					select @NewMemberNumber = COUNT(*)
						from aspnet_Members a
						left join aspnet_Distributors D on a.UserId= D.UserId
						where  CONVERT(varchar(10), CreateDate , 120 ) = @RecDate	
						--and D.UserId is null
						--and Status<>7  --7为逻辑删除
					
					--新增分销商数量
					select @NewAgentNumber = COUNT(*)
						from aspnet_Distributors
						where  CONVERT(varchar(10), CreateTime , 120 ) = @RecDate
						
					--插入数据库 vshop_Statistics_Globals 
					insert into vshop_Statistics_Globals (
						RecDate, OrderNumber, SaleAmountFee, BuyerNumber, UserAvgPrice, 
						FXOrderNumber, FXSaleAmountFee, FXResultPercent, CommissionFee, NewMemberNumber, NewAgentNumber
						)
						values (
						@RecDate, @OrderNumber, @SaleAmountFee, @BuyerNumber, @UserAvgPrice, 
						@FXOrderNumber, @FXSaleAmountFee, @FXResultPercent, @CommissionFee, @NewMemberNumber, @NewAgentNumber
						)
			end  --100				
			
			select 	@OrderNumber =0,	@SaleAmountFee =0,	@BuyerNumber =0,@UserAvgPrice =0,
					@FXOrderNumber=0,	@FXSaleAmountFee=0, @FXResultPercent=0,@CommissionFee=0,
					
					@NewMemberNumber=0, @NewAgentNumber =0
							
			

			--所有有成功购买的商品列表
			declare  @ProductList_Distribut   table
			(
				ProductId int,
				SaleQty int,
				SaleAmountFee money 
			) 		
 			insert into @ProductList_Distribut( ProductId  , SaleQty , SaleAmountFee  )
				select ProductId , count(*) ,  SUM( b.ItemListPrice * b.ShipmentQuantity )
					from vw_VShop_FinishOrder_Detail b
					where 
					(OrderStatus<>1 and CONVERT(varchar(10), PayDate , 120 ) = @RecDate	 and Gateway <> 'hishop.plugins.payment.podrequest')
					or
					( CONVERT(varchar(10), OrderDate , 120 ) = @RecDate	 and Gateway = 'hishop.plugins.payment.podrequest')	
					group by b.ProductId	
					
				
	
			--200、300、999需用到的临时表
			--多个过程均需用到的公用临时表
			--各个产品购买人数
			declare  @ProductListGroupByUser_Distribut   table
			(
				ProductId int,
				BuyerNumber int
			)
 

			insert into @ProductListGroupByUser_Distribut( ProductId  , BuyerNumber )
				select T1.ProductId ,  count( *)
				from 
				(
				select ProductId ,  count( b.UserId) as UserID_Count
					from vw_VShop_FinishOrder_Detail b
					where
					(OrderStatus<>1 and CONVERT(varchar(10), PayDate , 120 ) = @RecDate	 and Gateway <> 'hishop.plugins.payment.podrequest')
					or
					( CONVERT(varchar(10), OrderDate , 120 ) = @RecDate	 and Gateway = 'hishop.plugins.payment.podrequest')	
					
					group by ProductId, UserId
				) T1
				group by T1.ProductId 
					
			
			/*****************************************************
			按天统计_分销商数据 vshop_Statistics_Distributors
			*****************************************************/	
			/*		
			AgentID	分销商ID
			OrderNumber	订单数
			SaleAmountFee	销售额
			BuyerNumber	成交用户数
			BuyerAvgPrice	客单价
			CommissionAmount	佣金收入
			*/	
			if @FuncAction in ( 200,999,99 )
			begin			
			
					set @ErrorStep ='按天统计_分销商数据'
					
					if ( @FuncAction=99 or @FuncAction=999)  and @IsUpdateLog=1		
						Update vshop_Statistics_Log
							set  ErrorStep= @ErrorStep
							where  RecDate= @RecDate
												
					--2.按天统计_分销商数据 vshop_Statistics_Distributors
					if exists(Select top 1 * from vshop_Statistics_Distributors where RecDate= @RecDate )
						delete from  vshop_Statistics_Distributors where RecDate= @RecDate			
					
					--所有经销商的订单号列表
					declare  @OrderList_Distribut   table
					(
						OrderId nvarchar(50)
					)
					insert into @OrderList_Distribut( OrderId )
                            select a.OrderId
							from Hishop_Orders a
							  
								where 
								a.ReferralUserId>0 and 
								(
								(a.OrderStatus<>1 and CONVERT(varchar(10), a.PayDate , 120 ) = @RecDate	 and a.Gateway <> 'hishop.plugins.payment.podrequest')
								or(CONVERT(varchar(10),  a.OrderDate  , 120 ) =  @RecDate	and Gateway ='hishop.plugins.payment.podrequest')
								)
							group by a.OrderId
 
 
					--------------查询经销商本身店铺数据到临时表 #BuffTable 
					if object_id('tempdb..#BuffTable') is not null Begin
						drop table #BuffTable
					End		
						
						
						--//计算佣金静态表数据
					select @RecDate as RecDate , a.ReferralUserId  ,  b.OrderId,
						COUNT(b.OrderId) as OrderNumber_Item ,
						SUM( a.OrderTotal) as OrderTotal,
						sum(isnull(r.RefundMoney,0) ) 'RefundMoney', 
						SUM(a.OrderTotal) as  'ValidOrderTotal',
						a.Username,
						SUM( b.ItemsCommission) 'ItemsCommission' 
					 into #BuffTable
						from Hishop_OrderItems b
						inner join Hishop_Orders a on a.OrderId= b.OrderId 
						inner join   --一个订单可能有多次退款记录
						(
							Select SUM(RefundMoney) as RefundMoney, OrderID as OrderID_Refund
							from Hishop_OrderReturns 
							where HandleStatus=2 					
							group by OrderId
						) r  on r.OrderID_Refund= a.OrderId
						where 
						a.OrderStatus<>4 and a.ReferralUserId>0 and 
						(
						(a.OrderStatus <> 1 and a.Gateway <> 'hishop.plugins.payment.podrequest'  and CONVERT(varchar(10), a.PayDate  , 120 ) = @RecDate	)
						 or(a.Gateway = 'hishop.plugins.payment.podrequest'  and CONVERT(varchar(10), a.OrderDate  , 120 ) = @RecDate	)
						)
						group by ReferralUserId  ,  b.OrderId , a.Username
					
					if @DEF_IsShowDebugInfo	=1
					begin
						select * from #BuffTable
					end

		  
					------------------- 保存数据临时表 #tb_DiffOrderNumber_ValidOrderTotal 
					--订单数、销售额：不同经销商的不同订单数量
					if object_id('tempdb..#tb_DiffOrderNumber_ValidOrderTotal') is not null Begin
						drop table #tb_DiffOrderNumber_ValidOrderTotal
					End		
						
					select ReferralUserId, 
						COUNT(*) as DiffOrderNumber ,			--不同订单数
						sum(  ValidOrderTotal ) as ValidOrderTotal  --销售额(已减去退款)
						into #tb_DiffOrderNumber_ValidOrderTotal
						from  vw_VShop_FinishOrder_Main a
						where 
						(a.Gateway <>'hishop.plugins.payment.podrequest' and CONVERT(varchar(10), a.PayDate  , 120) = @RecDate)	
						or(CONVERT(varchar(10), a.OrderDate  , 120 ) = @RecDate and a.Gateway ='hishop.plugins.payment.podrequest')
						group by a.ReferralUserId  --, OrderId
						
					if @DEF_IsShowDebugInfo	=1
					begin		
						select '订单数、销售额：不同经销商的不同订单数量'
						select * from #tb_DiffOrderNumber_ValidOrderTotal
					end
					
					
					------------------- 保存数据临时表 #tb_DiffUserNumber 	
					 --成交用户数：不同经销商的不同用户数量，不能用上述 的分组方法，
					if object_id('tempdb..#tb_DiffUserNumber') is not null Begin
						drop table #tb_DiffUserNumber
					End		
							 
					--select '成交用户数: 不同经销商的不同用户数量'
					select   ReferralUserId, COUNT(*) as DiffUserNumber 
					into #tb_DiffUserNumber
					from
					(
					select   a.ReferralUserId, a.UserId -- COUNT(*) as DiffUserNumber 
						from vw_VShop_FinishOrder_Main a
						where 
						(CONVERT(varchar(10), a.PayDate  , 120 ) =@RecDate and Gateway<>'hishop.plugins.payment.podrequest')
						or (Gateway='hishop.plugins.payment.podrequest' and CONVERT(varchar(10), a.OrderDate  , 120 ) = @RecDate)	
						group by a.ReferralUserId,UserId
					) T1
					group by ReferralUserId 
		 
		  
					if @DEF_IsShowDebugInfo	=1
					begin
						select '成交用户数: 不同经销商的不同用户数量'
						select * from #tb_DiffUserNumber
					end
					
					 
						 
					--客单价(后面做除法运算)：		
					------------------- 保存数据临时表 #tb_DiffUserNumber 	
					--用户获取的佣金表（自己的和下属贡献的）
					if object_id('tempdb..#Commissions') is not null Begin
						drop table #Commissions
					End		
				 
						select UserID as  ParentUserID,  --ReferralUserId, 
							SUM( CommTotal) as YJAmount 
						into #Commissions
						from  Hishop_Commissions
						where OrderId in (Select OrderId from @OrderList_Distribut )
						group by UserId --, ReferralUserId
						
					if @DEF_IsShowDebugInfo	=1
					begin		
						select '#Commissions' '佣金表'
						select * from #Commissions
					end
					
					/*	Hishop_Commissions 表
						记录了给自己的佣金、给上级的佣金  UserID是受益人ID（包括了给自己的） ， Ref..ID是贡献人	
					*/


					--------------组合数据
					if object_id('tempdb..#Distributors_01') is not null Begin
						drop table #Distributors_01
					End			
					
					select top 1 * into #Distributors_01
						from vshop_Statistics_Distributors
						where 1=2
				 
				 
					if @DEF_IsShowDebugInfo	=1
					begin 
						select '组合数据'  --  最终经销商数据从 #Distributors_01 中取得
					end
					 
					insert into #Distributors_01 (
						RecDate, AgentID, OrderNumber, SaleAmountFee, BuyerNumber, BuyerAvgPrice, 
						CommissionAmountFee
						)
						select  @RecDate as RecDate,  a.ReferralUserId, a.DiffOrderNumber, a.ValidOrderTotal,
						   b.DiffUserNumber, 0 as BuyerAvgPrice ,  isnull(c.YJAmount,0)
						from #tb_DiffOrderNumber_ValidOrderTotal a
						left join #tb_DiffUserNumber b on a.ReferralUserId= b.ReferralUserId
						left join #Commissions c on a.ReferralUserId= c.ParentUserID
						union
						select  @RecDate as RecDate,  c.ParentUserID, 0, 0,  --相当于获取某些用户没有订单，但有下属贡献的佣金
						   0, 0 as BuyerAvgPrice , c.YJAmount
						from #tb_DiffOrderNumber_ValidOrderTotal a
						right join #Commissions c on a.ReferralUserId= c.ParentUserID
						where a.ReferralUserId is   null


					--核算客单价
					Update #Distributors_01
						set BuyerAvgPrice =
							case 
								when isnull(BuyerNumber,0) =0 then 0
								else SaleAmountFee * 1.0 / BuyerNumber
							end
						where 1=1
					 

					if @DEF_IsShowDebugInfo	=1
					begin
						select * from #Distributors_01 	where RecDate = @RecDate
						select * from @OrderList_Distribut
						select * from @ProductList_Distribut
						select * from @ProductListGroupByUser_Distribut
					end
			
					Insert into vshop_Statistics_Distributors (
						RecDate, AgentID, OrderNumber, SaleAmountFee, BuyerNumber, BuyerAvgPrice, CommissionAmountFee
						)
						select RecDate, AgentID, OrderNumber, SaleAmountFee, BuyerNumber, BuyerAvgPrice, CommissionAmountFee
						from #Distributors_01
			end  --200
					 
			
			
			/*****************************************************
			按天统计_商品数据 vshop_Statistics_Products
			*****************************************************/			
			if @FuncAction in ( 300 , 999,99)
			begin
					set @ErrorStep ='按天统计_商品数据'	
					if ( @FuncAction=99 or @FuncAction=999)  and @IsUpdateLog=1
						Update vshop_Statistics_Log
						set  ErrorStep= @ErrorStep
						where  RecDate= @RecDate
												
					--3.按天统计_商品数据 vshop_Statistics_Products
					--现改成每当某产品被浏览时，主动更新 vshop_Statistics_ProductsVisits 数据
					if exists(Select top 1 * from vshop_Statistics_Products where RecDate= @RecDate )
					begin
						delete from  vshop_Statistics_Products where RecDate= @RecDate		
					end
					  
					  
					--以购买过的产品列表为基准，分析产品购买信息(要考虑有的产品是昨天浏览今天付款)
					insert into  vshop_Statistics_Products(  Recdate, ProductID,SaleQty, SaleAmountFee, BuyerNumber, TotalVisits , ConversionRate)
						select @Recdate, ProductId,SaleQty, SaleAmountFee, BuyerNumber, TotalVisits,
							case
								when isnull(TotalVisits,0) =0 or  ISNULL( BuyerNumber,0)=0 then 0
								else BuyerNumber * 1.0 / TotalVisits * 100
							end as ConversionRate
						from
						(
						select  a.ProductID, a.SaleQty, a.SaleAmountFee ,
								ISNULL(c.BuyerNumber,0) as BuyerNumber,
								isnull(b.TotalVisits,0) as TotalVisits
							from @ProductList_Distribut a
							left join vshop_Statistics_ProductsVisits b on a.ProductId=b.ProductID and b.RecDate= @RecDate
							left join @ProductListGroupByUser_Distribut c on a.ProductId= c.ProductId
						) T1
			end  --300
						
						 
				 
				 
			set @ErrorStep ='全部成功.'		
							 
			if ( @FuncAction=99 or @FuncAction=999)  and @IsUpdateLog=1
				Update vshop_Statistics_Log
				set  EndTime= GETDATE(), IsSuccess=1, ErrorStep='', LogInfo='ok'
				where  RecDate= @RecDate
				
			select @RetCode =  1, @RetInfo ='核算成功。'
			
			print 'OK'
	end try
	BEGIN CATCH 
 
			DECLARE @ErrorMessage NVARCHAR(4000); 
			DECLARE @ErrorSeverity INT; 
			DECLARE @ErrorState INT; 

			SELECT 
			@ErrorMessage = ERROR_MESSAGE(), 
			@ErrorSeverity = ERROR_SEVERITY(), 
			@ErrorState = ERROR_STATE(); 

		
			select @ErrorState '@ErrorState', @ErrorMessage '@ErrorMessage'
			
			if ( @FuncAction=99 or @FuncAction=999)	  and @IsUpdateLog=1					
				Update vshop_Statistics_Log
					set  EndTime= GETDATE(), IsSuccess= -1, ErrorStep= @ErrorStep, LogInfo= @ErrorMessage
					where  RecDate= @RecDate
	 
			
			select @RetCode =  -1, @RetInfo ='核算出错。'

	END CATCH; 
	
	
	/*
	
	
	*/

	
end

GO
/****** Object:  StoredProcedure [dbo].[sp_vshop_Statistics_Notify]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_vshop_Statistics_Notify]
@CalDate datetime=null,

@FuncAction int =999,
@ActionDesc varchar(255)='',

@RetCode int output,  --仅当返回 1 才表示核算成功
@RetInfo varchar(256) output
as
begin

/*
@FuncAction取值:
  OrderUpdate = 100,
  CommissionsDraw=101,
  MemberUpdate = 200,
  ProductUpdate = 300,
  AllUpdate = 999
  
  
功能：统计每日订单情况，包括：
按天统计_店铺常规数据
按天统计_分销商数据
按天统计_商品数据

订单日期以付款日期为标准

exec sp_vshop_Statistics_Daily '2015-6-18', 100,1,
	 0, '原因'

sp_refreshview vw_VShop_FinishOrder_Detail

修改日期：2015-10-09

*/

	declare @DEF_IsShowDebugInfo int  --是否显示调试数据， 1=是  其它=否
	set @DEF_IsShowDebugInfo = -1

	select @RetCode = -1, @RetInfo ='原因未知。', @FuncAction=isnull(@FuncAction,999)

	if @CalDate is null
		set @CalDate= GETDATE()
		
		
	--declare @dDate date
	--set @dDate = GETDATE()
	
	--set @dDate = '2015-08-27 10:18:26.703'
	
	select @CalDate = CONVERT(varchar(10), @CalDate, 120 )
	
	--select @RecDate  '@RecDate'
	
	 
	 

		
		
	if CONVERT(varchar(10), @CalDate, 120) <> CONVERT(varchar(10),  GETDATE() , 120) 
	begin  --2015-10-09
	
		insert into vshop_Statistics_NotifyLog (
			RecDate, TaskTime, FuncAction, ActionDesc
			)
		values (
			CONVERT(varchar(10), @CalDate, 120), GETDATE(), @FuncAction,@ActionDesc
			)
			
			
		exec [sp_vshop_Statistics_Daily]  @CalDate , @FuncAction, 
			1, 
			0, '原因'
		select @RetCode =  1, @RetInfo ='核算成功。'	
	end
	else
	begin
		select @RetCode =  1, @RetInfo ='当天的数据不核算，返回。'		
	end
end

GO
/****** Object:  StoredProcedure [dbo].[ss_CreateOrder]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[ss_CreateOrder]
(
	-- 基本信息
    @OrderId nvarchar(50),
    @OrderMarking nvarchar(50),
	@OrderDate	datetime,	
	@UserId	int,
    @UserName nvarchar(50),
    @Wangwang nvarchar(20),
    @RealName nvarchar(50),
    @EmailAddress	nvarchar(255) = null,
    @Remark Nvarchar(4000) =null,
	@ClientShortType int=0,
	@AdjustedDiscount money,
	@OrderStatus int,
	-- 配送信息
	@ShippingRegion Nvarchar(300) = null,
	@Address Nvarchar(300) = null,
	@ZipCode Nvarchar(20) = null,
	@ShipTo Nvarchar(50) = null,
	@TelPhone Nvarchar(50) = null,
	@CellPhone Nvarchar(50) = null,
	@ShipToDate Nvarchar(50) = null,
	@ShippingModeId int = null,
	@ModeName Nvarchar(50) = null,
	@RegionId int = null,
	@Freight money = null,
	@AdjustedFreight money = null,
	@ShipOrderNumber Nvarchar(50) = null,	
    @Weight int = null,
	@ExpressCompanyName nvarchar(500),
    @ExpressCompanyAbb nvarchar(500),
    -- 支付信息
    @PaymentTypeId INT = null,
    @PaymentType Nvarchar(100) = null,	
    @PayCharge money = null,
    @RefundStatus int,
	@Gateway nvarchar(200)=null,
    -- 统计字段
    @OrderTotal money = null,
    @OrderPoint int = null,
    @OrderCostPrice money = null,
    @OrderProfit money = null,
    @OptionPrice money = null,
    @Amount money = null,    
    @DiscountAmount money=null,
	-- 促销信息
	@ReducedPromotionId int = null,
	@ReducedPromotionName nvarchar(100) = null,
	@ReducedPromotionAmount money = null,
	@IsReduced bit = null,

	@SentTimesPointPromotionId int = null,
	@SentTimesPointPromotionName nvarchar(100) = null,
	@TimesPoint money = null,
	@IsSendTimesPoint bit = null,

	@FreightFreePromotionId int = null,
	@FreightFreePromotionName nvarchar(100) = null,
	@IsFreightFree bit = null,
    -- 优惠券信息
    @CouponName nvarchar(100) = null,
	@CouponCode nvarchar(50) = null,
	@CouponAmount money = null,    
	@CouponValue money = null,
    -- 红包信息
    @RedPagerActivityName nvarchar(100) = null,
	@RedPagerID int = null,
	@RedPagerOrderAmountCanUse money = null,    
	@RedPagerAmount money = null,
	--团购活动信息
	@GroupBuyId int = null,
	@NeedPrice money = null,
	@GroupBuyStatus int = null,
		--限时抢购信息
	@CountDownBuyId int = null,

	--捆绑商品
	@Bundlingid int=null,
	--捆绑价格
    @BundlingPrice money = null,
	--税金相关
	@Tax money=null,
	@InvoiceTitle nvarchar(50),
	@ReferralUserId int,
	@ReferralPath varchar(50),
	@ActivitiesId nvarchar(50)=null,
	@ActivitiesName nvarchar(200)=null,
	@FirstCommission money =null,
	@SecondCommission money=null,
	@ThirdCommission money=null,
	@PointToCash money=null,
	@PointExchange int,
	@BargainDetialId int,
	@CouponFreightMoneyTotal money=0,
	@LogisticsTools int=0
)
as 
  IF EXISTS (SELECT OrderId  FROM Hishop_Orders WHERE OrderId = @OrderId)
    Return
  ELSE
  BEGIN
  INSERT INTO Hishop_Orders
   (OrderId,LogisticsTools, OrderDate, UserId, Username,RealName, EmailAddress, Remark,ClientShortType, AdjustedDiscount, OrderStatus,
   ShippingRegion, Address, ZipCode, ShipTo, TelPhone, CellPhone, ShipToDate, ShippingModeId, ModeName, RegionId, Freight, AdjustedFreight, ShipOrderNumber, [Weight], 
   PaymentTypeId,PaymentType, PayCharge, RefundStatus, OrderTotal, OrderPoint, OrderCostPrice, OrderProfit, OptionPrice, Amount, 
   ReducedPromotionId,ReducedPromotionName,ReducedPromotionAmount,IsReduced,SentTimesPointPromotionId,SentTimesPointPromotionName,TimesPoint,IsSendTimesPoint,
   FreightFreePromotionId,FreightFreePromotionName,IsFreightFree,CouponName, CouponCode, CouponAmount, CouponValue,RedPagerActivityName,RedPagerID,RedPagerOrderAmountCanUse,RedPagerAmount,GroupBuyId,NeedPrice,GroupBuyStatus,CountDownBuyId,BundlingId,
   DiscountAmount,ExpressCompanyName,ExpressCompanyAbb,BundlingPrice,Tax,InvoiceTitle,Gateway,ReferralUserId,ReferralPath,ActivitiesId,ActivitiesName,FirstCommission,SecondCommission,ThirdCommission,OrderMarking,PointToCash,PointExchange,BargainDetialId,CouponFreightMoneyTotal
  )
  VALUES 
  (@OrderId,@LogisticsTools, @OrderDate, @UserId, @Username,@RealName, @EmailAddress, @Remark,@ClientShortType, @AdjustedDiscount, @OrderStatus,
   @ShippingRegion, @Address, @ZipCode, @ShipTo, @TelPhone, @CellPhone, @ShipToDate, @ShippingModeId, @ModeName, @RegionId, @Freight, @AdjustedFreight, @ShipOrderNumber, @Weight, 
   @PaymentTypeId,@PaymentType, @PayCharge, @RefundStatus, @OrderTotal, @OrderPoint, @OrderCostPrice, @OrderProfit, @OptionPrice, @Amount, 
   @ReducedPromotionId,@ReducedPromotionName,@ReducedPromotionAmount,@IsReduced,@SentTimesPointPromotionId,@SentTimesPointPromotionName,@TimesPoint,@IsSendTimesPoint,
   @FreightFreePromotionId,@FreightFreePromotionName,@IsFreightFree,@CouponName, @CouponCode, @CouponAmount, @CouponValue,@RedPagerActivityName,@RedPagerID,@RedPagerOrderAmountCanUse,@RedPagerAmount,@GroupBuyId,@NeedPrice,@GroupBuyStatus,@CountDownBuyId,@Bundlingid,
   @DiscountAmount,@ExpressCompanyName,@ExpressCompanyAbb,@BundlingPrice,@Tax,@InvoiceTitle,@Gateway,@ReferralUserId,@ReferralPath,@ActivitiesId,@ActivitiesName,@FirstCommission,@SecondCommission,@ThirdCommission,@OrderMarking,@PointToCash,@PointExchange,@BargainDetialId,@CouponFreightMoneyTotal
   );
   END

GO
/****** Object:  StoredProcedure [dbo].[ss_ShoppingCart_AddLineItem]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ss_ShoppingCart_AddLineItem]
	(
		@UserId INT,
		@SkuId NVARCHAR(100),
		@Quantity INT,
		@CategoryId INT,
		@Type INT,
		@ExchangeId INT,
        @Templateid INT,
		@LimitedTimeDiscountId INT
	)
AS
	IF EXISTS (SELECT [SkuId] FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId  And [Type]=@Type And [LimitedTimeDiscountId]=@LimitedTimeDiscountId)
	BEGIN
		UPDATE 
				Hishop_ShoppingCarts 
		SET 
				Quantity = Quantity + @Quantity
		WHERE 
				UserId = @UserId AND SkuId = @SkuId And [Type]=@Type And [LimitedTimeDiscountId]=@LimitedTimeDiscountId
	END
	ELSE
	BEGIN
		INSERT INTO Hishop_ShoppingCarts 
			(UserId, SkuId, Quantity,CategoryId,[Type],ExchangeId,Templateid,LimitedTimeDiscountId) 
		VALUES 
			(@UserId, @SkuId, @Quantity,@CategoryId,@Type,@ExchangeId,@Templateid,@LimitedTimeDiscountId)
	END

GO
/****** Object:  StoredProcedure [dbo].[ss_ShoppingCart_GetItemInfo]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ss_ShoppingCart_GetItemInfo]
(
		@Quantity INT,
		@UserId INT,
		@SkuId NVARCHAR(100),
		@GradeId INT,
		@Type INT
	)
AS

DECLARE @ProductId INT, @Weight INT, @Stock INT, @SalePrice MONEY, @MemberPrice MONEY, @Discount INT, @SKU NVARCHAR(50),@ExChangeId INT 
 
 SELECT @ProductId = ProductId, @SKU = SKU, @Weight = [Weight], @Stock = Stock, @SalePrice = SalePrice FROM Hishop_SKUs WHERE SkuId = @SkuId
-- 会员查询
IF @UserId>0 
BEGIN
	SELECT @MemberPrice = MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = @SkuId AND GradeId = @GradeId
	SELECT @Discount = Discount FROM aspnet_MemberGrades WHERE GradeId = @GradeId		
	SELECT @Quantity=Quantity,@ExChangeId=ExChangeId FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId And [Type]=@Type
 	IF @MemberPrice IS NOT NULL
		SET @SalePrice = @MemberPrice
	ELSE
		SET @SalePrice = (@SalePrice * @Discount)/100
 END
	
 -- 返回商品基本信息
SELECT ProductId, SaleStatus, @SKU as SKU, @Stock as Stock, @Quantity as TotalQuantity, ProductName, CategoryId, @Weight AS [Weight], @SalePrice AS SalePrice, 
	ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220,IsfreeShipping,MainCategoryPath,IsSetCommission,ThirdCommission,SecondCommission,FirstCommission,FreightTemplateId,CubicMeter,FreightWeight FROM Hishop_Products WHERE ProductId = @ProductId AND SaleStatus=1
-- 返回当前规格信息
SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId
left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId
AND s.ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1)
--返回积分兑换信息
select ProductNumber,PointNumber,ChangedNumber,eachMaxNumber,[status],exChangeId from Hishop_PointExChange_Products where exChangeId=@ExChangeId and ProductId=@ProductId

GO
/****** Object:  Trigger [dbo].[T_Hishop_AttributeValues_Delete]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[T_Hishop_AttributeValues_Delete]
    ON [dbo].[Hishop_AttributeValues] FOR DELETE                          
    AS       
        DELETE Hishop_ProductAttributes 
		FROM Hishop_ProductAttributes at, Deleted d  
        WHERE at.ValueID=d.ValueID

GO
/****** Object:  Trigger [dbo].[T_Hishop_Categories_Delete]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   Trigger   [dbo].[T_Hishop_Categories_Delete] ON [dbo].[Hishop_Categories] FOR Delete AS
BEGIN
	DECLARE @ParentCategoryId INT
	SELECT @ParentCategoryId= ParentCategoryId FROM deleted;

	IF @ParentCategoryId = 0 OR @ParentCategoryId IS NULL
		RETURN
		
	IF EXISTS(SELECT CategoryId FROM Hishop_Categories WHERE CategoryId = @ParentCategoryId)
	BEGIN
		-- 如果上级分类存在，且上级分类已没有任何子分类，则更新上级分类的HasChildren字段为0
		IF (SELECT COUNT(CategoryId) FROM Hishop_Categories WHERE ParentCategoryId = @ParentCategoryId) = 0
			UPDATE Hishop_Categories SET HasChildren = 0 WHERE CategoryId = @ParentCategoryId
	END
END

GO
/****** Object:  Trigger [dbo].[T_Hishop_Categories_Insert]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   Trigger   [dbo].[T_Hishop_Categories_Insert] ON [dbo].[Hishop_Categories] FOR Insert AS
BEGIN
	DECLARE @ParentCategoryId INT
	SELECT @ParentCategoryId= ParentCategoryId FROM inserted;

	IF @ParentCategoryId = 0 OR @ParentCategoryId IS NULL
		RETURN

	IF EXISTS(SELECT CategoryId FROM Hishop_Categories WHERE CategoryId = @ParentCategoryId AND HasChildren = 0)
	BEGIN
		-- 如果上级分类存在，且上级分类当前无子分类，则更新上级分类的HasChildren字段为1
		UPDATE Hishop_Categories SET HasChildren = 1 WHERE CategoryId = @ParentCategoryId
	END
END

GO
/****** Object:  Trigger [dbo].[T_Hishop_CouponItems_Update]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create TRIGGER [dbo].[T_Hishop_CouponItems_Update]
ON [dbo].[Hishop_CouponItems]
FOR update
AS 
BEGIN
	DECLARE @CouponId INT
	declare @CouponStatus int
	SELECT @CouponId= CouponId,@CouponStatus=CouponStatus FROM inserted;
	if @CouponStatus=1
	begin
		UPDATE hishop_Coupons SET UsedCount =(select COUNT(CouponId) from dbo.Hishop_CouponItems  WHERE CouponStatus = 1 and CouponId = @CouponId )where CouponId = @CouponId
	end
END

GO
/****** Object:  Trigger [dbo].[T_Templates_Delete]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[T_Templates_Delete]
	ON [dbo].[Hishop_FreightTemplate_Templates] 
	FOR DELETE
	AS
	BEGIN
DECLARE @Did decimal(18, 0)
select @Did =TemplateId from DELETED
delete from Hishop_FreightTemplate_SpecifyRegionGroups where TemplateId=@Did
delete from Hishop_FreightTemplate_FreeShipping where TemplateId=@Did
delete from Hishop_FreightTemplate_FreeShippingRegions where TemplateId=@Did
delete from Hishop_FreightTemplate_SpecifyRegions where TemplateId=@Did
END

GO
/****** Object:  Trigger [dbo].[UpdateStatic_Daily]    Script Date: 2017/3/14 21:10:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		周新星
-- Create date: 2015-10-29
-- Description:	订单信息变化时，同步更新静态统计表。执行sp_vshop_Statistics_Daily存储过程
-- =============================================
CREATE TRIGGER [dbo].[UpdateStatic_Daily]
	ON [dbo].[Hishop_Orders] 
	FOR UPDATE
	AS
	BEGIN
--Inserted表有数据（新数据），Deleted表有数据（旧数据）
    declare @c bigint
    declare  @v varchar(256) 
    
    declare @NewOrderStatus int
    declare @OldOrderStatus int
    declare @NewOrderTotal money
    declare @OldOrderTotal money
    declare @OrderDate datetime --订单时间
    declare @PayDate datetime --支付时间
    declare @Gateway nvarchar(200) --付款方式 hishop.plugins.payment.podrequest 货到付款
    declare @UpDate varchar(10)
    declare @IsUpdate bigint --是否更新
    
    --订单金额跟状态变化时，出发事件
	if update(OrderTotal) OR update(OrderStatus)
	begin
	select @OrderDate=OrderDate,@PayDate=PayDate,@Gateway=Gateway,@NewOrderTotal= OrderTotal,@NewOrderStatus= OrderStatus from inserted
	select @OldOrderTotal= OrderTotal,@OldOrderStatus= OrderStatus from Deleted
	declare @RetCode int
    declare  @RetInfo varchar(255) 
    --CONVERT(varchar(10),@PayDate , 120 )
    
      if(@Gateway<>'hishop.plugins.payment.podrequest')
        set @UpDate=CONVERT(varchar(10),@PayDate , 120) --非货到付款，取支付时间统计订单
      else
        set @UpDate=CONVERT(varchar(10),@OrderDate , 120) --货到付款，取订单时间统计订单
    
        set @UpDate=ISNULL(@UpDate,'')
        set  @RetCode=0
        set  @RetInfo='step1'
        set  @IsUpdate=0
        
    
        --只有已付款货到付款订单可以取到 ,如果不是更新
        if(@UpDate<>'' and @UpDate<>CONVERT(varchar(10),GETDATE() , 120))
        begin
             set  @RetInfo='step2检查是否更新'
             
             --如果订单金额不一样，更新订单所在日期数据
             if(@NewOrderTotal<>@OldOrderTotal)
               set @IsUpdate=1
             
             --如果订单状态发生变化
             if(@NewOrderStatus<>@OldOrderStatus)
             begin
               if @NewOrderTotal in (4,9, 10)
                  set @IsUpdate=1 --当订单变成关闭，交易成功、退款、退货状态时
             end 
               
             if(@IsUpdate=1)
             begin
                exec sp_vshop_Statistics_Daily @UpDate, 999, 0, @RetCode output,@RetInfo  output 
               -- print @RetInfo
             	--set  @v =cast(@RetCode as varchar(10)) + '--' +cast(@NewOrderStatus as varchar(10)) + '^' + cast(@NewOrderTotal as varchar(10))+ '|'+ cast(@OldOrderStatus as varchar(10)) + '^' + cast(@OldOrderTotal as varchar(10))
	           -- insert into test(datet,v)values(GETDATE(), @v)
             end
                     
        end


	end
	
END

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级ID，唯一自增' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_DistributorGrade', @level2type=N'COLUMN',@level2name=N'GradeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_DistributorGrade', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_DistributorGrade', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满足佣金点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_DistributorGrade', @level2type=N'COLUMN',@level2name=N'CommissionsLimit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一级佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_DistributorGrade', @level2type=N'COLUMN',@level2name=N'FirstCommissionRise'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'二级佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_DistributorGrade', @level2type=N'COLUMN',@level2name=N'SecondCommissionRise'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'三级佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_DistributorGrade', @level2type=N'COLUMN',@level2name=N'ThirdCommissionRise'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否默认等级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_DistributorGrade', @level2type=N'COLUMN',@level2name=N'IsDefault'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级图标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_DistributorGrade', @level2type=N'COLUMN',@level2name=N'Ico'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'升级到该等级，奖励金额多少前，需要配合配置文件中的是否开启奖励（IsAddCommission）以及开启时间段来使用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_DistributorGrade', @level2type=N'COLUMN',@level2name=N'AddCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分销商等级表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_DistributorGrade'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分销商ID，与会员ID一致' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'店铺名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'StoreName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'店铺LOGO' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'Logo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'背景图，留用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'BackImage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请提现的账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'RequestAccount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请提现账号修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'AccountTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分销商代理级别1级2级3级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'GradeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级代理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'ReferralUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级代理完整路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'ReferralPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本店订单销售总额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'OrdersTotal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本店订单成交数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'ReferralOrders'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未提现佣金剩余金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'ReferralBlance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'佣金已提现总金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'ReferralRequestBalance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态(0代表正常，1代表冻结,9删除)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'ReferralStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'店铺简述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'StoreDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分销商等级（非代理等级）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors', @level2type=N'COLUMN',@level2name=N'DistributorGradeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分销商信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Distributors'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Managers', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Managers', @level2type=N'COLUMN',@level2name=N'RoleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Managers', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Managers', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Managers', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Managers', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Managers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员等级ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_MemberGrades', @level2type=N'COLUMN',@level2name=N'GradeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_MemberGrades', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_MemberGrades', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_MemberGrades', @level2type=N'COLUMN',@level2name=N'Points'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否默认' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_MemberGrades', @level2type=N'COLUMN',@level2name=N'IsDefault'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'折扣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_MemberGrades', @level2type=N'COLUMN',@level2name=N'Discount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单金额达到' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_MemberGrades', @level2type=N'COLUMN',@level2name=N'TranVol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单数量达到' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_MemberGrades', @level2type=N'COLUMN',@level2name=N'TranTimes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员等级表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_MemberGrades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户等级ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'GradeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'ReferralUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'OrderNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消费金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'Expenditure'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'剩余积分点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'Points'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属省的编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'TopRegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属地区的编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'真实姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'RealName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'CellPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QQ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'QQ'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员卡号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'VipCardNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'领卡日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'VipCardDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公众号Openid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'OpenId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'临时身份标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'SessionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'临时身份标识有效果期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'SessionEndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'MicroSignal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信头像' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'UserHead'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'绑定用户登录名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'UserBindName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户状态(正常:1 已删除:7)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身份证' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'CardID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最近订单时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'LastOrderDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最近订单付款时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'PayOrderDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最近订单完成时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'FinishOrderDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已授权登录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'IsAuthorizeWeiXin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'金额总额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'TotalAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可使用金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members', @level2type=N'COLUMN',@level2name=N'AvailableAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Members'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_RolePermissions', @level2type=N'COLUMN',@level2name=N'PermissionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门(权限组)ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_RolePermissions', @level2type=N'COLUMN',@level2name=N'RoleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门与权限对应表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_RolePermissions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Roles', @level2type=N'COLUMN',@level2name=N'RoleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Roles', @level2type=N'COLUMN',@level2name=N'RoleName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Roles', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否默认' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Roles', @level2type=N'COLUMN',@level2name=N'IsDefault'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'aspnet_Roles'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'ActivitiesId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'ActivitiesName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'ActivitiesType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'StartTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'EndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'ActivitiesDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??Ч????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'TakeEffect'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'MemberGrades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'attendTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'attendType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'isAllProduct'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'MeetMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'ReductionMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities', @level2type=N'COLUMN',@level2name=N'MeetType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities_Detail', @level2type=N'COLUMN',@level2name=N'ActivitiesId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������Ԫ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities_Detail', @level2type=N'COLUMN',@level2name=N'MeetMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������Ԫ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities_Detail', @level2type=N'COLUMN',@level2name=N'ReductionMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities_Detail', @level2type=N'COLUMN',@level2name=N'bFreeShipping'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ͻ��� ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities_Detail', @level2type=N'COLUMN',@level2name=N'Integral'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���Ż�ȯID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities_Detail', @level2type=N'COLUMN',@level2name=N'CouponId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities_Detail', @level2type=N'COLUMN',@level2name=N'MeetNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities_Detail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities_Product', @level2type=N'COLUMN',@level2name=N'ActivitiesId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ƷID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities_Product', @level2type=N'COLUMN',@level2name=N'ProductID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'״̬' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities_Product', @level2type=N'COLUMN',@level2name=N'status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Activities_Product'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满减活动ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ActivitiesMember', @level2type=N'COLUMN',@level2name=N'ActivitiesId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ActivitiesMember', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满减活动关联会员表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ActivitiesMember'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Attributes', @level2type=N'COLUMN',@level2name=N'AttributeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Attributes', @level2type=N'COLUMN',@level2name=N'AttributeName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顺序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Attributes', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Attributes', @level2type=N'COLUMN',@level2name=N'TypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性的使用方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Attributes', @level2type=N'COLUMN',@level2name=N'UsageMode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用属性值的图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Attributes', @level2type=N'COLUMN',@level2name=N'UseAttributeImage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Attributes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性值编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_AttributeValues', @level2type=N'COLUMN',@level2name=N'ValueId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_AttributeValues', @level2type=N'COLUMN',@level2name=N'AttributeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示顺序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_AttributeValues', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_AttributeValues', @level2type=N'COLUMN',@level2name=N'ValueStr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_AttributeValues', @level2type=N'COLUMN',@level2name=N'ImageUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性值表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_AttributeValues'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提现申请' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'SerialID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请人编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请类型（0微信 1淘宝，2网银转帐）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'RequestType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请人名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'RequestTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请人真实姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'AccountName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'CellPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收款账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'MerchantCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生成红包的个数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'RedpackRecordNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否审核通过 0未审核 1已审核 2已发佣金 -1驳回 3发放异常' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'IsCheck'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核通过时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'CheckTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'银行卡名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest', @level2type=N'COLUMN',@level2name=N'bankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提现申请记录表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BalanceDrawRequest'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Banner', @level2type=N'COLUMN',@level2name=N'BannerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'简短描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Banner', @level2type=N'COLUMN',@level2name=N'ShortDesc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Banner', @level2type=N'COLUMN',@level2name=N'ImageUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Banner', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Banner', @level2type=N'COLUMN',@level2name=N'LocationType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Banner', @level2type=N'COLUMN',@level2name=N'Url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型（导航【2】或者是轮播图【1】）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Banner', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Banner', @level2type=N'COLUMN',@level2name=N'IsDisable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'轮播图表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Banner'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分享标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动封面' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'ActivityCover'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'BeginDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'EndDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'Remarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动商品' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动库存' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'ActivityStock'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限购数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'PurchaseNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:每次砍掉  1 :随机砍掉' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'BargainType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动的初始价格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'InitialPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0: 不开启 1 ：开启' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'IsCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'底价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'FloorPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成交数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'TranNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除：1 删除 0 未删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Bargain', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'砍价发起人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BargainDetial', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hishop_Bargain表编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BargainDetial', @level2type=N'COLUMN',@level2name=N'BargainId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'购买数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BargainDetial', @level2type=N'COLUMN',@level2name=N'Number'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前价格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BargainDetial', @level2type=N'COLUMN',@level2name=N'Price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'参与人数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BargainDetial', @level2type=N'COLUMN',@level2name=N'NumberOfParticipants'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除：1 删除 0 未删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BargainDetial', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BrandCategories', @level2type=N'COLUMN',@level2name=N'BrandId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BrandCategories', @level2type=N'COLUMN',@level2name=N'BrandName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Logo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BrandCategories', @level2type=N'COLUMN',@level2name=N'Logo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BrandCategories', @level2type=N'COLUMN',@level2name=N'CompanyUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL重写名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BrandCategories', @level2type=N'COLUMN',@level2name=N'RewriteName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'搜索关键字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BrandCategories', @level2type=N'COLUMN',@level2name=N'MetaKeywords'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关键字描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BrandCategories', @level2type=N'COLUMN',@level2name=N'MetaDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌介绍' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BrandCategories', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BrandCategories', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_BrandCategories'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返现自增序列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_CashBack', @level2type=N'COLUMN',@level2name=N'CashBackId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'充值金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_CashBack', @level2type=N'COLUMN',@level2name=N'RechargeAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已返现金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_CashBack', @level2type=N'COLUMN',@level2name=N'CashBackAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返现百分比' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_CashBack', @level2type=N'COLUMN',@level2name=N'Percentage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返现类型（0：现金 1：积分）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_CashBack', @level2type=N'COLUMN',@level2name=N'CashBackType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_CashBack', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_CashBack', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已返完成' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_CashBack', @level2type=N'COLUMN',@level2name=N'IsFinished'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'完成返现日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_CashBack', @level2type=N'COLUMN',@level2name=N'FinishedDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返现金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_CashBackDetails', @level2type=N'COLUMN',@level2name=N'CashBackAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返现日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_CashBackDetails', @level2type=N'COLUMN',@level2name=N'CashBackDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'CategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类图标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示顺序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类图标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'IconUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'搜索标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'Meta_Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'Meta_Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'搜索关键字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'Meta_Keywords'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父分类ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'ParentCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类深度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'Depth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'Path'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL重写名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'RewriteName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商家编码前缀' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'SKUPrefix'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联的商品类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'AssociatedProductType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导购信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'Notes1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'Theme'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'获取商品分类是否有子分类，除了从数据库中读取此字段值以外，不要在其他地方给此属性赋值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'HasChildren'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成交店铺佣金比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'FirstCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上一级佣金比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'SecondCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上二级佣金比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories', @level2type=N'COLUMN',@level2name=N'ThirdCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Categories'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'佣金提成ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Commissions', @level2type=N'COLUMN',@level2name=N'CommId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'佣金获得者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Commissions', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生成佣金者ID ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Commissions', @level2type=N'COLUMN',@level2name=N'ReferralUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Commissions', @level2type=N'COLUMN',@level2name=N'OrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Commissions', @level2type=N'COLUMN',@level2name=N'TradeTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单总金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Commissions', @level2type=N'COLUMN',@level2name=N'OrderTotal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Commissions', @level2type=N'COLUMN',@level2name=N'CommTotal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'佣金类型(1代表佣金，2直接推广)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Commissions', @level2type=N'COLUMN',@level2name=N'CommType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'佣金状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Commissions', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Commissions', @level2type=N'COLUMN',@level2name=N'CommRemark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'佣金提成表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Commissions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'CouponId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'CouponName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'CouponValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'ConditionValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'BeginDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'EndDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'StockNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????????????????????Σ?' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'ReceiveNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????????????????Σ?' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'UsedNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'MemberGrades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'ImgUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????????????????????0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'ProductNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'Finished'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'IsAllProduct'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'maxReceivNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons', @level2type=N'COLUMN',@level2name=N'CouponTypes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Coupons'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ż�ȯID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons', @level2type=N'COLUMN',@level2name=N'CouponId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ԱID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons', @level2type=N'COLUMN',@level2name=N'MemberId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�յ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons', @level2type=N'COLUMN',@level2name=N'ReceiveDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ʹ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons', @level2type=N'COLUMN',@level2name=N'UsedDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons', @level2type=N'COLUMN',@level2name=N'OrderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'״̬' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ż�ȯ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons', @level2type=N'COLUMN',@level2name=N'CouponName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ֵ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons', @level2type=N'COLUMN',@level2name=N'ConditionValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʼ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons', @level2type=N'COLUMN',@level2name=N'BeginDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons', @level2type=N'COLUMN',@level2name=N'EndDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons', @level2type=N'COLUMN',@level2name=N'CouponValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_MemberCoupons'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ż�ȯID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Products', @level2type=N'COLUMN',@level2name=N'CouponId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ƷID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Products', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'״̬' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Products', @level2type=N'COLUMN',@level2name=N'status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Coupon_Products'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分销商升级奖励佣金表ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_DistributorGradeCommission', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分销商ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_DistributorGradeCommission', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖励的佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_DistributorGradeCommission', @level2type=N'COLUMN',@level2name=N'Commission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'记录的时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_DistributorGradeCommission', @level2type=N'COLUMN',@level2name=N'PubTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_DistributorGradeCommission', @level2type=N'COLUMN',@level2name=N'OperAdmin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖励说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_DistributorGradeCommission', @level2type=N'COLUMN',@level2name=N'Memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单号，可为空' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_DistributorGradeCommission', @level2type=N'COLUMN',@level2name=N'OrderID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'之前总佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_DistributorGradeCommission', @level2type=N'COLUMN',@level2name=N'OldCommissionTotal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_DistributorProducts', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_DistributorProducts', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分销商关联商品表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_DistributorProducts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收藏ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Favorite', @level2type=N'COLUMN',@level2name=N'FavoriteId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Favorite', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Favorite', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Favorite', @level2type=N'COLUMN',@level2name=N'Tags'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Favorite', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收藏商品表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Favorite'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对就运费模板ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_FreeShipping', @level2type=N'COLUMN',@level2name=N'TemplateId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运送方式1快递、2EMS、3顺丰、4平邮' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_FreeShipping', @level2type=N'COLUMN',@level2name=N'ModeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包邮类型1件数、2金额、3件数+金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_FreeShipping', @level2type=N'COLUMN',@level2name=N'ConditionType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满足条件值，注意如果ConditionType为3，值为‘件数|金额’' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_FreeShipping', @level2type=N'COLUMN',@level2name=N'ConditionNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免邮ID，自增唯一' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_FreeShipping', @level2type=N'COLUMN',@level2name=N'FreeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满件包邮表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_FreeShipping'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运费模板ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_FreeShippingRegions', @level2type=N'COLUMN',@level2name=N'TemplateId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地区ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_FreeShippingRegions', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运费ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_FreeShippingRegions', @level2type=N'COLUMN',@level2name=N'FreeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包邮区域表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_FreeShippingRegions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分组ID，唯一自增' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegionGroups', @level2type=N'COLUMN',@level2name=N'GroupId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对应的费运模板ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegionGroups', @level2type=N'COLUMN',@level2name=N'TemplateId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运送方式1快递、2EMS、3顺丰、4平邮' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegionGroups', @level2type=N'COLUMN',@level2name=N'ModeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'起件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegionGroups', @level2type=N'COLUMN',@level2name=N'FristNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'起件运费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegionGroups', @level2type=N'COLUMN',@level2name=N'FristPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'增件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegionGroups', @level2type=N'COLUMN',@level2name=N'AddNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'增件运费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegionGroups', @level2type=N'COLUMN',@level2name=N'AddPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否默认运费1是、0否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegionGroups', @level2type=N'COLUMN',@level2name=N'IsDefault'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'指定区域运费表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegionGroups'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对应运费模板ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegions', @level2type=N'COLUMN',@level2name=N'TemplateId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联的区域运费表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegions', @level2type=N'COLUMN',@level2name=N'GroupId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地区ID值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegions', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区域运费关联表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_SpecifyRegions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运费模板ID，唯一自增' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_Templates', @level2type=N'COLUMN',@level2name=N'TemplateId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模板名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_Templates', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'不否包邮，1包邮，0不包邮' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_Templates', @level2type=N'COLUMN',@level2name=N'FreeShip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'计价单位，1按件、2按重点、3按体积' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_Templates', @level2type=N'COLUMN',@level2name=N'MUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否包含免费条件，1是，0否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_Templates', @level2type=N'COLUMN',@level2name=N'HasFree'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运费模板主表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_FreightTemplate_Templates'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hishop_BargainDetial表编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_HelpBargainDetial', @level2type=N'COLUMN',@level2name=N'BargainDetialId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'砍掉的价格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_HelpBargainDetial', @level2type=N'COLUMN',@level2name=N'BargainPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_HelpBargainDetial', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscount', @level2type=N'COLUMN',@level2name=N'ActivityName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscount', @level2type=N'COLUMN',@level2name=N'BeginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscount', @level2type=N'COLUMN',@level2name=N'EndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscount', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每人限购' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscount', @level2type=N'COLUMN',@level2name=N'LimitNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员等级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscount', @level2type=N'COLUMN',@level2name=N'ApplyMembers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自动分组会员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscount', @level2type=N'COLUMN',@level2name=N'DefualtGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手动分组会员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscount', @level2type=N'COLUMN',@level2name=N'CustomGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscount', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscount', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否分佣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscount', @level2type=N'COLUMN',@level2name=N'IsCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品佣金打折' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscount', @level2type=N'COLUMN',@level2name=N'CommissionDiscount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscountProduct', @level2type=N'COLUMN',@level2name=N'LimitedTimeDiscountId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscountProduct', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'折扣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscountProduct', @level2type=N'COLUMN',@level2name=N'Discount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'减去多少元' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscountProduct', @level2type=N'COLUMN',@level2name=N'Minus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否去角' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscountProduct', @level2type=N'COLUMN',@level2name=N'IsDehorned'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否去角分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscountProduct', @level2type=N'COLUMN',@level2name=N'IsChamferPoint'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最终价格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscountProduct', @level2type=N'COLUMN',@level2name=N'FinalPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscountProduct', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscountProduct', @level2type=N'COLUMN',@level2name=N'BeginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscountProduct', @level2type=N'COLUMN',@level2name=N'EndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_LimitedTimeDiscountProduct', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Logs', @level2type=N'COLUMN',@level2name=N'LogId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作所在的页面' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Logs', @level2type=N'COLUMN',@level2name=N'PageUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作的时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Logs', @level2type=N'COLUMN',@level2name=N'AddedTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Logs', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户端机器的ip地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Logs', @level2type=N'COLUMN',@level2name=N'IPAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作的名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Logs', @level2type=N'COLUMN',@level2name=N'Privilege'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Logs', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作日志表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Logs'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountDetailed', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountDetailed', @level2type=N'COLUMN',@level2name=N'PayId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountDetailed', @level2type=N'COLUMN',@level2name=N'TradeAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可用余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountDetailed', @level2type=N'COLUMN',@level2name=N'AvailableAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 交易类型 0提现 1充值 2佣金转入 3在线支付 4售后退款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountDetailed', @level2type=N'COLUMN',@level2name=N'TradeType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountDetailed', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易方式 0微信钱包 1支付宝 2店铺佣金 3余额 4盛付通 5线下转账' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountDetailed', @level2type=N'COLUMN',@level2name=N'TradeWays'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountDetailed', @level2type=N'COLUMN',@level2name=N'TradeTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountDetailed', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'金额状态 0 未完成操作 1已完成操作' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountDetailed', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'RequestTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提现金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请类型 微信支付 = 0,支付宝 = 1,线下支付=2,其他支付=3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'RequestType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'AccountCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收款人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'AccountName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'BankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态 None=-2 未审核=0 已审核=1 已发放=2 驳回=-1 发放异常=3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核通过时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'CheckTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收款人联系电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'CellPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberAmountRequest', @level2type=N'COLUMN',@level2name=N'Operator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置类型（1、新客户、2活跃客户、3休眠客户）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberClientSet', @level2type=N'COLUMN',@level2name=N'ClientTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberClientSet', @level2type=N'COLUMN',@level2name=N'StartTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberClientSet', @level2type=N'COLUMN',@level2name=N'EndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满足条件的天数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberClientSet', @level2type=N'COLUMN',@level2name=N'LastDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满足条件的格式（大于或大于等于）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberClientSet', @level2type=N'COLUMN',@level2name=N'ClientChar'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满足值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberClientSet', @level2type=N'COLUMN',@level2name=N'ClientValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户分组配置表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MemberClientSet'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信用户：0 or 支付宝服务窗用户：1 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageAdminUserMsgList', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'MessageType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模板名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否需要通过邮件发送' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'SendEmail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息是否需要通过手机短信发送' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'SendSMS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息是否需要通过站内信发送' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'SendInnerMessage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否需要通过微信发送' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'SendWeixin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信模板消息Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'WeixinTemplateId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息模板标签说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'TagDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮件消息的主题格式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'EmailSubject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮件消息的内容格式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'EmailBody'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站内信消息的主题格式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'InnerMessageSubject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站内信消息的内容格式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'InnerMessageBody'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机短信消息的内容格式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates', @level2type=N'COLUMN',@level2name=N'SMSBody'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息模板表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_MessageTemplates'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公告或消息编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Notice', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Notice', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Notice', @level2type=N'COLUMN',@level2name=N'Memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Notice', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Notice', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布状态，0未发布，1已发布' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Notice', @level2type=N'COLUMN',@level2name=N'IsPub'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Notice', @level2type=N'COLUMN',@level2name=N'PubTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型：公告0，消息1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Notice', @level2type=N'COLUMN',@level2name=N'SendType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送对象（0所有,1分销商，2指定对象,对应发送用户数据表Hishop_NoticeUser）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Notice', @level2type=N'COLUMN',@level2name=N'SendTo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公共或消息ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_NoticeRead', @level2type=N'COLUMN',@level2name=N'NoticeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'读取人UserID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_NoticeRead', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'读取时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_NoticeRead', @level2type=N'COLUMN',@level2name=N'ReadTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公共或消息用户自己是否删除，1删除，0默认' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_NoticeRead', @level2type=N'COLUMN',@level2name=N'NoticeIsDel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'临时存放选择的用户表，管理员登录名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_NoticeTempUser', @level2type=N'COLUMN',@level2name=N'LoginName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户的编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_NoticeTempUser', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公共或消息ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_NoticeUser', @level2type=N'COLUMN',@level2name=N'NoticeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_NoticeUser', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderDebitNote', @level2type=N'COLUMN',@level2name=N'NoteId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderDebitNote', @level2type=N'COLUMN',@level2name=N'OrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderDebitNote', @level2type=N'COLUMN',@level2name=N'Operator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderDebitNote', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收款单表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderDebitNote'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'OrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'规格ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'SkuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'货号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'SKU'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'购买数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'Quantity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发货数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'ShipmentQuantity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成本价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'CostPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'ItemListPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'促销后的价格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'ItemAdjustedPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分销商调价佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'ItemAdjustedCommssion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单项名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'ItemDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品缩略图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'ThumbnailsUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'重量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品规格内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'SKUContent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'促销编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'PromotionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'促销名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'PromotionName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单商品基本状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'OrderItemsStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本店分销商商品佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'ItemsCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级分销商商品佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'SecondItemsCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上上级分销商商品佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'ThirdItemsCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分兑换' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'PointNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0为普通商品，1为积分兑换商品' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标志订单退款或退货是否被处理过' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'IsHandled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员确定的退款金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'ReturnMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品优惠的金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'DiscountAverage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否为后台管理员调价（1是后台管理员调价，0是分销商调价）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'IsAdminModify'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单删除前的状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'DeleteBeforeState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限时折扣活动ID，0表示不参与限时折扣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'LimitedTimeDiscountId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1级佣金比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'ItemsCommissionScale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'2级佣金比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'SecondItemsCommissionScale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'3级佣金比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'ThirdItemsCommissionScale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用余额抵扣金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems', @level2type=N'COLUMN',@level2name=N'BalancePayMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单商品详细表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderItems'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderRefund', @level2type=N'COLUMN',@level2name=N'RefundId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderRefund', @level2type=N'COLUMN',@level2name=N'OrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderRefund', @level2type=N'COLUMN',@level2name=N'ApplyForTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款途径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderRefund', @level2type=N'COLUMN',@level2name=N'RefundType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderRefund', @level2type=N'COLUMN',@level2name=N'RefundRemark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderRefund', @level2type=N'COLUMN',@level2name=N'AdminRemark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'处理状态,0为待处理,1为已处理,2为已拒绝' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderRefund', @level2type=N'COLUMN',@level2name=N'HandleStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'处理时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderRefund', @level2type=N'COLUMN',@level2name=N'HandleTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderRefund', @level2type=N'COLUMN',@level2name=N'Operator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款单基本信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderRefund'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'换货单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReplace', @level2type=N'COLUMN',@level2name=N'ReplaceId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReplace', @level2type=N'COLUMN',@level2name=N'OrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReplace', @level2type=N'COLUMN',@level2name=N'ApplyForTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'换货留言' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReplace', @level2type=N'COLUMN',@level2name=N'Comments'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'处理状态,0为处理中,1为已处理,2为已拒绝' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReplace', @level2type=N'COLUMN',@level2name=N'HandleStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'处理时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReplace', @level2type=N'COLUMN',@level2name=N'HandleTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReplace', @level2type=N'COLUMN',@level2name=N'AdminRemark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'换货单基本信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReplace'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'ReturnsId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'OrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'ApplyForTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'RefundType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'RefundMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退货留言' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'Comments'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'处理状态,0为处理中,1为已处理,2为已拒绝' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'HandleStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'处理时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'HandleTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'AdminRemark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'Operator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收款帐号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'Account'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品ID号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'AuditTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'RefundTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品规格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'SkuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除前的状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'DeleteBeforeState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'设置字段关联，使用订单详细ID关联' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'OrderItemID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退回余额的金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns', @level2type=N'COLUMN',@level2name=N'BalanceReturnMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退货单基本信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderReturns'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'OrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'多个订单标示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'OrderMarking'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'买家留言' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ManagerMark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ManagerRemark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单折扣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'AdjustedDiscount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'OrderStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单关闭原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'CloseReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下单时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'OrderDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'PayDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发货时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ShippingDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易完成时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'FinishDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'Username'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电子邮件地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'EmailAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'真实姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'RealName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QQ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'QQ'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'旺旺' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'Wangwang'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MSN' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'MSN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配送区域' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ShippingRegion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'街道地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮政编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ZipCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ShipTo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电话号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'TelPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'CellPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货时期段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ShipToDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配送方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ShippingModeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配送方式名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ModeName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实际配送方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'RealShippingModeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实际配送方式名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'RealModeName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配送区域' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原邮费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'Freight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实际邮费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'AdjustedFreight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发货单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ShipOrderNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单货物总重量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'快递公司名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ExpressCompanyName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'快递公司缩写' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ExpressCompanyAbb'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付方式编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'PaymentTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付方式名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'PaymentType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原支付手续费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'PayCharge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'RefundStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'RefundAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'RefundRemark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'Gateway'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单总金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'OrderTotal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单总积分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'OrderPoint'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单总成本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'OrderCostPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单利润（订单总金额 - 订单总成本）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'OrderProfit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实际运费（预留给管理员填写，以便更精确的计算订单总成本）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ActualFreight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'其他成本（预留给管理员填写，以便更精确的计算订单总成本）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'OtherCost'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单可选项金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'OptionPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品总金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满减活动减免总金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'DiscountAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满减活动ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ActivitiesId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满减活动名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ActivitiesName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满额或满量优惠的促销编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ReducedPromotionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满额或满量优惠的促销名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ReducedPromotionName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满额或满量优惠的金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ReducedPromotionAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有满额或满量优惠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'IsReduced'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满额送倍数积分的促销编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'SentTimesPointPromotionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满额送倍数积分的促销名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'SentTimesPointPromotionName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分倍数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'TimesPoint'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有送倍数积分的促销' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'IsSendTimesPoint'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'额免运费的促销编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'FreightFreePromotionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'满额免运费的促销名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'FreightFreePromotionName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有免运费的促销' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'IsFreightFree'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优惠券名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'CouponName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优惠券号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'CouponCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优惠券满足金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'CouponAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优惠券金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'CouponValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'团购活动ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'GroupBuyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'团购保证金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'NeedPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'团购状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'GroupBuyStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限时抢购ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'CountDownBuyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'捆绑商品ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'BundlingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'捆绑商品数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'BundlingNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'捆绑价格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'BundlingPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方支付公司的交易编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'GatewayOrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已经打印' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'IsPrinted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'税金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发票抬头' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'InvoiceTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发货人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'Sender'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单来源（分销商）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ReferralUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上二级分销商佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'FirstCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上一级分销商佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'SecondCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本店佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ThirdCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'抵用券名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'RedPagerActivityName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'抵用券ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'RedPagerID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'抵用券满足金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'RedPagerOrderAmountCanUse'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'抵用券金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'RedPagerAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'旧地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'OldAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分抵现金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'PointToCash'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分抵现点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'PointExchange'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拆分状态，0未拆分，1已拆分主订单，2已拆分从订单' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'SplitState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单删除前的状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'DeleteBeforeState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户端如支付宝服务窗2微信1,其他或者未知的浏览器：0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ClientShortType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'记录三级分佣的用户ID，以适应调整分销商关系后的分佣问题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'ReferralPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用于砍价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'BargainDetialId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用余额抵扣订单详情总金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'BalancePayMoneyTotal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用余额支付的运费部分总金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'BalancePayFreightMoneyTotal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优惠券抵用运费金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'CouponFreightMoneyTotal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物流查询工具，默认0用快递鸟' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders', @level2type=N'COLUMN',@level2name=N'LogisticsTools'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Orders'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发货单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderSendNote', @level2type=N'COLUMN',@level2name=N'NoteId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderSendNote', @level2type=N'COLUMN',@level2name=N'OrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderSendNote', @level2type=N'COLUMN',@level2name=N'Operator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发货单表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_OrderSendNote'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付方式编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PaymentTypes', @level2type=N'COLUMN',@level2name=N'ModeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付方式名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PaymentTypes', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'介绍' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PaymentTypes', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网关类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PaymentTypes', @level2type=N'COLUMN',@level2name=N'Gateway'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示顺序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PaymentTypes', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否用于在线充值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PaymentTypes', @level2type=N'COLUMN',@level2name=N'IsUseInpour'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否用于采购单付款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PaymentTypes', @level2type=N'COLUMN',@level2name=N'IsUseInDistributor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手续费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PaymentTypes', @level2type=N'COLUMN',@level2name=N'Charge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付方式配置信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PaymentTypes', @level2type=N'COLUMN',@level2name=N'Settings'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付方式表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PaymentTypes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片类型ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoCategories', @level2type=N'COLUMN',@level2name=N'CategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoCategories', @level2type=N'COLUMN',@level2name=N'CategoryName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoCategories', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0图片库，1图标库' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoCategories', @level2type=N'COLUMN',@level2name=N'TypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片类型表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoCategories'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoGallery', @level2type=N'COLUMN',@level2name=N'PhotoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片类型ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoGallery', @level2type=N'COLUMN',@level2name=N'CategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoGallery', @level2type=N'COLUMN',@level2name=N'PhotoName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoGallery', @level2type=N'COLUMN',@level2name=N'PhotoPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoGallery', @level2type=N'COLUMN',@level2name=N'FileSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片上传时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoGallery', @level2type=N'COLUMN',@level2name=N'UploadTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoGallery', @level2type=N'COLUMN',@level2name=N'LastUpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0图片库，1图标库' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoGallery', @level2type=N'COLUMN',@level2name=N'TypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片存储表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PhotoGallery'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ֶһ�ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExchange_Changed', @level2type=N'COLUMN',@level2name=N'exChangeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExchange_Changed', @level2type=N'COLUMN',@level2name=N'exChangeName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ƷID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExchange_Changed', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ֵ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExchange_Changed', @level2type=N'COLUMN',@level2name=N'PointNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExchange_Changed', @level2type=N'COLUMN',@level2name=N'Date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ԱID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExchange_Changed', @level2type=N'COLUMN',@level2name=N'MemberID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ա�ȼ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExchange_Changed', @level2type=N'COLUMN',@level2name=N'MemberGrades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExchange_Changed'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_PointExChanges', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_PointExChanges', @level2type=N'COLUMN',@level2name=N'MemberGrades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_PointExChanges', @level2type=N'COLUMN',@level2name=N'DefualtGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????????????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_PointExChanges', @level2type=N'COLUMN',@level2name=N'CustomGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_PointExChanges', @level2type=N'COLUMN',@level2name=N'BeginDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_PointExChanges', @level2type=N'COLUMN',@level2name=N'EndDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_PointExChanges', @level2type=N'COLUMN',@level2name=N'ProductNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_PointExChanges', @level2type=N'COLUMN',@level2name=N'ImgUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_PointExChanges'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ֶһ���ƷID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_Products', @level2type=N'COLUMN',@level2name=N'exChangeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ƷID��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_Products', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'״̬' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_Products', @level2type=N'COLUMN',@level2name=N'status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_Products', @level2type=N'COLUMN',@level2name=N'ProductNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�һ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_Products', @level2type=N'COLUMN',@level2name=N'PointNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ѷһ�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_Products', @level2type=N'COLUMN',@level2name=N'ChangedNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ÿ��������0Ϊ���ޣ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_Products', @level2type=N'COLUMN',@level2name=N'eachMaxNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PointExChange_Products'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrivilegeInRoles', @level2type=N'COLUMN',@level2name=N'RoleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrivilegeInRoles', @level2type=N'COLUMN',@level2name=N'Privilege'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色权限关联表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrivilegeInRoles'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自增ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord', @level2type=N'COLUMN',@level2name=N'Receiver'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord', @level2type=N'COLUMN',@level2name=N'Tel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态0地址未填，1待发货，2已发货，3已收货' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord', @level2type=N'COLUMN',@level2name=N'status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中奖记录ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord', @level2type=N'COLUMN',@level2name=N'LogId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发货时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord', @level2type=N'COLUMN',@level2name=N'DeliveryTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货确认时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord', @level2type=N'COLUMN',@level2name=N'ReceiveTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'快递公司' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord', @level2type=N'COLUMN',@level2name=N'ExpressName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'快递编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord', @level2type=N'COLUMN',@level2name=N'CourierNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区域ID PATHS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord', @level2type=N'COLUMN',@level2name=N'ReggionPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'详细地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已经打印' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord', @level2type=N'COLUMN',@level2name=N'IsPrinted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖品发货信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PrizesDeliveryRecord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductAttributes', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductAttributes', @level2type=N'COLUMN',@level2name=N'AttributeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性值ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductAttributes', @level2type=N'COLUMN',@level2name=N'ValueId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品属性关联表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductAttributes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'咨询编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductConsultations', @level2type=N'COLUMN',@level2name=N'ConsultationId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductConsultations', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'咨询人用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductConsultations', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'咨询人用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductConsultations', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'咨询人邮件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductConsultations', @level2type=N'COLUMN',@level2name=N'UserEmail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'咨询内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductConsultations', @level2type=N'COLUMN',@level2name=N'ConsultationText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductConsultations', @level2type=N'COLUMN',@level2name=N'ConsultationDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductConsultations', @level2type=N'COLUMN',@level2name=N'ReplyText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductConsultations', @level2type=N'COLUMN',@level2name=N'ReplyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复人编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductConsultations', @level2type=N'COLUMN',@level2name=N'ReplyUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'查看日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductConsultations', @level2type=N'COLUMN',@level2name=N'ViewDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品咨询表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductConsultations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductReviews', @level2type=N'COLUMN',@level2name=N'ReviewId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductReviews', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论人用户编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductReviews', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductReviews', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户邮件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductReviews', @level2type=N'COLUMN',@level2name=N'UserEmail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductReviews', @level2type=N'COLUMN',@level2name=N'ReviewText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductReviews', @level2type=N'COLUMN',@level2name=N'ReviewDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单详情ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductReviews', @level2type=N'COLUMN',@level2name=N'OrderItemID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品评论表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductReviews'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'CategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品类型编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'TypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ProductName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ProductCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品简介' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ShortDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'SaleStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'AddedDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品访问次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'VistiCounts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品销售数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'SaleCounts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'前台商品销售数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ShowSaleCounts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ImageUrl1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ImageUrl2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ImageUrl3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ImageUrl4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ImageUrl5'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品缩略图40' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ThumbnailUrl40'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品缩略图60' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ThumbnailUrl60'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品缩略图100' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ThumbnailUrl100'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品缩略图160' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ThumbnailUrl160'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品缩略图180' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ThumbnailUrl180'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品缩略图220' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ThumbnailUrl220'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品缩略图310' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ThumbnailUrl310'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品缩略图410' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ThumbnailUrl410'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'市场价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'MarketPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品所属的品牌分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'BrandId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主分类路经' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'MainCategoryPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扩展分类路经' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ExtendCategoryPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否开启了规格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'HasSKU'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否包邮' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'IsfreeShipping'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'淘宝商品编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'TaobaoProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品来源，是自己上架的商品，还是淘宝店或其他店同步过来的' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'Source'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最低销售价格（多规格中最低价）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'MinShowPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最高销售价格（多规格中最高价）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'MaxShowPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运费模版ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'FreightTemplateId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成交店铺佣金比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'FirstCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上一级佣金比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'SecondCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上二级佣金比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ThirdCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否开启了商品分佣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'IsSetCommission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品体积，用于物流模版计算运费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'CubicMeter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品重量（kg）,用于物流模版计算运费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'FreightWeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品名称简称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products', @level2type=N'COLUMN',@level2name=N'ProductShortName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Products'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductTag', @level2type=N'COLUMN',@level2name=N'TagId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductTag', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品标签表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductTag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductTypeBrands', @level2type=N'COLUMN',@level2name=N'ProductTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductTypeBrands', @level2type=N'COLUMN',@level2name=N'BrandId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌分类表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductTypeBrands'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品类型ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductTypes', @level2type=N'COLUMN',@level2name=N'TypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品类型名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductTypes', @level2type=N'COLUMN',@level2name=N'TypeName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductTypes', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品类型表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ProductTypes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'游戏ID，自增长' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'GameId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'游戏类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'GameType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'游戏标题（活动名称）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'GameTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相关说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'游戏开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'BeginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'游戏结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'EndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'适用会员(-1表所有会员)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'ApplyMembers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消耗积分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'NeedPoint'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'参与送积分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'GivePoint'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仅送给未中装的客户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'OnlyGiveNotPrizeMember'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'参与方式（次数，一天一次，一人一次等)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'PlayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未中奖说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'NotPrzeDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生成的游戏URL地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'GameUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'二维码地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'GameQRCodeAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'游戏状态(0表正常，1表结束，-1表示删除)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每人每天限次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'LimitEveryDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每人最多限次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'MaximumDailyLimit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中奖池' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame', @level2type=N'COLUMN',@level2name=N'MemberCheck'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'抽奖游戏信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGame'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖品信息ID，自增长' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGamePrizes', @level2type=N'COLUMN',@level2name=N'PrizeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'游戏ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGamePrizes', @level2type=N'COLUMN',@level2name=N'GameId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖品等级（一等奖，二等奖）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGamePrizes', @level2type=N'COLUMN',@level2name=N'PrizeGrade'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖品类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGamePrizes', @level2type=N'COLUMN',@level2name=N'PrizeType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'赠送积分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGamePrizes', @level2type=N'COLUMN',@level2name=N'GivePoint'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'赠送优惠券' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGamePrizes', @level2type=N'COLUMN',@level2name=N'GiveCouponId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'赠送商品' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGamePrizes', @level2type=N'COLUMN',@level2name=N'GiveShopBookId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'赠送商品图片地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGamePrizes', @level2type=N'COLUMN',@level2name=N'GriveShopBookPicUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖品数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGamePrizes', @level2type=N'COLUMN',@level2name=N'PrizeCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中奖率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGamePrizes', @level2type=N'COLUMN',@level2name=N'PrizeRate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否需要配送' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGamePrizes', @level2type=N'COLUMN',@level2name=N'IsLogistics'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'游戏奖品信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGamePrizes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中将记录ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGameResultMembersLog', @level2type=N'COLUMN',@level2name=N'LogId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'游戏ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGameResultMembersLog', @level2type=N'COLUMN',@level2name=N'GameId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖品ID（0表示未中奖）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGameResultMembersLog', @level2type=N'COLUMN',@level2name=N'PrizeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGameResultMembersLog', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'玩的时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGameResultMembersLog', @level2type=N'COLUMN',@level2name=N'PlayTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖品是否领取' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGameResultMembersLog', @level2type=N'COLUMN',@level2name=N'IsUsed'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'游戏关联用户表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionGameResultMembersLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'游戏信息表编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionWinningPool', @level2type=N'COLUMN',@level2name=N'GameId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中奖号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionWinningPool', @level2type=N'COLUMN',@level2name=N'Number'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中奖奖项' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionWinningPool', @level2type=N'COLUMN',@level2name=N'GamePrizeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否领取' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_PromotionWinningPool', @level2type=N'COLUMN',@level2name=N'IsReceive'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ż�ȯID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity', @level2type=N'COLUMN',@level2name=N'CouponId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʼʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity', @level2type=N'COLUMN',@level2name=N'BeginDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity', @level2type=N'COLUMN',@level2name=N'EndDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity', @level2type=N'COLUMN',@level2name=N'MeetValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����Ż�ȯ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity', @level2type=N'COLUMN',@level2name=N'CouponNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ż�ȯ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity', @level2type=N'COLUMN',@level2name=N'CouponName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity', @level2type=N'COLUMN',@level2name=N'ActivityName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͼƬURL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity', @level2type=N'COLUMN',@level2name=N'ImgUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity', @level2type=N'COLUMN',@level2name=N'ShareTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分享优惠券ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity_Record', @level2type=N'COLUMN',@level2name=N'shareId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分享者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity_Record', @level2type=N'COLUMN',@level2name=N'shareUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'领取者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity_Record', @level2type=N'COLUMN',@level2name=N'attendUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分享助力会员领取表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShareActivity_Record'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物流地址ID自增' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Shippers', @level2type=N'COLUMN',@level2name=N'ShipperId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'留用分销商ID目前只有一个值0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Shippers', @level2type=N'COLUMN',@level2name=N'DistributorUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认地址，目前未启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Shippers', @level2type=N'COLUMN',@level2name=N'IsDefault'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址标识1发货地址、2收货地址、3收发货同一个地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Shippers', @level2type=N'COLUMN',@level2name=N'ShipperTag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Shippers', @level2type=N'COLUMN',@level2name=N'ShipperName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地区ID值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Shippers', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址详情' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Shippers', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Shippers', @level2type=N'COLUMN',@level2name=N'CellPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系电话留用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Shippers', @level2type=N'COLUMN',@level2name=N'TelPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮编，留用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Shippers', @level2type=N'COLUMN',@level2name=N'Zipcode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注留用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Shippers', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物流地址表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Shippers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShoppingCarts', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShoppingCarts', @level2type=N'COLUMN',@level2name=N'SkuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShoppingCarts', @level2type=N'COLUMN',@level2name=N'Quantity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShoppingCarts', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShoppingCarts', @level2type=N'COLUMN',@level2name=N'CategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShoppingCarts', @level2type=N'COLUMN',@level2name=N'Templateid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????? 0?????湺??1????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShoppingCarts', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????????ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShoppingCarts', @level2type=N'COLUMN',@level2name=N'ExchangeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????ID??0???????????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShoppingCarts', @level2type=N'COLUMN',@level2name=N'LimitedTimeDiscountId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_ShoppingCarts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品规格ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUItems', @level2type=N'COLUMN',@level2name=N'SkuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品属性' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUItems', @level2type=N'COLUMN',@level2name=N'AttributeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性值ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUItems', @level2type=N'COLUMN',@level2name=N'ValueId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品规格明细表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUItems'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'规格ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUMemberPrice', @level2type=N'COLUMN',@level2name=N'SkuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员等级ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUMemberPrice', @level2type=N'COLUMN',@level2name=N'GradeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员销售价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUMemberPrice', @level2type=N'COLUMN',@level2name=N'MemberSalePrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员等级价格表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUMemberPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'规格ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUs', @level2type=N'COLUMN',@level2name=N'SkuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUs', @level2type=N'COLUMN',@level2name=N'SKU'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'重量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUs', @level2type=N'COLUMN',@level2name=N'Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'库存' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUs', @level2type=N'COLUMN',@level2name=N'Stock'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'折扣价格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUs', @level2type=N'COLUMN',@level2name=N'CostPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售价格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUs', @level2type=N'COLUMN',@level2name=N'SalePrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品规格表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_SKUs'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Tags', @level2type=N'COLUMN',@level2name=N'TagID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Tags', @level2type=N'COLUMN',@level2name=N'TagName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品标签表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Tags'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配送方式ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_TemplateRelatedShipping', @level2type=N'COLUMN',@level2name=N'ModeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配送方式名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_TemplateRelatedShipping', @level2type=N'COLUMN',@level2name=N'ExpressCompanyName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配送方式表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_TemplateRelatedShipping'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_TempSendCouponUserLists', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_TempSendCouponUserLists', @level2type=N'COLUMN',@level2name=N'AdminId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手动发送优惠劵的临时存储会员表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_TempSendCouponUserLists'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'设置活动会员天数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserGroupSet', @level2type=N'COLUMN',@level2name=N'ActiveDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'设置活动会员天数表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserGroupSet'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地区ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserShippingAddresses', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserShippingAddresses', @level2type=N'COLUMN',@level2name=N'ShippingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserShippingAddresses', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserShippingAddresses', @level2type=N'COLUMN',@level2name=N'ShipTo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'详细地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserShippingAddresses', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮编' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserShippingAddresses', @level2type=N'COLUMN',@level2name=N'Zipcode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电话号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserShippingAddresses', @level2type=N'COLUMN',@level2name=N'TelPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserShippingAddresses', @level2type=N'COLUMN',@level2name=N'CellPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserShippingAddresses', @level2type=N'COLUMN',@level2name=N'IsDefault'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户收货地址表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserShippingAddresses'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserSign', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最近签到时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserSign', @level2type=N'COLUMN',@level2name=N'SignDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'连续签到天数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserSign', @level2type=N'COLUMN',@level2name=N'Continued'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'留用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserSign', @level2type=N'COLUMN',@level2name=N'Stage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户签到记录表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_UserSign'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͶƱID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_VoteItems', @level2type=N'COLUMN',@level2name=N'VoteId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͶƱѡ��ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_VoteItems', @level2type=N'COLUMN',@level2name=N'VoteItemId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͶƱ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_VoteItems', @level2type=N'COLUMN',@level2name=N'VoteItemName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͶƱѡ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_VoteItems', @level2type=N'COLUMN',@level2name=N'ItemCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_VoteItems'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_VoteRecord', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͶƱID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_VoteRecord', @level2type=N'COLUMN',@level2name=N'VoteId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_VoteRecord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Votes', @level2type=N'COLUMN',@level2name=N'VoteId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Votes', @level2type=N'COLUMN',@level2name=N'VoteName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Votes', @level2type=N'COLUMN',@level2name=N'ImageUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Votes', @level2type=N'COLUMN',@level2name=N'StartDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Votes', @level2type=N'COLUMN',@level2name=N'EndDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Votes', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Votes', @level2type=N'COLUMN',@level2name=N'MemberGrades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Votes', @level2type=N'COLUMN',@level2name=N'DefualtGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Votes', @level2type=N'COLUMN',@level2name=N'CustomGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Votes', @level2type=N'COLUMN',@level2name=N'IsMultiCheck'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hishop_Votes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��½����(�����11λ�绰����)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeiQia_Userver', @level2type=N'COLUMN',@level2name=N'unit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeiQia_Userver', @level2type=N'COLUMN',@level2name=N'userver'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeiQia_Userver', @level2type=N'COLUMN',@level2name=N'password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ͷ��ǳ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeiQia_Userver', @level2type=N'COLUMN',@level2name=N'nickname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʵ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeiQia_Userver', @level2type=N'COLUMN',@level2name=N'realname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ȼ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeiQia_Userver', @level2type=N'COLUMN',@level2name=N'level'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�绰' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeiQia_Userver', @level2type=N'COLUMN',@level2name=N'tel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeiQia_Userver'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'叶子类目id ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'Cid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新旧程度new(新)，second(二手)，unused(闲置)。' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'StuffStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'ProTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'Num'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'省' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'LocationState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'市' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'LocationCity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运费承担方式可选值:seller（卖家承担）,buyer(买家承担)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'FreightPayer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平邮费用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'PostFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'快递费用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'ExpressFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EMS费用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'EMSFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有发票' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'HasInvoice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有保修' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'HasWarranty'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支持会员打折' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'HasDiscount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'有效期,可选值:7,14;单位:天;默认值:14 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'ValidThru'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'定时上架时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'ListTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性pid:vid;pid1:vid1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'PropertyAlias'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户自行输入的类目属性ID串,结构："pid1,pid2,pid3"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'InputPids'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户自行输入的子属性名和属性值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'InputStr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SKU属性pid:vid;pid1:vid1,pid:vid;pid:vid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'SkuProperties'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SKU库存2,3,4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'SkuQuantities'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SKU价格10.00,5.00' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'SkuPrices'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SKU商品编码1234,1342' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products', @level2type=N'COLUMN',@level2name=N'SkuOuterIds'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'淘宝商品表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Taobao_Products'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Activity', @level2type=N'COLUMN',@level2name=N'ActivityId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Activity', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Activity', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Activity', @level2type=N'COLUMN',@level2name=N'StartDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Activity', @level2type=N'COLUMN',@level2name=N'EndDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关闭时备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Activity', @level2type=N'COLUMN',@level2name=N'CloseRemark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关键字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Activity', @level2type=N'COLUMN',@level2name=N'Keys'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最大值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Activity', @level2type=N'COLUMN',@level2name=N'MaxValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Activity', @level2type=N'COLUMN',@level2name=N'PicUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Activity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员签到ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ActivitySignUp', @level2type=N'COLUMN',@level2name=N'ActivitySignUpId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ActivitySignUp', @level2type=N'COLUMN',@level2name=N'ActivityId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ActivitySignUp', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ActivitySignUp', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户真实姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ActivitySignUp', @level2type=N'COLUMN',@level2name=N'RealName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'签到时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ActivitySignUp', @level2type=N'COLUMN',@level2name=N'SignUpDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会员签到表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ActivitySignUp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公众号Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AlarmNotify', @level2type=N'COLUMN',@level2name=N'AppId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'告警通知错误类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AlarmNotify', @level2type=N'COLUMN',@level2name=N'ErrorType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AlarmNotify', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误详情' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AlarmNotify', @level2type=N'COLUMN',@level2name=N'AlarmContent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'时间戳' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AlarmNotify', @level2type=N'COLUMN',@level2name=N'TimeStamp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信告警通知表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AlarmNotify'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply', @level2type=N'COLUMN',@level2name=N'ReplyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关键字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply', @level2type=N'COLUMN',@level2name=N'Keys'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'匹配类型ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply', @level2type=N'COLUMN',@level2name=N'MatchType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复类型ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply', @level2type=N'COLUMN',@level2name=N'ReplyType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'响应消息类型ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply', @level2type=N'COLUMN',@level2name=N'MessageType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开启关闭' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply', @level2type=N'COLUMN',@level2name=N'IsDisable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply', @level2type=N'COLUMN',@level2name=N'LastEditDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply', @level2type=N'COLUMN',@level2name=N'LastEditor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply', @level2type=N'COLUMN',@level2name=N'ActivityId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply', @level2type=N'COLUMN',@level2name=N'ArticleID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信回复表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_AliFuwuReply'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Article', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Article', @level2type=N'COLUMN',@level2name=N'ArticleType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Article', @level2type=N'COLUMN',@level2name=N'LinkType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Article', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Article', @level2type=N'COLUMN',@level2name=N'ImageUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Article', @level2type=N'COLUMN',@level2name=N'Url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Article', @level2type=N'COLUMN',@level2name=N'Memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Article', @level2type=N'COLUMN',@level2name=N'PubTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上传到微信中时返回的标示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Article', @level2type=N'COLUMN',@level2name=N'mediaid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信图文表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Article'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联图文上级ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ArticleItems', @level2type=N'COLUMN',@level2name=N'ArticleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ArticleItems', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ArticleItems', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ArticleItems', @level2type=N'COLUMN',@level2name=N'ImageUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ArticleItems', @level2type=N'COLUMN',@level2name=N'Url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ArticleItems', @level2type=N'COLUMN',@level2name=N'LinkType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ArticleItems', @level2type=N'COLUMN',@level2name=N'PubTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上传到微信中时返回的标示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ArticleItems', @level2type=N'COLUMN',@level2name=N'mediaid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信多图文表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ArticleItems'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自定义分组ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_CustomGrouping', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自定义分组名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_CustomGrouping', @level2type=N'COLUMN',@level2name=N'GroupName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自定义分组用户总数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_CustomGrouping', @level2type=N'COLUMN',@level2name=N'UserCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_CustomGrouping', @level2type=N'COLUMN',@level2name=N'Memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_CustomGrouping', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自定义分组ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_CustomGroupingUser', @level2type=N'COLUMN',@level2name=N'GroupId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_CustomGroupingUser', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_CustomGroupingUser', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公众号Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_FeedBackNotify', @level2type=N'COLUMN',@level2name=N'AppId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'时间戳' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_FeedBackNotify', @level2type=N'COLUMN',@level2name=N'TimeStamp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付该笔订单的用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_FeedBackNotify', @level2type=N'COLUMN',@level2name=N'OpenId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'反馈通知类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_FeedBackNotify', @level2type=N'COLUMN',@level2name=N'MsgType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'投诉单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_FeedBackNotify', @level2type=N'COLUMN',@level2name=N'FeedBackId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易订单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_FeedBackNotify', @level2type=N'COLUMN',@level2name=N'TransId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户投诉原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_FeedBackNotify', @level2type=N'COLUMN',@level2name=N'Reason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户希望的解决方案' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_FeedBackNotify', @level2type=N'COLUMN',@level2name=N'Solution'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注信息+电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_FeedBackNotify', @level2type=N'COLUMN',@level2name=N'ExtInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信反馈通知表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_FeedBackNotify'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_HomeProducts', @level2type=N'COLUMN',@level2name=N'ProductId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_HomeProducts', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首页产品表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_HomeProducts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_HomeTopics', @level2type=N'COLUMN',@level2name=N'TopicId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_HomeTopics', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'专题表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_HomeTopics'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分类型：1为收入，2为支出' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_IntegralDetail', @level2type=N'COLUMN',@level2name=N'IntegralSourceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分来源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_IntegralDetail', @level2type=N'COLUMN',@level2name=N'IntegralSource'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分变化：-1000，+1000' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_IntegralDetail', @level2type=N'COLUMN',@level2name=N'IntegralChange'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_IntegralDetail', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_IntegralDetail', @level2type=N'COLUMN',@level2name=N'Userid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分产生的URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_IntegralDetail', @level2type=N'COLUMN',@level2name=N'GoToUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_IntegralDetail', @level2type=N'COLUMN',@level2name=N'TrateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_IntegralDetail', @level2type=N'COLUMN',@level2name=N'IntegralStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分明细表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_IntegralDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信菜单ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Menu', @level2type=N'COLUMN',@level2name=N'MenuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父菜单ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Menu', @level2type=N'COLUMN',@level2name=N'ParentMenuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Menu', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Menu', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Menu', @level2type=N'COLUMN',@level2name=N'ReplyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Menu', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'绑定的关联ID号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Menu', @level2type=N'COLUMN',@level2name=N'Bind'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Menu', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信菜单表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Menu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Message', @level2type=N'COLUMN',@level2name=N'ReplyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Message', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Message', @level2type=N'COLUMN',@level2name=N'ImageUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Message', @level2type=N'COLUMN',@level2name=N'Url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Message', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Message', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信图文消息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Message'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航菜单ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'VShop_NavMenu', @level2type=N'COLUMN',@level2name=N'MenuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级菜单ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'VShop_NavMenu', @level2type=N'COLUMN',@level2name=N'ParentMenuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'VShop_NavMenu', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'VShop_NavMenu', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'VShop_NavMenu', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'VShop_NavMenu', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'VShop_NavMenu', @level2type=N'COLUMN',@level2name=N'ShopMenuPic'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航菜单表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'VShop_NavMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderRedPager', @level2type=N'COLUMN',@level2name=N'OrderID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'红包活动ID，弱关联（红包活动可能会删除）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderRedPager', @level2type=N'COLUMN',@level2name=N'RedPagerActivityId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderRedPager', @level2type=N'COLUMN',@level2name=N'RedPagerActivityName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最大获取数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderRedPager', @level2type=N'COLUMN',@level2name=N'MaxGetTimes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已经获取了多少次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderRedPager', @level2type=N'COLUMN',@level2name=N'AlreadyGetTimes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单次最高金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderRedPager', @level2type=N'COLUMN',@level2name=N'ItemAmountLimit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单金额达到使用该红包' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderRedPager', @level2type=N'COLUMN',@level2name=N'OrderAmountCanUse'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 红包领取后，多少天失效' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderRedPager', @level2type=N'COLUMN',@level2name=N'ExpiryDays'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产生红包的用户ID，扩展，用于查询用户产生的红包' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderRedPager', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'红包活动表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderRedPager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单临时拆分ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderSplit', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'待生成订单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderSplit', @level2type=N'COLUMN',@level2name=N'OrderIDNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'老的订单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderSplit', @level2type=N'COLUMN',@level2name=N'OldOrderID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单数据列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderSplit', @level2type=N'COLUMN',@level2name=N'ItemList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderSplit', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单临时拆分表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_OrderSplit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ʡ��ID��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'VShop_Region', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ʡ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'VShop_Region', @level2type=N'COLUMN',@level2name=N'RegionName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'VShop_Region'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply', @level2type=N'COLUMN',@level2name=N'ReplyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关键字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply', @level2type=N'COLUMN',@level2name=N'Keys'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'匹配类型ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply', @level2type=N'COLUMN',@level2name=N'MatchType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复类型ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply', @level2type=N'COLUMN',@level2name=N'ReplyType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'响应消息类型ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply', @level2type=N'COLUMN',@level2name=N'MessageType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开启关闭' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply', @level2type=N'COLUMN',@level2name=N'IsDisable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply', @level2type=N'COLUMN',@level2name=N'LastEditDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply', @level2type=N'COLUMN',@level2name=N'LastEditor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply', @level2type=N'COLUMN',@level2name=N'ActivityId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply', @level2type=N'COLUMN',@level2name=N'ArticleID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信回复表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Reply'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扫描日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ScanOpenID', @level2type=N'COLUMN',@level2name=N'ScanDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公众号OPenID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ScanOpenID', @level2type=N'COLUMN',@level2name=N'AppID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扫描者OpenID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ScanOpenID', @level2type=N'COLUMN',@level2name=N'SCannerUserOpenID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扫描者昵称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ScanOpenID', @level2type=N'COLUMN',@level2name=N'SCannerUserNickName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'后台扫描设置微信公众号接收者时用的临时表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_ScanOpenID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信红包记录ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_SendRedpackRecord', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请提现ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_SendRedpackRecord', @level2type=N'COLUMN',@level2name=N'BalanceDrawRequestID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_SendRedpackRecord', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'绑定微信的OpenID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_SendRedpackRecord', @level2type=N'COLUMN',@level2name=N'OpenId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信红包金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_SendRedpackRecord', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'红包名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_SendRedpackRecord', @level2type=N'COLUMN',@level2name=N'ActName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'祝福语' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_SendRedpackRecord', @level2type=N'COLUMN',@level2name=N'Wishing'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_SendRedpackRecord', @level2type=N'COLUMN',@level2name=N'ClientIP'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否发送' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_SendRedpackRecord', @level2type=N'COLUMN',@level2name=N'IsSend'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_SendRedpackRecord', @level2type=N'COLUMN',@level2name=N'SendTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送微信红包表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_SendRedpackRecord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分享说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages', @level2type=N'COLUMN',@level2name=N'ShareDesc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages', @level2type=N'COLUMN',@level2name=N'CreatTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages', @level2type=N'COLUMN',@level2name=N'image1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages', @level2type=N'COLUMN',@level2name=N'image2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages', @level2type=N'COLUMN',@level2name=N'image3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages', @level2type=N'COLUMN',@level2name=N'image4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages', @level2type=N'COLUMN',@level2name=N'image5'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages', @level2type=N'COLUMN',@level2name=N'image6'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图7' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages', @level2type=N'COLUMN',@level2name=N'image7'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图8' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages', @level2type=N'COLUMN',@level2name=N'image8'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图9' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages', @level2type=N'COLUMN',@level2name=N'image9'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'九图一文分享素材' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vshop_ShareMaterial_NineImages'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Distributors', @level2type=N'COLUMN',@level2name=N'OrderNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������۽��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Distributors', @level2type=N'COLUMN',@level2name=N'SaleAmountFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Distributors', @level2type=N'COLUMN',@level2name=N'BuyerNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�;�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Distributors', @level2type=N'COLUMN',@level2name=N'BuyerAvgPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ӷ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Distributors', @level2type=N'COLUMN',@level2name=N'CommissionAmountFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����������Ϣͳ�Ʊ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Distributors'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͳ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'RecDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'OrderNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ܽ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'SaleAmountFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'BuyerNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�;�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'UserAvgPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'FXOrderNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������ܶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'FXSaleAmountFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ҵ��ռ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'FXResultPercent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ӷ���ܶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'CommissionFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������Ա��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'NewMemberNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'NewAgentNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ɹ������ܽ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'FinishedDrawCommissionFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ȴ������ܽ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals', @level2type=N'COLUMN',@level2name=N'WaitDrawCommissionFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���̸ſ���Ϣͳ�Ʊ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Globals'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͳ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Log', @level2type=N'COLUMN',@level2name=N'RecDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�״�ͳ��ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Log', @level2type=N'COLUMN',@level2name=N'FirstCountTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͳ�ƿ�ʼʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Log', @level2type=N'COLUMN',@level2name=N'BeginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͳ�����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Log', @level2type=N'COLUMN',@level2name=N'EndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ɹ���   1=��   ����=��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Log', @level2type=N'COLUMN',@level2name=N'IsSuccess'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Log', @level2type=N'COLUMN',@level2name=N'ErrorStep'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͳ�ƽ��˵��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Log', @level2type=N'COLUMN',@level2name=N'LogInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͳ����־��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Log'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ǰ֪ͨ�������ͳ�Ƶ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_NotifyLog', @level2type=N'COLUMN',@level2name=N'RecDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�յ�֪ͨ��ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_NotifyLog', @level2type=N'COLUMN',@level2name=N'TaskTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ò���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_NotifyLog', @level2type=N'COLUMN',@level2name=N'FuncAction'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֪ͨ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_NotifyLog', @level2type=N'COLUMN',@level2name=N'ActionDesc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������Ʒ����Ա�ȱ��֪ͨ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_NotifyLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͳ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Products', @level2type=N'COLUMN',@level2name=N'RecDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʒ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Products', @level2type=N'COLUMN',@level2name=N'ProductID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Products', @level2type=N'COLUMN',@level2name=N'SaleQty'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���۽��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Products', @level2type=N'COLUMN',@level2name=N'SaleAmountFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Products', @level2type=N'COLUMN',@level2name=N'TotalVisits'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Products', @level2type=N'COLUMN',@level2name=N'BuyerNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ת����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_Products', @level2type=N'COLUMN',@level2name=N'ConversionRate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��¼����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_ProductsVisits', @level2type=N'COLUMN',@level2name=N'RecDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʒ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_ProductsVisits', @level2type=N'COLUMN',@level2name=N'ProductID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_ProductsVisits', @level2type=N'COLUMN',@level2name=N'TotalVisits'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʒÿ���������¼��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'vshop_Statistics_ProductsVisits'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Menu', @level2type=N'COLUMN',@level2name=N'MenuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级菜单ID，顶级则为0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Menu', @level2type=N'COLUMN',@level2name=N'ParentMenuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Menu', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Menu', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Menu', @level2type=N'COLUMN',@level2name=N'DisplaySequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Menu', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微博菜单表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Menu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'MessageId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'接收者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Receiver_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Sender_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Created_at'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文本消息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Text'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'语音消息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Vfid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片消息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Tovfid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'SenderMessage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'SenderDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Display_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Summary'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Image'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'多图文关联ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'ArticleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Token值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message', @level2type=N'COLUMN',@level2name=N'Access_Token'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微博消息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Message'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自动回复ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关键字ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply', @level2type=N'COLUMN',@level2name=N'ReplyKeyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文回复ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply', @level2type=N'COLUMN',@level2name=N'ArticleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否开启' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply', @level2type=N'COLUMN',@level2name=N'IsDisable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提交时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply', @level2type=N'COLUMN',@level2name=N'EditDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply', @level2type=N'COLUMN',@level2name=N'ReceiverType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply', @level2type=N'COLUMN',@level2name=N'Displayname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply', @level2type=N'COLUMN',@level2name=N'Summary'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply', @level2type=N'COLUMN',@level2name=N'Image'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply', @level2type=N'COLUMN',@level2name=N'Url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微博自动回复详细表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_Reply'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关键字回复ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_ReplyKeys', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关键字名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_ReplyKeys', @level2type=N'COLUMN',@level2name=N'Keys'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关键字类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_ReplyKeys', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关键字查询方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_ReplyKeys', @level2type=N'COLUMN',@level2name=N'Matching'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微博自动回复表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Weibo_ReplyKeys'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户OpenID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_RecentOpenID', @level2type=N'COLUMN',@level2name=N'OpenID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最近访问公众号的用户表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_RecentOpenID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_SendAll', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_SendAll', @level2type=N'COLUMN',@level2name=N'MessageType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图文ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_SendAll', @level2type=N'COLUMN',@level2name=N'ArticleID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_SendAll', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_SendAll', @level2type=N'COLUMN',@level2name=N'SendState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_SendAll', @level2type=N'COLUMN',@level2name=N'SendTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送统计' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_SendAll', @level2type=N'COLUMN',@level2name=N'SendCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返回消息ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_SendAll', @level2type=N'COLUMN',@level2name=N'msgid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返回的总数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_SendAll', @level2type=N'COLUMN',@level2name=N'TotalCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返回Json数据' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_SendAll', @level2type=N'COLUMN',@level2name=N'ReturnJsonData'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信客服接口回复表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeiXin_SendAll'

