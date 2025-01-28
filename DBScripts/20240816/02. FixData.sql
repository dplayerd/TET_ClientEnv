USE [TET_Supplier]
GO

-- 新增模組： 供應商SPA評鑑審核者設定
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'B6754D2D-53CC-4476-BF04-7558F403895F', N'供應商SPA評鑑期間設定', N'SPA_Period', N'Index', NULL, NULL, N'Admin', '2024-01-01', N'Admin', '2024-01-01')


-- 新增頁面
INSERT [TET_Supplier_Menu] 
([ID], [SiteID], [ParentID], [Name], [Description], [MenuType], [Linkurl], [ModuleID], [PageIcon], [SortNo], [IsEnable], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
(N'C1D6F186-E769-4354-89DE-C977EFFE2E58', N'15e34669-cc25-48c5-85c6-6af49252cbfe', NULL, N'供應商SPA評鑑期間設定', N'供應商SPA評鑑期間設定', 2, N'/Supplier/SPA_ApproverSetup/index', 'B6754D2D-53CC-4476-BF04-7558F403895F', N'flaticon-app', 11, 1, N'Admin', '2024-01-01', N'Admin', '2024-01-01')
GO


-- 設定頁面權限
 INSERT [TET_Supplier_RoleMenu] 
	([ID], [MenuID], [RoleID], [AllowActs], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'C1D6F186-E769-4354-89DE-C977EFFE2E58', N'C1D6F186-E769-4354-89DE-C977EFFE2E58', N'B4EF6AE4-5873-4511-8C7B-7F1281DD1B5E', 127, N'00001', '2024-01-01', N'00001', '2024-01-01')
