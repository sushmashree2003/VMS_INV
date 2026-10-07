using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_NoticeIssueDelegate 
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_PlannerCreationDTO, ISM_PlannerCreationDTO> COMMM = new CommonDelegate<ISM_PlannerCreationDTO, ISM_PlannerCreationDTO>();

        public ISM_PlannerCreationDTO getdetails(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NoticeIssueFacade/getdetails/");
        }
        public ISM_PlannerCreationDTO getdepartmentlist(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NoticeIssueFacade/getdepartmentlist");
        }
        public ISM_PlannerCreationDTO getplannerlist(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NoticeIssueFacade/getplannerlist");
        }
        public ISM_PlannerCreationDTO getplannerdetails(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NoticeIssueFacade/getplannerdetails");
        }
        public ISM_PlannerCreationDTO updateplannerStatus(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NoticeIssueFacade/updateplannerStatus");
        }
        public ISM_PlannerCreationDTO allupdateplannerStatus(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NoticeIssueFacade/allupdateplannerStatus");
        }
        public ISM_PlannerCreationDTO saveAddTask(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NoticeIssueFacade/saveAddTask");
        }
        public ISM_PlannerCreationDTO getextratask(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NoticeIssueFacade/getextratask");
        }
       public ISM_PlannerCreationDTO getApprovedPlannerDetail(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NoticeIssueFacade/getApprovedPlannerDetail");
        }
      

    }
}
