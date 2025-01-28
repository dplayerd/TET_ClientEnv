USE [TET_Supplier]
GO

/****** Object:  Table [dbo].[TET_SPA_Tooltips]    Script Date: 2024/10/22 上午 08:11:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TET_SPA_Tooltips](
	[ID] [uniqueidentifier] NOT NULL,
	[ModuleName] [nvarchar](64) NOT NULL,
	[FieldName] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](512) NOT NULL,
 CONSTRAINT [PK_TET_SPA_Tooltips] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TET_SPA_Tooltips] ADD  CONSTRAINT [DF_TET_SPA_Tooltips_ID]  DEFAULT (newid()) FOR [ID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Tooltips', @level2type=N'COLUMN',@level2name=N'ModuleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'欄位名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Tooltips', @level2type=N'COLUMN',@level2name=N'FieldName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'欄位說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Tooltips', @level2type=N'COLUMN',@level2name=N'Description'
GO


