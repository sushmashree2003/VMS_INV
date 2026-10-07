using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Portals.Chairman;

using DomainModel.Model.com.vapstech.LeaveManagement;

using DomainModel.Model.com.vapstech.Transport;

using DomainModel.Model.com.vapstech.Portals.HOD;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.Portals.Employee;
using DomainModel.Model.com.vapstech.Portals.Student;
using DomainModel.Model.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.MobileApp;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel;
using DomainModel.Model.com.vapstech.College.Preadmission;

using DomainModel.Model.com.vapstech.College.Fees;

using DomainModel.Model.com.vapstech.OnlineExam;
using DomainModel.Model.com.vapstech.Portals.IVRM;
using DomainModel.Model.com.vapstech.VisitorsManagement;

using DomainModel.Model.com.vapstech.Library;


using DomainModel.Model.com.vapstech.Purchase.Inventory;
using DomainModel.Model.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.SpaceBooking;
using DomainModel.Model.com.vapstech.IssueManager.PettyCash;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using DomainModel.Model.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking.AssetTag;
using DomainModel.Model.com.vapstech.VMS.Training;
using DomainModel.Model.com.vapstech.Portals.Tenants;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using DomainModel.Model.com.vapstech.IssueManager.TADA;

namespace DataAccessMsSqlServerProvider
{
    public class DomainModelMsSqlServerContext : DbContext
    {
        public DomainModelMsSqlServerContext(DbContextOptions<DomainModelMsSqlServerContext> options) : base(options) { Database.SetCommandTimeout(300000000); }
        public DbSet<DataEventRecord> DataEventRecords { get; set; }
        // public DbSet<logindata> logindata { get; set; }
        // public DbSet<IVRMMasterConfigurationDMO> IVRMMasterConfigurationDMO { get; set; }


        public DbSet<SMSEmailSetting> SMSEmailSetting { get; set; }
        public DbSet<Staff_User_Login> IVRM_Staff_User_Login { get; set; }
        public DbSet<INV_Asset_CheckOutDMO> INV_Asset_CheckOutDMO { get; set; }
        public DbSet<INV_Asset_CheckInDMO> INV_Asset_CheckInDMO { get; set; }
        public DbSet<INV_M_GRN_Return_DMO> INV_M_GRN_Return_DMO_con { get; set; }
        public DbSet<INV_M_Sales_Return_DMO> INV_M_Sales_Return_DMO_con { get; set; }
        public DbSet<HR_CandidateInterviewScheduleDMO> HR_CandidateInterviewScheduleDMO { get; set; }
        public DbSet<ivrm_email_sentbox> ivrm_email_sentbox { get; set; }
        public DbSet<HR_Candidate_DetailsDMO> HR_Candidate_DetailsDMO { get; set; }
        public DbSet<SMS_Email_Setting_RoleTypeDMO> SMS_Email_Setting_RoleTypeDMO { get; set; }
        public DbSet<IVRM_Master_HTMLTemplatesDMO> IVRM_Master_HTMLTemplatesDMO { get; set; }
        //chair man
        //  public DbSet<IVRM_Email_sentBoxDMO> IVRM_Email_sentBoxDMO { get; set; }
        //chair man
        public DbSet<ISM_DailyReport_FeedBack_Transaction_DMO> ISM_DailyReport_FeedBack_Transaction_DMO_con { get; set; }
        public DbSet<ISM_DailyReport_FeedBack_DMO> ISM_DailyReport_FeedBack_DMO_con { get; set; }
        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
        public DbSet<ISM_ACKN_Status_DMO> ISM_ACKN_Status_DMO_con { get; set; }
        public DbSet<StudentAppUserLoginDMO> StudentAppUserLoginDMO { get; set; }
        public DbSet<SourceInfo> SourceInfos { get; set; }
        public DbSet<Registration> Registration { get; set; }
        public DbSet<userDetails> userDetails { get; set; }
        public DbSet<Enquiry> Enquiry { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<HR_Master_DepartmentCode_HeadDMO> HR_Master_DepartmentCode_HeadDMO { get; set; }
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<School_M_Class> admissioncls { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<Institution> Institute { get; set; }

        public DbSet<ISM_User_Employees_MappingDMO> ISM_User_Employees_MappingDMO { get; set; }
        public DbSet<ISM_Email_Department_MappingDMO> ISM_Email_Department_MappingDMO { get; set; }

        public DbSet<Alumni_M_StudentDMO> Alumni_M_StudentDMO { get; set; }
        public DbSet<CLGAlumniUserRegistrationDMO> CLGAlumniUserRegistrationDMO { get; set; }

        public DbSet<ISM_DailyReportExtensionDMO> ISM_DailyReportExtensionDMO  { get; set; }
        public DbSet<ISM_DailyReportGenerationDMO> ISM_DailyReportGenerationDMO { get; set; }

        public DbSet<CLGAlumni_User_LoginDMO> CLGAlumni_User_LoginDMO { get; set; }
        public DbSet<StudentSibling> StudentSibling { get; set; }
        public DbSet<AdmissionStatus> AdmissionStatus { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<MasterConfiguration> mstConfig { get; set; }
        public DbSet<MasterTemplate> mstTemplate { get; set; }
        public DbSet<InstituteTemplate> InstitutionTemplate { get; set; }
         
        public DbSet<MobileApplAuthenticationDMO> MobileApplAuthenticationDMO { get; set; }
        public DbSet<AdmissionStatus> status { get; set; }
        public DbSet<PA_Student_Sibblings_Details> PA_Student_Sibblings_Details { get; set; }
        public DbSet<PAStudentEmployee> PAStudentEmployee { get; set; }
        public DbSet<HR_Master_DepartmentCodeDMO> HR_Master_DepartmentCodeDMO { get; set; }

        //public DbSet<HR_Master_DepartmentCode_HeadDMO> HR_Master_DepartmentCode_HeadDMO { get; set; }

        public DbSet<TR_student_LocMappingDMO> TR_student_LocMappingDMO { get; set; }

        public DbSet<PA_Student_Transport_ApplicationDMO> PA_Student_Transport_ApplicationDMO { get; set; }
        public DbSet<TR_Student_RouteDMO> TR_Student_RouteDMO { get; set; }

        public DbSet<TR_Student_Route_FeeGroupDMO> TR_Student_Route_FeeGroupDMO { get; set; }
        public DbSet<StudentSiblingDMO> StudentSiblingDMO { get; set; }
        public DbSet<PA_Student_Sibblings> PA_Student_Sibblings  { get; set; }
        public DbSet<Adm_M_Employee_StudentDMO> Adm_M_Employee_StudentDMO { get; set; }
        public DbSet<IVRM_Master_FinancialYear> IVRM_Master_FinancialYear { get; set; }
        public DbSet<PA_College_Application> PA_College_Application { get; set; }

        public DbSet<TR_Location_FeeGroup_MappingDMO> TR_Location_FeeGroup_MappingDMO { get; set; }
        public DbSet<PA_Master_Vaccines> PA_Master_Vaccines { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }

        public DbSet<CLGAlumni_M_StudentDMO> CLGAlumni_M_StudentDMO { get; set; }
        public DbSet<CollegeStudentlogin> CollegeStudentlogin { get; set; }

        public DbSet<MOBILE_INSTITUTION> MOBILE_INSTITUTION { get; set; }

        public DbSet<API_MOBILE> API_MOBILE { get; set; }
        public DbSet<ApplUser> ApplicationUser { get; set; }

        public DbSet<UserLoginPrivileges> UserLoginPrivileges { get; set; }

        public DbSet<IVRM_User_MobileApp_Login_Privileges> IVRM_User_MobileApp_Login_Privileges { get; set; }
        public DbSet<IVRM_MobileApp_Page> IVRM_MobileApp_Page { get; set; }
        public DbSet<IVRM_Role_MobileApp_Privileges> IVRM_Role_MobileApp_Privileges { get; set; }
        public DbSet<IVRM_Mandatory_Setting> IVRM_Mandatory_Setting { get; set; }

        public DbSet<IVRM_Mandatory_Setting_IW> IVRM_Mandatory_Setting_IW { get; set; }
        public DbSet<StudentStatusHistory> studentstatushistory { get; set; }
        public DbSet<TransactionNumbering> TransactionNumbering { get; set; }
        public DbSet<Transaction_Numbering_Type> Transaction_Numbering_Type { get; set; }

        public DbSet<AlumniUserRegistrationDMO> AlumniUserRegistrationDMO { get; set; }

        public DbSet<Alumni_User_LoginDMO> Alumni_User_LoginDMO { get; set; }

        public DbSet<MasterRolePreviledgeDMO> Role_Privileges { get; set; }
        public DbSet<HR_Master_LeaveYearDMO> HR_Master_LeaveYearDMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<Office_Space_User_Registration> Office_Space_User_Registration { get; set; }
        public DbSet<Institution_Module_Page> Institution_Module_Page { get; set; }

        public DbSet<MasterModule> masterModule { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }

        public DbSet<Institution_Module> Institution_Module { get; set; }

        public DbSet<MasterPageModuleMapping> masterPageModuleMapping { get; set; }
        public DbSet<MasterPage> masterPage { get; set; }
        public DbSet<MasterTemplate> MasterTemplate { get; set; }
        public DbSet<MasterCategory> mastercategory { get; set; }
        public DbSet<MasterSchoolType> masterSchoolType { get; set; }
        public DbSet<MasterBorad> masterBorad { get; set; }
        public DbSet<ApplicationUserRole> appUserRole { get; set; }
        public DbSet<ApplRole> applicationRole { get; set; }

        public DbSet<OrganisationEmail> OrganisationEmail { get; set; }

        public DbSet<OrganisationPhone> OrganisationPhone { get; set; }

        public DbSet<Institution_Phone_No> Institution_Phone_No { get; set; }
        public DbSet<Institution_MobileNo> Institution_MobileNo { get; set; }
        public DbSet<Institution_EmailId> Institution_EmailId { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<City> city { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Preadmission_SeatBlocked_Student> Preadmission_SeatBlocked_Student { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<StudentApplication> StudentApplication { get; set; }
        public DbSet<IVRM_Email_sentBoxDMO> IVRM_Email_sentBoxDMO { get; set; }

        public DbSet<Adm_Student_Transport_ApplicationDMO> Adm_Student_Transport_ApplicationDMO { get; set; }
        public DbSet<MasterPage> masterpage { get; set; }

        // Added On 14/10/2016
        public DbSet<VirtaulSchool> VirtualSchool { get; set; }
        // Added On 14/10/2016 end

        public DbSet<MandatoryFields> mandatory { get; set; }

        public DbSet<ModulePage_Category> modulepagecoategory { get; set; }

        public DbSet<Master_Numbering> Master_Numbering { get; set; }

        public DbSet<HR_Master_Building> HR_Master_Building { get; set; }
        public DbSet<HR_Master_Floor> HR_Master_Floor { get; set; }
        public DbSet<HR_Master_Room> HR_Master_Room { get; set; }



        //11/11/2016

        // Added on 11-11-2016 for menu change
        public DbSet<MasterMenu> Mastermenu { get; set; }
        //public DbSet<MasterMenuPageMapping> Mastermenupagemapping { get; set; }
        public DbSet<MasterMenuInstitutionWise> Mastermenuinstitutewise { get; set; }
        public DbSet<MasterMenuPageMappingInstituteWise> Mastermenupagemappinginstitutewise { get; set; }
        public DbSet<InstitutionRolePrivileges> InstitutionRolePrivileges { get; set; }
        // Added on 11-11-2016 for menu change

        // Added on 19-11-2016
        // public DbSet<IVRM_Master_SubjectsDMO> Subject { get; set; }
        public DbSet<School_M_Section> Section { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUser> Adm_SchAttLoginUser { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUserClass> Adm_SchAttLoginUserClass { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUserClassSubject> Adm_schAttLoginUserClassSubjects { get; set; }
        //Added By Sripad Joshi
        public DbSet<SMSEmailSetting> smsEmailSetting { get; set; }
        public DbSet<SmsEmailHeader> smsEmailHeader { get; set; }
        public DbSet<Month> month { get; set; }
        public DbSet<MasterClassHeld> masterclassHeld { get; set; }
        //
        public DbSet<School_M_Section> School_M_Section { get; set; }

        public DbSet<School_Adm_Y_StudentDMO> SchoolYearWiseStudent { get; set; }
        public DbSet<AdmSchoolAttendanceSubjectBatch> AdmSchoolAttendanceSubjectBatch { get; set; }
        public DbSet<AdmSchoolAttendanceSubjectBatchStudents> AdmSchoolAttendanceSubjectBatchStudents { get; set; }
        public DbSet<MasterSubjectDMO> MasterSubject { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        // Added on 19-11-2016

        // Added on 26-11-2016
        public DbSet<Adm_SchoolAttendanceLoginUser> attloginuser { get; set; }
        public DbSet<Adm_studentAttendance> Adm_studentAttendance { get; set; }
        public DbSet<Adm_studentAttendanceStudents> Adm_studentAttendanceStudents { get; set; }
        public DbSet<Adm_studentAttendanceSubjects> Adm_studentAttendanceSubjects { get; set; }
        public DbSet<Adm_StudentAttendancePeriodwiseDMO> Adm_StudentAttendancePeriodwiseDMO { get; set; }
        public DbSet<AdditionalField> additional_field_context { get; set; }

        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<SMS_DETAILS_DMO> SMS_DETAILS_DMO { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }

        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }


        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }

        public DbSet<Preadmission_Special_Registration> Preadmission_Special_Registration { get; set; }
        public DbSet<Master_Institution_SubscriptionValidity> Master_Institution_SubscriptionValidity { get; set; }
        // Added on 26-11-2016

        // added on 12 jan 2017

        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }

        public DbSet<MasterReligionDMO> masterReligion { get; set; }

        public DbSet<IVRM_Master_Menu_Page_MappingDMO> IVRM_Master_Menu_Page_MappingDMO { get; set; }

        public DbSet<MasterMenuPageMappingInstituteWise> IVRM_Master_Menu_Page_Mapping_InstitutionwiseDMO { get; set; }

        public DbSet<MasterMenuInstitutionWise> MasterMenuInstitutionWise { get; set; }

        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<StudentAchivementDMO> Achivement { get; set; }

        public DbSet<StudentTC> Student_TC { get; set; }

        public DbSet<AttendanceEntryTypeDMO> AttendanceEntryTypeDMO { get; set; }

        public DbSet<ProspectusFilePath> ProspectusFilePath { get; set; }
        public DbSet<Prospectus> prospectus { get; set; }

        public DbSet<FeeMasterConfigurationDMO> FeeMasterConfigurationDMO { get; set; }

        public DbSet<readmitstudentDMO> readmitstudentDMO { get; set; }
        public DbSet<Adm_Master_Batch> Adm_Master_Batch { get; set; }

        //public DbSet<MasterEmployee> masterStaff { get; set; }

        public DbSet<Receipt_Numbering> Receipt_Numbering { get; set; }
        public DbSet<Voucher_Numbering> Voucher_Numbering { get; set; }
        public DbSet<SmsEmailParameterMappingDMO> SmsEmailParameterMappingDMO { get; set; }
        public DbSet<IVRM_School_Master_SubjectsDMO> MasterSubjectList { get; set; }
        public DbSet<AdmissionStandardDMO> AdmissionStandardDMO { get; set; }
        public DbSet<FeeStudentTransactionDMO> feestudentstatus { get; set; }

        public DbSet<StaffLoginDMO> StaffLoginDMO { get; set; }

        public DbSet<MasterEmployee> HR_Master_Employee_DMO { get; set; }

        public DbSet<FeeStudentGroupMappingDMO> studentgroupmapping { get; set; }
        public DbSet<FeeStudentGroupInstallmentMappingDMO> studentgroupinstallmapping { get; set; }

        public DbSet<FeePaymentDetailsDMO> FeePaymentDetails { get; set; }

        public DbSet<Fee_Y_PaymentDMO> FeePaymentDetailsCollege { get; set; }

        public DbSet<IVRM_sms_sentBoxDMO> IVRM_sms_sentBoxDMO { get; set; }

        //added by vishnu
        public DbSet<Adm_M_Student_FatherMobileNo> Adm_M_Student_FatherMobileNo { get; set; }
        public DbSet<Adm_Master_Father_Email> Adm_Master_Father_Email { get; set; }
        public DbSet<Adm_M_Mother_MobileNo> Adm_M_Mother_MobileNo { get; set; }
        public DbSet<Adm_M_Mother_Emailid> Adm_M_Mother_Emailid { get; set; }
        public DbSet<Adm_M_Student_MobileNo> Adm_M_Student_MobileNo { get; set; }
        public DbSet<Adm_M_Student_Email_Id> Adm_M_Student_Email_Id { get; set; }
        public DbSet<Adm_Student_EcsDetailsDMO> Adm_Student_EcsDetailsDMO { get; set; }
        public DbSet<StudentUserLoginDMO> StudentUserLoginDMO { get; set; }
        public DbSet<StudentUserLogin_Institutionwise> StudentUserLogin_Institutionwise { get; set; }

        //Class Section Mapping On 16-08-2017.
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }

        public DbSet<FeeMasterRefundDMO> FeeMasterRefundDMO { get; set; }

        public DbSet<PreadmisionLoginHistory> PreadmisionLoginHistory { get; set; }

        public DbSet<IVRM_Storage_path_Details> IVRM_Storage_path_Details { get; set; }
        public DbSet<TT_Master_PeriodDMO> TT_Master_PeriodDMO { get; set; }
        public DbSet<StudentMappingDMO> StudentMappingDMO { get; set; }

        public DbSet<Attendance_Students_SmartCard> Attendance_Students_SmartCard { get; set; }
        public DbSet<Attendance_Lunch_Students_SmartCardDMO> Attendance_Lunch_Students_SmartCardDMO { get; set; }
        public DbSet<ClassTeacherMappingDMO> ClassTeacherMappingDMO { get; set; }
        public DbSet<Fee_M_Online_TransactionDMO> Fee_M_Online_TransactionDMO { get; set; }

        public DbSet<Dashboard_page_mapping> Dashboard_page_mapping { get; set; }

        public DbSet<StudycertificateReportDMO> StudycertificateReportDMO { get; set; }

        public DbSet<ivrm_email_sentbox> IVRM_Email_sentBox { get; set; }
        public DbSet<IVRM_Master_Gender> IVRM_Master_Gender { get; set; }

        // public DbSet<IVRM_Master_SubjectsDMO> IVRM_Master_Subjects { get; set; }

        public DbSet<Payment_PA_Prospectus> Payment_PA_ProspectusDMO { get; set; }
        public DbSet<HR_Emp_Leave_ApplicationDMO> HR_Emp_Leave_ApplicationDMO { get; set; }


        public DbSet<HR_Emp_OB_Leave_DMO> HR_Emp_OB_Leave_DMO { get; set; }


        public DbSet<PA_School_Application_ProspectusDMO> PA_School_Application_ProspectusDMO { get; set; }
        //Bus Hire related DMO's.
        public DbSet<TripOnlineBookingDMO> TripOnlineBookingDMO { get; set; }
        public DbSet<MasterHirerGroupDMO> MasterHirerGroupDMO { get; set; }
        public DbSet<TripDMO> TripDMO { get; set; }
        public DbSet<IVRM_ModeOfPaymentDMO> IVRM_ModeOfPaymentDMO { get; set; }
        public DbSet<TR_ServiceDMO> TR_ServiceDMO { get; set; }

        //Trip Payment.
        public DbSet<TR_Trip_PaymentDMO> TR_Trip_PaymentDMO { get; set; }
        public DbSet<TR_Trip_Payment_TripsDMO> TR_Trip_Payment_TripsDMO { get; set; }
        public DbSet<TR_Hirer_OB_DMO> TR_Hirer_OB_DMO { get; set; }
        public DbSet<MasterHirerDMO> MasterHirerDMO { get; set; }
        public DbSet<Adm_Master_Student_PA> Adm_Master_Student_PA { get; set; }
        public DbSet<MasterAreaDMO> MasterAreaDMO { get; set; }
        public DbSet<SMS_Sent_Details> SMS_Sent_Details { get; set; }
        public DbSet<IVRM_EMAIL_ATT_DMO> IVRM_EMAIL_ATT_DMO { get; set; }
        public DbSet<IVRM_HOD_Class_DMO> IVRM_HOD_Class_DMO { get; set; }
        public DbSet<HOD_DMO> HOD_DMO { get; set; }
        public DbSet<SMS_Sent_Details_LoginUserDMO> SMS_Sent_Details_LoginUserDMO { get; set; }
        public DbSet<SMS_Sent_Details_NowiseDMO> SMS_Sent_Details_NowiseDMO { get; set; }

        //========Message Interaction 
        public DbSet<IVRM_Interactions_StudentDMO> IVRM_Interactions_StudentDMO { get; set; }
        public DbSet<IVRM_Interactions_Student_StaffDMO> IVRM_Interactions_Student_StaffDMO { get; set; }
        public DbSet<IVRM_School_Master_InteractionsDMO> IVRM_School_Master_InteractionsDMO { get; set; }
        public DbSet<IVRM_School_Transaction_InteractionsDMO> IVRM_School_Transaction_InteractionsDMO { get; set; }


        //===============Push IVRM_PushNotification
        public DbSet<IVRM_PushNotificationDMO> IVRM_PushNotificationDMO { get; set; }


        //===============INVENTORY       
        public DbSet<INV_M_PurchaseRequisitionDMO> INV_M_PurchaseRequisitionDMO { get; set; }
        public DbSet<INV_M_PurchaseIndentDMO> INV_M_PurchaseIndentDMO { get; set; }
        public DbSet<INV_M_SupplierQuotationDMO> INV_M_SupplierQuotationDMO { get; set; }
        public DbSet<INV_M_PurchaseOrderDMO> INV_M_PurchaseOrderDMO { get; set; }
        public DbSet<INV_M_GRNDMO> INV_M_GRNDMO { get; set; }
        public DbSet<INV_M_SalesDMO> INV_M_SalesDMO { get; set; }
        public DbSet<Attendance_Students_SmartCard_Timings> Attendance_Students_SmartCard_Timings { get; set; }

        //Visitor Management
        public DbSet<FO_Inward_DMO> FO_Inward_DMO { get; set; }
        public DbSet<FO_Outward_DMO> FO_Outward_DMO { get; set; }
        public DbSet<Gate_Pass_Student_DMO> Gate_Pass_Student_DMO { get; set; }
        public DbSet<Gate_Pass_Staff_DMO> Gate_Pass_Staff_DMO { get; set; }
        public DbSet<LIB_Master_Subscription_DMO> LIB_Master_Subscription_DMO { get; set; }

        //adde praveen
        public DbSet<SMSMasterApprovalDMO> SMSMasterApprovalDMO { get; set; }
        public DbSet<SMSApprovalStatusDMO> SMSApprovalStatusDMO { get; set; }


        //online exam
        public DbSet<PA_Master_OE_QNS_OptionsDMO> PA_Master_OE_QNS_OptionsDMO { get; set; }
        public DbSet<PA_Master_OE_Questions_ClassDMO> PA_Master_OE_Questions_ClassDMO { get; set; }
        public DbSet<PA_Master_OE_QuestionsDMO> PA_Master_OE_QuestionsDMO { get; set; }
        public DbSet<PA_Master_OE_ANS_OptionsDMO> PA_Master_OE_ANS_OptionsDMO { get; set; }
        public DbSet<PA_Master_OE_SettingDMO> PA_Master_OE_SettingDMO { get; set; }
        public DbSet<PA_Students_Exam_AnswerDMO> PA_Students_Exam_AnswerDMO { get; set; }
        public DbSet<PA_Students_ExamDMO> PA_Students_ExamDMO { get; set; }

        //Petty Cash
        public DbSet<PC_RequisitionDMO> PC_RequisitionDMO { get; set; }
        public DbSet<PC_IndentDMO> PC_IndentDMO { get; set; }
        public DbSet<PC_ExpenditureDMO> PC_ExpenditureDMO { get; set; }
        public DbSet<ISM_Proforma_InvoiceDMO> ISM_Proforma_InvoiceDMO { get; set; }
        public DbSet<HR_Master_BankDeatils> HR_Master_BankDeatils { get; set; }
        public DbSet<INV_AssetTag_CheckOutDMO> INV_AssetTag_CheckOutDMO { get; set; }
        public DbSet<INV_AssetTag_CheckInDMO> INV_AssetTag_CheckInDMO { get; set; }
        //added by roopa
        public DbSet<TenantPaymentDetailsDMO> TenantPaymentDetailsDMO { get; set; }
        //

        //online program

        //public DbSet<ProgramsYearlyDMO> ProgramsYearlyDMO { get; set; }
        //public DbSet<ProgramsYearlyActivitiesDMO> ProgramsYearlyActivitiesDMO { get; set; }
        //public DbSet<ProgramsYearlyGuestDMO> ProgramsYearlyGuestDMO { get; set; }
        //public DbSet<ProgramsYearlyPhotosDMO> ProgramsYearlyPhotosDMO { get; set; }
        //public DbSet<ProgramsYearlyVideosDMO> ProgramsYearlyVideosDMO { get; set; }

        public DbSet<Fee_Master_ConcessionDMO> Fee_Master_ConcessionDMO { get; set; }

        public DbSet<PAYUDETAILS> PAYUDETAILS { get; set; }

        public DbSet<Fee_PaymentGateway_DetailsDMO> Fee_PaymentGateway_Details { get; set; }

        //=============== VMS ISM
        public DbSet<ISM_TaskCreationDMO> ISM_TaskCreationDMO { get; set; }
        public DbSet<ISM_InteractionsDMO> ISM_InteractionsDMO { get; set; }
        public DbSet<ISM_Task_PlannerDMO> ISM_Task_PlannerDMO_con { get; set; }
        public DbSet<ISM_Interactions_DetailsDMO> ISM_Interactions_DetailsDMO { get; set; }
        public DbSet<ISM_InvoiceDMO> ISM_InvoiceDMO { get; set; }
        public DbSet<HR_PROCESSDMO> HR_PROCESSDMO { get; set; }
        public DbSet<HR_Process_Auth_OrderNoDMO> HR_Process_Auth_OrderNoDMO { get; set; }
        public DbSet<VMS_Payment_VoucherDMO> VMS_Payment_VoucherDMO { get; set; }
        public DbSet<ISM_NotificationsDMO> ISM_NotificationsDMO { get; set; }
        public DbSet<TADA_PlannerExtensionDMO> TADA_PlannerExtensionDMO { get; set; }
        public DbSet<ISM_Task_Advance_PlannerDMO> ISM_Task_Advance_PlannerDMO { get; set; }
        public DbSet<ISM_Task_Advance_Planner_DatesDMO> ISM_Task_Advance_Planner_DatesDMO { get; set; }
        public DbSet<ISM_TaskCreation_AssignedToDMO> ISM_TaskCreation_AssignedToDMO { get; set; }
        //Tenent DMO
        public DbSet<TenantAgreement> TenantAgreement { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProspectusFilePath>().HasKey(m => m.IPPC_Id);
            builder.Entity<DataEventRecord>().HasKey(m => m.DataEventRecordId);
            builder.Entity<SourceInfo>().HasKey(m => m.SourceInfoId);
            //builder.Entity<logindata>().HasKey(m => m.userid);
            builder.Entity<Registration>().HasKey(m => m.AOAD_ID);
            builder.Entity<userDetails>().HasKey(m => m.AMPOT_Id);
            builder.Entity<Enquiry>().HasKey(m => m.PASE_Id);
            builder.Entity<Organisation>().HasKey(m => m.MO_Id);
            builder.Entity<MasterTemplate>().HasKey(m => m.IVRMT_Id);
            builder.Entity<MasterCategory>().HasKey(m => m.AMC_Id);
            builder.Entity<MasterBorad>().HasKey(m => m.IVRMMB_Id);
            builder.Entity<MasterSchoolType>().HasKey(m => m.IVRMMTYP_Id);
            builder.Entity<SMSEmailSetting>().HasKey(m => m.ISES_Id);
            builder.Entity<MasterClassHeld>().HasKey(m => m.ASCH_Id);
            builder.Entity<AdditionalField>().HasKey(m => m.IPAF_Id);
            builder.Entity<Masterclasscategory>().HasKey(m => m.ASMCC_Id);

            builder.Entity<MasterReligionDMO>().HasKey(m => m.IVRMMR_Id);
            builder.Entity<StudentTC>().HasKey(m => m.ASTC_Id);

            builder.Entity<IVRM_sms_sentBoxDMO>().HasKey(m => m.IVRM_SSB_ID);
            builder.Entity<AdmSchoolMasterClassCatSec>().HasKey(m => m.ASMCCS_Id);
            builder.Entity<StudentUserLoginDMO>().HasKey(m => m.IVRMSTUUL_Id);
            builder.Entity<StudentUserLogin_Institutionwise>().HasKey(m => m.IVRMSTUULI_Id);
            builder.Entity<StudycertificateReportDMO>().HasKey(m => m.ASC_Id);
            // shadow properties
            builder.Entity<DataEventRecord>().Property<DateTime>("UpdatedTimestamp");
            builder.Entity<SourceInfo>().Property<DateTime>("UpdatedTimestamp");

            // builder.Entity<users>().Property<DateTime>("UpdatedTimestamp");

            builder.Entity<School_M_Class>().ToTable("Adm_School_M_Class");

            builder.Entity<School_Adm_Y_StudentDMO>().ToTable("Adm_School_Y_Student");
            builder.Entity<AdmSchoolAttendanceSubjectBatch>().ToTable("Adm_School_Attendance_Subject_Batch");
            builder.Entity<AdmSchoolAttendanceSubjectBatchStudents>().ToTable("Adm_School_Attendance_Subject_Batch_Students");
            builder.Entity<PreadmisionLoginHistory>().ToTable("PreadmisionLoginHistory");
            builder.Entity<Payment_PA_Prospectus>().HasKey(m => m.FYPPP_Id);
            builder.Entity<HR_Emp_OB_Leave_DMO>().HasKey(m => m.HREOBL_Id);

            //Bus hire.
            builder.Entity<TripOnlineBookingDMO>().HasKey(d => d.TRTOB_Id);
            builder.Entity<TripDMO>().HasKey(d => d.TRTP_Id);
            builder.Entity<IVRM_ModeOfPaymentDMO>().HasKey(d => d.IVRMMOD_Id);
            builder.Entity<TR_Trip_PaymentDMO>().HasKey(d => d.TRTPP_Id);
            builder.Entity<TR_Trip_Payment_TripsDMO>().HasKey(d => d.TRTPPT_Id);
            builder.Entity<TR_Hirer_OB_DMO>().HasKey(d => d.TRHOB_Id);
            builder.Entity<MasterHirerDMO>().HasKey(d => d.TRMH_Id);

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<SourceInfo>();
            updateUpdatedProperty<DataEventRecord>();

            updateUpdatedProperty<DataEventRecord>();

            //ps
            updateUpdatedProperty<Institution>();

            updateUpdatedProperty<Institution_Phone_No>();
            updateUpdatedProperty<Institution_MobileNo>();
            updateUpdatedProperty<Institution_EmailId>();
            // updateUpdatedProperty<users>();
            updateUpdatedProperty<Preadmission_SeatBlocked_Student>();
            updateUpdatedProperty<School_M_Class>();
            updateUpdatedProperty<SMSEmailSetting>();
            updateUpdatedProperty<MasterClassHeld>();
            updateUpdatedProperty<StudycertificateReportDMO>();
            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
