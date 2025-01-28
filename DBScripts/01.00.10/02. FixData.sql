USE [TET_Supplier]
GO


-- 常用參數管理
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'96611825-C63A-436E-AB53-FDF50102DCC8', N'常用參數管理', N'Parameters', N'Index', NULL, NULL, N'Admin', '2024-01-01', N'Admin', '2024-01-01')

UPDATE TET_Supplier_Menu
SET ModuleID = '96611825-C63A-436E-AB53-FDF50102DCC8'
WHERE ID = '3D5DEC53-C4AB-418A-9D88-F4355DF5C50F'

