using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ImsClientDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Clients_DTO, Clients_DTO> COMMM = new CommonDelegate<Clients_DTO, Clients_DTO>();

        public Clients_DTO getdetails(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/getdetails/");
        }
        public Clients_DTO OnChangeTab1Inst(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/OnChangeTab1Inst");
        }
        public Clients_DTO saveClientdata(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/saveClientdata");
        }
        public Clients_DTO clientDecative(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/clientDecative");
        }
        public Clients_DTO editClientdata(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/editClientdata");
        }
        public Clients_DTO OnChangeTab2Inst(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/OnChangeTab2Inst");
        }
        public Clients_DTO saveClientMappingdata(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/saveClientMappingdata");
        }
        public Clients_DTO editClientMappingdata(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/editClientMappingdata");
        }
        public Clients_DTO deactiveClientMappingdata(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/deactiveClientMappingdata");
        }
        public Clients_DTO modalListdata(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/modalListdata");
        }

        //VMS Client And IVRM Client Mapping 
        public Clients_DTO OnChangeClientTab3(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/OnChangeClientTab3");
        }
        public Clients_DTO SaveVMSIVRMMapping(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/SaveVMSIVRMMapping");
        }
        //savefeedback
        public Clients_DTO savefeedback(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/savefeedback");
        }
        //editdata
        public Clients_DTO editdata(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ImsClientFacade/editdata");
        }
    }
}
