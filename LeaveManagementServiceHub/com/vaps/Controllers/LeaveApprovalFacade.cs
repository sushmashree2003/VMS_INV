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
    public class LeaveApprovalFacade : Controller
    {
        public LeaveApprovalInterface _leave;
        public LeaveApprovalFacade(LeaveApprovalInterface _leavec)
        {
            _leave = _leavec;
        }
        [HttpPost]
        [Route("getApprovalStatus")]
        public Task<LeaveCreditDTO> getApprovalStatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.getApprovalStatus(data);
        }
        [Route("get_status")]
        public Task<LeaveCreditDTO.LeaveApprovalDTO> get_status([FromBody] LeaveCreditDTO.LeaveApprovalDTO data)
        {
            return _leave.get_status(data);
        }
        [Route("reject_status")]
        public Task<LeaveCreditDTO.LeaveApprovalDTO> reject_status([FromBody] LeaveCreditDTO.LeaveApprovalDTO data)
        {
            return _leave.reject_status(data);
        }
        [Route("viewcomment")]
        public LeaveCreditDTO viewcomment([FromBody] LeaveCreditDTO data)
        {
            return _leave.viewcomment(data);
        }

        [Route("getRequestStatus")]
        public LeaveCreditDTO getRequestStatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.getRequestStatus(data);
        }

        [Route("get_approvestatus")]
        public Task<LeaveCreditDTO> get_approvestatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_approvestatusAsync(data);
        }

        [Route("get_rejectedstatus")]
        public Task<LeaveCreditDTO> get_rejectedstatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_rejectedstatusAsync(data);
        }

        [Route("ViewRemarksLevelWise")]
        public LeaveCreditDTO ViewRemarksLevelWise([FromBody] LeaveCreditDTO data)
        {
            return _leave.ViewRemarksLevelWise(data);
        }

        [Route("Viewleavebalancehistory")]
        public LeaveCreditDTO Viewleavebalancehistory([FromBody] LeaveCreditDTO data)
        {
            return _leave.Viewleavebalancehistory(data);
        }
        
        [Route("LeaveCredition")]
        public LeaveCreditDTO LeaveCredition([FromBody] LeaveCreditDTO data)
        {
            return _leave.LeaveCredition(data);
        }

        [Route("ViewOtherDetails")]
        public LeaveCreditDTO ViewOtherDetails([FromBody] LeaveCreditDTO data)
        {
            return _leave.ViewOtherDetails(data);
        }
    }
}
