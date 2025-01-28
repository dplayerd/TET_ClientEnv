/* 為了避免任何可能發生資料遺失的問題，您應該先詳細檢視此指令碼，然後才能在資料庫設計工具環境以外的位置執行。*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_TET_Supplier_Menu
	(
	ID uniqueidentifier NOT NULL,
	SiteID uniqueidentifier NULL,
	ParentID uniqueidentifier NULL,
	Name nvarchar(64) NOT NULL,
	Description nvarchar(128) NULL,
	MenuType tinyint NULL,
	Linkurl nvarchar(128) NOT NULL,
	ModuleID uniqueidentifier NULL,
	PageIcon nvarchar(128) NULL,
	SortNo int NOT NULL,
	IsEnable bit NULL,
	CreateUser nvarchar(64) NULL,
	CreateDate datetime NULL,
	ModifyUser nvarchar(64) NULL,
	ModifyDate datetime NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TET_Supplier_Menu SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_TET_Supplier_Menu ADD CONSTRAINT
	DF_TET_Supplier_Menu_CreateDate DEFAULT (getdate()) FOR CreateDate
GO
IF EXISTS(SELECT * FROM dbo.TET_Supplier_Menu)
	 EXEC('INSERT INTO dbo.Tmp_TET_Supplier_Menu (ID, Name, Description, Linkurl, PageIcon, SortNo)
		SELECT Guid, Name, Description, Linkurl, Iconurl, Seq FROM dbo.TET_Supplier_Menu WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TET_Supplier_Menu
GO
EXECUTE sp_rename N'dbo.Tmp_TET_Supplier_Menu', N'TET_Supplier_Menu', 'OBJECT' 
GO
ALTER TABLE dbo.TET_Supplier_Menu ADD CONSTRAINT
	PK_TET_Supplier_Menu PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT



/* 為了避免任何可能發生資料遺失的問題，您應該先詳細檢視此指令碼，然後才能在資料庫設計工具環境以外的位置執行。*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_TET_Supplier_RoleMenu
	(
	ID uniqueidentifier NOT NULL,
	MenuID uniqueidentifier NOT NULL,
	RoleID uniqueidentifier NOT NULL,
	AllowActs tinyint NULL,
	CreateUser nvarchar(64) NOT NULL,
	CreateDate datetime NOT NULL,
	ModifyUser nvarchar(64) NULL,
	ModifyDate datetime NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TET_Supplier_RoleMenu SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_TET_Supplier_RoleMenu ADD CONSTRAINT
	DF_TET_Supplier_RoleMenu_ID DEFAULT (newid()) FOR ID
GO
IF EXISTS(SELECT * FROM dbo.TET_Supplier_RoleMenu)
	 EXEC('INSERT INTO dbo.Tmp_TET_Supplier_RoleMenu (MenuID, RoleID, CreateUser, CreateDate)
		SELECT MenuID, RoleID, LastModifiedBy, LastModifiedTime FROM dbo.TET_Supplier_RoleMenu WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TET_Supplier_RoleMenu
GO
EXECUTE sp_rename N'dbo.Tmp_TET_Supplier_RoleMenu', N'TET_Supplier_RoleMenu', 'OBJECT' 
GO
ALTER TABLE dbo.TET_Supplier_RoleMenu ADD CONSTRAINT
	PK_TET_Supplier_RoleMenu PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
