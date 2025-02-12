using BI.Shared;
using BI.SPA_ApproverSetup;
using BI.SPA_ApproverSetup.Models;
using BI.SPA_CostService.Models;
using BI.SPA_Evaluation.Models;
using BI.SPA_ScoringInfo.Models;
using BI.SPA_Violation.Models;
using Platform.AbstractionClass;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Evaluation
{
    internal class Calculator
    {
        private const string _fixText_NA = "NA";
        private const string _fixText_Yes = "Yes";
        private const string _fixText_No = "No";
        private const string _fixText_Startup = "Startup";
        private const string _fixText_FE = "FE";
        private const string _fixText_Safety = "Safety";
        private const string _fixText_DSS_non_startup = "Non-startup(DSS)";
        private const string _fixText_DSS_startup = "Startup(DSS)";
        private const string _fixText_CT = "CT";
        private const string _fixText_EmpStatus1 = "在職";
        private const string _fixText_EmpStatus2 = "離職";
        private const string _fixText_EmpStatus3 = "新進";
        private readonly static string[] _fixText_SkillLevelArray = { "S0", "S1", "S2", "S3", "S4", "S5" };
        private const string _fixText_MainJob1 = "間接";
        private const string _fixText_PIP = "PIP";
        private const string _fixText_PPE = "PPE";

        private const string _fixTextScoreLevel1 = "Ⅰ";
        private const string _fixTextScoreLevel2 = "Ⅱ";
        private const string _fixTextScoreLevel3 = "Ⅲ";
        private const string _fixTextScoreLevel4 = "Ⅳ";
        private const string _fixTextScoreLevel5 = "Ⅴ";

        private const string _fixTextSelfTraining1 = "完全由供應商自行訓練";
        private const string _fixTextSelfTraining2 = "部分人員須由TEL代訓";
        private const string _fixTextSelfTraining3 = "完全由TEL代訓";

        private const string _fixTextComplain1 = "無";
        private const string _fixTextComplain2 = "有抱怨，未造成客戶或TEL損失";
        private const string _fixTextComplain3 = "有抱怨，且造成客戶或TEL損失";


        private TET_ParametersManager _paramMgr = new TET_ParametersManager();
        private SPA_ScoringRatioManager _scoringRatioMgr = new SPA_ScoringRatioManager();

        /// <summary> 產生計算分數後的結果 </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <param name="cDate"></param>
        /// <returns></returns>
        public List<TET_SPA_Evaluation> ComputeAllScore(SPA_Eva_PeriodModel model, string userID, DateTime cDate)
        {
            var resultList = new List<TET_SPA_Evaluation>();
            var ratioList = this._scoringRatioMgr.GetList(userID, cDate, new Pager() { AllowPaging = false });


            foreach (var item in model.ScoringInfoList)
            {
                // 找出 CostService Detail 清單
                // 評鑑單位、服務對象、受評供應商、POSource、評鑑項目 都必須相同
                var csDetailList = model.CostServiceList.SelectMany(obj => obj.DetailList).Where(obj => obj.BU == item.BU && obj.AssessmentItem == item.ServiceItem && obj.BelongTo == item.BelongTo && obj.POSource == item.POSource && obj.ServiceFor == item.ServiceFor).ToList();


                // 找出 Violation Detail 清單
                // 評鑑單位、服務對象、受評供應商、POSource、評鑑項目 都必須相同
                var vioDetailList = model.ViolationList.SelectMany(obj => obj.DetailList).Where(obj => obj.BU == item.BU && obj.AssessmentItem == item.ServiceItem && obj.BelongTo == item.BelongTo).ToList();


                // 找出計分比例
                var ratio = ratioList.Where(obj => obj.POSource == item.POSource && obj.ServiceItem == item.ServiceItem).FirstOrDefault();


                //TScore1, TScore2, DScore1, DScore2, QScore1, QScore2, CScore1, CScore2, SScore1, SScore2
                decimal? TScore1 = ComputeTScore1(item, csDetailList, vioDetailList);
                decimal? TScore2 = ComputeTScore2(item, csDetailList, vioDetailList);
                decimal? DScore1 = ComputeDScore1(item, csDetailList, vioDetailList);
                decimal? DScore2 = ComputeDScore2(item, csDetailList, vioDetailList);
                decimal? QScore1 = ComputeQScore1(item, csDetailList, vioDetailList);
                decimal? QScore2 = ComputeQScore2(item, csDetailList, vioDetailList);
                decimal? CScore1 = ComputeCScore1(item, csDetailList, vioDetailList);
                decimal? CScore2 = ComputeCScore2(item, csDetailList, vioDetailList);
                decimal? SScore1 = ComputeSScore1(item, csDetailList, vioDetailList);
                decimal? SScore2 = ComputeSScore2(item, csDetailList, vioDetailList);

                //TScore, DScore, QScore, CScore, SScore
                decimal? TScore = null;
                decimal? DScore = null;
                decimal? QScore = null;
                decimal? CScore = null;
                decimal? SScore = null;

                // TotalScore
                decimal? totalScore = null;

                // Performance Level
                string performanceLevel = "NA";

                if (ratio != null)
                {
                    TScore = ComputeTDQCS(TScore1, TScore2, ratio.TRatio1, ratio.TRatio2);
                    DScore = ComputeTDQCS(DScore1, DScore2, ratio.DRatio1, ratio.DRatio2);
                    QScore = ComputeTDQCS(QScore1, QScore2, ratio.QRatio1, ratio.QRatio2);
                    CScore = ComputeTDQCS(CScore1, CScore2, ratio.CRatio1, ratio.CRatio2);
                    SScore = ComputeTDQCS(SScore1, SScore2, ratio.SRatio1, ratio.SRatio2);

                    // 將TDQCS分數，依照SPA評鑑計分比例設定的比例，算出Total Score，四捨五入到小數第二位
                    totalScore =
                        (
                        (TScore1 ?? 0) * (ratio.TRatio1) +
                        (TScore2 ?? 0) * (ratio.TRatio2) +
                        (DScore1 ?? 0) * (ratio.DRatio1) +
                        (DScore2 ?? 0) * (ratio.DRatio2) +
                        (QScore1 ?? 0) * (ratio.QRatio1) +
                        (QScore2 ?? 0) * (ratio.QRatio2) +
                        (CScore1 ?? 0) * (ratio.CRatio1) +
                        (CScore2 ?? 0) * (ratio.CRatio2) +
                        (SScore1 ?? 0) * (ratio.SRatio1) +
                        (SScore2 ?? 0) * (ratio.SRatio2)
                        //(TScore ?? 0) * (ratio.TRatio1 + ratio.TRatio2) +
                        //(DScore ?? 0) * (ratio.DRatio1 + ratio.DRatio2) +
                        //(QScore ?? 0) * (ratio.QRatio1 + ratio.QRatio2) +
                        //(CScore ?? 0) * (ratio.CRatio1 + ratio.CRatio2) +
                        //(SScore ?? 0) * (ratio.SRatio1 + ratio.SRatio2)
                        ) / 100;

                    totalScore = Math.Round(totalScore.Value, 2);


                    //  計算Performance Level([TET_SPA_Evaluation].[PerformanceLevel])
                    //  Total Score ≧ 3.6: Performance Level=V 
                    //  3.2 ≦ Total Score < 3.6: Performance Level=IV
                    //  2.8 ≦ Total Score < 3.2: Performance Level=III
                    //  2.0 ≦ Total Score < 2.8: Performance Level=II
                    //  Total Score < 2.0: Performance Level=I
                    if (totalScore >= 3.6M)
                        performanceLevel = _fixTextScoreLevel5;
                    else if (totalScore >= 3.2M && totalScore < 3.6M)
                        performanceLevel = _fixTextScoreLevel4;
                    else if (totalScore >= 2.8M && totalScore < 3.2M)
                        performanceLevel = _fixTextScoreLevel3;
                    else if (totalScore >= 2.0M && totalScore < 2.8M)
                        performanceLevel = _fixTextScoreLevel2;
                    else if (totalScore < 2.0M)
                        performanceLevel = _fixTextScoreLevel1;
                }



                TET_SPA_Evaluation dbEntity = new TET_SPA_Evaluation()
                {
                    ID = Guid.NewGuid(),

                    Period = model.Period,
                    BU = item.BU,
                    ServiceItem = item.ServiceItem,
                    ServiceFor = item.ServiceFor,
                    BelongTo = item.BelongTo,
                    POSource = item.POSource,

                    PerformanceLevel = performanceLevel,
                    TotalScore = ToNumberText(totalScore),
                    TScore = ToNumberText(TScore),
                    DScore = ToNumberText(DScore),
                    QScore = ToNumberText(QScore),
                    CScore = ToNumberText(CScore),
                    SScore = ToNumberText(SScore),

                    TScore1 = ToNumberText(TScore1),
                    TScore2 = ToNumberText(TScore2),
                    DScore1 = ToNumberText(DScore1),
                    DScore2 = ToNumberText(DScore2),
                    QScore1 = ToNumberText(QScore1),
                    QScore2 = ToNumberText(QScore2),
                    CScore1 = ToNumberText(CScore1),
                    CScore2 = ToNumberText(CScore2),
                    SScore1 = ToNumberText(SScore1),
                    SScore2 = ToNumberText(SScore2),

                    CreateUser = userID,
                    CreateDate = cDate,
                    ModifyUser = userID,
                    ModifyDate = cDate,
                };

                resultList.Add(dbEntity);
            }


            return resultList;
        }

        #region 計算基本分數 (TScore1, TScore2, DScore1, DScore2, QScore1, QScore2, CScore1, CScore2, SScore1, SScore2)
        /// <summary> 計算施工正確性 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? ComputeTScore1(SPA_ScoringInfoModel model, List<SPA_CostServiceDetailModel> costServiceDetailModelList, List<SPA_ViolationDetailModel> violationDetailModelList)
        {
            int? result = null;

            if (IsTextInArray(model.ServiceItem, new string[] { _fixText_Startup, _fixText_FE }))
            {
                // 施工正確性(Startup、FE)([TET_SPA_Evaluation].[TScore1])
                //  R=MO次數/出工人數 ([TET_SPA_ScoringInfo].[MOCount]/[TET_SPA_ScoringInfo].[WorkerCount])
                if (model.WorkerCount.HasValue && model.MOCount != null)
                {
                    decimal rVal = decimal.Parse(model.MOCount ?? "0") / (decimal)(model.WorkerCount ?? 0);

                    //R = 0: 4分
                    //  0 < R < 0.01: 3分
                    //  0.01 ≦ R < 0.03 or 有TEL財損([TET_SPA_ScoringInfo].[TELLoss]=Yes) : 2分
                    //  0.03 ≦ R < 0.05 or 有客戶財損([TET_SPA_ScoringInfo].[CustomerLoss]=Yes): 1分
                    //  R ≦ 0.05 or有人身事故([TET_SPA_ScoringInfo].[Accident]=Yes): 0分
                    if (rVal >= 0.05M || IsTextInArray(model.Accident, _fixText_Yes))
                        result = 0;
                    else if ((rVal >= 0.03M && rVal < 0.05M) || IsTextInArray(model.CustomerLoss, _fixText_Yes))
                        result = 1;
                    else if ((rVal >= 0.01M && rVal < 0.03M) || IsTextInArray(model.TELLoss, _fixText_Yes))
                        result = 2;
                    else if (rVal > 0 && rVal < 0.01M)
                        result = 3; 
                    else if (rVal == 0)
                        result = 4;              
                }
            }
            else if (IsTextInArray(model.ServiceItem, _fixText_Safety))
            {
                // 作業正確性(Safety)([TET_SPA_Evaluation].[TScore1])
                // 依照作業正確性的值給分([TET_SPA_ScoringInfo].[Correctness])
                result = Convert.ToInt32(model.Correctness);
            }

            return result;
        }

        /// <summary> 計算施工技術水平 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? ComputeTScore2(SPA_ScoringInfoModel model, List<SPA_CostServiceDetailModel> costServiceDetailModelList, List<SPA_ViolationDetailModel> violationDetailModelList)
        {
            int? result = null;
            decimal? rVal = null;

            if (IsTextInArray(model.ServiceItem, new string[] { _fixText_Startup, _fixText_FE }))
            {
                if (IsTextInArray(model.ServiceItem, _fixText_Startup) && IsTextInArray(model.BU, _fixText_CT))
                {
                    // 施工技術水平(Startup、FE)([TET_SPA_Evaluation].[TScore2])
                    // 經TET認證可獨立作業人員比率(R): 


                    //R=人力盤點: 員工狀態=在職or新進、Skill Level=(S0~S5)、能否獨立作業=O / 員工狀態=在職or新  
                    //進、Skill Level=(S0~S5) 
                    //([TET_SPA_ScoringInfoModule1].[EmpStatus] =在職or新進 and   
                    //[TET_SPA_ScoringInfoModule1].[SkillLevel]=(S0~S5) and 
                    //[TET_SPA_ScoringInfoModule1].[IsIndependent]=O / [TET_SPA_ScoringInfoModule1].[EmpStatus] = 
                    //在職or新進 and [TET_SPA_ScoringInfoModule1].[SkillLevel]=(S0~S5))

                    string[] empStatusArray = { _fixText_EmpStatus1, _fixText_EmpStatus3 };
                    var sourceList = model.Module1List.Where(obj => IsTextInArray(obj.EmpStatus, empStatusArray) && IsTextInArray(obj.SkillLevel, _fixText_SkillLevelArray));

                    if (sourceList.Any())
                    {
                        // 除數
                        var divisorList = sourceList;

                        // 被除數
                        var dividendList = sourceList.Where(obj => obj.IsIndependent == "O");

                        rVal = (decimal)dividendList.Count() / (decimal)divisorList.Count();
                    }
                }
                else
                {
                    //其他: 
                    //R=人力盤點: 員工狀態=在職or新進、主要負責作業！＝間接、能否獨立作業=O/員工狀態=在職or新 
                    //進、主要負責作業！＝間接
                    //([TET_SPA_ScoringInfoModule1].[EmpStatus] =在職or新進 and   
                    //[TET_SPA_ScoringInfoModule1].[MajorJob]<>間接 and 
                    //[TET_SPA_ScoringInfoModule1].[IsIndependent]=O / [TET_SPA_ScoringInfoModule1].[EmpStatus] =
                    //在職or新進 and [TET_SPA_ScoringInfoModule1].[MajorJob]<>間接

                    string[] empStatusArray = { _fixText_EmpStatus1, _fixText_EmpStatus3 };
                    var sourceList = model.Module1List.Where(obj => IsTextInArray(obj.EmpStatus, empStatusArray) && obj.MajorJob != _fixText_MainJob1);

                    if (sourceList.Any())
                    {     // 除數
                        var divisorList = sourceList;

                        // 被除數
                        var dividendList = sourceList.Where(obj => obj.IsIndependent == "O");

                        rVal = (decimal)dividendList.Count() / (decimal)divisorList.Count();
                    }
                }
            }


            if (rVal.HasValue)
            {
                //R ≧ 0.75: 4分
                //0.5 ≦ R < 0.75: 3分
                //0.25 ≦ R < 0.5: 2分
                //0.05 ≦ R < 0.25: 1分
                //R < 0.05: 0分
                if (rVal >= 0.75M)
                    result = 4;
                else if (rVal >= 0.5M && rVal < 0.75M)
                    result = 3;
                else if (rVal >= 0.25M && rVal < 0.5M)
                    result = 2;
                else if (rVal >= 0.05M && rVal < 0.25M)
                    result = 1;
                else if (rVal <= 0.05M)
                    result = 0;
            }

            return result;
        }

        /// <summary> 計算人員穩定度 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? ComputeDScore1(SPA_ScoringInfoModel model, List<SPA_CostServiceDetailModel> costServiceDetailModelList, List<SPA_ViolationDetailModel> violationDetailModelList)
        {
            int? result = null;
            decimal? rVal = null;

            if (IsTextInArray(model.ServiceItem, new string[] { _fixText_Startup, _fixText_FE, _fixText_Safety }))
            {
                //-人員穩定度(Startup、FE、Safety)([TET_SPA_Evaluation].[DScore1])
                //R=人力盤點: 員工狀態=在職、主要負責作業！＝間接/員工狀態=在職or離職、主要負責作業！＝間接
                //([TET_SPA_ScoringInfoModule1].[EmpStatus] =在職 and   
                //[TET_SPA_ScoringInfoModule1].[MajorJob]<>間接 / [TET_SPA_ScoringInfoModule1].[EmpStatus] =  
                //在職or離職 and [TET_SPA_ScoringInfoModule1].[MajorJob]<>間接)



                // 除數
                var divisorList = model.Module1List.Where(obj => IsTextInArray(obj.EmpStatus, new string[] { _fixText_EmpStatus1, _fixText_EmpStatus2 }) && obj.MajorJob != _fixText_MainJob1);

                // 被除數
                var dividendList = model.Module1List.Where(obj => obj.EmpStatus == _fixText_EmpStatus1 && obj.MajorJob != _fixText_MainJob1);

                if (divisorList.Any())
                    rVal = (decimal)dividendList.Count() / (decimal)divisorList.Count();
                else
                    rVal = 0.0M;
            }


            if (rVal.HasValue)
            {
                //R ≧ 0.95: 4分
                //0.9 ≦ R < 0.95: 3分
                //0.85 ≦ R < 0.9: 2分
                //0.80 ≦ R < 0.85: 1分
                //R < 0.8: 0分
                if (rVal >= 0.95M)
                    result = 4;
                else if (rVal >= 0.9M && rVal < 0.95M)
                    result = 3;
                else if (rVal >= 0.85M && rVal < 0.9M)
                    result = 2;
                else if (rVal >= 0.8M && rVal < 0.85M)
                    result = 1;
                else if (rVal <= 0.8M)
                    result = 0;
            }

            return result;
        }

        /// <summary> 計算準時完工交付 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? ComputeDScore2(SPA_ScoringInfoModel model, List<SPA_CostServiceDetailModel> costServiceDetailModelList, List<SPA_ViolationDetailModel> violationDetailModelList)
        {
            int? result = null;
            decimal? rVal = null;

            // 準時完工交付(Startup、Startup(DSS)、Non-startup(DSS))([TET_SPA_Evaluation].[DScore2])
            //  R=施工達交狀況盤點: 是否準時交付=Yes筆數/總比數
            //  ([TET_SPA_ScoringInfoModule2].[OnTime]=Yes / [TET_SPA_ScoringInfoModule2].[OnTime]=Yes or No)
            if (IsTextInArray(model.ServiceItem, new string[] { _fixText_Startup, _fixText_DSS_startup, _fixText_DSS_non_startup }))
            {
                // 除數
                var divisorList = model.Module2List.Where(obj => IsTextInArray(obj.OnTime, new string[] { _fixText_Yes, _fixText_No }));

                // 被除數
                var dividendList = model.Module2List.Where(obj => IsTextInArray(obj.OnTime, new string[] { _fixText_Yes }));

                if (divisorList.Any())
                {
                    rVal = (decimal)dividendList.Count() / (decimal)divisorList.Count();

                    //  R = 1: 4分
                    //  0.9 ≦ R < 1: 3分
                    //  0.7 ≦ R < 0.9: 2分
                    //  0.5 ≦ R < 0.7: 1分
                    //  R < 0.5: 0分
                    if (rVal == 1M)
                        result = 4;
                    else if (rVal >= 0.9M && rVal < 1M)
                        result = 3;
                    else if (rVal >= 0.7M && rVal < 0.9M)
                        result = 2;
                    else if (rVal >= 0.5M && rVal < 0.7M)
                        result = 1;
                    else if (rVal <= 0.5M)
                        result = 0;
                }
                else
                    result = 0;
            }
            else if (IsTextInArray(model.ServiceItem, new string[] { _fixText_Safety }))
            {
                //-人員備齊貢獻度(Safety) ([TET_SPA_Evaluation].[DScore2])
                //依照人員備齊貢獻度的值給分([TET_SPA_ScoringInfo].[Contribution])

                if (int.TryParse(model.Contribution, out int tempInt))
                    result = tempInt;
            }


            return result;
        }

        /// <summary> 計算守規性 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? ComputeQScore1(SPA_ScoringInfoModel model, List<SPA_CostServiceDetailModel> costServiceDetailModelList, List<SPA_ViolationDetailModel> violationDetailModelList)
        {
            int? result = null;
            decimal? rVal = 0;

            //-守規性(Startup、FE、Safety、Startup(DSS)、Non-startup(DSS))([TET_SPA_Evaluation].[QScore1])
            //依照SRI違規紀錄資料給分

            //無任何違規紀錄: 4分
            //有違規紀錄，但是無重大違規: 2分
            //有違規紀錄，且有PPE或PIP重大違規: 0分
            if (IsTextInArray(model.ServiceItem, new string[] { _fixText_Startup, _fixText_FE, _fixText_Safety, _fixText_DSS_startup, _fixText_DSS_non_startup }))
            {
                if (!violationDetailModelList.Any())
                    result = 4;
                else
                {
                    if (violationDetailModelList.Where(obj =>
                        IsTextInArray(obj.MiddleCategory, new string[] { _fixText_PIP, _fixText_PPE }) ||
                        IsTextInArray(obj.SmallCategory, new string[] { _fixText_PIP, _fixText_PPE })).Any()
                    )
                    {
                        result = 0;
                    }
                    else
                    {
                        result = 2;
                    }
                }

            }

            return result;
        }

        /// <summary> 計算自訓能力 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? ComputeQScore2(SPA_ScoringInfoModel model, List<SPA_CostServiceDetailModel> costServiceDetailModelList, List<SPA_ViolationDetailModel> violationDetailModelList)
        {
            int? result = null;

            // -自訓能力(Startup、Safety) ([TET_SPA_Evaluation].[QScore2])
            // 依照人員備齊貢獻度的值給分([TET_SPA_ScoringInfo].[SelfTrainingRemark])
            // 完全由供應商自行訓練: 4分
            // 部分人員須由TEL代訓: 2分
            // 完全由TEL代訓: 0分
            if (IsTextInArray(model.ServiceItem, new string[] { _fixText_Startup, _fixText_Safety }))
            {
                if (IsTextInArray(model.SelfTraining, _fixTextSelfTraining1))
                    result = 4;
                else if (IsTextInArray(model.SelfTraining, _fixTextSelfTraining2))
                    result = 2;
                else if (IsTextInArray(model.SelfTraining, _fixTextSelfTraining3))
                    result = 0;
            }

            return result;
        }

        /// <summary> 計算價格競爭力 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? ComputeCScore1(SPA_ScoringInfoModel model, List<SPA_CostServiceDetailModel> costServiceDetailModelList, List<SPA_ViolationDetailModel> violationDetailModelList)
        {
            int? result = null;

            //-價格競爭力(Startup、FE、Safety、Startup(DSS)、Non-startup(DSS)) ([TET_SPA_Evaluation].[CScore1])
            //依照Cost & Service資料，價格競爭力的值給分([TET_SPA_CostServiceDetail].[PriceDeflator])
            //    NA: NA
            //    其餘: 分數=常用參數設定中，該選項的Seq的值
            if (IsTextInArray(model.ServiceItem, new string[] { _fixText_Startup, _fixText_FE, _fixText_Safety, _fixText_DSS_startup, _fixText_DSS_non_startup }))
            {
                if (costServiceDetailModelList.Count > 0)
                {
                    var costService = costServiceDetailModelList.FirstOrDefault();

                    if (!IsTextInArray(costService.PriceDeflator, _fixText_NA))
                    {
                        var scoreList = this._paramMgr.GetTET_ParametersList("SPA價格競爭力");

                        var selectedParam = scoreList.Where(obj => obj.Item == costService.PriceDeflator).FirstOrDefault();
                        if (selectedParam != null)
                            result = selectedParam.Seq;
                    }
                }
            }

            return result;
        }

        /// <summary> 計算付款條件 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? ComputeCScore2(SPA_ScoringInfoModel model, List<SPA_CostServiceDetailModel> costServiceDetailModelList, List<SPA_ViolationDetailModel> violationDetailModelList)
        {
            int? result = null;

            //-付款條件(Startup、FE、Safety、Startup(DSS)、Non-startup(DSS))   ([TET_SPA_Evaluation].[CScore2])
            //依照Cost & Service資料，付款條件的值給分([TET_SPA_CostServiceDetail].[PaymentTerm])
            //NA: NA
            //其餘: 分數=常用參數設定中，該選項的Seq的值
            if (IsTextInArray(model.ServiceItem, new string[] { _fixText_Startup, _fixText_FE, _fixText_Safety, _fixText_DSS_startup, _fixText_DSS_non_startup }))
            {
                if (costServiceDetailModelList.Count > 0)
                {
                    var costService = costServiceDetailModelList.FirstOrDefault();

                    if (!IsTextInArray(costService.PaymentTerm, _fixText_NA))
                    {
                        var scoreList = this._paramMgr.GetTET_ParametersList("SPA付款條件");

                        var selectedParam = scoreList.Where(obj => obj.Item == costService.PaymentTerm).FirstOrDefault();
                        if (selectedParam != null)
                            result = selectedParam.Seq;
                    }
                }
            }

            return result;
        }

        /// <summary> 計算客戶抱怨 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? ComputeSScore1(SPA_ScoringInfoModel model, List<SPA_CostServiceDetailModel> costServiceDetailModelList, List<SPA_ViolationDetailModel> violationDetailModelList)
        {
            int? result = null;

            //  客戶抱怨(Startup、FE、Safety、Startup(DSS)、Non-startup(DSS)) ([TET_SPA_Evaluation].[SScore1])
            //  依照客戶抱怨的值給分([TET_SPA_ScoringInfo].[Complain])

            //  無: 4分
            //  有抱怨，未造成客戶或TEL損失: 1分
            //  有抱怨，且造成客戶或TEL損失:0分

            if (IsTextInArray(model.ServiceItem, new string[] { _fixText_Startup, _fixText_FE, _fixText_Safety, _fixText_DSS_startup, _fixText_DSS_non_startup }))
            {
                if (model.Complain == null || IsTextInArray(model.Complain, _fixTextComplain1))
                    result = 4;
                else if (IsTextInArray(model.Complain, _fixTextComplain2))
                    result = 1;
                else if (IsTextInArray(model.Complain, _fixTextComplain3))
                    result = 0;
            }

            return result;
        }

        /// <summary> 計算配合度 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public decimal? ComputeSScore2(SPA_ScoringInfoModel model, List<SPA_CostServiceDetailModel> costServiceDetailModelList, List<SPA_ViolationDetailModel> violationDetailModelList)
        {
            decimal? result = null;

            // -配合度(Startup、FE、Safety、Startup(DSS)、Non-startup(DSS)) ([TET_SPA_Evaluation].[SScore2])
            // 依照Cost & Service資料，配合度的值([TET_SPA_CostServiceDetail].[Cooperation]) * 0.5 + 依照  
            // 配合度的值([TET_SPA_ScoringInfo].[Cooperation]) * 0.5給分

            // 若兩值都=NA: 分數=NA
            // 若只有一值=NA: 分數=另一值在常用參數設定中，該選項的Seq的值；
            // 若沒有值=NA: 分數=兩個值在常用參數設定中，該選項的Seq的值 * 0.5的加總
            if (IsTextInArray(model.ServiceItem, new string[] { _fixText_Startup, _fixText_FE, _fixText_Safety, _fixText_DSS_startup, _fixText_DSS_non_startup }))
            {
                var scoreList = this._paramMgr.GetTET_ParametersList("SPA配合度");
                decimal? cooperationInScoringInfo = null;
                decimal? cooperationInCostService = null;

                if (costServiceDetailModelList.Count > 0)
                {
                    var costService = costServiceDetailModelList.FirstOrDefault();

                    if (!IsTextInArray(costService.Cooperation, _fixText_NA))
                    {
                        var selectedParam = scoreList.Where(obj => obj.Item == costService.Cooperation).FirstOrDefault();
                        if (selectedParam != null)
                            cooperationInCostService = selectedParam.Seq;
                    }
                }

                if (!IsTextInArray(model.Cooperation, _fixText_NA))
                {
                    var selectedParam = scoreList.Where(obj => obj.Item == model.Cooperation).FirstOrDefault();
                    if (selectedParam != null)
                        cooperationInScoringInfo = selectedParam.Seq;
                }


                if (cooperationInCostService.HasValue && cooperationInScoringInfo.HasValue)
                    result = ((cooperationInScoringInfo.Value + cooperationInCostService.Value) / 2);
                else if (cooperationInCostService.HasValue)
                    result = cooperationInCostService.Value;
                else if (cooperationInScoringInfo.HasValue)
                    result = cooperationInScoringInfo.Value;
            }

            return result;
        }
        #endregion

        #region 計算分數
        /// <summary> 計算 TDQCS 原始分數 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public decimal? ComputeTDQCS(decimal? score1, decimal? score2, decimal ratio1, decimal ratio2)
        {
            //2.將十項單項分數，依照SPA評鑑計分比例設定的比例，算出TDQCS Score，四捨五入到小數第一位
            //  若TDQCS中的兩單項分數都=NA: TDQCS分數=NA

            if (!score1.HasValue && !score2.HasValue)
                return null;

            // 分母
            decimal totalRatio = ratio1 + ratio2;

            // 分子1
            decimal s1 = ratio1 * score1 ?? 0 ;

            // 分子2 
            decimal s2 = ratio2 * score2 ?? 0 ;

            // 計算結果，並四捨五入
            var result = (s1 + s2) / totalRatio;
            result = Math.Round(result, 1);
            return result;
        }

        #endregion

        #region Private method
        /// <summary> 文字是否在指定的陣列中 </summary>
        /// <param name="compareText"></param>
        /// <param name="targetTexts"></param>
        /// <returns></returns>
        public static bool IsTextInArray(string compareText, params string[] targetTexts)
        {
            foreach (var item in targetTexts)
            {
                if (string.Compare(compareText, item, true) == 0)
                    return true;
            }

            return false;
        }

        /// <summary> 轉換文字格式 </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static string ToNumberText(decimal? val)
        {
            if (!val.HasValue)
                return _fixText_NA;
            else
                return val.Value.ToString();
        }
        #endregion
    }
}
