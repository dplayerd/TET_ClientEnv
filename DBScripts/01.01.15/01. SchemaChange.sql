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
ALTER TABLE dbo.MediaFiles
	DROP CONSTRAINT DF__MediaFiles__ID__70DDC3D8
GO
ALTER TABLE dbo.MediaFiles
	DROP CONSTRAINT DF__MediaFile__b_dat__71D1E811
GO
CREATE TABLE dbo.Tmp_MediaFiles
	(
	SeqNo int NOT NULL IDENTITY (1, 1),
	ID uniqueidentifier NOT NULL,
	ModuleName varchar(50) NOT NULL,
	ModuleID varchar(200) NOT NULL,
	Purpose varchar(50) NOT NULL,
	FilePath varchar(200) NOT NULL,
	OrgFileName nvarchar(300) NOT NULL,
	OutputFileName nvarchar(300) NOT NULL,
	MimeType varchar(100) NOT NULL,
	RequireAuth bit NOT NULL,
	IsEnable bit NOT NULL,
	CreateUser nvarchar(64) NOT NULL,
	CreateDate datetime NOT NULL,
	ModifyUser nvarchar(64) NULL,
	ModifyDate datetime NULL,
	DeleteUser nvarchar(64) NULL,
	DeleteDate datetime NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_MediaFiles SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_MediaFiles ADD CONSTRAINT
	DF__MediaFiles__ID__70DDC3D8 DEFAULT (newid()) FOR ID
GO
ALTER TABLE dbo.Tmp_MediaFiles ADD CONSTRAINT
	DF__MediaFile__b_dat__71D1E811 DEFAULT (getdate()) FOR CreateDate
GO
SET IDENTITY_INSERT dbo.Tmp_MediaFiles ON
GO
IF EXISTS(SELECT * FROM dbo.MediaFiles)
	 EXEC('INSERT INTO dbo.Tmp_MediaFiles (SeqNo, ID, ModuleName, ModuleID, Purpose, FilePath, OrgFileName, OutputFileName, MimeType, RequireAuth, IsEnable, CreateUser, CreateDate, ModifyUser, ModifyDate, DeleteUser, DeleteDate)
		SELECT SeqNo, ID, ModuleName, ModuleID, Purpose, FilePath, OrgFileName, OutputFileName, MimeType, RequireAuth, IsEnable, CONVERT(nvarchar(64), CreateUser), CreateDate, CONVERT(nvarchar(64), ModifyUser), ModifyDate, CONVERT(nvarchar(64), DeleteUser), DeleteDate FROM dbo.MediaFiles WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_MediaFiles OFF
GO
DROP TABLE dbo.MediaFiles
GO
EXECUTE sp_rename N'dbo.Tmp_MediaFiles', N'MediaFiles', 'OBJECT' 
GO
ALTER TABLE dbo.MediaFiles ADD CONSTRAINT
	PK_MediaFiles_ID PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
