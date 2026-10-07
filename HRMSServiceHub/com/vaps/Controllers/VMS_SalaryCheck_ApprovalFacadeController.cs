using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using HRMSServicesHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VMS_SalaryCheck_ApprovalFacadeController : Controller
    {
        // GET: api/values
        public VMS_SalaryCheck_ApprovalInterface _ads;

        public VMS_SalaryCheck_ApprovalFacadeController(VMS_SalaryCheck_ApprovalInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Employee_Salary_PaymentDetailsDTO getinitialdata([FromBody]HR_Employee_Salary_PaymentDetailsDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<HR_Employee_Salary_PaymentDetailsDTO> getEmployeedetailsBySelection([FromBody]HR_Employee_Salary_PaymentDetailsDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }

        [Route("saveapproval")]
        public HR_Employee_Salary_PaymentDetailsDTO saveapproval([FromBody]HR_Employee_Salary_PaymentDetailsDTO dto)
        {
            return _ads.saveapproval(dto);
        }
        
    }
}
