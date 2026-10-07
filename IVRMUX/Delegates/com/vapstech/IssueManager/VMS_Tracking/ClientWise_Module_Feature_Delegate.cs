using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.VMS_Tracking
{
    public class ClientWise_Module_Feature_Delegate
    {
        CommonDelegate<ClientWise_Module_Feature_DTO, ClientWise_Module_Feature_DTO> COMM = new CommonDelegate<ClientWise_Module_Feature_DTO, ClientWise_Module_Feature_DTO>();

        public ClientWise_Module_Feature_DTO getloaddata(ClientWise_Module_Feature_DTO dto)
        {
            return COMM.POSTIssueManager(dto, "ClientWise_Module_Feature_Facade/getloaddata");
        }
        public ClientWise_Module_Feature_DTO getproject(ClientWise_Module_Feature_DTO dto)
        {
            return COMM.POSTIssueManager(dto, "ClientWise_Module_Feature_Facade/getproject");
        }
        public ClientWise_Module_Feature_DTO getmodule(ClientWise_Module_Feature_DTO dto)
        {
            return COMM.POSTIssueManager(dto, "ClientWise_Module_Feature_Facade/getmodule");
        }
        public ClientWise_Module_Feature_DTO getreport(ClientWise_Module_Feature_DTO dto)
        {
            return COMM.POSTIssueManager(dto, "ClientWise_Module_Feature_Facade/getreport");
        }

    }
}
