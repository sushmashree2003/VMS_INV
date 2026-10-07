using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.RTO
{
    public class RTO_Master_FineDelegate
    {
        //loaddata
        CommonDelegate<RTO_Master_FineDTO, RTO_Master_FineDTO> comm = new CommonDelegate<RTO_Master_FineDTO, RTO_Master_FineDTO>();
        CommonDelegate<VMS_Vehicle_KeyDTO, VMS_Vehicle_KeyDTO> commT = new CommonDelegate<VMS_Vehicle_KeyDTO, VMS_Vehicle_KeyDTO>();
       
        public RTO_Master_FineDTO loaddata(RTO_Master_FineDTO data)
        {
            return comm.POSTIssueManager(data, "RTO_Master_FineFacade/loaddata");
        }
        public VMS_Vehicle_KeyDTO savedata(VMS_Vehicle_KeyDTO data)
        {
            return commT.POSTIssueManager(data, "RTO_Master_FineFacade/savedata");
        }
        //onchange
        public VMS_Vehicle_KeyDTO onchange(VMS_Vehicle_KeyDTO data)
        {
            return commT.POSTIssueManager(data, "RTO_Master_FineFacade/onchange");
        }
        //savedataSource
        public RTO_Master_FineDTO savedataSource(RTO_Master_FineDTO data)
        {
            return comm.POSTIssueManager(data, "RTO_Master_FineFacade/savedataSource");
        }
        //clientDecative
        public RTO_Master_FineDTO clientDecative(RTO_Master_FineDTO data)
        {
            return comm.POSTIssueManager(data, "RTO_Master_FineFacade/clientDecative");
        }

        //
        public RTO_Master_FineDTO RecieptReport(RTO_Master_FineDTO data)
        {
            return comm.POSTIssueManager(data, "RTO_Master_FineFacade/RecieptReport");
        }
        //VehicalUpdate
        public VMS_Vehicle_KeyDTO VehicalUpdate(VMS_Vehicle_KeyDTO data)
        {
            return commT.POSTIssueManager(data, "RTO_Master_FineFacade/VehicalUpdate");
        }
    }
}
