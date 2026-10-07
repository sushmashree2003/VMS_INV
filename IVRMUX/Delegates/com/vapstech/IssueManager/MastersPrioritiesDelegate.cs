using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class MastersPrioritiesDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MastersPriorities_DTO, MastersPriorities_DTO> COMMM = new CommonDelegate<MastersPriorities_DTO, MastersPriorities_DTO>();

        public MastersPriorities_DTO getdetails(MastersPriorities_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MastersPrioritiesFacade/getdetails/");
        }
        public MastersPriorities_DTO saverecord(MastersPriorities_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MastersPrioritiesFacade/saverecord");
        }
        public MastersPriorities_DTO deactiveY(MastersPriorities_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MastersPrioritiesFacade/deactiveY");
        }

    }
}
