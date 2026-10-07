using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
   public interface ApprovalInOutStatusInterface
    {
        ApprovalInOutStatus_DTO getDetails(ApprovalInOutStatus_DTO data);
        ApprovalInOutStatus_DTO EditDetails(ApprovalInOutStatus_DTO id);
        ApprovalInOutStatus_DTO saveData(ApprovalInOutStatus_DTO data);
        ApprovalInOutStatus_DTO change_radiobutton(ApprovalInOutStatus_DTO data);

    }
}
