using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface flathistoryreportInterface
    {
        //Master Type
        TenantAgreementDTO OnLoadRatingMasterType(TenantAgreementDTO data);
        TenantAgreementDTO get_report(TenantAgreementDTO data);
        TenantAgreementDTO roomchange(TenantAgreementDTO data);
        TenantAgreementDTO buildingchange(TenantAgreementDTO data);

        //added by anvesha
        TenantAgreementDTO ViewHealthCheckUpFiles(TenantAgreementDTO data);

    }
}
