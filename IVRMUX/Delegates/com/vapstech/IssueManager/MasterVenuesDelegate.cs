using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class MasterVenuesDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterVenues_DTO, MasterVenues_DTO> COMMM = new CommonDelegate<MasterVenues_DTO, MasterVenues_DTO>();

        public MasterVenues_DTO getdetails(MasterVenues_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MasterVenuesFacade/getdetails/");
        }
        public MasterVenues_DTO saverecord(MasterVenues_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MasterVenuesFacade/saverecord");
        }
        public MasterVenues_DTO deactiveY(MasterVenues_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MasterVenuesFacade/deactiveY");
        }

    }
}
