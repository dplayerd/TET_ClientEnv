USE [TET_Supplier]
GO

-- 供應商資料查詢
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'E871A89E-6F93-4E85-8F5F-65CBEB9BE1F1', N'供應商資料查詢', N'Supplier', N'Query', NULL, NULL, N'Admin', '2024-01-01', N'Admin', '2024-01-01')

UPDATE TET_Supplier_Menu
SET ModuleID = 'E871A89E-6F93-4E85-8F5F-65CBEB9BE1F1'
WHERE ID = '581D5CBE-EF5A-426F-B2F5-1F0CA2C2371A'



-- 供應商資料查詢(採購)
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'78432310-0FFD-4A73-88D4-DB07B1B2F429', N'供應商資料查詢(採購)', N'Supplier', N'QuerySS', NULL, NULL, N'Admin', '2024-01-01', N'Admin', '2024-01-01')


INSERT [TET_Supplier_Menu] 
([ID], [SiteID], [ParentID], [Name], [Description], [MenuType], [Linkurl], [ModuleID], [PageIcon], [SortNo], [IsEnable], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
(N'AF0F44EF-5F47-4D77-B312-191CA39957DB', N'15e34669-cc25-48c5-85c6-6af49252cbfe', NULL, N'供應商資料查詢(採購)', N'提供使用者查詢供應商資料', 2, N'/Supplier/SupplierQuerySS/index', '78432310-0FFD-4A73-88D4-DB07B1B2F429', N'flaticon-app', 11, 1, N'Admin', '2024-01-01', N'Admin', '2024-01-01')
GO

-- 設定頁面權限
 INSERT [TET_Supplier_RoleMenu] 
	([ID], [MenuID], [RoleID], [AllowActs], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'BE74BA72-2935-49DA-9E00-F6CF71948617', N'AF0F44EF-5F47-4D77-B312-191CA39957DB', N'B4EF6AE4-5873-4511-8C7B-7F1281DD1B5E', 127, N'00001', '2024-01-01', N'00001', '2024-01-01'),
	(N'158B9117-4CE8-4446-BB20-1B5281CB07B0', N'AF0F44EF-5F47-4D77-B312-191CA39957DB', N'1D7BEC68-A588-41C9-AB39-6D7D2162A799', 127, N'00001', '2024-01-01', N'00001', '2024-01-01')