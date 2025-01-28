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
CREATE TABLE dbo.Tmp_TET_SPA_ScoringInfoModule1
	(
	ID uniqueidentifier NOT NULL,
	SIID uniqueidentifier NOT NULL,
	Source nvarchar(16) NOT NULL,
	Type nvarchar(16) NOT NULL,
	Supplier nvarchar(128) NOT NULL,
	EmpName nvarchar(64) NOT NULL,
	MajorJob nvarchar(64) NULL,
	IsIndependent nvarchar(16) NULL,
	SkillLevel nvarchar(64) NULL,
	EmpStatus nvarchar(64) NULL,
	TELSeniorityY nvarchar(4) NULL,
	TELSeniorityM nvarchar(4) NULL,
	Remark nvarchar(1000) NULL,
	CreateUser nvarchar(64) NOT NULL,
	CreateDate datetime NOT NULL,
	ModifyUser nvarchar(64) NOT NULL,
	ModifyDate datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TET_SPA_ScoringInfoModule1 SET (LOCK_ESCALATION = TABLE)
GO
DECLARE @v sql_variant 
SET @v = N'系統辨識碼'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'ID'
GO
DECLARE @v sql_variant 
SET @v = N'評鑑計分資料系統辨識碼'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'SIID'
GO
DECLARE @v sql_variant 
SET @v = N'資料來源'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'Source'
GO
DECLARE @v sql_variant 
SET @v = N'本社/協力廠商'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'Type'
GO
DECLARE @v sql_variant 
SET @v = N'供應商名稱'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'Supplier'
GO
DECLARE @v sql_variant 
SET @v = N'員工姓名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'EmpName'
GO
DECLARE @v sql_variant 
SET @v = N'主要負責作業'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'MajorJob'
GO
DECLARE @v sql_variant 
SET @v = N'能否獨立作業'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'IsIndependent'
GO
DECLARE @v sql_variant 
SET @v = N'Skill Level'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'SkillLevel'
GO
DECLARE @v sql_variant 
SET @v = N'員工狀態'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'EmpStatus'
GO
DECLARE @v sql_variant 
SET @v = N'派工至TEL的年資(年)'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'TELSeniorityY'
GO
DECLARE @v sql_variant 
SET @v = N'派工至TEL的年資(月)'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'TELSeniorityM'
GO
DECLARE @v sql_variant 
SET @v = N'備註'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'Remark'
GO
DECLARE @v sql_variant 
SET @v = N'建立人員'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'CreateUser'
GO
DECLARE @v sql_variant 
SET @v = N'新增時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'CreateDate'
GO
DECLARE @v sql_variant 
SET @v = N'最後更新人員'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'ModifyUser'
GO
DECLARE @v sql_variant 
SET @v = N'最後更新時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TET_SPA_ScoringInfoModule1', N'COLUMN', N'ModifyDate'
GO
IF EXISTS(SELECT * FROM dbo.TET_SPA_ScoringInfoModule1)
	 EXEC('INSERT INTO dbo.Tmp_TET_SPA_ScoringInfoModule1 (ID, SIID, Source, Type, Supplier, EmpName, MajorJob, IsIndependent, SkillLevel, TELSeniorityY, TELSeniorityM, Remark, CreateUser, CreateDate, ModifyUser, ModifyDate)
		SELECT ID, SIID, Source, Type, Supplier, EmpName, MajorJob, IsIndependent, SkillLevel, TELSeniorityY, TELSeniorityM, Remark, CreateUser, CreateDate, ModifyUser, ModifyDate FROM dbo.TET_SPA_ScoringInfoModule1 WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TET_SPA_ScoringInfoModule1
GO
EXECUTE sp_rename N'dbo.Tmp_TET_SPA_ScoringInfoModule1', N'TET_SPA_ScoringInfoModule1', 'OBJECT' 
GO
ALTER TABLE dbo.TET_SPA_ScoringInfoModule1 ADD CONSTRAINT
	PK_TET_SPA_ScoringInfoModule1 PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
