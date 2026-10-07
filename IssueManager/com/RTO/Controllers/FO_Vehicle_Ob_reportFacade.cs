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
    public class FO_Vehicle_Ob_reportFacade : Controller
    {
        public FO_Vehicle_Ob_reportInterface inter;
        public FO_Vehicle_Ob_reportFacade(FO_Vehicle_Ob_reportInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public FO_Vehicle_Ob_reportDTO loaddata([FromBody] FO_Vehicle_Ob_reportDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("RecieptReport1")]
        public FO_Vehicle_Ob_reportDTO RecieptReport1([FromBody] FO_Vehicle_Ob_reportDTO data)
        {
            return inter.RecieptReport1(data);
        }
        //onchange
        [Route("onchange")]
        public FO_Vehicle_Ob_reportDTO onchange([FromBody] FO_Vehicle_Ob_reportDTO data)
        {
            return inter.onchange(data);
        }

       
    }
}
