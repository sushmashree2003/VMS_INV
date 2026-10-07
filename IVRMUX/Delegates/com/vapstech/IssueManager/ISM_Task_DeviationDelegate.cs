using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_Task_DeviationDelegate
    {
        CommonDelegate<ISM_Task_DeviationDTO, ISM_Task_DeviationDTO> COMMM = new CommonDelegate<ISM_Task_DeviationDTO, ISM_Task_DeviationDTO>();
        public ISM_Task_DeviationDTO GetDetails(ISM_Task_DeviationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_Task_DeviationFacade/GetDetails/");
        }
        public ISM_Task_DeviationDTO getdeviationReport(ISM_Task_DeviationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_Task_DeviationFacade/getdeviationReport");
        }
        public ISM_Task_DeviationDTO DeptSelectionDetails(ISM_Task_DeviationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_Task_DeviationFacade/DeptSelectionDetails/");
        }
        public ISM_Task_DeviationDTO DesgSelectionDetails(ISM_Task_DeviationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_Task_DeviationFacade/DesgSelectionDetails/");
        }
        public ISM_Task_DeviationDTO SaveDateExtention(ISM_Task_DeviationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_Task_DeviationFacade/SaveDateExtention");
        }

        public ISM_Task_DeviationDTO OnloadApproval(ISM_Task_DeviationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_Task_DeviationFacade/OnloadApproval/");
        }

        //==========Request Details Approval=================
        public ISM_Task_DeviationDTO Getrequestdetails(ISM_Task_DeviationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_Task_DeviationFacade/Getrequestdetails");
        }

        public ISM_Task_DeviationDTO SaveApprovalExtention(ISM_Task_DeviationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_Task_DeviationFacade/SaveApprovalExtention");
        }
        public ISM_Task_DeviationDTO ViewApproveDetails(ISM_Task_DeviationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_Task_DeviationFacade/ViewApproveDetails");
        }

       
    }
}
