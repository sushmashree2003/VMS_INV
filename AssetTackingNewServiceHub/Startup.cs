using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using AssetTrackingServiceHub.com.vaps.Implementation;
using AssetTrackingServiceHub.com.vaps.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AssetTrackingServiceHub.com.vaps.AssetTag.Interface;
using AssetTrackingServiceHub.com.vaps.AssetTag.Implementation;
using AssetTrackingServiceHub.com.vaps.Master.Interface;
using AssetTrackingServiceHub.com.vaps.Master.Implementation;

namespace AssetTackingNewServiceHub
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
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            // var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=VMS_Test_New;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            var sqlConnectionString = "Data Source=172.16.32.23;Initial Catalog=VMS_TEST_2025;Integrated Security=False;User ID=Vaps;Password=Vts@3214;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
            // var sqlConnectionString = "Data Source=103.251.253.2;Initial Catalog=VMS_Live;Persist Security Info=False;User ID=vaps-db;Password=Vaps@#1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";


            // var sqlConnectionString = "Data Source=103.251.253.2;Initial Catalog=VMS_Live;Persist Security Info=False;User ID=vaps-db;Password=Vaps@#1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";







            services.AddScoped<DomainModelMsSqlServerContext>().AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddScoped<AssetTrackingContext>().AddDbContext<AssetTrackingContext>(options =>
         options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("AssetTrackingServiceHub")
         )
         );
            services.AddIdentity<ApplicationUser, ApplicationRole>()
             .AddEntityFrameworkStores<ApplicationDBContext>()
             .AddDefaultTokenProviders();

            //================ ASSETS TRACKING INTERFACE / IMPLEMENTION
            services.AddScoped<AssetTagSMS_NotificationInterface, AssetTagSMS_NotificationImpl>();
            services.AddScoped<AT_MasterSiteInterface, AT_MasterSiteImpl>();
            services.AddScoped<AT_MasterLocationInterface, AT_MasterLocationImpl>();
            services.AddScoped<CheckOutAssetsInterface, CheckOutAssetsImpl>();
            services.AddScoped<CheckInAssetsInterface, CheckInAssetsImpl>();
            services.AddScoped<DisposeAssetsInterface, DisposeAssetsImpl>();
            services.AddScoped<TransferAssetsInterface, TransferAssetsImpl>();
            //================================= ASSETS TRACKING REPORT
            services.AddScoped<SiteLocationsReportInterface, SiteLocationsReportImpl>();
            services.AddScoped<AssetsReportInterface, AssetsReportImpl>();
            services.AddScoped<CheckoutReportInterface, CheckoutReportImpl>();
            services.AddScoped<CheckInReportInterface, CheckInReportImpl>();
            services.AddScoped<DisposeReportInterface, DisposeReportImpl>();

            //================================= ASSETS TAG
            services.AddScoped<AssetTagInterface, AssetTagImpl>();
            services.AddScoped<AssetTagCheckOutInterface, AssetTagCheckOutImpl>();
            services.AddScoped<AssetTagCheckInInterface, AssetTagCheckInImpl>();
            services.AddScoped<AssetTagDisposeInterface, AssetTagDisposeImpl>();
            services.AddScoped<AssetTagTransferInterface, AssetTagTransferImpl>();
            //================================= ASSETS TAG Report
            services.AddScoped<AssetTag_ReportInterface, AssetTag_ReportImpl>();
            services.AddScoped<AssetTagCheckout_ReportInterface, AssetTagCheckout_ReportImpl>();
            services.AddScoped<AssetTagCheckIn_ReportInterface, AssetTagCheckIn_ReportImpl>();
            services.AddScoped<AssetTagDispose_ReportInterface, AssetTagDispose_ReportImpl>();
            services.AddScoped<AssetTagTransfer_ReportInterface, AssetTagTransfer_ReportImpl>();
            services.AddScoped<CheckOutAssetsDCInterface, CheckOutAssetsDCImpl>();
            services.AddScoped<CheckInAssetsDCInterface, CheckInAssetsDCImpl>();
            services.AddScoped<DCReportInterface, DCReportImpl>();
            services.AddScoped<AssetTagCheckOutDCInterface, AssetTagCheckOutDCIMPL>();
            services.AddScoped<AssetsTagCheckInDCInterface, AssetsTagCheckInDCIMPL>();
			
			  //sanjai
  services.AddScoped<IMasterCategory, MasterCategoryImpl>();
  services.AddTransient<IMasterSystem, MasterSystemImpl>();
  services.AddScoped<IAT_Master_CheckPoint, AT_Master_CheckPointImpl>();
  services.AddScoped<IAT_System_Category_Checkpoint_Mapping, AT_System_Category_Checkpoint_MappingImpl>();
  services.AddScoped<IHealthCheckupEntry, HealthEntryCheckupImpl>();

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

            loggerFactory.AddFile("Logs/AssetTrackingServiceHub-{Date}.txt");

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
