using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.LeaveManagement
{
    public class LeaveApprovalDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<LeaveCreditDTO, LeaveCreditDTO> COMMM = new CommonDelegate<LeaveCreditDTO, LeaveCreditDTO>();

        
        private readonly object resource;
        private readonly string serviceBaseUrl;
       
        CommonDelegate<LeaveCreditDTO, LeaveCreditDTO> COMFRNT = new CommonDelegate<LeaveCreditDTO, LeaveCreditDTO>();
        CommonDelegate<LeaveCreditDTO.LeaveApprovalDTO, LeaveCreditDTO.LeaveApprovalDTO> COMFRNT1 = new CommonDelegate<LeaveCreditDTO.LeaveApprovalDTO, LeaveCreditDTO.LeaveApprovalDTO>();
       
        public LeaveCreditDTO getApprovalStatus(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/getApprovalStatus/");
        }
        public LeaveCreditDTO.LeaveApprovalDTO get_status(LeaveCreditDTO.LeaveApprovalDTO data)
        {
            return COMFRNT1.POSTDataOnlineLeave(data, "LeaveApprovalFacade/get_status/");
        }
        public LeaveCreditDTO.LeaveApprovalDTO reject_status(LeaveCreditDTO.LeaveApprovalDTO data)
        {
            return COMFRNT1.POSTDataOnlineLeave(data, "LeaveApprovalFacade/reject_status/");
        }        
        public LeaveCreditDTO viewcomment(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/viewcomment/");
        }
        public LeaveCreditDTO getRequestStatus(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/getRequestStatus/");
        }
        public LeaveCreditDTO get_approvestatus(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/get_approvestatus/");
        }
        public LeaveCreditDTO get_rejectedstatus(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/get_rejectedstatus/");
        }
        public LeaveCreditDTO ViewRemarksLevelWise(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/ViewRemarksLevelWise/");
        }
        
        public LeaveCreditDTO Viewleavebalancehistory(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/Viewleavebalancehistory/");
        }
        public LeaveCreditDTO ViewOtherDetails(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/ViewOtherDetails/");
        }

    }
}
