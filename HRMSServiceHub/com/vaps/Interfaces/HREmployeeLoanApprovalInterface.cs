using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface HREmployeeLoanApprovalInterface
    {
        HREmployeeLoanApprovalDTO getBasicData(HREmployeeLoanApprovalDTO dto);


        HREmployeeLoanApprovalDTO getapprvaldata(HREmployeeLoanApprovalDTO dto);
        HREmployeeLoanApprovalDTO SalaryApproval(HREmployeeLoanApprovalDTO dto);
        HREmployeeLoanApprovalDTO SalaryApprovalgriddata(HREmployeeLoanApprovalDTO dto);
        HREmployeeLoanApprovalDTO report(HREmployeeLoanApprovalDTO dto);
        HREmployeeLoanApprovalDTO ViewFiles(HREmployeeLoanApprovalDTO dto);
        HREmployeeLoanApprovalDTO OBCB_Email(HREmployeeLoanApprovalDTO dto);
        HREmployeeLoanApprovalDTO BondCompTaskCreate(HREmployeeLoanApprovalDTO data);
    }
}
