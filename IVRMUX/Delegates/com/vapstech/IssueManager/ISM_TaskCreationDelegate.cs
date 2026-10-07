using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_TaskCreationDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_TaskCreationDTO, ISM_TaskCreationDTO> COMMM = new CommonDelegate<ISM_TaskCreationDTO, ISM_TaskCreationDTO>();

        public ISM_TaskCreationDTO getdetails(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskCreationFacade/getdetails/");
        }
        public ISM_TaskCreationDTO getmodule(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskCreationFacade/getmodule");
        }
        public ISM_TaskCreationDTO getIEuser(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskCreationFacade/getIEuser");
        }
        public ISM_TaskCreationDTO savedata(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskCreationFacade/savedata");
        }
        public ISM_TaskCreationDTO deactive(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskCreationFacade/deactive");
        }
         public ISM_TaskCreationDTO gettaskAttachment(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskCreationFacade/gettaskAttachment");
        }
         public ISM_TaskCreationDTO get_category(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskCreationFacade/get_category");
        }
        public ISM_TaskCreationDTO SaveTaskDeptWise(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskCreationFacade/SaveTaskDeptWise");
        }

    }
}
