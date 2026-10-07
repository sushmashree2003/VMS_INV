using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface employeeadvanceapplyInterface
    {
        HR_Emp_SalaryAdvanceDTO getBasicData(HR_Emp_SalaryAdvanceDTO dto);
        HR_Emp_SalaryAdvanceDTO SaveUpdate(HR_Emp_SalaryAdvanceDTO dto);
        HR_Emp_SalaryAdvanceDTO Edit(HR_Emp_SalaryAdvanceDTO dto);
        HR_Emp_SalaryAdvanceDTO deactivate(HR_Emp_SalaryAdvanceDTO dto);
        HR_Emp_SalaryAdvanceDTO getDetailsByEmployee(HR_Emp_SalaryAdvanceDTO dto);

    }
}
