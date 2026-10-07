using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_ImplTaskMappingDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_ImplTaskMappingDTO, ISM_ImplTaskMappingDTO> COMMM = new CommonDelegate<ISM_ImplTaskMappingDTO, ISM_ImplTaskMappingDTO>();

        public ISM_ImplTaskMappingDTO getdetails(ISM_ImplTaskMappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ImplTaskMappingFacade/getdetails/");
        }


        public ISM_ImplTaskMappingDTO deptfilter(ISM_ImplTaskMappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ImplTaskMappingFacade/deptfilter");
        }
        public ISM_ImplTaskMappingDTO gettaskdetails(ISM_ImplTaskMappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ImplTaskMappingFacade/gettaskdetails");
        }
        public ISM_ImplTaskMappingDTO savePlanner(ISM_ImplTaskMappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ImplTaskMappingFacade/savePlanner");
        }
        public ISM_ImplTaskMappingDTO saveTransfer(ISM_ImplTaskMappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ImplTaskMappingFacade/saveTransfer");
        }
        public ISM_ImplTaskMappingDTO saveResponse(ISM_ImplTaskMappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ImplTaskMappingFacade/saveResponse");
        }
        public ISM_ImplTaskMappingDTO getsearchtask(ISM_ImplTaskMappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ImplTaskMappingFacade/getsearchtask");
        }
        public ISM_ImplTaskMappingDTO getcount(ISM_ImplTaskMappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ImplTaskMappingFacade/getcount");
        }
        public ISM_ImplTaskMappingDTO deactive(ISM_ImplTaskMappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ImplTaskMappingFacade/deactive");
        }
         public ISM_ImplTaskMappingDTO gettaskAttachment(ISM_ImplTaskMappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ImplTaskMappingFacade/gettaskAttachment");
        }

    }
}
