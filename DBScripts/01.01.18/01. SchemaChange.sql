ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule1] ALTER COLUMN [Type] [nvarchar](16) NULL
ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule1] ALTER COLUMN [Supplier] [nvarchar](128) NULL
ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule1] ALTER COLUMN [EmpName] [nvarchar](64) NULL

ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule2] ALTER COLUMN [MachineName] [nvarchar](64) NULL
ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule2] ALTER COLUMN [MachineNo] [nvarchar](64) NULL
ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule2] ALTER COLUMN [OnTime] [nvarchar](16) NULL

ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule3] ALTER COLUMN [Date] [date] NULL
ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule3] ALTER COLUMN [Location] [nvarchar](64) NULL
ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule3] ALTER COLUMN [TELLoss] [nvarchar](16) NULL
ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule3] ALTER COLUMN [CustomerLoss] [nvarchar](16) NULL
ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule3] ALTER COLUMN [Accident] [nvarchar](16) NULL
ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule3] ALTER COLUMN [Description] [nvarchar](1000) NULL

ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule4] ALTER COLUMN [Date] [date] NULL
ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule4] ALTER COLUMN [Location] [nvarchar](64) NULL
ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule4] ALTER COLUMN [IsDamage] [nvarchar](16) NULL
ALTER TABLE [dbo].[TET_SPA_ScoringInfoModule4] ALTER COLUMN [Description] [nvarchar](1000) NULL




CREATE TABLE [dbo].[MailReminderExecutionLog](
	[ID] [uniqueidentifier] NOT NULL,
	[ReminderType] [nvarchar](64) NOT NULL,
	[ExecuteDate] [date] NOT NULL,
	[StartedAt] [datetime] NOT NULL,
	[FinishedAt] [datetime] NULL,
	[Status] [nvarchar](16) NOT NULL,
	[MailCount] [int] NOT NULL,
	[Message] [nvarchar](max) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_MailReminderExecutionLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_MailReminderExecutionLog_Completed]
ON [dbo].[MailReminderExecutionLog] ([ReminderType], [ExecuteDate])
WHERE [Status] = N'Completed'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提醒信排程執行紀錄' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailReminderExecutionLog'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailReminderExecutionLog', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提醒信類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailReminderExecutionLog', @level2type=N'COLUMN',@level2name=N'ReminderType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'執行日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailReminderExecutionLog', @level2type=N'COLUMN',@level2name=N'ExecuteDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'執行開始時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailReminderExecutionLog', @level2type=N'COLUMN',@level2name=N'StartedAt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'執行完成時間，執行中尚未完成時為 Null' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailReminderExecutionLog', @level2type=N'COLUMN',@level2name=N'FinishedAt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'執行狀態：Running=執行中、Completed=執行完成、Failed=執行失敗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailReminderExecutionLog', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產生信件數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailReminderExecutionLog', @level2type=N'COLUMN',@level2name=N'MailCount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'執行訊息或失敗原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailReminderExecutionLog', @level2type=N'COLUMN',@level2name=N'Message'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailReminderExecutionLog', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailReminderExecutionLog', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO


