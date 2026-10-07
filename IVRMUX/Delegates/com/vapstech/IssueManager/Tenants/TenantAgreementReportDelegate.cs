using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class TenantAgreementReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TenantAgreementDTO, TenantAgreementDTO> COMMM = new CommonDelegate<TenantAgreementDTO, TenantAgreementDTO>();

        public TenantAgreementDTO getdetails(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/getdetails/");
        }
        public TenantAgreementDTO get_report(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/get_report");
        }

       
        public TenantAgreementDTO buildingchange(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/buildingchange");
        }
        public TenantAgreementDTO Floorchange(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/Floorchange");
        }

        //ELECTRICAL
        public TenantAgreementDTO getallofflinedetails(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/getallofflinedetails/");
        }

        public TenantAgreementDTO SaveElectricalbill(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/SaveElectricalbill/");
        }


        public TenantAgreementDTO editdetails(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/editdetails/");
        }
        public TenantAgreementDTO deactive(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/deactive/");
        }
        public TenantAgreementDTO sendmail(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/sendmail/");
        }
        public TenantAgreementDTO Saveallbilltype(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/Saveallbilltype/");
        }
        // Fine Caluclation
        public TenantAgreementDTO loadtenants(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/loadtenants/");
        }
        public TenantAgreementDTO getTenantAmountDetails(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/getTenantAmountDetails/");
        }
        public TenantAgreementDTO Savefine(TenantAgreementDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantAgreementReportFacade/Savefine/");
        }
    }
}
