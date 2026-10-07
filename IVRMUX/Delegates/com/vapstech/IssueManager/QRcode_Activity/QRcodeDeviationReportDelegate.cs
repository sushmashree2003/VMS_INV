using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.QRcode_Activity
{
    public class QRcodeDeviationReportDelegate
    {
        CommonDelegate<QRcodeDeviationReportDTO, QRcodeDeviationReportDTO> COMINV = new CommonDelegate<QRcodeDeviationReportDTO, QRcodeDeviationReportDTO>();

        public QRcodeDeviationReportDTO getloaddata(QRcodeDeviationReportDTO data)
        {
            return COMINV.POSTIssueManager(data, "QRcodeDeviationReportFacade/getloaddata/");
        }
        public QRcodeDeviationReportDTO getEmployeeName(QRcodeDeviationReportDTO data)
        {
            return COMINV.POSTIssueManager(data, "QRcodeDeviationReportFacade/getEmployeeName/");
        }

        public QRcodeDeviationReportDTO QRcodedeviation(QRcodeDeviationReportDTO data)
        {
            return COMINV.POSTIssueManager(data, "QRcodeDeviationReportFacade/QRcodedeviation/");
        }

      

    }
}
