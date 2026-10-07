using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface TenantPaymentOrFlatwiseReportInterface
    {
        Task<TenantAgreementDTO> getdetails(TenantAgreementDTO data);
        Task<TenantAgreementDTO> get_report(TenantAgreementDTO data);

        TenantAgreementDTO buildingchange(TenantAgreementDTO data);
        TenantAgreementDTO floorchange(TenantAgreementDTO data);

        TenantAgreementDTO companychange(TenantAgreementDTO data);
    }
}
