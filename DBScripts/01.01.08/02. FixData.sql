USE [TET_Supplier]
GO

-- 新增模組： 供應商SPA評鑑計分資料維護
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'E9061633-9E23-437E-8DB7-2E9DA624A9EE', N'供應商SPA評鑑計分資料維護', N'SPA_ScoringInfo', N'Index', NULL, NULL, N'Admin', '2024-01-01', N'Admin', '2024-01-01')


-- 新增頁面
INSERT [TET_Supplier_Menu] 
([ID], [SiteID], [ParentID], [Name], [Description], [MenuType], [Linkurl], [ModuleID], [PageIcon], [SortNo], [IsEnable], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
(N'BEE71585-EC1F-400C-8416-5C9A2B856DE6', N'15e34669-cc25-48c5-85c6-6af49252cbfe', NULL, N'供應商SPA評鑑計分資料維護', N'供應商SPA評鑑計分資料維護', 2, N'/Supplier/SPA_ScoringInfo/index', 'E9061633-9E23-437E-8DB7-2E9DA624A9EE', N'flaticon-app', 11, 1, N'Admin', '2024-01-01', N'Admin', '2024-01-01')
GO


-- 設定頁面權限
 INSERT [TET_Supplier_RoleMenu] 
	([ID], [MenuID], [RoleID], [AllowActs], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'BEE71585-EC1F-400C-8416-5C9A2B856DE6', N'BEE71585-EC1F-400C-8416-5C9A2B856DE6', N'B4EF6AE4-5873-4511-8C7B-7F1281DD1B5E', 127, N'00001', '2024-01-01', N'00001', '2024-01-01')
