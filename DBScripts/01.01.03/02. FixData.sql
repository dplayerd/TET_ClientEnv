USE [TET_Supplier]
GO

-- 新增模組： 供應商SPA評鑑計分比例設定
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'9D401FF0-32DD-48AF-A4B4-6A44013F9BB7', N'供應商SPA評鑑計分比例設定', N'SPA_ScoringRatio', N'Index', NULL, NULL, N'Admin', '2024-01-01', N'Admin', '2024-01-01')


-- 新增頁面
INSERT [TET_Supplier_Menu] 
([ID], [SiteID], [ParentID], [Name], [Description], [MenuType], [Linkurl], [ModuleID], [PageIcon], [SortNo], [IsEnable], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
(N'0451FE02-7F5A-4E64-B3AA-6ECC8B0DBF4E', N'15e34669-cc25-48c5-85c6-6af49252cbfe', NULL, N'供應商SPA評鑑計分比例設定', N'供應商SPA評鑑計分比例設定', 2, N'/Supplier/SPA_ScoringRatio/index', '9D401FF0-32DD-48AF-A4B4-6A44013F9BB7', N'flaticon-app', 11, 1, N'Admin', '2024-01-01', N'Admin', '2024-01-01')
GO


-- 設定頁面權限
 INSERT [TET_Supplier_RoleMenu] 
	([ID], [MenuID], [RoleID], [AllowActs], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'0451FE02-7F5A-4E64-B3AA-6ECC8B0DBF4E', N'0451FE02-7F5A-4E64-B3AA-6ECC8B0DBF4E', N'B4EF6AE4-5873-4511-8C7B-7F1281DD1B5E', 127, N'00001', '2024-01-01', N'00001', '2024-01-01')
