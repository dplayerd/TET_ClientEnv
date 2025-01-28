Alter table [dbo].[TET_Supplier]
add IsActive nvarchar(10) null
go

update [dbo].[TET_Supplier] set IsActive='Active'
go