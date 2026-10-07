using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface ImsClientInterface
    {
        Clients_DTO getdetails(Clients_DTO data);
        Clients_DTO OnChangeTab1Inst(Clients_DTO data);
        Clients_DTO saveClientdata(Clients_DTO data);
        Clients_DTO clientDecative(Clients_DTO data);
        Clients_DTO editClientdata(Clients_DTO data);
        Clients_DTO OnChangeTab2Inst(Clients_DTO data);
        Clients_DTO saveClientMappingdata(Clients_DTO data);
        Clients_DTO editClientMappingdata(Clients_DTO data);
        Clients_DTO deactiveClientMappingdata(Clients_DTO data);
        Clients_DTO modalListdata(Clients_DTO data);

        //VMS Client And IVRM Client Mapping
        Clients_DTO OnChangeClientTab3(Clients_DTO data);
        Clients_DTO SaveVMSIVRMMapping(Clients_DTO data);
        //savefeedback
        Clients_DTO savefeedback(Clients_DTO data);
        //editdata
        Clients_DTO editdata(Clients_DTO data);        
    }
}
