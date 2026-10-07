using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{

    public class TenantPaidandPendinglistDelegate
    {
        CommonDelegate<TenantPaidandPendinglistDTO, TenantPaidandPendinglistDTO> _comm = new CommonDelegate<TenantPaidandPendinglistDTO, TenantPaidandPendinglistDTO>();

        public TenantPaidandPendinglistDTO onloadgetdetails(TenantPaidandPendinglistDTO dto)
        {
            return _comm.POSTIssueManager(dto, "TenantPaidandPendinglistFacade/onloadgetdetails");
        }

        public TenantPaidandPendinglistDTO get_report(TenantPaidandPendinglistDTO dto)
        {
            return _comm.POSTIssueManager(dto, "TenantPaidandPendinglistFacade/get_report");
        }
        public TenantPaidandPendinglistDTO sendmail(TenantPaidandPendinglistDTO dto)
        {
            return _comm.POSTIssueManager(dto, "TenantPaidandPendinglistFacade/sendmail");
        }
    }
}
