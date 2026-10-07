using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface NocGenerationReportInterface
    {
        Task<NocGenerationReportDTO> getdetails(NocGenerationReportDTO data);
        Task<NocGenerationReportDTO> get_report(NocGenerationReportDTO data);

        NocGenerationReportDTO buildingchange(NocGenerationReportDTO data);
        NocGenerationReportDTO Floorchange(NocGenerationReportDTO data);
        NocGenerationReportDTO getallofflinedetails(NocGenerationReportDTO data);
        NocGenerationReportDTO SaveElectricalbill(NocGenerationReportDTO data);
        NocGenerationReportDTO deactive(NocGenerationReportDTO data);
        NocGenerationReportDTO editdetails(NocGenerationReportDTO data);
        NocGenerationReportDTO sendmail(NocGenerationReportDTO data);

    }
}
