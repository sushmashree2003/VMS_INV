using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface NocCreationInterface
    {
        NocCreationDTO getloaddata(NocCreationDTO data);

        NocCreationDTO getfloors(NocCreationDTO data); 
            
        NocCreationDTO gettenantname(NocCreationDTO data);
        NocCreationDTO getAreaName(NocCreationDTO data);

        NocCreationDTO savedetailsStaffMapping(NocCreationDTO data);

        NocCreationDTO deactive(NocCreationDTO data);

        //NocCreationDTO edit(NocCreationDTO data);

    }
}
