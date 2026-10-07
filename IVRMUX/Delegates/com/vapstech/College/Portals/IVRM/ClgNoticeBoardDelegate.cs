using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;

namespace corewebapi18072016.Delegates.com.vapstech.College.Portals.IVRM
{
    public class ClgNoticeBoardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgNoticeBoardDTO, ClgNoticeBoardDTO> COMMM = new CommonDelegate<ClgNoticeBoardDTO, ClgNoticeBoardDTO>();
        public ClgNoticeBoardDTO getloaddata(ClgNoticeBoardDTO data)
        {     
            return COMMM.CLGPortalPOSTData(data, "ClgNoticeBoardFacade/getloaddata/");
        }
        public ClgNoticeBoardDTO getbranchdata(ClgNoticeBoardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgNoticeBoardFacade/getbranchdata/");
        }
        public ClgNoticeBoardDTO getsemdata(ClgNoticeBoardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgNoticeBoardFacade/getsemdata/");
        }
        public ClgNoticeBoardDTO savedata(ClgNoticeBoardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgNoticeBoardFacade/savedata/");
        }
        public ClgNoticeBoardDTO getNoticedata(ClgNoticeBoardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgNoticeBoardFacade/getNoticedata/");
        }
        public ClgNoticeBoardDTO editdetails(ClgNoticeBoardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgNoticeBoardFacade/editdetails/");
        }
        public ClgNoticeBoardDTO deactive(ClgNoticeBoardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgNoticeBoardFacade/deactive/");
        }
        public ClgNoticeBoardDTO deactivedetails(ClgNoticeBoardDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgNoticeBoardFacade/deactivedetails/");
        }


        
    }
}
