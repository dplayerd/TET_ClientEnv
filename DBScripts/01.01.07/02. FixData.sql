USE [TET_Supplier]
GO

-- 新增模組： 供應商SPA評鑑計分比例設定
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'3A8CC648-FCE5-4060-BA59-9EC77626C85E', N'Cost&Service資料維護', N'SPA_CostService', N'Index', NULL, NULL, N'Admin', '2024-01-01', N'Admin', '2024-01-01')


-- 新增頁面
INSERT [TET_Supplier_Menu] 
([ID], [SiteID], [ParentID], [Name], [Description], [MenuType], [Linkurl], [ModuleID], [PageIcon], [SortNo], [IsEnable], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
(N'ACB6D8BE-52A6-4185-B9C4-E283FA77FCAA', N'15e34669-cc25-48c5-85c6-6af49252cbfe', NULL, N'Cost&Service資料維護', N'Cost&Service資料維護', 2, N'/Supplier/SPA_CostService/index', '3A8CC648-FCE5-4060-BA59-9EC77626C85E', N'flaticon-app', 11, 1, N'Admin', '2024-01-01', N'Admin', '2024-01-01')
GO


-- 設定頁面權限
 INSERT [TET_Supplier_RoleMenu] 
	([ID], [MenuID], [RoleID], [AllowActs], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'ACB6D8BE-52A6-4185-B9C4-E283FA77FCAA', N'ACB6D8BE-52A6-4185-B9C4-E283FA77FCAA', N'B4EF6AE4-5873-4511-8C7B-7F1281DD1B5E', 127, N'00001', '2024-01-01', N'00001', '2024-01-01')
