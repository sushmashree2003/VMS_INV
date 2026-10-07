using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class LeaveReportFacade : Controller
    {
        public LeaveReportInterface _leave;
        public LeaveReportFacade(LeaveReportInterface _leavec)
        {
            _leave = _leavec;
        }
        [HttpPost]
        [Route("getleavereport")]
        public LeaveCreditDTO getleavereport([FromBody] LeaveCreditDTO data)
        {
            return _leave.getleavereport(data);
        }

        [Route("get_stafftypes")]
        public LeaveCreditDTO get_stafftypes([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_stafftypes(data);
        }

        [Route("get_departments")]
        public LeaveCreditDTO get_departments([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_departments(data);
        }
       
        [Route("get_designation")]
        public LeaveCreditDTO get_designation([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_designation(data);
        }
        [Route("get_Employees")]
        public LeaveCreditDTO get_Employees([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_Employees(data);
        }
        [Route("get_report")]
        public Task<LeaveCreditDTO> get_report([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_report(data);
        }

        //To fetch details of Leave AuthorizationReport
        [Route("get_authorization_report")]
        public Task<LeaveCreditDTO> get_authorization_report([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_authorization_report(data);
        }
    }
}
