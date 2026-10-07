using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_PlannerApprovalDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_PlannerCreationDTO, ISM_PlannerCreationDTO> COMMM = new CommonDelegate<ISM_PlannerCreationDTO, ISM_PlannerCreationDTO>();

        public ISM_PlannerCreationDTO getdetails(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerApprovalFacade/getdetails/");
        }
        //loaddata
        public ISM_PlannerCreationDTO loaddata(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerApprovalFacade/loaddata/");
        }
        public ISM_PlannerCreationDTO getdepartmentlist(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerApprovalFacade/getdepartmentlist");
        }
        public ISM_PlannerCreationDTO getplannerlist(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerApprovalFacade/getplannerlist");
        }
        public ISM_PlannerCreationDTO getplannerdetails(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerApprovalFacade/getplannerdetails");
        }
        public ISM_PlannerCreationDTO updateplannerStatus(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerApprovalFacade/updateplannerStatus");
        }
        public ISM_PlannerCreationDTO allupdateplannerStatus(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerApprovalFacade/allupdateplannerStatus");
        }
        public ISM_PlannerCreationDTO saveAddTask(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerApprovalFacade/saveAddTask");
        }
        public ISM_PlannerCreationDTO getextratask(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerApprovalFacade/getextratask");
        }
       public ISM_PlannerCreationDTO getApprovedPlannerDetail(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerApprovalFacade/getApprovedPlannerDetail");
        }
        //deactiveemployee
        public ISM_PlannerCreationDTO deactiveemployee(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerApprovalFacade/deactiveemployee");
        }
        //saveemployee
        public ISM_PlannerCreationDTO saveemployee(ISM_PlannerCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerApprovalFacade/saveemployee");
        }

    }
}
