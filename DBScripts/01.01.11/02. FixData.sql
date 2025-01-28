USE [TET_Supplier]
GO

-- 新增模組： SPA 績效評鑑報告維護
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'FB3AC278-CD50-49AD-9010-4E9E9F624B4F', N'SPA 績效評鑑報告維護', N'SPA_EvaluationReport', N'Index', NULL, NULL, N'Admin', '2024-01-01', N'Admin', '2024-01-01')


-- 新增頁面
INSERT [TET_Supplier_Menu] 
([ID], [SiteID], [ParentID], [Name], [Description], [MenuType], [Linkurl], [ModuleID], [PageIcon], [SortNo], [IsEnable], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
(N'6F4B8EF8-21EA-4133-8F42-0492F08F31A1', N'15e34669-cc25-48c5-85c6-6af49252cbfe', NULL, N'SPA 績效評鑑報告維護', N'SPA 績效評鑑報告維護', 2, N'/Supplier/SPA_EvaluationReport/index', 'FB3AC278-CD50-49AD-9010-4E9E9F624B4F', N'flaticon-app', 11, 1, N'Admin', '2024-01-01', N'Admin', '2024-01-01')
GO


-- 設定頁面權限
 INSERT [TET_Supplier_RoleMenu] 
	([ID], [MenuID], [RoleID], [AllowActs], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'6F4B8EF8-21EA-4133-8F42-0492F08F31A1', N'6F4B8EF8-21EA-4133-8F42-0492F08F31A1', N'B4EF6AE4-5873-4511-8C7B-7F1281DD1B5E', 127, N'00001', '2024-01-01', N'00001', '2024-01-01')


