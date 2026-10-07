using CommonLibrary;
using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FMS
{
    public class FMSApprovalDelegate
    {
        CommonDelegate<FMSApprovalDTO, FMSApprovalDTO> _comm = new CommonDelegate<FMSApprovalDTO, FMSApprovalDTO>();
        public FMSApprovalDTO loaddata(FMSApprovalDTO data)
        {
            return _comm.FMSGetPostData(data, "FMSApprovalRequestPageFacade/loaddata/");
        }

        public FMSApprovalDTO getdatas(FMSApprovalDTO data)
        {
            return _comm.FMSGetPostData(data, "FMSApprovalFacade/getdatas/");
        }

        public FMSApprovalDTO savedetails(FMSApprovalDTO data)
        {
            return _comm.FMSGetPostData(data, "FMSApprovalFacade/savedetails/");
        }
    }
}
