USE [TET_Supplier]
GO

alter table [dbo].[TET_SPA_ViolationDetail]
alter column customerdetail nvarchar(128) null
go

alter table [dbo].[TET_SPA_ViolationDetail]
alter column Description nvarchar(1000) not null
go

update TET_SPA_Tooltips set Description=N'即客戶端違規統計的[客戶名稱]欄位'
where ModuleName='SPA_Violation' and FieldName='CustomerName'
go