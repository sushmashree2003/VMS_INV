using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;
using HRMSServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class DepartmentWiseSalaryReportFacadeController : Controller
    {
        public DepartmentWiseSalaryReportInterface _ads;

        public DepartmentWiseSalaryReportFacadeController(DepartmentWiseSalaryReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("getdata")]
        public DepartmentWiseSalaryReportDTO getinitialdata([FromBody]DepartmentWiseSalaryReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public DepartmentWiseSalaryReportDTO getdaily_data([FromBody]DepartmentWiseSalaryReportDTO dto)
        {
            return _ads.getdaily_data(dto);
        }




        //setemporder
        [Route("getsalaryalldetails")]
        public DepartmentWiseSalaryReportDTO getsalaryalldetails([FromBody]DepartmentWiseSalaryReportDTO dto)
        {
            return _ads.getsalaryalldetails(dto);
        }

        [Route("get_depts")]
        public DepartmentWiseSalaryReportDTO get_depts([FromBody]DepartmentWiseSalaryReportDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("Getreport")]
        public DepartmentWiseSalaryReportDTO Getreport([FromBody]DepartmentWiseSalaryReportDTO dto)
        {
            return _ads.Getreport(dto);
        }


    }
}
