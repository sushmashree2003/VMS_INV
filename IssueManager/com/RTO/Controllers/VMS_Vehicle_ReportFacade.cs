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
    public class VMS_Vehicle_ReportFacade : Controller
    {
        public VMS_Vehicle_ReportInterface inter;
        public VMS_Vehicle_ReportFacade(VMS_Vehicle_ReportInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public VMS_Vehicle_ReportDTO loaddata([FromBody] VMS_Vehicle_ReportDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public VMS_Vehicle_ReportDTO savedata([FromBody] VMS_Vehicle_ReportDTO data)
        {
            return inter.savedata(data);
        }
        //onchange
        [Route("onchange")]
        public VMS_Vehicle_ReportDTO onchange([FromBody] VMS_Vehicle_ReportDTO data)
        {
            return inter.onchange(data);
        }

        //savedataSource
        [Route("savedataSource")]
        public VMS_Vehicle_ReportDTO savedataSource([FromBody] VMS_Vehicle_ReportDTO data)
        {
            return inter.savedataSource(data);
        }
        //clientDecative
        [Route("clientDecative")]
        public VMS_Vehicle_ReportDTO clientDecative([FromBody] VMS_Vehicle_ReportDTO data)
        {
            return inter.clientDecative(data);
        }
        //RecieptReport
        [Route("RecieptReport")]
        public VMS_Vehicle_ReportDTO RecieptReport([FromBody] VMS_Vehicle_ReportDTO data)
        {
            return inter.RecieptReport(data);
        }
    }
}
