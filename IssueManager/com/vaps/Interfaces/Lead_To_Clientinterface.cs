using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface Lead_To_Clientinterface
    {
        Lead_To_ClientDTO getdetails(Lead_To_ClientDTO data);
        Lead_To_ClientDTO OnChangeTab1Inst(Lead_To_ClientDTO data);
        Lead_To_ClientDTO saveClientdata(Lead_To_ClientDTO data);
        Lead_To_ClientDTO clientDecative(Lead_To_ClientDTO data);
        Lead_To_ClientDTO editClientdata(Lead_To_ClientDTO data);
        Lead_To_ClientDTO OnChangeTab2Inst(Lead_To_ClientDTO data);
        Lead_To_ClientDTO saveClientMappingdata(Lead_To_ClientDTO data);
        Lead_To_ClientDTO editClientMappingdata(Lead_To_ClientDTO data);
        Lead_To_ClientDTO deactiveClientMappingdata(Lead_To_ClientDTO data);
        Lead_To_ClientDTO modalListdata(Lead_To_ClientDTO data);

       
        Lead_To_ClientDTO OnChangeClientTab3(Lead_To_ClientDTO data);
        Lead_To_ClientDTO SaveVMSIVRMMapping(Lead_To_ClientDTO data);
    }
}
