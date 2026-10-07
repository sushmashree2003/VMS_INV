using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface ActiveDeactiveEmployeeReportInterface

    {
        ActiveDeactiveEmployeeReportDTO getalldetails(ActiveDeactiveEmployeeReportDTO dto);

        ActiveDeactiveEmployeeReportDTO report(ActiveDeactiveEmployeeReportDTO dto);
    }


}
