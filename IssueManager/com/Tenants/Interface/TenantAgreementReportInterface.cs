using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface TenantAgreementReportInterface
    {
        Task<TenantAgreementDTO> getdetails(TenantAgreementDTO data);
        Task<TenantAgreementDTO> get_report(TenantAgreementDTO data);
       
        TenantAgreementDTO buildingchange(TenantAgreementDTO data);
        TenantAgreementDTO Floorchange(TenantAgreementDTO data);
        TenantAgreementDTO getallofflinedetails(TenantAgreementDTO data);
        TenantAgreementDTO SaveElectricalbill(TenantAgreementDTO data);
        TenantAgreementDTO deactive(TenantAgreementDTO data);
        TenantAgreementDTO editdetails(TenantAgreementDTO data);
        TenantAgreementDTO sendmail(TenantAgreementDTO data);
        TenantAgreementDTO Saveallbilltype(TenantAgreementDTO data);
        TenantAgreementDTO loadtenants(TenantAgreementDTO data);
        TenantAgreementDTO getTenantAmountDetails(TenantAgreementDTO data);
        TenantAgreementDTO Savefine(TenantAgreementDTO data);
    }
}
