using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class TenantTaskCreationDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TenantTaskCreationDTO, TenantTaskCreationDTO> COMMM = new CommonDelegate<TenantTaskCreationDTO, TenantTaskCreationDTO>();

        public TenantTaskCreationDTO getdetails(TenantTaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantTaskCreationFacade/getdetails/");
        }
        public TenantTaskCreationDTO getmodule(TenantTaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantTaskCreationFacade/getmodule");
        }
        public TenantTaskCreationDTO getIEuser(TenantTaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantTaskCreationFacade/getIEuser");
        }
        public TenantTaskCreationDTO savedata(TenantTaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantTaskCreationFacade/savedata");
        }
        public TenantTaskCreationDTO deactive(TenantTaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantTaskCreationFacade/deactive");
        }
         public TenantTaskCreationDTO gettaskAttachment(TenantTaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantTaskCreationFacade/gettaskAttachment");
        }
         public TenantTaskCreationDTO get_category(TenantTaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantTaskCreationFacade/get_category");
        }
        public TenantTaskCreationDTO saveResponse(TenantTaskCreationDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantTaskCreationFacade/saveResponse");
        }

    }
}
