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
CREATE TABLE dbo.Tmp_TET_SPA_Period
	(
	ID uniqueidentifier NOT NULL,
	Period nvarchar(16) NOT NULL,
	Status nvarchar(16) NOT NULL,
	CreateUser nvarchar(64) NOT NULL,
	CreateDate datetime NOT NULL,
	ModifyUser nvarchar(64) NOT NULL,
	ModifyDate datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TET_SPA_Period SET (LOCK_ESCALATION = TABLE)
GO
DECLARE @v sql_variant 
SET @v = N'評鑑期間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_Period', N'COLUMN', N'Period'
GO
DECLARE @v sql_variant 
SET @v = N'評鑑狀態'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_Period', N'COLUMN', N'Status'
GO
DECLARE @v sql_variant 
SET @v = N'建立人員'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_Period', N'COLUMN', N'CreateUser'
GO
DECLARE @v sql_variant 
SET @v = N'新增時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_Period', N'COLUMN', N'CreateDate'
GO
DECLARE @v sql_variant 
SET @v = N'最後更新人員'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_Period', N'COLUMN', N'ModifyUser'
GO
DECLARE @v sql_variant 
SET @v = N'最後更新時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_Period', N'COLUMN', N'ModifyDate'
GO
IF EXISTS(SELECT * FROM dbo.TET_SPA_Period)
	 EXEC('INSERT INTO dbo.Tmp_TET_SPA_Period (Period, Status, CreateUser, CreateDate, ModifyUser, ModifyDate)
		SELECT Period, Status, CreateUser, CreateDate, ModifyUser, ModifyDate FROM dbo.TET_SPA_Period WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TET_SPA_Period
GO
EXECUTE sp_rename N'dbo.Tmp_TET_SPA_Period', N'TET_SPA_Period', 'OBJECT' 
GO
ALTER TABLE dbo.TET_SPA_Period ADD CONSTRAINT
	PK_TET_SPA_Period PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
