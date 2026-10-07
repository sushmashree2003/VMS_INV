using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_TaskRegisterReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_TaskCreationDTO, ISM_TaskCreationDTO> COMMM = new CommonDelegate<ISM_TaskCreationDTO, ISM_TaskCreationDTO>();

        public ISM_TaskCreationDTO getdetails(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskRegisterReportFacade/getdetails/");
        }

        public ISM_TaskCreationDTO getemployeelist(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskRegisterReportFacade/getemployeelist/");
        }
        public ISM_TaskCreationDTO gettaskregisterReport(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskRegisterReportFacade/gettaskregisterReport");
        }
        public ISM_TaskCreationDTO Deptselectiondetails(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskRegisterReportFacade/Deptselectiondetails");
        }
        public ISM_TaskCreationDTO Desgselectiondetails(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskRegisterReportFacade/Desgselectiondetails");
        }
        //=========== makerchecker consolidate report
        public ISM_TaskCreationDTO getconsuldatedetails(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskRegisterReportFacade/getconsuldatedetails");
        }
         public ISM_TaskCreationDTO getmakercheckerReport(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskRegisterReportFacade/getmakercheckerReport");
        }
         public ISM_TaskCreationDTO getmakercheckerview(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskRegisterReportFacade/getmakercheckerview");
        }
        //ISM_TaskRegister
        public ISM_TaskCreationDTO ISM_TaskRegister(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskRegisterReportFacade/ISM_TaskRegister");
        }
        //ISM_TaskReport
        public ISM_TaskCreationDTO ISM_TaskReport(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskRegisterReportFacade/ISM_TaskReport");
        }
        public ISM_TaskCreationDTO getmakerRejectedReport(ISM_TaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskRegisterReportFacade/getmakerRejectedReport");
        }
    }
}
