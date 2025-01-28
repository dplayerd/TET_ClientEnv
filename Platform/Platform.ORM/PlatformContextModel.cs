using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Platform.ORM
{
    public partial class PlatformContextModel : DbContext
    {
        public PlatformContextModel()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<AdminLog> AdminLogs { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<MediaFileRole> MediaFileRoles { get; set; }
        public virtual DbSet<MailPool> MailPools { get; set; }
        public virtual DbSet<MailPoolWithCC> MailPoolWithCCs { get; set; }
        public virtual DbSet<MediaFile> MediaFiles { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<PageFunctionRole> PageFunctionRoles { get; set; }
        public virtual DbSet<PageRole> PageRoles { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<SystemUser> SystemUsers { get; set; }
        public virtual DbSet<TET_Parameters> TET_Parameters { get; set; }
        public virtual DbSet<TET_PaymentSupplier> TET_PaymentSupplier { get; set; }
        public virtual DbSet<TET_PaymentSupplierApproval> TET_PaymentSupplierApproval { get; set; }
        public virtual DbSet<TET_PaymentSupplierAttachments> TET_PaymentSupplierAttachments { get; set; }
        public virtual DbSet<TET_PaymentSupplierContact> TET_PaymentSupplierContact { get; set; }
        public virtual DbSet<TET_SPA_ApproverSetup> TET_SPA_ApproverSetup { get; set; }
        public virtual DbSet<TET_SPA_CostService> TET_SPA_CostService { get; set; }
        public virtual DbSet<TET_SPA_CostServiceApproval> TET_SPA_CostServiceApproval { get; set; }
        public virtual DbSet<TET_SPA_CostServiceAttachments> TET_SPA_CostServiceAttachments { get; set; }
        public virtual DbSet<TET_SPA_CostServiceDetail> TET_SPA_CostServiceDetail { get; set; }
        public virtual DbSet<TET_SPA_Evaluation> TET_SPA_Evaluation { get; set; }
        public virtual DbSet<TET_SPA_EvaluationReport> TET_SPA_EvaluationReport { get; set; }
        public virtual DbSet<TET_SPA_EvaluationReportAttachments> TET_SPA_EvaluationReportAttachments { get; set; }
        public virtual DbSet<TET_SPA_Period> TET_SPA_Period { get; set; }
        public virtual DbSet<TET_SPA_ScoringInfo> TET_SPA_ScoringInfo { get; set; }
        public virtual DbSet<TET_SPA_ScoringInfoAttachments> TET_SPA_ScoringInfoAttachments { get; set; }
        public virtual DbSet<TET_SPA_ScoringInfoModule1> TET_SPA_ScoringInfoModule1 { get; set; }
        public virtual DbSet<TET_SPA_ScoringInfoModule2> TET_SPA_ScoringInfoModule2 { get; set; }
        public virtual DbSet<TET_SPA_ScoringInfoModule3> TET_SPA_ScoringInfoModule3 { get; set; }
        public virtual DbSet<TET_SPA_ScoringInfoModule4> TET_SPA_ScoringInfoModule4 { get; set; }
        public virtual DbSet<TET_SPA_ScoringRatio> TET_SPA_ScoringRatio { get; set; }
        public virtual DbSet<TET_SPA_ViolationApproval> TET_SPA_ViolationApproval { get; set; }
        public virtual DbSet<TET_SPA_ViolationAttachments> TET_SPA_ViolationAttachments { get; set; }
        public virtual DbSet<TET_SPA_ViolationDetail> TET_SPA_ViolationDetail { get; set; }
        public virtual DbSet<TET_SPA_ScoringInfoApproval> TET_SPA_ScoringInfoApproval { get; set; }
        public virtual DbSet<TET_SPA_Tooltips> TET_SPA_Tooltips { get; set; }
        public virtual DbSet<TET_SPA_Violation> TET_SPA_Violation { get; set; }
        public virtual DbSet<TET_Supplier> TET_Supplier { get; set; }
        public virtual DbSet<TET_SupplierApproval> TET_SupplierApproval { get; set; }
        public virtual DbSet<TET_SupplierAttachments> TET_SupplierAttachments { get; set; }
        public virtual DbSet<TET_SupplierContact> TET_SupplierContact { get; set; }
        public virtual DbSet<TET_SupplierSPA> TET_SupplierSPA { get; set; }
        public virtual DbSet<TET_SupplierSPAApproval> TET_SupplierSPAApproval { get; set; }
        public virtual DbSet<TET_SupplierSTQA> TET_SupplierSTQA { get; set; }
        public virtual DbSet<TET_SupplierSTQAApproval> TET_SupplierSTQAApproval { get; set; }
        public virtual DbSet<TET_SupplierTrade> TET_SupplierTrade { get; set; }
        public virtual DbSet<UserLoginRecord> UserLoginRecords { get; set; }
        public virtual DbSet<UserPasswordRecord> UserPasswordRecords { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<vwTET_Supplier_LastVersion> vwTET_Supplier_LastVersion { get; set; }
        public virtual DbSet<vwTET_PaymentSupplier_LastVersion> vwTET_PaymentSupplier_LastVersion { get; set; }
        public virtual DbSet<vwApprovalList> vwApprovalList { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminLog>()
                .Property(e => e.ReferenceID)
                .IsUnicode(false);

            modelBuilder.Entity<AdminLog>()
                .Property(e => e.AccessName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.ReferenceID)
                .IsUnicode(false);

            modelBuilder.Entity<MediaFile>()
                .Property(e => e.ModuleName)
                .IsUnicode(false);

            modelBuilder.Entity<MediaFile>()
                .Property(e => e.ModuleID)
                .IsUnicode(false);

            modelBuilder.Entity<MediaFile>()
                .Property(e => e.Purpose)
                .IsUnicode(false);

            modelBuilder.Entity<MediaFile>()
                .Property(e => e.FilePath)
                .IsUnicode(false);

            modelBuilder.Entity<MediaFile>()
                .Property(e => e.MimeType)
                .IsUnicode(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.Controller)
                .IsUnicode(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.Action)
                .IsUnicode(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.AdminController)
                .IsUnicode(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.AdminAction)
                .IsUnicode(false);

            modelBuilder.Entity<PageFunctionRole>()
                .Property(e => e.FunctionCode)
                .IsUnicode(false);

            modelBuilder.Entity<Page>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Page>()
                .Property(e => e.PageIcon)
                .IsUnicode(false);

            modelBuilder.Entity<Site>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Account)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.HashKey)
                .IsUnicode(false);

            modelBuilder.Entity<TET_SPA_ScoringRatio>()
                .Property(e => e.TRatio1)
                .HasPrecision(7, 4);

            modelBuilder.Entity<TET_SPA_ScoringRatio>()
                .Property(e => e.TRatio2)
                .HasPrecision(7, 4);

            modelBuilder.Entity<TET_SPA_ScoringRatio>()
                .Property(e => e.DRatio1)
                .HasPrecision(7, 4);

            modelBuilder.Entity<TET_SPA_ScoringRatio>()
                .Property(e => e.DRatio2)
                .HasPrecision(7, 4);

            modelBuilder.Entity<TET_SPA_ScoringRatio>()
                .Property(e => e.QRatio1)
                .HasPrecision(7, 4);

            modelBuilder.Entity<TET_SPA_ScoringRatio>()
                .Property(e => e.QRatio2)
                .HasPrecision(7, 4);

            modelBuilder.Entity<TET_SPA_ScoringRatio>()
                .Property(e => e.CRatio1)
                .HasPrecision(7, 4);

            modelBuilder.Entity<TET_SPA_ScoringRatio>()
                .Property(e => e.CRatio2)
                .HasPrecision(7, 4);

            modelBuilder.Entity<TET_SPA_ScoringRatio>()
                .Property(e => e.SRatio1)
                .HasPrecision(7, 4);

            modelBuilder.Entity<TET_SPA_ScoringRatio>()
                .Property(e => e.SRatio2)
                .HasPrecision(7, 4);

            modelBuilder.Entity<TET_SupplierTrade>()
                .Property(e => e.Amount)
                .HasPrecision(12, 2);

            modelBuilder.Entity<UserLoginRecord>()
                .Property(e => e.Account)
                .IsUnicode(false);

            modelBuilder.Entity<UserLoginRecord>()
                .Property(e => e.UserIP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UserPasswordRecord>()
                .Property(e => e.Password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UserPasswordRecord>()
                .Property(e => e.Salt)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
