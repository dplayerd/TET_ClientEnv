-- 新增模組： 供應商SPA評鑑計分比例設定
INSERT [Modules] 
	([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'5C64E882-3C20-44DB-A1E1-26BB87899193', N'SPA_ScoringInfoSheetSetup', N'SPA_ScoringInfoSheetSetup', N'Index', N'SPA_ScoringInfoSheetSetup', N'Index', N'system', '2026-08-23 21:10:03.150', N'system', '2026-08-23 21:10:03.150', NULL, NULL)


-- 新增頁面
INSERT [TET_Supplier_Menu] 
([ID], [SiteID], [ParentID], [Name], [Description], [MenuType], [Linkurl], [ModuleID], [PageIcon], [SortNo], [IsEnable], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
(N'BD2DC94D-4328-4B0A-A1E1-A91C2C570239', N'15E34669-CC25-48C5-85C6-6AF49252CBFE', N'EF715230-10D1-4B56-993A-00C5A9E7FD98', N'SPA評鑑計分資料頁籤顯示設定', N'SPA評鑑計分資料頁籤顯示設定', 2, N'', N'5C64E882-3C20-44DB-A1E1-26BB87899193', N'flaticon-app', 44, 1, N'212486', '2026-08-24 02:28:33.117', N'212486', '2026-08-24 02:28:33.117')


-- 設定頁面權限
 INSERT [TET_Supplier_RoleMenu] 
	([ID], [MenuID], [RoleID], [AllowActs], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'BD2DC94D-4328-4B0A-A1E1-A91C2C570239', N'BD2DC94D-4328-4B0A-A1E1-A91C2C570239', N'B4EF6AE4-5873-4511-8C7B-7F1281DD1B5E', 127, N'00001', '2024-01-01', N'00001', '2024-01-01')
