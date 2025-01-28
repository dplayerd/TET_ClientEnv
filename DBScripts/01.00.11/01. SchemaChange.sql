USE [TET_Supplier]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailPool](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SenderName] [nvarchar](100) NULL,
	[SenderEmail] [nvarchar](255) NOT NULL,
	[RecipientName] [nvarchar](100) NULL,
	[RecipientEmail] [nvarchar](255) NOT NULL,
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
 CONSTRAINT [PK__MailPool__09A874FAEBEDF57B] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
