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
    public class OnlineLeaveApplicationFacade : Controller
    {
        public OnlineLeaveApplicationInterface _leave;
        public OnlineLeaveApplicationFacade(OnlineLeaveApplicationInterface _leavec)
        {
            _leave = _leavec;
        }

        [Route("getonlineLeave")]
        public LeaveCreditDTO getonlineLeave([FromBody] LeaveCreditDTO data)
        {
            return _leave.getonlineLeave(data);
        }

        [Route("save")]
        public LeaveCreditDTO saveLeave([FromBody] LeaveCreditDTO data)
        {
            return _leave.saveonlineLeave(data);
        }

        [Route("getonlineLeavestatus")]
        public LeaveCreditDTO getonlineLeavestatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.getonlineLeavestatus(data);
        }

        [Route("saveadminLeave")]
        public LeaveCreditDTO saveadminLeave([FromBody] LeaveCreditDTO data)
        {
            return _leave.saveadminLeave(data);
        }

        [Route("getemployeeadmin")]
        public LeaveCreditDTO getemployeeadmin([FromBody] LeaveCreditDTO data)
        {
            return _leave.getemployeeadmin(data);
        }

        [Route("getSingleEmpLeavestatus")]
        public LeaveCreditDTO getSingleEmpLeavestatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.getSingleEmpLeavestatus(data);
        }

        [Route("getempleavehistory")]
        public Task<LeaveCreditDTO> getempleavehistory([FromBody] LeaveCreditDTO data)
        {
            return _leave.getempleavehistory(data);
        }

        [Route("requestleave")]
        public LeaveCreditDTO requestleave([FromBody] LeaveCreditDTO data)
        {
            return _leave.requestleave(data);
        }

        [Route("viewcomment")]
        public LeaveCreditDTO viewcomment([FromBody] LeaveCreditDTO data)
        {
            return _leave.viewcomment(data);
        }

        [Route("CheckAppliedDate")]
        public LeaveCreditDTO CheckAppliedDate([FromBody] LeaveCreditDTO data)
        {
            return _leave.CheckAppliedDate(data);
        }

        [Route("CheckApprovalAuthorizationMapped")]
        public LeaveCreditDTO CheckApprovalAuthorizationMapped([FromBody] LeaveCreditDTO data)
        {
            return _leave.CheckApprovalAuthorizationMapped(data);
        }

        [Route("LeaveCredition")]
        [HttpGet]
        public bool LeaveCredition()
        {
            return _leave.LeaveCredition();

        }

        [Route("Leaveapprover")]
        public LeaveCreditDTO approver([FromBody] LeaveCreditDTO data)
        {
            return _leave.Leaveapprover(data);
        }
        [Route("proxytaskdetails")]
        public LeaveCreditDTO proxytaskdetails([FromBody] LeaveCreditDTO data)
        {
            return _leave.proxytaskdetails(data);
        }

        //---------------------------------------------------------------LRSaveUpdate-------////

        [Route("getdepartment")]
        public LeaveCreditDTO getdepartment([FromBody] LeaveCreditDTO data)
        {
            return _leave.getdepartment(data);
        }
        [Route("LRdetails")]
        public LeaveCreditDTO LRdetails([FromBody] LeaveCreditDTO data)
        {
            return _leave.LRdetails(data);
        }
        [Route("LRSaveUpdate")]
        public LeaveCreditDTO LRSaveUpdate([FromBody] LeaveCreditDTO data)
        {
            return _leave.LRSaveUpdate(data);
        }
        [Route("LREdit")]
        public LeaveCreditDTO LREdit([FromBody] LeaveCreditDTO data)
        {
            return _leave.LREdit(data);
        }
        [Route("ActiveDeactiveRecord")]
        public LeaveCreditDTO ActiveDeactiveRecord([FromBody] LeaveCreditDTO data)
        {
            return _leave.ActiveDeactiveRecord(data);
        }
        [Route("leveConditions")]
        public LeaveCreditDTO leveConditions([FromBody] LeaveCreditDTO data)
        {
            return _leave.leveConditions(data);
        }

    }

}
