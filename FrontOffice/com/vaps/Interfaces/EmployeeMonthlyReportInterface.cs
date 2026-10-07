using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
    public interface EmployeeMonthlyReportInterface
    {
        EmployeeMonthlyReportDTO getdata(EmployeeMonthlyReportDTO data);
        EmployeeMonthlyReportDTO get_departments(EmployeeMonthlyReportDTO data);
        EmployeeMonthlyReportDTO get_designation(EmployeeMonthlyReportDTO data);
        EmployeeMonthlyReportDTO get_employee(EmployeeMonthlyReportDTO data);
        EmployeeMonthlyReportDTO get_employeeselected(EmployeeMonthlyReportDTO data);
        Task<EmployeeMonthlyReportDTO> getreport(EmployeeMonthlyReportDTO data);
        EmployeeMonthlyReportDTO getalldetailsselected(EmployeeMonthlyReportDTO data);
        EmployeeMonthlyReportDTO get_departmentselected(EmployeeMonthlyReportDTO data);
        EmployeeMonthlyReportDTO get_designationselected(EmployeeMonthlyReportDTO data);
    }
}
