using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.RTO.Interfaces
{
   public interface RTO_Master_FineInterface
    {
        RTO_Master_FineDTO loaddata(RTO_Master_FineDTO data);
        VMS_Vehicle_KeyDTO savedata(VMS_Vehicle_KeyDTO data);
        //onchange
        VMS_Vehicle_KeyDTO onchange(VMS_Vehicle_KeyDTO data);

        //RTO_Master_FineDTO
        RTO_Master_FineDTO savedataSource(RTO_Master_FineDTO data);
        //clientDecative
        RTO_Master_FineDTO clientDecative(RTO_Master_FineDTO data);
        //RecieptReport
        RTO_Master_FineDTO RecieptReport(RTO_Master_FineDTO data);

        //VehicalUpdate
        VMS_Vehicle_KeyDTO VehicalUpdate(VMS_Vehicle_KeyDTO data);
    }
}
