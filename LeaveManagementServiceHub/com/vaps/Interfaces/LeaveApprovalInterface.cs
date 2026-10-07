using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface LeaveApprovalInterface
    {
        Task<LeaveCreditDTO> getApprovalStatus(LeaveCreditDTO data);
        Task<LeaveCreditDTO.LeaveApprovalDTO> get_status(LeaveCreditDTO.LeaveApprovalDTO data);
        Task<LeaveCreditDTO.LeaveApprovalDTO> reject_status(LeaveCreditDTO.LeaveApprovalDTO data);
        LeaveCreditDTO viewcomment(LeaveCreditDTO data);
        LeaveCreditDTO getRequestStatus(LeaveCreditDTO data);
        Task<LeaveCreditDTO> get_approvestatusAsync(LeaveCreditDTO data);
        Task<LeaveCreditDTO> get_rejectedstatusAsync(LeaveCreditDTO data);
        LeaveCreditDTO ViewRemarksLevelWise(LeaveCreditDTO data);
        LeaveCreditDTO Viewleavebalancehistory(LeaveCreditDTO data);
        LeaveCreditDTO LeaveCredition(LeaveCreditDTO data);
        
            LeaveCreditDTO ViewOtherDetails(LeaveCreditDTO data);
    }
}
