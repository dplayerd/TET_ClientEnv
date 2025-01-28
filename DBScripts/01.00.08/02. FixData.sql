USE [TET_Supplier]
GO

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