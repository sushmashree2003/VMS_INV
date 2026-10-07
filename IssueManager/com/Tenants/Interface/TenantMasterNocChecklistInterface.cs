using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface TenantMasterNocChecklistInterface
    {
        //Master Type
        TenantMasterNocChecklistDTO getloaddata(TenantMasterNocChecklistDTO data);
        TenantMasterNocChecklistDTO getamenity(TenantMasterNocChecklistDTO data);
        TenantMasterNocChecklistDTO savedetails(TenantMasterNocChecklistDTO data);
        TenantMasterNocChecklistDTO activedeactive(TenantMasterNocChecklistDTO data);
    }
}
