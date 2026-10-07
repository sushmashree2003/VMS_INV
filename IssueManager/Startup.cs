using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager.HRMasterArea;
using DataAccessMsSqlServerProvider.FeedBack;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using IssueManager.com.DriversChart.Interface;
using IssueManager.com.DriversChart.Services;
using IssueManager.com.Feedback.Interface;
using IssueManager.com.Feedback.Services;
using IssueManager.com.PettyCash.Interface;
using IssueManager.com.PettyCash.Services;
using IssueManager.com.QRcode_Activity.Interfaces;
using IssueManager.com.QRcode_Activity.Services;
using IssueManager.com.Rating.Interfacce;
using IssueManager.com.Rating.Services;
using IssueManager.com.RTO.Interfaces;
using IssueManager.com.RTO.Services;
using IssueManager.com.TADA.Interface;
using IssueManager.com.TADA.Services;
using IssueManager.com.TeleCaller.Interfaces;
using IssueManager.com.TeleCaller.Services;
using IssueManager.com.Tenants.Implimentation;
using IssueManager.com.Tenants.Interface;
using IssueManager.com.vaps.Interface;
using IssueManager.com.vaps.Interfaces;
using IssueManager.com.vaps.Services;
using IssueManager.com.vaps.VMS_BUDGET.Implimentation;
using IssueManager.com.vaps.VMS_BUDGET.Interface;
using IssueManager.com.VersionControl.Interfaces;
using IssueManager.com.VersionControl.Services;
using IssueManager.com.VMS_Tracking.Interface;
using IssueManager.com.VMS_Tracking.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NaacServiceHub.Feedback.Interface;
using NaacServiceHub.Feedback.Services;
using NaacServiceHub.FeedBack.Interface;
using NaacServiceHub.FeedBack.Services;

namespace IssueManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);
            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            var sqlConnectionString = "Data Source=172.16.32.23;Initial Catalog=VMS_TEST_2025;Integrated Security=False;User ID=Vaps;Password=Vts@3214;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            //var sqlConnectionString = "Data Source=103.251.253.2;Initial Catalog=VMS_Live;Persist Security Info=False;User ID=vaps-db;Password=Vaps@#1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";




            services.AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddTransient<HRMSContext>().AddDbContext<HRMSContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("HRMSServicesHub")));

            services.AddTransient<IssueManagerContext>().AddDbContext<IssueManagerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IssueManager")));

            services.AddTransient<PettyCashContext>().AddDbContext<PettyCashContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IssueManager")));
            //added by raj
            services.AddTransient<HRMasterAreaContext>().AddDbContext<HRMasterAreaContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IssueManager")));


            services.AddTransient<FeedBackContext>().AddDbContext<FeedBackContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IssueManager")));

            services.AddTransient<FeeGroupContext>().AddDbContext<FeeGroupContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IssueManager")));

            services.AddTransient<RTOContext>().AddDbContext<RTOContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IssueManager")));

            services.AddTransient<DriversChartContext>().AddDbContext<DriversChartContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("IssueManager")));

            //================================================ Master
            services.AddScoped<ClientWise_Module_Feature_Inrerface, ClientWise_Module_FeatureImpl>();
            services.AddScoped<Fee_Payment_ManagementInterface, Fee_Payment_ManagementImpl>();
            services.AddScoped<VMS_Live_MeetingInterface, VMS_Live_MeetingImpl>();
            services.AddScoped<ISM_DailyReport_CheckListInterface, ISM_DailyReport_CheckListImpl>();
            services.AddScoped<ISM_Master_ClientProject_PaymentInterface, ISM_Master_ClientProject_PaymentImpl>();
            services.AddScoped<ISM_Master_TaskGroupInterface, ISM_Master_TaskGroupImpl>();
            services.AddScoped<Tele_caller_demoInterface, Tele_caller_DemoImpl>();

            services.AddScoped<MastersProjectInterface, MastersProjectImpl>();
            services.AddScoped<MastersPrioritiesInterface, MastersPrioritiesImpl>();
            services.AddScoped<MasterVenuesInterface, MasterVenuesImpl>();
            services.AddScoped<MasterActivitiesInterface, MasterActivitiesImpl>();
            services.AddScoped<VenueMappingInterface, VenueMappingImpl>();
            services.AddScoped<workingdaysInterface, workingdaysImpl>();
            services.AddScoped<LaptopApplicationInterface, LaptopApplicationImpl>();

            services.AddScoped<MasterStatusInterface, MasterStatusImpl>();
            services.AddScoped<MastersModuleInterface, MastersModuleImpl>();
            services.AddScoped<UserMappingInterface, UserMappingImpl>();
            services.AddScoped<ISMDashboardInterface, ISMDashboardImpl>();
            services.AddScoped<ISM_UserEmployeeMappingInterface, ISM_UserEmployeeMappingImpl>();
            services.AddScoped<ISM_BlockEmployeeInterface, ISM_BlockEmployeeImpl>();
            services.AddScoped<ISM_RoleDeptMappingInterface, ISM_RoleDeptMappingImpl>();
            services.AddScoped<ISM_ClientProject_MappingInterface, ISM_ClientProject_MappingIMPL>();
            //Added By Rohan
            services.AddScoped<ivrm_LegalProcessInterface, ivrm_LegalProcessImpl>();
            ///Added By Praveen gouda
            services.AddScoped<INV_LocationDashboardInterface, INV_LocationDashboardImpl>();
            //================================================ Transcation
            services.AddScoped<ISM_TaskCreationInterface, ISM_TaskCreationImpl>();
            services.AddScoped<ISM_ComplaintsTaskListInterface, ISM_ComplaintsTaskListImpl>();
            services.AddScoped<ISM_Client_ComplaintsTaskListInterface, ISM_Client_ComplaintsTaskListImpl>();
            services.AddScoped<ImsClientInterface, ImsClientImpl>();
            services.AddScoped<ISM_PlannerCreationInterface, ISM_PlannerCreationImpl>();
            services.AddScoped<ISM_PlannerApprovalInterface, ISM_PlannerApprovalImpl>();
            services.AddScoped<ISM_NoticeIssueInterface, ISM_NoticeIssueImpl>();
            services.AddScoped<ISM_DailyReportGenerationInterface, ISM_DailyReportGenerationImpl>();
            services.AddScoped<ISMImportInterface, ISMImportImpl>();
            services.AddScoped<EmailDepartmentMappingInterface, EmailDepartmentMappingImpl>();
            services.AddScoped<ISM_NotificationInterface, ISM_NotificationImpl>();
            services.AddScoped<ISM_InteractionInterface, ISM_InteractionImpl>();
            services.AddScoped<ISM_ExtensionInterface, ISM_ExtensionImpl>();
            services.AddScoped<ISM_PDpnt_SDpnt_MappingInterface, ISM_PDpnt_SDpnt_MappingImpl>();
            services.AddScoped<ISM_SwapIssuesInterface, ISM_SwapIssuesImpl>();
            //added
            services.AddScoped<ISM_PrinterStatisticsInterface, ISM_PrinterStatisticsImpl>();
            services.AddScoped<ISM_PrinterStatisticsReportInterface, ISM_PrinterStatisticsReportImpl>();
            services.AddScoped<Employee_VMS_DeactivateInterface, Employee_VMS_DeactivateIMLP>();
            services.AddScoped<ISM_Task_DeviationInterface, ISM_Task_DeviationImpl>();

            //
            //================================================ Reports
            services.AddScoped<ISM_PlannerDeviationReportInterface, ISM_PlannerDeviationReportImpl>();
            services.AddScoped<ISM_InterdepartmentIsuuesReportInterface, ISM_InterdepartmentIsuuesReportImpl>();
            services.AddScoped<ISM_PlannerDeptDeviationReportInterface, ISM_PlannerDeptDeviationReportImpl>();
            services.AddScoped<ISM_ConsolidateDailyReportInterface, ISM_ConsolidateDailyReportImpl>();
            services.AddScoped<ISM_TaskRegisterReportInterface, ISM_TaskRegisterReportImpl>();
            services.AddScoped<ISM_EmployeeBlockReportInterface, ISM_EmployeeBlockReportImpl>();
            services.AddScoped<ISM_ApprovalReportInterface, ISM_ApprovalReportImpl>();
            services.AddScoped<ISM_PlannerReportInterface, ISM_PlannerReportImpl>();
            services.AddScoped<ISM_MemoNoticeReportInterface, ISM_MemoNoticeReportImpl>();
            services.AddScoped<EmailDeviationInterface, EmailDeviationImpl>();
            services.AddScoped<ISM_TaskGroup_ReportInterface, ISM_TaskGroup_ReportImpl>();
            services.AddScoped<HR_Planner_ManagementInterface, HR_Planner_ManagementImpl>();
            services.AddScoped<ComplaintRegisterInterface, ComplaintRegisterImpl>();
            services.AddScoped<EmployeeMonthEndInterface, EmployeeMonthEndImpl>();
            services.AddScoped<ISM_SwapIssueReportInterface, ISM_SwapIssueReportImpl>();

            //===================================================Common Task
            services.AddScoped<ISM_CommonTaskInterface, ISM_CommonTaskImpl>();
            services.AddScoped<DriverIndentInterface, DriverIndentImpl>();
            services.AddScoped<ISM_ImplTaskMappingInterface, ISM_ImplTaskMappingImpl>();
            services.AddScoped<ISM_Master_TaskCategoryInterface, ISM_Master_TaskCategoryImpl>();
            services.AddScoped<ISM_Master_TaskCategory_EmployeeInterface, ISM_Master_TaskCategory_EmployeeImpl>();


            // PETTY CASH 
            services.AddScoped<PC_Master_ParticularsInterface, PC_Master_ParticularsImpl>();
            services.AddScoped<PC_RequisitionInterface, PC_RequisitionImpl>();
            services.AddScoped<PC_IndentInterface, PC_IndentImpl>();
            services.AddScoped<PC_Indent_ApprovalInterface, PC_Indent_ApprovalImpl>();
            services.AddScoped<PC_ReportInterface, PC_ReportImpl>();

            services.AddScoped<ISM_AttendanceReportInterface, ISM_AttendanceReportImpl>();
            services.AddScoped<ISM_DailyReport_ApprovalInterface, ISM_DailyReport_ApprovalImpl>();
            services.AddScoped<ISM_PrioritySwitchReportInterface, ISM_PrioritySwitchReportImpl>();
            services.AddScoped<ClientProformaInvoiceInterface, ClientProformaInvoiceImpl>();
            services.AddScoped<ClientInvoiceInterface, ClientInvoiceImpl>();
            services.AddScoped<CheckBookEntryApprovalInterface, CheckBookEntryApprovalImpl>();
            services.AddScoped<CheckBookEntryInterface, CheckBookEntryImpl>();
            services.AddScoped<CheckBookEntryReportInterface, CheckBookEntryReportImpl>();
            services.AddScoped<CheckBankBalanceInterface, CheckBankBalanceImpl>();
            services.AddScoped<CheckBookPrintInterface, CheckBookPrintImpl>();
            services.AddScoped<VMS_Receipt_VoucherInterface, VMS_Receipt_VoucherIMPL>();


            //QRcode_Activity//
            services.AddScoped<HRMasterAreaInterface, HRMasterAreaImpl>();
            services.AddScoped<HRMasterAreaStaffMappingInterface, HRMasterAreaStaffMappingImpl>();
            services.AddScoped<StaffAreaActivityRecordInterface, StaffAreaActivityRecordImpl>();
            services.AddScoped<QRcodeDeviationReportInterface, QRcodeDeviationReportImpl>();
            services.AddScoped<QRcodeNotPunchDeviationReportInterface, QRcodeNotPunchDeviationReportImpl>();

            //Tenants
            services.AddScoped<TenantAgreementInterface, TenantAgreementImpl>();

             services.AddScoped<TenantUploadNoticeInterface, TenantUploadNoticeImpl>();
             services.AddScoped<TenantAgreementReportInterface,TenantAgreementReportImpl>();
             services.AddScoped<TenantViewNoticesInterface, TenantViewNoticesImpl>();
             services.AddScoped<TenantPaymentReprotInterface,TenantPaymentReprotImpl>();
            
            services.AddScoped<TenantAgreementApprovalInterface, TenantAgreementApprovalImpl>();
            services.AddScoped<TenantUploadNoticeInterface, TenantUploadNoticeImpl>();
            services.AddScoped<TenantAgreementReportInterface, TenantAgreementReportImpl>();
            services.AddScoped<TenantViewNoticesInterface, TenantViewNoticesImpl>();
            services.AddScoped<TenantPaymentReprotInterface, TenantPaymentReprotImpl>();
            services.AddScoped<TenantTaskCreationInterface, TenantTaskCreationImpl>();
            services.AddScoped<RoomBankDeatilsInterface, RoomBankDeatilsImpl>();

            //Drivers Chart 

            services.AddScoped<Master_Driver_ShiftsInterface, Master_Driver_ShiftsImpl>();
            services.AddScoped<DriverChartGenerationInterface, DriverChartGenerationImpl>();

            //=========================TADA==
            services.AddScoped<TADA_Advance_Interface, TADAAdvance_ApplyIMPL>();
            services.AddScoped<TADACityMappingInterface, TADA_CityMappingIMPL>();
            services.AddScoped<TADA_ApplyInterface, TADA_ApplyIMPL>();
            services.AddScoped<TADA_ReportsInterface, TADA_ReportsIMPL>();
            services.AddScoped<TADAAdvanceApprovalInterface, TADAAdvanceApprovalImpl>();
            services.AddScoped<TADAApprovalInterface, TADAApprovalImpl>();
            services.AddScoped<TADAApply_PaymentInterFace, TADAApply_PaymentIMPL>();
            services.AddScoped<TADA_AdavanceApply_PaymentInterface, TADA_AdavanceApply_PaymentIMPL>();
            services.AddScoped<TADA_PaymentReportInterface, TADA_PaymentReportIMPL>();
            //=============
            services.AddScoped<EmailSms_HistoryReportInterface, EmailSms_HistoryReportIMPL>();
            services.AddScoped<IssueManagerReportInterface, IssueManagerReportIMPL>();
            services.AddScoped<RemaindersDetailsInterface, RemaindersDetailsImpl>();

            services.AddScoped<Rating_MasterInterface, Rating_MasterImpl>();
            services.AddScoped<Rating_TransactionInterface, Rating_TransactionImpl>();
            services.AddScoped<TypeQuestionOptionMappingInterface, TypeQuestionOptionMappingImpl>();
            services.AddScoped<HW_HeathCheckUpInterface, HW_HeathCheckUpImpl>();

            services.AddScoped<VMS_SendNoticeInterface, VMS_SendNoticeImpl>();

            services.AddScoped<FeedBackMasterTypeInterface, FeedBackMasterTypeImpl>();
            services.AddScoped<FeedbackTypeQuestionMappingInterface, FeedbackTypeQuestionMappingImpl>();
            services.AddScoped<FeedbackTransactionInterface, FeedbackTransactionImpl>();
            services.AddScoped<FeedBackReportInterface, FeedBackReportImpl>();
            services.AddScoped<FeedbackSchoolGeneralTransactionInterface, FeedbackSchoolGeneralTransactionImpl>();
            services.AddScoped<AcademicCalenderReportInterface, AcademicCalenderReportImpl>();
            services.AddScoped<FeedBackSchoolReportInterface, FeedBackSchoolReportIMPL>();
            services.AddScoped<ViewOfDeploymentInterface, ViewOfDeploymentImpl>();
            services.AddScoped<VmsAuditReportsInterface, VmsAuditReportsIMPL>();
            services.AddScoped<FeeInstallmentInterface, FeeInstallmentImpl>();
            //RTO
            services.AddScoped<RTO_Master_FineInterface, RTO_Master_FineIMPL>();
            services.AddScoped<Vehicle_ObservationInterface, Vehicle_ObservationIMPL>();
            services.AddScoped<RTOViolationInterface, RTOViolationIMPL>();
            services.AddScoped<VMS_Vehicle_ReportInterface, VMS_Vehicle_ReportIMPL>();
            services.AddScoped<RTO_VoilationReportInterface, RTO_VoilationReportIMPL>();
            //
            services.AddScoped<CheckBook_BudgetHeadsInterface, CheckBook_BudgetHeadsIMPL>();
            services.AddScoped<FO_Vehicle_Ob_reportInterface, FO_Vehicle_Ob_reportIMPL>();
            services.AddScoped<TenantWaterBillReportInterface, TenantWaterBillReportImp>();
            services.AddScoped<RoomWiseAmenitiesMappingInterface, RoomWiseAmenitiesMappingimpl>();
            services.AddScoped<flathistoryreportInterface, flathistoryreportImpl>();
            services.AddScoped<TenantOfflinePaymentReportInterface, TenantOfflinePaymentReportImpl>();
            services.AddScoped<TenantPaymentOrFlatwiseReportInterface, TenantPaymentOrFlatwiseReportImpl>();
            services.AddScoped<NocCreationInterface, NocCreationImpl>();
            services.AddScoped<TenantMasterNocChecklistInterface, TenantMasterNocChecklistImpl>();
            services.AddScoped<NocGenerationReportInterface, NocGenerationReportImpl>();
            services.AddScoped<TenantWaterBillInterface, TenantWaterBillImpl>();
            services.AddScoped<CheckBookLogReportInterface, CheckBookLogReportImpl>();
            services.AddScoped<VMS_ChequePrinting_SettingInterface, VMS_ChequePrinting_SettingImpl>();
            services.AddScoped<Master_DepartmentHead_MappingInterface, Master_DepartmentHead_MappingIMPL>();
            services.AddScoped<EmployeeHeadFeedBackInterface, EmployeeHeadFeedBackImpl>();
            services.AddScoped<TenantPaidandPendinglistInterface, TenantPaidandPendinglistImpl>();

            //===========Version Control
            services.AddScoped<IVRM_VersionControlProcessInterface, IVRM_VersionControlProcessIMPL>();
            services.AddScoped<TADA_Advance_TourInterface, TADA_Advance_TourIMPL>();

            services.AddScoped<TADACityDistanceMappingInterface, TADACityDistanceMappingImpl>();
            services.AddScoped<TADARoot_CategoryInterface, TADARoot_CategoryIMPL>();
            services.AddScoped<LegalUpdateInterface, LegalUpdateIMPL>();
            services.AddScoped<LegalHistoryInterface, LegalHistoryIMPL>();
            services.AddScoped<LegalUpdateMasterInterface, LegalUpdateMasterImpl>();
            services.AddScoped<ModuleWiseFeedbackReportInterface, ModuleWiseFeedbackReportImpl>();
            //===========Budget 
            services.AddScoped<Vms_BudgetInterface, Vms_BudgetImpl>();


            //===========Version Control END
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/IssueManager-{Date}.txt");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}