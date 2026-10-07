using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface TenantPaymentReprotInterface
    {
        Task<TenantAgreementDTO> getdetails(TenantAgreementDTO data);
        Task<TenantAgreementDTO> get_report(TenantAgreementDTO data);

        TenantAgreementDTO institutionchange(TenantAgreementDTO data);
        TenantAgreementDTO buildingchange(TenantAgreementDTO data);
        TenantAgreementDTO Floorchange(TenantAgreementDTO data);
        TenantAgreementDTO TenantDetails(TenantAgreementDTO data);
        TenantAgreementDTO TenantConsolidatedReport(TenantAgreementDTO data);
        TenantAgreementDTO TenantMonthlyPaymentDetails(TenantAgreementDTO data);
    }
}
