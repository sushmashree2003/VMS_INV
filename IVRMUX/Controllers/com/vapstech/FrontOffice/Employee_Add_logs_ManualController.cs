using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.FrontOffice
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class Employee_Add_logs_ManualController : Controller
    {
        Employee_Add_logs_ManualDelegate ad = new Employee_Add_logs_ManualDelegate();

        [Route("getalldetails/{id:int}")]
        public FO_Emp_PunchDTO getdetails(int id)
        {
            FO_Emp_PunchDTO data = new FO_Emp_PunchDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.getdetails(data);
        }

        [Route("empname")]
        public FO_Emp_PunchDTO empname([FromBody]FO_Emp_PunchDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.empname(data);
        }

        [Route("savedetail")]
        public FO_Emp_PunchDTO savedetail([FromBody]FO_Emp_PunchDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.savedetail(data);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("editdata/{id:int}")]
        public FO_Emp_PunchDTO editdata(int id)
        {
            FO_Emp_PunchDTO data = new FO_Emp_PunchDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.FOEPD_Id = id;
            return ad.editdata(data);
        }

        [Route("approvalloaddata/{id:int}")]
        public FO_Emp_PunchDTO approvalloaddata(int id)
        {
            FO_Emp_PunchDTO data = new FO_Emp_PunchDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.approvalloaddata(data);
        }

        [Route("FindData")]
        public FO_Emp_PunchDTO FindData([FromBody]FO_Emp_PunchDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.FindData(data);
        }

        [Route("saveapproval")]
        public FO_Emp_PunchDTO saveapproval([FromBody]FO_Emp_PunchDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.saveapproval(data);
        }
        [HttpGet]
        [Route("getalldetails1/{id:int}")]
        public FO_Emp_PunchDTO getalldetails(int id)
        {
            FO_Emp_PunchDTO dto = new FO_Emp_PunchDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return ad.onloadgetdetails(dto);
        }
    }
}
