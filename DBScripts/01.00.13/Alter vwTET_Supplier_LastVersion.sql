ALTER VIEW [dbo].[vwTET_Supplier_LastVersion]
AS
SELECT          dbo.TET_Supplier.ID, dbo.TET_Supplier.IsActive,dbo.TET_Supplier.IsSecret, dbo.TET_Supplier.IsNDA, dbo.TET_Supplier.ApplyReason, 
                            dbo.TET_Supplier.BelongTo, dbo.TET_Supplier.VenderCode, dbo.TET_Supplier.SupplierCategory, 
                            dbo.TET_Supplier.BusinessCategory, dbo.TET_Supplier.BusinessAttribute, dbo.TET_Supplier.SearchKey, 
                            dbo.TET_Supplier.RelatedDept, dbo.TET_Supplier.Buyer, dbo.TET_Supplier.RegisterDate, dbo.TET_Supplier.CName, 
                            dbo.TET_Supplier.EName, dbo.TET_Supplier.Country, dbo.TET_Supplier.TaxNo, dbo.TET_Supplier.Address, 
                            dbo.TET_Supplier.OfficeTel, dbo.TET_Supplier.ISO, dbo.TET_Supplier.Email, dbo.TET_Supplier.Website, 
                            dbo.TET_Supplier.CapitalAmount, dbo.TET_Supplier.MainProduct, dbo.TET_Supplier.Employees, 
                            dbo.TET_Supplier.Charge, dbo.TET_Supplier.PaymentTerm, dbo.TET_Supplier.BillingDocument, 
                            dbo.TET_Supplier.Incoterms, dbo.TET_Supplier.Remark, dbo.TET_Supplier.BankCountry, 
                            dbo.TET_Supplier.BankName, dbo.TET_Supplier.BankCode, dbo.TET_Supplier.BankBranchName, 
                            dbo.TET_Supplier.BankBranchCode, dbo.TET_Supplier.Currency, dbo.TET_Supplier.BankAccountName, 
                            dbo.TET_Supplier.BankAccountNo, dbo.TET_Supplier.CompanyCity, dbo.TET_Supplier.BankAddress, 
                            dbo.TET_Supplier.SwiftCode, dbo.TET_Supplier.NDANo, dbo.TET_Supplier.Contract, dbo.TET_Supplier.IsSign1, 
                            dbo.TET_Supplier.SignDate1, dbo.TET_Supplier.IsSign2, dbo.TET_Supplier.SignDate2, 
                            dbo.TET_Supplier.STQAApplication, dbo.TET_Supplier.KeySupplier, dbo.TET_Supplier.Version, 
                            dbo.TET_Supplier.RevisionType, dbo.TET_Supplier.IsLastVersion, dbo.TET_Supplier.ApproveStatus, 
                            dbo.TET_Supplier.CreateUser, dbo.TET_Supplier.CreateDate, dbo.TET_Supplier.ModifyUser, 
                            dbo.TET_Supplier.ModifyDate, Temp1.VenderCode AS Expr1, Temp1.Version AS Expr2
FROM              dbo.TET_Supplier INNER JOIN
                                (SELECT          VenderCode, MAX(Version) AS Version
                                  FROM               dbo.TET_Supplier AS TET_Supplier_1
                                  GROUP BY    VenderCode) AS Temp1 ON dbo.TET_Supplier.VenderCode = Temp1.VenderCode AND 
                            dbo.TET_Supplier.Version = Temp1.Version
WHERE          (dbo.TET_Supplier.IsActive = 'Active') AND (dbo.TET_Supplier.Version = 0) AND (dbo.TET_Supplier.IsLastVersion = 'Y') OR
                            (dbo.TET_Supplier.Version > 0)
GO


