using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_UserEmployeeMappingDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Clients_DTO, Clients_DTO> COMMM = new CommonDelegate<Clients_DTO, Clients_DTO>();

        public Clients_DTO getdetails(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_UserEmployeeMappingFacade/getdetails/");
        }
        public Clients_DTO getemployeelist(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_UserEmployeeMappingFacade/getemployeelist");
        }
     
        public Clients_DTO savedata(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_UserEmployeeMappingFacade/savedata");
        }
          public Clients_DTO deactive(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_UserEmployeeMappingFacade/deactive");
        }
        public Clients_DTO updateorder(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_UserEmployeeMappingFacade/updateorder");
        }

        public Clients_DTO getdetailsNew(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_UserEmployeeMappingFacade/getdetailsNew/");
        }
        public Clients_DTO getUserEmpMappedReport(Clients_DTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_UserEmployeeMappingFacade/getUserEmpMappedReport/");
        }
    }
}
