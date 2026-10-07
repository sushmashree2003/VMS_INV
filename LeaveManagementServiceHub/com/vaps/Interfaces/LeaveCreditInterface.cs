using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface LeaveCreditInterface
    {
        LeaveCreditDTO getleave(LeaveCreditDTO data);
        LeaveCreditDTO get_departments(LeaveCreditDTO data);
        LeaveCreditDTO get_designation(LeaveCreditDTO data);
        LeaveCreditDTO get_leavecode(LeaveCreditDTO data);
        LeaveCreditDTO SaveData(LeaveCreditDTO data);
        LeaveCreditDTO EditData(LeaveCreditDTO data);
        LeaveCreditDTO get_grade(LeaveCreditDTO data);

        //Manual Leave Credit
        LeaveCreditDTO OnLoadManualLeaveCredit(LeaveCreditDTO data);
        LeaveCreditDTO SearchManualLeaveCreditDetails(LeaveCreditDTO data);
        LeaveCreditDTO SaveManualLeaveCreditDetails(LeaveCreditDTO data);
        LeaveCreditDTO viewmonthwiseleave(LeaveCreditDTO data);
        //Auto Leave Cretid API (Schedular)
        LeaveCreditDTO.CreditLeaveDTO AutoCompOffCredit(LeaveCreditDTO.CreditLeaveDTO data);
        LeaveCreditDTO.CreditLeaveDTO AutoLeaveCredit(LeaveCreditDTO.CreditLeaveDTO data);
        LeaveCreditDTO LeavePendingMailTrigger(LeaveCreditDTO data);
        //END
    }
}
