USE [TET_Supplier]
GO

INSERT [dbo].[Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate], [DeleteUser], [DeleteDate]) 
VALUES (N'6fa280d3-e6d6-4439-bce8-63335041259b', N'供應商重要資訊改版申請', N'SupplierRevision', N'Index', NULL, NULL, N'Admin', CAST(N'2023-10-01T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL)
GO


UPDATE TET_Supplier_Menu
SET ModuleID = '6FA280D3-E6D6-4439-BCE8-63335041259B'
WHERE ID = '996406C5-1AF0-46B7-B6A3-EEEA35CED58F'
GO