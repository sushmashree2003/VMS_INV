using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class EmpLoanPayoutDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmpLoanPayoutDTO, EmpLoanPayoutDTO> COMMM = new CommonDelegate<EmpLoanPayoutDTO, EmpLoanPayoutDTO>();

        public EmpLoanPayoutDTO onloadgetdetails(EmpLoanPayoutDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmpLoanPayoutFacade/onloadgetdetails");
        }

        public EmpLoanPayoutDTO savedetails(EmpLoanPayoutDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmpLoanPayoutFacade/");
        }
        public EmpLoanPayoutDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "EmpLoanPayoutFacade/getRecordById/");
        }
        public EmpLoanPayoutDTO employeechange(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "EmpLoanPayoutFacade/employeechange/");
        }
        public EmpLoanPayoutDTO deleterec(EmpLoanPayoutDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmpLoanPayoutFacade/deactivateRecordById/");
        }

    }
}
