using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Services
{
    public class VisitorMailSMSFacadeImpl
    {
        int MI_ID = 0;
        public VisitorsManagementContext _VisitorContext;
        public DomainModelMsSqlServerContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;
        public VisitorMailSMSFacadeImpl(VisitorsManagementContext contxt, DomainModelMsSqlServerContext db, IHostingEnvironment hostingEnvironment)
        {
            _VisitorContext = contxt;
            _db = db;
            _hostingEnvironment = hostingEnvironment;

        }
        public void getdata(int id)
        {
            try
            {
                MI_ID = id;

               // SendEmail(MI_ID);

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }


        


    }
}
