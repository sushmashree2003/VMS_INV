using CommonLibrary;
//using IVRMUX.Controllers.com.vapstech.IssueManager.com.vaps.Services;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_ClientProject_MappingDelegate
    {
        CommonDelegate<ISM_ClientProject_MappingDTO, ISM_ClientProject_MappingDTO> comm = new CommonDelegate<ISM_ClientProject_MappingDTO, ISM_ClientProject_MappingDTO>();
        public ISM_ClientProject_MappingDTO loaddata(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_ClientProject_MappingFacade/loaddata");
        }
        public ISM_ClientProject_MappingDTO savedata(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_ClientProject_MappingFacade/savedata");
        }
        public ISM_ClientProject_MappingDTO Editdata(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_ClientProject_MappingFacade/Editdata");
        }
        public ISM_ClientProject_MappingDTO getproject(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_ClientProject_MappingFacade/getproject");
        }
        public ISM_ClientProject_MappingDTO getmodule(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_ClientProject_MappingFacade/getmodule");
        }
        public ISM_ClientProject_MappingDTO clientDecative(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_ClientProject_MappingFacade/clientDecative");
        }
        public ISM_ClientProject_MappingDTO savedataLead(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_ClientProject_MappingFacade/savedataLead");
        }
        //added by praveenkumar 
        public ISM_ClientProject_MappingDTO SaveApprovalVerify(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_ClientProject_MappingFacade/SaveApprovalVerify");
        }
        //added by praveenkumar 
        public ISM_ClientProject_MappingDTO SaveOrderAcceptanceApproval(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_ClientProject_MappingFacade/SaveOrderAcceptanceApproval");
        }
        //added by praveenkumarSigned mou view details
        public ISM_ClientProject_MappingDTO ViewMOUCheckUpFiles(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_ClientProject_MappingFacade/ViewMOUCheckUpFiles");
        }
        public ISM_ClientProject_MappingDTO viewtemplatedetails(ISM_ClientProject_MappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_ClientProject_MappingFacade/viewtemplatedetails/");
        }
        public ISM_ClientProject_MappingDTO OnChangeOnChangeBOMList(ISM_ClientProject_MappingDTO dto)
        {
            return comm.POSTIssueManager(dto, "ISM_ClientProject_MappingFacade/OnChangeOnChangeBOMList/");
        }
        //sendMail
        public ISM_ClientProject_MappingDTO sendMail(ISM_ClientProject_MappingDTO dto)
        {
            return comm.POSTIssueManager(dto, "ISM_ClientProject_MappingFacade/sendMail/");
        }
        public ISM_ClientProject_MappingDTO releaseversion(ISM_ClientProject_MappingDTO dto)
        {
            return comm.POSTIssueManager(dto, "ISM_ClientProject_MappingFacade/releaseversion/");
        }
    }
}
