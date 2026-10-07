using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_ExtensionDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_ExtensionDTO, ISM_ExtensionDTO> COMMM = new CommonDelegate<ISM_ExtensionDTO, ISM_ExtensionDTO>();

        public ISM_ExtensionDTO getdetails(ISM_ExtensionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ExtensionFacade/getdetails/");
        }
        public ISM_ExtensionDTO get_details(ISM_ExtensionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ExtensionFacade/get_details");
        }    
        public ISM_ExtensionDTO savedata(ISM_ExtensionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ExtensionFacade/savedata");
        }

        //TADA Extention
        public ISM_ExtensionDTO getdata(ISM_ExtensionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ExtensionFacade/getdata");
        }
        public ISM_ExtensionDTO savedetails(ISM_ExtensionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ExtensionFacade/savedetails");
        }

    }
}
