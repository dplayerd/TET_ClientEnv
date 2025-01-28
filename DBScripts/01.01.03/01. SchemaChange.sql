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
CREATE TABLE dbo.Tmp_TET_SPA_ApproverSetup
	(
	ID uniqueidentifier NOT NULL,
	ServiceItemID uniqueidentifier NOT NULL,
	BUID uniqueidentifier NOT NULL,
	InfoFill nvarchar(640) NOT NULL,
	InfoConfirm nvarchar(64) NOT NULL,
	Lv1Apprvoer nvarchar(64) NOT NULL,
	Lv2Apprvoer nvarchar(64) NULL,
	CreateUser nvarchar(64) NOT NULL,
	CreateDate datetime NOT NULL,
	ModifyUser nvarchar(64) NOT NULL,
	ModifyDate datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TET_SPA_ApproverSetup SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_TET_SPA_ApproverSetup ADD CONSTRAINT
	DF_TET_SPA_ApproverSetup_ID DEFAULT NEWID() FOR ID
GO
IF EXISTS(SELECT * FROM dbo.TET_SPA_ApproverSetup)
	 EXEC('INSERT INTO dbo.Tmp_TET_SPA_ApproverSetup (ServiceItemID, BUID, InfoFill, InfoConfirm, Lv1Apprvoer, Lv2Apprvoer, CreateUser, CreateDate, ModifyUser, ModifyDate)
		SELECT ServiceItemID, BUID, InfoFill, InfoConfirm, Lv1Apprvoer, Lv2Apprvoer, CreateUser, CreateDate, ModifyUser, ModifyDate FROM dbo.TET_SPA_ApproverSetup WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TET_SPA_ApproverSetup
GO
EXECUTE sp_rename N'dbo.Tmp_TET_SPA_ApproverSetup', N'TET_SPA_ApproverSetup', 'OBJECT' 
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
CREATE TABLE dbo.Tmp_TET_SPA_ScoringRatio
	(
	ID uniqueidentifier NULL,
	ServiceItemID uniqueidentifier NOT NULL,
	POSource nvarchar(16) NOT NULL,
	TRatio1 decimal(7, 4) NOT NULL,
	TRatio2 decimal(7, 4) NOT NULL,
	DRatio1 decimal(7, 4) NOT NULL,
	DRatio2 decimal(7, 4) NOT NULL,
	QRatio1 decimal(7, 4) NOT NULL,
	QRatio2 decimal(7, 4) NOT NULL,
	CRatio1 decimal(7, 4) NOT NULL,
	CRatio2 decimal(7, 4) NOT NULL,
	SRatio1 decimal(7, 4) NOT NULL,
	SRatio2 decimal(7, 4) NOT NULL,
	CreateUser nvarchar(64) NOT NULL,
	CreateDate datetime NOT NULL,
	ModifyUser nvarchar(64) NOT NULL,
	ModifyDate datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TET_SPA_ScoringRatio SET (LOCK_ESCALATION = TABLE)
GO
DECLARE @v sql_variant 
SET @v = N'評鑑項目系統辨識碼'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'ServiceItemID'
GO
DECLARE @v sql_variant 
SET @v = N'PO Source'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'POSource'
GO
DECLARE @v sql_variant 
SET @v = N'施工正確性比例或作業正確性比例'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'TRatio1'
GO
DECLARE @v sql_variant 
SET @v = N'施工技術水平比例'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'TRatio2'
GO
DECLARE @v sql_variant 
SET @v = N'人員穩定度比例'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'DRatio1'
GO
DECLARE @v sql_variant 
SET @v = N'準時完工交付比例或人員備齊貢獻度比例'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'DRatio2'
GO
DECLARE @v sql_variant 
SET @v = N'守規性比例'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'QRatio1'
GO
DECLARE @v sql_variant 
SET @v = N'自訓能力比例'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'QRatio2'
GO
DECLARE @v sql_variant 
SET @v = N'價格競爭力比例'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'CRatio1'
GO
DECLARE @v sql_variant 
SET @v = N'付款條件比例'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'CRatio2'
GO
DECLARE @v sql_variant 
SET @v = N'客戶抱怨比例'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'SRatio1'
GO
DECLARE @v sql_variant 
SET @v = N'配合度比例'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'SRatio2'
GO
DECLARE @v sql_variant 
SET @v = N'建立人員'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'CreateUser'
GO
DECLARE @v sql_variant 
SET @v = N'新增時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'CreateDate'
GO
DECLARE @v sql_variant 
SET @v = N'最後更新人員'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'ModifyUser'
GO
DECLARE @v sql_variant 
SET @v = N'最後更新時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringRatio', N'COLUMN', N'ModifyDate'
GO
ALTER TABLE dbo.Tmp_TET_SPA_ScoringRatio ADD CONSTRAINT
	DF_TET_SPA_ScoringRatio_ID DEFAULT NEWID() FOR ID
GO
IF EXISTS(SELECT * FROM dbo.TET_SPA_ScoringRatio)
	 EXEC('INSERT INTO dbo.Tmp_TET_SPA_ScoringRatio (ServiceItemID, POSource, TRatio1, TRatio2, DRatio1, DRatio2, QRatio1, QRatio2, CRatio1, CRatio2, SRatio1, SRatio2, CreateUser, CreateDate, ModifyUser, ModifyDate)
		SELECT ServiceItemID, POSource, TRatio1, TRatio2, DRatio1, DRatio2, QRatio1, QRatio2, CRatio1, CRatio2, SRatio1, SRatio2, CreateUser, CreateDate, ModifyUser, ModifyDate FROM dbo.TET_SPA_ScoringRatio WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TET_SPA_ScoringRatio
GO
EXECUTE sp_rename N'dbo.Tmp_TET_SPA_ScoringRatio', N'TET_SPA_ScoringRatio', 'OBJECT' 
GO
COMMIT
