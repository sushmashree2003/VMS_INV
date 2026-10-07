using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ViewOfDeploymentDelegate
    {
        CommonDelegate<ViewOfDeploymentDTO, ViewOfDeploymentDTO> COMMM = new CommonDelegate<ViewOfDeploymentDTO, ViewOfDeploymentDTO>();
        public ViewOfDeploymentDTO getloaddata(ViewOfDeploymentDTO data)
        {
            return COMMM.POSTIssueManager(data, "ViewOfDeploymentFacade/getloaddata/");
        }
        public ViewOfDeploymentDTO gettaskslist(ViewOfDeploymentDTO data)
        {
            return COMMM.POSTIssueManager(data, "ViewOfDeploymentFacade/gettaskslist/");
        }
        
        public ViewOfDeploymentDTO GetOnchangeData(ViewOfDeploymentDTO data)
        {
            return COMMM.POSTIssueManager(data, "ViewOfDeploymentFacade/GetOnchangeData");
        }
        public ViewOfDeploymentDTO GetOnchangeClient(ViewOfDeploymentDTO data)
        {
            return COMMM.POSTIssueManager(data, "ViewOfDeploymentFacade/GetOnchangeClient");
        }
        public ViewOfDeploymentDTO savedata(ViewOfDeploymentDTO data)
        {
            return COMMM.POSTIssueManager(data, "ViewOfDeploymentFacade/savedata");
        }
        //scope report

        public ViewOfDeploymentDTO getReport(ViewOfDeploymentDTO data)
        {
            return COMMM.POSTIssueManager(data, "ViewOfDeploymentFacade/getReport/");
        }
        public ViewOfDeploymentDTO approve_reject_scope(ViewOfDeploymentDTO data)
        {
            return COMMM.POSTIssueManager(data, "ViewOfDeploymentFacade/approve_reject_scope/");
        }
        public ViewOfDeploymentDTO get_scope_tasklist_for_approval(ViewOfDeploymentDTO data)
        {
            return COMMM.POSTIssueManager(data, "ViewOfDeploymentFacade/get_scope_tasklist_for_approval/");
        }
    }
}
