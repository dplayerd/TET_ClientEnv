USE [TET_Supplier]
GO
/****** Object:  Table [dbo].[AdminLogs]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminLogs](
	[SeqNo] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceID] [varchar](200) NOT NULL,
	[Title] [nvarchar](300) NULL,
	[AccessType] [tinyint] NULL,
	[AccessName] [char](50) NULL,
	[Message] [nvarchar](max) NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[ModuleID] [uniqueidentifier] NULL,
	[CreateUser] [uniqueidentifier] NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_AdminLogs_SeqNo] PRIMARY KEY CLUSTERED 
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[SeqNo] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceID] [varchar](200) NULL,
	[LogLevel] [tinyint] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Exceptions] [nvarchar](max) NULL,
	[UserID] [uniqueidentifier] NULL,
	[SiteID] [uniqueidentifier] NULL,
	[ModuleID] [uniqueidentifier] NULL,
	[PageID] [uniqueidentifier] NULL,
	[CreateUser] [uniqueidentifier] NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Logs_SeqNo] PRIMARY KEY CLUSTERED 
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MediaFileRoles]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaFileRoles](
	[ID] [uniqueidentifier] NOT NULL,
	[MediaFileID] [uniqueidentifier] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [uniqueidentifier] NULL,
	[ModifyDate] [datetime] NULL,
	[DeleteUser] [uniqueidentifier] NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK_MediaFileRoles_MediaFileIDRoleID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MediaFiles]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaFiles](
	[SeqNo] [int] IDENTITY(1,1) NOT NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[ModuleName] [varchar](50) NOT NULL,
	[ModuleID] [varchar](200) NOT NULL,
	[Purpose] [varchar](50) NOT NULL,
	[FilePath] [varchar](200) NOT NULL,
	[OrgFileName] [nvarchar](300) NOT NULL,
	[OutputFileName] [nvarchar](300) NOT NULL,
	[MimeType] [varchar](100) NOT NULL,
	[RequireAuth] [bit] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [uniqueidentifier] NULL,
	[ModifyDate] [datetime] NULL,
	[DeleteUser] [uniqueidentifier] NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK_MediaFiles_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modules]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modules](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Controller] [varchar](100) NOT NULL,
	[Action] [varchar](100) NOT NULL,
	[AdminController] [varchar](100) NULL,
	[AdminAction] [varchar](100) NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [uniqueidentifier] NULL,
	[ModifyDate] [datetime] NULL,
	[DeleteUser] [uniqueidentifier] NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK_Modules_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_Modules_ID] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PageFunctionRoles]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PageFunctionRoles](
	[ID] [uniqueidentifier] NOT NULL,
	[PageID] [uniqueidentifier] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
	[FunctionCode] [varchar](20) NOT NULL,
	[IsAllow] [bit] NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [uniqueidentifier] NULL,
	[ModifyDate] [datetime] NULL,
	[DeleteUser] [uniqueidentifier] NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK_PageFunctionRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PageRoles]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PageRoles](
	[ID] [uniqueidentifier] NOT NULL,
	[PageID] [uniqueidentifier] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
	[AllowActs] [tinyint] NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [uniqueidentifier] NULL,
	[ModifyDate] [datetime] NULL,
	[DeleteUser] [uniqueidentifier] NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK_PageRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pages]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pages](
	[SeqNo] [int] IDENTITY(1,1) NOT NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,
	[ParentID] [uniqueidentifier] NULL,
	[Name] [varchar](50) NOT NULL,
	[PageNo] [varchar](50) NOT NULL,
	[PageTitle] [nvarchar](100) NOT NULL,
	[IsShowOnHeader] [bit] NOT NULL,
	[IsShowOnFooter] [bit] NOT NULL,
	[MenuType] [tinyint] NOT NULL,
	[OuterLink] [nvarchar](500) NULL,
	[ModuleID] [uniqueidentifier] NULL,
	[Hits] [int] NOT NULL,
	[PageIcon] [varchar](50) NULL,
	[SortNo] [int] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [uniqueidentifier] NULL,
	[ModifyDate] [datetime] NULL,
	[DeleteUser] [uniqueidentifier] NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK_Pages_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sites]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sites](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[MediaFileID] [uniqueidentifier] NULL,
	[HeaderText] [nvarchar](300) NOT NULL,
	[FooterText] [nvarchar](300) NOT NULL,
 CONSTRAINT [PK_Sites_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_Sites_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemUsers]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUsers](
	[ID] [uniqueidentifier] NOT NULL,
	[Account] [varchar](200) NOT NULL,
	[Password] [varchar](128) NOT NULL,
	[HashKey] [varchar](128) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](200) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[MediaFileID] [uniqueidentifier] NULL,
	[IsEnable] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [uniqueidentifier] NULL,
	[ModifyDate] [datetime] NULL,
	[DeleteUser] [uniqueidentifier] NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK_SystemUsers_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_Parameters]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_Parameters](
	[ID] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](64) NOT NULL,
	[Item] [nvarchar](64) NOT NULL,
	[Seq] [int] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_Parameters] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_Supplier]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_Supplier](
	[ID] [uniqueidentifier] NOT NULL,
	[IsSecret] [nvarchar](8) NOT NULL,
	[IsNDA] [nvarchar](32) NOT NULL,
	[ApplyReason] [nvarchar](512) NULL,
	[BelongTo] [nvarchar](128) NULL,
	[VenderCode] [nvarchar](32) NULL,
	[SupplierCategory] [nvarchar](512) NOT NULL,
	[BusinessCategory] [nvarchar](512) NOT NULL,
	[BusinessAttribute] [nvarchar](512) NOT NULL,
	[SearchKey] [nvarchar](256) NULL,
	[RelatedDept] [nvarchar](256) NULL,
	[Buyer] [nvarchar](64) NULL,
	[RegisterDate] [date] NULL,
	[CName] [nvarchar](128) NOT NULL,
	[EName] [nvarchar](128) NOT NULL,
	[Country] [nvarchar](64) NOT NULL,
	[TaxNo] [nvarchar](16) NOT NULL,
	[Address] [nvarchar](128) NOT NULL,
	[OfficeTel] [nvarchar](32) NOT NULL,
	[ISO] [nvarchar](128) NULL,
	[Email] [nvarchar](128) NULL,
	[Website] [nvarchar](128) NULL,
	[CapitalAmount] [nvarchar](32) NOT NULL,
	[MainProduct] [nvarchar](512) NOT NULL,
	[Employees] [nvarchar](32) NULL,
	[Charge] [nvarchar](32) NOT NULL,
	[PaymentTerm] [nvarchar](64) NOT NULL,
	[BillingDocument] [nvarchar](64) NOT NULL,
	[Incoterms] [nvarchar](64) NOT NULL,
	[Remark] [nvarchar](512) NULL,
	[BankCountry] [nvarchar](32) NOT NULL,
	[BankName] [nvarchar](128) NOT NULL,
	[BankCode] [nvarchar](16) NOT NULL,
	[BankBranchName] [nvarchar](128) NOT NULL,
	[BankBranchCode] [nvarchar](16) NOT NULL,
	[Currency] [nvarchar](64) NOT NULL,
	[BankAccountName] [nvarchar](64) NOT NULL,
	[BankAccountNo] [nvarchar](32) NOT NULL,
	[CompanyCity] [nvarchar](32) NULL,
	[BankAddress] [nvarchar](128) NULL,
	[SwiftCode] [nvarchar](32) NULL,
	[NDANo] [nvarchar](32) NULL,
	[Contract] [nvarchar](1) NULL,
	[IsSign1] [nvarchar](32) NULL,
	[SignDate1] [date] NULL,
	[IsSign2] [nvarchar](32) NULL,
	[SignDate2] [date] NULL,
	[STQAApplication] [nvarchar](32) NULL,
	[KeySuppiler] [nvarchar](1) NULL,
	[Version] [int] NOT NULL,
	[RevisionType] [nvarchar](1) NULL,
	[IsLastVersion] [nvarchar](1) NULL,
	[ApproveStatus] [nvarchar](32) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_Supplier] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_Supplier_Menu]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_Supplier_Menu](
	[Guid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](128) NULL,
	[Linkurl] [nvarchar](128) NOT NULL,
	[Iconurl] [nvarchar](128) NULL,
	[Seq] [int] NOT NULL,
 CONSTRAINT [PK_TET_Supplier_Menu] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_Supplier_RoleMenu]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_Supplier_RoleMenu](
	[MenuID] [uniqueidentifier] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
	[LastModifiedBy] [nvarchar](64) NOT NULL,
	[LastModifiedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_Supplier_RoleMenu] PRIMARY KEY CLUSTERED 
(
	[MenuID] ASC,
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_Supplier_Roles]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_Supplier_Roles](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[IsEnable] [bit] NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_Supplier_Roles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_Supplier_UserRoles]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_Supplier_UserRoles](
	[RoleID] [uniqueidentifier] NOT NULL,
	[UserID] [nvarchar](64) NOT NULL,
	[LastModifiedBy] [nvarchar](64) NOT NULL,
	[LastModifiedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_Supplier_UserRoles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SupplierApproval]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SupplierApproval](
	[ID] [uniqueidentifier] NOT NULL,
	[SupplierID] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](64) NOT NULL,
	[Level] [nvarchar](32) NOT NULL,
	[Approver] [nvarchar](64) NOT NULL,
	[Result] [nvarchar](16) NULL,
	[Comment] [nvarchar](256) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SupplierApproval] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SupplierAttachments]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SupplierAttachments](
	[ID] [uniqueidentifier] NOT NULL,
	[SupplierID] [uniqueidentifier] NOT NULL,
	[FilePath] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](128) NOT NULL,
	[OrgFileName] [nvarchar](128) NOT NULL,
	[FileExtension] [nvarchar](32) NOT NULL,
	[FileSize] [int] NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SupplierAttachments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SupplierContact]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SupplierContact](
	[ID] [uniqueidentifier] NOT NULL,
	[SupplierID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Title] [nvarchar](64) NULL,
	[Tel] [nvarchar](32) NOT NULL,
	[Email] [nvarchar](32) NULL,
	[Remark] [nvarchar](128) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SupplierContact] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SupplierSPA]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SupplierSPA](
	[Guid] [uniqueidentifier] NOT NULL,
	[BelongTo] [nvarchar](128) NOT NULL,
	[Period] [nvarchar](32) NOT NULL,
	[BU] [nvarchar](64) NOT NULL,
	[ServiceFor] [nvarchar](64) NOT NULL,
	[AssessmentItem] [nvarchar](64) NOT NULL,
	[PerformanceLevel] [nvarchar](16) NOT NULL,
	[TotalScore] [nvarchar](16) NOT NULL,
	[TScore] [nvarchar](16) NOT NULL,
	[DScore] [nvarchar](16) NOT NULL,
	[QScore] [nvarchar](16) NOT NULL,
	[CScore] [nvarchar](16) NOT NULL,
	[SScore] [nvarchar](16) NOT NULL,
	[Comment] [nvarchar](512) NULL,
	[ApproveStatus] [nvarchar](32) NULL,
	[CreatedBy] [nvarchar](64) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[LastModifiedBy] [nvarchar](64) NOT NULL,
	[LastModifiedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SupplierSPA] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SupplierSPAApproval]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SupplierSPAApproval](
	[Guid] [uniqueidentifier] NOT NULL,
	[SPAID] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](64) NOT NULL,
	[Level] [nvarchar](32) NOT NULL,
	[Approver] [nvarchar](64) NOT NULL,
	[Result] [nvarchar](16) NULL,
	[Comment] [nvarchar](256) NULL,
	[CreatedBy] [nvarchar](64) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[LastModifiedBy] [nvarchar](64) NOT NULL,
	[LastModifiedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SupplierSPAApproval] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SupplierSTQA]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SupplierSTQA](
	[Guid] [uniqueidentifier] NOT NULL,
	[BelongTo] [nvarchar](128) NOT NULL,
	[Purpose] [nvarchar](64) NOT NULL,
	[BusinessTerm] [nvarchar](64) NOT NULL,
	[Date] [nvarchar](64) NOT NULL,
	[Type] [nvarchar](64) NOT NULL,
	[UnitALevel] [nvarchar](64) NOT NULL,
	[UnitCLevel] [nvarchar](64) NOT NULL,
	[UnitDLevel] [nvarchar](64) NOT NULL,
	[Comment] [nvarchar](512) NULL,
	[ApproveStatus] [nvarchar](32) NULL,
	[CreatedBy] [nvarchar](64) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[LastModifiedBy] [nvarchar](64) NOT NULL,
	[LastModifiedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SupplierSTQA] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SupplierSTQAApproval]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SupplierSTQAApproval](
	[Guid] [uniqueidentifier] NOT NULL,
	[STQAID] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](64) NOT NULL,
	[Level] [nvarchar](32) NOT NULL,
	[Approver] [nvarchar](64) NOT NULL,
	[Result] [nvarchar](16) NULL,
	[Comment] [nvarchar](256) NULL,
	[CreatedBy] [nvarchar](64) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[LastModifiedBy] [nvarchar](64) NOT NULL,
	[LastModifiedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SupplierSTQAApproval] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SupplierTrade]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SupplierTrade](
	[SubpoenaNo] [nvarchar](64) NOT NULL,
	[SubpoenaDate] [datetime] NOT NULL,
	[VenderCode] [nvarchar](32) NOT NULL,
	[Currency] [nvarchar](64) NOT NULL,
	[Amount] [decimal](12, 2) NOT NULL,
	[CreatedBy] [nvarchar](64) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[LastModifiedBy] [nvarchar](64) NOT NULL,
	[LastModifiedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SupplierTrade] PRIMARY KEY CLUSTERED 
(
	[SubpoenaNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLoginRecords]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLoginRecords](
	[SeqNo] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NULL,
	[Account] [varchar](200) NULL,
	[UserIP] [char](50) NOT NULL,
	[IsSuccess] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_UserLoginRecords_SeqNo] PRIMARY KEY CLUSTERED 
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPasswordRecords]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPasswordRecords](
	[SeqNo] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[Password] [char](128) NOT NULL,
	[Salt] [char](128) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_UserPasswordRecords_SeqNo] PRIMARY KEY CLUSTERED 
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[ID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [uniqueidentifier] NULL,
	[ModifyDate] [datetime] NULL,
	[DeleteUser] [uniqueidentifier] NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK_UserRoles_UserIDRoleID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [nvarchar](64) NOT NULL,
	[EmpID] [nvarchar](64) NOT NULL,
	[FirstNameEN] [nvarchar](64) NOT NULL,
	[LastNameEN] [nvarchar](64) NOT NULL,
	[FirstNameCH] [nvarchar](64) NOT NULL,
	[LastNameCH] [nvarchar](64) NOT NULL,
	[UnitCode] [nvarchar](64) NOT NULL,
	[UnitName] [nvarchar](128) NOT NULL,
	[LeaderID] [nvarchar](64) NOT NULL,
	[EMail] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendors]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendors](
	[SeqNo] [int] IDENTITY(1,1) NOT NULL,
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Code] [varchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[IsEnable] [bit] NOT NULL,
	[CreateUser] [uniqueidentifier] NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [uniqueidentifier] NULL,
	[ModifyDate] [datetime] NULL,
	[DeleteUser] [uniqueidentifier] NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK__Vendors__3214EC27EFC4DCBD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vwPageLevelCTE]    Script Date: 2024/1/9 上午 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwPageLevelCTE]
AS
WITH PageLevelCTE AS (SELECT   ID, ParentID, SiteID, 0 AS Depth, Name, CreateDate, IsEnable, MenuType
                                 FROM      dbo.Pages
                                 WHERE    (ParentID IS NULL)
                                 UNION ALL
                                 SELECT   ChildFunction.ID, ChildFunction.ParentID, ChildFunction.SiteID, PageLevelCTE_2.Depth + 1 AS Expr1, ChildFunction.Name, ChildFunction.CreateDate, ChildFunction.IsEnable, 
                                               ChildFunction.MenuType
                                 FROM     dbo.Pages AS ChildFunction INNER JOIN
                                               PageLevelCTE AS PageLevelCTE_2 ON ChildFunction.ParentID = PageLevelCTE_2.ID)
    SELECT   TOP (100) PERCENT Depth, ID, ParentID, SiteID, REPLICATE('\', Depth) + Name AS Caption, Name AS OrgCaption, CreateDate, IsEnable, MenuType
  FROM      PageLevelCTE AS PageLevelCTE_1
  ORDER BY Depth, CreateDate DESC
GO
ALTER TABLE [dbo].[AdminLogs] ADD  CONSTRAINT [DF_AdminLogs_b_date]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Logs] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[MediaFileRoles] ADD  CONSTRAINT [DF_MediaFileRoles_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[MediaFileRoles] ADD  CONSTRAINT [DF__MediaFile__Creat__7E37BEF6]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[MediaFiles] ADD  CONSTRAINT [DF__MediaFiles__ID__70DDC3D8]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[MediaFiles] ADD  CONSTRAINT [DF__MediaFile__b_dat__71D1E811]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Modules] ADD  CONSTRAINT [DF__Modules__ID__02084FDA]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Modules] ADD  CONSTRAINT [DF__Modules__CreateD__02FC7413]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[PageFunctionRoles] ADD  CONSTRAINT [DF_PageFunctionRoles_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[PageFunctionRoles] ADD  CONSTRAINT [DF_PageFunctionRoles_b_date]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[PageRoles] ADD  CONSTRAINT [DF_PageRoles_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[PageRoles] ADD  CONSTRAINT [DF_PageRoles_b_date]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Pages] ADD  CONSTRAINT [DF__Pages__ID__6D0D32F4]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Pages] ADD  CONSTRAINT [DF__Pages__CreateDat__6E01572D]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Sites] ADD  CONSTRAINT [DF__Sites__ID__71D1E811]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[SystemUsers] ADD  CONSTRAINT [DF__SystemUsers__ID__10566F31]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[SystemUsers] ADD  CONSTRAINT [DF__SystemUsers__CreateDa__114A936A]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[TET_Parameters] ADD  CONSTRAINT [DF_TET_Parameters_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[UserLoginRecords] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[UserPasswordRecords] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [DF_UserRoles_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [DF__UserRoles__Creat__693CA210]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Vendors] ADD  CONSTRAINT [DF_Vendors_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Vendors] ADD  CONSTRAINT [DF_Vendors_b_date]  DEFAULT (getdate()) FOR [CreateDate]
GO
