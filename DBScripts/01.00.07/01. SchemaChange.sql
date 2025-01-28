USE [TET_Supplier]
GO


-- Change Approval's desc column length
ALTER TABLE TET_SupplierSTQAApproval
ALTER COLUMN Description nvarchar(256);

ALTER TABLE TET_SupplierSPAApproval
ALTER COLUMN Description nvarchar(256);

ALTER TABLE TET_SupplierSTQA
ALTER COLUMN [Date] Date;
