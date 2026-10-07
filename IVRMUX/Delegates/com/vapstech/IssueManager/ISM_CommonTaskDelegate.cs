using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_CommonTaskDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_CommonTask_DTO, ISM_CommonTask_DTO> COMMM = new CommonDelegate<ISM_CommonTask_DTO, ISM_CommonTask_DTO>();

        public ISM_CommonTask_DTO Loaddata(ISM_CommonTask_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_CommonTaskFacade/Loaddata/");
        }
        public ISM_CommonTask_DTO get_Designation(ISM_CommonTask_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_CommonTaskFacade/get_Designation/");
        }
        public ISM_CommonTask_DTO get_Employee(ISM_CommonTask_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_CommonTaskFacade/get_Employee/");
        }
        
    }
}
