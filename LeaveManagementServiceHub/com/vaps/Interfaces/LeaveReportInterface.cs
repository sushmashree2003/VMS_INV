using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface LeaveReportInterface
    {
        LeaveCreditDTO getleavereport(LeaveCreditDTO data);
        LeaveCreditDTO get_stafftypes(LeaveCreditDTO data);
        LeaveCreditDTO get_departments(LeaveCreditDTO data);
        LeaveCreditDTO get_designation(LeaveCreditDTO data);
        LeaveCreditDTO get_Employees(LeaveCreditDTO data);
        Task<LeaveCreditDTO> get_report(LeaveCreditDTO data);

        //Interface for AuthourizationReport
        Task<LeaveCreditDTO> get_authorization_report(LeaveCreditDTO data);


    }
}
