using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class TenantOfflinePaymentReportDelegate
    {
        CommonDelegate<TenantOfflinePaymentReportDTO, TenantOfflinePaymentReportDTO> _comm = new CommonDelegate<TenantOfflinePaymentReportDTO, TenantOfflinePaymentReportDTO>();


        

        public TenantOfflinePaymentReportDTO onloaddata(TenantOfflinePaymentReportDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantOfflinePaymentReportFacade/onloaddata/");
        }
        public TenantOfflinePaymentReportDTO report(TenantOfflinePaymentReportDTO obj)
        {
            return _comm.POSTIssueManager(obj, "TenantOfflinePaymentReportFacade/report/");
        }
        public TenantOfflinePaymentReportDTO viewuploadflies(TenantOfflinePaymentReportDTO obj)
        {
            return _comm.POSTIssueManager(obj, "TenantOfflinePaymentReportFacade/viewuploadflies/");
        }
        public TenantOfflinePaymentReportDTO getpaymentreport(TenantOfflinePaymentReportDTO obj)
        {
            return _comm.POSTIssueManager(obj, "TenantOfflinePaymentReportFacade/getpaymentreport/");
        }
    }
}
