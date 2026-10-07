using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_PlannerCreationDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_PlannerCreationDTO, ISM_PlannerCreationDTO> COMMM = new CommonDelegate<ISM_PlannerCreationDTO, ISM_PlannerCreationDTO>();

        public ISM_PlannerCreationDTO getdetails(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerCreationFacade/getdetails/");
        }
        public ISM_PlannerCreationDTO gettaskdetails(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerCreationFacade/gettaskdetails");
        }

        public ISM_PlannerCreationDTO geteffortdetails(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerCreationFacade/geteffortdetails");
        }
        public ISM_PlannerCreationDTO savePlanner(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerCreationFacade/savePlanner");
        }
        public ISM_PlannerCreationDTO saveAddTask(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerCreationFacade/saveAddTask");
        }
        public ISM_PlannerCreationDTO removetask(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerCreationFacade/removetask");
        }
        
            public ISM_PlannerCreationDTO getplannerdetails(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerCreationFacade/getplannerdetails");
        }
        

    }
}
