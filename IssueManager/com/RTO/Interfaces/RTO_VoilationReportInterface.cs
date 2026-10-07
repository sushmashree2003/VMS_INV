using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.RTO.Interfaces
{
    public interface RTO_VoilationReportInterface
    {
        RTO_VoilationReportDTO loaddata(RTO_VoilationReportDTO data);
        RTO_VoilationReportDTO RecieptReport1(RTO_VoilationReportDTO data);
        //VMS_Vehicle_ReportDTO
        RTO_VoilationReportDTO onchange(RTO_VoilationReportDTO data);


    }
}