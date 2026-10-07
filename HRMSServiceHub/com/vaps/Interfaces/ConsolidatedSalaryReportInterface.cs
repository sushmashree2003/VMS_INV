using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface ConsolidatedSalaryReportInterface
    {
        ConsolidatedSalaryReportDTO getBasicData(ConsolidatedSalaryReportDTO dto);
        //FilterEmployeeData
        ConsolidatedSalaryReportDTO salaryreport(ConsolidatedSalaryReportDTO dto);

    }
}
