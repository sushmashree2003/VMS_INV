using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class flathistoryreportDelegate
    {
        CommonDelegate<TenantAgreementDTO, TenantAgreementDTO> _comm = new CommonDelegate<TenantAgreementDTO, TenantAgreementDTO>();

        //Master Type
        public TenantAgreementDTO OnLoadRatingMasterType(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "flathistoryreportFacade/OnLoadRatingMasterType");
        }
        public TenantAgreementDTO get_report(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "flathistoryreportFacade/get_report");
        }
         public TenantAgreementDTO roomchange(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "flathistoryreportFacade/roomchange");
        }
        public TenantAgreementDTO buildingchange(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "flathistoryreportFacade/buildingchange");
        }

        //added by anvesha
        public TenantAgreementDTO ViewHealthCheckUpFiles(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "flathistoryreportFacade/ViewHealthCheckUpFiles");
        }
    }
}
