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
EXECUTE sp_rename N'dbo.TET_Supplier.KeySuppiler', N'Tmp_KeySupplier', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_Supplier.Tmp_KeySupplier', N'KeySupplier', 'COLUMN' 
GO
ALTER TABLE dbo.TET_Supplier SET (LOCK_ESCALATION = TABLE)
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
ALTER TABLE dbo.Modules
	DROP CONSTRAINT DF__Modules__ID__02084FDA
GO
ALTER TABLE dbo.Modules
	DROP CONSTRAINT DF__Modules__CreateD__02FC7413
GO
CREATE TABLE dbo.Tmp_Modules
	(
	ID uniqueidentifier NOT NULL,
	Name nvarchar(50) NOT NULL,
	Controller varchar(100) NOT NULL,
	Action varchar(100) NOT NULL,
	AdminController varchar(100) NULL,
	AdminAction varchar(100) NULL,
	CreateUser nvarchar(50) NOT NULL,
	CreateDate datetime NOT NULL,
	ModifyUser nvarchar(50) NULL,
	ModifyDate datetime NULL,
	DeleteUser nvarchar(50) NULL,
	DeleteDate datetime NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Modules SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Modules ADD CONSTRAINT
	DF__Modules__ID__02084FDA DEFAULT (newid()) FOR ID
GO
ALTER TABLE dbo.Tmp_Modules ADD CONSTRAINT
	DF__Modules__CreateD__02FC7413 DEFAULT (getdate()) FOR CreateDate
GO
IF EXISTS(SELECT * FROM dbo.Modules)
	 EXEC('INSERT INTO dbo.Tmp_Modules (ID, Name, Controller, Action, AdminController, AdminAction, CreateUser, CreateDate, ModifyUser, ModifyDate, DeleteUser, DeleteDate)
		SELECT ID, CONVERT(nvarchar(50), Name), Controller, Action, AdminController, AdminAction, CreateUser, CreateDate, ModifyUser, ModifyDate, DeleteUser, DeleteDate FROM dbo.Modules WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.Modules
GO
EXECUTE sp_rename N'dbo.Tmp_Modules', N'Modules', 'OBJECT' 
GO
ALTER TABLE dbo.Modules ADD CONSTRAINT
	PK_Modules_ID PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Modules ADD CONSTRAINT
	UK_Modules_ID UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
