using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class IVRM_ClassWorkDelegats
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_ClassWorkDTO, IVRM_ClassWorkDTO> COMMM = new CommonDelegate<IVRM_ClassWorkDTO, IVRM_ClassWorkDTO>();
        public IVRM_ClassWorkDTO savedetail(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/savedetail/");
        }
        public IVRM_ClassWorkDTO Getdetails(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/Getdetails/");
        }
        public IVRM_ClassWorkDTO deactivate(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/deactivate/");
        }
        public IVRM_ClassWorkDTO get_classes(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/get_classes/");
        }

        public IVRM_ClassWorkDTO getsectiondata(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/getsectiondata/");
        }
        public IVRM_ClassWorkDTO getsubject(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/getsubject/");
        }

        public IVRM_ClassWorkDTO editData(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/editData/");
        }

    }
}
