USE [TET_Supplier]
GO


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

-- TET_SupplierSTQA
EXECUTE sp_rename N'dbo.TET_SupplierSTQA.Guid', N'Tmp_ID_1', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQA.CreatedBy', N'Tmp_CreateUser_2', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQA.CreatedTime', N'Tmp_CreateDate_3', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQA.LastModifiedBy', N'Tmp_ModifyUser_4', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQA.LastModifiedTime', N'Tmp_ModifyDate_5', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQA.Tmp_ID_1', N'ID', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQA.Tmp_CreateUser_2', N'CreateUser', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQA.Tmp_CreateDate_3', N'CreateDate', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQA.Tmp_ModifyUser_4', N'ModifyUser', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQA.Tmp_ModifyDate_5', N'ModifyDate', 'COLUMN' 
GO
ALTER TABLE dbo.TET_SupplierSTQA SET (LOCK_ESCALATION = TABLE)
GO


-- TET_SupplierSTQAApproval
EXECUTE sp_rename N'dbo.TET_SupplierSTQAApproval.Guid', N'Tmp_ID_6', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQAApproval.CreatedBy', N'Tmp_CreateUser_7', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQAApproval.CreatedTime', N'Tmp_CreateDate_8', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQAApproval.LastModifiedBy', N'Tmp_ModifyUser_9', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQAApproval.LastModifiedTime', N'Tmp_ModifyDate_10', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQAApproval.Tmp_ID_6', N'ID', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQAApproval.Tmp_CreateUser_7', N'CreateUser', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQAApproval.Tmp_CreateDate_8', N'CreateDate', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQAApproval.Tmp_ModifyUser_9', N'ModifyUser', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSTQAApproval.Tmp_ModifyDate_10', N'ModifyDate', 'COLUMN' 
GO
ALTER TABLE dbo.TET_SupplierSTQAApproval SET (LOCK_ESCALATION = TABLE)
GO


-- TET_SupplierSPA
EXECUTE sp_rename N'dbo.TET_SupplierSPA.Guid', N'Tmp_ID_11', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPA.CreatedBy', N'Tmp_CreateUser_12', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPA.CreatedTime', N'Tmp_CreateDate_13', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPA.LastModifiedBy', N'Tmp_ModifyUser_14', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPA.LastModifiedTime', N'Tmp_ModifyDate_15', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPA.Tmp_ID_11', N'ID', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPA.Tmp_CreateUser_12', N'CreateUser', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPA.Tmp_CreateDate_13', N'CreateDate', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPA.Tmp_ModifyUser_14', N'ModifyUser', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPA.Tmp_ModifyDate_15', N'ModifyDate', 'COLUMN' 
GO
ALTER TABLE dbo.TET_SupplierSPA SET (LOCK_ESCALATION = TABLE)
GO

-- TET_SupplierSPAApproval
EXECUTE sp_rename N'dbo.TET_SupplierSPAApproval.Guid', N'Tmp_ID_16', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPAApproval.CreatedBy', N'Tmp_CreateUser_17', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPAApproval.CreatedTime', N'Tmp_CreateDate_18', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPAApproval.LastModifiedBy', N'Tmp_ModifyUser_19', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPAApproval.LastModifiedTime', N'Tmp_ModifyDate_20', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPAApproval.Tmp_ID_16', N'ID', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPAApproval.Tmp_CreateUser_17', N'CreateUser', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPAApproval.Tmp_CreateDate_18', N'CreateDate', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPAApproval.Tmp_ModifyUser_19', N'ModifyUser', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierSPAApproval.Tmp_ModifyDate_20', N'ModifyDate', 'COLUMN' 
GO
ALTER TABLE dbo.TET_SupplierSPAApproval SET (LOCK_ESCALATION = TABLE)
GO

COMMIT
GO
