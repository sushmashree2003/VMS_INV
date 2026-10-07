using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
     public  interface LeaveOveallReportInerface
    {

        LeaveOverallReportDTO getleavereport(LeaveOverallReportDTO data);

        LeaveOverallReportDTO get_report(LeaveOverallReportDTO data);

        LeaveOverallReportDTO getleavetype(LeaveOverallReportDTO data);

        ////////MONTHENDREPORT////////////////////////////////////////////////////////////////////////////////

        LeaveOverallReportDTO getmonthendreport(LeaveOverallReportDTO data);
    }
}
