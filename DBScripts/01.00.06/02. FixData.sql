USE [TET_Supplier]
GO

INSERT [dbo].[Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate], [DeleteUser], [DeleteDate]) 
VALUES (N'7F728D0B-8AD7-4A02-8DA1-E37B8939A92B', N'STQA資料維護', N'STQA', N'Index', NULL, NULL, N'Admin', CAST(N'2024-01-03T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL)
GO

UPDATE TET_Supplier_Menu
SET ModuleID = '7F728D0B-8AD7-4A02-8DA1-E37B8939A92B'
WHERE ID = '18EE36D7-F73B-48C5-8A73-CBBEB9629E46'

INSERT [dbo].[Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate], [DeleteUser], [DeleteDate]) 
VALUES (N'08F65476-4BD1-4B3E-AF44-4E60FBE2282F', N'SPA資料維護', N'SPA', N'Index', NULL, NULL, N'Admin', CAST(N'2024-01-03T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL)
GO

UPDATE TET_Supplier_Menu
SET ModuleID = '08F65476-4BD1-4B3E-AF44-4E60FBE2282F'
WHERE ID = 'B2EB8028-FAD4-45AC-83DE-29F9D6FEB380'
	