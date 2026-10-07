using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface VMS_ReportInterface
    {
        VMS_ReportDTO onloadgetdetails(VMS_ReportDTO dto);
        VMS_ReportDTO report(VMS_ReportDTO dto);
        VMS_ReportDTO get_Employee(VMS_ReportDTO dto);
    }
}
