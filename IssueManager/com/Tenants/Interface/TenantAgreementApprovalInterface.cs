using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface TenantAgreementApprovalInterface
    {
        TenantAgreementApprovalDTO loaddata(TenantAgreementApprovalDTO data);
        TenantAgreementApprovalDTO getdatas(TenantAgreementApprovalDTO data);
        TenantAgreementApprovalDTO savedetails(TenantAgreementApprovalDTO data);
    }
}
