using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.RTO.Interfaces
{
   public interface Vehicle_ObservationInterface
    {
        VMS_FO_Vehicle_ObservationDTO loaddata(VMS_FO_Vehicle_ObservationDTO data);
        VMS_FO_Vehicle_ObservationDTO savedata(VMS_FO_Vehicle_ObservationDTO data);
        //onchange
        VMS_FO_Vehicle_ObservationDTO onchange(VMS_FO_Vehicle_ObservationDTO data);

        //VMS_FO_Vehicle_ObservationDTO
        VMS_FO_Vehicle_ObservationDTO savedataSource(VMS_FO_Vehicle_ObservationDTO data);
        //clientDecative
        VMS_FO_Vehicle_ObservationDTO clientDecative(VMS_FO_Vehicle_ObservationDTO data);
        //RecieptReport
        VMS_FO_Vehicle_ObservationDTO RecieptReport(VMS_FO_Vehicle_ObservationDTO data);
    }
}
