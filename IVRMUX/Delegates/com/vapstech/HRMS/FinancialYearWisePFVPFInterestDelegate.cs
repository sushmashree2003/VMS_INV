using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class FinancialYearWisePFVPFInterestDelegate
    {
        CommonDelegate<FinancialYearWisePFVPFInterestDTO, FinancialYearWisePFVPFInterestDTO> COMMM = new CommonDelegate<FinancialYearWisePFVPFInterestDTO, FinancialYearWisePFVPFInterestDTO>();
        public FinancialYearWisePFVPFInterestDTO getalldetails(FinancialYearWisePFVPFInterestDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "FinancialYearWisePFVPFInterestFacade/getdetails/");
        }
        public FinancialYearWisePFVPFInterestDTO Savedetails(FinancialYearWisePFVPFInterestDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "FinancialYearWisePFVPFInterestFacade/Savedetails/");
        }
        public FinancialYearWisePFVPFInterestDTO edit(FinancialYearWisePFVPFInterestDTO data)
        {
            return COMMM.POSTDataHRMS(data, "FinancialYearWisePFVPFInterestFacade/edit/");
        }
        public FinancialYearWisePFVPFInterestDTO deactive(FinancialYearWisePFVPFInterestDTO data)
        {
            return COMMM.POSTDataHRMS(data, "FinancialYearWisePFVPFInterestFacade/deactive/");
        }

    }
}
