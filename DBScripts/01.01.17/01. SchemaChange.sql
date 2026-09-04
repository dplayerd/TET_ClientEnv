/****** Object:  Table [dbo].[TET_SPA_ScoringInfoSheets]    Script Date: 2026/8/24 下午 04:33:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TET_SPA_ScoringInfoSheets](
	[ServiceItemID] [uniqueidentifier] NOT NULL,
	[POSource] [nvarchar](16) NOT NULL,
	[IsSheet1Show] [bit] NOT NULL,
	[IsSheet1TypeFill] [bit] NOT NULL,
	[IsSheet1SupplierFill] [bit] NOT NULL,
	[IsSheet1SourceFill] [bit] NOT NULL,
	[IsSheet1EmpNameFill] [bit] NOT NULL,
	[IsSheet1MajorJobFill] [bit] NOT NULL,
	[IsSheet1IsIndependentFill] [bit] NOT NULL,
	[IsSheet1SkillLevelFill] [bit] NOT NULL,
	[IsSheet1EmpStatusFill] [bit] NOT NULL,
	[IsSheet1TELSeniorityYFill] [bit] NOT NULL,
	[IsSheet1TELSeniorityMFill] [bit] NOT NULL,
	[IsSheet1RemarkFill] [bit] NOT NULL,
	[IsSheet2Show] [bit] NOT NULL,
	[IsSheet2ServiceForFill] [bit] NOT NULL,
	[IsSheet2WorkItemFill] [bit] NOT NULL,
	[IsSheet2MachineNameFill] [bit] NOT NULL,
	[IsSheet2MachineNoFill] [bit] NOT NULL,
	[IsSheet2OnTimeFill] [bit] NOT NULL,
	[IsSheet2RemarkFill] [bit] NOT NULL,
	[IsSheet3Show] [bit] NOT NULL,
	[IsSheet3WorkerCountFill] [bit] NOT NULL,
	[IsSheet3DateFill] [bit] NOT NULL,
	[IsSheet3LocationFill] [bit] NOT NULL,
	[IsSheet3TELLossFill] [bit] NOT NULL,
	[IsSheet3CustomerLossFill] [bit] NOT NULL,
	[IsSheet3AccidentFill] [bit] NOT NULL,
	[IsSheet3DescriptionFill] [bit] NOT NULL,
	[IsSheet4Show] [bit] NOT NULL,
	[IsSheet4CorrectnessFill] [bit] NOT NULL,
	[IsSheet4ContributionFill] [bit] NOT NULL,
	[IsSheet5Show] [bit] NOT NULL,
	[IsSheet5SelfTrainingFill] [bit] NOT NULL,
	[IsSheet5SelfTrainingRemarkFill] [bit] NOT NULL,
	[IsSheet6Show] [bit] NOT NULL,
	[IsSheet6CooperationFill] [bit] NOT NULL,
	[IsSheet6DateFill] [bit] NOT NULL,
	[IsSheet6LocationFill] [bit] NOT NULL,
	[IsSheet6IsDamageFill] [bit] NOT NULL,
	[IsSheet6DescriptionFill] [bit] NOT NULL,
	[IsSheet7Show] [bit] NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_ScoringInfoSheets] PRIMARY KEY CLUSTERED 
(
	[ServiceItemID] ASC,
	[POSource] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TET_SPA_ScoringInfoSheets] ADD  CONSTRAINT [DF_TET_SPA_ScoringInfoSheets_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑項目系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'ServiceItemID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PO Source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'POSource'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人工盤點頁籤是否顯示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet1Show'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人工盤點頁籤本社/協力廠商欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet1TypeFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人工盤點頁籤供應商名稱欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet1SupplierFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人工盤點頁籤資料來源欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet1SourceFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人工盤點頁籤員工姓名欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet1EmpNameFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人工盤點頁籤主要負責作業欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet1MajorJobFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人工盤點頁籤能否獨立作業欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet1IsIndependentFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人工盤點頁籤Skill Level欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet1SkillLevelFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人工盤點頁籤員工狀態欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet1EmpStatusFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人工盤點頁籤派工至TEL的年資(年)欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet1TELSeniorityYFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人工盤點頁籤派工至TEL的年資(月)欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet1TELSeniorityMFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人工盤點頁籤備註欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet1RemarkFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工達交狀況盤點頁籤是否顯示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet2Show'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工達交狀況盤點頁籤服務對象欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet2ServiceForFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工達交狀況盤點頁籤作業項目欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet2WorkItemFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工達交狀況盤點頁籤承攬機台名稱欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet2MachineNameFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工達交狀況盤點頁籤機台Serial No.欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet2MachineNoFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工達交狀況盤點頁籤是否準時交付欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet2OnTimeFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工達交狀況盤點頁籤備註欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet2RemarkFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工正確性頁籤是否顯示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet3Show'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工正確性頁籤出工人數欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet3WorkerCountFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工正確性頁籤時間欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet3DateFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工正確性頁籤地點欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet3LocationFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工正確性頁籤TEL財損欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet3TELLossFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工正確性頁籤客戶財損欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet3CustomerLossFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工正確性頁籤人身事故欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet3AccidentFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工正確性頁籤事件說明欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet3DescriptionFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作業正確性 & 人員備齊貢獻度頁籤是否顯示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet4Show'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作業正確性 & 人員備齊貢獻度頁籤作業正確性欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet4CorrectnessFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作業正確性 & 人員備齊貢獻度頁籤人員備齊貢獻度欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet4ContributionFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自訓能力頁籤是否顯示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet5Show'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自訓能力頁籤供應商自訓程度欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet5SelfTrainingFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自訓能力頁籤備註欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet5SelfTrainingRemarkFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服務頁籤是否顯示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet6Show'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服務頁籤配合度欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet6CooperationFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服務頁籤時間欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet6DateFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服務頁籤地點欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet6LocationFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服務頁籤造成財損欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet6IsDamageFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服務頁籤事件說明欄位是否必填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet6DescriptionFill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件頁籤是否顯示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'IsSheet7Show'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoSheets', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO


