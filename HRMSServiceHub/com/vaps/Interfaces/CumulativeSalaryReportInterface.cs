using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface CumulativeSalaryReportInterface
    {
        HR_Employee_SalaryDTO getBasicData(HR_Employee_SalaryDTO dto);
        Task<HR_Employee_SalaryDTO> getEmployeedetailsBySelection(HR_Employee_SalaryDTO dto);

        HR_Employee_SalaryDTO get_depts(HR_Employee_SalaryDTO dto);
        HR_Employee_SalaryDTO get_desig(HR_Employee_SalaryDTO dto);
        HR_Employee_SalaryDTO getapprvaldata(HR_Employee_SalaryDTO dto);
        HR_Employee_SalaryDTO SalaryApproval(HR_Employee_SalaryDTO dto);
        HR_Employee_SalaryDTO SalaryApprovalgriddata(HR_Employee_SalaryDTO dto);
        HR_Employee_SalaryDTO report(HR_Employee_SalaryDTO dto);
    }
}
