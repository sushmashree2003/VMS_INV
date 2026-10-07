using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class UserMappingDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Clients_DTO, Clients_DTO> COMMM = new CommonDelegate<Clients_DTO, Clients_DTO>();

        public Clients_DTO getdetails(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "UserMappingFacade/getdetails/");
        }
        public Clients_DTO saveClientMappingdata(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "UserMappingFacade/saveClientMappingdata");
        }
        public Clients_DTO deactiveClientMappingdata(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "UserMappingFacade/deactiveClientMappingdata");
        }
        public Clients_DTO editClientMappingdata(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "UserMappingFacade/editClientMappingdata");
        }
        public Clients_DTO get_MappedUserlist(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "UserMappingFacade/get_MappedUserlist");
        }
        
    }
}
