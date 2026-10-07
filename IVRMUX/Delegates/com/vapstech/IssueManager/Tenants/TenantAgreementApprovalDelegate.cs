using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class TenantAgreementApprovalDelegate
    {
        CommonDelegate<TenantAgreementApprovalDTO, TenantAgreementApprovalDTO> _comm = new CommonDelegate<TenantAgreementApprovalDTO, TenantAgreementApprovalDTO>();
        public TenantAgreementApprovalDTO loaddata(TenantAgreementApprovalDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementApprovalFacade/loaddata/");
        }

        public TenantAgreementApprovalDTO getdatas(TenantAgreementApprovalDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementApprovalFacade/getdatas/");
        }

        public TenantAgreementApprovalDTO savedetails(TenantAgreementApprovalDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementApprovalFacade/savedetails/");
        }
    }
}