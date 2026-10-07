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
    public class RTO_VoilationReportFacade : Controller
    {
        public RTO_VoilationReportInterface inter;
        public RTO_VoilationReportFacade(RTO_VoilationReportInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public RTO_VoilationReportDTO loaddata([FromBody] RTO_VoilationReportDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("RecieptReport1")]
        public RTO_VoilationReportDTO RecieptReport1([FromBody] RTO_VoilationReportDTO data)
        {
            return inter.RecieptReport1(data);
        }
        //onchange
        [Route("onchange")]
        public RTO_VoilationReportDTO onchange([FromBody] RTO_VoilationReportDTO data)
        {
            return inter.onchange(data);
        }


    }
}