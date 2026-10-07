using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class MasterStatusDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterStatus_DTO, MasterStatus_DTO> COMMM = new CommonDelegate<MasterStatus_DTO, MasterStatus_DTO>();

        public MasterStatus_DTO getdetails(MasterStatus_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MasterStatusFacade/getdetails/");
        }
        public MasterStatus_DTO saverecord(MasterStatus_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MasterStatusFacade/saverecord");
        }
        public MasterStatus_DTO deactiveY(MasterStatus_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MasterStatusFacade/deactiveY");
        }
        public MasterStatus_DTO save_ack(MasterStatus_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MasterStatusFacade/save_ack");
        }


    }
}
