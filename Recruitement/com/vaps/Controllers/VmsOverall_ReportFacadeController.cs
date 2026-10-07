using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Produces("application/json")]
    [Route("api/VmsOverall_ReportFacade")]
    public class VmsOverall_ReportFacadeController : Controller
    {

        VMSoverall_ReportInterface interobj;
        public VmsOverall_ReportFacadeController(VMSoverall_ReportInterface obj)
        {
            interobj = obj;
        }
        
        [Route("getalldetails")]
        public VmsoverallDTO getalldetails([FromBody]VmsoverallDTO dto)
        {
            return interobj.getalldetails(dto);
        }
        [Route("getReport")]
            public VmsoverallDTO getReport([FromBody]VmsoverallDTO dto)
        {
            return interobj.getReport(dto);
        }
    }
}