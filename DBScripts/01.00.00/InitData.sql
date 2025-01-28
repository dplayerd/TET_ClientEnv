USE [UBayPlatform]
GO

INSERT [dbo].[Sites] 
	([ID], [Name], [Title], [MediaFileID], [HeaderText], [FooterText]) 
VALUES 
	(N'15e34669-cc25-48c5-85c6-6af49252cbfe', N'宥倍實業', N'宥倍實業', NULL, N'', N'')
GO


INSERT [dbo].[Modules] 
	([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate], [DeleteUser], [DeleteDate]) 
VALUES 
	(N'3228be98-f7dc-450b-b7a2-0ed8b45f9ea1', N'選單管理', N'PageManagement', N'Index', NULL, NULL, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-01-01T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL),
	(N'81bb133f-318c-49ed-bcd6-3628240a4d7b', N'使用者管理', N'UserManager', N'Index', NULL, NULL, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-01-01T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL),
	(N'bc7fddad-220b-43c7-b80d-74528a0d086d', N'帳號角色管理', N'UserRoleManagement', N'Index', NULL, NULL, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-01-01T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL),
	(N'bb5bd534-13a6-45ba-a324-8cc020c5f0e2', N'範例 AJAX 模組', N'SampleDataAjax', N'Index', NULL, NULL, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-01-01T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL),
	(N'd025d8b1-200c-4ffd-b7b5-a1b43be61d7f', N'角色管理', N'RoleManagement', N'Index', NULL, NULL, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-01-01T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL),
	(N'5c2774d6-390e-4c10-bbd7-c3276335c49c', N'範例模組', N'SampleData', N'Index', NULL, NULL, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-01-01T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL)
GO

INSERT [dbo].[Users] 
	([ID], [Account], [Password], [HashKey], [FirstName], [LastName], [Email], [Title], [MediaFileID], [IsEnable], [IsDeleted], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate], [DeleteUser], [DeleteDate]) 
VALUES 
	(N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', N'Admin', N'FXw+nI3f2jdFO2cYA8DOEK10eReYRcdDBUr3v6ovqM8RJuIyYXnhOxAQSoE9GWi68OZf2PH3/Kc3vbjbYbPEgg==', N'9PfC+RrPwEG3Zw1SF+iGPw==', N'Admin', N'Admin', N'Admin@Admin.com', N'Admin', NULL, 1, 0, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-14T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL)
GO


INSERT [dbo].[Roles] 
	([ID], [RoleName], [IsEnable], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate], [DeleteUser], [DeleteDate]) 
VALUES 
	(N'56b36fe5-bbd8-4ca1-b600-baa7cd929645', N'Super', 0, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-11-02T11:12:57.410' AS DateTime), N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-11-02T11:32:34.327' AS DateTime), NULL, NULL)
GO


INSERT [dbo].[UserRoles] 
	([UserID], [RoleID], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate], [DeleteUser], [DeleteDate]) 
VALUES 
	(N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', N'56b36fe5-bbd8-4ca1-b600-baa7cd929645', N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-11-06T00:11:05.630' AS DateTime), NULL, NULL, NULL, NULL)
GO


INSERT [dbo].[Pages] 
	([ID], [SiteID], [ParentID], [Name], [PageTitle], [IsShowOnHeader], [IsShowOnFooter], [MenuType], [OuterLink], [ModuleID], [Hits], [PageIcon], [IsEnable], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate], [DeleteUser], [DeleteDate]) 
VALUES 
	(N'890e32ba-de00-48cd-8b9c-1784785a8cab', N'15e34669-cc25-48c5-85c6-6af49252cbfe', N'b7928c1b-e8e5-46c7-ac41-fc629171f10b', N'使用者管理頁', N'後台 - 使用者管理', 1, 1, 2, NULL, N'81bb133f-318c-49ed-bcd6-3628240a4d7b', 0, N'flaticon2-user', 1, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-26T14:38:15.260' AS DateTime), N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-11-01T11:13:17.770' AS DateTime), NULL, NULL), 
	(N'9fe46596-9487-4dab-9ada-1babb2861dd4', N'15e34669-cc25-48c5-85c6-6af49252cbfe', N'6878ec82-16ae-435d-abc1-4bfb1ecd7f85', N'Test SubSub Folder', N'Test SubSub Folder', 0, 0, 1, NULL, NULL, 0, NULL, 1, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-26T13:44:18.703' AS DateTime), N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-28T11:06:15.667' AS DateTime), NULL, NULL), 
	(N'6f1cfd6f-886d-4c07-978b-21231616d442', N'15e34669-cc25-48c5-85c6-6af49252cbfe', N'850ef2ca-cf6f-4eb4-959f-9d03c1242af8', N'範例 AJAX 模組頁', N'範例 AJAX 模組頁', 1, 1, 2, NULL, N'bb5bd534-13a6-45ba-a324-8cc020c5f0e2', 0, NULL, 1, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-26T14:38:40.147' AS DateTime), NULL, NULL, NULL, NULL), 
	(N'6878ec82-16ae-435d-abc1-4bfb1ecd7f85', N'15e34669-cc25-48c5-85c6-6af49252cbfe', N'e2b95ca0-8676-4876-8816-c2c9ea15b72f', N'Test Sub Folder', N'Test Sub Folder', 1, 1, 1, NULL, NULL, 0, N' flaticon2-folder', 1, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-26T14:39:00.350' AS DateTime), N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-28T15:34:38.570' AS DateTime), NULL, NULL), 
	(N'63f7adaa-231b-4ba2-bad4-6c1bc98ad175', N'15e34669-cc25-48c5-85c6-6af49252cbfe', N'b7928c1b-e8e5-46c7-ac41-fc629171f10b', N'角色管理頁', N'後台 - 角色管理頁', 0, 0, 2, NULL, N'd025d8b1-200c-4ffd-b7b5-a1b43be61d7f', 0, N'flaticon2-group', 1, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-11-01T11:11:50.450' AS DateTime), NULL, NULL, NULL, NULL), 
	(N'c0cd5d75-4898-4e39-9353-7373b15fce45', N'15e34669-cc25-48c5-85c6-6af49252cbfe', N'b7928c1b-e8e5-46c7-ac41-fc629171f10b', N'帳號角色管理', N'後台 - 帳號角色管理', 0, 0, 2, NULL, N'bc7fddad-220b-43c7-b80d-74528a0d086d', 0, N'flaticon2-user-1', 1, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-28T14:47:07.957' AS DateTime), N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-28T16:10:42.500' AS DateTime), NULL, NULL), 
	(N'850ef2ca-cf6f-4eb4-959f-9d03c1242af8', N'15e34669-cc25-48c5-85c6-6af49252cbfe', NULL, N'範例模組', N'範例模組', 1, 1, 1, NULL, NULL, 0, N' flaticon2-folder', 1, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-26T13:44:18.703' AS DateTime), NULL, NULL, NULL, NULL), 
	(N'15221d83-8ef1-4d97-958d-bd37fd80e479', N'15e34669-cc25-48c5-85c6-6af49252cbfe', N'850ef2ca-cf6f-4eb4-959f-9d03c1242af8', N'範例模組頁', N'後台 - 範例模組頁', 1, 1, 2, NULL, N'5c2774d6-390e-4c10-bbd7-c3276335c49c', 0, NULL, 1, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-26T14:38:26.467' AS DateTime), NULL, NULL, NULL, NULL), 
	(N'e2b95ca0-8676-4876-8816-c2c9ea15b72f', N'15e34669-cc25-48c5-85c6-6af49252cbfe', NULL, N'Test Folder', N'Test Folder', 1, 0, 1, NULL, NULL, 0, N' flaticon2-folder', 1, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-25T10:54:11.523' AS DateTime), N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-26T13:43:15.847' AS DateTime), NULL, NULL), 
	(N'6dee99d6-f314-436c-8be6-f8dca6de7512', N'15e34669-cc25-48c5-85c6-6af49252cbfe', N'b7928c1b-e8e5-46c7-ac41-fc629171f10b', N'選單管理頁', N'後台 - 選單管理', 1, 1, 2, NULL, N'3228be98-f7dc-450b-b7a2-0ed8b45f9ea1', 0, N'flaticon-app', 1, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-26T14:38:00.027' AS DateTime), N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-11-01T11:12:42.170' AS DateTime), NULL, NULL), 
	(N'b7928c1b-e8e5-46c7-ac41-fc629171f10b', N'15e34669-cc25-48c5-85c6-6af49252cbfe', NULL, N'後台管理', N'後台管理', 0, 0, 1, NULL, NULL, 0, N' flaticon2-folder', 1, N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-28T14:47:07.957' AS DateTime), N'f9c2f7f4-cf1a-41c0-b767-0d5217e8863f', CAST(N'2021-10-28T16:10:42.500' AS DateTime), NULL, NULL)
GO