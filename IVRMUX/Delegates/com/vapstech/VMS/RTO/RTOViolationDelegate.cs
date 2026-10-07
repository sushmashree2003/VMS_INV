using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.RTO
{
    public class RTOViolationDelegate
    {
        CommonDelegate<RTO_Violation_ObservationDTO, RTO_Violation_ObservationDTO> comm = new CommonDelegate<RTO_Violation_ObservationDTO, RTO_Violation_ObservationDTO>();
      
        public RTO_Violation_ObservationDTO loaddata(RTO_Violation_ObservationDTO data)
        {
            return comm.POSTIssueManager(data, "RTOViolationFacade/loaddata");
        }
        public RTO_Violation_ObservationDTO savedata(RTO_Violation_ObservationDTO data)
        {
            return comm.POSTIssueManager(data, "RTOViolationFacade/savedata");
        }
        //onchange
        public RTO_Violation_ObservationDTO onchange(RTO_Violation_ObservationDTO data)
        {
            return comm.POSTIssueManager(data, "RTOViolationFacade/onchange");
        }
        //savedataSource
        public RTO_Violation_ObservationDTO savedataSource(RTO_Violation_ObservationDTO data)
        {
            return comm.POSTIssueManager(data, "RTOViolationFacade/savedataSource");
        }
        //clientDecative
        public RTO_Violation_ObservationDTO clientDecative(RTO_Violation_ObservationDTO data)
        {
            return comm.POSTIssueManager(data, "RTOViolationFacade/clientDecative");
        }

        //
        public RTO_Violation_ObservationDTO RecieptReport(RTO_Violation_ObservationDTO data)
        {
            return comm.POSTIssueManager(data, "RTOViolationFacade/RecieptReport");
        }
    }
}
