using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface FinancialYearWisePFVPFInterestInterface
    {
        FinancialYearWisePFVPFInterestDTO getalldetails(FinancialYearWisePFVPFInterestDTO data);

        FinancialYearWisePFVPFInterestDTO Savedetails(FinancialYearWisePFVPFInterestDTO data);
        FinancialYearWisePFVPFInterestDTO edit(FinancialYearWisePFVPFInterestDTO data);
        FinancialYearWisePFVPFInterestDTO deactive(FinancialYearWisePFVPFInterestDTO data);
    }
}