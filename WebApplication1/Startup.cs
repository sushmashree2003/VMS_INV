using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DataAccessMsSqlServerProvider;
using DomainModel;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Localization;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("config.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);


            // Use a PostgreSQL database
            //var sqlConnectionString = Configuration["DataAccessMsSqlServerProvider:ConnectionString"];

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
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            //var sqlConnectionString = Configuration["DataAccessMsSqlServerProvider:ConnectionString"];

            services.AddSession();


            //var sqlConnectionString = "Data Source=103.251.253.2;Initial Catalog=VMS_Live;Persist Security Info=False;User ID=vaps-db;Password=Vaps@#1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";



            var sqlConnectionString = "Data Source=172.16.32.23;Initial Catalog=VMS_TEST_2025;Integrated Security=False;User ID=Vaps;Password=Vts@3214;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";




            services.AddDbContext<logincontext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApstplication1")));

            services.AddDbContext<registrationcontext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<monthendreportContext>().AddDbContext<monthendreportContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddDbContext<Enquirycontext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<userDetailscontext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<StudentApplicationContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<OrganisationContext>().AddDbContext<OrganisationContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1").UseRowNumberForPaging()));

            services.AddDbContext<MasterPageContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<MasterTemplateContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<MasterCategoryContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<IssueManagerContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<MasterBoardandSchoolTypeContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            //       services.AddDbContext<CommunicationContext>(options =>
            //    options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
            //    )
            //);

            services.AddScoped<MasterPageModuleMappingContext>().AddDbContext<MasterPageModuleMappingContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<IDataAccessProvider, DataAccessMsSqlServerProvider.DataAccessMsSqlServerProvider>();

            services.AddDbContext<DomainModelMsSqlServerContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")                  
                    .UseRowNumberForPaging()));

            services.AddDbContext<MasterSourceContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            //         services.AddDbContext<InstitutionRoleModulePreviledgeContext>(options =>
            //    options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")
            //    )
            //);


            services.AddDbContext<MasterRoleContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<MasterRoleTypeContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<MasterRolePreviledgesContext>().AddDbContext<MasterRolePreviledgesContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<WrittenTestScheduleContext>().AddDbContext<WrittenTestScheduleContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<StudentDetailsContext>().AddDbContext<StudentDetailsContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<OralTestScheduleContext>().AddDbContext<OralTestScheduleContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<WrittenTestMarksEntryContext>().AddDbContext<WrittenTestMarksEntryContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<OralTestMarksENtryContext>().AddDbContext<OralTestMarksENtryContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<MasterModulesContext>().AddDbContext<MasterModulesContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<AcademicContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<ProspectusContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<MasterSectionContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<MasterReferenceContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<MasterSubjectContext>().AddDbContext<MasterSubjectContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<ApplicationDBContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            //services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            //{
            //    options.Cookies.ApplicationCookie.AuthenticationScheme = "ApplicationCookie";
            //}).AddEntityFrameworkStores<ApplicationDBContext, int>()
            //  .AddDefaultTokenProviders();


            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDBContext>();

            services.AddScoped<OralTestMarksENtryContext>().AddDbContext<OralTestMarksENtryContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<StaffLoginContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<enquiryreportContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<ReportProspectusContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<TransfrPreAdmtoAdmContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<Preadmission_School_Registration_CatergoryContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<AdmissionStatusContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<MasterMainMenuContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<ScheduleReportContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<InstituteMainMenuContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<RegistrationReportContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<MarksReportContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<FeeGroupContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<AdmissionFormContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddDbContext<SubjectwisePeriodSettingsContext>(options =>options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AdmissionServiceHub")));

            services.AddScoped<OrganisationContext>();

            services.AddMvc().AddJsonOptions(options =>
            {
                //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //options.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
                //options.SerializerSettings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                //options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

                //options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });

            //services.AddSingleton<Test,TestImpl>();

            services.AddScoped<logininterface, loginimpl>();
            services.AddScoped<registration, registrationimp>();
            services.AddScoped<userdet, userDetailsImpl>();
            services.AddScoped<StudentApplicationInterface, StudentApplicationImpl>();
            services.AddScoped<Organisationinterface, OrganisationImpl>();
            services.AddScoped<MasterPageInterface, MasterPageImpl>();
            services.AddScoped<MasterPageModuleMappingInterface, MasterPageModuleMappingImpl>();
            services.AddScoped<MasterSourceInterface, MasterSourceImpl>();
            // services.AddSingleton<InstitutionRoleModulePreviledgeInterface, InstitutionRoleModulePreviledgeImpl>();
            services.AddScoped<MasterRoleInterface, MasterRoleImpl>();
            services.AddScoped<MasterRoleTypeInterface, MasterRoleTypeImpl>();
            services.AddScoped<MasterRolePreviledgeInterface, MasterRolePreviledgeImpl>();
            services.AddScoped<StudentMasterConfigurationInterface, StudentMasterConfigurationImpl>();
            services.AddScoped<InstitutionTemplateInterface, InstitutionTemplateImpl>();

            services.AddScoped<masterTemplateInterface, masterTemplateImpl>();
            services.AddScoped<MasterCategoryInterface, MasterCategoryImpl>();
            services.AddScoped<MasterBoardandSchoolTypeInterface, MasterBoardandSchoolTypeImpl>();
            services.AddScoped<TransactionNumberingInterface, TransactionNumberingImpl>();
            services.AddScoped<MasterInstitutionRoleandModuleMappingInterface, MasterInstitutionRoleandModuleMappingImpl>();
            services.AddScoped<MasterModulesInterface, MasterModulesImp>();
            services.AddScoped<WrittenTestScheduleInterface, WrittenTestScheduleImp>();
            services.AddScoped<OralTestScheduleInterface, OralTestScheduleImp>();
            services.AddScoped<WrittenTestMarksEntryInterface, WrittenTestMarksEntryImpl>();
            services.AddScoped<OralTestMarksEntryInterface, OralTestMarksEntryImpl>();
            services.AddScoped<AcademicInterface, AcademicImpl>();
            services.AddScoped<prospectus, ProspectusImpl>();
            services.AddScoped<EnquiryInterface, EnqImpl>();
            services.AddScoped<SMSReportInterface, SMSReportImpl>();
            services.AddScoped<Institutioninterface, InstitutionImpl>();
            services.AddScoped<SeatBlockInterface, SeatBlockImpl>();
            services.AddScoped<School_M_ClassInterface, School_M_ClassImpl>();
            services.AddScoped<CommonInterface, CommonImpl>();

            services.AddScoped<MasterSectionInterface, MasterSectionImpl>();

            services.AddScoped<MasterReferenceInterface, MasterReferenceImpl>();

            services.AddScoped<MasterSubjectInterface, MasterSubjectImpl>();
            services.AddScoped<StaffLoginInterface, StaffLoginImpl>();

            services.AddScoped<enquiryreportInterface, enquiryreportImpl>();
            services.AddScoped<ReportProspectusInterface, ReportProspectusImpl>();

            services.AddScoped<TransfrPreAdmtoAdmInterface, TransfrPreAdmtoAdmImpl>(); //added on 13 dec

            services.AddScoped<StatusInterface, StatusImpl>();  //added on 20 dec

            services.AddScoped<ConcessionApprovalInterface, ConcessionApprovalImpl>();  //added on 27 dec

            services.AddScoped<AdmissionStatusInterface, AdmissionStatusImpl>();  //added on 04 Jan 2017

            services.AddScoped<MasterMainMenuInterface, MasterMainMenuImp>();

            services.AddScoped<MasterSubMenuInterface, MasterSubMenuImp>();


            services.AddScoped<ScheduleReportInterface, ScheduleReportImpl>(); //added by vishnu
            services.AddScoped<SeatBlockReportInterface, SeatBlockReportImpl>(); //added by vishnu

            services.AddScoped<MasterClassCategoryInterface, MasterClassCategoryImpl>();

            services.AddScoped<MasterMenuPageMappingInterface, MasterMenuPageMappingService>(); //added by vishnu
            services.AddScoped<MasterMenuPageMappingInstitutionwisInterface, MasterMenuPageMappingInstitutionwisService>(); //added by vishnu
            services.AddScoped<InstituteMainMenuInterface, InstituteMainMenuImp>(); //added by vishnu
            services.AddScoped<InstituteSubMenuInterface, InstituteSubMenuImp>(); //added by vishnu
            services.AddScoped<RegistrationReportInterface, RegistrationReportImpl>();

            services.AddScoped<changepwdInterface, changepwdImpl>();
            services.AddScoped<MasterSubjectAllMInterface, MasterSubjectAllMImpl>();

            services.AddScoped<MarksReportInterface, MarksReportImpl>();

            services.AddScoped<DocumentViewInterface, DocumentViewImpl>();

            services.AddScoped<MeritListReportInterface, MeritListReportImpl>();


            // Added on 25-10-2017
            services.AddScoped<MandatorysettingsInterface, MandatorysettingsImpl>();
            services.AddScoped<InstituteWiseMandatorysettingsInterface, InstituteWiseMandatorysettingsImpl>();

            services.AddScoped<GenConfigInterface, GenConfigImpl>();

            services.AddScoped<PointsInterface, PointsImpl>();
            services.AddScoped<PointsReportInterface, PointsReportImpl>();
            services.AddScoped<OralTestReScheduleInterface, OralTestReScheduleImp>();
            services.AddScoped<BuspassFormInterface, BuspassFormImpl>();

            services.AddScoped<intimationscheduleInterface, intimationscheduleImpl>();
            services.AddScoped<ConsolidatesRankReportInterface, ConsolidatesRankReportImp>();
            services.AddScoped<PremonthendreportInterface, PremonthendreportImpl>();
            services.AddScoped<flashnewsInterface, flashnewsImpl>();

          

            services.AddScoped<MasterQuestionInterface, MasterQuestionImpl>();
            services.AddScoped<OnlineExamInterface, OnlineExamImpl>();
            services.AddScoped<OnlineExamConfigInterface, OnlineExamConfigImpl>();

            services.AddScoped<IVRMandVMSInterlinkingDashboardInterface, IVRMandVMSInterlinkingDashboardIMPL>();


            //smartcard freeze
            services.AddScoped<SmartCardFreezeInterface, SmartCardFreezeImpl>();

        



            Mapper.Initialize(config =>

            {
                config.CreateMap<Enquiry, Enq>().ReverseMap();
                config.CreateMap<Registration, regis>().ReverseMap();
                config.CreateMap<userDetails, usrdetails>().ReverseMap();
                config.CreateMap<StudentApplication, StudentApplicationDTO>().ReverseMap();
                config.CreateMap<OralTestScheduleDMO, OralTestScheduleDTO>().ReverseMap();

                // Added 19-9-2016
                config.CreateMap<StudentGuardian, StudentApplicationDTO>().ReverseMap();
                config.CreateMap<StudentUploadImage, StudentApplicationDTO>().ReverseMap();
                // config.CreateMap<StudentTrnxDoc, StudentApplicationDTO>().ReverseMap();

                config.CreateMap<StudentTrnxDoc, PreadmissionSchoolRegistrationDocumentsDTO>().ReverseMap();

                config.CreateMap<StudentSibling, StudentSiblingDTO>().ReverseMap();
                config.CreateMap<StudentPreviousSchool, StudentPrevSchoolDTO>().ReverseMap();
                config.CreateMap<StudentTransport, StudentApplicationDTO>().ReverseMap();

                config.CreateMap<MasterConfiguration, MasterConfigurationDTO>().ReverseMap();
                // Added 19-9-2016

                // Added on 6-10-2016 by sachin
                config.CreateMap<InstituteTemplate, InstitutionTemplateDTO>().ReverseMap();
                // Added on 6-10-2016 by sachin

                config.CreateMap<Organisation, OrganisationDTO>().ReverseMap();
                config.CreateMap<MasterPage, MasterPageDTO>().ReverseMap();
                config.CreateMap<Institution, InstitutionDTO>().ReverseMap();
                config.CreateMap<MasterRole, MasterRoleDTO>().ReverseMap();

                config.CreateMap<OrganisationEmail, Organisation_EmailIdDTO>().ReverseMap();
                config.CreateMap<OrganisationMobile, Organisation_MobileDTO>().ReverseMap();
                config.CreateMap<OrganisationPhone, Organisation_Phone_NoDTO>().ReverseMap();

                config.CreateMap<MasterModule, MasterModuleDTO>().ReverseMap();
                config.CreateMap<MasterModules, MasterModulesDTO>().ReverseMap();
                config.CreateMap<MasterPageModuleMapping, MasterPageModuleMappingDTO>().ReverseMap();
                config.CreateMap<MasterSource, MasterSourceDTO>().ReverseMap();

                config.CreateMap<TransactionNumbering, TransactionNumberingDTO>().ReverseMap();
                config.CreateMap<Institution_Module, Institution_ModuleDTO>().ReverseMap();
                config.CreateMap<Institution_Module_Page, Institution_Module_PageDTO>().ReverseMap();

                //  config.CreateMap<InstitutionRoleModulePreviledgeDMO, InstitutionRoleModulePreviledgeDTO>().ReverseMap();

                config.CreateMap<MasterRole, MasterRoleDTO>().ReverseMap();
                config.CreateMap<MasterRoleType, MasterRoleTypeDTO>().ReverseMap();

                config.CreateMap<WrittenTestStudentSubjectWiseMarksDMO, WirettenTestSubjectWiseStudentMarksDTO>().ReverseMap();
                config.CreateMap<WIrttenTestSubjectWiseMarksDMO, WirttenTestSubjectWiseMarksEntryDTO>().ReverseMap();
                config.CreateMap<WrittenTestStudentWiseTotalMarksDMO, WrittenTestSutdentTotalMarksDTO>().ReverseMap();
                config.CreateMap<WIrttenTestSubjectWiseMarksDMO, WrittenTestMarksBindDataDTO>().ReverseMap();
                config.CreateMap<WrittenTestScheduleMarksDMO, WrittenTestScheduleWiseMarksDTO>().ReverseMap();

                config.CreateMap<WrittenTestScheduleDMO, WrittenTestScheduleDTO>().ReverseMap();


                config.CreateMap<OralTestOralByMarksDMO, OralTestOralByMarksDTO>().ReverseMap();
                config.CreateMap<OralTestScheduleMarksMapDMO, OralTestScheduleMarksMapDTO>().ReverseMap();
                config.CreateMap<OralTestStudentWiseMarksDMO, OralTestStudentWiseMarksDTO>().ReverseMap();
                config.CreateMap<OralTestStudentStatusDMO, OralTestStudentStatusDTO>().ReverseMap();
                config.CreateMap<OralTestOralByMarksDMO, OralTestMarksBindDataDTO>().ReverseMap();
                config.CreateMap<MasterRolePreviledgeDMO, MasterRolePreviledgeDTO>().ReverseMap();

                //ps
                config.CreateMap<Institution, InstitutionDTO>().ReverseMap();

                config.CreateMap<Institution_MobileNo, Institution_MobileDTO>().ReverseMap();
                config.CreateMap<Institution_Phone_No, Institution_Phone_NoDTO>().ReverseMap();
                config.CreateMap<Institution_EmailId, Institution_EmailIdDTO>().ReverseMap();

                config.CreateMap<Preadmission_SeatBlocked_Student, Preadmission_SeatBlocked_StudentDTO>().ReverseMap();
                config.CreateMap<School_M_Class, School_M_ClassDTO>().ReverseMap();

                config.CreateMap<MasterCategory, MasterCategoryDTO>().ReverseMap();
                config.CreateMap<MasterTemplate, MasterTemplateDTO>().ReverseMap();
                config.CreateMap<MasterBorad, MasterBoardDTO>().ReverseMap();
                config.CreateMap<MasterSchoolType, MasterSchoolTypeDTO>().ReverseMap();
                config.CreateMap<MasterAcademic, AcademicDTO>().ReverseMap();
                config.CreateMap<Prospectus, ProspectusDTO>().ReverseMap();

                config.CreateMap<School_M_Section, MasterSectionDTO>().ReverseMap();

                config.CreateMap<MasterReference, MasterRefernceDTO>().ReverseMap();

                config.CreateMap<MasterSubjectDMO, MasterSubjectDTO>().ReverseMap();
                config.CreateMap<StaffLoginDMO, StaffLoginDTO>().ReverseMap();
                config.CreateMap<DomainModel.Model.ApplicationUserRole, AppUserRoleDTO>().ReverseMap();

                config.CreateMap<Master_Numbering, Master_NumberingDTO>().ReverseMap(); //11/11/2016  
                config.CreateMap<InstitutionRolePrivileges, InstitutionRolePrivilegesDTO>().ReverseMap(); // 15/11/2016


                config.CreateMap<Adm_M_Student, Adm_M_StudentDTO>().ReverseMap();
                config.CreateMap<Master_Institution_SubscriptionValidity, Master_Institution_SubscriptionValidityDTO>().ReverseMap();

                config.CreateMap<Preadmission_School_Registration_CatergoryDMO, Preadmission_School_Registration_CatergoryDTO>().ReverseMap();
                config.CreateMap<Fee_Master_ConcessionDMO, Fee_Master_ConcessionDTO>().ReverseMap();

                config.CreateMap<Preadmission_School_Registration_Concession_StatusDMO, Preadmission_School_Registration_Concession_StatusDTO>().ReverseMap();

                config.CreateMap<AdmissionStatus, AdmissionStatusDTO>().ReverseMap();  //added on 04 Jan 2017

                config.CreateMap<MasterMainMenuDMO, MasterMainMenuDTO>().ReverseMap();
                config.CreateMap<UserRoleWithInstituteDMO, UserRoleWithInstituteDTO>().ReverseMap();

                config.CreateMap<OralTestScheduleMarksMapDMO, ScheduleReportDTO>().ReverseMap(); //added by vishnu

                config.CreateMap<Preadmission_SeatBlocked_Student, SeatBlockReportDTO>().ReverseMap(); //added by vishnu

                config.CreateMap<IVRM_Master_Menu_Page_MappingDMO, IVRM_Master_Menu_Page_MappingDTO>().ReverseMap(); //added by vishnu
                config.CreateMap<MasterMenuPageMappingInstituteWise, IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO>().ReverseMap(); //added by vishnu

                config.CreateMap<Masterclasscategory, MasterClassCategoryDTO>().ReverseMap();

                config.CreateMap<InstituteMainMenuDTO, InstituteMainMenuDMO>().ReverseMap();
                services.AddSingleton<countstatusreportInterface, countstatusreportImpl>();


                config.CreateMap<PaymentDetails, Prospectus>().ReverseMap();
                config.CreateMap<Prospepaymentamount, Prospectus>().ReverseMap();
                config.CreateMap<IVRM_Master_SubjectsDMO, MasterSubjectAllMDTO>().ReverseMap();


                config.CreateMap<IVRM_Master_SubjectsDMO, subject_orderDTO>().ReverseMap();

                config.CreateMap<GeneralConfigDMO, GeneralConfigDTO>().ReverseMap();


                config.CreateMap<PA_School_Application_ProspectusDMO, PA_School_Application_ProspectusDTO>().ReverseMap();


                // Added on 25-10-2017
                config.CreateMap<IVRM_Mandatory_Setting, IVRM_Mandatory_SettingDTO>().ReverseMap();
                config.CreateMap<IVRM_Mandatory_Setting_IW, IVRM_Mandatory_Setting_IWDTO>().ReverseMap();

                config.CreateMap<StudentHelthcertificateDMO, StudentHelthcertificateDTO>().ReverseMap();

                config.CreateMap<PointsDMO, PointsDTO>().ReverseMap();
                config.CreateMap<PA_Student_Transport_ApplicationDMO, StudentHelthcertificateDTO>().ReverseMap();
                config.CreateMap<subCaste, StateDTO>().ReverseMap();


            });

            // services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/feehub-{Date}.txt");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();

            app.Use((context, next) =>
            {
                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.StatusCode = 200;
                    return context.Response.WriteAsync("handled");
                }

                return next.Invoke();
            });
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();
        }
    }
}
