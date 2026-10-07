using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.QRcode_Activity.Interfaces
{
  public  interface QRcodeDeviationReportInterface
    {
        QRcodeDeviationReportDTO getloaddata(QRcodeDeviationReportDTO data);
        QRcodeDeviationReportDTO getEmployeeName(QRcodeDeviationReportDTO data);


        QRcodeDeviationReportDTO QRcodedeviation(QRcodeDeviationReportDTO data);
    }
}
