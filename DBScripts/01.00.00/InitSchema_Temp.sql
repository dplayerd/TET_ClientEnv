USE [TELDev]
GO
/****** Object:  Table [dbo].[AdminLogs] ******/
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
/****** Object:  Table [dbo].[Logs]    Script Date: 2023/12/19 下午 09:32:56 ******/
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
/****** Object:  Table [dbo].[MediaFileRoles]    Script Date: 2023/12/19 下午 09:32:56 ******/
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
/****** Object:  Table [dbo].[MediaFiles]    Script Date: 2023/12/19 下午 09:32:56 ******/
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
/****** Object:  Table [dbo].[Modules]    Script Date: 2023/12/19 下午 09:32:56 ******/
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
/****** Object:  Table [dbo].[PageFunctionRoles]    Script Date: 2023/12/19 下午 09:32:56 ******/
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
/****** Object:  Table [dbo].[PageRoles]    Script Date: 2023/12/19 下午 09:32:56 ******/
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
/****** Object:  Table [dbo].[Pages]    Script Date: 2023/12/19 下午 09:32:56 ******/
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
/****** Object:  Table [dbo].[Roles]    Script Date: 2023/12/19 下午 09:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](100) NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [uniqueidentifier] NULL,
	[ModifyDate] [datetime] NULL,
	[DeleteUser] [uniqueidentifier] NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK_Roles_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sites]    Script Date: 2023/12/19 下午 09:32:56 ******/
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
/****** Object:  Table [dbo].[UserLoginRecords]    Script Date: 2023/12/19 下午 09:32:56 ******/
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
/****** Object:  Table [dbo].[UserPasswordRecords]    Script Date: 2023/12/19 下午 09:32:56 ******/
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
/****** Object:  Table [dbo].[UserRoles]    Script Date: 2023/12/19 下午 09:32:56 ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 2023/12/19 下午 09:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
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
 CONSTRAINT [PK_Users_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendors]    Script Date: 2023/12/19 下午 09:32:56 ******/
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
ALTER TABLE [dbo].[Roles] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Roles] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Sites] ADD  CONSTRAINT [DF__Sites__ID__71D1E811]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[UserLoginRecords] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[UserPasswordRecords] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [DF_UserRoles_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [DF__UserRoles__Creat__693CA210]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Vendors] ADD  CONSTRAINT [DF_Vendors_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Vendors] ADD  CONSTRAINT [DF_Vendors_b_date]  DEFAULT (getdate()) FOR [CreateDate]
GO
