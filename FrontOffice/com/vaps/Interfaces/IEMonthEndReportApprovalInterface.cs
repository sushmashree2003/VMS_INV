using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Interfaces
{
    public interface IEMonthEndReportApprovalInterface
    {
        IEMontEndReportForReportSubmissionDTO Onload(IEMontEndReportForReportSubmissionDTO data);
        IEMontEndReportForReportSubmissionDTO saverecord(IEMontEndReportForReportSubmissionDTO data);
        IEMontEndReportForReportSubmissionDTO DocView(IEMontEndReportForReportSubmissionDTO data);
        IEMontEndReportForReportSubmissionDTO Approve_status(IEMontEndReportForReportSubmissionDTO data);

    }
}
