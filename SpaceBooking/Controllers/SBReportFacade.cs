using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using SpaceBooking.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpaceBooking.Controllers
{
    [Route("api/[controller]")]
    public class SBReportFacade : Controller
    {
        public SBReportInterface _Interface;
        public SBReportFacade(SBReportInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("getdetails")]
        public SBReport_DTO getdetails([FromBody] SBReport_DTO data)
        {

            return _Interface.getdetails(data);
        }

        [Route("registration_report")]
        public SBReport_DTO registration_report([FromBody] SBReport_DTO data)
        {
            return _Interface.registration_report(data);
        }

        [Route("enquary_report")]
        public Task<SBReport_DTO> enquary_report([FromBody]SBReport_DTO data)
        {
            return _Interface.enquary_report(data);
        }

        [Route("booking_report")]
        public Task<SBReport_DTO> booking_report([FromBody]SBReport_DTO data)
        {
            return _Interface.booking_report(data);
        }
        
        [Route("getroom")]
        public Task<SBReport_DTO> getroom([FromBody]SBReport_DTO data)
        {
            return _Interface.getroom(data);
        }

        
    }
}
