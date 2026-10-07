using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.RTO.Interfaces
{
  public  interface VMS_Vehicle_ReportInterface
    {
        VMS_Vehicle_ReportDTO loaddata(VMS_Vehicle_ReportDTO data);
        VMS_Vehicle_ReportDTO savedata(VMS_Vehicle_ReportDTO data);
        //VMS_Vehicle_ReportDTO
        VMS_Vehicle_ReportDTO onchange(VMS_Vehicle_ReportDTO data);

        //RTO_Master_FineDTO
        VMS_Vehicle_ReportDTO savedataSource(VMS_Vehicle_ReportDTO data);
        //clientDecative
        VMS_Vehicle_ReportDTO clientDecative(VMS_Vehicle_ReportDTO data);
        //RecieptReport

        VMS_Vehicle_ReportDTO RecieptReport(VMS_Vehicle_ReportDTO data);
    }
}
