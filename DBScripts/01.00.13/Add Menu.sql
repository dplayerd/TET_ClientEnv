-- �s�W�ҲաG ������SPA��Ų�f�֪̳]�w
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'62BE6CC7-E0FC-4FA8-A940-C2ACF6EA5B2B', N'�����Ӽf�֪��ܧ�', N'SupplierApproverChange', N'Index', NULL, NULL, N'Admin', '2024-01-01', N'Admin', '2024-01-01')
GO

-- �s�W����
INSERT [TET_Supplier_Menu] 
([ID], [SiteID], [ParentID], [Name], [Description], [MenuType], [Linkurl], [ModuleID], [PageIcon], [SortNo], [IsEnable], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
(N'6E488A7C-F2B2-459F-86DF-F66F78130DED', N'15e34669-cc25-48c5-85c6-6af49252cbfe', NULL, N'�ܧ�����Ӽf�֪�', N'�ܧ�����Ӽf�֪�', 2, N'/SupplierApproverChange/index', '62BE6CC7-E0FC-4FA8-A940-C2ACF6EA5B2B', N'flaticon-app', 11, 1, N'Admin', '2024-01-01', N'Admin', '2024-01-01')
GO

-- �]�w�����v��
 INSERT [TET_Supplier_RoleMenu] 
	([ID], [MenuID], [RoleID], [AllowActs], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'6E488A7C-F2B2-459F-86DF-F66F78130DED', N'6E488A7C-F2B2-459F-86DF-F66F78130DED', N'B4EF6AE4-5873-4511-8C7B-7F1281DD1B5E', 127, N'00001', '2024-01-01', N'00001', '2024-01-01')
GO
