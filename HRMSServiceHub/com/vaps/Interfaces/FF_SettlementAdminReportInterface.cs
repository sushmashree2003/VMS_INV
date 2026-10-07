using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
   public  interface FF_SettlementAdminReportInterface
   {
        AdvanceReportDTO getBasicData(AdvanceReportDTO dto);
        Task<AdvanceReportDTO> getEmployeedetailsBySelection(AdvanceReportDTO dto);

        AdvanceReportDTO get_depts(AdvanceReportDTO dto);
        AdvanceReportDTO get_desig(AdvanceReportDTO dto);
   }
}



