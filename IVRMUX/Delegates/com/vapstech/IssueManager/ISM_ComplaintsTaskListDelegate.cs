using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_ComplaintsTaskListDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_ComplaintsTaskListDTO, ISM_ComplaintsTaskListDTO> COMMM = new CommonDelegate<ISM_ComplaintsTaskListDTO, ISM_ComplaintsTaskListDTO>();

        public ISM_ComplaintsTaskListDTO getdetails(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/getdetails/");
        }


        public ISM_ComplaintsTaskListDTO deptfilter(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/deptfilter");
        }
        public ISM_ComplaintsTaskListDTO gettaskdetails(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/gettaskdetails");
        }
        public ISM_ComplaintsTaskListDTO savePlanner(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/savePlanner");
        }
        public ISM_ComplaintsTaskListDTO saveTransfer(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/saveTransfer");
        }
        public ISM_ComplaintsTaskListDTO saveResponse(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/saveResponse");
        }
        public ISM_ComplaintsTaskListDTO saveMultiResponse(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/saveMultiResponse");
        }
        public ISM_ComplaintsTaskListDTO getsearchtask(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/getsearchtask");
        }
        public ISM_ComplaintsTaskListDTO getsearchtaskdetails(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/getsearchtaskdetails");
        }
        public ISM_ComplaintsTaskListDTO getcount(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/getcount");
        }
        public ISM_ComplaintsTaskListDTO deactive(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/deactive");
        }
        public ISM_ComplaintsTaskListDTO gettaskAttachment(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/gettaskAttachment");
        }

        //Global Search Task
        public ISM_ComplaintsTaskListDTO GetTaskSearchDetails(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/GetTaskSearchDetails");
        }
        //SaveApprovaldata
        public ISM_ComplaintsTaskListDTO SaveApprovaldata(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/SaveApprovaldata");
        }
        #region DuplicateTaskClose
        public ISM_ComplaintsTaskListDTO getDuplicateTasksList(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/getDuplicateTasksList/");
        }
        //saveDuplicateTask
        public ISM_ComplaintsTaskListDTO saveDuplicateTask(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/saveDuplicateTask");
        }
        public ISM_ComplaintsTaskListDTO getDuplicateApprovalTasksList(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/getDuplicateApprovalTasksList/");
        }
        
        public ISM_ComplaintsTaskListDTO approveOrRejectDuplicateTask(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ComplaintsTaskListFacade/approveOrRejectDuplicateTask");
        }
        #endregion
    }
}
