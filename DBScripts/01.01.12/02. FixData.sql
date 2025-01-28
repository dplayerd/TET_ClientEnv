USE [TET_Supplier]
GO

-- 調整模組名稱
UPDATE [Modules] SET Name = 'SPA_ScoringInfo' WHERE Controller = 'SPA_ScoringInfo'
UPDATE [Modules] SET Name = 'SPA_ApproverSetup' WHERE Controller = 'SPA_ApproverSetup'
UPDATE [Modules] SET Name = 'SPA_EvaluationReport' WHERE Controller = 'SPA_EvaluationReport'
UPDATE [Modules] SET Name = 'SPA_ScoringRatio' WHERE Controller = 'SPA_ScoringRatio'
UPDATE [Modules] SET Name = 'SPA_Violation' WHERE Controller = 'SPA_Violation'
UPDATE [Modules] SET Name = 'SPA_Period' WHERE Controller = 'SPA_Period'
UPDATE [Modules] SET Name = 'SPA_Evaluation' WHERE Controller = 'SPA_Evaluation'
UPDATE [Modules] SET Name = 'SPA_CostService' WHERE Controller = 'SPA_CostService'
