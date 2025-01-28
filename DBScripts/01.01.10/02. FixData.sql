USE [TET_Supplier]
GO

-- 新增模組： 供應商SPA評鑑
INSERT [Modules] ([ID], [Name], [Controller], [Action], [AdminController], [AdminAction], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'805716ED-862C-49E6-B035-7C2B4528AE41', N'供應商SPA評鑑', N'SPA_Evaluation', N'Index', NULL, NULL, N'Admin', '2024-01-01', N'Admin', '2024-01-01')


-- 新增頁面
INSERT [TET_Supplier_Menu] 
([ID], [SiteID], [ParentID], [Name], [Description], [MenuType], [Linkurl], [ModuleID], [PageIcon], [SortNo], [IsEnable], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
(N'A2442E02-6760-4030-8772-E8396434C5F6', N'15e34669-cc25-48c5-85c6-6af49252cbfe', NULL, N'供應商SPA評鑑', N'供應商SPA評鑑', 2, N'/Supplier/SPA_Evaluation/index', '805716ED-862C-49E6-B035-7C2B4528AE41', N'flaticon-app', 11, 1, N'Admin', '2024-01-01', N'Admin', '2024-01-01')
GO


-- 設定頁面權限
 INSERT [TET_Supplier_RoleMenu] 
	([ID], [MenuID], [RoleID], [AllowActs], [CreateUser], [CreateDate], [ModifyUser], [ModifyDate]) 
VALUES 
	(N'A2442E02-6760-4030-8772-E8396434C5F6', N'A2442E02-6760-4030-8772-E8396434C5F6', N'B4EF6AE4-5873-4511-8C7B-7F1281DD1B5E', 127, N'00001', '2024-01-01', N'00001', '2024-01-01')




update TET_Parameters set Seq=4 where id='F18089CA-61A4-4B89-AD2A-65A76A53256D'
update TET_Parameters set Seq=3 where id='71FB65DC-CF1F-45BA-BED7-9858966E0AB5'
update TET_Parameters set Seq=2 where id='38867F4A-303F-4C6D-9F59-3288DCD1BE66'
update TET_Parameters set Seq=1 where id='1EB9317E-D4E9-43B8-A8F7-B513FC9C099B'
update TET_Parameters set Seq=0 where id='CFCBF6DA-3E88-4CD5-998A-3028C794BC38'
update TET_Parameters set Seq=0 where id='8A694C63-636E-41BC-8388-78BB17D259A4'
update TET_Parameters set Seq=2 where id='5CD42F91-D7E7-4B26-86A2-1AE0D67B8749'
update TET_Parameters set Seq=4 where id='2976E3F9-A339-4744-AC78-B559F869B9E0'

update TET_Parameters set Seq=4 where id='C41FBCEA-F18C-4A9E-8C46-75B0DA459C7B'
update TET_Parameters set Seq=3 where id='714B4776-AF12-49CC-869C-58D7B19BA263'
update TET_Parameters set Seq=2 where id='7A188102-DEFD-497B-8BD3-F9D85D8B7CD2'
update TET_Parameters set Seq=1 where id='981D1573-C485-4CDB-A149-25FF48FD091E'
update TET_Parameters set Seq=0 where id='F7A87378-2F50-46F0-9FF7-52F5911723F4'