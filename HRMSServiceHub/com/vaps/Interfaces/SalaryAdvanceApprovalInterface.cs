using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface SalaryAdvanceApprovalInterface
    {
        HR_Emp_SalaryAdvanceDTO getBasicData(HR_Emp_SalaryAdvanceDTO dto);
        HR_Emp_SalaryAdvanceDTO empdetails(HR_Emp_SalaryAdvanceDTO dto);


        HR_Emp_SalaryAdvanceDTO approvedrecord(HR_Emp_SalaryAdvanceDTO dto);

        //------------------Advance Approval Report------------------------------
        HR_Emp_SalaryAdvanceDTO Onload(HR_Emp_SalaryAdvanceDTO dto);
        HR_Emp_SalaryAdvanceDTO getapprovalreport(HR_Emp_SalaryAdvanceDTO dto);
    }
}
