using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class VenueMappingDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<VenueMapping_DTO, VenueMapping_DTO> COMMM = new CommonDelegate<VenueMapping_DTO, VenueMapping_DTO>();

        public VenueMapping_DTO getdetails(VenueMapping_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "VenueMappingFacade/getdetails/");
        }
        public VenueMapping_DTO saverecord(VenueMapping_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "VenueMappingFacade/saverecord");
        }
        public VenueMapping_DTO deactiveY(VenueMapping_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "VenueMappingFacade/deactiveY");
        }


    }
}
