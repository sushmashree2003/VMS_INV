using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.VMS_Tracking.Interface
{
    public interface ClientWise_Module_Feature_Inrerface
    {
        ClientWise_Module_Feature_DTO getloaddata(ClientWise_Module_Feature_DTO dto);
        Task<ClientWise_Module_Feature_DTO> getmoduleAsync(ClientWise_Module_Feature_DTO dto);
        ClientWise_Module_Feature_DTO getreport(ClientWise_Module_Feature_DTO dto);
    }
}
