using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.DriversChart.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.DriversChart;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.DriversChart.Facade
{
    [Route("api/[controller]")]
    public class DriverChartGenerationFacade : Controller
    {
        public DriverChartGenerationInterface inter;
        public DriverChartGenerationFacade(DriverChartGenerationInterface s)
        {
            inter = s;
        }
        [Route("getdetails")]
        public DriverChartGenerationDTO getdetails([FromBody] DriverChartGenerationDTO data)
        {
            return inter.getdetails(data);
        }


        [Route("savedata")]
        public DriverChartGenerationDTO savedata([FromBody] DriverChartGenerationDTO data)
        {
            return inter.savedata(data);
        }

    }
}
