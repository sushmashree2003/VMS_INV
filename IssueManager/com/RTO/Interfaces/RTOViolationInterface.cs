using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.RTO.Interfaces
{
  public  interface RTOViolationInterface
    {
        RTO_Violation_ObservationDTO loaddata(RTO_Violation_ObservationDTO data);
        RTO_Violation_ObservationDTO savedata(RTO_Violation_ObservationDTO data);
        //onchange
        RTO_Violation_ObservationDTO onchange(RTO_Violation_ObservationDTO data);

        //RTO_Violation_ObservationDTO
        RTO_Violation_ObservationDTO savedataSource(RTO_Violation_ObservationDTO data);
        //clientDecative
        RTO_Violation_ObservationDTO clientDecative(RTO_Violation_ObservationDTO data);
        //RecieptReport
        RTO_Violation_ObservationDTO RecieptReport(RTO_Violation_ObservationDTO data);
    }
}
