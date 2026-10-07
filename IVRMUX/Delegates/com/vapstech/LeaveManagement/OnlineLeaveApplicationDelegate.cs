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
    public class OnlineLeaveApplicationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<LeaveCreditDTO, LeaveCreditDTO> COMMM = new CommonDelegate<LeaveCreditDTO, LeaveCreditDTO>();


        public LeaveCreditDTO savedata(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/save/");
        }
        
        public LeaveCreditDTO saveadminLeave(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/saveadminLeave/");
        }

        public LeaveCreditDTO getonlineLeave(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/getonlineLeave/");
        }
        public LeaveCreditDTO getonlineLeavestatus(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/getonlineLeavestatus/");
        }

        public LeaveCreditDTO getemployeeadmin(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/getemployeeadmin/");
        }

        public LeaveCreditDTO getSingleEmpLeavestatus(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/getSingleEmpLeavestatus/");
        }

        public LeaveCreditDTO requestleave(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/requestleave/");
        }

        public LeaveCreditDTO viewcomment(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/viewcomment/");
        }

        public LeaveCreditDTO CheckAppliedDate(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/CheckAppliedDate/");
        }

        public LeaveCreditDTO CheckApprovalAuthorizationMapped(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/CheckApprovalAuthorizationMapped/");
        }

        public LeaveCreditDTO Leaveapprover(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/Leaveapprover/");
        }


        public LeaveCreditDTO proxytaskdetails(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/proxytaskdetails/");
        }
        //-------------LRSaveUpdate---------------//  
        public LeaveCreditDTO LRdetails(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/LRdetails/");
        }
        public LeaveCreditDTO getdepartment(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/getdepartment/");
        }
        public LeaveCreditDTO LRSaveUpdate(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/LRSaveUpdate/");
        }
        public LeaveCreditDTO LREdit(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/LREdit/");
        }
        public LeaveCreditDTO ActiveDeactiveRecord(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/ActiveDeactiveRecord/");
        }
        public LeaveCreditDTO leveConditions(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/leveConditions/");
        }
    }
}
