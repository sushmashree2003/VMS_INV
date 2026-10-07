using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface TenantViewNoticesInterface
    {
        Task<TenantAgreementDTO> getdetails(TenantAgreementDTO data);
        Task<TenantAgreementDTO> get_report(TenantAgreementDTO data);
        Task<TenantAgreementDTO> ViewFiles(TenantAgreementDTO data);
        Task<TenantAgreementDTO> get_NoticeBoardreport(TenantAgreementDTO data);

    }
}
