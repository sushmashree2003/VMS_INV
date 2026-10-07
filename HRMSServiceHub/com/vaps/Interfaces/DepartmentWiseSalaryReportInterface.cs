using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface DepartmentWiseSalaryReportInterface
    {
        DepartmentWiseSalaryReportDTO getBasicData(DepartmentWiseSalaryReportDTO dto);
        DepartmentWiseSalaryReportDTO getdaily_data(DepartmentWiseSalaryReportDTO dto);

        DepartmentWiseSalaryReportDTO getsalaryalldetails(DepartmentWiseSalaryReportDTO dto);

        DepartmentWiseSalaryReportDTO get_depts(DepartmentWiseSalaryReportDTO dto);

        DepartmentWiseSalaryReportDTO Getreport(DepartmentWiseSalaryReportDTO dto);

    }
}
