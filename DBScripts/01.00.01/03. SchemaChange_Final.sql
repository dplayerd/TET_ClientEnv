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
ALTER TABLE dbo.TET_Supplier_UserRoles
	DROP CONSTRAINT DF_TET_Supplier_UserRoles_ID
GO
CREATE TABLE dbo.Tmp_TET_Supplier_UserRoles
	(
	ID uniqueidentifier NOT NULL,
	RoleID uniqueidentifier NOT NULL,
	UserID nvarchar(64) NOT NULL,
	CreateUser nvarchar(64) NOT NULL,
	CreateDate datetime NOT NULL,
	ModifyUser nvarchar(64) NOT NULL,
	ModifyDate datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TET_Supplier_UserRoles SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_TET_Supplier_UserRoles ADD CONSTRAINT
	DF_TET_Supplier_UserRoles_ID DEFAULT (newid()) FOR ID
GO
IF EXISTS(SELECT * FROM dbo.TET_Supplier_UserRoles)
	 EXEC('INSERT INTO dbo.Tmp_TET_Supplier_UserRoles (ID, RoleID, UserID, CreateUser, CreateDate, ModifyUser, ModifyDate)
		SELECT ID, RoleID, UserID, CreateUser, CreateDate, ModifyUser, ModifyDate FROM dbo.TET_Supplier_UserRoles WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TET_Supplier_UserRoles
GO
EXECUTE sp_rename N'dbo.Tmp_TET_Supplier_UserRoles', N'TET_Supplier_UserRoles', 'OBJECT' 
GO
ALTER TABLE dbo.TET_Supplier_UserRoles ADD CONSTRAINT
	PK_TET_Supplier_UserRoles PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
