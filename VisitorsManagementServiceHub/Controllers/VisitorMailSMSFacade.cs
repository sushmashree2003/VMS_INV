using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VisitorsManagementServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VisitorsManagementServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class VisitorMailSMSFacade : Controller
    {
        public VisitorMailSMSFacadeInterface _Interface;

        public VisitorMailSMSFacade(VisitorMailSMSFacadeInterface maspag)
        {
            _Interface = maspag;
        }

        [Route("getdata/{id:int}")]
        public void getdata(int id)
        {
            _Interface.getdata(id);
        }
    }
}
