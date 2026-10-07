using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.LeaveManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace IVRMUX.Controllers.com.vapstech.LeaveManagement
{
    [Route("api/[controller]")]
    public class Employee_WeekOffController : Controller
    {
        Employee_WeekOffDelegate em = new Employee_WeekOffDelegate();


        [Route("OnLoadData/{id:int}")]
        public Employee_DataDto OnLoadData(int id)
        {
            Employee_DataDto dto = new Employee_DataDto();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return em.getdatedetails(dto);
        }
        [Route("Save")]
        public Employee_DataDto Save([FromBody] Employee_DataDto dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return em.Save(dto);
        }

    }
}