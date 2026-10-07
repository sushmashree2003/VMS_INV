using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class TenantViewNoticesDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TenantAgreementDTO, TenantAgreementDTO> COMMM = new CommonDelegate<TenantAgreementDTO, TenantAgreementDTO>();

        public TenantAgreementDTO getdetails(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantViewNoticesFacade/getdetails/");
        }
        public TenantAgreementDTO get_report(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantViewNoticesFacade/get_report");
        }
         public TenantAgreementDTO ViewFiles(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantViewNoticesFacade/ViewFiles");
        }

        public TenantAgreementDTO get_NoticeBoardreport(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantViewNoticesFacade/get_NoticeBoardreport");
        }

    }
}
