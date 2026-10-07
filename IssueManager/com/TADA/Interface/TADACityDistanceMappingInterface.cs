using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
  public  interface TADACityDistanceMappingInterface
    {
        TADACityDistanceMappingDTO getalldata(TADACityDistanceMappingDTO data);
        TADACityDistanceMappingDTO OnchangeCity(TADACityDistanceMappingDTO data);
        TADACityDistanceMappingDTO savedata(TADACityDistanceMappingDTO data);
        TADACityDistanceMappingDTO EditData(TADACityDistanceMappingDTO data);
        TADACityDistanceMappingDTO TravelDetails(TADACityDistanceMappingDTO data);
        TADACityDistanceMappingDTO GetDesignation(TADACityDistanceMappingDTO data);
        TADACityDistanceMappingDTO Savedetails(TADACityDistanceMappingDTO data);
        TADACityDistanceMappingDTO EditDetails(TADACityDistanceMappingDTO data);
        TADACityDistanceMappingDTO Getcities(TADACityDistanceMappingDTO data);
        TADACityDistanceMappingDTO SaveTierdata(TADACityDistanceMappingDTO data);
    }
}
