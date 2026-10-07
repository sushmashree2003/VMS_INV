using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.TADA
{
    public class TADA_PaymentReportDelegete
    {
        CommonDelegate<TADA_PaymentReportDTO, TADA_PaymentReportDTO> COMMI = new CommonDelegate<TADA_PaymentReportDTO, TADA_PaymentReportDTO>();
        public TADA_PaymentReportDTO getalldata(TADA_PaymentReportDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_PaymentReportFacade/getalldata/");
        }
        //savedta
        public TADA_PaymentReportDTO GetReport(TADA_PaymentReportDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_PaymentReportFacade/GetReport/");
        }
    }
}
