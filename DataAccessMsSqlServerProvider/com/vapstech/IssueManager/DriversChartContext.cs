using DomainModel.Model.com.vapstech.IssueManager.DriversChart;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.IssueManager
{
   public class DriversChartContext : DbContext
    {
        public DriversChartContext(DbContextOptions<DriversChartContext> options) : base(options)
        {
            { Database.SetCommandTimeout(900000000); }
        }

        public DbSet<MasterDriverShiftsDMO> MasterDriverShiftsDMO { get; set; }
        public DbSet<DriverChartGenerationDMO> DriverChartGenerationDMO { get; set; }
        
    }
}
