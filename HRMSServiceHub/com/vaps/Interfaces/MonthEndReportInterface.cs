using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MonthEndReportInterface
    {
        MonthEndReportDTO getBasicData(MonthEndReportDTO dto);
        Task<MonthEndReportDTO> getEmployeedetailsBySelection(MonthEndReportDTO dto);
        MonthEndReportDTO getalldetailsQForm(MonthEndReportDTO dto);
        MonthEndReportDTO getEmployeedetailsQForm(MonthEndReportDTO dto);
        MonthEndReportDTO SearchUdetails(MonthEndReportDTO dto);
        MonthEndReportDTO SearchFReport(MonthEndReportDTO dto);
    }
}
