using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.DriversChart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.DriversChart;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager.DriversChart
{
    [Route("api/[controller]")]
    public class DriverChartGenerationController : Controller
    {
        DriverChartGenerationDelegate del = new DriverChartGenerationDelegate();

        [Route("getdetails/{id:int}")]
        public DriverChartGenerationDTO getdetails(int id)
        {
            DriverChartGenerationDTO data = new DriverChartGenerationDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdetails(data);
        }

        [HttpPost]
        [Route("savedata")]
        public DriverChartGenerationDTO savedata([FromBody]DriverChartGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }
    }
}
