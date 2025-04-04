USE [TET_Supplier]
GO
/****** Object:  Table [dbo].[TET_SPA_ApproverSetup]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_ApproverSetup](
	[ServiceItemID] [uniqueidentifier] NOT NULL,
	[BUID] [uniqueidentifier] NOT NULL,
	[InfoFill] [nvarchar](640) NOT NULL,
	[InfoConfirm] [nvarchar](64) NOT NULL,
	[Lv1Apprvoer] [nvarchar](64) NOT NULL,
	[Lv2Apprvoer] [nvarchar](64) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_ApproverSetup] PRIMARY KEY CLUSTERED 
(
	[ServiceItemID] ASC,
	[BUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_CostService]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_CostService](
	[ID] [uniqueidentifier] NOT NULL,
	[Period] [nvarchar](16) NOT NULL,
	[Filler] [nvarchar](64) NOT NULL,
	[ApproveStatus] [nvarchar](16) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_CostService] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_CostServiceApproval]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_CostServiceApproval](
	[ID] [uniqueidentifier] NOT NULL,
	[CSID] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[Level] [nvarchar](32) NOT NULL,
	[Approver] [nvarchar](64) NOT NULL,
	[Result] [nvarchar](16) NULL,
	[Comment] [nvarchar](256) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_CostServiceApproval] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_CostServiceAttachments]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_CostServiceAttachments](
	[ID] [uniqueidentifier] NOT NULL,
	[CSDetailID] [uniqueidentifier] NOT NULL,
	[FilePath] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](128) NOT NULL,
	[OrgFileName] [nvarchar](128) NOT NULL,
	[FileExtension] [nvarchar](32) NOT NULL,
	[FileSize] [int] NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_CostServiceAttachments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_CostServiceDetail]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_CostServiceDetail](
	[ID] [uniqueidentifier] NOT NULL,
	[CSID] [uniqueidentifier] NOT NULL,
	[Source] [nvarchar](16) NOT NULL,
	[IsEvaluate] [nvarchar](16) NOT NULL,
	[BU] [nvarchar](64) NOT NULL,
	[ServiceFor] [nvarchar](64) NOT NULL,
	[BelongTo] [nvarchar](128) NOT NULL,
	[POSource] [nvarchar](16) NOT NULL,
	[AssessmentItem] [nvarchar](64) NOT NULL,
	[PriceDeflator] [nvarchar](64) NOT NULL,
	[PaymentTerm] [nvarchar](64) NOT NULL,
	[Cooperation] [nvarchar](64) NOT NULL,
	[Advantage] [nvarchar](1000) NULL,
	[Improved] [nvarchar](1000) NULL,
	[Comment] [nvarchar](1000) NULL,
	[Remark] [nvarchar](1000) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_CostServiceDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_Evaluation]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_Evaluation](
	[ID] [uniqueidentifier] NOT NULL,
	[Period] [nvarchar](16) NOT NULL,
	[BU] [nvarchar](64) NOT NULL,
	[ServiceItem] [nvarchar](64) NOT NULL,
	[ServiceFor] [nvarchar](64) NOT NULL,
	[BelongTo] [nvarchar](128) NOT NULL,
	[POSource] [nvarchar](16) NOT NULL,
	[PerformanceLevel] [nvarchar](16) NOT NULL,
	[TotalScore] [nvarchar](16) NOT NULL,
	[TScore] [nvarchar](16) NOT NULL,
	[DScore] [nvarchar](16) NOT NULL,
	[QScore] [nvarchar](16) NOT NULL,
	[CScore] [nvarchar](16) NOT NULL,
	[SScore] [nvarchar](16) NOT NULL,
	[TScore1] [nvarchar](16) NOT NULL,
	[TScore2] [nvarchar](16) NOT NULL,
	[DScore1] [nvarchar](16) NOT NULL,
	[DScore2] [nvarchar](16) NOT NULL,
	[QScore1] [nvarchar](16) NOT NULL,
	[QScore2] [nvarchar](16) NOT NULL,
	[CScore1] [nvarchar](16) NOT NULL,
	[CScore2] [nvarchar](16) NOT NULL,
	[SScore1] [nvarchar](16) NOT NULL,
	[SScore2] [nvarchar](16) NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_Evaluation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_EvaluationReport]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_EvaluationReport](
	[ID] [uniqueidentifier] NOT NULL,
	[Period] [nvarchar](16) NOT NULL,
	[BU] [nvarchar](64) NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_EvaluationReport] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_EvaluationReportAttachments]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_EvaluationReportAttachments](
	[ID] [uniqueidentifier] NOT NULL,
	[EPID] [uniqueidentifier] NOT NULL,
	[FileCategory] [nvarchar](32) NOT NULL,
	[FilePath] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](128) NOT NULL,
	[OrgFileName] [nvarchar](128) NOT NULL,
	[FileExtension] [nvarchar](32) NOT NULL,
	[FileSize] [int] NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_EvaluationReportAttachments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_Period]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_Period](
	[Period] [nvarchar](16) NOT NULL,
	[Status] [nvarchar](16) NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_Period] PRIMARY KEY CLUSTERED 
(
	[Period] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_ScoringInfo]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_ScoringInfo](
	[ID] [uniqueidentifier] NOT NULL,
	[Period] [nvarchar](16) NOT NULL,
	[BU] [nvarchar](64) NOT NULL,
	[ServiceItem] [nvarchar](64) NOT NULL,
	[ServiceFor] [nvarchar](64) NOT NULL,
	[BelongTo] [nvarchar](128) NOT NULL,
	[POSource] [nvarchar](16) NOT NULL,
	[ApproveStatus] [nvarchar](16) NULL,
	[MOCount] [nvarchar](16) NULL,
	[TELLoss] [nvarchar](16) NULL,
	[CustomerLoss] [nvarchar](16) NULL,
	[Accident] [nvarchar](16) NULL,
	[WorkerCount] [int] NULL,
	[Correctness] [nvarchar](16) NULL,
	[Contribution] [nvarchar](16) NULL,
	[SelfTraining] [nvarchar](64) NULL,
	[SelfTrainingRemark] [nvarchar](1000) NULL,
	[Cooperation] [nvarchar](16) NULL,
	[Complain] [nvarchar](64) NULL,
	[Advantage] [nvarchar](1000) NULL,
	[Improved] [nvarchar](1000) NULL,
	[Comment] [nvarchar](1000) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_ScoringInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_ScoringInfoApproval]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_ScoringInfoApproval](
	[ID] [uniqueidentifier] NOT NULL,
	[SIID] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[Level] [nvarchar](32) NOT NULL,
	[Approver] [nvarchar](64) NOT NULL,
	[Result] [nvarchar](16) NULL,
	[Comment] [nvarchar](256) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_ScoringInfoAttachments]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_ScoringInfoAttachments](
	[ID] [uniqueidentifier] NOT NULL,
	[SIID] [uniqueidentifier] NOT NULL,
	[FilePath] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](128) NOT NULL,
	[OrgFileName] [nvarchar](128) NOT NULL,
	[FileExtension] [nvarchar](32) NOT NULL,
	[FileSize] [int] NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_ScoringInfoAttachments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_ScoringInfoModule1]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_ScoringInfoModule1](
	[ID] [uniqueidentifier] NOT NULL,
	[SIID] [uniqueidentifier] NOT NULL,
	[Source] [nvarchar](16) NOT NULL,
	[Type] [nvarchar](16) NOT NULL,
	[Supplier] [nvarchar](128) NOT NULL,
	[EmpName] [nvarchar](64) NOT NULL,
	[MajorJob] [nvarchar](64) NULL,
	[IsIndependent] [nvarchar](16) NULL,
	[SkillLevel] [nvarchar](64) NULL,
	[TELSeniorityY] [nvarchar](4) NULL,
	[TELSeniorityM] [nvarchar](4) NULL,
	[Remark] [nvarchar](1000) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_ScoringInfoModule1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_ScoringInfoModule2]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_ScoringInfoModule2](
	[ID] [uniqueidentifier] NOT NULL,
	[SIID] [uniqueidentifier] NOT NULL,
	[ServiceFor] [nvarchar](64) NULL,
	[WorkItem] [nvarchar](64) NULL,
	[MachineName] [nvarchar](64) NOT NULL,
	[MachineNo] [nvarchar](64) NOT NULL,
	[OnTime] [nvarchar](16) NOT NULL,
	[Remark] [nvarchar](1000) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_ScoringInfoModule2] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_ScoringInfoModule3]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_ScoringInfoModule3](
	[ID] [uniqueidentifier] NOT NULL,
	[SIID] [uniqueidentifier] NOT NULL,
	[Date] [date] NOT NULL,
	[Location] [nvarchar](64) NOT NULL,
	[TELLoss] [nvarchar](16) NOT NULL,
	[CustomerLoss] [nvarchar](16) NOT NULL,
	[Accident] [nvarchar](16) NOT NULL,
	[Description] [nvarchar](1000) NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_ScoringInfoModule3] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_ScoringInfoModule4]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_ScoringInfoModule4](
	[ID] [uniqueidentifier] NOT NULL,
	[SIID] [uniqueidentifier] NOT NULL,
	[Date] [date] NOT NULL,
	[Location] [nvarchar](64) NOT NULL,
	[IsDamage] [nvarchar](16) NOT NULL,
	[Description] [nvarchar](1000) NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_ScoringInfoModule4] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_ScoringRatio]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_ScoringRatio](
	[ServiceItemID] [uniqueidentifier] NOT NULL,
	[POSource] [nvarchar](16) NOT NULL,
	[TRatio1] [decimal](7, 4) NOT NULL,
	[TRatio2] [decimal](7, 4) NOT NULL,
	[DRatio1] [decimal](7, 4) NOT NULL,
	[DRatio2] [decimal](7, 4) NOT NULL,
	[QRatio1] [decimal](7, 4) NOT NULL,
	[QRatio2] [decimal](7, 4) NOT NULL,
	[CRatio1] [decimal](7, 4) NOT NULL,
	[CRatio2] [decimal](7, 4) NOT NULL,
	[SRatio1] [decimal](7, 4) NOT NULL,
	[SRatio2] [decimal](7, 4) NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_ScoringRatio] PRIMARY KEY CLUSTERED 
(
	[ServiceItemID] ASC,
	[POSource] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_Violation]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_Violation](
	[ID] [uniqueidentifier] NOT NULL,
	[Period] [nvarchar](16) NOT NULL,
	[ApproveStatus] [nvarchar](16) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_ViolationApproval]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_ViolationApproval](
	[ID] [uniqueidentifier] NOT NULL,
	[ViolationID] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[Level] [nvarchar](32) NOT NULL,
	[Approver] [nvarchar](64) NOT NULL,
	[Result] [nvarchar](16) NULL,
	[Comment] [nvarchar](256) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_ViolationApproval] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_ViolationAttachments]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_ViolationAttachments](
	[ID] [uniqueidentifier] NOT NULL,
	[VDetailID] [uniqueidentifier] NOT NULL,
	[FilePath] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](128) NOT NULL,
	[OrgFileName] [nvarchar](128) NOT NULL,
	[FileExtension] [nvarchar](32) NOT NULL,
	[FileSize] [int] NOT NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_ViolationAttachments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TET_SPA_ViolationDetail]    Script Date: 2024/8/1 下午 05:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TET_SPA_ViolationDetail](
	[ID] [uniqueidentifier] NOT NULL,
	[ViolationID] [uniqueidentifier] NOT NULL,
	[Date] [date] NOT NULL,
	[BelongTo] [nvarchar](128) NOT NULL,
	[BU] [nvarchar](64) NOT NULL,
	[AssessmentItem] [nvarchar](64) NOT NULL,
	[MiddleCategory] [nvarchar](64) NOT NULL,
	[SmallCategory] [nvarchar](64) NOT NULL,
	[CustomerName] [nvarchar](128) NOT NULL,
	[CustomerPlant] [nvarchar](128) NOT NULL,
	[CustomerDetail] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[CreateUser] [nvarchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar](64) NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TET_SPA_ViolationDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑項目系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ApproverSetup', @level2type=N'COLUMN',@level2name=N'ServiceItemID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑單位系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ApproverSetup', @level2type=N'COLUMN',@level2name=N'BUID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'計分資料填寫者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ApproverSetup', @level2type=N'COLUMN',@level2name=N'InfoFill'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'計分資料確認者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ApproverSetup', @level2type=N'COLUMN',@level2name=N'InfoConfirm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第一關審核者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ApproverSetup', @level2type=N'COLUMN',@level2name=N'Lv1Apprvoer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第二關審核者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ApproverSetup', @level2type=N'COLUMN',@level2name=N'Lv2Apprvoer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ApproverSetup', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ApproverSetup', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ApproverSetup', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ApproverSetup', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostService', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑期間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostService', @level2type=N'COLUMN',@level2name=N'Period'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'填寫人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostService', @level2type=N'COLUMN',@level2name=N'Filler'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostService', @level2type=N'COLUMN',@level2name=N'ApproveStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostService', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostService', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostService', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostService', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceApproval', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cost&Service系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceApproval', @level2type=N'COLUMN',@level2name=N'CSID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceApproval', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceApproval', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核關卡' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceApproval', @level2type=N'COLUMN',@level2name=N'Level'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceApproval', @level2type=N'COLUMN',@level2name=N'Approver'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceApproval', @level2type=N'COLUMN',@level2name=N'Result'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核意見' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceApproval', @level2type=N'COLUMN',@level2name=N'Comment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceApproval', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceApproval', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceApproval', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceApproval', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceAttachments', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cost&Service明細系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceAttachments', @level2type=N'COLUMN',@level2name=N'CSDetailID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceAttachments', @level2type=N'COLUMN',@level2name=N'FilePath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceAttachments', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceAttachments', @level2type=N'COLUMN',@level2name=N'OrgFileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'副檔名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceAttachments', @level2type=N'COLUMN',@level2name=N'FileExtension'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceAttachments', @level2type=N'COLUMN',@level2name=N'FileSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceAttachments', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceAttachments', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceAttachments', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceAttachments', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cost&Service系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'CSID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料來源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'Source'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑與否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'IsEvaluate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'BU'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服務對象' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'ServiceFor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'受評供應商' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'BelongTo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PO Source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'POSource'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑項目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'AssessmentItem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'價格競爭力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'PriceDeflator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'付款條件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'PaymentTerm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配合度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'Cooperation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'優點、滿意、值得鼓勵之處' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'Advantage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'不滿意、期望改善之處' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'Improved'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶評論與其他補充說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'Comment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_CostServiceDetail', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑期間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'Period'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'BU'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑項目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'ServiceItem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服務對象' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'ServiceFor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'受評供應商' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'BelongTo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PO Source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'POSource'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Performance Level' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'PerformanceLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Score' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'TotalScore'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Technology Score' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'TScore'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Delivery Score' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'DScore'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quality Score' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'QScore'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cost Score' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'CScore'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Service Score' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'SScore'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Evaluation', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReport', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑期間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReport', @level2type=N'COLUMN',@level2name=N'Period'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReport', @level2type=N'COLUMN',@level2name=N'BU'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReport', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReport', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReport', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReport', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReportAttachments', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'績效評鑑報告系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReportAttachments', @level2type=N'COLUMN',@level2name=N'EPID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReportAttachments', @level2type=N'COLUMN',@level2name=N'FileCategory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReportAttachments', @level2type=N'COLUMN',@level2name=N'FilePath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReportAttachments', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReportAttachments', @level2type=N'COLUMN',@level2name=N'OrgFileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'副檔名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReportAttachments', @level2type=N'COLUMN',@level2name=N'FileExtension'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReportAttachments', @level2type=N'COLUMN',@level2name=N'FileSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReportAttachments', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReportAttachments', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReportAttachments', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_EvaluationReportAttachments', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑期間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Period', @level2type=N'COLUMN',@level2name=N'Period'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Period', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Period', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Period', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Period', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Period', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑期間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'Period'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'BU'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑項目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'ServiceItem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服務對象' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'ServiceFor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'受評供應商' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'BelongTo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PO Source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'POSource'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'ApproveStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MO次數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'MOCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'TEL財損' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'TELLoss'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶財損' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'CustomerLoss'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人身事故' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'Accident'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出工人數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'WorkerCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作業正確性' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'Correctness'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人員備齊貢獻度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'Contribution'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商自訓程度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'SelfTraining'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商自訓程度備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'SelfTrainingRemark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配合度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'Cooperation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶抱怨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'Complain'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'優點、滿意、值得鼓勵之處' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'Advantage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'不滿意、期望改善之處' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'Improved'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶評論與其他補充說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'Comment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfo', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoApproval', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑計分資料系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoApproval', @level2type=N'COLUMN',@level2name=N'SIID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoApproval', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoApproval', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核關卡' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoApproval', @level2type=N'COLUMN',@level2name=N'Level'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoApproval', @level2type=N'COLUMN',@level2name=N'Approver'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoApproval', @level2type=N'COLUMN',@level2name=N'Result'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核意見' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoApproval', @level2type=N'COLUMN',@level2name=N'Comment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoApproval', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoApproval', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoApproval', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoApproval', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoAttachments', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑計分資料系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoAttachments', @level2type=N'COLUMN',@level2name=N'SIID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoAttachments', @level2type=N'COLUMN',@level2name=N'FilePath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoAttachments', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoAttachments', @level2type=N'COLUMN',@level2name=N'OrgFileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'副檔名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoAttachments', @level2type=N'COLUMN',@level2name=N'FileExtension'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoAttachments', @level2type=N'COLUMN',@level2name=N'FileSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoAttachments', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoAttachments', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoAttachments', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoAttachments', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑計分資料系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'SIID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料來源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'Source'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本社/協力廠商' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'Supplier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'員工姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'EmpName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主要負責作業' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'MajorJob'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'能否獨立作業' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'IsIndependent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Skill Level' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'SkillLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'派工至TEL的年資(年)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'TELSeniorityY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'派工至TEL的年資(月)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'TELSeniorityM'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule1', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule2', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑計分資料系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule2', @level2type=N'COLUMN',@level2name=N'SIID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服務對象' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule2', @level2type=N'COLUMN',@level2name=N'ServiceFor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作業項目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule2', @level2type=N'COLUMN',@level2name=N'WorkItem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'承攬機台名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule2', @level2type=N'COLUMN',@level2name=N'MachineName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'機台Serial No.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule2', @level2type=N'COLUMN',@level2name=N'MachineNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否準時交付' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule2', @level2type=N'COLUMN',@level2name=N'OnTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule2', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule2', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule2', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule2', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule2', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule3', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑計分資料系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule3', @level2type=N'COLUMN',@level2name=N'SIID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule3', @level2type=N'COLUMN',@level2name=N'Date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地點' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule3', @level2type=N'COLUMN',@level2name=N'Location'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'TEL財損' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule3', @level2type=N'COLUMN',@level2name=N'TELLoss'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶財損' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule3', @level2type=N'COLUMN',@level2name=N'CustomerLoss'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人身事故' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule3', @level2type=N'COLUMN',@level2name=N'Accident'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事件說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule3', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule3', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule3', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule3', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule3', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule4', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑計分資料系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule4', @level2type=N'COLUMN',@level2name=N'SIID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule4', @level2type=N'COLUMN',@level2name=N'Date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地點' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule4', @level2type=N'COLUMN',@level2name=N'Location'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'造成財損' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule4', @level2type=N'COLUMN',@level2name=N'IsDamage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事件說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule4', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule4', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule4', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule4', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringInfoModule4', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑項目系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'ServiceItemID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PO Source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'POSource'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工正確性比例或作業正確性比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'TRatio1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施工技術水平比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'TRatio2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人員穩定度比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'DRatio1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'準時完工交付比例或人員備齊貢獻度比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'DRatio2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'守規性比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'QRatio1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自訓能力比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'QRatio2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'價格競爭力比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'CRatio1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'付款條件比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'CRatio2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶抱怨比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'SRatio1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配合度比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'SRatio2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ScoringRatio', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Violation', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑期間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Violation', @level2type=N'COLUMN',@level2name=N'Period'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Violation', @level2type=N'COLUMN',@level2name=N'ApproveStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Violation', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Violation', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Violation', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_Violation', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationApproval', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'違規紀錄系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationApproval', @level2type=N'COLUMN',@level2name=N'ViolationID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationApproval', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationApproval', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核關卡' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationApproval', @level2type=N'COLUMN',@level2name=N'Level'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationApproval', @level2type=N'COLUMN',@level2name=N'Approver'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationApproval', @level2type=N'COLUMN',@level2name=N'Result'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核意見' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationApproval', @level2type=N'COLUMN',@level2name=N'Comment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationApproval', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationApproval', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationApproval', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationApproval', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationAttachments', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'違規紀錄明細系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationAttachments', @level2type=N'COLUMN',@level2name=N'VDetailID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationAttachments', @level2type=N'COLUMN',@level2name=N'FilePath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationAttachments', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationAttachments', @level2type=N'COLUMN',@level2name=N'OrgFileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'副檔名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationAttachments', @level2type=N'COLUMN',@level2name=N'FileExtension'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationAttachments', @level2type=N'COLUMN',@level2name=N'FileSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationAttachments', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationAttachments', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationAttachments', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationAttachments', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'違規紀錄系統辨識碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'ViolationID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'Date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'BelongTo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'BU'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'評鑑項目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'AssessmentItem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'MiddleCategory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'SmallCategory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'CustomerName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'CustomerPlant'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶細分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'CustomerDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'違規事項說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TET_SPA_ViolationDetail', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
