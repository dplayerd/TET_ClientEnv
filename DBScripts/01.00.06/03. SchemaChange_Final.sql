USE TET_Supplier
GO


-- vwApprovalList
CREATE VIEW [dbo].[vwApprovalList]
AS
	SELECT [ID]
		,[SupplierID] AS ParentID
		,'Supplier' AS ParentType
		,[Type]
		,[Description]
		,[Level]
		,[Approver]
		,[Result]
		,[Comment]
		,[CreateUser]
		,[CreateDate]
		,[ModifyUser]
		,[ModifyDate]
	FROM [TET_SupplierApproval]
UNION
	SELECT 
		[ID]
		,SPAID AS ParentID
		,'SPA' AS ParentType
		,[Type]
		,[Description]
		,[Level]
		,[Approver]
		,[Result]
		,[Comment]
		,[CreateUser]
		,[CreateDate]
		,[ModifyUser]
		,[ModifyDate]
	FROM [TET_SupplierSPAApproval]
UNION
	SELECT
		[ID]
		,STQAID AS ParentID
		,'STQA' AS ParentType
		,[Type]
		,[Description]
		,[Level]
		,[Approver]
		,[Result]
		,[Comment]
		,[CreateUser]
		,[CreateDate]
		,[ModifyUser]
		,[ModifyDate]
	FROM  [TET_SupplierSTQAApproval]
