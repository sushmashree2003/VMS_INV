using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Interface
{
    public interface FMSApprovalReportInterface
    {
        FMSApprovalReportDTO loaddata(FMSApprovalReportDTO data);
        FMSApprovalReportDTO report(FMSApprovalReportDTO data);
        
    }
}
