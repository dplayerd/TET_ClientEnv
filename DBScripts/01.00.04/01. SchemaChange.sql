USE [TET_Supplier]
GO

/****** Object:  View [dbo].[vwTET_Supplier_LastVersion]    Script Date: 2024/1/19 下午 03:59:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwTET_Supplier_LastVersion]
AS
SELECT          dbo.TET_Supplier.ID, dbo.TET_Supplier.IsSecret, dbo.TET_Supplier.IsNDA, dbo.TET_Supplier.ApplyReason, 
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
WHERE          (dbo.TET_Supplier.Version = 0) AND (dbo.TET_Supplier.IsLastVersion = 'Y') OR
                            (dbo.TET_Supplier.Version > 0)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TET_Supplier"
            Begin Extent = 
               Top = 12
               Left = 76
               Bottom = 274
               Right = 498
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Temp1"
            Begin Extent = 
               Top = 12
               Left = 574
               Bottom = 200
               Right = 942
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwTET_Supplier_LastVersion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwTET_Supplier_LastVersion'
GO


