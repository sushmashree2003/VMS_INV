using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class Lead_To_ClientDelegate
    {
        CommonDelegate<Lead_To_ClientDTO, Lead_To_ClientDTO> COMMM = new CommonDelegate<Lead_To_ClientDTO, Lead_To_ClientDTO>();

        public Lead_To_ClientDTO getdetails(Lead_To_ClientDTO data)
        {
            return COMMM.POSTIssueManager(data, "Lead_To_ClientFacade/getdetails/");
        }
        public Lead_To_ClientDTO OnChangeTab1Inst(Lead_To_ClientDTO data)
        {
            return COMMM.POSTIssueManager(data, "Lead_To_ClientFacade/OnChangeTab1Inst");
        }
        public Lead_To_ClientDTO saveClientdata(Lead_To_ClientDTO data)
        {
            return COMMM.POSTIssueManager(data, "Lead_To_ClientFacade/saveClientdata");
        }
        public Lead_To_ClientDTO clientDecative(Lead_To_ClientDTO data)
        {
            return COMMM.POSTIssueManager(data, "Lead_To_ClientFacade/clientDecative");
        }
        public Lead_To_ClientDTO editClientdata(Lead_To_ClientDTO data)
        {
            return COMMM.POSTIssueManager(data, "Lead_To_ClientFacade/editClientdata");
        }
        public Lead_To_ClientDTO OnChangeTab2Inst(Lead_To_ClientDTO data)
        {
            return COMMM.POSTIssueManager(data, "Lead_To_ClientFacade/OnChangeTab2Inst");
        }
        public Lead_To_ClientDTO saveClientMappingdata(Lead_To_ClientDTO data)
        {
            return COMMM.POSTIssueManager(data, "Lead_To_ClientFacade/saveClientMappingdata");
        }
        public Lead_To_ClientDTO editClientMappingdata(Lead_To_ClientDTO data)
        {
            return COMMM.POSTIssueManager(data, "Lead_To_ClientFacade/editClientMappingdata");
        }
        public Lead_To_ClientDTO deactiveClientMappingdata(Lead_To_ClientDTO data)
        {
            return COMMM.POSTIssueManager(data, "Lead_To_ClientFacade/deactiveClientMappingdata");
        }
        public Lead_To_ClientDTO modalListdata(Lead_To_ClientDTO data)
        {
            return COMMM.POSTIssueManager(data, "Lead_To_ClientFacade/modalListdata");
        }

        //VMS Client And IVRM Client Mapping 
        public Lead_To_ClientDTO OnChangeClientTab3(Lead_To_ClientDTO data)
        {
            return COMMM.POSTIssueManager(data, "Lead_To_ClientFacade/OnChangeClientTab3");
        }
        public Lead_To_ClientDTO SaveVMSIVRMMapping(Lead_To_ClientDTO data)
        {
            return COMMM.POSTIssueManager(data, "Lead_To_ClientFacade/SaveVMSIVRMMapping");
        }
    }
}
