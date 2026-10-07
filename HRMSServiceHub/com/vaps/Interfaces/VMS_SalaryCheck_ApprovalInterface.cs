using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface VMS_SalaryCheck_ApprovalInterface
    {
        HR_Employee_Salary_PaymentDetailsDTO getBasicData(HR_Employee_Salary_PaymentDetailsDTO dto);
        Task<HR_Employee_Salary_PaymentDetailsDTO> getEmployeedetailsBySelection(HR_Employee_Salary_PaymentDetailsDTO dto);
        HR_Employee_Salary_PaymentDetailsDTO saveapproval(HR_Employee_Salary_PaymentDetailsDTO dto);
    }
}
