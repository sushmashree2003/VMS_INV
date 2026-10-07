using CommonLibrary;
using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FMS
{
    public class FMSApprovalRequestPageDelegate
    {
        CommonDelegate<FMSApprovalRequestPageDTO, FMSApprovalRequestPageDTO> _comm = new CommonDelegate<FMSApprovalRequestPageDTO, FMSApprovalRequestPageDTO>();
        public FMSApprovalRequestPageDTO loaddata(FMSApprovalRequestPageDTO data)
        {
            return _comm.FMSGetPostData(data, "FMSApprovalRequestPageFacade/loaddata/");
        }

        public FMSApprovalRequestPageDTO savedetails(FMSApprovalRequestPageDTO data)
        {
            return _comm.FMSGetPostData(data, "FMSApprovalRequestPageFacade/savedetails/");
        }
        public FMSApprovalRequestPageDTO editdetails(FMSApprovalRequestPageDTO data)
        {
            return _comm.FMSGetPostData(data, "FMSApprovalRequestPageFacade/editdetails");
        }
        public FMSApprovalRequestPageDTO deactive(FMSApprovalRequestPageDTO data)
        {
            return _comm.FMSGetPostData(data, "FMSApprovalRequestPageFacade/deactive/");
        }

        public FMSApprovalRequestPageDTO viewuploadflies(FMSApprovalRequestPageDTO data)
        {
            return _comm.FMSGetPostData(data, "FMSApprovalRequestPageFacade/viewuploadflies/");
        }
        public FMSApprovalRequestPageDTO Updatedetails(FMSApprovalRequestPageDTO data)
        {
            return _comm.FMSGetPostData(data, "FMSApprovalRequestPageFacade/Updatedetails/");
        }
    }
}
