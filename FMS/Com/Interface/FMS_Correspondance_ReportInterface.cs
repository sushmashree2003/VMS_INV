using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Interface
{
    public interface FMS_Correspondance_ReportInterface
    {
        FMS_Correspondance_ReportDTO onload(FMS_Correspondance_ReportDTO data);
        FMS_Correspondance_ReportDTO onchnageradiobtnsfile(FMS_Correspondance_ReportDTO data);
        FMS_Correspondance_ReportDTO getprojectist(FMS_Correspondance_ReportDTO data);
        FMS_Correspondance_ReportDTO getreport(FMS_Correspondance_ReportDTO data);
        FMS_Correspondance_ReportDTO SearchReport(FMS_Correspondance_ReportDTO data);
        FMS_Correspondance_ReportDTO viewreferencedetais(FMS_Correspondance_ReportDTO data);
        FMS_Correspondance_ReportDTO CountReport(FMS_Correspondance_ReportDTO data);
        FMS_Correspondance_ReportDTO ViewRefernceNoDetails(FMS_Correspondance_ReportDTO data);

        // File Cabinet Reports
        FMS_Correspondance_ReportDTO OnloadFileCabinetReport(FMS_Correspondance_ReportDTO data);
        FMS_Correspondance_ReportDTO FileCabinetReport(FMS_Correspondance_ReportDTO data);
    }
}
