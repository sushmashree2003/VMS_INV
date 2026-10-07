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
    public class Vehicle_ObservationFacade : Controller
    {
        public Vehicle_ObservationInterface inter;
        public Vehicle_ObservationFacade(Vehicle_ObservationInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public VMS_FO_Vehicle_ObservationDTO loaddata([FromBody] VMS_FO_Vehicle_ObservationDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public VMS_FO_Vehicle_ObservationDTO savedata([FromBody] VMS_FO_Vehicle_ObservationDTO data)
        {
            return inter.savedata(data);
        }
        //onchange
        [Route("onchange")]
        public VMS_FO_Vehicle_ObservationDTO onchange([FromBody] VMS_FO_Vehicle_ObservationDTO data)
        {
            return inter.onchange(data);
        }

        //savedataSource
        [Route("savedataSource")]
        public VMS_FO_Vehicle_ObservationDTO savedataSource([FromBody] VMS_FO_Vehicle_ObservationDTO data)
        {
            return inter.savedataSource(data);
        }
        //clientDecative
        [Route("clientDecative")]
        public VMS_FO_Vehicle_ObservationDTO clientDecative([FromBody] VMS_FO_Vehicle_ObservationDTO data)
        {
            return inter.clientDecative(data);
        }
        //RecieptReport
        [Route("RecieptReport")]
        public VMS_FO_Vehicle_ObservationDTO RecieptReport([FromBody] VMS_FO_Vehicle_ObservationDTO data)
        {
            return inter.RecieptReport(data);
        }
    }
}
