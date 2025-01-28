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
CREATE TABLE dbo.Tmp_TET_SupplierApproval
	(
	ID uniqueidentifier NOT NULL,
	SupplierID uniqueidentifier NOT NULL,
	Type nvarchar(64) NOT NULL,
	Description nvarchar(256) NOT NULL,
	[Level] nvarchar(32) NOT NULL,
	Approver nvarchar(64) NOT NULL,
	Result nvarchar(16) NULL,
	Comment nvarchar(256) NULL,
	CreateUser nvarchar(64) NOT NULL,
	CreateDate datetime NOT NULL,
	ModifyUser nvarchar(64) NOT NULL,
	ModifyDate datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TET_SupplierApproval SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.TET_SupplierApproval)
	 EXEC('INSERT INTO dbo.Tmp_TET_SupplierApproval (ID, SupplierID, Type, Description, [Level], Approver, Result, Comment, CreateUser, CreateDate, ModifyUser, ModifyDate)
		SELECT ID, SupplierID, Type, Description, [Level], Approver, Result, Comment, CreateUser, CreateDate, ModifyUser, ModifyDate FROM dbo.TET_SupplierApproval WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TET_SupplierApproval
GO
EXECUTE sp_rename N'dbo.Tmp_TET_SupplierApproval', N'TET_SupplierApproval', 'OBJECT' 
GO
ALTER TABLE dbo.TET_SupplierApproval ADD CONSTRAINT
	PK_TET_SupplierApproval PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
