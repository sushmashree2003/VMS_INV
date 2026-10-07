using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SalaryAdvanceApprovalFacadeController : Controller
    {
        // GET: api/values

        public SalaryAdvanceApprovalInterface _ads;

        public SalaryAdvanceApprovalFacadeController(SalaryAdvanceApprovalInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Emp_SalaryAdvanceDTO getinitialdata([FromBody]HR_Emp_SalaryAdvanceDTO dto)
        {
            return _ads.getBasicData(dto);
        }
        // POST api/values

        [Route("empdetails")]
        public HR_Emp_SalaryAdvanceDTO empdetails([FromBody]HR_Emp_SalaryAdvanceDTO dto)
        {
            return _ads.empdetails(dto);
        }


        [Route("approvedrecord")]
        public HR_Emp_SalaryAdvanceDTO approvedrecord([FromBody] HR_Emp_SalaryAdvanceDTO dto)
        {
            return _ads.approvedrecord(dto);
        }


        //------------------  Advance Approval Report------------------------------


        [Route("Onload")]
        public HR_Emp_SalaryAdvanceDTO Onload([FromBody]HR_Emp_SalaryAdvanceDTO dto)
        {
            return _ads.Onload(dto);
        }
        [Route("getapprovalreport")]
        public HR_Emp_SalaryAdvanceDTO getapprovalreport([FromBody]HR_Emp_SalaryAdvanceDTO dto)
        {
            return _ads.getapprovalreport(dto);
        }
    }
}