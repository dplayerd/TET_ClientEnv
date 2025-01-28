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
CREATE TABLE dbo.Tmp_TET_Supplier
	(
	ID uniqueidentifier NOT NULL,
	CoSignApprover nvarchar(256) NULL,
	IsSecret nvarchar(8) NOT NULL,
	IsNDA nvarchar(32) NOT NULL,
	ApplyReason nvarchar(512) NULL,
	BelongTo nvarchar(128) NULL,
	VenderCode nvarchar(32) NULL,
	SupplierCategory nvarchar(512) NOT NULL,
	BusinessCategory nvarchar(512) NOT NULL,
	BusinessAttribute nvarchar(512) NOT NULL,
	SearchKey nvarchar(256) NULL,
	RelatedDept nvarchar(256) NULL,
	Buyer nvarchar(256) NULL,
	RegisterDate date NULL,
	CName nvarchar(128) NOT NULL,
	EName nvarchar(128) NOT NULL,
	Country nvarchar(64) NOT NULL,
	TaxNo nvarchar(16) NOT NULL,
	Address nvarchar(128) NOT NULL,
	OfficeTel nvarchar(32) NOT NULL,
	ISO nvarchar(128) NULL,
	Email nvarchar(128) NULL,
	Website nvarchar(128) NULL,
	CapitalAmount nvarchar(32) NOT NULL,
	MainProduct nvarchar(512) NOT NULL,
	Employees nvarchar(32) NULL,
	Charge nvarchar(32) NOT NULL,
	PaymentTerm nvarchar(64) NOT NULL,
	BillingDocument nvarchar(64) NOT NULL,
	Incoterms nvarchar(64) NOT NULL,
	Remark nvarchar(512) NULL,
	BankCountry nvarchar(32) NOT NULL,
	BankName nvarchar(128) NOT NULL,
	BankCode nvarchar(16) NOT NULL,
	BankBranchName nvarchar(128) NOT NULL,
	BankBranchCode nvarchar(16) NOT NULL,
	Currency nvarchar(64) NOT NULL,
	BankAccountName nvarchar(64) NOT NULL,
	BankAccountNo nvarchar(32) NOT NULL,
	CompanyCity nvarchar(32) NULL,
	BankAddress nvarchar(128) NULL,
	SwiftCode nvarchar(32) NULL,
	NDANo nvarchar(32) NULL,
	Contract nvarchar(1) NULL,
	IsSign1 nvarchar(32) NULL,
	SignDate1 date NULL,
	IsSign2 nvarchar(32) NULL,
	SignDate2 date NULL,
	STQAApplication nvarchar(32) NULL,
	KeySupplier nvarchar(64) NULL,
	Version int NOT NULL,
	RevisionType nvarchar(1) NULL,
	IsLastVersion nvarchar(1) NULL,
	ApproveStatus nvarchar(32) NULL,
	CreateUser nvarchar(64) NOT NULL,
	CreateDate datetime NOT NULL,
	ModifyUser nvarchar(64) NOT NULL,
	ModifyDate datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TET_Supplier SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.TET_Supplier)
	 EXEC('INSERT INTO dbo.Tmp_TET_Supplier (ID, IsSecret, IsNDA, ApplyReason, BelongTo, VenderCode, SupplierCategory, BusinessCategory, BusinessAttribute, SearchKey, RelatedDept, Buyer, RegisterDate, CName, EName, Country, TaxNo, Address, OfficeTel, ISO, Email, Website, CapitalAmount, MainProduct, Employees, Charge, PaymentTerm, BillingDocument, Incoterms, Remark, BankCountry, BankName, BankCode, BankBranchName, BankBranchCode, Currency, BankAccountName, BankAccountNo, CompanyCity, BankAddress, SwiftCode, NDANo, Contract, IsSign1, SignDate1, IsSign2, SignDate2, STQAApplication, KeySupplier, Version, RevisionType, IsLastVersion, ApproveStatus, CreateUser, CreateDate, ModifyUser, ModifyDate)
		SELECT ID, IsSecret, IsNDA, ApplyReason, BelongTo, VenderCode, SupplierCategory, BusinessCategory, BusinessAttribute, SearchKey, RelatedDept, Buyer, RegisterDate, CName, EName, Country, TaxNo, Address, OfficeTel, ISO, Email, Website, CapitalAmount, MainProduct, Employees, Charge, PaymentTerm, BillingDocument, Incoterms, Remark, BankCountry, BankName, BankCode, BankBranchName, BankBranchCode, Currency, BankAccountName, BankAccountNo, CompanyCity, BankAddress, SwiftCode, NDANo, Contract, IsSign1, SignDate1, IsSign2, SignDate2, STQAApplication, KeySupplier, Version, RevisionType, IsLastVersion, ApproveStatus, CreateUser, CreateDate, ModifyUser, ModifyDate FROM dbo.TET_Supplier WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TET_Supplier
GO
EXECUTE sp_rename N'dbo.Tmp_TET_Supplier', N'TET_Supplier', 'OBJECT' 
GO
ALTER TABLE dbo.TET_Supplier ADD CONSTRAINT
	PK_TET_Supplier PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
