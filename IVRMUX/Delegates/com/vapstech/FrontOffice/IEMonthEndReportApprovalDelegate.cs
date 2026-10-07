using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FrontOffice
{
    public class IEMonthEndReportApprovalDelegate
    {
        CommonDelegate<IEMontEndReportForReportSubmissionDTO, IEMontEndReportForReportSubmissionDTO> _com = new CommonDelegate<IEMontEndReportForReportSubmissionDTO, IEMontEndReportForReportSubmissionDTO>();

        public IEMontEndReportForReportSubmissionDTO Onload(IEMontEndReportForReportSubmissionDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMonthEndReportApprovalFacade/Onload");
        }

        public IEMontEndReportForReportSubmissionDTO saverecord(IEMontEndReportForReportSubmissionDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMonthEndReportApprovalFacade/saverecord");
        }
        public IEMontEndReportForReportSubmissionDTO DocView(IEMontEndReportForReportSubmissionDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMonthEndReportApprovalFacade/DocView");
        }
        public IEMontEndReportForReportSubmissionDTO Approve_status(IEMontEndReportForReportSubmissionDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMonthEndReportApprovalFacade/Approve_status");
        }


    }
}

