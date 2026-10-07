using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Interfaces
{
    public interface FrontOfficeLogReportInterface
    {
        FrontOfficeLogReportDTO Savedata(FrontOfficeLogReportDTO data);
       
    }
}
