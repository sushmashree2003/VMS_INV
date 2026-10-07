using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.RTO.Interfaces
{
  public  interface FO_Vehicle_Ob_reportInterface
    {
        FO_Vehicle_Ob_reportDTO loaddata(FO_Vehicle_Ob_reportDTO data);
        FO_Vehicle_Ob_reportDTO RecieptReport1(FO_Vehicle_Ob_reportDTO data);
        //VMS_Vehicle_ReportDTO
        FO_Vehicle_Ob_reportDTO onchange(FO_Vehicle_Ob_reportDTO data);
       
    }
}
