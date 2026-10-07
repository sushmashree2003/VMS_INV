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
    public class Master_Driver_ShiftsFacade : Controller
    {
        public Master_Driver_ShiftsInterface inter;
        public Master_Driver_ShiftsFacade(Master_Driver_ShiftsInterface s)
        {
            inter = s;
        }
        [Route("getdetails")]
        public DriverShiftsDTO getdetails([FromBody] DriverShiftsDTO data)
        {
            return inter.getdetails(data);
        }

        [Route("savedata")]
        public DriverShiftsDTO savedata([FromBody] DriverShiftsDTO data)
        {
            return inter.savedata(data);
        }

        [Route("EditData")]
        public DriverShiftsDTO Editdata([FromBody] DriverShiftsDTO data)
        {
            return inter.Editdata(data);
        }
    }
}
