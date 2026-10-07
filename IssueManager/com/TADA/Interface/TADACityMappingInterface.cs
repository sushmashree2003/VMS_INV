using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
  public interface TADACityMappingInterface
    {
        TRDACityMappingDTO loaddata(TRDACityMappingDTO data);
        TRDACityMappingDTO saverecord(TRDACityMappingDTO data);
        TRDACityMappingDTO edit(TRDACityMappingDTO data);

        //deactive
        TRDACityMappingDTO deactive(TRDACityMappingDTO data);
        TRDACityMappingDTO getcity(TRDACityMappingDTO data);
        //getMasterReport
        TRDACityMappingDTO getMasterReport(TRDACityMappingDTO data);
    }
}
