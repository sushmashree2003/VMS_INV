using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrontOfficeHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeMonthlyReportFacade : Controller
    {
        public EmployeeMonthlyReportInterface _org;


        public EmployeeMonthlyReportFacade(EmployeeMonthlyReportInterface maspag)
        {
            _org = maspag;
        }
        [HttpPost]
        [Route("getalldetails")]
        public EmployeeMonthlyReportDTO Getdet([FromBody] EmployeeMonthlyReportDTO data)
        {
            return _org.getdata(data);
        }

        [HttpPost]
        [Route("get_departments")]
        public EmployeeMonthlyReportDTO get_departments([FromBody] EmployeeMonthlyReportDTO data)
        {
            return _org.get_departments(data);
        }

        [HttpPost]
        [Route("get_designation")]
        public EmployeeMonthlyReportDTO get_designation([FromBody] EmployeeMonthlyReportDTO data)
        {
            return _org.get_designation(data);
        }
        [HttpPost]
        [Route("get_employee")]
        public EmployeeMonthlyReportDTO get_employee([FromBody] EmployeeMonthlyReportDTO data)
        {
            return _org.get_employee(data);
        }

        [HttpPost]
        [Route("get_employeeselected")]
        public EmployeeMonthlyReportDTO get_employeeselected([FromBody] EmployeeMonthlyReportDTO data)
        {
            return _org.get_employeeselected(data);
        }        

        [Route("getrpt")]
        public async Task<EmployeeMonthlyReportDTO> getrpt([FromBody] EmployeeMonthlyReportDTO org)
        {
            return await _org.getreport(org);
        }

        [HttpPost]
        [Route("getalldetailsselected")]
        public EmployeeMonthlyReportDTO getalldetailsselected([FromBody] EmployeeMonthlyReportDTO data)
        {
            return _org.getalldetailsselected(data);
        }

        [HttpPost]
        [Route("get_departmentselected")]
        public EmployeeMonthlyReportDTO get_departmentselected([FromBody] EmployeeMonthlyReportDTO data)
        {
            return _org.get_departmentselected(data);
        }

        [HttpPost]
        [Route("get_designationselected")]
        public EmployeeMonthlyReportDTO get_designationselected([FromBody] EmployeeMonthlyReportDTO data)
        {
            return _org.get_designationselected(data);
        }
    }
}
