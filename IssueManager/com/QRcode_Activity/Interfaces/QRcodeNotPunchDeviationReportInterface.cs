using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.QRcode_Activity.Interfaces
{
  public  interface QRcodeNotPunchDeviationReportInterface
    {
        QRcodeNotPunchDeviationReportDTO getloaddata(QRcodeNotPunchDeviationReportDTO data);
        QRcodeNotPunchDeviationReportDTO get_Floors(QRcodeNotPunchDeviationReportDTO data);
        QRcodeNotPunchDeviationReportDTO get_Areas(QRcodeNotPunchDeviationReportDTO data);
        QRcodeNotPunchDeviationReportDTO getEmployeeName(QRcodeNotPunchDeviationReportDTO data);
        
        QRcodeNotPunchDeviationReportDTO QRcodedeviation(QRcodeNotPunchDeviationReportDTO data);
        QRcodeNotPunchDeviationReportDTO SendMail(QRcodeNotPunchDeviationReportDTO data);
    }
}
