using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.ORM
{
    public class TET_SPA_ScoringInfoSheets
    {
        [Key]
        [Column(Order = 0)]
        public Guid ServiceItemID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(16)]
        public string POSource { get; set; }

        public bool IsSheet1Show { get; set; }

        public bool IsSheet1TypeFill { get; set; }

        public bool IsSheet1SupplierFill { get; set; }

        public bool IsSheet1SourceFill { get; set; }

        public bool IsSheet1EmpNameFill { get; set; }

        public bool IsSheet1MajorJobFill { get; set; }

        public bool IsSheet1IsIndependentFill { get; set; }

        public bool IsSheet1SkillLevelFill { get; set; }

        public bool IsSheet1EmpStatusFill { get; set; }

        public bool IsSheet1TELSeniorityYFill { get; set; }

        public bool IsSheet1TELSeniorityMFill { get; set; }

        public bool IsSheet1RemarkFill { get; set; }

        public bool IsSheet2Show { get; set; }

        public bool IsSheet2ServiceForFill { get; set; }

        public bool IsSheet2WorkItemFill { get; set; }

        public bool IsSheet2MachineNameFill { get; set; }

        public bool IsSheet2MachineNoFill { get; set; }

        public bool IsSheet2OnTimeFill { get; set; }

        public bool IsSheet2RemarkFill { get; set; }

        public bool IsSheet3Show { get; set; }

        public bool IsSheet3WorkerCountFill { get; set; }

        public bool IsSheet3DateFill { get; set; }

        public bool IsSheet3LocationFill { get; set; }

        public bool IsSheet3TELLossFill { get; set; }

        public bool IsSheet3CustomerLossFill { get; set; }

        public bool IsSheet3AccidentFill { get; set; }

        public bool IsSheet3DescriptionFill { get; set; }

        public bool IsSheet4Show { get; set; }

        public bool IsSheet4CorrectnessFill { get; set; }

        public bool IsSheet4ContributionFill { get; set; }

        public bool IsSheet5Show { get; set; }

        public bool IsSheet5SelfTrainingFill { get; set; }

        public bool IsSheet5SelfTrainingRemarkFill { get; set; }

        public bool IsSheet6Show { get; set; }

        public bool IsSheet6CooperationFill { get; set; }

        public bool IsSheet6DateFill { get; set; }

        public bool IsSheet6LocationFill { get; set; }

        public bool IsSheet6IsDamageFill { get; set; }

        public bool IsSheet6DescriptionFill { get; set; }

        public bool IsSheet7Show { get; set; }

        [Required]
        [StringLength(64)]
        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(64)]
        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
