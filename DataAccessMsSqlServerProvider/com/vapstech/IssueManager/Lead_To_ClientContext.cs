using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.VMS.Sales;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccessMsSqlServerProvider.com.vapstech.IssueManager
{
   public class Lead_To_ClientContext : DbContext
    {
        public Lead_To_ClientContext(DbContextOptions<Lead_To_ClientContext> options) : base(options)
        {
            {
                Database.SetCommandTimeout(900000000);
            }

        }
        public DbSet<ISM_Sales_Lead_DMO> ISM_Sales_Lead_DMO_con { get; set; }
        public DbSet<MastersProject_DMO> MastersProject_DMO { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<ISM_Master_Client_DMO> clientTable { get; set; }
        public DbSet<MastersModule_DMO> MastersModule_DMO { get; set; }
        public DbSet<MasterModules> MasterModules { get; set; }
        public DbSet<ISM_Client_Master_Components_DMO> ISM_Client_Master_Components_DMO_con { get; set; }
    }
}
