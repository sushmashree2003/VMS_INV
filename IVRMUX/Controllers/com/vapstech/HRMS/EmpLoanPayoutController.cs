using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Route("api/[controller]")]
    public class EmpLoanPayoutController : Controller
    {
        // GET: api/values
        EmpLoanPayoutDelegate del = new EmpLoanPayoutDelegate();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public EmpLoanPayoutDTO getalldetails(int id)
        {
            EmpLoanPayoutDTO dto = new EmpLoanPayoutDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
        }

     
        [HttpPost]
        public EmpLoanPayoutDTO Post([FromBody]EmpLoanPayoutDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public EmpLoanPayoutDTO editRecord(int id)
        {
            EmpLoanPayoutDTO dto = new EmpLoanPayoutDTO();
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }
        [Route("employeechange/{id:int}")]
        public EmpLoanPayoutDTO employeechange(int id)
        {
            EmpLoanPayoutDTO dto = new EmpLoanPayoutDTO();
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.employeechange(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public EmpLoanPayoutDTO ActiveDeactiveRecord(int id)
        {
            EmpLoanPayoutDTO dto = new EmpLoanPayoutDTO();
            dto.HRELDIS_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleterec(dto);
        }
    }
}
