using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class TenantPaymentReprotDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TenantAgreementDTO, TenantAgreementDTO> COMMM = new CommonDelegate<TenantAgreementDTO, TenantAgreementDTO>();

        public TenantAgreementDTO getdetails(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentReprotFacade/getdetails/");
        }
        public TenantAgreementDTO get_report(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentReprotFacade/get_report");
        }


        public TenantAgreementDTO institutionchange(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentReprotFacade/institutionchange");
        }
        public TenantAgreementDTO buildingchange(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentReprotFacade/buildingchange");
        }
        public TenantAgreementDTO Floorchange(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentReprotFacade/Floorchange");
        }
        public TenantAgreementDTO TenantDetails(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentReprotFacade/TenantDetails");
        }
        public TenantAgreementDTO TenantConsolidatedReport(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentReprotFacade/TenantConsolidatedReport");
        }
        public TenantAgreementDTO TenantMonthlyPaymentDetails(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantPaymentReprotFacade/TenantMonthlyPaymentDetails");
        }
    }
}
