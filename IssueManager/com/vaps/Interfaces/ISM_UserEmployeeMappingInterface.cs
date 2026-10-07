using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
  public interface ISM_UserEmployeeMappingInterface
    {
        Task<Clients_DTO> getdetails(Clients_DTO data);
        Clients_DTO getemployeelist(Clients_DTO data);
     
        Clients_DTO savedata(Clients_DTO data);
        Clients_DTO deactive(Clients_DTO data);
        Clients_DTO updateorder(Clients_DTO data);

        Task<Clients_DTO> getdetailsNew(Clients_DTO data);
        Task<Clients_DTO> getUserEmpMappedReport(Clients_DTO data);
    }
}
