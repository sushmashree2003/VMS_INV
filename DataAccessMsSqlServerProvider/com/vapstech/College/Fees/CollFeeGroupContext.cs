using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;

namespace DataAccessMsSqlServerProvider.com.vapstech.College.Fee
{
    public class CollFeeGroupContext : DbContext
    {
        public CollFeeGroupContext(DbContextOptions<CollFeeGroupContext> options) :base(options)
        { Database.SetCommandTimeout(30000); }

        public DbSet<Fee_PaymentGateway_DetailsDMO> Fee_PaymentGateway_Details { get; set; }

        public DbSet<PAYUDETAILS> PAYUDETAILS { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }

        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }

        public DbSet<Adm_Course_Branch_MappingDMO> coursebranch { get; set; }

        public DbSet<AdmCourseBranchSemesterMappingDMO> Coursebranchsem { get; set; }

        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }

        public DbSet<MasterAcademic> AcademicYear { get; set; }

        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }

        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }

        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }

        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }

        public DbSet<FeeGroupClgDMO> FeeGroupClgDMO { get; set; }
        public DbSet<FeeYearGroupClgDMO> FeeYearGroupDMO { get; set; }

        public DbSet<Fee_Master_Amount_OthStaffs> CLG_Adm_College_Seat_Distribution { get; set; }

        public DbSet<CLG_Fee_Yearly_Group_Head_Mapping> CLG_Fee_Yearly_Group_Head_Mapping { get; set; }

        // public DbSet<FeeGroupClgDMO> FeeGroupClgDMO { get; set; }
        public DbSet<FeeHeadClgDMO> FeeHeadClgDMO { get; set; }
        public DbSet<Clg_Fee_Installment_DMO> Clg_Fee_Installment_DMO { get; set; }
        public DbSet<Clg_Fee_Installments_Yearly_DMO> Clg_Fee_Installments_Yearly_DMO { get; set; }

        public DbSet<Clg_Fee_AmountEntry_DMO> Clg_Fee_AmountEntry_DMO { get; set; }

        public DbSet<CLG_Fee_College_Master_Amount_Semesterwise> CLG_Fee_College_Master_Amount_Semesterwise { get; set; }

        public DbSet<CLG_Fee_College_T_Due_DateDMO> CLG_Fee_College_T_Due_DateDMO { get; set; }
        public DbSet<CLG_Fee_College_T_Fine_Slabs> CLG_Fee_College_T_Fine_Slabs { get; set; }

        public DbSet<FeeFineSlabDMO> feeFS { get; set; }

        public DbSet<MasterMonthDMO> IVRM_Month { get; set; }

        public DbSet<Fee_College_Student_StatusDMO> Fee_College_Student_StatusDMO { get; set; }

        public DbSet<Fee_Y_Payment_PA_Application> Fee_Y_Payment_PA_Application { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }

        public  DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }

        public DbSet<Fee_College_Master_Student_GroupHeadDMO> Fee_College_Master_Student_GroupHeadDMO { get; set; }

        public DbSet<Fee_C_Master_Student_GroupHead_InstallmentsDMO> Fee_C_Master_Student_GroupHead_InstallmentsDMO { get; set; }

        public  DbSet <Clg_Fee_Installment_Due_Date_DMO> Clg_Fee_Installment_Due_Date_DMO { get; set; }

        public DbSet<FEeGroupLoginPreviledgeDMO> FEeGroupLoginPreviledgeDMO { get; set; }

        public DbSet<MasterCompanyDMO> MasterCompanyDMO { get; set; }
       
        public DbSet<Fee_Groupwise_AutoReceiptDMO> Fee_Groupwise_AutoReceiptDMO { get; set; }
        public DbSet<Fee_Groupwise_AutoReceipt_GroupsDMO> Fee_Groupwise_AutoReceipt_GroupsDMO { get; set; }

        public DbSet<FeeMasterConfigurationDMO> feemastersettings { get; set; }
        public DbSet<Master_Numbering> Master_Numbering { get; set; }
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        //public DbSet<FeePaymentDetailsDMO> FeePaymentDetailsDMO { get; set; }
        public DbSet<FeeTermDMO> feeTr { get; set; }
        public DbSet<FeeMasterTermHeadsDMO> feeMTH { get; set; }
        public DbSet<FeeSpecialFeeGroupDMO> feespecialHead { get; set; }
        public DbSet<FeeSpecialFeeGroupsGroupingDMO> feeSGGG { get; set; }
      
        public DbSet<CollegeConcessionDMO> CollegeConcessionDMO { get; set; }
        public DbSet<CollegeConcessionInstallmentDMO> CollegeConcessionInstallmentDMO { get; set; }
        public DbSet<Fee_Master_ConcessionDMO> Fee_Master_ConcessionDMO { get; set; }
        public DbSet<Fee_Y_PaymentDMO> Fee_Y_PaymentDMO { get; set; }
        public DbSet<Fee_Y_Payment_PaymentModeDMO> Fee_Y_Payment_PaymentModeDMO { get; set; }
        public DbSet<Fee_Y_Payment_College_StudentDMO> Fee_Y_Payment_College_StudentDMO { get; set; }
        public DbSet<Fee_T_College_PaymentDMO> Fee_T_College_PaymentDMO { get; set; }

        public DbSet<FeeGroupGroupingDMO> FeeGroupGroupingDMO { get; set; }
        public DbSet<FeeGroupGroupingDMO> FeeGroupMappingDMO { get; set; }
        public DbSet<v_studentPendingconcessionDMO> v_studentPendingconcessionDMO { get; set; }

        public DbSet<Clg_Fee_Studentwise_DMO> Clg_Fee_Studentwise_DMO { get; set; }

        public DbSet<ApplicationUserDMO> applicationUser { get; set; }
        public DbSet<ApplicationUserRoleDMO> ApplicationUserRole { get; set; }
        public DbSet<IVRM_User_Login_InstitutionwiseDMO> UserRoleWithInstituteDMO { get; set; }

        public DbSet<Fee_College_Master_Opening_BalanceDMO> Fee_College_Master_Opening_BalanceDMO { get; set; }
        public DbSet<Fee_College_RefundDMO> Fee_College_RefundDMO { get; set; }
        public DbSet<Fee_College_Student_AdjustmentDMO> Fee_College_Student_AdjustmentDMO { get; set; }
        public DbSet<Fee_College_Cheque_BounceDMO> Fee_College_Cheque_BounceDMO { get; set; }
        public DbSet<Fee_College_Student_WaivedOffDMO> Fee_College_Student_WaivedOffDMO { get; set; }
        public DbSet<Clg_Adm_College_QuotaDMO> Clg_Adm_College_QuotaDMO { get; set; }
        public DbSet<ClgMasterCategoryDMO> mastercategory { get; set; }
        public DbSet<ClgMasterCourseCategoryMapDMO> ClgMasterCourseCategoryMapDMO { get; set; }

        public DbSet<Fee_M_Online_TransactionDMO> Fee_M_Online_TransactionDMO { get; set; }
        public DbSet<Fee_T_Online_TransactionDMO> Fee_T_Online_TransactionDMO { get; set; }

        public DbSet<Fee_Y_Payment_Preadmission_ApplicationDMO> Fee_Y_Payment_Preadmission_ApplicationDMO { get; set; }

        public DbSet<Fee_OnlinePayment_MappingDMO> Fee_OnlinePayment_Mapping { get; set; }
        public DbSet<Fee_OnlinePayment_MappingDMO> GenConfig { get; set; }
        public DbSet<AdmCollegeSchemeTypeDMO> AdmCollegeSchemeTypeDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;;;;;
            }
        }

    }
}









