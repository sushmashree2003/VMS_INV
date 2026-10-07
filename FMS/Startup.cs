using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FMS;
using DomainModel.Model;
using FMS.Com.Interface;
using FMS.Com.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace FMS
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

            var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=VMS_Test_New;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

            services.AddDbContext<DomainModelMsSqlServerContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("WebApplication1")));

            services.AddDbContext<FMSContext>(options => options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("FMS")));

            services.AddScoped<FMS_Master_FileCategoryInterface, FMS_Master_FileCategoryImpl>();
            services.AddScoped<FMS_CorrespondenceInterface, FMS_CorrespondenceImpl>();
            
            services.AddScoped<FMS_Correspondance_ReportInterface, FMS_Correspondance_ReportImpl>();
            services.AddScoped<FMS_Master_PhysicalFileNameInterface, FMS_Master_PhysicalFileNameImpl>();
            services.AddScoped<FMS_Dept_Letter_HeadMappingInterface, FMS_Dept_Letter_HeadMappingImpl>();
            //added by akash
            services.AddScoped<FMSApprovalRequestPageInterface, FMSApprovalRequestPageImp>();
            //added by akash
            services.AddScoped<FMSApprovalInterface, FMSApprovalImp>();
            //added by akash
            services.AddScoped<FMSApprovalReportInterface, FMSApprovalReportImp>();

            // added by praveenkumar
            services.AddScoped<Employee_Wise_VMS_FeedBackInterface, Employee_Wise_VMS_FeedBackImpl>();

            //ADDED BY ADARSH FOR VMS FEED-BACK
            services.AddScoped<ModuleFeedbackInterface, ModuleFeedbackImpl>();

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
            loggerFactory.AddFile("Logs/FMS-{Date}.txt");

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
