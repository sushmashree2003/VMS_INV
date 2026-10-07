using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ViewOfDeploymentInterface
    {
        ViewOfDeploymentDTO getloaddata (ViewOfDeploymentDTO data);
        ViewOfDeploymentDTO gettaskslist (ViewOfDeploymentDTO data);
        ViewOfDeploymentDTO GetOnchangeData(ViewOfDeploymentDTO data);
        ViewOfDeploymentDTO GetOnchangeClient(ViewOfDeploymentDTO data);
        ViewOfDeploymentDTO savedata(ViewOfDeploymentDTO data);
        //scope report
        ViewOfDeploymentDTO getReport(ViewOfDeploymentDTO data);
        ViewOfDeploymentDTO approve_reject_scope(ViewOfDeploymentDTO data);
        ViewOfDeploymentDTO get_scope_tasklist_for_approval(ViewOfDeploymentDTO data);
    }
}
