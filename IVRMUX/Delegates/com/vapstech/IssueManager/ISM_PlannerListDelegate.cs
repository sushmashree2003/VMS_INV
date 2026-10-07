using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_PlannerListDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_PlannerCreationDTO, ISM_PlannerCreationDTO> COMMM = new CommonDelegate<ISM_PlannerCreationDTO, ISM_PlannerCreationDTO>();

        public ISM_PlannerCreationDTO getdetails(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerListFacade/getdetails/");
        }
        public ISM_PlannerCreationDTO gettaskdetails(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerListFacade/gettaskdetails");
        }
        public ISM_PlannerCreationDTO savePlanner(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerListFacade/savePlanner");
        }
        public ISM_PlannerCreationDTO saveAddTask(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerListFacade/saveAddTask");
        }
        public ISM_PlannerCreationDTO removetask(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerListFacade/removetask");
        }
      

    }
}
