USE [TET_Supplier]
GO

/****** Object:  View [dbo].[vwApprovalList]    Script Date: 2024/10/10 上午 09:54:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vwApprovalList]
AS
SELECT [ID], [SupplierID] AS ParentID, 'Supplier' AS ParentType, [Type], [Description], [Level], [Approver], [Result], [Comment], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]
FROM [TET_SupplierApproval]
WHERE [Type] = N'新增供應商審核'
UNION
SELECT [ID], [SupplierID] AS ParentID, 'Revision' AS ParentType, [Type], [Description], [Level], [Approver], [Result], [Comment], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]
FROM [TET_SupplierApproval]
WHERE [Type] = N'供應商改版審核'
UNION
SELECT [ID], SPAID AS ParentID, N'SPA' AS ParentType, [Type], [Description], [Level], [Approver], [Result], [Comment], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]
FROM [TET_SupplierSPAApproval]
UNION
SELECT [ID], STQAID AS ParentID, N'STQA' AS ParentType, [Type], [Description], [Level], [Approver], [Result], [Comment], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]
FROM [TET_SupplierSTQAApproval]
UNION
SELECT [ID], CSID AS ParentID, N'COSTSERVICE' AS ParentType, [Type], [Description], [Level], [Approver], [Result], [Comment], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]
FROM [TET_SPA_CostServiceApproval]
UNION
SELECT [ID], ViolationID AS ParentID, N'VIOLATION' AS ParentType, [Type], [Description], [Level], [Approver], [Result], [Comment], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]
FROM [TET_SPA_ViolationApproval]
GO