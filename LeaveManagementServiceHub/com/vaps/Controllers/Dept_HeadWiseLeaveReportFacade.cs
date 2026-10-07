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
    public class Dept_HeadWiseLeaveReportFacade : Controller
    {
        public Dept_HeadWiseLeaveReportInterface _leave;
        public Dept_HeadWiseLeaveReportFacade(Dept_HeadWiseLeaveReportInterface _leavec)
        {
            _leave = _leavec;
        }

        [Route("getdatedetails")]
        public Dept_HeadWiseLeaveReportDTO getdatedetails([FromBody] Dept_HeadWiseLeaveReportDTO data)
        {
            return _leave.getdatedetails(data);
        }
        
    }
}
