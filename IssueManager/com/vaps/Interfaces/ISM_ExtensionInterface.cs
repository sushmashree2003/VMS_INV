using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_ExtensionInterface
    {
        Task<ISM_ExtensionDTO> getdetails(ISM_ExtensionDTO data);
        ISM_ExtensionDTO get_details(ISM_ExtensionDTO data);
        ISM_ExtensionDTO savedata(ISM_ExtensionDTO data);
        Task<ISM_ExtensionDTO> getdata(ISM_ExtensionDTO data);
        ISM_ExtensionDTO savedetails(ISM_ExtensionDTO data);
    }
}
