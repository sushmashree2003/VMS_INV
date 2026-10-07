using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Interfaces
{
    public interface IEMonthEndSubmissionAndApprovalReportInterface
    {
        IEMonthEndSubmissionAndApprovalReportDTO Onload(IEMonthEndSubmissionAndApprovalReportDTO data);
        IEMonthEndSubmissionAndApprovalReportDTO getmonthreport(IEMonthEndSubmissionAndApprovalReportDTO data);
        IEMonthEndSubmissionAndApprovalReportDTO DocView(IEMonthEndSubmissionAndApprovalReportDTO data);
        IEMonthEndSubmissionAndApprovalReportDTO getIEDetails(IEMonthEndSubmissionAndApprovalReportDTO data);
    }
}
