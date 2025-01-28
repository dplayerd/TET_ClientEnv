USE [TET_Supplier2]
GO

/****** Object:  Table [dbo].[MailPoolWithCC]    Script Date: 2025/1/3 上午 02:51:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MailPoolWithCC](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SenderName] [nvarchar](100) NULL,
	[SenderEmail] [nvarchar](255) NOT NULL,
	[Receivers] [nvarchar](max) NULL,
	[CCs] [nvarchar](max) NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[Priority] [tinyint] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[SendDateTime] [datetime] NULL,
	[IsSent] [bit] NOT NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[RetryCount] [int] NOT NULL,
	[LastRetryDateTime] [datetime] NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_MailPoolWithCC] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


