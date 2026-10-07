using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface TenantOfflinePaymentReportInterface
    {
        TenantOfflinePaymentReportDTO onloaddata(TenantOfflinePaymentReportDTO data);
        TenantOfflinePaymentReportDTO report(TenantOfflinePaymentReportDTO data);
        TenantOfflinePaymentReportDTO viewuploadflies(TenantOfflinePaymentReportDTO data);
        TenantOfflinePaymentReportDTO getpaymentreport(TenantOfflinePaymentReportDTO data);
    }
}
