using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface TenantTaskCreationInterface
    {
        Task<TenantTaskCreationDTO> getdetails(TenantTaskCreationDTO data);
        TenantTaskCreationDTO getmodule(TenantTaskCreationDTO data);
        Task<TenantTaskCreationDTO> getIEuser(TenantTaskCreationDTO data);
        TenantTaskCreationDTO savedata(TenantTaskCreationDTO data);
        TenantTaskCreationDTO deactive(TenantTaskCreationDTO data);
        TenantTaskCreationDTO gettaskAttachment(TenantTaskCreationDTO data);
        TenantTaskCreationDTO get_category(TenantTaskCreationDTO data);

        Task<TenantTaskCreationDTO> getdetailsClient(TenantTaskCreationDTO data);
        TenantTaskCreationDTO savedataClient(TenantTaskCreationDTO data);
        TenantTaskCreationDTO saveResponse(TenantTaskCreationDTO data);
    }
}
