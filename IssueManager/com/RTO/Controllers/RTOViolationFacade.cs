using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.RTO.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.RTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.RTO.Controllers
{
    [Route("api/[controller]")]
    public class RTOViolationFacade : Controller
    {
        public RTOViolationInterface inter;
        public RTOViolationFacade(RTOViolationInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public RTO_Violation_ObservationDTO loaddata([FromBody] RTO_Violation_ObservationDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public RTO_Violation_ObservationDTO savedata([FromBody] RTO_Violation_ObservationDTO data)
        {
            return inter.savedata(data);
        }
        //onchange
        [Route("onchange")]
        public RTO_Violation_ObservationDTO onchange([FromBody] RTO_Violation_ObservationDTO data)
        {
            return inter.onchange(data);
        }

        //savedataSource
        [Route("savedataSource")]
        public RTO_Violation_ObservationDTO savedataSource([FromBody] RTO_Violation_ObservationDTO data)
        {
            return inter.savedataSource(data);
        }
        //clientDecative
        [Route("clientDecative")]
        public RTO_Violation_ObservationDTO clientDecative([FromBody] RTO_Violation_ObservationDTO data)
        {
            return inter.clientDecative(data);
        }
        //RecieptReport
        [Route("RecieptReport")]
        public RTO_Violation_ObservationDTO RecieptReport([FromBody] RTO_Violation_ObservationDTO data)
        {
            return inter.RecieptReport(data);
        }
    }
}
