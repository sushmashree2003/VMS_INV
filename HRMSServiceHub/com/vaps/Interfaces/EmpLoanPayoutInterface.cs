using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface EmpLoanPayoutInterface
    {
        EmpLoanPayoutDTO getBasicData(EmpLoanPayoutDTO dto);
        EmpLoanPayoutDTO SaveUpdate(EmpLoanPayoutDTO dto);
        EmpLoanPayoutDTO editData(int id);
        EmpLoanPayoutDTO employeechange(int id);

        EmpLoanPayoutDTO deactivate(EmpLoanPayoutDTO dto);
    }
}
