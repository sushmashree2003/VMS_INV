using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface OnlineLeaveApplicationInterface
    {
        LeaveCreditDTO getonlineLeave(LeaveCreditDTO data);       
        LeaveCreditDTO saveonlineLeave(LeaveCreditDTO data);
        LeaveCreditDTO saveadminLeave(LeaveCreditDTO data);        
        LeaveCreditDTO getonlineLeavestatus(LeaveCreditDTO data);
        LeaveCreditDTO getemployeeadmin(LeaveCreditDTO data);
        LeaveCreditDTO getSingleEmpLeavestatus(LeaveCreditDTO data);
        Task<LeaveCreditDTO> getempleavehistory(LeaveCreditDTO data);
        LeaveCreditDTO requestleave(LeaveCreditDTO data);
        LeaveCreditDTO viewcomment(LeaveCreditDTO data);
        LeaveCreditDTO CheckAppliedDate(LeaveCreditDTO data);
        LeaveCreditDTO CheckApprovalAuthorizationMapped(LeaveCreditDTO data);
        bool LeaveCredition();
        
        LeaveCreditDTO Leaveapprover(LeaveCreditDTO data);
        LeaveCreditDTO proxytaskdetails(LeaveCreditDTO data);

        //------Leave_Restriction------//

        LeaveCreditDTO LRdetails(LeaveCreditDTO data);
        LeaveCreditDTO getdepartment(LeaveCreditDTO data);
        LeaveCreditDTO LRSaveUpdate(LeaveCreditDTO data);
        LeaveCreditDTO LREdit(LeaveCreditDTO data);
        LeaveCreditDTO ActiveDeactiveRecord(LeaveCreditDTO data);

        LeaveCreditDTO leveConditions(LeaveCreditDTO data);
    }
}
