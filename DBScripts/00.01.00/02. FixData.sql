USE [TET_Supplier]
GO

-- 新增模組： 供應商SPA評鑑計分比例設定
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'48EF0AFF-60C9-469F-9AF0-9ACC7B455CA4', N'Sample-Project', N'SampleProject', N'Index', NULL, NULL, N'Admin', '2024-01-01', N'Admin', '2024-01-01')


-- 新增頁面
INSERT [TET_Supplier_Menu] 
([ID], [SiteID], [ParentID], [Name], [Description], [MenuType], [Linkurl], [ModuleID], [PageIcon], [SortNo], [IsEnable], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
(N'7602142D-246A-40BB-9959-936C3354308A', N'15e34669-cc25-48c5-85c6-6af49252cbfe', NULL, N'Sample-Project', N'Sample-Project', 2, N'/Supplier/SampleProject/index', '48EF0AFF-60C9-469F-9AF0-9ACC7B455CA4', N'flaticon-app', 99, 1, N'Admin', '2024-01-01', N'Admin', '2024-01-01')
GO


-- 設定頁面權限
 INSERT [TET_Supplier_RoleMenu] 
	([ID], [MenuID], [RoleID], [AllowActs], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'7602142D-246A-40BB-9959-936C3354308A', N'7602142D-246A-40BB-9959-936C3354308A', N'B4EF6AE4-5873-4511-8C7B-7F1281DD1B5E', 127, N'00001', '2024-01-01', N'00001', '2024-01-01')
