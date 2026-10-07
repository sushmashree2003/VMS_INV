using CommonLibrary;
using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FMS
{
    public class FMSApprovalReportDelegate
    {
        CommonDelegate<FMSApprovalReportDTO, FMSApprovalReportDTO> _comm = new CommonDelegate<FMSApprovalReportDTO, FMSApprovalReportDTO>();
        public FMSApprovalReportDTO loaddata(FMSApprovalReportDTO data)
        {
            return _comm.FMSGetPostData(data, "FMSApprovalReportFacade/loaddata/");
        }

        public FMSApprovalReportDTO report(FMSApprovalReportDTO data)
        {
            return _comm.FMSGetPostData(data, "FMSApprovalReportFacade/report/");
        }
    }
}
