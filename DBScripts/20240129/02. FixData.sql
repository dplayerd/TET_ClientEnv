USE [TET_Supplier]
GO

-- STQA資料維護
INSERT [dbo].[Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate], [DeleteUser], [DeleteDate]) 
VALUES (N'7F728D0B-8AD7-4A02-8DA1-E37B8939A92B', N'STQA資料維護', N'STQA', N'Index', NULL, NULL, N'Admin', CAST(N'2024-01-03T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL)
GO

UPDATE TET_Supplier_Menu
SET ModuleID = '7F728D0B-8AD7-4A02-8DA1-E37B8939A92B'
WHERE ID = '18EE36D7-F73B-48C5-8A73-CBBEB9629E46'

-- SPA資料維護
INSERT [dbo].[Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate], [DeleteUser], [DeleteDate]) 
VALUES (N'08F65476-4BD1-4B3E-AF44-4E60FBE2282F', N'SPA資料維護', N'SPA', N'Index', NULL, NULL, N'Admin', CAST(N'2024-01-03T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL)
GO

UPDATE TET_Supplier_Menu
SET ModuleID = '08F65476-4BD1-4B3E-AF44-4E60FBE2282F'
WHERE ID = 'B2EB8028-FAD4-45AC-83DE-29F9D6FEB380'
	


-- 修復多餘的空白
UPDATE TET_Parameters
SET Item = 'Commercial Invoice'
WHERE ID = '10793101-98bb-43f2-a840-e187f8746ed0'
	
-- 修復 BU 複選
UPDATE TET_Supplier
SET RelatedDept = '["' + RelatedDept + '"]'
WHERE 
	LEN(TRIM(RelatedDept)) > 0 AND
	RelatedDept NOT LIKE '["%' AND
	RelatedDept NOT LIKE '%"]' 


-- SPA查詢(單一評鑑期別)
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'21C3B503-2A9E-44E0-910E-77BEF89FB71A', N'SPA查詢(單一評鑑期別)', N'SPA', N'SingleQuery', NULL, NULL, N'Admin', '2024-01-03', N'Admin', '2024-01-03')

UPDATE TET_Supplier_Menu
SET ModuleID = '21C3B503-2A9E-44E0-910E-77BEF89FB71A'
WHERE ID = 'A3D1CEE5-7D81-43B5-87E1-6D36A696E221'

-- SPA查詢(多個評鑑期別)
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'E5BEA3C8-6294-4F9D-9E10-74F436A1A28E', N'SPA查詢(多個評鑑期別)', N'SPA', N'MultiQuery',  NULL, NULL, N'Admin', '2024-01-03', N'Admin', '2024-01-03')

UPDATE TET_Supplier_Menu
SET ModuleID = 'E5BEA3C8-6294-4F9D-9E10-74F436A1A28E'
WHERE ID = 'C7EBDB2E-0C88-4C42-99A0-DC008E178936'