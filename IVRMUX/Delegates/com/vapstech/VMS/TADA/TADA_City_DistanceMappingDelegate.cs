using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.TADA
{
    public class TADA_City_DistanceMappingDelegate
    {
        CommonDelegate<TADACityDistanceMappingDTO, TADACityDistanceMappingDTO> COMMI = new CommonDelegate<TADACityDistanceMappingDTO, TADACityDistanceMappingDTO>();
        public TADACityDistanceMappingDTO getalldata(TADACityDistanceMappingDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADACityDistanceMappingFacade/getalldata");
        }
        public TADACityDistanceMappingDTO OnchangeCity(TADACityDistanceMappingDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADACityDistanceMappingFacade/OnchangeCity/");
        }
        public TADACityDistanceMappingDTO savedata(TADACityDistanceMappingDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADACityDistanceMappingFacade/savedata/");
        }
        public TADACityDistanceMappingDTO EditData(TADACityDistanceMappingDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADACityDistanceMappingFacade/EditData/");
        }
        public TADACityDistanceMappingDTO TravelDetails(TADACityDistanceMappingDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADACityDistanceMappingFacade/TravelDetails/");
        }
        public TADACityDistanceMappingDTO GetDesignation(TADACityDistanceMappingDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADACityDistanceMappingFacade/GetDesignation/");
        }
        public TADACityDistanceMappingDTO Savedetails(TADACityDistanceMappingDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADACityDistanceMappingFacade/Savedetails/");
        }
        public TADACityDistanceMappingDTO EditDetails(TADACityDistanceMappingDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADACityDistanceMappingFacade/EditDetails/");
        }
        public TADACityDistanceMappingDTO Getcities(TADACityDistanceMappingDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADACityDistanceMappingFacade/Getcities/");
        }
        public TADACityDistanceMappingDTO SaveTierdata(TADACityDistanceMappingDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADACityDistanceMappingFacade/SaveTierdata/");
        }
    }

}
