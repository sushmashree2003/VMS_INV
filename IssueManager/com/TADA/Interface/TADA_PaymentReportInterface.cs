using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
   public interface TADA_PaymentReportInterface
    {
        TADA_PaymentReportDTO getalldata(TADA_PaymentReportDTO data);
        TADA_PaymentReportDTO GetReport(TADA_PaymentReportDTO data);
       
       
    }
}
