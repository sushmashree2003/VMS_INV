using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.RTO
{
    public class Vehicle_ObservationDelegate
    {
        CommonDelegate<VMS_FO_Vehicle_ObservationDTO, VMS_FO_Vehicle_ObservationDTO> comm = new CommonDelegate<VMS_FO_Vehicle_ObservationDTO, VMS_FO_Vehicle_ObservationDTO>();
       // CommonDelegate<VMS_FO_Vehicle_ObservationDTO, VMS_FO_Vehicle_ObservationDTO> commT = new CommonDelegate<VMS_FO_Vehicle_ObservationDTO, VMS_FO_Vehicle_ObservationDTO>();

        public VMS_FO_Vehicle_ObservationDTO loaddata(VMS_FO_Vehicle_ObservationDTO data)
        {
            return comm.POSTIssueManager(data, "Vehicle_ObservationFacade/loaddata");
        }
        public VMS_FO_Vehicle_ObservationDTO savedata(VMS_FO_Vehicle_ObservationDTO data)
        {
            return comm.POSTIssueManager(data, "Vehicle_ObservationFacade/savedata");
        }
        //onchange
        public VMS_FO_Vehicle_ObservationDTO onchange(VMS_FO_Vehicle_ObservationDTO data)
        {
            return comm.POSTIssueManager(data, "Vehicle_ObservationFacade/onchange");
        }
        //savedataSource
        public VMS_FO_Vehicle_ObservationDTO savedataSource(VMS_FO_Vehicle_ObservationDTO data)
        {
            return comm.POSTIssueManager(data, "Vehicle_ObservationFacade/savedataSource");
        }
        //clientDecative
        public VMS_FO_Vehicle_ObservationDTO clientDecative(VMS_FO_Vehicle_ObservationDTO data)
        {
            return comm.POSTIssueManager(data, "Vehicle_ObservationFacade/clientDecative");
        }

        //
        public VMS_FO_Vehicle_ObservationDTO RecieptReport(VMS_FO_Vehicle_ObservationDTO data)
        {
            return comm.POSTIssueManager(data, "Vehicle_ObservationFacade/RecieptReport");
        }
    }
}
