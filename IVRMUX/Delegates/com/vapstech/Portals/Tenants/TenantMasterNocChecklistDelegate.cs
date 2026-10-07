using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class TenantMasterNocChecklistDelegate
    {
        CommonDelegate<TenantMasterNocChecklistDTO, TenantMasterNocChecklistDTO> _comm = new CommonDelegate<TenantMasterNocChecklistDTO, TenantMasterNocChecklistDTO>();

        //Master Type
        public TenantMasterNocChecklistDTO getloaddata(TenantMasterNocChecklistDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantMasterNocChecklistFacade/getloaddata");
        }

        public TenantMasterNocChecklistDTO getamenity(TenantMasterNocChecklistDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantMasterNocChecklistFacade/getamenity");
        }
        public TenantMasterNocChecklistDTO savedetails(TenantMasterNocChecklistDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantMasterNocChecklistFacade/savedetails");
        }

        public TenantMasterNocChecklistDTO activedeactive(TenantMasterNocChecklistDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantMasterNocChecklistFacade/activedeactive/");
        }
    }
}
