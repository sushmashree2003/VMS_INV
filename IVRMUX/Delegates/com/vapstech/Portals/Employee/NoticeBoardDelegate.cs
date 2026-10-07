using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class NoticeBoardDelegats
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_NoticeBoardDTO, IVRM_NoticeBoardDTO> COMMM = new CommonDelegate<IVRM_NoticeBoardDTO, IVRM_NoticeBoardDTO>();
        public IVRM_NoticeBoardDTO savedetail(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/savedetail/");
        }
        public IVRM_NoticeBoardDTO Getdetails(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/Getdetails/");
        }
        public IVRM_NoticeBoardDTO deactivate(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/deactivate/");
        }

        public IVRM_NoticeBoardDTO editdetails(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/editdetails/");
        }

        

    }
}
