using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Interfaces
{
    public interface AttendanceApprovalReportInterface
    {
        AttendanceApprovalReportDTO getdata(AttendanceApprovalReportDTO data);

       AttendanceApprovalReportDTO report(AttendanceApprovalReportDTO data);
    }
}
