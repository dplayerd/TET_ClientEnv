USE [TET_Supplier]
GO


-- 新增資料： TET_Parameters 裡面的 SupplierStatus
INSERT INTO TET_Supplier.dbo.TET_Parameters (ID,[Type],Item,Seq,IsEnable,CreateUser,CreateDate,ModifyUser,ModifyDate) VALUES
	 (N'035B6C43-57C5-4204-BBFC-143D45C26C5F',N'供應商狀態',N'Yes',1,1,N'Admin','2023-12-15 13:10:23.477',N'Admin','2023-12-15 13:10:23.477'),
	 (N'93F46E6D-CB3C-4E67-B53B-DD0C50D8D2BD',N'供應商狀態',N'No', 2,1,N'Admin','2023-12-15 13:10:23.477',N'Admin','2023-12-15 13:10:23.477');