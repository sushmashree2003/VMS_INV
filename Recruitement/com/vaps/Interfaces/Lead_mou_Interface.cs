using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Lead_mou_Interface
    {
        Lead_Target_MappingDTO getBasicData(Lead_Target_MappingDTO dto);
        Lead_Target_MappingDTO lead_product(Lead_Target_MappingDTO dto);
        Lead_Target_MappingDTO savedata(Lead_Target_MappingDTO dto);
        Lead_Target_MappingDTO MOUViewdata(Lead_Target_MappingDTO dto);
        Lead_Target_MappingDTO sendmail(Lead_Target_MappingDTO dto);
        
    }
}
