USE [UBayPlatform]
GO
/****** Object:  Table [dbo].[AdminLogs]    Script Date: 2021/11/8 上午 08:54:47 ******/
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
/****** Object:  Table [dbo].[Logs]    Script Date: 2021/11/8 上午 08:54:47 ******/
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
/****** Object:  Table [dbo].[MediaFileRoles]    Script Date: 2021/11/8 上午 08:54:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaFileRoles](
	[MediaFileID] [uniqueidentifier] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
	[CreateUser] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_MediaFileRoles_MediaFileIDRoleID] PRIMARY KEY CLUSTERED 
(
	[MediaFileID] ASC,
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MediaFiles]    Script Date: 2021/11/8 上午 08:54:47 ******/
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
	[IsDeleted] [bit] NOT NULL,
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
/****** Object:  Table [dbo].[Modules]    Script Date: 2021/11/8 上午 08:54:47 ******/
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
/****** Object:  Table [dbo].[Pages]    Script Date: 2021/11/8 上午 08:54:47 ******/
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
	[PageTitle] [nvarchar](100) NOT NULL,
	[IsShowOnHeader] [bit] NOT NULL,
	[IsShowOnFooter] [bit] NOT NULL,
	[MenuType] [tinyint] NOT NULL,
	[OuterLink] [nvarchar](500) NULL,
	[ModuleID] [uniqueidentifier] NULL,
	[Hits] [int] NOT NULL,
	[PageIcon] [varchar](50) NULL,
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
/****** Object:  Table [dbo].[Roles]    Script Date: 2021/11/8 上午 08:54:47 ******/
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
/****** Object:  Table [dbo].[Sites]    Script Date: 2021/11/8 上午 08:54:47 ******/
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
/****** Object:  Table [dbo].[UserLoginRecords]    Script Date: 2021/11/8 上午 08:54:47 ******/
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
/****** Object:  Table [dbo].[UserPasswordRecords]    Script Date: 2021/11/8 上午 08:54:47 ******/
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
/****** Object:  Table [dbo].[UserRoles]    Script Date: 2021/11/8 上午 08:54:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
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
	[UserID] ASC,
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2021/11/8 上午 08:54:47 ******/
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
/****** Object:  View [dbo].[vwPageLevelCTE]    Script Date: 2021/11/8 上午 08:54:47 ******/
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
ALTER TABLE [dbo].[Logs] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[MediaFileRoles] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[MediaFiles] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[MediaFiles] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Modules] ADD  CONSTRAINT [DF__Modules__ID__02084FDA]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Modules] ADD  CONSTRAINT [DF__Modules__CreateD__02FC7413]  DEFAULT (getdate()) FOR [CreateDate]
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
ALTER TABLE [dbo].[UserRoles] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[24] 4[14] 2[29] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "PageLevelCTE_1"
            Begin Extent = 
               Top = 12
               Left = 76
               Bottom = 249
               Right = 360
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 750
         Width = 750
         Width = 750
         Width = 750
         Width = 750
         Width = 750
         Width = 750
         Width = 750
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwPageLevelCTE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwPageLevelCTE'
GO
