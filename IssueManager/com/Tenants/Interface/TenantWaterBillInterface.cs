using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface TenantWaterBillInterface
    {
        TenantWaterBillDTO loaddata(TenantWaterBillDTO dto);
 
        TenantWaterBillDTO savetrainerfeedback(TenantWaterBillDTO dto);
        TenantWaterBillDTO deactive(TenantWaterBillDTO dto);
        TenantWaterBillDTO viewroomwisewaterbill(TenantWaterBillDTO dto);
    }
}
