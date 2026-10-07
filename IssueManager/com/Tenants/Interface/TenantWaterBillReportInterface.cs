using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface TenantWaterBillReportInterface
    {
        Task<TenantWaterBillReportDTO> getdetails(TenantWaterBillReportDTO data);
        Task<TenantWaterBillReportDTO> get_report(TenantWaterBillReportDTO data);
        TenantWaterBillReportDTO buildingchange(TenantWaterBillReportDTO data);
        TenantWaterBillReportDTO Floorchange(TenantWaterBillReportDTO data);

        TenantWaterBillReportDTO sendmail(TenantWaterBillReportDTO data);
    }
}
