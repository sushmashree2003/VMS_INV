using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
   public interface TenantPaidandPendinglistInterface
    {
        TenantPaidandPendinglistDTO getBasicData(TenantPaidandPendinglistDTO dto);
        TenantPaidandPendinglistDTO get_report(TenantPaidandPendinglistDTO dto);
        TenantPaidandPendinglistDTO sendmail(TenantPaidandPendinglistDTO dto);

    }
}
