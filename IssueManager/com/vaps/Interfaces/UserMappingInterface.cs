using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
  public interface UserMappingInterface
    {
        Clients_DTO getdetails(Clients_DTO data);
        Clients_DTO saveClientMappingdata(Clients_DTO data);
        Clients_DTO deactiveClientMappingdata(Clients_DTO data);
        Clients_DTO editClientMappingdata(Clients_DTO data);
        Clients_DTO get_MappedUserlist(Clients_DTO data);

    }
}
