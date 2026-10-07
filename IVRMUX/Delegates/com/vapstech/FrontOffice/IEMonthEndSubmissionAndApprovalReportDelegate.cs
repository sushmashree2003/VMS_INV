using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FrontOffice
{
    public class IEMonthEndSubmissionAndApprovalReportDelegate
    {
        CommonDelegate<IEMonthEndSubmissionAndApprovalReportDTO, IEMonthEndSubmissionAndApprovalReportDTO> _com = new CommonDelegate<IEMonthEndSubmissionAndApprovalReportDTO, IEMonthEndSubmissionAndApprovalReportDTO>();

        public IEMonthEndSubmissionAndApprovalReportDTO Onload(IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMonthEndSubmissionAndApprovalReportFacade/Onload");
        }

        public IEMonthEndSubmissionAndApprovalReportDTO getmonthreport(IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMonthEndSubmissionAndApprovalReportFacade/getmonthreport");
        }
        public IEMonthEndSubmissionAndApprovalReportDTO DocView(IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMonthEndSubmissionAndApprovalReportFacade/DocView");
        }
        public IEMonthEndSubmissionAndApprovalReportDTO getIEDetails(IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMonthEndSubmissionAndApprovalReportFacade/getIEDetails");
        }

    }
}

