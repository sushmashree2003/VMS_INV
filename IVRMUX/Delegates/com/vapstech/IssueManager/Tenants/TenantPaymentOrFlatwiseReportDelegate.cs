using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class TenantPaymentOrFlatwiseReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TenantAgreementDTO, TenantAgreementDTO> COMMM = new CommonDelegate<TenantAgreementDTO, TenantAgreementDTO>();

        public TenantAgreementDTO getdetails(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentOrFlatwiseReportFacade/getdetails/");
        }
        public TenantAgreementDTO get_report(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentOrFlatwiseReportFacade/get_report");
        }


        public TenantAgreementDTO buildingchange(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentOrFlatwiseReportFacade/buildingchange");
        }
        public TenantAgreementDTO floorchange(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentOrFlatwiseReportFacade/floorchange");
        }
        public TenantAgreementDTO companychange(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentOrFlatwiseReportFacade/companychange");
        }
    }
}
