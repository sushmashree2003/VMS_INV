using HRMSServiceHub.com.vaps.Interfaces;

using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class employeeadvanceapplyFacadeController : Controller
    {
        // GET: api/values

        public employeeadvanceapplyInterface _ads;

        public employeeadvanceapplyFacadeController(employeeadvanceapplyInterface adstu)
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
        [HttpPost]
        public HR_Emp_SalaryAdvanceDTO Post([FromBody]HR_Emp_SalaryAdvanceDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }
        [Route("Edit")]
        public HR_Emp_SalaryAdvanceDTO Edit([FromBody] HR_Emp_SalaryAdvanceDTO dto)
        {
            return _ads.Edit(dto);
        }

        [Route("deactivateRecordById")]
        public HR_Emp_SalaryAdvanceDTO deactivateRecordById([FromBody]HR_Emp_SalaryAdvanceDTO dto)
        {
            return _ads.deactivate(dto);
        }
        [Route("getDetailsByEmployee")]
        public HR_Emp_SalaryAdvanceDTO getDetailsByEmployee([FromBody]HR_Emp_SalaryAdvanceDTO dto)
        {
            return _ads.getDetailsByEmployee(dto);
        }
    }
}
