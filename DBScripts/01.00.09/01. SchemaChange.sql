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
EXECUTE sp_rename N'dbo.TET_SupplierTrade.CreatedBy', N'Tmp_CreateUser_4', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierTrade.CreatedTime', N'Tmp_CreateDate_5', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierTrade.LastModifiedBy', N'Tmp_ModifyUser_6', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierTrade.LastModifiedTime', N'Tmp_ModifyDate_7', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierTrade.Tmp_CreateUser_4', N'CreateUser', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierTrade.Tmp_CreateDate_5', N'CreateDate', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierTrade.Tmp_ModifyUser_6', N'ModifyUser', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TET_SupplierTrade.Tmp_ModifyDate_7', N'ModifyDate', 'COLUMN' 
GO
ALTER TABLE dbo.TET_SupplierTrade SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
