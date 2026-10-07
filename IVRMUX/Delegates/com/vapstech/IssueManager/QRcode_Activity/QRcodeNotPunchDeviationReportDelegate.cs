using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.QRcode_Activity
{
    public class QRcodeNotPunchDeviationReportDelegate
    {
        CommonDelegate<QRcodeNotPunchDeviationReportDTO, QRcodeNotPunchDeviationReportDTO> COMINV = new CommonDelegate<QRcodeNotPunchDeviationReportDTO, QRcodeNotPunchDeviationReportDTO>();

        public QRcodeNotPunchDeviationReportDTO getloaddata(QRcodeNotPunchDeviationReportDTO data)
        {
            return COMINV.POSTIssueManager(data, "QRcodeNotPunchDeviationReportFacade/getloaddata/");
        }

        public QRcodeNotPunchDeviationReportDTO get_Floors(QRcodeNotPunchDeviationReportDTO data)
        {
            return COMINV.POSTIssueManager(data, "QRcodeNotPunchDeviationReportFacade/get_Floors/");
        }
        public QRcodeNotPunchDeviationReportDTO get_Areas(QRcodeNotPunchDeviationReportDTO data)
        {
            return COMINV.POSTIssueManager(data, "QRcodeNotPunchDeviationReportFacade/get_Areas/");
        }
        public QRcodeNotPunchDeviationReportDTO getEmployeeName(QRcodeNotPunchDeviationReportDTO data)
        {
            return COMINV.POSTIssueManager(data, "QRcodeNotPunchDeviationReportFacade/getEmployeeName/");
        }

        public QRcodeNotPunchDeviationReportDTO QRcodedeviation(QRcodeNotPunchDeviationReportDTO data)
        {
            return COMINV.POSTIssueManager(data, "QRcodeNotPunchDeviationReportFacade/QRcodedeviation/");
        }

        public QRcodeNotPunchDeviationReportDTO SendMail(QRcodeNotPunchDeviationReportDTO data)
        {
            return COMINV.POSTIssueManager(data, "QRcodeNotPunchDeviationReportFacade/SendMail/");
        }


    }
}
