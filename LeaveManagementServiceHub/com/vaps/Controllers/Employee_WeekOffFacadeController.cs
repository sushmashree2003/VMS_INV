using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Employee_WeekOffFacadeController : Controller
    {
        public Employee_WeekOffInterface _intface;
        public Employee_WeekOffFacadeController(Employee_WeekOffInterface intface)
        {
            _intface = intface;
        }

        [Route("OnLoadData/")]
        public Employee_DataDto getdatedetails([FromBody] Employee_DataDto dto)
        {
            return _intface.OnLoadData(dto);
        }

        [Route("Save/")]
        public Employee_DataDto Save([FromBody] Employee_DataDto dto)
        {
            return _intface.Save(dto);
        }
    }
}