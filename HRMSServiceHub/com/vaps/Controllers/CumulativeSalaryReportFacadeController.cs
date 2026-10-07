using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CumulativeSalaryReportFacadeController : Controller
    {
        public CumulativeSalaryReportInterface _ads;

        public CumulativeSalaryReportFacadeController(CumulativeSalaryReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Employee_SalaryDTO getinitialdata([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<HR_Employee_SalaryDTO> getEmployeedetailsBySelection([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }
        [Route("get_depts")]
        public HR_Employee_SalaryDTO get_depts([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public HR_Employee_SalaryDTO get_desig([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _ads.get_desig(dto);
        }
        [Route("getapprvaldata")]
        public HR_Employee_SalaryDTO getapprvaldata([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _ads.getapprvaldata(dto);
        }
        [Route("SalaryApproval")]
        public HR_Employee_SalaryDTO SalaryApproval([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _ads.SalaryApproval(dto);
        }
        [Route("SalaryApprovalgriddata")]
        public HR_Employee_SalaryDTO SalaryApprovalgriddata([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _ads.SalaryApprovalgriddata(dto);
        }
        [Route("report")]
        public HR_Employee_SalaryDTO report([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _ads.report(dto);
        }
    }
}

