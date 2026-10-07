using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class IVRMandVMSInterlinkingDashboardFacadeController : Controller
    {
        // GET: api/values
        public IVRMandVMSInterlinkingDashboardInterface _ads;

        public IVRMandVMSInterlinkingDashboardFacadeController(IVRMandVMSInterlinkingDashboardInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public IVRMandVMSInterlinkingDashboardDTO getinitialdata([FromBody]IVRMandVMSInterlinkingDashboardDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("getclientdetails")]
        public Task<IVRMandVMSInterlinkingDashboardDTO> getclientdetails([FromBody]IVRMandVMSInterlinkingDashboardDTO dto)
        {
            return _ads.getclientdetails(dto);
        }

        [Route("getalldetailsQForm")]
        public IVRMandVMSInterlinkingDashboardDTO getalldetailsQForm([FromBody]IVRMandVMSInterlinkingDashboardDTO dto)
        {
            return _ads.getalldetailsQForm(dto);
        }

        [Route("getEmployeedetailsQForm")]
        public IVRMandVMSInterlinkingDashboardDTO getEmployeedetailsQForm([FromBody]IVRMandVMSInterlinkingDashboardDTO dto)
        {
            return _ads.getEmployeedetailsQForm(dto);
        }

        [Route("SearchUdetails")]
        public IVRMandVMSInterlinkingDashboardDTO SearchUdetails([FromBody]IVRMandVMSInterlinkingDashboardDTO dto)
        {
            return _ads.SearchUdetails(dto);
        }

        [Route("SearchFReport")]
        public IVRMandVMSInterlinkingDashboardDTO SearchFReport([FromBody]IVRMandVMSInterlinkingDashboardDTO dto)
        {
            return _ads.SearchFReport(dto);
        }

    }
}
