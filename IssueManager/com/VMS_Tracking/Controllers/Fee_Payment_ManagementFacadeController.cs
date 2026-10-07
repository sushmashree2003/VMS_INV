using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.VMS_Tracking.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.VMS_Tracking.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class Fee_Payment_ManagementFacadeController : Controller
    {
        public Fee_Payment_ManagementInterface _inter;

        public Fee_Payment_ManagementFacadeController(Fee_Payment_ManagementInterface inter)
        {
            _inter = inter;
        }

        [Route("getloaddata")]
        public Fee_Payment_ManagementDTO getloaddata( [FromBody] Fee_Payment_ManagementDTO dto)
        {
          return  _inter.getloaddata(dto);
        }
        [Route("getreport")]
        public Fee_Payment_ManagementDTO getreport( [FromBody] Fee_Payment_ManagementDTO dto)
        {
          return  _inter.getreport(dto);
        }

    }
}