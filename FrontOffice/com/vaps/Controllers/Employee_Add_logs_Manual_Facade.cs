using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontOffice.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Employee_Add_logs_Manual_Facade : Controller
    {
        public Employee_Add_logs_ManualInterface _org;

        public Employee_Add_logs_Manual_Facade(Employee_Add_logs_ManualInterface maspag)
        {
            _org = maspag;
        }

        [HttpPost]
        [Route("getalldetails")]
        public FO_Emp_PunchDTO Getdet([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.getdata(data);
        }

        [Route("empname")]
        public FO_Emp_PunchDTO empname([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.empname(data);
        }

        [Route("savedetail")]
        public FO_Emp_PunchDTO savedetail([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.savedetail(data);
        }

        [Route("editdata")]
        public FO_Emp_PunchDTO editdata([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.editdata(data);
        }

        [Route("approvalloaddata")]
        public FO_Emp_PunchDTO approvalloaddata([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.approvalloaddata(data);
        }

        [Route("FindData")]
        public Task<FO_Emp_PunchDTO> FindData([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.FindDataAsync(data);
        }

        [Route("saveapproval")]
        public FO_Emp_PunchDTO saveapproval([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.saveapproval(data);
        }

        [Route("onloadgetdetails")]
        public FO_Emp_PunchDTO onloadgetdetails([FromBody]FO_Emp_PunchDTO data)
        {
            return _org.getBasicData(data);
        }

    }
}
