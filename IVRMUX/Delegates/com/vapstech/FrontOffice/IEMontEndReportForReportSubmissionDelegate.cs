using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FrontOffice
{
    public class IEMontEndReportForReportSubmissionDelegate
    {
        CommonDelegate<IEMontEndReportForReportSubmissionDTO, IEMontEndReportForReportSubmissionDTO> _com = new CommonDelegate<IEMontEndReportForReportSubmissionDTO, IEMontEndReportForReportSubmissionDTO>();

        public IEMontEndReportForReportSubmissionDTO Onload (IEMontEndReportForReportSubmissionDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMontEndReportForReportSubmissionFacade/Onload");
        }

        public IEMontEndReportForReportSubmissionDTO saverecord(IEMontEndReportForReportSubmissionDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMontEndReportForReportSubmissionFacade/saverecord");
        }
        public IEMontEndReportForReportSubmissionDTO DocView(IEMontEndReportForReportSubmissionDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMontEndReportForReportSubmissionFacade/DocView");
        }
        public IEMontEndReportForReportSubmissionDTO deactiveY(IEMontEndReportForReportSubmissionDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMontEndReportForReportSubmissionFacade/deactiveY");
        }

        public IEMontEndReportForReportSubmissionDTO EditData(IEMontEndReportForReportSubmissionDTO data)
        {
            return _com.POSTDataHolidayReport(data, "IEMontEndReportForReportSubmissionFacade/EditData");
        }


    }
}
